using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Areas.Master.Models;
using PhramacyApp.Controllers;
using PhramacyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Controllers
{
    [Area("Master")]
    public class ShelfController : BaseController<ShelfController>
    {

        private readonly IUnitOfWork unitOfWork;

        public ShelfController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var model = new ShelfModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var selfList = await unitOfWork.Shelfs.GetAllAsync();
            if (selfList != null)
            {
                return PartialView("_viewall", selfList);
            }
            return null;
        }

        [Authorize(Policy = Permissions.Shelfs.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {

            if (id == 0)
            {
                var selfViewModel = new ShelfModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", selfViewModel) });
            }
            else
            {
                var shelf = await unitOfWork.Shelfs.GetByIdAsync(id);
                if (shelf != null)
                {
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", shelf) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, ShelfModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createShelf = await unitOfWork.Shelfs.AddAsync(entity);
                    id = entity.Id;
                    _notify.Success($"Shelf with ID { id } Created.");
                }

                else
                {
                    var self = await unitOfWork.Shelfs.GetByIdAsync(id);
                    if (self == null)
                    {
                        return null;
                    }
                    await unitOfWork.Shelfs.UpdateAsync(id, entity);
                    _notify.Information($"Shelf with ID { id } Updated.");
                }

                var selfList = await unitOfWork.Shelfs.GetAllAsync();
                if (selfList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", selfList);
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
            var deleteShelf = unitOfWork.Shelfs.DeleteAsync(id);
            if (deleteShelf != null)
            {
                _notify.Information($"Shelf with Id {id} Deleted.");
                var selfList = await unitOfWork.Shelfs.GetAllAsync();
                if (selfList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", selfList);
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
