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
    public class SubMenuRepository : ISubMenuRepository
    {

        private readonly IConfiguration configuration;
        private readonly IDGenerated _iDGenerated;
        public SubMenuRepository(IConfiguration configuration, IDGenerated iDGenerated)
        {
            _iDGenerated = iDGenerated;
            this.configuration = configuration;
        }


        public async Task<IEnumerable<SubMenuViewModel>> GetAllSubMenuByMenuId(string Role_Id, int mhId)
        {
            string query = "select A.Sm_Id, A.Sm_Name from Sub_Menu a inner join Menu_Head b on a.Mh_Id=b.Mh_Id where a.Sm_Id NOT IN (SELECT c.Sm_Id FROM Menu_Conf c WHERE c.Role_Id='" + Role_Id + "' And c.Mh_Id=" + mhId + ") AND a.Mh_Id=" + mhId + "";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                var subMenuList = await connection.QueryAsync<SubMenuViewModel>(query, new { mhId });
                return subMenuList.ToList();
            }
        }


        public async Task<SubMenuViewModel> AddAsync(SubMenuViewModel entity)
        {
            entity.CreatedOn = DateTime.Now;
            int MaxID = _iDGenerated.getMAXSLFIN("Sub_Menu", "Sm_Id");
            entity.Sm_Id = MaxID;

            var query = "INSERT INTO Sub_Menu (Sm_Id, Sm_Name, Sm_Seq, URL, Mh_Id, CreatedOn) VALUES (@MaxID, @Sm_Name, @Sm_Seq, @URL, @Mh_Id, @CreatedOn)";
            var parameters = new DynamicParameters();
            parameters.Add("@MaxID", entity.Sm_Id, DbType.Int32);
            parameters.Add("@Sm_Name", entity.Sm_Name, DbType.String);
            parameters.Add("@Sm_Seq", entity.Sm_Seq, DbType.Int32);
            parameters.Add("@URL", entity.URL, DbType.String);
            parameters.Add("@Mh_Id", entity.Mh_Id, DbType.Int32);
            parameters.Add("@CreatedOn", entity.CreatedOn, DbType.Date);
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var id = await connection.ExecuteAsync(query, parameters);
                //var id = await _dbContext.Connection.ExecuteAsync(query, parameters);



                var submenuDTO = new SubMenuViewModel
                {
                    Sm_Id = id,
                    Sm_Name = entity.Sm_Name,
                    Sm_Seq = entity.Sm_Seq,
                    URL = entity.URL,
                    Mh_Id = entity.Mh_Id,
                    CreatedOn = entity.CreatedOn
                };
                return submenuDTO;
                
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Sub_Menu WHERE Sm_Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<SubMenuViewModel>> GetAllAsync()
        {

            var query = "SELECT sm.Sm_Id,sm.Sm_Name,sm.Sm_Seq,sm.URL,sm.Mh_Id,mh.Mh_Name FROM Sub_Menu sm , Menu_Head mh where sm.Mh_Id = mh.Mh_Id";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SubMenuViewModel>(query);
                return result.ToList();
            }

        }

        public async Task<SubMenuViewModel> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Sub_Menu WHERE Sm_Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<SubMenuViewModel>(query, new { Id = id });
                return result;
            }

        }

        public async Task<int> UpdateAsync( int id, SubMenuViewModel entity)
        {
            entity.LastModifiedOn = DateTime.Now;
            var query = "UPDATE Sub_Menu SET Sm_Name = @Sm_Name, Sm_Seq = @Sm_Seq, URL = @URL, Mh_Id = @Mh_Id WHERE Sm_Id = @Id";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                parameters.Add("@Sm_Name", entity.Sm_Name, DbType.String);
                parameters.Add("@Sm_Seq", entity.Sm_Seq, DbType.Int32);
                parameters.Add("@URL", entity.URL, DbType.String);
                parameters.Add("@Mh_Id", entity.Mh_Id, DbType.Int32);
                parameters.Add("@LastModifiedOn", entity.LastModifiedOn, DbType.Date);

                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }


    }
}
