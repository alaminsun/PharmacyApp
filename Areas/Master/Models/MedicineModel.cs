using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Models
{

    public class Medicine
    {
        public List<MedicineModel> MedicineList { get; set; }
        public int CurrentIndex { get; set; }
        public int PageCount { get; set; }
    }
    public partial class MedicineModel
    {
        public int Id { get; set; }
        public string Category_Id { get; set; }
        public string Category_Name { get; set; }
        public string Shelf_Id { get; set; }
        public string Shelf_Name { get; set; }
        public string  Medicine_Name { get; set; }
        public string Generic_Name { get; set; }
        public string Supplier_Id { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Nic_Name { get; set; }
        
        public SelectList SupplierList { get; set; }
        public string Batch_No { get; set; }
        public decimal Buying_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Shelfs { get; set; }
        public string Expiry_Date { get; set; }
        public string CreatedOn { get; internal set; }
        public string LastModifiedOn { get; internal set; }
        public string Medicine_Id { get; set; }
        public string Strength_Code { get; set; }
        public string Strength_Name { get; set; }
    }

    public partial class MedicineModel
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }


}
