using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Dashboard.Models;
using PhramacyApp.Controllers;
using PhramacyApp.DAL.Gateway;
using PhramacyApp.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : BaseController<HomeController>
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        private readonly DBHelper _dBHelper;
        public HomeController(IConfiguration configuration, IDGenerated iDGenerated, DBHelper dBHelper)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
            _dBHelper = dBHelper;

        }
        public async Task<IActionResult> Index()
        {
            _notify.Information("Hi There!");
            DashboardViewModel dashboard = new DashboardViewModel();

            dashboard.self_count = GetSelfCount();
            dashboard.medicine_count = GetTotalMedicine();
            dashboard.stock_count = GetTotalStock();
            dashboard.supplier_count = GetTotalSupplier();
            dashboard.purchase_count = GetTotalPurchase();
            dashboard.purchase_amount = GetTotalPurchaseAmount();
            dashboard.sale_count = GetTotalSale();
            dashboard.sale_amount = GetTotalSaleAmount();
            dashboard.lowStocks = await GetLowtockMedicine();
            return View(dashboard);
            //return View();
        }

        private async Task<List<DashboardViewModel>> GetLowtockMedicine()
        {
            var query = "Select pd.Medicine_Id, m.Medicine_Name,pd.Batch_No,pd.Quantity,pd.Buying_Price from Purchase_Detail_tbl pd, MedicineInfo m "+
                    " where m.Medicine_Id = pd.Medicine_Id And pd.Quantity <= 5";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                List<DashboardViewModel> items = new List<DashboardViewModel>();
                //var model = new PurchaseDetailModel();
                connection.Open();
                var result = await connection.QueryAsync<DashboardViewModel>(query);
                var length = result.Count();
                int Sl = 1;
                if (length > 0)
                {
                    foreach (var item in result)
                    {
                        var model = new DashboardViewModel();
                        model.Sl = Sl;
                        model.Medicine_Id = item.Medicine_Id;
                        model.Medicine_Name = item.Medicine_Name;
                        model.Batch_No = item.Batch_No;
                        model.Buying_Price = item.Buying_Price;
                        //model.Expiry_Date = item.Expiry_Date;
                        //model.Selling_Price = item.Selling_Price;
                        model.Quantity = item.Quantity;
                        //model.Total_Price = item.Total_Price;
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

        private int GetSelfCount()
        {
            int selfNo = 0;
            string query = "SELECT COUNT(*) FROM ShelfInfo";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    selfNo = Convert.ToInt16(dt.Rows[0][0].ToString());
                }
            }

            return selfNo;
        }

        private int GetTotalMedicine()
        {
            int medicineNo = 0;
            string query = "SELECT COUNT(*) FROM MedicineInfo";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    medicineNo = Convert.ToInt16(dt.Rows[0][0].ToString());
                }
            }

            return medicineNo;
        }
        private int GetTotalStock()
        {
            int selfNo = 0;
            string query = "SELECT ISNULL(sum(Quantity),0) FROM Purchase_Detail_tbl";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    selfNo = Convert.ToInt16(dt.Rows[0][0].ToString());
                }
            }

            return selfNo;
        }
        private int GetTotalSupplier()
        {
            int supplierCount = 0;
            string query = "SELECT COUNT(*) FROM SupplierInfo";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    supplierCount = Convert.ToInt16(dt.Rows[0][0].ToString());
                }
            }

            return supplierCount;
        }
        private int GetTotalPurchase()
        {
            int purchaseCount = 0;
            string query = "SELECT Count(Purchase_Master_Id) FROM Purchase_Master_tbl";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    purchaseCount = Convert.ToInt16(dt.Rows[0][0].ToString());
                }
            }

            return purchaseCount;
        }
        private decimal GetTotalPurchaseAmount()
        {
            decimal purchaseAmount = 0;
            string query = "SELECT ISNULL(sum(Grand_Total),0) FROM Purchase_Master_tbl";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    purchaseAmount = Convert.ToDecimal(dt.Rows[0][0].ToString());
                }
            }

            return purchaseAmount;
        }
        private int GetTotalSale()
        {
            int purchaseCount = 0;
            string query = "SELECT Count(Sale_Master_Id) FROM Sale_Master_tbl";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    purchaseCount = Convert.ToInt16(dt.Rows[0][0].ToString());
                }
            }

            return purchaseCount;
        }
        private decimal GetTotalSaleAmount()
        {
            decimal saleAmount = 0;
            string query = "SELECT ISNULL(sum(Grand_Total),0) FROM Sale_Master_tbl";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = _dBHelper.GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    saleAmount = Convert.ToDecimal(dt.Rows[0][0].ToString());
                }
            }

            return saleAmount;
        }
        private async Task<List<DashboardViewModel>> GetExpiryProductList()
        {
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            var query = "Select pd.Medicine_Id, m.Medicine_Name,pd.Batch_No,pd.Quantity,pd.Buying_Price, m.Selling_Price,m.Expiry_Date "+
        " from Purchase_Detail_tbl pd, MedicineInfo m "+
        " where m.Medicine_Id = pd.Medicine_Id and Convert(date,m.expiry_date,103) < Convert(date,'" + currentDate + "',103)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                List<DashboardViewModel> items = new List<DashboardViewModel>();
                //var model = new PurchaseDetailModel();
                connection.Open();
                var result = await connection.QueryAsync<DashboardViewModel>(query);
                var length = result.Count();
                int Sl = 1;
                if (length > 0)
                {
                    foreach (var item in result)
                    {
                        var model = new DashboardViewModel();
                        model.Sl = Sl;
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
                        Sl++;
                        items.Add(model);
                    }

                }

                return items;
            }
        }

        public async Task<IActionResult> GetDetails(string type)
        {
            var result = await GetExpiryProductList();
            //return PartialView("_viewall", PurchaseList);
            //return PartialView("_viewAll", result);
            return Json(data: result);

        }
        //public List<ProductOrCustomerViewModel> GetProductOrCustomer(string type)
        //{
        //    List<ProductOrCustomerViewModel> result = null;

        //    using (DashboardContext _context = new DataAccess.DashboardContext())
        //    {
        //        if (!string.IsNullOrEmpty(type))
        //        {
        //            if (type == "customers")
        //            {
        //                result = _context.CustomerSet.Select(c => new ProductOrCustomerViewModel
        //                {
        //                    Name = c.CustomerName,
        //                    Image = c.CustomerImage,
        //                    TypeOrCountry = c.CustomerCountry,
        //                    Type = "Customers"

        //                }).ToList();

        //            }
        //            else if (type == "products")
        //            {
        //                result = _context.ProductSet.Select(p => new ProductOrCustomerViewModel
        //                {
        //                    Name = p.ProductName,
        //                    Image = p.ProductImage,
        //                    TypeOrCountry = p.ProductType,
        //                    Type = p.ProductType

        //                }).ToList();

        //            }
        //        }

        //        return result;
        //    }

        //}



        //public int GetStockQty(string medicineId)
        //{
        //    int SL = 0;
        //    string query = "Select Stock_Qty from Purchase_Detail_tbl Where Medicine_Id='" + medicineId + "'";
        //    using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
        //    {
        //        connection.Open();
        //        DataTable dt = GetDataTable(query);
        //        //DataTable dt = connection.QuerySingleOrDefault(query);
        //        if (dt.Rows.Count > 0)
        //        {
        //            SL = Convert.ToInt16(dt.Rows[0][0].ToString());
        //        }
        //    }

        //    return SL;
        //}





        //public ActionResult GetMenuList()
        //{
        //    try
        //    {
        //        var result = (from m in objEntity.Menu_Tree_Structure
        //                      select new Dynamic_Menu.Models.Menu_List
        //                      {
        //                          M_CODE = m.M_CODE,
        //                          M_PARENT_CODE = m.M_PARENT_CODE,
        //                          M_NAME = m.M_NAME,
        //                          CONTROLLER_NAME = m.Controller_Name,
        //                          ACTION_NAME = m.Action_Name,
        //                      }).ToList();
        //        return View("Menu", result);
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = ex.Message.ToString();
        //        return Content("Error");
        //    }
        //}

        //[HttpGet]
        //public async Task<string> CreateMenu()
        //{
        //    const string UserMenu = "_UserMenu";
        //    HttpContext.Session.SetString(UserMenu, "");
        //    try
        //    {
        //        if (HttpContext.Session.GetString(UserMenu).Length == 0)
        //        {
        //            string htmlMenu = "";

        //            //int empId = Convert.ToInt32(Session["USER_ID"].ToString());
        //            //string roleName = Session["ROLE_NAME"].ToString();
        //            var userName = Input.Email;
        //            var user =  await _userManager.FindByNameAsync(userName);
        //            string userId = user.Id;
        //            //int empId = 1;
        //            string MHQry =
        //                "SELECT DISTINCT SMH.MH_NAME,SMH.MH_ID,SMH.MH_SEQ,SMH.MH_CSS_CLASS,SMC.RL_ID FROM SA_ROLE_CONF SRC,SA_ROLE SR,SA_MENU_CONF SMC,SA_MENU_HEAD SMH"
        //                + " WHERE SRC.ROLE_ID = SR.ROLE_ID AND SMC.RL_ID = SRC.ROLE_ID AND SMH.MH_ID = SMC.MH_ID AND SRC.USER_ID=" +
        //                userId + "  ORDER BY SMH.MH_SEQ ASC";
        //            //DataTable MHdt = _dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), MHQry);
        //            //var MHdt = await connection.QueryAsync<MenuViewModel>(MHQry)
        //            List<MenuViewModel> mhList;
        //            using (var connection = _dbContext.Connection)
        //            {
        //                var menuHeads = await connection.QueryAsync<MenuViewModel>(MHQry);
        //                menuHeads.ToList();
        //            }


        //            //if (roleName != "FSM" && roleName != "AM" && roleName != "Requisition")
        //            ////if (roleName != "FSM")
        //            //{
        //            //    //    htmlMenu = htmlMenu + " <a href = '/FSM/DashboardNationalPrescription/frmDashboardNationalPrescription' >";
        //            //    //}
        //            //    //else
        //            //    //{
        //            //    htmlMenu = htmlMenu + " <li class=''>";
        //            //    htmlMenu = htmlMenu + " <a href = '/Dashboard/HomeDashboard/frmHomeDashboard' >";
        //            //    htmlMenu = htmlMenu + " <i class='fa fa-th'></i> <span>Home</span>";
        //            //    //htmlMenu = htmlMenu + " <span class='pull -right-container' > ";
        //            //    //htmlMenu = htmlMenu + " <i class='fa fa-angle-left pull-right'></i>";
        //            //    //htmlMenu = htmlMenu + " </span>";
        //            //    htmlMenu = htmlMenu + " </a></li>";
        //            //}

        //            foreach (var u in mhList)
        //            {
        //                htmlMenu = htmlMenu + " <li class='treeview'>";
        //                htmlMenu = htmlMenu + " <a href = '#' >";
        //                htmlMenu = htmlMenu + " <i class='fa fa-th'></i> <span>" + u.MH_NAME + "</span>";
        //                htmlMenu = htmlMenu + " <span class='pull-right-container' >";
        //                htmlMenu = htmlMenu + " <i class='fa fa-angle-left pull-right'></i>";
        //                htmlMenu = htmlMenu + " </span>";
        //                htmlMenu = htmlMenu + " </a>";
        //                string SMQry =
        //                    "SELECT DISTINCT SSM.SM_NAME,SSM.SM_ID,SSM.SM_SEQ,SSM.SM_CSS_CLASS,SSM.URL FROM SA_ROLE_CONF SRC,SA_ROLE SR,SA_MENU_CONF SMC,SA_SUB_MENU SSM "
        //                    + " WHERE SRC.ROLE_ID = SR.ROLE_ID AND SMC.RL_ID = SRC.ROLE_ID AND SSM.SM_ID = SMC.SM_ID AND SMC.MH_ID=" +
        //                    u.MH_ID + " AND SMC.RL_ID=" + u.RL_ID + "ORDER BY SSM.SM_SEQ ASC";
        //                DataTable SMdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), SMQry);
        //                List<SubMenu> smList;
        //                smList = (from DataRow row in SMdt.Rows
        //                          select new SubMenu
        //                          {
        //                              SM_ID = Convert.ToInt32(row["SM_ID"].ToString()),
        //                              SM_NAME = row["SM_NAME"].ToString(),
        //                              SM_SEQ = row["SM_SEQ"].ToString(),
        //                              SM_CSS_CLASS = row["SM_CSS_CLASS"].ToString(),
        //                              URL = row["URL"].ToString()

        //                          }).ToList();
        //                if (smList.Any())
        //                {
        //                    htmlMenu = htmlMenu + " <ul class='treeview-menu' >";
        //                    foreach (var v in smList)
        //                    {

        //                        htmlMenu = htmlMenu + " <li class=''><a href = '" + v.URL +
        //                                   "' ><i class='fa fa-circle-o'></i>  " + v.SM_NAME + "</a></li>";
        //                        //htmlMenu = htmlMenu + " <li><a href = 'index2.html'><i class='fa fa-circle-o'></i> Dashboard v2</a></li>";

        //                    }

        //                    htmlMenu = htmlMenu + " </ul>";
        //                }
        //            }
        //            htmlMenu = htmlMenu + " </li>";

        //            Session["UserMenu"] = htmlMenu;
        //        }

        //        return Session["UserMenu"].ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }

        //}



    }
}