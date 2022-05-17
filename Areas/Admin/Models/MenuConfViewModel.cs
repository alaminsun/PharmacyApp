using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Admin.Models
{
    public class MenuConfViewModel
    {
        public int Menu_Id { get; set; }
        public string Role_Id { get; set; }
        public string Role_Name { get; set; }
        public int Sm_Id { get; set; }
        public int Sm_Seq { get; set; }
        public string Sm_Name { get; set; }
        public string URL { get; set; }
        public int Mh_Id { get; set; }
        public string Mh_Name { get; set; }
        public string SubName { get; set; }
        public SelectList Roles { get; set; }
        public SelectList Menus { get; set; }
        public SelectList SubMenus { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now.Date;
        public DateTime LastModifiedOn { get; set; } = DateTime.Now.Date;
        public string Rl_Id { get; internal set; }
    }
}
