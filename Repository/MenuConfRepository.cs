using Dapper;
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
    public class MenuConfRepository : IMenuConfRepository
    {

        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public MenuConfRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }
        public async Task<MenuConfViewModel> AddAsync(MenuConfViewModel entity)
        {
            int MaxID = _iDGenerated.getMAXSLFIN("Menu_Conf", "Menu_Id");
            entity.Menu_Id = MaxID;

            var query = "INSERT INTO Menu_Conf (Menu_Id, Mh_Id, Sm_Id, Role_Id, CreatedOn) VALUES (@MaxID, @Mh_Id, @Sm_Id, @Role_Id, @CreatedOn)";
            //" SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", entity.Menu_Id, DbType.Int32);
            parameters.Add("@Mh_Id", entity.Mh_Id, DbType.Int32);
            parameters.Add("@Sm_Id", entity.Sm_Id, DbType.Int32);
            parameters.Add("@Role_Id", entity.Role_Id, DbType.String);
            //parameters.Add("@Mh_Id", subMenu.Mh_Id, DbType.Int32);
            parameters.Add("@CreatedOn", entity.CreatedOn, DbType.Date);
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);

                var menuConfDTO = new MenuConfViewModel
                {
                    Menu_Id = id,
                    Mh_Id = entity.Mh_Id,
                    Sm_Id = entity.Sm_Id,
                    Rl_Id = entity.Rl_Id,
                    //Mh_Id = Menu.Mh_Id,
                    CreatedOn = entity.CreatedOn
                };

                return menuConfDTO;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Menu_Conf WHERE Menu_Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<MenuConfViewModel>> GetAllAsync()
        {

            var query = " SELECT m.Menu_Id,m.Role_Id, r.Name Role_Name,m.Mh_Id,mh.Mh_Name,m.Sm_Id,sm.Sm_Name  FROM Menu_Conf m, [Identity].Roles r, Menu_Head mh, Sub_Menu sm where " +
                        " m.Role_Id = r.Id And m.Mh_Id = mh.Mh_Id And m.Sm_Id = sm.Sm_Id ";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<MenuConfViewModel>(query);
                return result.ToList();
            }

        }

        public async Task<MenuConfViewModel> GetByIdAsync(int id)
        {
            var query = " SELECT m.Menu_Id,m.Role_Id, r.Name Role_Name,m.Mh_Id,mh.Mh_Name,m.Sm_Id,sm.Sm_Name  FROM Menu_Conf m, [Identity].Roles r, Menu_Head mh, Sub_Menu sm where " +
                       " m.Role_Id = r.Id And m.Mh_Id = mh.Mh_Id And m.Sm_Id = sm.Sm_Id And m.Menu_Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<MenuConfViewModel>(query, new { Id = id });
                return result;
            }

        }

        public async Task<int> UpdateAsync(int id, MenuConfViewModel entity)
        {
            entity.LastModifiedOn = DateTime.Now;
            var query = "UPDATE Menu_Conf SET Mh_Id = @Mh_Id, Sm_Id = @Sm_Id, Role_Id = @Role_Id, LastModifiedOn = @LastModifiedOn WHERE Menu_Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@Sm_Id", entity.Sm_Id, DbType.String);
            parameters.Add("@Mh_Id", entity.Mh_Id, DbType.Int32);
            parameters.Add("@Role_Id", entity.Role_Id, DbType.String);
            parameters.Add("@LastModifiedOn", entity.LastModifiedOn, DbType.Date);

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }


    }
}
