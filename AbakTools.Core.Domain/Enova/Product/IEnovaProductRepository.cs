﻿using EnovaApi.Models.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductPriceInfo = AbakTools.Core.Domain.Common.ProductPriceInfo;

namespace AbakTools.Core.Domain.Enova.Product
{
    public interface IEnovaProductRepository : IGenericEntityRepository<EnovaProduct>
    {
        ProductPriceInfo GetPriceForCustomer(Guid productGuid, Guid customerGuid);
        Task<IEnumerable<Price>> GetModifiedPricesAsync(Guid definitionGuid, long stampFrom, long stampTo);
    }
}
