using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Models
{
    public class MedicineDTO
    {
        public int Id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string category_id { get; set; }
        public string shelf_id { get; set; }
        public string shelfName { get; set; }
        public string batch_id { get; set; }
        public decimal price { get; set; }
        public decimal manufacturer_price { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Shelfs { get; set; }
        public string expeire_date { get; set; }
        public DateTime CreatedOn { get; internal set; }
        public DateTime LastModifiedOn { get; internal set; }
        //public int MedicineId { get; internal set; }
    }
}
