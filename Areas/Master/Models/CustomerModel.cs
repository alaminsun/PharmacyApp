using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn { get; internal set; }
        public DateTime LastModifiedOn { get; internal set; }

    }
}
