using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Transaction.Models;
using PhramacyApp.DAL.Gateway;
using PhramacyApp.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Report.Controllers
{
    [Area("Report")]
    public class PurchaseReportController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        private readonly DBHelper _dBHelper;
        public PurchaseReportController(IConfiguration configuration, IDGenerated iDGenerated, DBHelper dBHelper)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
            _dBHelper = dBHelper;

        }
        public IActionResult Index()
        {
            return View();
        }

        private async Task<List<PurchaseModel>> GetStockList()
        {
            //string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            var query = "Select pd.Medicine_Id, m.Medicine_Name,pd.Batch_No,pd.Quantity,pd.Buying_Price, m.Selling_Price,m.Expiry_Date " +
        " from Purchase_Detail_tbl pd, MedicineInfo m " +
        " where m.Medicine_Id = pd.Medicine_Id Order By pd.Medicine_Id ";
            //"and Convert(date,m.expiry_date,103) < Convert(date,'" + currentDate + "',103)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                List<PurchaseModel> items = new List<PurchaseModel>();
                //var model = new PurchaseDetailModel();
                connection.Open();
                var result = await connection.QueryAsync<PurchaseModel>(query);
                var length = result.Count();
                //int Sl = 1;
                if (length > 0)
                {
                    foreach (var item in result)
                    {
                        var model = new PurchaseModel();
                        //model.Sl = Sl;
                        model.Medicine_Id = item.Medicine_Id;
                        model.Medicine_Name = item.Medicine_Name;
                        model.Batch_No = item.Batch_No;
                        model.Quantity = item.Quantity;
                        model.Buying_Price = item.Buying_Price;
                        model.Selling_Price = item.Selling_Price;
                        model.Expiry_Date = item.Expiry_Date;

                        //model.Total_Price = item.Total_Price;
                        //model.Quantity = item.Quantity;
                        //model.Purchase_Detail_Id = item.Purchase_Detail_Id;
                        //model.Purchase_Master_Id = item.Purchase_Master_Id;
                        //Sl++;
                        items.Add(model);
                    }

                }

                return items;
            }
        }

        public async Task<IActionResult> StockList(string type)
        {
            var result = await GetStockList();
            //return PartialView("_viewall", PurchaseList);
            //return PartialView("_viewAll", result);
            return Json(data: result);

        }

    }
}
