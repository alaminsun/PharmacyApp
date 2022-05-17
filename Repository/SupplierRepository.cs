using Dapper;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Master.Models;
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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public SupplierRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }


        public async Task<IReadOnlyList<SupplierModel>> GetAllAsync()
        {

            var query = "SELECT * FROM SupplierInfo";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SupplierModel>(query);
                return result.ToList();
            }

        }

        public async Task<SupplierModel> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM SupplierInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<SupplierModel>(query, new { Id = id });
                return result;
            }

        }
        public async Task<SupplierModel> AddAsync(SupplierModel supplier)
        {
            supplier.CreatedOn = DateTime.Now.ToString();
            int MaxID = _iDGenerated.getMAXSLFIN("SupplierInfo", "Id");
            supplier.Id = MaxID;
            string supplierId = _iDGenerated.getMAXSLFIN("SupplierInfo", "SupplierId").ToString();
            var query = "INSERT INTO SupplierInfo (Id, Supplier_Id, Supplier_Name, Email, Phone, Address, Notes, CreatedOn) VALUES (@MaxID, @SupplierId,@SupplierName,@Email,@Phone,@Address, @Notes, @CreatedOn)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", supplier.Id, DbType.Int32);
            parameters.Add("@SupplierId", supplierId, DbType.String);
            parameters.Add("@SupplierName", supplier.Supplier_Name, DbType.String);
            parameters.Add("@Email", supplier.Email, DbType.String);
            parameters.Add("@Phone", supplier.Phone, DbType.String);
            parameters.Add("@Address", supplier.Address, DbType.String);
            parameters.Add("@Notes", supplier.Notes, DbType.String);
            //parameters.Add("@Area", shelf.Area, DbType.String);
            parameters.Add("@CreatedOn", supplier.CreatedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);

                var supplierDTO = new SupplierModel
                {
                    Id = id,
                    Supplier_Id = supplierId,
                    Supplier_Name = supplier.Supplier_Name,
                    Email = supplier.Email,
                    Phone = supplier.Phone,
                    Address = supplier.Address,
                    Notes = supplier.Notes,
                    CreatedOn = supplier.CreatedOn
                };
                return supplierDTO;
            }
        }


        public async Task<int> UpdateAsync(int id, SupplierModel supplier)
        {
            supplier.LastModifiedOn = DateTime.Now.ToString();
            var query = "UPDATE SupplierInfo SET  Supplier_Name = @SupplierName, Email = @Email, Phone = @Phone, Address = @Address, Notes = @Notes, LastModifiedOn=@LastModifiedOn WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@SupplierId", supplier.Supplier_Id, DbType.String);
            parameters.Add("@SupplierName", supplier.Supplier_Name, DbType.String);
            parameters.Add("@Email", supplier.Email, DbType.String);
            parameters.Add("@phone", supplier.Phone, DbType.String);
            parameters.Add("@Address", supplier.Address, DbType.String);
            parameters.Add("@Notes", supplier.Notes, DbType.String);
            parameters.Add("@LastModifiedOn", supplier.LastModifiedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM SupplierInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }
    }
}
