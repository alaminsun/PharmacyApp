using AspNetCore.Reporting;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhramacyApp.Areas.Master.Models;
using PhramacyApp.Areas.Transaction.Models;
using PhramacyApp.Controllers;
using PhramacyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Transaction.Controllers
{
    [Area("Transaction")]
    public class SaleController : BaseController<SaleController>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SaleController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index(string from)
        {
            ViewBag.from = from;
            var model = new SaleMasterModel();
            return View(model);
        }

        public async Task<IActionResult> LoadAll(string from)
        {
            ViewBag.from = from;
            var SaleList = await unitOfWork.Sales.GetAllAsync();
            if (SaleList != null)
            {
                return PartialView("_viewall", SaleList);
            }
            return null;
        }

        public async Task<JsonResult> CustomerList()
        {
            var customerlist = await unitOfWork.Customers.GetAllAsync();
            return Json(data: customerlist);
        }

        [HttpPost]
        public async Task<JsonResult> Medicine(string medicine)
        {
            var medicineList = await unitOfWork.Sales.GetMedicineInfo(medicine);
            return Json(data: medicineList);
        }

        [HttpPost]
        public async Task<JsonResult> MedicineInfo(string medicineId, string customer_id)
        {
            var medicineList = await unitOfWork.Sales.GetMedicineId(medicineId, customer_id);
            return Json(data: medicineList);
        }

        [HttpGet]
        public async Task<ActionResult> OnGetCreateOrEdit(int id = 0)
        {
            var CustomerList = await unitOfWork.Customers.GetAllAsync();
            var model = new SaleMasterModel();
            if (id == 0)
            {
                var saleModel = model;
                if (CustomerList != null)
                {
                    saleModel.Customers = new SelectList(CustomerList, nameof(CustomerModel.Customer_Id), nameof(CustomerModel.Customer_Name), null, null);
                }
                
                return View("CreateForm", saleModel);
            }
            else
            {
                var saleInfo = await unitOfWork.Sales.GetByMasterIdAsync(id);
                if (saleInfo != null)
                {
                    if (CustomerList != null)
                    {
                        saleInfo.Customers = new SelectList(CustomerList, nameof(CustomerModel.Customer_Id), nameof(CustomerModel.Customer_Name), null, null);
                    }
                    
                    return View("EditForm", saleInfo);
                }
                return null;

            }

        }



        [HttpPost]
        public async Task<IActionResult> OnPostCreateOrEdit(int id, SaleMasterModel entity)
        {
            if (entity != null)
            {
                if (entity.Sale_Master_Id == 0)
                {
                    var createSale = await unitOfWork.Sales.AddAsync(entity);
                    id = entity.Sale_Master_Id;
                }

                else
                {
                    var sale = await unitOfWork.Sales.GetByMasterIdAsync(entity.Sale_Master_Id);
                    if (sale == null)
                    {
                        return null;
                    }
                    await unitOfWork.Sales.UpdateSaleAsync(entity.Sale_Master_Id, entity);
                }



                var saleList = await unitOfWork.Sales.GetAllAsync();

                if (saleList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_viewall", saleList);
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
                var html = await _viewRenderer.RenderViewToStringAsync("CreateForm", entity);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        [HttpPost]
        public async Task<JsonResult> loaddata(int Sale_Master_Id)
        {
            var loadMedicine = await unitOfWork.Sales.GetDetailListByMasterId(Sale_Master_Id);
            return Json(data: loadMedicine);
        }

        [HttpPost]
        public async Task<JsonResult> GetChildDelete(int detailId, int masterId)
        {
            var deleteChild = unitOfWork.Sales.DeleteChildAsync(detailId, masterId);
            if (deleteChild != null)
            {
                _notify.Error($"Sale with Id {detailId} Deleted.");
                var saleList = await unitOfWork.Sales.GetAllAsync();
                if (saleList != null)
                {
                    var html = await _viewRenderer.RenderViewToStringAsync("_viewall", saleList);
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
            var deleteMaster = await unitOfWork.Sales.DeleteMasterAsync(masterId);
            if (deleteMaster != 0)
            {
                _notify.Information($"Sale with Id {masterId} Deleted.");
            }
            else
            {
                _notify.Error("Failed To Delete");
                return null;
            }

            var saleList = await unitOfWork.Sales.GetAllAsync();
            var html = await _viewRenderer.RenderViewToStringAsync("_viewall", saleList);
            return new JsonResult(new { isValid = true, html = html });


        }


        [HttpGet]
        public async Task<IActionResult> SaleMasterData(int id)
        {
            var masterData = await unitOfWork.Sales.GetSaleMasterData(id);
            return Json(data: masterData);
        }

        [HttpGet]
        public async Task<ActionResult> GetSaleDetail(int id)
        {

            var CustomerList = await unitOfWork.Customers.GetAllAsync();

            var saleInfo = await unitOfWork.Sales.GetByMasterIdAsync(id);
            if (saleInfo != null)
            {
                if (CustomerList != null)
                {
                    saleInfo.Customers = new SelectList(CustomerList, nameof(CustomerModel.Customer_Id), nameof(CustomerModel.Customer_Name), null, null);

                }
                return View("SaleDetail", saleInfo);
            }
            return null;

        }

        

        public async Task<IActionResult> StockList(int id)
        {
            var result = await unitOfWork.Sales.GetStockList(id);
            return Json(data: result);

        }


        [HttpPost]
        public async Task<IActionResult> Print(int Sale_Master_Id, string Sale_By_Date, int Invoice_No, string Customer_Name, string Customer_Phone, string Payment_Type, string Grand_Total, string Discount, string Status)
        {
            string mimtype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Sale.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Invoice_No", Invoice_No.ToString());
            parameters.Add("Sale_By_Date", Sale_By_Date);
            parameters.Add("Customer_Name", Customer_Name);
            //parameters.Add("Customer_Phone", Customer_Phone);
            //parameters.Add("Customer_Email", Supplier_Email);
            parameters.Add("Discount", Discount);
            parameters.Add("Grand_Total", Grand_Total);
            parameters.Add("Pymt_Mode", Payment_Type);
            parameters.Add("Status", Status);
            var Invoices = await unitOfWork.Sales.GetSaleList(Sale_Master_Id);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet2", Invoices);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
            return File(result.MainStream, "application/pdf");
        }


        public async Task<IActionResult> Export(int Sale_Master_Id,string Sale_By_Date, int Invoice_No, string Customer_Name, string Customer_Phone, string Payment_Type, string Grand_Total, string Discount, string Status)
        {
            //var dt = new DataTable();
            
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Sale.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Invoice_No", Invoice_No.ToString());
            parameters.Add("Sale_By_Date", Sale_By_Date);
            parameters.Add("Customer_Name", Customer_Name);
            parameters.Add("Customer_Phone", Customer_Phone);
            //parameters.Add("Customer_Email", Supplier_Email);
            parameters.Add("Discount", Discount);
            parameters.Add("Grand_Total", Grand_Total);
            parameters.Add("Pymt_Mode", Payment_Type);
            parameters.Add("Status", Status);
            var Invoices = await unitOfWork.Sales.GetSaleList(Sale_Master_Id);
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource("DataSet2", Invoices);
            var result = lr.Execute(RenderType.Excel, extension, parameters, mimetype);
            return File(result.MainStream, "application/msexcel", "Export.xls");
        }


    }
}
