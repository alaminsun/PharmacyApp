
using Microsoft.Extensions.DependencyInjection;
using PharmacyApp.Repository;
using PharmacyApp.Services;
using PhramacyApp.DAL.Gateway;
using PhramacyApp.Helpers;
using PhramacyApp.Interfaces;
using PhramacyApp.Interfaces.Repositories;
using PhramacyApp.Interfaces.Shared;
using PhramacyApp.Repository;
using System.Data;

namespace PhramacyApp.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IGenericRepository, GenericRepository>();
            //services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));
            services.AddTransient<IDataAccessService, DataAccessService>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<ISubMenuRepository, SubMenuRepository>();
            services.AddTransient<IMenuConfRepository, MenuConfRepository>();
            services.AddTransient<IShelfRepository, ShelfRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IMedicineRepository, MedicineRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<ISaleRepository, SaleRepository>();
            //services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.AddScoped<IDGenerated>();
            services.AddScoped<DBHelper>();
            services.AddScoped<IProductService, ProductService>();
            //services.AddTransient<>();
            //services.AddTransient<IDataRecord, DataRecord>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
        }
    }
}
