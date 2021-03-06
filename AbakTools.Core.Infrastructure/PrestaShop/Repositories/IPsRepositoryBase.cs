﻿using Bukimedia.PrestaSharp.Entities;
using System;
using System.Collections.Generic;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    public interface IPsRepositoryBase<TEntry>
        where TEntry : PrestaShopEntity, IPrestaShopFactoryEntity, new()
    {
        TEntry Get(long id);
        IReadOnlyCollection<TEntry> GetByFilter(object filterObj);
        IReadOnlyCollection<long> GetAllModifiedBetween(DateTime from, DateTime to);
    }
}
