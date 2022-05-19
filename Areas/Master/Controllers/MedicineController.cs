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


        [HttpPost]
        public async Task<DataTableResponse<MedicineModel>> GetProducts()
       {
            var request = new DataTableRequest();

            request.Draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            request.Start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            request.Length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            request.Search = new DataTableSearch()
            {
                Value = Request.Form["search[value]"].FirstOrDefault()
            };
            request.Order = new DataTableOrder[] {
                new DataTableOrder()
                {
                    Dir = Request.Form["order[0][dir]"].FirstOrDefault(),
                    Column = Convert.ToInt32(Request.Form["order[0][column]"].FirstOrDefault())
                }};

            return await _productService.GetProductsAsync(request);
        }

        

        ////[HttpPost]
        //public async Task<JsonResult> GetFilteredItems()
        //{
        //    System.Threading.Thread.Sleep(2000);//Used to display loading message in demonstration, remove this line in production
        //    int draw = Convert.ToInt32(Request.Query["draw"]);

        //    // Data to be skipped , 
        //    // if 0 first "length" records will be fetched
        //    // if 1 second "length" of records will be fethced ...
        //    int start = Convert.ToInt32(Request.Query["start"]);

        //    // Records count to be fetched after skip
        //    int length = Convert.ToInt32(Request.Query["length"]);

        //    // Getting Sort Column Name
        //    int sortColumnIdx = Convert.ToInt32(Request.Query["order[0][column]"]);
        //    string sortColumnName = Request.Query["columns[" + sortColumnIdx + "][name]"];

        //    // Sort Column Direction  
        //    string sortColumnDirection = Request.Query["order[0][dir]"];

        //    // Search Value
        //    string searchValue = Request.Query["search[value]"].FirstOrDefault()?.Trim();

        //    // Total count matching search criteria 
        //    //int recordsFilteredCount =
        //    //        Data.StudentContext.StudentList
        //    //        .Where(a => a.Lastname.Contains(searchValue) || a.Firstname.Contains(searchValue))
        //    //        .Count();

        //    int recordsFilteredCount = unitOfWork.Medicines.GetSearchValueCount(searchValue);


        //    // Total Records Count
        //    int recordsTotalCount = unitOfWork.Medicines.GetAllAsync().Result.Count;

        //    // Filtered & Sorted & Paged data to be sent from server to view
        //    List<MedicineModel> filteredData = null;
        //    if (sortColumnDirection == "asc")
        //    {
        //        //filteredData =
        //        //    Data.StudentContext.StudentList
        //        //    .Where(a => a.Lastname.Contains(searchValue) || a.Firstname.Contains(searchValue))
        //        //    .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))//Sort by sortColumn
        //        //    .Skip(start)
        //        //    .Take(length)
        //        //    .ToList<Student>();
        //        filteredData = unitOfWork.Medicines.GetSearchValue(searchValue);
        //    }
        //    else
        //    {
        //        //filteredData =
        //        //   Data.StudentContext.StudentList
        //        //   .Where(a => a.Lastname.Contains(searchValue) || a.Firstname.Contains(searchValue))
        //        //   .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
        //        //   .Skip(start)
        //        //   .Take(length)
        //        //   .ToList<Student>();
        //        filteredData = unitOfWork.Medicines.GetSearchValue(searchValue);
        //    }

        //    return Json(
        //                new
        //                {
        //                    data = filteredData,
        //                    draw = Request.Query["draw"],
        //                    recordsFiltered = recordsFilteredCount,
        //                    recordsTotal = recordsTotalCount
        //                }
        //            );
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
