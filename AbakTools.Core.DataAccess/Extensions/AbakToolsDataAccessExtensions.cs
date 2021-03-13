using AbakTools.Core.DataAccess;
using AbakTools.Core.DataAccess.Repository;
using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Order.Repositories;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework.UnitOfWork;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbakToolsDataAccessExtensions
    {
        public static void AddDataAccsessComponent(this IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWorkProvider, UnitOfWorkProvider>();
            services.AddTransient<ISynchronizeStampRepository, SynchronizeStampRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ITaxRepository, TaxRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IDiscountGroupRepository, DiscountGroupRepository>();
        }
    }
}
