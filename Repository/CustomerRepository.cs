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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public CustomerRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }


        public async Task<IReadOnlyList<CustomerModel>> GetAllAsync()
        {

            var query = "SELECT * FROM CustomerInfo";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<CustomerModel>(query);
                return result.ToList();
            }

        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM CustomerInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<CustomerModel>(query, new { Id = id });
                return result;
            }

        }
        public async Task<CustomerModel> AddAsync(CustomerModel customer)
        {
            customer.CreatedOn = DateTime.Now;
            int MaxID = _iDGenerated.getMAXSLFIN("CustomerInfo", "Id");
            customer.Id = MaxID;
            string customerId = _iDGenerated.getMAXSLFIN("CustomerInfo", "Customer_Id").ToString();
            var query = "INSERT INTO CustomerInfo (Id, Customer_Id, Customer_Name, Email, Phone, Address, Notes, CreatedOn) VALUES (@MaxID, @CustomerId,@CustomerName,@Email,@Phone,@Address, @Notes, @CreatedOn)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", customer.Id, DbType.Int32);
            parameters.Add("@CustomerId", customerId, DbType.String);
            parameters.Add("@CustomerName", customer.Customer_Name, DbType.String);
            parameters.Add("@Email", customer.Email, DbType.String);
            parameters.Add("@Phone", customer.Phone, DbType.String);
            parameters.Add("@Address", customer.Address, DbType.String);
            parameters.Add("@Notes", customer.Notes, DbType.String);
            //parameters.Add("@Area", shelf.Area, DbType.String);
            parameters.Add("@CreatedOn", customer.CreatedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);

                var customerDTO = new CustomerModel
                {
                    Id = id,
                    Customer_Id = customerId,
                    Customer_Name = customer.Customer_Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    Notes = customer.Notes,
                    CreatedOn = customer.CreatedOn
                };
                return customerDTO;
            }
        }


        public async Task<int> UpdateAsync(int id, CustomerModel customer)
        {
            customer.LastModifiedOn = DateTime.Now;
            var query = "UPDATE CustomerInfo SET  Customer_Name = @CustomerName, Email = @Email, Phone = @Phone, Address = @Address, Notes = @Notes, LastModifiedOn=@LastModifiedOn WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@CustomerId", customer.Customer_Id, DbType.String);
            parameters.Add("@CustomerName", customer.Customer_Name, DbType.String);
            parameters.Add("@Email", customer.Email, DbType.String);
            parameters.Add("@phone", customer.Phone, DbType.String);
            parameters.Add("@Address", customer.Address, DbType.String);
            parameters.Add("@Notes", customer.Notes, DbType.String);
            parameters.Add("@LastModifiedOn", customer.LastModifiedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM CustomerInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }



    }
}
