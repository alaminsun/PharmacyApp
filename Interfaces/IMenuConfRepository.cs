
using Microsoft.AspNetCore.Mvc;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces.Repositories
{
    public interface IMenuConfRepository : IGenericRepository<MenuConfViewModel>
    {

    }
}
