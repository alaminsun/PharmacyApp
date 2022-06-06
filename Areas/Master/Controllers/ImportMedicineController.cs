//using java.util.zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhramacyApp.Areas.Master.Controllers
{
    [Area("Master")]
    public class ImportMedicineController : Controller
    {
        //private IHostingEnvironment Environment;
        private IConfiguration Configuration;

        public ImportMedicineController( IConfiguration _configuration)
        {
            //Environment = _environment;
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Index(IFormFile postedFile)
        //{
        //    if (postedFile != null)
        //    {
        //        //Create a Folder.
        //        string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        //Save the uploaded Excel file.
        //        string fileName = Path.GetFileName(postedFile.FileName);
        //        string filePath = Path.Combine(path, fileName);
        //        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            postedFile.CopyTo(stream);
        //        }

        //        //Read the connection string for the Excel file.
        //        string conString = this.Configuration.GetConnectionString("ExcelConString");
        //        DataTable dt = new DataTable();
        //        conString = string.Format(conString, filePath);

        //        using (OleDbConnection connExcel = new OleDbConnection(conString))
        //        {
        //            using (OleDbCommand cmdExcel = new OleDbCommand())
        //            {
        //                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
        //                {
        //                    cmdExcel.Connection = connExcel;

        //                    //Get the name of First Sheet.
        //                    connExcel.Open();
        //                    DataTable dtExcelSchema;
        //                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //                    connExcel.Close();

        //                    //Read Data from First Sheet.
        //                    connExcel.Open();
        //                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
        //                    odaExcel.SelectCommand = cmdExcel;
        //                    odaExcel.Fill(dt);
        //                    connExcel.Close();
        //                }
        //            }
        //        }

        //        //Insert the Data read from the Excel file to Database Table.
        //        conString = this.Configuration.GetConnectionString("ApplicationConnection");
        //        using (SqlConnection con = new SqlConnection(conString))
        //        {
        //            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //            {
        //                //Set the database table name.
        //                sqlBulkCopy.DestinationTableName = "dbo.SupplierInfo";

        //                //[OPTIONAL]: Map the Excel columns with that of the database table
        //                //sqlBulkCopy.ColumnMappings.Add("Id", "Id");
        //                //sqlBulkCopy.ColumnMappings.Add("Medicine_Id", "Medicine_Id");
        //                //sqlBulkCopy.ColumnMappings.Add("Medicine_Name", "Medicine_Name");
        //                //sqlBulkCopy.ColumnMappings.Add("Strength_Code", "Strength_Code");
        //                //sqlBulkCopy.ColumnMappings.Add("Strength_Name", "Strength_Name");
        //                //sqlBulkCopy.ColumnMappings.Add("Generic_Name", "Generic_Name");
        //                //sqlBulkCopy.ColumnMappings.Add("Supplier_Id", "Supplier_Id");
        //                //sqlBulkCopy.ColumnMappings.Add("Supplier_Name", "Supplier_Name");
        //                //sqlBulkCopy.ColumnMappings.Add("Selling_Price", "Selling_Price");
        //                //sqlBulkCopy.ColumnMappings.Add("Buying_Price", "Buying_Price");
        //                //sqlBulkCopy.ColumnMappings.Add("Category_Id", "Category_Id");
        //                //sqlBulkCopy.ColumnMappings.Add("Category_Name", "Category_Name");
        //                //sqlBulkCopy.ColumnMappings.Add("Shelf_Id", "Shelf_Id");
        //                //sqlBulkCopy.ColumnMappings.Add("Shelf_Name", "Shelf_Name");
        //                //sqlBulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
        //                sqlBulkCopy.ColumnMappings.Add("Id", "Id");
        //                sqlBulkCopy.ColumnMappings.Add("Supplier_Name", "Supplier_Name");
        //                sqlBulkCopy.ColumnMappings.Add("Supplier_Id", "Supplier_Id");
        //                sqlBulkCopy.ColumnMappings.Add("Supplier_Nic_Name", "Supplier_Nic_Name");

        //                try
        //                {
        //                    con.Open();
        //                    sqlBulkCopy.WriteToServer(dt);
        //                    con.Close();
        //                }
        //                catch (SqlException ex)
        //                {
        //                    if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
        //                    {
        //                        string pattern = @"\d+";
        //                        Match match = Regex.Match(ex.Message.ToString(), pattern);
        //                        var index = Convert.ToInt32(match.Value) - 1;

        //                        FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
        //                        var sortedColumns = fi.GetValue(sqlBulkCopy);
        //                        var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

        //                        FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
        //                        var metadata = itemdata.GetValue(items[index]);

        //                        var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
        //                        var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
        //                        throw new DataFormatException(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
        //                    }

        //                    throw;
        //                }

        //                //con.Open();
        //                //sqlBulkCopy.WriteToServer(dt);
        //               //on.Close();
        //            }
        //        }
        //    }

        //    return View();
        //}
    }
}
