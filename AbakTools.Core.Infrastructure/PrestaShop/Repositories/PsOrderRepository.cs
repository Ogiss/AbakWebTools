using Bukimedia.PrestaSharp.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using PsOrder = Bukimedia.PrestaSharp.Entities.order;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    internal class PsOrderRepository : PsRepositoryBase<PsOrder>, IPsOrderRepository
    {
        protected override GenericFactory<PsOrder> Factory => PrestaShopClient.OrderFactory;

        public PsOrderRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }
    }
}
