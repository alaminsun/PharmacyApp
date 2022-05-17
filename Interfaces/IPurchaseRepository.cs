using Microsoft.AspNetCore.Mvc;
using PhramacyApp.Areas.Transaction.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces
{
    public interface IPurchaseRepository : IGenericRepository<PurchaseMasterModel>
    {
        Task<IEnumerable<PurchaseMasterModel>> GetAllMedicine();
        //Task<PurchaseModel> GetMedicineInfo(string medicine);
        Task<PurchaseModel> GetMedicineId(string medicineId, string manufacturer_id);
        Task<IEnumerable<PurchaseModel>> GetMedicineInfo(string medicine, string manufacturer_id);
        //Task<IEnumerable<PurchaseMasterModel>> GetByMasterIdAsync(int id, PurchaseMasterModel model);
        Task<PurchaseMasterModel> GetByMasterIdAsync(int id);
        Task<PurchaseMasterModel> UpdatePurchaseAsync(int id, PurchaseMasterModel entity);
        Task<List<PurchaseDetailModel>> GetDetailListByMasterId(int purchase_Master_Id);
        Task<int> DeleteChildAsync(int detailId, int masterId);
        Task<int> DeleteMasterAsync(int masterId);
        Task<IEnumerable<PurchaseModel>> GetPurchaseList();
        Task<IEnumerable<PurchaseModel>> GetPurchaseMasterData(int id);

    }
}
