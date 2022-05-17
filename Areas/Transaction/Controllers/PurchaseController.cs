using AspNetCore.Reporting;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Dashboard.Models;
using PhramacyApp.Areas.Master.Models;
using PhramacyApp.Areas.Transaction.Models;
using PhramacyApp.Controllers;
using PhramacyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace PhramacyApp.Areas.Transaction.Controllers
{
    [Area("Transaction")]
    public class PurchaseController : BaseController<PurchaseController>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        //private readonly IWebHostEnvironment _oHostEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly IRepositoryInvoice _repositoryInvoice;
        public PurchaseController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        }
        public IActionResult Index(string from)
        {
            //var supplierlist = await unitOfWork.Suppliers.GetAllAsync();
            ViewBag.from = from;
            var model = new PurchaseMasterModel();
            //model.Suppliers = new SelectList(supplierlist, nameof(SupplierModel.SupplierId), nameof(SupplierModel.SupplierName), null, null);
            return View(model);
        }

        public async Task<IActionResult> LoadAll(string from)
        {
            ViewBag.from = from;
            //var model = new PurchaseMasterModel();
            var PurchaseList = await unitOfWork.Purchases.GetAllAsync();
            if (PurchaseList != null)
            {
                return PartialView("_viewall", PurchaseList);
            }
            //return PartialView("_viewall", model);
            //return View("_viewall", model);
            return null;
        }

        public async Task<ViewResult> RedirectAction()
        {
            var PurchaseList = await unitOfWork.Purchases.GetAllAsync();
            if (PurchaseList != null)
            {
                return View("_viewall", PurchaseList);
            }
            return null;
        }

        //public IActionResult Index1()
        //{
        //    var model = new PurchaseModel();
        //    return View(model);
        //    //return View();
        //}

        [HttpGet]
        public async Task<IActionResult> PurchaseMasterData(int id)
        {
            var masterData = await unitOfWork.Purchases.GetPurchaseMasterData(id);
            return Json(data: masterData);
            //return PartialView("_MedicineList", medicineList);
        }

        public async Task<JsonResult> SupplierList()
        {
            var supplierlist = await unitOfWork.Suppliers.GetAllAsync();
            return Json(data: supplierlist);
        }

        [HttpPost]
        public async Task<JsonResult> MedicineList(string Prefix)
        {
            var medicineList = await unitOfWork.Purchases.GetAllMedicine();
            //return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_MedicineList", medicineList) });
            //return PartialView("_MedicineList", medicineList);
            return Json(data: medicineList);
        }

        [HttpPost]
        public async Task<JsonResult> Medicine(string medicine, string manufacturer_id)
        {
            var medicineList = await unitOfWork.Purchases.GetMedicineInfo(medicine, manufacturer_id);
            return Json(data: medicineList);
        }

        [HttpPost]
        public async Task<JsonResult> MedicineInfo(string medicineId, string manufacturer_id)
        {
            var medicineList = await unitOfWork.Purchases.GetMedicineId(medicineId, manufacturer_id);
            //return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_MedicineList", medicineList) });
            //return PartialView("_MedicineList", medicineList);
            return Json(data: medicineList);
        }


        //[Authorize(Policy = Permissions.Pu.View)]
        [HttpGet]
        public async Task<ActionResult> OnGetCreateOrEdit(int id = 0)
        {

            //var categoryList = await unitOfWork.Categories.GetAllAsync();
            //var ShelfList = await unitOfWork.Shelfs.GetAllAsync();
            var SupplierList = await unitOfWork.Suppliers.GetAllAsync();
            var model = new PurchaseMasterModel();
            if (id == 0)
            {
                var purchaseModel = model;
                //if (categoryList != null)
                //{
                //    var categories = categoryList.ToList();
                //    //categories.Insert(0, new CategoryModel { Id = 0, CategoryName = "Select Categoy" });
                //    medicineModel.Categories = new SelectList(categories, nameof(CategoryModel.Id), nameof(CategoryModel.CategoryName), null, null);
                //}
                //if (ShelfList != null)
                //{
                //    var shelfs = ShelfList.ToList();
                //    //shelfs.Insert(0, new ShelfModel { Id = 0, ShelfName = "Select Shelf" });
                //    medicineModel.Shelfs = new SelectList(shelfs, nameof(ShelfModel.Id), nameof(ShelfModel.ShelfName), null, null);
                //}
                if (SupplierList != null)
                {
                    purchaseModel.Suppliers = new SelectList(SupplierList, nameof(SupplierModel.Supplier_Id), nameof(SupplierModel.Supplier_Name), null, null);
                }
                //return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", purchaseModel) });
                return View("CreateForm", purchaseModel);
            }
            else
            {
                var purchaseInfo = await unitOfWork.Purchases.GetByMasterIdAsync(id);
                if (purchaseInfo != null)
                {

                    if (SupplierList != null)
                    {
                        purchaseInfo.Suppliers = new SelectList(SupplierList, nameof(SupplierModel.Supplier_Id), nameof(SupplierModel.Supplier_Name), null, null);
                    }
                   
                    return View("EditForm", purchaseInfo);
                }
                return null;

            }

        }

        [HttpGet]
        public async Task<ActionResult> GetPurchaseDetail(int id)
        {

            var SupplierList = await unitOfWork.Suppliers.GetAllAsync();

            var purchaseInfo = await unitOfWork.Purchases.GetByMasterIdAsync(id);
            if (purchaseInfo != null)
            {
                if (SupplierList != null)
                {
                    purchaseInfo.Suppliers = new SelectList(SupplierList, nameof(SupplierModel.Supplier_Id), nameof(SupplierModel.Supplier_Name), null, null);
                }
                return View("PurchaseDetail", purchaseInfo);
            }
            return null;

        }

        public async Task<List<PurchaseDetailModel>> GetStockList(int id)
        {
            var query = "Select pd.Medicine_Id, m.Medicine_Name,pd.Batch_No,pd.Quantity,pd.Buying_Price, m.Selling_Price,m.Expiry_Date, pd.Total_Price " +
        " from Purchase_Detail_tbl pd, MedicineInfo m " +
        " where m.Medicine_Id = pd.Medicine_Id And pd.Purchase_Master_Id="+id+" Order By pd.Medicine_Id ";
            //"and Convert(date,m.expiry_date,103) < Convert(date,'" + currentDate + "',103)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                List<PurchaseDetailModel> items = new List<PurchaseDetailModel>();
                //var model = new PurchaseDetailModel();
                connection.Open();
                var result = await connection.QueryAsync<PurchaseDetailModel>(query);
                var length = result.Count();
                int Sl = 1;
                if (length > 0)
                {
                    foreach (var item in result)
                    {
                        var model = new PurchaseDetailModel();
                        model.Sl = Sl;
                        model.Medicine_Id = item.Medicine_Id;
                        model.Medicine_Name = item.Medicine_Name;
                        model.Batch_No = item.Batch_No;
                        model.Quantity = item.Quantity;
                        model.Buying_Price = item.Buying_Price;
                        //model.Selling_Price = item.Selling_Price;
                        model.Expiry_Date = item.Expiry_Date;

                        model.Total_Price = item.Total_Price;
                        //model.Quantity = item.Quantity;
                        //model.Purchase_Detail_Id = item.Purchase_Detail_Id;
                        //model.Purchase_Master_Id = item.Purchase_Master_Id;
                        Sl++;
                        items.Add(model);
                    }

                }

                return items;
            }
        }

        public async Task<IActionResult> StockList(int id)
        {
            var result = await GetStockList(id);
            //return PartialView("_viewall", PurchaseList);
            //return PartialView("_viewAll", result);
            return Json(data: result);

        }

        [HttpPost]
        public IActionResult PrintInvoice(int param)
        {
            var query = "Select pd.Medicine_Id, m.Medicine_Name,pd.Batch_No,pd.Quantity,pd.Buying_Price, m.Selling_Price,m.Expiry_Date " +
        " from Purchase_Detail_tbl pd, MedicineInfo m " +
        " where m.Medicine_Id = pd.Medicine_Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                List<PurchaseDetailModel> items = new List<PurchaseDetailModel>();
                //var model = new PurchaseDetailModel();
                connection.Open();
                var result = connection.Query<PurchaseDetailModel>(query);
                var length = result.Count();
                int Sl = 1;
                if (length > 0)
                {
                    foreach (var item in result)
                    {
                        var model = new PurchaseDetailModel();
                        model.Sl = Sl;
                        model.Medicine_Id = item.Medicine_Id;
                        model.Medicine_Name = item.Medicine_Name;
                        model.Batch_No = item.Batch_No;
                        model.Buying_Price = item.Buying_Price;
                        model.Expiry_Date = item.Expiry_Date;
                        model.Selling_Price = item.Selling_Price;
                        //model.Stock_Qty = item.Stock_Qty;
                        //model.Total_Price = item.Total_Price;
                        //model.Quantity = item.Quantity;
                        //model.Purchase_Detail_Id = item.Purchase_Detail_Id;
                        //model.Purchase_Master_Id = item.Purchase_Master_Id;
                        Sl++;
                        items.Add(model);
                    }

                }

                InvoiceReport rpt = new InvoiceReport(_webHostEnvironment);
                return File(rpt.Report(items), "application/pdf");

                //return items;
            }
        }



        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, PurchaseMasterModel entity)
        {
            if (entity != null)
            {
                if (entity.Purchase_Master_Id == 0)
                {
                    var createPurchase = await unitOfWork.Purchases.AddAsync(entity);
                    id = entity.Purchase_Master_Id;
                    //_notify.Success($"Purchase with ID { id } Created.");
                }

                else
                {
                    var purchase = await unitOfWork.Purchases.GetByMasterIdAsync(entity.Purchase_Master_Id);
                    if (purchase == null)
                    {
                        return null;
                    }
                    await unitOfWork.Purchases.UpdatePurchaseAsync(entity.Purchase_Master_Id, entity);
                    //_notify.Information($"Purchase with ID { entity.Purchase_Master_Id } Updated.");
                }

                //var purchaseList = await unitOfWork.Purchases.GetAllAsync();
                //var html = await _viewRenderer.RenderViewToStringAsync("_ManagePurchase", purchaseList);
                //return new JsonResult(new { isValid = true, html = html });

                //return RedirectToAction("_ManagePurchase", purchaseList);

                var purchaseList = await unitOfWork.Purchases.GetAllAsync();

                if (purchaseList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_viewall", purchaseList);
                    return new JsonResult(new { isValid = true, html = html });
                    //return Redirect("/transaction/purchase");
                    //return Redirect("~/transaction/Purchase/RedirectAction");
                    //return RedirectToAction(nameof(Index));
                }

                else
                {
                    _notify.Error("Failed To Save");
                    return null;
                }
            }
            else
            {
                //_notify.Error("Failed To Save");
                //return null;
                var html = await _viewRenderer.RenderViewToStringAsync("CreateForm", entity);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<JsonResult> loaddata(int Purchase_Master_Id)
        {
            var loadMedicine = await unitOfWork.Purchases.GetDetailListByMasterId(Purchase_Master_Id);
            return Json(data: loadMedicine);
        }

        [HttpPost]
        public async Task<JsonResult> GetChildDelete(int detailId, int masterId)
        {
            var deleteChield = unitOfWork.Purchases.DeleteChildAsync(detailId, masterId);
            if (deleteChield != null)
            {
                _notify.Error($"Purchase with Id {detailId} Deleted.");
                var purchaseList = await unitOfWork.Purchases.GetAllAsync();
                if (purchaseList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_viewall", purchaseList);
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

        [HttpPost]
        public async Task<ActionResult> GetMasterDelete(int masterId)
        {
            var deleteMaster = await unitOfWork.Purchases.DeleteMasterAsync(masterId);
            if (deleteMaster != 0)
            {
                _notify.Information($"Purchase with Id {masterId} Deleted.");
            }
            else
            {
                _notify.Error("Failed To Delete");
                return null;
            }

            var purchaseList = await unitOfWork.Purchases.GetAllAsync();
            var html = await _viewRenderer.RenderViewToStringAsync("_viewall", purchaseList);
            return new JsonResult(new { isValid = true, html = html });


        }

        [HttpPost]
        public async Task<IActionResult> Print(string Purchase_Date, int Invoice_No, string Supplier_Name, string Status, string Payment_Type, string Grand_Total, string Discount)
        {
            string mimtype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Purchase.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Invoice_no", Invoice_No.ToString());
            parameters.Add("Purchase_Date", Purchase_Date);
            parameters.Add("Supplier_Name", Supplier_Name);
            //parameters.Add("Supplier_Phone", Supplier_Phone);
            //parameters.Add("Customer_Email", Supplier_Email);
            parameters.Add("Discount", Discount);
            parameters.Add("Grand_Total", Grand_Total);
            parameters.Add("Pymt_Mode", Payment_Type);
            parameters.Add("Status", Status);
            var Invoices = await unitOfWork.Purchases.GetPurchaseList();
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", Invoices);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
            return File(result.MainStream, "application/pdf");
        }

    }

}
