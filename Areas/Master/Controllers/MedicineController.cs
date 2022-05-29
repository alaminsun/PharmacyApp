using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Areas.Master.Models;
using PhramacyApp.Controllers;
using PhramacyApp.Interfaces;
using PhramacyApp.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Controllers
{
    [Area("Master")]
    public class MedicineController : BaseController<MedicineController>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductService _productService;

        public MedicineController(IUnitOfWork unitOfWork, IProductService productService)
        {
            this.unitOfWork = unitOfWork;
            _productService = productService;
        }
        public IActionResult Index()
        {
            var model = new MedicineModel();
            return View(model);
        }

        public async Task<IActionResult> GetAllMedicine()
        {
            //var medicineList = await unitOfWork.Medicines.GetAllAsync();
            var medicineList = await unitOfWork.Medicines.GetAllAsync();
            return Json(data: medicineList); 
        }


        //[HttpPost]
        //public async Task<DataTableResponse<MedicineModel>> GetProducts()
        //{
        //    var request = new DataTableRequest();

        //    request.Draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
        //    request.Start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
        //    request.Length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
        //    request.Search = new DataTableSearch()
        //    {
        //        Value = Request.Form["search[value]"].FirstOrDefault()
        //    };
        //    request.Order = new DataTableOrder[] {
        //         new DataTableOrder()
        //         {
        //             Dir = Request.Form["order[0][dir]"].FirstOrDefault(),
        //             Column = Convert.ToInt32(Request.Form["order[0][column]"].FirstOrDefault())
        //         }};

        //    return await _productService.GetProductsAsync(request);

        //     //await _productService.GetProductsAsync(request);
        //    //return Json(data: medicineList);
        //}

        [HttpPost]
        public IActionResult GetMedicineList()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //var customerData = (from tempcustomer in context.Customers select tempcustomer);
                var medicineData = unitOfWork.Medicines.GetAllMedicine();
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                    medicineData =  unitOfWork.Medicines.medicineDataForSorting(sortColumn, sortColumnDirection);
                }

                    //int recordsFilteredCount = unitOfWork.Medicines.GetSearchValueCount(searchValue);


                // // Total Records Count
                 recordsTotal = unitOfWork.Medicines.GetAllAsync().Result.Count; 
                if (!string.IsNullOrEmpty(searchValue))
                {

                    medicineData = unitOfWork.Medicines.GetSearchValue(searchValue, sortColumn, sortColumnDirection);
                    //customerData = customerData.Where(m => m.FirstName.Contains(searchValue)
                    //                            || m.LastName.Contains(searchValue)
                    //                            || m.Contact.Contains(searchValue)
                    //                            || m.Email.Contains(searchValue));
                }
                recordsTotal = medicineData.Count();
                var data = medicineData.Skip(skip).Take(pageSize).ToList();
                //var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(
                            new
                            {
                                data = data,
                                draw = draw,
                                recordsFiltered = recordsTotal,
                                recordsTotal = recordsTotal
                            }
                        );
                //return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //}



        public async Task<IActionResult> LoadAll(int Currentpage)
        {
            //var medicineList = await unitOfWork.Medicines.GetAllAsync();
            var medicineList = await unitOfWork.Medicines.GetMedicine(1);
            if (medicineList != null)
            {
                return PartialView("Index", medicineList);
            }
            return null;
        }

        //public async Task<JsonResult> SupplierList()
        //{
        //    var supplierlist = await unitOfWork.Suppliers.GetAllAsync();
        //    return Json(data: supplierlist);
        //}

        public JsonResult SupplierNicNameList(string suplier_Id)
        {
            var supplierNicName = unitOfWork.Medicines.GetSupplierNicName(suplier_Id);
            return Json(data: supplierNicName);
        }

        [Authorize(Policy = Permissions.Medicines.View)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {

            var categoryList = await unitOfWork.Categories.GetAllAsync();
            var ShelfList = await unitOfWork.Shelfs.GetAllAsync();
            var SupplierList = await unitOfWork.Suppliers.GetAllAsync();
            if (id == 0)
            {
                var medicineModel = new MedicineModel();
                if (categoryList != null)
                {
                    var categories = categoryList.ToList();
                    //categories.Insert(0, new CategoryModel { Id = 0, CategoryName = "Select Categoy" });
                    medicineModel.Categories = new SelectList(categories, nameof(CategoryModel.Id), nameof(CategoryModel.Category_Name), null, null);
                }
                if (ShelfList != null)
                {
                    var shelfs = ShelfList.ToList();
                    //shelfs.Insert(0, new ShelfModel { Id = 0, ShelfName = "Select Shelf" });
                    medicineModel.Shelfs = new SelectList(shelfs, nameof(ShelfModel.Id), nameof(ShelfModel.Shelf_Name), null, null);
                }
                if (SupplierList != null)
                {
                    var suppliers = SupplierList.ToList();
                    //shelfs.Insert(0, new ShelfModel { Id = 0, ShelfName = "Select Shelf" });
                    medicineModel.SupplierList = new SelectList(suppliers, nameof(SupplierModel.Supplier_Id), nameof(SupplierModel.Supplier_Name), null, null);
                }
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", medicineModel) });
            }
            else
            {
                var medicineInfo = await unitOfWork.Medicines.GetByIdAsync(id);
                if (medicineInfo != null)
                {
                    if (categoryList != null)
                    {
                        medicineInfo.Categories = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Category_Name), null, null);
                    }
                    if (ShelfList != null)
                    {
                        medicineInfo.Shelfs = new SelectList(ShelfList, nameof(ShelfModel.Id), nameof(ShelfModel.Shelf_Name), null, null);
                    }
                    if (SupplierList != null)
                    {
                        //shelfs.Insert(0, new ShelfModel { Id = 0, ShelfName = "Select Shelf" });
                        medicineInfo.SupplierList = new SelectList(SupplierList, nameof(SupplierModel.Supplier_Id), nameof(SupplierModel.Supplier_Name), null, null);
                    }
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", medicineInfo) });
                }
                return null;

            }

        }

        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, MedicineModel entity)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var createMedicine = await unitOfWork.Medicines.AddAsync(entity);
                    id = entity.Id;
                    _notify.Success($"Medicine with ID { id } Created.");
                }

                else
                {
                    var medicine = await unitOfWork.Medicines.GetByIdAsync(id);
                    if (medicine == null)
                    {
                        return null;
                    }
                    
                    await unitOfWork.Medicines.UpdateAsync(id, entity);
                    _notify.Information($"Medicine with ID { id } Updated.");
                }

                var medicineList = await unitOfWork.Medicines.GetAllAsync();
                if (medicineList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("Index", medicineList);
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
            var deleteMedicine = unitOfWork.Medicines.DeleteAsync(id);
            if (deleteMedicine != null)
            {
                _notify.Information($"Medicine with Id {id} Deleted.");
                var medicineList = await unitOfWork.Medicines.GetAllAsync();
                if (medicineList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("Index", medicineList);
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
