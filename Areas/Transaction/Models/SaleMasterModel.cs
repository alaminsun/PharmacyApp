using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Transaction.Models
{
    public class SaleMasterModel
    {
        public int Sl { get; set; }
        public int Sale_Master_Id { get; set; }
        //public int PurchaseId { get; set; }
        public string Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public SelectList Customers { get; set; }
        public string Selling_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Details { get; set; }
        public string Payment_Type { get; set; }
        public string Status { get; set; }
        public decimal Discount_Price { get; set; }
        public decimal Grand_Total { get; set; }
        public string CreatedOn { get; set; }
        public string LastModifiedOn { get; set; }

        public List<SaleDetailModel> DetailList { get; set; }
    }

    public class SaleDetailModel
    {
        public int Sale_Detail_Id { get; set; }
        public int Sale_Master_Id { get; set; }
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
