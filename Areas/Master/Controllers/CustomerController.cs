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
    public class CustomerController : BaseController<CustomerController>
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var model = new CustomerModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll()
        {
            var customerList = await unitOfWork.Customers.GetAllAsync();
            if (customerList != null)
            {
                return PartialView("_viewall", customerList);
            }
            return null;
        }

        [Authorize(Policy = Permissions.Customers.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var customerModel = new CustomerModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", customerModel) });
            }
            else
            {
                var customerInfo = await unitOfWork.Customers.GetByIdAsync(id);
                if (customerInfo != null)
                {
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", customerInfo) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, CustomerModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createCustomer = await unitOfWork.Customers.AddAsync(entity);
                    //id = entity.Id;
                    _notify.Success($"Customer with ID { entity.Customer_Id } Created.");
                }

                else
                {
                    var customer = await unitOfWork.Customers.GetByIdAsync(id);
                    if (customer == null)
                    {
                        return null;
                    }
                    await unitOfWork.Customers.UpdateAsync(id, entity);
                    _notify.Information($"Customer with ID { id } Updated.");
                }

                var customerList = await unitOfWork.Customers.GetAllAsync();
                if (customerList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", customerList);
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
            var deleteCustomer = unitOfWork.Customers.DeleteAsync(id);
            if (deleteCustomer != null)
            {
                _notify.Information($"Customer with Id {id} Deleted.");
                var customerList = await unitOfWork.Customers.GetAllAsync();
                if (customerList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", customerList);
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
