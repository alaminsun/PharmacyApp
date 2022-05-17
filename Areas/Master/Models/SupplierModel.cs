using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Models
{
    public class SupplierModel
    {
        public int Id { get; set; }
        public string Supplier_Id { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Nic_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string CreatedOn { get; internal set; }
        public string LastModifiedOn { get; internal set; }
    }
}
