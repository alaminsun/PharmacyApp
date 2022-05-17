using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Dashboard.Models
{
    public class DashboardViewModel
    {
        public int self_count { get; set; }
        public int medicine_count { get; set; }
        public int stock_count { get; set; }
        public int supplier_count { get; set; }
        public int purchase_count { get; set; }
        public decimal purchase_amount { get; set; }
        public int sale_count { get; set; }
        public decimal sale_amount { get; set; }
        public List<DashboardViewModel> lowStocks{ get; set; }
        public int Sl { get; internal set; }
        public string Medicine_Id { get; set; }
        public string Medicine_Name { get; set; }
        public string Batch_No { get; set; }
        public decimal Buying_Price { get; set; }
        public decimal Selling_Price { get; set; }
        public int Quantity { get; set; }
        public string Expiry_Date { get; set; }

    }
}
