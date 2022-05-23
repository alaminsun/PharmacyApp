using Dapper;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Transaction.Models;
using PhramacyApp.Helpers;
using PhramacyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public SaleRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }
        //Task<PurchaseMasterModel> IGenericRepository<PurchaseMasterModel>.AddAsync(PurchaseMasterModel entity)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<SaleMasterModel> AddAsync(SaleMasterModel entity)
        {
            entity.CreatedOn = DateTime.Now.ToString("dd-MM-yyyy");
            int MaxID = _iDGenerated.getMAXSLFIN("Sale_Master_tbl", "Sale_Master_Id");
            entity.Sale_Master_Id = MaxID;
            //int MedicineId = _iDGenerated.getMAXSLFIN("MedicineInfo", "MedicineId");
            //entity.MedicineId = MedicineId;
            string queryMast = "INSERT INTO Sale_Master_tbl (Sale_Master_Id, Customer_Id, Selling_Date, Invoice_No, Details, Payment_Type, Status, Discount_Price, Grand_Total, CreatedOn) " +
                "VALUES (" + entity.Sale_Master_Id + ",'" + entity.Customer_Id + "','" + entity.Selling_Date + "', '" + entity.Invoice_No + "','" + entity.Details + "','" + entity.Payment_Type + "','" + entity.Status + "'," + entity.Discount_Price + "," + entity.Grand_Total + ",'" + entity.CreatedOn + "')";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(queryMast);
                //return medicineDTO;
            }
            foreach (var detailModel in entity.DetailList)
            {
                detailModel.Sale_Detail_Id = _iDGenerated.getMAXSLFIN("Sale_Detail_tbl", "Sale_Detail_Id");

                int stockQty = GetStockQty(detailModel.Medicine_Id);

                detailModel.Stock_Qty = stockQty - detailModel.Quantity;

                string stockInsert = "Update Purchase_Detail_tbl SET Stock_Qty = "+detailModel.Stock_Qty+" Where Medicine_Id='" + detailModel.Medicine_Id+"'";
                using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
                {
                    connection.Open();
                    await connection.ExecuteAsync(stockInsert);
                }

                string queryDetail = "INSERT INTO Sale_Detail_tbl(Sale_Master_Id,Sale_Detail_Id, Medicine_Id,Batch_No,Expiry_Date,Quantity,Selling_Price,Total_Price, CreatedOn) VALUES(" + entity.Sale_Master_Id + ", " + detailModel.Sale_Detail_Id + "," +
                    " '" + detailModel.Medicine_Id + "', '" + detailModel.Batch_No + "', '" + detailModel.Expiry_Date + "'," + detailModel.Quantity + ", " + detailModel.Selling_Price + ", " + detailModel.Total_Price + ", '" + entity.CreatedOn + "')";

                using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
                {
                    connection.Open();
                    await connection.ExecuteAsync(queryDetail);
                    //return medicineDTO;
                }
                //_dbHelper.CmdExecute(qry);
            }

            return entity;


        }

        public async Task<SaleMasterModel> UpdateSaleAsync(int id, SaleMasterModel entity)
        {
            entity.LastModifiedOn = DateTime.Now.ToString("dd-MM-yyyy");
            //int MaxID = _iDGenerated.getMAXSLFIN("Purchase_Master_tbl", "Purchase_Master_Id");
            //entity.Purchase_Master_Id = MaxID;
            //int MedicineId = _iDGenerated.getMAXSLFIN("MedicineInfo", "MedicineId");
            //entity.MedicineId = MedicineId;
            //string queryMast = "INSERT INTO Purchase_Master_tbl (Purchase_Master_Id, Supplier_Id, Purchase_Date, Invoice_No, Deatils, Payment_Type, Discount_Price, Grand_Total, CreatedOn) " +
            //"VALUES (" + entity.Purchase_Master_Id + ",'" + entity.Supplier_Id + "','" + entity.Purchase_Date + "', '" + entity.Invoice_No + "','" + entity.Details + "','" + entity.Payment_Type + "'," + entity.Discount_Price + "," + entity.Grand_Total + ",'" + entity.CreatedOn + "')";
            string queryMast = "UPDATE Sale_Master_tbl SET Customer_Id='" + entity.Customer_Id + "',Selling_Date='" + entity.Selling_Date + "', Invoice_No='" + entity.Invoice_No + "', Details='" + entity.Details + "', Payment_Type='" + entity.Payment_Type + "',Status='" + entity.Status + "', Discount_Price=" + entity.Discount_Price + ",Grand_Total=" + entity.Grand_Total + ",LastModifiedOn='" + entity.LastModifiedOn + "' Where Sale_Master_Id = " + entity.Sale_Master_Id + " ";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(queryMast);
                //return medicineDTO;
            }
            if (id > 0)
            {
                string queryDetail;
                foreach (var detailModel in entity.DetailList)
                {
                    if (detailModel.Sale_Detail_Id == 0)
                    {
                        detailModel.Sale_Detail_Id = _iDGenerated.getMAXSLFIN("Sale_Detail_tbl", "Sale_Detail_Id");

                        int stockQty = GetStockQty(detailModel.Medicine_Id);

                        detailModel.Stock_Qty = stockQty + detailModel.Quantity;

                        queryDetail = "INSERT INTO Sale_Detail_tbl(Sale_Master_Id,Sale_Detail_Id, Medicine_Id,Batch_No,Expiry_Date,Stock_Qty,Quantity,Buying_Price,Total_Price, CreatedOn) VALUES(" + entity.Sale_Master_Id + ", " + detailModel.Sale_Detail_Id + "," +
                           " '" + detailModel.Medicine_Id + "', '" + detailModel.Batch_No + "', '" + detailModel.Expiry_Date + "'," + detailModel.Stock_Qty + "," + detailModel.Quantity + ", " + detailModel.Buying_Price + ", " + detailModel.Total_Price + ", '" + entity.CreatedOn + "')";
                        using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
                        {
                            connection.Open();
                            await connection.ExecuteAsync(queryDetail);
                            //return result;
                        }
                    }
                    else
                    {

                        int stockQty = GetStockQty(detailModel.Medicine_Id);

                        detailModel.Stock_Qty = stockQty + detailModel.Quantity;

                        //queryDetail = "INSERT INTO Purchase_Detail_tbl(Purchase_Master_Id,Purchase_Detail_Id, Medicine_Id,Batch_No,Expiry_Date,Stock_Qty,Quantity,Buying_Price,Total_Price, CreatedOn) VALUES(" + entity.Purchase_Master_Id + ", " + detailModel.Purchase_Detail_Id + "," +
                        //   " '" + detailModel.Medicine_Id + "', '" + detailModel.Batch_No + "', '" + detailModel.Expiry_Date + "'," + detailModel.Stock_Qty + "," + detailModel.Quantity + ", " + detailModel.Buying_Price + ", " + detailModel.Total_Price + ", '" + entity.CreatedOn + "')";
                        queryDetail = "UPDATE Sale_Detail_tbl SET Expiry_Date='" + detailModel.Expiry_Date + "',  Quantity=" + detailModel.Quantity + ",Stock_Qty= " + detailModel.Stock_Qty + ",Total_Price=" + detailModel.Total_Price + ",LastModifiedOn= " + entity.LastModifiedOn + " Where Sale_Detail_Id = " + detailModel.Sale_Detail_Id + "";
                        using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
                        {
                            connection.Open();
                            await connection.ExecuteAsync(queryDetail);
                            //return result;
                        }
                    }



                    //_dbHelper.CmdExecute(qry);

                }
            }

            return entity;

        }

        //private int GetStockQty(string medicineId)
        //{
        //    int SL = 0;
        //    string query = "Select Stock_Qty from Purchase_Detail_tbl Where Medicine_Id='" + medicineId + "'";
        //    using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
        //    {
        //        connection.Open();
        //        DataTable dt = connection.QuerySingleOrDefault(query);
        //        if (dt.Rows.Count > 0)
        //        {
        //            SL = Convert.ToInt16(dt.Rows[0][0].ToString());
        //        }
        //    }
        //    return SL;
        //}

        public async Task<IEnumerable<SaleModel>> GetSaleList(int Sale_Master_Id)
        {
            var query = "Select * From VW_Sale_Invoice Where Sale_Id="+ Sale_Master_Id + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {

                connection.Open();
                var result = await connection.QueryAsync<SaleModel>(query);
                return result;
            }
        }
        public int GetStockQty(string medicineId)
        {
            int SL = 0;
            string query = "Select Stock_Qty from Purchase_Detail_tbl Where Medicine_Id='" + medicineId + "'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                DataTable dt = GetDataTable(query);
                //DataTable dt = connection.QuerySingleOrDefault(query);
                if (dt.Rows.Count > 0)
                {
                    SL = Convert.ToInt16(dt.Rows[0][0].ToString());
                }
            }

            return SL;
        }



        public DataTable GetDataTable(string Qry)
        {
            DataTable dt = new DataTable();
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {

                SqlCommand objCmd = new SqlCommand();
                objCmd.CommandText = Qry;
                objCmd.Connection = connection;
                connection.Open();
                objCmd.ExecuteNonQuery();
                using (SqlDataReader rdr = objCmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        dt.Load(rdr);
                    }
                }

            }

            return dt;
        }



        public async Task<int> DeleteChildAsync(int detailId, int masterId)
        {
            var query = "DELETE FROM Sale_Detail_tbl WHERE Sale_Detail_Id = " + detailId + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query);
                return result;
            }
            //updateGrandTotalAsync(masterId);
        }
        public async Task<int> DeleteMasterAsync(int masterId)
        {
            var query = "DELETE FROM Sale_Master_tbl WHERE Sale_Master_Id = " + masterId + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query);
                return result;
            }
            //updateGrandTotalAsync(masterId);
        }

        //private async Task updateGrandTotalAsync(int masterId)
        //{
        //    //queryDetail = "UPDATE Purchase_Detail_tbl SET Expiry_Date='" + detailModel.Expiry_Date + "',  Quantity=" + detailModel.Quantity + ",Stock_Qty= " + detailModel.Stock_Qty + ",Total_Price=" + detailModel.Total_Price + ",LastModifiedOn= " + entity.LastModifiedOn + " Where Purchase_Detail_Id = " + detailModel.Purchase_Detail_Id + "";
        //    string queryDetail = "UPDATE Purchase_Master_tbl SET Grand_Total='" +  + "' Where Purchase_Detail_Id = " + masterId + "";
        //    using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
        //    {
        //        connection.Open();
        //        await connection.ExecuteAsync(queryDetail);
        //        //return result;
        //    }
        //}

        public async Task<IReadOnlyList<SaleMasterModel>> GetAllAsync()
        {
            var query = "Select SM.Invoice_No,SM.Sale_Master_Id,ci.Customer_Name,SM.Selling_Date,Grand_Total,Payment_Type,Status From Sale_Master_tbl SM,CustomerInfo ci Where sm.Customer_Id = ci.Customer_Id";
            //var query = "Select PM.Purchase_Master_Id, PM.SupplierId, PM.Purchase_Date, PM.Invoice_No,PM.Deatils,PM.Payment_Type,PM.GrandTotal, " +
            //            "PM.DiscountPrice, PD.Medicine_Id, Pd.Batch_No, PD.Expiry_Date, Pd.Stock_Qty, PD.Quantity, PD.Quantity, PD.Buying_Price, PD.Total_Price " +
            //            "From Purchase_Master_tbl PM, Purchase_Detail_tbl PD";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SaleMasterModel>(query);
                return result.ToList();
            }
        }

        //public async Task<IEnumerable<PurchaseMasterModel>> GetByMasterIdAsync(int id, PurchaseMasterModel model)
        //{
        //    //var query = "SELECT m.*, c.CategoryName, s.ShelfName, si.SupplierName FROM MedicineInfo m, CategoryInfo c, ShelfInfo s, SupplierInfo si Where m.CategoryId = c.Id  And m.ShelfId = s.Id AND m.SupplierId = si.SupplierId AND m.Id = @Id";
        //    var query = "Select PM.Purchase_Master_Id, PM.SupplierId, PM.Purchase_Date, PM.Invoice_No,PM.Deatils,PM.Payment_Type,PM.GrandTotal, " +
        //                "PM.DiscountPrice, PD.Medicine_Id, Pd.Batch_No, PD.Expiry_Date, Pd.Stock_Qty, PD.Quantity, PD.Quantity, PD.Buying_Price, PD.Total_Price " +
        //                "From Purchase_Master_tbl PM, Purchase_Detail_tbl PD Where PM.Purchase_Master_Id = " + id + "";
        //    using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<PurchaseMasterModel>(query);
        //        return result;
        //    }
        //}




        public async Task<IEnumerable<SaleMasterModel>> GetAllMedicine()
        {
            var query = " SELECT * FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.CategoryId = c.Id " +
                        " And m.ShelfId = s.Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SaleMasterModel>(query);
                return result.ToList();
            }
        }

        public async Task<IEnumerable<SaleModel>> GetMedicineInfo(string medicine)
        {
            //var query = " SELECT * FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.CategoryId = c.Id " +
            //            " And m.ShelfId = s.Id AND m.MedicineName Like '" + medicine + "%'";
            //var query = " SELECT m.Id,m.Medicine_Id,m.Medicine_Name,m.Category_Id,c.Category_Name,m.Shelf_Id,s.Shelf_Name,s.Shelf_Number,m.Buying_Price,m.Selling_Price,m.Expiry_Date " +
            //            " FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.Category_Id = c.Id  And m.Shelf_Id = s.Id AND m.Medicine_Name Like '" + medicine + "%'";
            var query = " SELECT Top(100) m.Id,m.Medicine_Id,m.Medicine_Name,m.Category_Id,c.Category_Name,m.Shelf_Id,s.Shelf_Name,s.Shelf_Number,m.Buying_Price,m.Selling_Price,m.Expiry_Date " +
                        " FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.Category_Id = c.Id  And m.Shelf_Id = s.Id AND m.Medicine_Name Like '" + medicine + "%'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SaleModel>(query);
                return result;
            }
        }

        public async Task<SaleModel> GetMedicineId(string medicineId)
        {
           
            //var query = "SELECT Id, Medicine_Id, Medicine_Name, Selling_Price FROM MedicineInfo Where Medicine_Id = '"+ medicineId + "'";
            var query = " SELECT distinct m.Id, m.Medicine_Id, m.Medicine_Name, pd.Stock_Qty, m.Selling_Price FROM MedicineInfo m, Purchase_Detail_tbl pd "+
                        " Where pd.Medicine_Id = m.Medicine_Id AND m.Medicine_Id = '"+ medicineId + "'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleAsync<SaleModel>(query);
                return result;
            }
        }

        //public async Task<PurchaseMasterModel> GetByIdAsync(int id)
        //{
        //    var query = "Select PM.Invoice_No,PM.Purchase_Master_Id, s.SupplierName,PM.Purchase_Date,GrandTotal From Purchase_Master_tbl PM left join SupplierInfo s on PM.SupplierId = s.Id Where PM.Purchase_Master_Id = "+id+"";
        //    //var query = "Select PM.Purchase_Master_Id, PM.SupplierId, PM.Purchase_Date, PM.Invoice_No,PM.Deatils,PM.Payment_Type,PM.GrandTotal, " +
        //    //            "PM.DiscountPrice, PD.Medicine_Id, Pd.Batch_No, PD.Expiry_Date, Pd.Stock_Qty, PD.Quantity, PD.Quantity, PD.Buying_Price, PD.Total_Price " +
        //    //            "From Purchase_Master_tbl PM, Purchase_Detail_tbl PD";
        //    using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
        //    {
        //        connection.Open();
        //        var result = await connection.QuerySingleOrDefaultAsync<PurchaseMasterModel>(query);
        //        return result;
        //    }
        //}

        public async Task<SaleMasterModel> GetMasterInfoAsync(int id)
        {
            var query = "Select SM.Invoice_No,SM.Sale_Master_Id, SM.Customer_Id, s.Customer_Name,SM.Selling_Date,SM.Payment_Type,SM.Status,SM.Details,SM.Discount_Price,SM.Grand_Total From Sale_Master_tbl SM left join CustomerInfo s on SM.Customer_Id = s.Id Where SM.Sale_Master_Id = " + id + "";
            //var query = "Select PM.Purchase_Master_Id, PM.SupplierId, PM.Purchase_Date, PM.Invoice_No,PM.Deatils,PM.Payment_Type,PM.GrandTotal, " +
            //            "PM.DiscountPrice, PD.Medicine_Id, Pd.Batch_No, PD.Expiry_Date, Pd.Stock_Qty, PD.Quantity, PD.Quantity, PD.Buying_Price, PD.Total_Price " +
            //            "From Purchase_Master_tbl PM, Purchase_Detail_tbl PD";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<SaleMasterModel>(query);
                return result;
            }
        }


        public async Task<SaleMasterModel> GetByMasterIdAsync(int id)
        {
            var masterInfo = await GetMasterInfoAsync(id);


            //if (!master.Eq null)
            //{
            //    return new List<SubMenuViewModel>();
            //}

            //var vmList = new PurchaseMasterModel();
            var vm = new SaleMasterModel();
            //foreach (var item in master)
            //{
            //    var menu = await GetMenuItemAsync(menuList, item.Mh_Id, ctx);
            //    var vm = new SubMenuViewModel();
            //    vm.Mh_Id = menu.Mh_Id;
            //    vm.Mh_Name = menu.Mh_Name;
            //    vm.Area = menu.Area;
            //    vm.Children = await GetChildrenMenuAsync(subMenuList, item.Mh_Id, ctx);
            //    vmList.Add(vm);
            //}
            if (masterInfo.Sale_Master_Id > 0)
            {
                var master = await GetMasterInfoAsync(id);
                vm.Sale_Master_Id = master.Sale_Master_Id;
                vm.Customer_Id = master.Customer_Id;
                vm.Customer_Name = master.Customer_Name;
                vm.Selling_Date = master.Selling_Date;
                vm.Invoice_No = master.Invoice_No;
                vm.Payment_Type = master.Payment_Type;
                vm.Status = master.Status;
                vm.Details = master.Details;
                vm.Discount_Price = master.Discount_Price;
                vm.Grand_Total = master.Grand_Total;
                vm.DetailList = await GetDetailListByMasterId(masterInfo.Sale_Master_Id);
                //vmList.Add(vm);
            }
            return vm;
        }

        public async Task<List<SaleDetailModel>> GetDetailListByMasterId(int sale_Master_Id)
        {
            var query = " Select SD.Sale_Master_Id, SD.Sale_Detail_Id, SD.Medicine_Id, m.Medicine_Name, SD.Batch_No, SD.Expiry_Date, "+
                        " SD.Stock_Qty, SD.Quantity, SD.Selling_Price, SD.Total_Price From Sale_Detail_tbl SD Left Join MedicineInfo m on m.Medicine_Id = SD.Medicine_Id Where SD.Sale_Master_Id = " + sale_Master_Id + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                List<SaleDetailModel> items = new List<SaleDetailModel>();
                //var model = new PurchaseDetailModel();
                connection.Open();
                var result = await connection.QueryAsync<SaleDetailModel>(query);
                var length = result.Count();
                int Sl = 1;
                if (length > 0)
                {
                    foreach (var item in result)
                    {
                        var model = new SaleDetailModel();
                        model.Sl = Sl;
                        model.Medicine_Id = item.Medicine_Id;
                        model.Medicine_Name = item.Medicine_Name;
                        model.Batch_No = item.Batch_No;
                        model.Buying_Price = item.Buying_Price;
                        model.Expiry_Date = item.Expiry_Date;
                        model.Selling_Price = item.Selling_Price;
                        model.Stock_Qty = item.Stock_Qty;
                        model.Total_Price = item.Total_Price;
                        model.Quantity = item.Quantity;
                        model.Sale_Detail_Id = item.Sale_Detail_Id;
                        model.Sale_Master_Id = item.Sale_Master_Id;
                        Sl++;
                        items.Add(model);
                    }

                }

                return items;
            }
        }


        public async Task<IEnumerable<SaleModel>> GetSaleMasterData(int id)
        {
            //var query = "Select Purchase_Master_Id,Supplier_Id,Purchase_Date,Invoice_No,Payment_Type,Discount_Price,Grand_Total From Purchase_Master_tbl Where Purchase_Master_Id = "+id+"";
            //var query = "Select a.Purchase_Master_Id,a.Supplier_Id, b.Supplier_Name ,a.Purchase_Date,a.Invoice_No,a.Details,a.Payment_Type,a.Discount_Price,a.Grand_Total " +
            //            " From Purchase_Master_tbl a,SupplierInfo b  Where a.Supplier_Id = b.Supplier_Id And a.Purchase_Master_Id = " + id + "";
            var query = " Select a.Sale_Master_Id,a.Customer_Id, b.Customer_Name ,a.Selling_Date Sale_By_Date,a.Invoice_No,a.Details,a.Payment_Type,a.Status,a.Discount_Price,a.Grand_Total From Sale_Master_tbl a,CustomerInfo b " +
                        " Where a.Customer_Id = b.Customer_Id And a.Sale_Master_Id = " + id + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {

                connection.Open();
                var result = await connection.QueryAsync<SaleModel>(query);
                return result;
            }
        }

        public async Task<List<SaleDetailModel>> GetStockList(int id)
        {
            //string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            //    var query = "Select pd.Medicine_Id, m.Medicine_Name,pd.Batch_No,pd.Quantity,pd.Buying_Price, m.Selling_Price,m.Expiry_Date, pd.Total_Price " +
            //" from Purchase_Detail_tbl pd, MedicineInfo m " +
            //" where m.Medicine_Id = pd.Medicine_Id And pd.Purchase_Master_Id=" + id + " Order By pd.Medicine_Id ";
            var query = " Select sd.Medicine_Id, m.Medicine_Name,sd.Quantity,sd.Selling_Price, sd.Total_Price from Sale_Detail_tbl sd, MedicineInfo m " +
                        " where m.Medicine_Id = sd.Medicine_Id And sd.Sale_Master_Id = " + id + " Order By sd.Medicine_Id ";
            //"and Convert(date,m.expiry_date,103) < Convert(date,'" + currentDate + "',103)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                List<SaleDetailModel> items = new List<SaleDetailModel>();
                //var model = new PurchaseDetailModel();
                connection.Open();
                var result = await connection.QueryAsync<SaleDetailModel>(query);
                var length = result.Count();
                int Sl = 1;
                if (length > 0)
                {
                    foreach (var item in result)
                    {
                        var model = new SaleDetailModel();
                        model.Sl = Sl;
                        model.Medicine_Id = item.Medicine_Id;
                        model.Medicine_Name = item.Medicine_Name;
                        //model.Batch_No = item.Batch_No;
                        model.Quantity = item.Quantity;
                        //model.Buying_Price = item.Buying_Price;
                        model.Selling_Price = item.Selling_Price;
                        //model.Expiry_Date = item.Expiry_Date;

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

        //Task<SaleMasterModel> ISaleRepository.UpdateSaleAsync(int id, SaleMasterModel entity)
        //{
        //    throw new NotImplementedException();
        //}

        Task<SaleMasterModel> IGenericRepository<SaleMasterModel>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<int> IGenericRepository<SaleMasterModel>.UpdateAsync(int id, SaleMasterModel entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IGenericRepository<SaleMasterModel>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
