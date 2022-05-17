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
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public PurchaseRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }
        //Task<PurchaseMasterModel> IGenericRepository<PurchaseMasterModel>.AddAsync(PurchaseMasterModel entity)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<PurchaseMasterModel> AddAsync(PurchaseMasterModel entity)
        {
            entity.CreatedOn = DateTime.Now.ToString("dd-MM-yyyy");
            int MaxID = _iDGenerated.getMAXSLFIN("Purchase_Master_tbl", "Purchase_Master_Id");
            entity.Purchase_Master_Id = MaxID;
            //int MedicineId = _iDGenerated.getMAXSLFIN("MedicineInfo", "MedicineId");
            //entity.MedicineId = MedicineId;
            string queryMast = "INSERT INTO Purchase_Master_tbl (Purchase_Master_Id, Supplier_Id, Purchase_Date, Invoice_No, Details, Payment_Type, Status, Discount_Price, Grand_Total, CreatedOn) " +
                "VALUES ("+entity.Purchase_Master_Id+",'"+entity.Supplier_Id+"','"+entity.Purchase_Date+"', '"+entity.Invoice_No+"','"+entity.Details+"','"+entity.Payment_Type+ "','" + entity.Status + "'," + entity.Discount_Price+ "," + entity.Grand_Total + ",'" + entity.CreatedOn+"')";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(queryMast);
                //return medicineDTO;
            }
            foreach (var detailModel in entity.DetailList)
            {
                detailModel.Purchase_Detail_Id = _iDGenerated.getMAXSLFIN("Purchase_Detail_tbl", "Purchase_Detail_Id");

                int stockQty = GetStockQty(detailModel.Medicine_Id);

                detailModel.Stock_Qty = stockQty + detailModel.Quantity; 

                string queryDetail = "INSERT INTO Purchase_Detail_tbl(Purchase_Master_Id,Purchase_Detail_Id, Medicine_Id,Batch_No,Expiry_Date,Stock_Qty,Quantity,Buying_Price,Total_Price, CreatedOn) VALUES(" + entity.Purchase_Master_Id + ", " + detailModel.Purchase_Detail_Id + "," +
                    " '" + detailModel.Medicine_Id + "', '" + detailModel.Batch_No + "', '" + detailModel.Expiry_Date + "',"+detailModel.Stock_Qty+ ","+ detailModel.Quantity + ", "+detailModel.Buying_Price+", "+detailModel.Total_Price+ ", '" + entity.CreatedOn + "')";

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

        public async Task<PurchaseMasterModel> UpdatePurchaseAsync(int id, PurchaseMasterModel entity)
        {
            entity.LastModifiedOn = DateTime.Now.ToString("dd-MM-yyyy");
            //int MaxID = _iDGenerated.getMAXSLFIN("Purchase_Master_tbl", "Purchase_Master_Id");
            //entity.Purchase_Master_Id = MaxID;
            //int MedicineId = _iDGenerated.getMAXSLFIN("MedicineInfo", "MedicineId");
            //entity.MedicineId = MedicineId;
            //string queryMast = "INSERT INTO Purchase_Master_tbl (Purchase_Master_Id, Supplier_Id, Purchase_Date, Invoice_No, Deatils, Payment_Type, Discount_Price, Grand_Total, CreatedOn) " +
            //"VALUES (" + entity.Purchase_Master_Id + ",'" + entity.Supplier_Id + "','" + entity.Purchase_Date + "', '" + entity.Invoice_No + "','" + entity.Details + "','" + entity.Payment_Type + "'," + entity.Discount_Price + "," + entity.Grand_Total + ",'" + entity.CreatedOn + "')";
            string queryMast = "UPDATE Purchase_Master_tbl SET Supplier_Id='" + entity.Supplier_Id + "',Purchase_Date='" + entity.Purchase_Date + "', Invoice_No='" + entity.Invoice_No + "', Details='" + entity.Details + "', Payment_Type='" + entity.Payment_Type + "',Status='" + entity.Status + "', Discount_Price=" + entity.Discount_Price + ",Grand_Total=" + entity.Grand_Total + ",LastModifiedOn='" + entity.LastModifiedOn + "' Where Purchase_Master_Id = "+entity.Purchase_Master_Id+" ";
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
                    if (detailModel.Purchase_Detail_Id == 0)
                    {
                        detailModel.Purchase_Detail_Id = _iDGenerated.getMAXSLFIN("Purchase_Detail_tbl", "Purchase_Detail_Id");

                        int stockQty = GetStockQty(detailModel.Medicine_Id);

                        detailModel.Stock_Qty = stockQty + detailModel.Quantity;

                         queryDetail = "INSERT INTO Purchase_Detail_tbl(Purchase_Master_Id,Purchase_Detail_Id, Medicine_Id,Batch_No,Expiry_Date,Stock_Qty,Quantity,Buying_Price,Total_Price, CreatedOn) VALUES(" + entity.Purchase_Master_Id + ", " + detailModel.Purchase_Detail_Id + "," +
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

                        //detailModel.Stock_Qty = stockQty + detailModel.Quantity;

                        //queryDetail = "INSERT INTO Purchase_Detail_tbl(Purchase_Master_Id,Purchase_Detail_Id, Medicine_Id,Batch_No,Expiry_Date,Stock_Qty,Quantity,Buying_Price,Total_Price, CreatedOn) VALUES(" + entity.Purchase_Master_Id + ", " + detailModel.Purchase_Detail_Id + "," +
                        //   " '" + detailModel.Medicine_Id + "', '" + detailModel.Batch_No + "', '" + detailModel.Expiry_Date + "'," + detailModel.Stock_Qty + "," + detailModel.Quantity + ", " + detailModel.Buying_Price + ", " + detailModel.Total_Price + ", '" + entity.CreatedOn + "')";
                        queryDetail = "UPDATE Purchase_Detail_tbl SET Expiry_Date='" + detailModel.Expiry_Date + "', Batch_No='" + detailModel.Batch_No + "',  Quantity=" + detailModel.Quantity + ",Stock_Qty= "+ stockQty + ",Total_Price="+detailModel.Total_Price+ ",LastModifiedOn= "+ entity.LastModifiedOn + " Where Purchase_Detail_Id = "+ detailModel.Purchase_Detail_Id+"";
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

            return entity ;
           
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
            var query = "DELETE FROM Purchase_Detail_tbl WHERE Purchase_Detail_Id = " + detailId + "";
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
            var query = "DELETE FROM Purchase_Master_tbl WHERE Purchase_Master_Id = " + masterId + "";
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

        public async Task<IReadOnlyList<PurchaseMasterModel>> GetAllAsync()
        {
            var query = "Select PM.Invoice_No,PM.Purchase_Master_Id, s.Supplier_Id, s.Supplier_Name, PM.Purchase_Date, Pm.Payment_Type, PM.Status, PM.Grand_Total From Purchase_Master_tbl PM left join SupplierInfo s on PM.Supplier_Id = s.Supplier_Id";
            //var query = "Select PM.Purchase_Master_Id, PM.SupplierId, PM.Purchase_Date, PM.Invoice_No,PM.Deatils,PM.Payment_Type,PM.GrandTotal, " +
            //            "PM.DiscountPrice, PD.Medicine_Id, Pd.Batch_No, PD.Expiry_Date, Pd.Stock_Qty, PD.Quantity, PD.Quantity, PD.Buying_Price, PD.Total_Price " +
            //            "From Purchase_Master_tbl PM, Purchase_Detail_tbl PD";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<PurchaseMasterModel>(query);
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

        
       

        public async Task<IEnumerable<PurchaseMasterModel>> GetAllMedicine()
        {
            var query = " SELECT * FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.CategoryId = c.Id " +
                        " And m.ShelfId = s.Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<PurchaseMasterModel>(query);
                return result.ToList();
            }
        }

        public async Task<IEnumerable<PurchaseModel>> GetMedicineInfo(string medicine, string manufacturer_id)
        {
            
            var query = " SELECT m.Id,m.Medicine_Id,m.Medicine_Name,m.Category_Id,c.Category_Name,m.Shelf_Id,s.Shelf_Name,s.Shelf_Number,m.Buying_Price,m.Selling_Price,m.Expiry_Date " +
                        " FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.Category_Id = c.Id  And m.Shelf_Id = s.Id AND m.Medicine_Name Like '" + medicine + "%'And m.Supplier_Id = '"+ manufacturer_id + "'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<PurchaseModel>(query);
                return result;
            }
        }

        public async Task<PurchaseModel> GetMedicineId(string medicineId, string manufacturer_id)
        {
            //var query = " SELECT * FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.CategoryId = c.Id " +
            //            " And m.ShelfId = s.Id AND m.MedicineId = '"+ medicineId + "'";
            var query = " SELECT m.Id, m.Medicine_Id, m.Medicine_Name, m.Category_Id, c.Category_Name, m.Shelf_Id, s.Shelf_Name, s.Shelf_Number, m.Buying_Price, m.Selling_Price, m.Expiry_Date " +
                        " FROM MedicineInfo m left Join  CategoryInfo c on m.Category_Id = c.Id left join  ShelfInfo s on m.Shelf_Id = s.Id " +
                        "Where m.Medicine_Id = '" + medicineId + "'And m.Supplier_Id = '" + manufacturer_id + "'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleAsync<PurchaseModel>(query);
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

        public async Task<PurchaseMasterModel> GetMasterInfoAsync(int id)
        {
            var query = "Select PM.Invoice_No,PM.Purchase_Master_Id, PM.Supplier_Id, s.Supplier_Name, PM.Purchase_Date, PM.Payment_Type, PM.Details, PM.Status, PM.Grand_Total From Purchase_Master_tbl PM left join SupplierInfo s on PM.Supplier_Id = s.Supplier_Id Where PM.Purchase_Master_Id = " + id + "";
            //var query = "Select PM.Purchase_Master_Id, PM.SupplierId, PM.Purchase_Date, PM.Invoice_No,PM.Deatils,PM.Payment_Type,PM.GrandTotal, " +
            //            "PM.DiscountPrice, PD.Medicine_Id, Pd.Batch_No, PD.Expiry_Date, Pd.Stock_Qty, PD.Quantity, PD.Quantity, PD.Buying_Price, PD.Total_Price " +
            //            "From Purchase_Master_tbl PM, Purchase_Detail_tbl PD";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<PurchaseMasterModel>(query);
                return result;
            }
        }


        public async Task<PurchaseMasterModel> GetByMasterIdAsync(int id)
        {
            var masterInfo = await GetMasterInfoAsync(id);


            //if (!master.Eq null)
            //{
            //    return new List<SubMenuViewModel>();
            //}

            //var vmList = new PurchaseMasterModel();
            var vm = new PurchaseMasterModel();
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
            if (masterInfo.Purchase_Master_Id > 0)
            {
                var master = await GetMasterInfoAsync(id);
                vm.Purchase_Master_Id = master.Purchase_Master_Id;
                vm.Supplier_Id = master.Supplier_Id;
                vm.Supplier_Name = master.Supplier_Name;
                vm.Purchase_Date = master.Purchase_Date;
                vm.Invoice_No = master.Invoice_No;
                vm.Payment_Type = master.Payment_Type;
                vm.Details = master.Details;
                vm.Status = master.Status;
                vm.Discount_Price = master.Discount_Price;
                vm.Grand_Total = master.Grand_Total;
                vm.DetailList = await GetDetailListByMasterId(masterInfo.Purchase_Master_Id);
                //vmList.Add(vm);
            }
            return vm;
        }

        public async Task<List<PurchaseDetailModel>> GetDetailListByMasterId(int purchase_Master_Id)
        {
            var query = "Select PD.Purchase_Master_Id, PD.Purchase_Detail_Id, PD.Medicine_Id, m.Medicine_Name, Pd.Batch_No, PD.Expiry_Date, Pd.Stock_Qty, PD.Quantity, " +
                "PD.Buying_Price, PD.Total_Price From Purchase_Detail_tbl PD Left Join MedicineInfo m on m.Medicine_Id = PD.Medicine_Id Where PD.Purchase_Master_Id = " + purchase_Master_Id + "";
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
                        model.Buying_Price = item.Buying_Price;
                        model.Expiry_Date = item.Expiry_Date;
                        model.Selling_Price = item.Selling_Price;
                        model.Stock_Qty = item.Stock_Qty;
                        model.Total_Price = item.Total_Price;
                        model.Quantity = item.Quantity;
                        model.Purchase_Detail_Id = item.Purchase_Detail_Id;
                        model.Purchase_Master_Id = item.Purchase_Master_Id;
                        Sl++;
                        items.Add(model);
                    }
                    
                }

                return items;
            }
        }

        public async Task<IEnumerable<PurchaseModel>> GetPurchaseList()
        {
            var query = "Select * From VW_Purchase_Invoice Where Purchase_Id=1";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {

                connection.Open();
                var result = await connection.QueryAsync<PurchaseModel>(query);
                return result;
            }
        }

        public async Task<IEnumerable<PurchaseModel>> GetPurchaseMasterData(int id)
        {
            //var query = "Select Purchase_Master_Id,Supplier_Id,Purchase_Date,Invoice_No,Payment_Type,Discount_Price,Grand_Total From Purchase_Master_tbl Where Purchase_Master_Id = "+id+"";
            var query = "Select a.Purchase_Master_Id,a.Supplier_Id, b.Supplier_Name ,a.Purchase_Date,a.Invoice_No,a.Details,a.Payment_Type, a.Status,a.Discount_Price,a.Grand_Total " +
                        " From Purchase_Master_tbl a,SupplierInfo b  Where a.Supplier_Id = b.Supplier_Id And a.Purchase_Master_Id = "+ id + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {

                connection.Open();
                var result = await connection.QueryAsync<PurchaseModel>(query);
                return result;
            }
        }


        public Task<PurchaseMasterModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(int id, PurchaseMasterModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
