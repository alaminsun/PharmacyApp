using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Dashboard.Models;
using PhramacyApp.Areas.Transaction.Models;
using PhramacyApp.Controllers;
using PhramacyApp.DAL.Gateway;
using PhramacyApp.Helpers;
using PhramacyApp.Interfaces;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Report.Controllers
{
    [Area("Report")]
    public class ReportController : BaseController<ReportController>
    {
        //private readonly IReportService _reportService;
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        private readonly DBHelper _dBHelper;
        private readonly IWebHostEnvironment _oHostEnvironment;
        public ReportController(IConfiguration configuration, IDGenerated iDGenerated, DBHelper dBHelper, IWebHostEnvironment oHostEnvironment)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
            _dBHelper = dBHelper;
            _oHostEnvironment = oHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult CreateDocument()
        //{
        //    //Create a new PDF document.
        //    PdfDocument doc = new PdfDocument();
        //    //Add a page.
        //    PdfPage page = doc.Pages.Add();
        //    //Create a PdfGrid.
        //    PdfGrid pdfGrid = new PdfGrid();
        //    //Add values to list
        //    List<object> data = new List<object>();
        //    Object row1 = new { ID = "E01", Name = "Clay" };
        //    Object row2 = new { ID = "E02", Name = "Thomas" };
        //    Object row3 = new { ID = "E03", Name = "Andrew" };
        //    Object row4 = new { ID = "E04", Name = "Paul" };
        //    Object row5 = new { ID = "E05", Name = "Gray" };
        //    data.Add(row1);
        //    data.Add(row2);
        //    data.Add(row3);
        //    data.Add(row4);
        //    data.Add(row5);
        //    //Add list to IEnumerable
        //    IEnumerable<object> dataTable = data;
        //    //Assign data source.
        //    pdfGrid.DataSource = dataTable;
        //    //Draw grid to the page of PDF document.
        //    pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
        //    //Save the PDF document to stream
        //    MemoryStream stream = new MemoryStream();
        //    doc.Save(stream);
        //    //If the position is not set to '0' then the PDF will be empty.
        //    stream.Position = 0;
        //    //Close the document.
        //    doc.Close(true);
        //    //Defining the ContentType for pdf file.
        //    string contentType = "application/pdf";
        //    //Define the file name.
        //    string fileName = "Output.pdf";
        //    //Creates a FileContentResult object by using the file contents, content type, and file name.
        //    return File(stream, contentType, fileName);
        //}

        ////[HttpGet]
        ////public IActionResult Get()
        ////{
        ////    var pdfFile = _reportService.GeneratePdfReport();
        ////    return File(pdfFile,"application/octet-stream", "SimplePdf.pdf");
        ////}
        ///

        //public ActionResult PrintInvoice(int param)
        //{
        //    List<PurchaseModel> oStudents = new List<PurchaseModel>();
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        PurchaseModel purchaseModel = new PurchaseModel();
        //        purchaseModel.Id = i;
        //        purchaseModel.Name = "Student" + i;
        //        purchaseModel.Address = "Address" + i;
        //        //oStudent.Add(oStudent);

        //    }

        //    StudentReport rpt = new StudentReport(_oHostEnvironment);
        //    return File(rpt.Report(oStudents), "application/pdf");
        //}

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

                InvoiceReport rpt = new InvoiceReport(_oHostEnvironment);
                return File(rpt.Report(items), "application/pdf");

                //return items;
            }
        }


    }
}
