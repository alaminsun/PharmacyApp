using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Category_Name { get; set; }
        public string Description { get; set; }
        public string CreatedOn { get; internal set; }
        public string LastModifiedOn { get; internal set; }
    }
}
