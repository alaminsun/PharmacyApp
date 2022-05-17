

using PhramacyApp.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces
{
    public interface IDataAccessService
    {
        //Task<IEnumerable<SubMenuViewModel>> GetMenuHeadsAsync(ClaimsPrincipal ctx);
        //Task<IEnumerable<SubMenuViewModel>> GetSubMenus(IEnumerable<SubMenuViewModel> menuItems, ClaimsPrincipal ctx);
        //Task GetMenuItems(ClaimsPrincipal user);
        //Task GetMenuItemsAsync(HttpContext httpContext, object user);
        Task<IList<MenuViewModel>> GetMenuHeadsAsync(ClaimsPrincipal ctx);
        //Task<IEnumerable<string>> GetMenus(IEnumerable<SubMenuViewModel> subMenuItems, IEnumerable<MenuViewModel> MenuItems, ClaimsPrincipal principal);
        Task<IList<SubMenuViewModel>> GetSubMenuAsync(int parentId, ClaimsPrincipal ctx);
        //Task<IEnumerable<SubMenuViewModel>> GetSubMenus(ClaimsPrincipal principal);
        Task<IList<SubMenuViewModel>> GetMenuAsync(IList<MenuViewModel> menuList, IList<SubMenuViewModel> subMenuList, int parentId, ClaimsPrincipal ctx);
    }
}
