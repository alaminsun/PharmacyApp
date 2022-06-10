using Dapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhramacyApp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.ExternalLoginModel;

namespace PhramacyApp.Views.Shared.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IDataAccessService _dataAccessService;

        public SidebarViewComponent(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

       public async Task<IViewComponentResult> InvokeAsync()
       {
            //var items = await _dataAccessService.GetMenuHeadsAsync(HttpContext.User);
            //var item1 = await _dataAccessService.GetMenuHeadsAsync(HttpContext.User);
            string baseUrl = Request.Scheme + "://" + Request.Host.Value;
            var httpRequestFeature = Request.HttpContext.Features.Get<IHttpRequestFeature>();
            var uri = httpRequestFeature.RawTarget;
            var id = HttpContext.Request.Query["id"].ToString();  //Parse Query String
            //var menuItems = await _dataAccessService.GetAllMenuItems(baseUrl + uri, id); //Can pass in testId here if required.

            var menuItems = await _dataAccessService.GetMenuHeadsAsync(HttpContext.User);
            int parentId = 0;
            var subMenuItems = await _dataAccessService.GetSubMenuAsync(parentId, HttpContext.User);
            return View(await _dataAccessService.GetMenuAsync(menuItems, subMenuItems, parentId, HttpContext.User));
            //return View(items);

            //return View();
        }

        //public async Task<IViewComponentResult> SubMenuAsync()
        //{
        //    var items = await _dataAccessService.GetSubMenus(HttpContext.User);

        //    return View(items);

        //    //return View(menuHeads);
        //}

    }
}