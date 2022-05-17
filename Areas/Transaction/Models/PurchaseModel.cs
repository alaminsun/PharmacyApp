using Microsoft.AspNetCore.Mvc.Rendering;
using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Transaction.Models
{
    public class PurchaseModel
    {
        public int Purchase_Master_Id { get; set; }
        public int Id { get; set; }
        //public int MasterId { get; set; }
        public string Supplier_Id { get; set; }
        public string Supplier_Name { get; set; }
        public SelectList Suppliers { get; set; }
        public string Purchase_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Details { get; set; }
        public string Payment_Type { get; set; }
        public string Status { get; set; }
        public string Medicine_Id { get; set; }
        public string Medicine_Name { get; set; }
        public string Batch_No { get; set; }
        public int Stock_Qty { get; set; }
        public decimal Buying_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public string Expiry_Date { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Price { get; set; }
        public decimal Grand_Total { get; set; }
        public decimal Discount_Price { get; set; }
        public DateTime CreatedOn { get; internal set; }
    }

    public class BigViewModel
    {
        public PurchaseModel PurchaseModel { get; set; }
        public IEnumerable<MedicineModel> MedicineModel { get; set; }
    }

    public class PurchaseMasterModel
    {
        public int Sl { get; set; }
        public int Purchase_Master_Id { get; set; }
        //public int PurchaseId { get; set; }
        public string Supplier_Id { get; set; }
        public string Supplier_Name { get; set; }
        public SelectList Suppliers { get; set; }
        public string Purchase_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Details { get; set; }
        public string Payment_Type { get; set; }
        public decimal Discount_Price { get; set; }
        public decimal Grand_Total { get; set; }
        public string CreatedOn { get; set; }
        public string LastModifiedOn { get; set; }
        
        public List<PurchaseDetailModel> DetailList { get; set; }
        public string Status { get; set; }
    }

    public class PurchaseDetailModel
    {
        public int Purchase_Detail_Id { get; set; }
        public int Purchase_Master_Id { get; set; }
        public int Sl { get; set; }
        public string Medicine_Id { get; set; }
        public string Medicine_Name { get; set; }
        public string Batch_No { get; set; }
        public decimal Buying_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public string Expiry_Date { get; set; }
        public int Stock_Qty { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Price { get; set; }
        public decimal Grand_Total { get; set; }
        public decimal Discount_Price { get; set; }
        public string CreatedOn { get; set; }
        public string LastModifiedOn { get; set; }
    }



}
