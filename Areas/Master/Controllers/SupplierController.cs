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
    public class SupplierController : BaseController<SupplierController>
    {
        private readonly IUnitOfWork unitOfWork;

        public SupplierController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var model = new SupplierModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var supplierList = await unitOfWork.Suppliers.GetAllAsync();
            if (supplierList != null)
            {
                return PartialView("_viewall", supplierList);
            }
            return null;
        }

        [Authorize(Policy = Permissions.Suppliers.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var supplierModel = new SupplierModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", supplierModel) });
            }
            else
            {
                var supplierInfo = await unitOfWork.Suppliers.GetByIdAsync(id);
                if (supplierInfo != null)
                {
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", supplierInfo) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, SupplierModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createSupplier = await unitOfWork.Suppliers.AddAsync(entity);
                    //id = entity.Id;
                    _notify.Success($"Supplier with ID { entity.Id } Created.");
                }

                else
                {
                    var supplier = await unitOfWork.Suppliers.GetByIdAsync(id);
                    if (supplier == null)
                    {
                        return null;
                    }
                    await unitOfWork.Suppliers.UpdateAsync(id, entity);
                    _notify.Information($"Supplier with ID { id } Updated.");
                }

                var supplierList = await unitOfWork.Suppliers.GetAllAsync();
                if (supplierList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", supplierList);
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
            var deleteSupplier = unitOfWork.Suppliers.DeleteAsync(id);
            if (deleteSupplier != null)
            {
                _notify.Information($"Supplier with Id {id} Deleted.");
                var supplierList = await unitOfWork.Suppliers.GetAllAsync();
                if (supplierList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", supplierList);
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
