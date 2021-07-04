using Bukimedia.PrestaSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    public interface IPsRepositoryBase<TEntry>
        where TEntry : PrestaShopEntity, IPrestaShopFactoryEntity, new()
    {
        TEntry Get(long id);
        IReadOnlyCollection<TEntry> GetByFilter(object filterObj);
        IReadOnlyCollection<long> GetAllModifiedBetween(DateTime from, DateTime to);
        TEntry SaveOrUpdate(TEntry entry);
        void Delete(TEntry entry);

        void SetLangValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression, string value)
            where TEntity : PrestaShopEntity
            where TProperty : List<Bukimedia.PrestaSharp.Entities.AuxEntities.language>;
    }
}
