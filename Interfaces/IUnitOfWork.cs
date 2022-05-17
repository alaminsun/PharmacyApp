using PhramacyApp.Interfaces.Repositories;
using PhramacyApp.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhramacyApp.Interfaces
{
    public interface IUnitOfWork
    {
        IMenuRepository Menus { get; }
        ISubMenuRepository SubMenus { get; }
        IMenuConfRepository MenuConfigs { get; }
        IShelfRepository Shelfs { get; }
        ICategoryRepository Categories { get; }
        IMedicineRepository Medicines { get; }
        ICustomerRepository Customers { get; }
        ISupplierRepository Suppliers { get; }
        IPurchaseRepository Purchases { get; }
        ISaleRepository Sales { get; }
    }
}
