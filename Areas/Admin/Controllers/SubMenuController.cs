using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Controllers;
using PhramacyApp.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubMenuController : BaseController<SubMenuController>
    {
        private readonly IUnitOfWork unitOfWork;

        public SubMenuController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var model = new SubMenuViewModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var subMenuList = await unitOfWork.SubMenus.GetAllAsync();
            if (subMenuList != null)
            {
                var viewModel = subMenuList;
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }

        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            var menuList = await unitOfWork.Menus.GetAllAsync();
            var menus = menuList.ToList();
            if (id == 0)
            {
                var subMenuViewModel = new SubMenuViewModel();  
                if (menuList != null)
                {
                    var menuViewModel = menus;
                    menuViewModel.Insert(0, new MenuViewModel { Mh_Id = 0, Mh_Name = "Select Menu Head" });
                    subMenuViewModel.Menus = new SelectList(menuViewModel, nameof(MenuViewModel.Mh_Id), nameof(MenuViewModel.Mh_Name), null, null);
                }
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", subMenuViewModel) });
            }
            else
            {
                var subMenu = await unitOfWork.SubMenus.GetByIdAsync(id);
                if (subMenu != null)
                {
                    if (menuList != null)
                    {
                        var menuViewModel = menuList;
                        subMenu.Menus = new SelectList(menuViewModel, nameof(MenuViewModel.Mh_Id), nameof(MenuViewModel.Mh_Name), null, null);
                    }
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", subMenu) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, SubMenuViewModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createSubMenu = await unitOfWork.SubMenus.AddAsync(entity);
                    id = entity.Sm_Id;

                    _notify.Success($"SubMenu with ID { id } Created.");
                }
                else
                {
                    var subMenuInfo = await unitOfWork.SubMenus.GetByIdAsync(id);
                    if (subMenuInfo == null)
                    {
                        return null;
                    }
                    await unitOfWork.SubMenus.UpdateAsync(id,entity);
                    _notify.Information($"SubMenu with ID { id } Updated.");
                }

                var subMenuList = await unitOfWork.SubMenus.GetAllAsync();
                if (subMenuList != null)
                {
                    var viewModel = subMenuList;
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
            var deleteSubMenu = unitOfWork.SubMenus.DeleteAsync(id);
            if (deleteSubMenu != null)
            {
                _notify.Information($"SubMenu with Id {id} Deleted.");
                var subMenuList = await unitOfWork.SubMenus.GetAllAsync();
                if (subMenuList != null)
                {
                    var viewModel = subMenuList;
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
