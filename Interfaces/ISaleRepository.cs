using PhramacyApp.Areas.Transaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces
{
    public interface ISaleRepository : IGenericRepository<SaleMasterModel>
    {
        Task<IEnumerable<SaleMasterModel>> GetAllMedicine();
        Task<SaleModel> GetMedicineId(string medicineId, string manufacturer_id);
        Task<IEnumerable<SaleModel>> GetMedicineInfo(string medicine);
        Task<SaleMasterModel> GetByMasterIdAsync(int id);
        Task<SaleMasterModel> UpdateSaleAsync(int id, SaleMasterModel entity);
        Task<List<SaleDetailModel>> GetDetailListByMasterId(int sale_Master_Id);
        Task<int> DeleteChildAsync(int detailId, int masterId);
        Task<int> DeleteMasterAsync(int masterId);
        Task<IEnumerable<SaleModel>> GetSaleMasterData(int id);
        Task<List<SaleDetailModel>> GetStockList(int id);
        Task<IEnumerable<SaleModel>> GetSaleList(int Sale_Master_Id);
    }
}
