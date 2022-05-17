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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public CategoryRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }
        public async Task<CategoryModel> AddAsync(CategoryModel category)
        {
            category.CreatedOn = DateTime.Now.ToString();
            int MaxID = _iDGenerated.getMAXSLFIN("CategoryInfo", "Id");
            category.Id = MaxID;
            var query = "INSERT INTO CategoryInfo (Id, Category_Name, Description, CreatedOn) VALUES (@MaxID, @CategoryName, @Description, @CreatedOn)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", category.Id, DbType.Int32);
            parameters.Add("@CategoryName", category.Category_Name, DbType.String);
            parameters.Add("@Description", category.Description, DbType.String);
            //parameters.Add("@Area", shelf.Area, DbType.String);
            parameters.Add("@CreatedOn", category.CreatedOn, DbType.String);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);

                var categoryDTO = new CategoryModel
                {
                    Id = id,
                    Category_Name = category.Category_Name,
                    Description = category.Description,
                    CreatedOn = category.CreatedOn
                };
                return categoryDTO;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM CategoryInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<CategoryModel>> GetAllAsync()
        {

            var query = "SELECT Id, Category_Name, Description, CreatedOn, LastModifiedOn FROM CategoryInfo";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<CategoryModel>(query);
                return result.ToList();
            }

        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM CategoryInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<CategoryModel>(query, new { Id = id });
                return result;
            }

        }

        public async Task<int> UpdateAsync(int id, CategoryModel category)
        {
            category.LastModifiedOn = DateTime.Now.ToString();
            var query = "UPDATE CategoryInfo SET Category_Name = @CategoryName, Description = @Description, LastModifiedOn=@LastModifiedOn WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@CategoryName", category.Category_Name, DbType.String);
            parameters.Add("@Description", category.Description, DbType.String);
            parameters.Add("@LastModifiedOn", category.LastModifiedOn, DbType.String);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }
    }
}
