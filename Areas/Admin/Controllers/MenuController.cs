using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Controllers;
using PhramacyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : BaseController<MenuController>
    {
        private readonly IUnitOfWork unitOfWork;

        public MenuController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var model = new MenuViewModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var menuList = await unitOfWork.Menus.GetAllAsync();
            if (menuList != null)
            {
                var viewmodel = menuList;
                return PartialView("_viewall", viewmodel);
            }
            return null;
        }

        [Authorize(Policy = Permissions.Menus.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
         {

            if (id == 0)
            {
                var menuViewModel = new MenuViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", menuViewModel) });
            }
            else
            {
                var menu = await unitOfWork.Menus.GetByIdAsync(id);
                if (menu != null)
                {
                    //if (roles != null)
                    //{
                    //    var role = roles;
                    //    menu.Roles = new SelectList(role, nameof(IdentityRole.Id), nameof(IdentityRole.Name), null, null);
                    //}
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", menu) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, MenuViewModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createMenu = await unitOfWork.Menus.AddAsync(entity);
                    id = entity.Mh_Id;
                    _notify.Success($"Menu with ID { id } Created.");
                }

                else
                {
                    var menuInfo = await unitOfWork.Menus.GetByIdAsync(id);
                    if (menuInfo == null)
                    {
                        return null;
                    }
                    await unitOfWork.Menus.UpdateAsync(id,entity);
                    _notify.Information($"Menu with ID { id } Updated.");
                }

                var menuList = await unitOfWork.Menus.GetAllAsync();
                if (menuList != null)
                {
                    var viewModel = menuList;
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }

                else
                {
                    _notify.Error("Failed To Save");
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
            var deleteMenu = unitOfWork.Menus.DeleteAsync(id);
            if (deleteMenu != null)
            {
                _notify.Information($"Menu with Id {id} Deleted.");
                var MenuList = await unitOfWork.Menus.GetAllAsync();
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
