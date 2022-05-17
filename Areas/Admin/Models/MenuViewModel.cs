using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Admin.Models
{
    public class MenuViewModel
    {
        public string Area { get; set; }
        public int Mh_Id { get; set; }
        public int Mh_Seq { get; set; }
        public string Role_Id { get; set; }
        public string Role_name { get; set; }
        public string Mh_Name { get; set; }
        public DateTime CreatedOn { get; set; } 
        public DateTime LastModifiedOn { get; set; } 
        public SelectList Roles { get; set; }

        //public virtual List<SubMenuViewModel> SubMenus { get; set; }
        //public string Sm_Name { get; internal set; }
        //public int Sm_Id { get; internal set; }
        //public string URL { get; set; }
        //public int MH_ID { get; set; }
        //public int MH_SEQ { get; set; }
        //public string MH_NAME { get; set; }
        //public DateTime CreatedOn { get; set; } = DateTime.Now.Date;
        //public DateTime LastModifiedOn { get; set; } = DateTime.Now.Date;
    }

    public class Menu
    {
        public int Mh_Id { get; set; }
        public int Mh_Seq { get; set; }
        public string Role_Id { get; set; }
        public string Role_name { get; set; }
        public string Mh_Name { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now.Date;
        public DateTime LastModifiedOn { get; set; } = DateTime.Now.Date;
        public SelectList Roles { get; set; }

        //public virtual List<SubMenuViewModel> SubMenus { get; set; }
        public string Sm_Name { get; internal set; }
        public int Sm_Id { get; internal set; }
        //public int MH_ID { get; set; }
        //public int MH_SEQ { get; set; }
        //public string MH_NAME { get; set; }
        //public DateTime CreatedOn { get; set; } = DateTime.Now.Date;
        //public DateTime LastModifiedOn { get; set; } = DateTime.Now.Date;
    }
}
