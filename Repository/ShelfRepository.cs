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
    public class ShelfRepository : IShelfRepository
    {
        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public ShelfRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }
        public async Task<ShelfModel> AddAsync(ShelfModel shelf)
        {
            shelf.CreatedOn = DateTime.Now;
            int MaxID = _iDGenerated.getMAXSLFIN("ShelfInfo", "Id");
            shelf.Id = MaxID;
            var query = "INSERT INTO ShelfInfo (Id, Shelf_Name, Shelf_Number, CreatedOn) VALUES (@MaxID, @ShelfName, @ShelfNumber, @CreatedOn)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", shelf.Id, DbType.Int32);
            parameters.Add("@ShelfName", shelf.Shelf_Name, DbType.String);
            parameters.Add("@ShelfNumber", shelf.Shelf_Number, DbType.Int32);
            //parameters.Add("@Area", shelf.Area, DbType.String);
            parameters.Add("@CreatedOn", shelf.CreatedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);

                var shelfDTO = new ShelfModel
                {
                    Id = id,
                    Shelf_Name = shelf.Shelf_Name,
                    Shelf_Number = shelf.Shelf_Number,
                    CreatedOn = shelf.CreatedOn
                };
                return shelfDTO;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM ShelfInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<ShelfModel>> GetAllAsync()
        {

            var query = "SELECT Id, Shelf_Name, Shelf_Number, CreatedOn, LastModifiedOn FROM ShelfInfo";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<ShelfModel>(query);
                return result.ToList();
            }

        }

        public async Task<ShelfModel> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM ShelfInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<ShelfModel>(query, new { Id = id });
                return result;
            }

        }

        public async Task<int> UpdateAsync(int id, ShelfModel shelf)
        {
            shelf.LastModifiedOn = DateTime.Now;
            var query = "UPDATE ShelfInfo SET Shelf_Name = @ShelfName, Shelf_Number = @ShelfNumber, LastModifiedOn=@LastModifiedOn WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@ShelfName", shelf.Shelf_Name, DbType.String);
            parameters.Add("@ShelfNumber", shelf.Shelf_Number, DbType.Int32);
            parameters.Add("@LastModifiedOn", shelf.LastModifiedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

    }
}
