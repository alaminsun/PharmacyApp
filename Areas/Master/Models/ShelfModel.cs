using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Models
{
    public class ShelfModel
    {
        public int Id { get; set; }
        public string Shelf_Name { get; set; }
        public string Shelf_Number { get; set; }
        public DateTime CreatedOn { get; internal set; }
        public DateTime LastModifiedOn { get; internal set; }
    }
}
