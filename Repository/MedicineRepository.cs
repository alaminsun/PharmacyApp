using Dapper;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Master.Models;
using PhramacyApp.DAL.Gateway;
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
    public class MedicineRepository : IMedicineRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        private readonly DBHelper _dbHelper;
        
        public MedicineRepository(IConfiguration configuration, IDGenerated iDGenerated, DBHelper dbHelper)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
            _dbHelper = dbHelper;
        }
        public async Task<MedicineModel> AddAsync(MedicineModel medicine)
        {
            string code = string.Empty;
            medicine.CreatedOn = DateTime.Now.ToString();
            int MaxID = _iDGenerated.getMAXSLFIN("MedicineInfo", "Id");
            medicine.Id = MaxID;
            //int MedicineId = _iDGenerated.getMAXSLFIN("MedicineInfo", "Medicine_Id");
            //medicine.Medicine_Id = MedicineId.ToString();
            string firstCharacter = string.Empty;
            if (medicine.Supplier_Nic_Name.Length == 3)
                firstCharacter = medicine.Supplier_Nic_Name;
            else
                firstCharacter = medicine.Supplier_Nic_Name.Substring(1, 3);
            string Qry = "SELECT Medicine_Id FROM MedicineInfo WHERE Medicine_Id LIKE '" + firstCharacter + "%' AND LEN(Medicine_Id) =10 ORDER BY Medicine_Id DESC";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                //if (dt.Rows[0]["PROD_ID"].ToString().Length == 10)
                //{
                string lastNum = dt.Rows[0]["Medicine_Id"].ToString().Substring(3, dt.Rows[0]["Medicine_Id"].ToString().Length - 3);
                long lastNumber = Convert.ToInt64(lastNum) + 1;

                if (lastNumber < 10)
                    code = firstCharacter + "000000" + lastNumber.ToString();
                else if (lastNumber < 100)
                    code = firstCharacter + "00000" + lastNumber.ToString();
                else if (lastNumber < 1000)
                    code = firstCharacter + "0000" + lastNumber.ToString();
                else if (lastNumber < 10000)
                    code = firstCharacter + "000" + lastNumber.ToString();
                else if (lastNumber < 100000)
                    code = firstCharacter + "00" + lastNumber.ToString();
                else if (lastNumber < 1000000)
                    code = firstCharacter + "0" + lastNumber.ToString();
                else if (lastNumber < 10000000)
                    code = firstCharacter + lastNumber.ToString();
                //}
                //else
                //    code = firstCharacter + "0000001";
            }
            else
                code = firstCharacter + "0000001";

            var query = "INSERT INTO MedicineInfo (Id, Medicine_Id, Category_Id, Shelf_Id, Strength_Name ,Medicine_Name, Generic_Name, Buying_Price, Selling_Price, Supplier_Id, CreatedOn) VALUES (@MaxID, @MedicineId, @CategoryId, @ShelfId, @StrengthName, @MedicineName, @GenericName,@BuyingPrice,@SellingPrice,@SupplierId, @CreatedOn)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", medicine.Id, DbType.Int32);
            parameters.Add("@MedicineId", code, DbType.String);
            parameters.Add("@CategoryId", medicine.Category_Id, DbType.String);
            parameters.Add("@ShelfId", medicine.Shelf_Id, DbType.String);
            parameters.Add("@MedicineName", medicine.Medicine_Name, DbType.String);
            parameters.Add("@GenericName", medicine.Generic_Name, DbType.String);
            parameters.Add("@StrengthName", medicine.Strength_Name, DbType.String);
            parameters.Add("@BuyingPrice", medicine.Buying_Price, DbType.Decimal);
            parameters.Add("@SellingPrice", medicine.Selling_Price, DbType.Decimal);
            //parameters.Add("@ExpiryDate", medicine.Expiry_Date, DbType.String);
            parameters.Add("@SupplierId", medicine.Supplier_Id, DbType.String);
            parameters.Add("@CreatedOn", medicine.CreatedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);

                var medicineDTO = new MedicineModel
                {
                    Id = id,
                    Medicine_Id = code,
                    Category_Id = medicine.Category_Id,
                    Shelf_Id = medicine.Shelf_Id,
                    Medicine_Name = medicine.Medicine_Name,
                    Strength_Name = medicine.Strength_Name,
                    Buying_Price = medicine.Buying_Price,
                    Selling_Price = medicine.Selling_Price,
                    //Expiry_Date = medicine.Expiry_Date,
                    CreatedOn = medicine.CreatedOn
                };
                return medicineDTO;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM MedicineInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<MedicineModel>> GetAllAsync()
        {
            ////var query = " SELECT m.*, c.CategoryName, s.ShelfName FROM MedicineInfo m, CategoryInfo c, ShelfInfo s Where m.CategoryId = c.Id " +
            ////" And m.ShelfId = s.Id ";
            ////var query = " SELECT m.*, c.Category_Name, s.Shelf_Name, si.Supplier_Name FROM MedicineInfo m, CategoryInfo c, ShelfInfo s, SupplierInfo si Where m.Category_Id = c.Id  And m.Shelf_Id = s.Id AND m.Supplier_Id = si.Supplier_Id";
            //var query = "Select Id,Medicine_Id,Medicine_Name,Generic_Name,Supplier_Id,Supplier_Name,Category_Id,Category_Name,Strength_Code,Strength_Name,Shelf_Id,Shelf_Name,Buying_Price,Selling_Price from MedicineInfo";
            ////using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            ////{
            //using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            //{
            //    List<MedicineModel> items = new List<MedicineModel>();
            //    //var model = new PurchaseDetailModel();
            //    connection.Open();
            //    var result = await connection.QueryAsync<MedicineModel>(query);
            //    var length = result.Count();
            //    //int Sl = 1;
            //    if (length > 0)
            //    {
            //        foreach (var item in result)
            //        {
            //            var model = new MedicineModel();
            //            //model.Sl = Sl;
            //            model.Id = item.Id;
            //            model.Medicine_Id = item.Medicine_Id;
            //            model.Medicine_Name = item.Medicine_Name;
            //            model.Generic_Name = item.Generic_Name;
            //            model.Supplier_Id = item.Supplier_Id;
            //            model.Supplier_Name = item.Supplier_Name;
            //            model.Category_Id = item.Category_Id;
            //            model.Category_Name = item.Category_Name;
            //            model.Strength_Code = item.Strength_Code;
            //            model.Strength_Name = item.Strength_Name;
            //            model.Shelf_Id = item.Shelf_Id;
            //            model.Shelf_Name = item.Shelf_Name;
            //            model.Buying_Price = item.Buying_Price;
            //            model.Selling_Price = item.Selling_Price;
            //            //model.Stock_Qty = item.Stock_Qty;
            //            //model.Total_Price = item.Total_Price;
            //            //model.Quantity = item.Quantity;
            //            //model.Purchase_Detail_Id = item.Purchase_Detail_Id;
            //            //model.Purchase_Master_Id = item.Purchase_Master_Id;
            //            //Sl++;
            //            items.Add(model);
            //        }

            //    }

            //    return items;
            //}

            //connection.Open();
            //    var result = await connection.QueryAsync<MedicineModel>(query);
            //    return result.ToList();
            //}

            //var query = "Select Id,Medicine_Id,Medicine_Name,Generic_Name,Supplier_Id,Supplier_Name,Category_Id,Category_Name,Strength_Code,Strength_Name,Shelf_Id,Shelf_Name,Buying_Price,Selling_Price from MedicineInfo";
            //var query =  "Select * From MedicineInfo";
            var query = " Select a.Id,a.Medicine_Id,a.Medicine_Name,a.Generic_Name,a.Supplier_Id,a.Supplier_Name,a.Category_Id,c.Category_Name,a.Strength_Code," +
                        " a.Strength_Name,a.Shelf_Id,s.Shelf_Name,a.Buying_Price,a.Selling_Price from MedicineInfo a " +
                        " left join  CategoryInfo c on a.Category_Id = c.Id " +
                        " left join ShelfInfo s on a.Shelf_Id = s.Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<MedicineModel>(query);
                return result.ToList();
            }


          }


        public List<MedicineModel> GetAllMedicine()
        {
            var query = " Select a.Id,a.Medicine_Id,a.Medicine_Name,a.Generic_Name,a.Supplier_Id,a.Supplier_Name,a.Category_Id,c.Category_Name,a.Strength_Code," +
                        " a.Strength_Name,a.Shelf_Id,s.Shelf_Name,a.Buying_Price,a.Selling_Price from MedicineInfo a " +
                        " left join  CategoryInfo c on a.Category_Id = c.Id " +
                        " left join ShelfInfo s on a.Shelf_Id = s.Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = connection.Query<MedicineModel>(query,commandTimeout:30);
                return result.ToList();
            }


        }




        //public async Task<Medicine> GetMedicine(int Currentpage)
        //{
        //    using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
        //    {
        //        //connection.Open();
        //        //var result = await connection.QueryAsync<MedicineModel>(query);
        //        //return result.ToList();
        //        int maxRows = 10;
        //        SqlCommand com =  new SqlCommand("Sp_Medicine_Paging", connection);
        //        com.CommandType =  CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@Pageindex", Currentpage);
        //        com.Parameters.AddWithValue("@Pagesize", maxRows);

        //        SqlDataAdapter da = new SqlDataAdapter(com);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        connection.Open();
        //        //SqlCommand cmd = new SqlCommand(qry, connection);
        //        SqlDataReader reader = com.ExecuteReader();
        //        List<MedicineModel> items = new List<MedicineModel>();

        //        Medicine medicineModel = new Medicine();
        //        //List<MedicineModel> list = new List<MedicineModel>(); 
        //        //foreach (DataRow dr in ds.Tables[0].Rows)
        //        //{
        //        //    list.Add(new MedicineModel
        //        //    {
        //        //        Id = Convert.ToInt32(dr["Id"]),
        //        //        Medicine_Id = dr["Medicine_Id"].ToString(),
        //        //        Medicine_Name = dr["Medicine_Name"].ToString(),
        //        //        Strength_Code = dr["Strength_Code"].ToString(),
        //        //        Strength_Name = dr["Strength_Name"].ToString(),
        //        //        Category_Id = dr["Category_Id"].ToString(),
        //        //        Category_Name = dr["Category_Name"].ToString(),
        //        //        Shelf_Id = dr["Shelf_Id"].ToString(),
        //        //        Shelf_Name = dr["Shelf_Name"].ToString(),
        //        //        Generic_Name = dr["Generic_Name"].ToString(),
        //        //        Buying_Price = Convert.ToDecimal(dr["Buying_Price"]),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        Selling_Price = Convert.ToDecimal(dr["Selling_Price"]),
        //        //        Supplier_Id = dr["Supplier_Id"].ToString(),
        //        //        Supplier_Name = dr["Supplier_Name"].ToString()
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),
        //        //        //Medicine_Id = dr["Id"].ToString(),

        //        //    });
        //        //}

        //        while (reader.Read())
        //        {
        //            MedicineModel model = new MedicineModel();
        //            model.Id = Convert.ToInt32(reader["Id"]);
        //            model.Medicine_Id = reader["Medicine_Id"].ToString();
        //            model.Medicine_Name = reader["Medicine_Name"].ToString();
        //            model.Strength_Code = reader["Strength_Code"].ToString();
        //            model.Strength_Name = reader["Strength_Name"].ToString();
        //            model.Category_Id = reader["Category_Id"].ToString();
        //            model.Category_Name = reader["Category_Name"].ToString();
        //            model.Shelf_Id = reader["Shelf_Id"].ToString();
        //            model.Shelf_Name = reader["Shelf_Name"].ToString();
        //            model.Generic_Name = reader["Generic_Name"].ToString();
        //            //model.Buying_Price = Convert.ToDecimal(reader["Buying_Price"]);
        //            if (!reader.IsDBNull(10))
        //            {
        //                model.Buying_Price = Convert.ToDecimal(reader["Buying_Price"]);

        //            }
        //            else
        //            {
        //                model.Buying_Price = 0;
        //            }
        //            if (!reader.IsDBNull(11))
        //            {
        //                model.Selling_Price = Convert.ToDecimal(reader["Selling_Price"]);

        //            }
        //            else
        //            {
        //                model.Selling_Price = 0;
        //            }
        //            //Medicine_Id = dr["Id"].ToString(),
        //           // model.Selling_Price = Convert.ToDecimal(reader["Selling_Price"]);
        //            model.Supplier_Id = reader["Supplier_Id"].ToString();
        //            model.Supplier_Name = reader["Supplier_Name"].ToString();

        //                items.Add(model);
        //        }

        //        medicineModel.MedicineList = items;
        //        medicineModel.PageCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]) / maxRows;
        //        medicineModel.CurrentIndex = Currentpage;
        //        return  medicineModel;
        //    }
        //}
            

        public async Task<MedicineModel> GetByIdAsync(int id)
        {
            //var query = "SELECT * FROM MedicineInfo WHERE Id = @Id";
            var query = "SELECT m.*, c.Category_Name, s.Shelf_Name, si.Supplier_Name FROM MedicineInfo m, CategoryInfo c, ShelfInfo s, SupplierInfo si Where m.Category_Id = c.Id  And m.Shelf_Id = s.Id AND m.Supplier_Id = si.Supplier_Id AND m.Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<MedicineModel>(query, new { Id = id });
                return result;
            }

        }

        public async Task<int> UpdateAsync(int id, MedicineModel medicine)
        {
            medicine.LastModifiedOn = DateTime.Now.ToString();
            string code = string.Empty;
            code = GetMedicineId(id);
            var query = "UPDATE MedicineInfo SET Medicine_Id=@MedicineId,Medicine_Name = @MedicineName, Category_Id = @CategoryId, Shelf_Id = @ShelfId, Strength_Name= @Strength_Name, Generic_Name = @GenericName, Buying_Price = @BuyingPrice, Selling_Price = @SellingPrice, Supplier_Id = @SupplierId, LastModifiedOn=@LastModifiedOn WHERE Id = @Id";
            //INSERT INTO MedicineInfo(Id, CategoryId, ShelfId, MedicineName, BatchNo, BuyingPrice, SellingPrice, ExpiryDate, CreatedOn) VALUES(@MaxID, @CategoryId, @ShelfId, @MedicineName, @BatchNo, @BuyingPrice, @SellingPrice, @ExpiryDate @CreatedOn)
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@MedicineId", code, DbType.String);
            parameters.Add("@CategoryId", medicine.Category_Id, DbType.String);
            parameters.Add("@ShelfId", medicine.Shelf_Id, DbType.String);
            parameters.Add("@MedicineName", medicine.Medicine_Name, DbType.String);
            parameters.Add("@GenericName", medicine.Generic_Name, DbType.String);
            parameters.Add("@Strength_Name", medicine.Strength_Name, DbType.String);
            parameters.Add("@BuyingPrice", medicine.Buying_Price, DbType.Decimal);
            parameters.Add("@SellingPrice", medicine.Selling_Price, DbType.Decimal);
            //parameters.Add("@ExpiryDate", medicine.Expiry_Date, DbType.String);
            parameters.Add("@SupplierId", medicine.Supplier_Id, DbType.String);
            parameters.Add("@LastModifiedOn", medicine.LastModifiedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        private string GetMedicineId(int id)
        {
            string qry = "Select Medicine_Id From MedicineInfo where Id ="+ id +"";
            DataTable dt = _dbHelper.GetDataTable(qry);
            if (dt.Rows.Count > 0)
            {
                string Medicine_Id = dt.Rows[0]["Medicine_Id"].ToString();

                return Medicine_Id;
            }
            else
                return string.Empty;
        }

        public int GetSearchValueCount(string searchValue)
        {
            var query = "Select * From MedicineInfo Where 1 = 1";
            query = query + "AND Medicine_Name like '%" +searchValue+"%'";
            query = query + "AND Medicine_Id like '%" + searchValue + "%'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = connection.Query<MedicineModel>(query);
                return result.Count();
            }
        }

        public List<MedicineModel> GetSearchValue(string searchValue, string sortColumn, string sortColumnDirection)
        {
            var query = " Select a.Id,a.Medicine_Id,a.Medicine_Name,a.Generic_Name,a.Supplier_Id,a.Supplier_Name,a.Category_Id,c.Category_Name,a.Strength_Code," +
                 " a.Strength_Name,a.Shelf_Id,s.Shelf_Name,a.Buying_Price,a.Selling_Price from MedicineInfo a " +
                 " left join  CategoryInfo c on a.Category_Id = c.Id " +
                 " left join ShelfInfo s on a.Shelf_Id = s.Id Where 1=1 ";

            query = query + " AND a.Medicine_Name like '" + searchValue + "%'  OR a.Supplier_Name LIKE '%" + searchValue + "%' OR a.Generic_Name LIKE '%" + searchValue + "%'";

            //if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            //{
            //    query = query + " Order By " + sortColumn + " " + sortColumnDirection + "";
            //}
            //if (sortColumn != "Id")
            //{
            //    query = query + " Order By " + sortColumn + " " + sortColumnDirection + "";
            //}

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = connection.Query<MedicineModel>(query,commandTimeout:80);
                return result.ToList();
            }
        }


        public async Task<List<MedicineModel>> GetProductsAsync(ProductListRequest request)
        {
            try
            {
                var procedure = "spGetProductsList";

                var parameters = new DynamicParameters();
                parameters.Add("SearchValue", request.SearchValue, DbType.String);
                parameters.Add("PageNo", request.PageNo, DbType.Int32);
                parameters.Add("PageSize", request.PageSize, DbType.Int32);
                parameters.Add("SortColumn", request.SortColumn, DbType.Int32);
                parameters.Add("SortDirection", request.SortDirection, DbType.String);

                using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
                {
                    return (await connection.QueryAsync<MedicineModel>(procedure, parameters, commandTimeout:80, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public MedicineModel GetSupplierNicName(string suplier_Id)
        {

            //    Data.StudentContext.StudentList
            //    .Where(a => a.Lastname.Contains(searchValue) || a.Firstname.Contains(searchValue))
            //    .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))//Sort by sortColumn
            //    .Skip(start)
            //    .Take(length)
            //    .ToList<Student>();
            string query = "Select Supplier_Nic_Name from SupplierInfo Where Supplier_Id= '"+ suplier_Id + "' "; 
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<MedicineModel>(query);
                return result;
            }
        }

        public List<MedicineModel>medicineDataForSorting(string sortColumn, string sortColumnDirection)
        {
            var query = " Select a.Id,a.Medicine_Id,a.Medicine_Name,a.Generic_Name,a.Supplier_Id,a.Supplier_Name,a.Category_Id,c.Category_Name,a.Strength_Code," +
             " a.Strength_Name,a.Shelf_Id,s.Shelf_Name,a.Buying_Price,a.Selling_Price from MedicineInfo a " +
             " left join  CategoryInfo c on a.Category_Id = c.Id " +
             " left join ShelfInfo s on a.Shelf_Id = s.Id Order By "+ sortColumn + " "+ sortColumnDirection + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = connection.Query<MedicineModel>(query,commandTimeout:30);
                return result.ToList();
            }
        }
    }
}
