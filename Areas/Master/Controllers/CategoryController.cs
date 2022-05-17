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
    public class CategoryController : BaseController<CategoryController>
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var model = new CategoryModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var categoryList = await unitOfWork.Categories.GetAllAsync();
            if (categoryList != null)
            {
                return PartialView("_viewall", categoryList);
            }
            return null;
        }

        [Authorize(Policy = Permissions.Categories.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {

            if (id == 0)
            {
                var categoryViewModel = new CategoryModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", categoryViewModel) });
            }
            else
            {
                var categoryInfo = await unitOfWork.Categories.GetByIdAsync(id);
                if (categoryInfo != null)
                {
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", categoryInfo) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, CategoryModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createCategory = await unitOfWork.Categories.AddAsync(entity);
                    id = entity.Id;
                    _notify.Success($"Category with ID { id } Created.");
                }

                else
                {
                    var category = await unitOfWork.Categories.GetByIdAsync(id);
                    if (category == null)
                    {
                        return null;
                    }
                    await unitOfWork.Categories.UpdateAsync(id, entity);
                    _notify.Information($"Category with ID { id } Updated.");
                }

                var categoryList = await unitOfWork.Categories.GetAllAsync();
                if (categoryList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", categoryList);
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
            var deleteCategory = unitOfWork.Categories.DeleteAsync(id);
            if (deleteCategory != null)
            {
                _notify.Information($"Category with Id {id} Deleted.");
                var categoryList = await unitOfWork.Categories.GetAllAsync();
                if (categoryList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", categoryList);
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
