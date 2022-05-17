
using PhramacyApp.Interfaces;
using PhramacyApp.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhramacyApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork( IMenuRepository menuRepository, 
            ISubMenuRepository subMenuRepository, 
            IMenuConfRepository menuConfRepository,
            IShelfRepository shelfRepository,
            ICategoryRepository categoryRepository,
            IMedicineRepository medicineRepository,
            ICustomerRepository  customerRepository,
            ISupplierRepository  supplierRepository,
            IPurchaseRepository purchaseRepository,
            ISaleRepository saleRepository)
        {
            Menus = menuRepository;
            SubMenus = subMenuRepository;
            MenuConfigs = menuConfRepository;
            Shelfs = shelfRepository;
            Categories = categoryRepository;
            Medicines = medicineRepository;
            Customers = customerRepository;
            Suppliers = supplierRepository;
            Purchases = purchaseRepository;
            Sales = saleRepository;
        }
        public IMenuRepository Menus { get; }
        public ISubMenuRepository SubMenus { get; }
        public IMenuConfRepository MenuConfigs { get; }
        public IShelfRepository Shelfs { get; }
        public ICategoryRepository Categories { get; }
        public IMedicineRepository Medicines { get; }
        public ICustomerRepository Customers { get; }
        public ISupplierRepository Suppliers { get; }
        public IPurchaseRepository Purchases { get; }
        public ISaleRepository Sales { get; }
    }
}
