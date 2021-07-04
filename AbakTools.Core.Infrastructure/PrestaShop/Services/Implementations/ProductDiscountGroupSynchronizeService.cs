using AbakTools.Core.Domain.Product;
using PsProductDiscountGroup = Bukimedia.PrestaSharp.Entities.product_discount_group;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services.Implementations
{
    class ProductDiscountGroupSynchronizeService : IProductDiscountGroupSynchronizeService
    {
        private readonly ILogger _logger;
        private readonly IPsProductDiscountGroupRepository _psProductDiscountGroupRepository;

        public ProductDiscountGroupSynchronizeService(
            ILogger<ProductDiscountGroupSynchronizeService> logger,
            IPsProductDiscountGroupRepository psProductDiscountGroupRepository)
            => (_logger, _psProductDiscountGroupRepository)
            = (logger, psProductDiscountGroupRepository);

        public void Synchronize(ProductEntity product, Bukimedia.PrestaSharp.Entities.product psProduct)
        {
            var productDiscountGroups = product.ProductDiscountGroups.ToList();

            foreach (var productDiscountGroup in productDiscountGroups)
            {
                Synchronize((int)psProduct.id, productDiscountGroup);
            }

            var psProductDiscountGroups = _psProductDiscountGroupRepository.GetByFilter(new { id_product = psProduct.id });

            foreach (var psProductDiscoutGroup in psProductDiscountGroups)
            {
                if (productDiscountGroups.All(x => x.DiscountGroup.WebId != psProductDiscoutGroup.id_discount_group))
                {
                    DeleteProductDiscountGroup(null, psProductDiscoutGroup);
                }
            }
        }

        public PsProductDiscountGroup Synchronize(int psProductId, ProductDiscountGroupEntity productDiscountGroup)
        {
            var psProductGroups = _psProductDiscountGroupRepository.Get(psProductId, productDiscountGroup.DiscountGroup.WebId.Value);

            if (productDiscountGroup.IsArchived)
            {
                DeleteProductDiscountGroup(productDiscountGroup, psProductGroups);
            }
            else
            {
                if (psProductGroups == null)
                {
                    psProductGroups = InsertProductDiscountGroup(psProductId, productDiscountGroup);
                }
                else
                {
                    UpdateProductDiscountGroup(productDiscountGroup, psProductGroups);
                }
            }

            return psProductGroups;
        }

        private PsProductDiscountGroup InsertProductDiscountGroup(int productWebId, ProductDiscountGroupEntity group)
        {
            _logger.LogDebug($"Insert discount group {group.DiscountGroup.Name} for product web id {productWebId}.");

            var psDiscountGroup = new PsProductDiscountGroup
            {
                id_product = productWebId,
                id_discount_group = group.DiscountGroup.WebId.Value
            };

            return _psProductDiscountGroupRepository.SaveOrUpdate(psDiscountGroup);

        }

        private void UpdateProductDiscountGroup(ProductDiscountGroupEntity group, PsProductDiscountGroup psProductGroups)
        {
            // TODO: product and group can't be changed
        }

        private void DeleteProductDiscountGroup(ProductDiscountGroupEntity group, PsProductDiscountGroup psProductGroups)
        {
            if (psProductGroups != null)
            {
                _psProductDiscountGroupRepository.Delete(psProductGroups);
            }
        }
    }
}
