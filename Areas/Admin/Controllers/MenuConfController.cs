using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Controllers;
using PhramacyApp.Interfaces;
using PhramacyApp.Interfaces.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuConfController : BaseController<MenuConfController>
    {
        private readonly ISubMenuRepository _subMenuRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly IMenuConfRepository _menuConfRepo;
        private readonly RoleManager<IdentityRole> _roleManager;



        private readonly IUnitOfWork unitOfWork;

        public MenuConfController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, 
            IMenuConfRepository menuConfRepo, IMenuRepository menuRepo, ISubMenuRepository subMenuRepo)
        {
            this.unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _menuConfRepo = menuConfRepo;
            _menuRepo = menuRepo;
            _subMenuRepo = subMenuRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadAll()
        {
            var menus = await unitOfWork.MenuConfigs.GetAllAsync();
            if (menus != null)
            {
                var viewModel = menus;
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        public async Task<JsonResult> GetMenuByRole()
        {
            var menus = await unitOfWork.Menus.GetAllAsync();

            var menusList = menus.ToList();
            menusList.Insert(0, new MenuViewModel { Mh_Id = 0, Mh_Name = "Select" });

            return Json(new SelectList(menusList, "Mh_Id", "Mh_Name"));
        }
        public async Task<JsonResult> GetSubMenu(string Role_Id, int mhId)
        {
            var subMenusbyMenu = await unitOfWork.SubMenus.GetAllSubMenuByMenuId(Role_Id, mhId);

            var subMenusbyMenuId = subMenusbyMenu.ToList();
            subMenusbyMenuId.Insert(0, new SubMenuViewModel { Sm_Id = 0, Sm_Name = "Select" });

            return Json(new SelectList(subMenusbyMenuId, "Sm_Id", "Sm_Name"));
        }

        public async Task<ActionResult> OnGetCreateOrEdit(int id = 0)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var menus = await unitOfWork.Menus.GetAllAsync();
            var subMenus = await unitOfWork.SubMenus.GetAllAsync();
            if (id == 0)
            {
                var menuConfViewModel = new MenuConfViewModel();
                
                if (roles != null)
                {
                    var roleList = await _roleManager.Roles.ToListAsync();
                    roles.Insert(0, new IdentityRole { Id = "0", Name = "Select" });
                    menuConfViewModel.Roles = new SelectList(roleList, nameof(IdentityRole.Id), nameof(IdentityRole.Name), null, null);
                }
                if (menus != null)
                {
                    menuConfViewModel.Menus = new SelectList(string.Empty, nameof(MenuViewModel.Mh_Id), nameof(MenuViewModel.Mh_Name), null, null);
                }
                if (subMenus != null)
                {
                    menuConfViewModel.SubMenus = new SelectList(string.Empty, nameof(SubMenuViewModel.Sm_Id), nameof(SubMenuViewModel.Sm_Name), null, null);
                }
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", menuConfViewModel) });
            }
            else
            {
                var MenuInfo = await unitOfWork.MenuConfigs.GetByIdAsync(id);
                if (MenuInfo != null)
                {
                    if(roles != null){
                        var roleViewModel = roles;
                        MenuInfo.Roles = new SelectList(roleViewModel, nameof(IdentityRole.Id), nameof(IdentityRole.Name), null, null);
                    }
                    if (menus != null)
                    {
                        var menuViewModel = menus;
                        MenuInfo.Menus = new SelectList(menuViewModel, nameof(MenuViewModel.Mh_Id), nameof(MenuViewModel.Mh_Name), null, null);
                    }
                    if (subMenus != null)
                    {
                        var menuViewModel = subMenus;
                        MenuInfo.SubMenus = new SelectList(menuViewModel, nameof(SubMenuViewModel.Sm_Id), nameof(SubMenuViewModel.Sm_Name), null, null);
                    }
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", MenuInfo) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, MenuConfViewModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createMenu = await unitOfWork.MenuConfigs.AddAsync(entity);
                    id = createMenu.Mh_Id;

                    _notify.Success($"Menu with ID { id } Created.");
                }
                else
                {
                    var MenuInfo = await unitOfWork.MenuConfigs.GetByIdAsync(id);
                    if (MenuInfo == null)
                    {
                        return null;
                    }
                    await unitOfWork.MenuConfigs.UpdateAsync(id,entity);
                    _notify.Information($"Menu with ID { id } Updated.");
                }

                var MenuList = await unitOfWork.MenuConfigs.GetAllAsync();
                if (MenuList != null)
                {
                    var viewModel = MenuList;
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error("No Data Found");
                    return null;
                }
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_Create", entity);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteMenu = unitOfWork.MenuConfigs.DeleteAsync(id);
            if (deleteMenu != null)
            {
                _notify.Information($"Menu with Id {id} Deleted.");
                var MenuList = await unitOfWork.MenuConfigs.GetAllAsync();
                if (MenuList != null)
                {
                    var viewModel = MenuList;
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error("No Data Found");
                    return null;
                }
            }
            else
            {
                _notify.Error("Failed To Delete");
                return null;
            }
        }

    }
}
