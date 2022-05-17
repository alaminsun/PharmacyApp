using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Helpers;
using PhramacyApp.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Repository
{
    public class MenuRepository : IMenuRepository
    {

        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public MenuRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }
        public async Task<MenuViewModel> AddAsync(MenuViewModel menu)
        {
            menu.CreatedOn = DateTime.Now;
            int MaxID = _iDGenerated.getMAXSLFIN("Menu_Head", "Mh_Id");
            menu.Mh_Id = MaxID;
            var query = "INSERT INTO Menu_Head (Mh_Id, Mh_Name, Mh_Seq, CreatedOn, Area) VALUES (@MaxID, @Name, @MHSeq, @CreatedOn, @Area)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", menu.Mh_Id, DbType.Int32);
            parameters.Add("@Name", menu.Mh_Name, DbType.String);
            parameters.Add("@MHSeq", menu.Mh_Seq, DbType.Int32);
            parameters.Add("@Area", menu.Area, DbType.String);
            parameters.Add("@CreatedOn", menu.CreatedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);

                var menuDTO = new MenuViewModel
                {
                    Mh_Id = id,
                    Mh_Name = menu.Mh_Name,
                    Mh_Seq = menu.Mh_Seq,
                    Role_Id = menu.Role_Id,
                    Area = menu.Area,
                    CreatedOn = menu.CreatedOn
                };
                return menuDTO;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Menu_Head WHERE Mh_Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<MenuViewModel>> GetAllAsync()
        {
            
            var query = "SELECT Mh_Id, Mh_Seq, Mh_Name, CreatedOn, LastModifiedOn, m.Area FROM Menu_Head m";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<MenuViewModel>(query);
                return result.ToList();
            }

        }

        public async Task<MenuViewModel> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Menu_Head WHERE Mh_Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<MenuViewModel>(query, new { Id = id });
                return result;
            }

        }

        public async Task<int> UpdateAsync(int id,MenuViewModel menu)
        {
            menu.LastModifiedOn = DateTime.Now;
            var query = "UPDATE Menu_Head SET Mh_Name = @Name, Mh_Seq = @MHSeq, Area=@Area WHERE Mh_Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@Name", menu.Mh_Name, DbType.String);
            parameters.Add("@MHSeq", menu.Mh_Seq, DbType.Int32);
            //parameters.Add("@Role_Id", menu.Role_Id, DbType.String);
            parameters.Add("@Area", menu.Area, DbType.String);
            parameters.Add("@LastModifiedOn", menu.LastModifiedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }





    }
}