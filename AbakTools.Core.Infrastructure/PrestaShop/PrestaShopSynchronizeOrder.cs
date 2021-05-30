using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Order;
using AbakTools.Core.Domain.Order.Repositories;
using AbakTools.Core.Domain.Policies;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.Helpers;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PsOrder = Bukimedia.PrestaSharp.Entities.order;
using PsOrderRow = Bukimedia.PrestaSharp.Entities.order_row;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    internal partial class PrestaShopSynchronizeOrder : IPrestaShopSynchronizeOrder
    {
        private readonly ILogger logger;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ISynchronizeStampRepository synchronizeStampRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderStatusRepository orderStatusRepository;
        private readonly IProductRepository productRepository;
        private readonly IPrestaShopClient prestaShopClient;
        private readonly IProductPricePolicy pricePolicy;

        private readonly IPSOrderRepository psOrderRepository;
        private readonly IPSMessageRepository psMessageRepository;

        public PrestaShopSynchronizeOrder(
            ILogger<PrestaShopSynchronizeOrder> _logger,
            IUnitOfWorkProvider _unitOfWorkProvider,
            ISynchronizeStampRepository _synchronizeStampRepository,
            ICustomerRepository _customerRepository,
            IOrderRepository _orderRepository,
            IOrderStatusRepository _orderStatusRepository,
            IProductRepository _productRepository,
            IPrestaShopClient _prestaShopClient,
            IPSOrderRepository _psOrderRepository,
            IPSMessageRepository _psMessageRepository,
            IProductPricePolicy _pricePolicy)
        {
            logger = _logger;
            unitOfWorkProvider = _unitOfWorkProvider;
            synchronizeStampRepository = _synchronizeStampRepository;
            customerRepository = _customerRepository;
            orderRepository = _orderRepository;
            orderStatusRepository = _orderStatusRepository;
            productRepository = _productRepository;
            prestaShopClient = _prestaShopClient;
            psOrderRepository = _psOrderRepository;
            pricePolicy = _pricePolicy;
            psMessageRepository = _psMessageRepository;
        }


        public async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                SynchronizeStampEntity synchronizeStamp = null;
                DateTime stampTo = DateTime.Now;

                using (var uow = unitOfWorkProvider.CreateReadOnly())
                {
                    synchronizeStamp = synchronizeStampRepository.Get(SynchronizeCodes.Order, SynchronizeDirectionType.Import);
                }

                DateTime stampFrom = synchronizeStamp?.DateTimeStamp ?? DateTime.MinValue;

                IReadOnlyCollection<long> psOrderIds = psOrderRepository.GetAllModifiedBetween(stampFrom, stampTo);

                if (Enumerable.Any(psOrderIds))
                {
                    logger.LogDebug($"Starting synchronize {psOrderIds.Count} orders");

                    var parallelOptions = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 1
                    };

                    Parallel.ForEach(psOrderIds, parallelOptions, x => ProcessOrderId(x, stampTo));

                    if (synchronizeStamp == null)
                    {
                        synchronizeStamp = SynchronizeStampFactory.Create(SynchronizeCodes.Order, SynchronizeDirectionType.Import);
                    }

                    synchronizeStamp.DateTimeStamp = stampTo;

                    using (var uow = unitOfWorkProvider.Create())
                    {
                        synchronizeStampRepository.SaveOrUpdate(synchronizeStamp);
                        uow.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Synchronize orders from PrestaShop error: {ex}");
            }
        }

        private void ProcessOrderId(long id, DateTime stampTo)
        {
            var psOrder = psOrderRepository.Get(id);

            if (psOrder != null && DateTimeHelper.ParseInvariant(psOrder.date_upd) <= stampTo)
            {
                using (var uow = unitOfWorkProvider.Create())
                {
                    var order = orderRepository.GetByWebId((int)psOrder.id);

                    if (order == null)
                    {
                        order = InsertOrder(psOrder);
                    }
                    else
                    {
                        UpdateOrder(order, psOrder);
                    }

                    order.WebId = (int)psOrder.id;

                    orderRepository.SaveOrUpdate(order);
                    uow.Commit();
                }
            }
        }

        private void UpdateOrder(OrderEntity order, Bukimedia.PrestaSharp.Entities.order psOrder)
        {
            //throw new NotImplementedException();
            SynchronizeOrderMessages(order, psOrder);
        }

        private OrderEntity InsertOrder(PsOrder psOrder)
        {
            var customer = customerRepository.GetByWebId((int)psOrder.id_customer);
            PrestaShopSynchronizeException.TrowIfNull(customer, $"Customer not found (WebId: {psOrder.id_customer})");

            var order = OrderEntity.Create(customer, OrderSourceType.WWW);

            order.InvoiceAddress = customer.GetMainAddress();
            order.DeliveryAddress = customer.GetDefaultDeliveryAddress();
            SetOrderStateToDefault(order);
            SynchronizeOrderDetails(order, psOrder);
            SynchronizeOrderMessages(order, psOrder);

            return order;
        }

        private void SetOrderStateToDefault(OrderEntity order)
        {
            var state = orderStatusRepository.GetDefault();

            if (state != null)
            {
                order.ChangeState(state);
            }
        }

        private void SynchronizeOrderDetails(OrderEntity order, PsOrder psOrder)
        {
            foreach (var psRow in psOrder.associations.order_rows)
            {
                SynchronizePsOrderRow(order, psRow);
            }

            foreach (var row in order.GetRows().Where(x => x.Id > 0 && x.WebId > 0).ToList())
            {
                SynchronizeOrderRow(order, psOrder, row);
            }
        }

        private void SynchronizePsOrderRow(OrderEntity order, PsOrderRow psRow)
        {
            var row = order.GetRowByWebId((int)psRow.id);

            if (row == null)
            {
                row = AddNewRow(order, psRow);
                row.WebId = (int)psRow.id;
            }
            else
            {
                UpdateRow(row, psRow);
            }
        }

        private void SynchronizeOrderRow(OrderEntity order, PsOrder psOrder, OrderRowEntity row)
        {
            if (psOrder.associations.order_rows.All(x => x.id != row.Id))
            {
                order.RemoveRow(row);
            }
        }

        private OrderRowEntity AddNewRow(OrderEntity order, PsOrderRow psRow)
        {
            return AddNewRow(order, (int)psRow.product_id, psRow.product_quantity);
        }

        private OrderRowEntity AddNewRow(OrderEntity order, int productId, int quantity)
        {
            var product = productRepository.GetByWebId(productId);

            if (product == null)
            {
                var psProduct = prestaShopClient.ProductFactory.Get(productId);

                if (psProduct.associations.product_bundle.Count == 1)
                {
                    var pack = psProduct.associations.product_bundle.First();
                    return AddNewRow(order, (int)pack.id, quantity * pack.quantity);
                }
            }

            PrestaShopSynchronizeException.TrowIfNull(product, $"Product not found (WebId:{productId})");

            return order.AddRow(product, quantity, pricePolicy);
        }

        private void UpdateRow(OrderRowEntity row, PsOrderRow psRow)
        {
            row.ChangeQuantity(psRow.product_quantity);
        }

        private void SynchronizeOrderMessages(OrderEntity order, PsOrder psOrder)
        {
            var messages = psMessageRepository.GetByFilter(new { id_order = psOrder.id.ToString() });

            if (messages.Any())
            {
                foreach(var msg in messages)
                {
                    order.AddOrModifyMessage(msg.Message, (int)msg.id);
                }
            }
        }
    }
}
