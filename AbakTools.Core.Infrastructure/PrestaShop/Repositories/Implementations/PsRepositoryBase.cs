﻿using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    internal abstract class PsRepositoryBase<TEntry> : IPsRepositoryBase<TEntry>
        where TEntry : PrestaShopEntity, IPrestaShopFactoryEntity, new()
    {
        protected IPrestaShopClient PrestaShopClient { get; private set; }

        protected abstract GenericFactory<TEntry> Factory { get; }

        public PsRepositoryBase(IPrestaShopClient prestaShopClient)
        {
            PrestaShopClient = prestaShopClient;
        }

        public TEntry Get(long id)
        {
            try
            {
                return Factory.Get(id);
            }
            catch (Bukimedia.PrestaSharp.PrestaSharpException ex)
            {
                if (ex.ResponseHttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public IReadOnlyCollection<TEntry> GetByFilter(object filterObj)
        {
            var filter = CreateFilterFromObject(filterObj);

            return Factory.GetByFilter(filter, null, null);
        }

        public IReadOnlyCollection<long> GetAllModifiedBetween(DateTime from, DateTime to)
        {
            var filter = new Dictionary<string, string>
            {
                { "date_upd", $"[{from:yyyy-MM-dd HH:mm:ss},{to:yyyy-MM-dd HH:mm:ss}]" }
            };

            return Factory.GetIdsByFilter(filter, null, null);
        }

        private Dictionary<string, string> CreateFilterFromObject(object obj)
        {
            if (obj != null)
            {
                var filter = new Dictionary<string, string>();
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

                foreach (var property in properties)
                {
                    filter.Add(property.Name, property.GetValue(obj)?.ToString());
                }

                return filter;
            }

            return null;
        }

        public TEntry SaveOrUpdate(TEntry entry)
        {
            if (entry.id > 0)
            {
                Factory.Update(entry);
            }
            else
            {
                entry = Factory.Add(entry);
            }

            return entry;
        }

        public void Delete(TEntry entry)
        {
            Factory.Delete(entry);
        }

        public void SetLangValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression, string value)
            where TEntity : PrestaShopEntity
            where TProperty : List<Bukimedia.PrestaSharp.Entities.AuxEntities.language>
        {
            PrestaShopClient.SetLangValue(entity, expression, value);
        }
    }
}