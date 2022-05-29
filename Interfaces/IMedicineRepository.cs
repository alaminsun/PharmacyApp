using AspNetCoreDapperPagingSortingFilteringDemo.Models;
using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces
{
    public interface IMedicineRepository : IGenericRepository<MedicineModel>
    {
        Task<Medicine> GetMedicine(int Currentpage);
        int GetSearchValueCount(string searchValue);
        List<MedicineModel> GetSearchValue(string searchValue, string sortColumn, string sortColumnDirection);
        MedicineModel GetSupplierNicName(string suplier_Id);
        Task<List<MedicineModel>> GetProductsAsync(ProductListRequest request);
        //List<MedicineModel> medicineDataForSorting(string sortColumn, string sortColumnDirection);
        List<MedicineModel> GetAllMedicine();
        List<MedicineModel> medicineDataForSorting(string sortColumn, string sortColumnDirection);
    }
}

