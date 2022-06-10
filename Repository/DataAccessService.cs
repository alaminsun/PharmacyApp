using Dapper;
using Microsoft.Extensions.Configuration;
using PhramacyApp.Areas.Admin.Models;
using PhramacyApp.Interfaces;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PharmacyApp.Repository
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IConfiguration configuration;
        public DataAccessService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IList<MenuViewModel>> GetMenuHeadsAsync( ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            string roleName = await GetUserRoleNameAsync(ctx);
            string MHQry = "";
            if (roleName == "SuperAdmin")
            {
                MHQry = " SELECT DISTINCT MH.Mh_Id, Mh.Mh_Name, Mh.Mh_Seq, MH.Mh_Id FROM [Identity].UserRoles SRC,[Identity].Roles SR, Menu_Conf SMC, Menu_Head MH "+
                        " WHERE SRC.RoleId = SR.Id AND SMC.Role_Id = SRC.RoleId  And Mh.Mh_Id = SMC.Mh_Id  ORDER BY Mh.Mh_Id ASC ";
            }
            else
            {
                MHQry = " SELECT DISTINCT MH.Mh_Id, Mh.Mh_Name, Mh.Mh_Seq, MH.Mh_Id FROM [Identity].UserRoles SRC,[Identity].Roles SR, Menu_Conf SMC, Menu_Head MH " +
                        " WHERE SRC.RoleId = SR.Id AND SMC.Role_Id = SRC.RoleId  And Mh.Mh_Id = SMC.Mh_Id AND SRC.UserId= '"+ userId + "'  ORDER BY Mh.Mh_Id ASC ";

            }

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<MenuViewModel>(MHQry);
                return result.ToList();
            }
            

        }


        public async Task<IList<SubMenuViewModel>> GetChildrenMenuAsync(IList<SubMenuViewModel> MenuList, int id, ClaimsPrincipal ctx)
        {
            //var userId = GetUserId(ctx);
            string roleName = await GetUserRoleNameAsync(ctx);
            string SMQry = "";
            if (roleName == "SuperAdmin")
            {
                
                SMQry = "SELECT DISTINCT SSM.SM_NAME,SSM.SM_ID,SSM.SM_SEQ,SSM.SM_CSS_CLASS,SSM.URL ,SSM.Mh_Id "+
                        "FROM Sub_Menu SSM Where SSM.Mh_Id ="+ id +"";
            }
            else
            {
                var userId = GetUserId(ctx);
                var roleIds = GetUserRoleIdsAsync(userId);
                SMQry = " SELECT DISTINCT SSM.SM_NAME,SSM.SM_ID,SSM.SM_SEQ,SSM.SM_CSS_CLASS,SSM.URL,SMC.Mh_Id FROM[Identity].UserRoles SRC,[Identity].Roles SR, Menu_Conf SMC,Sub_Menu SSM " +
           " WHERE SRC.RoleId = SR.Id AND SMC.Role_Id = SRC.RoleId AND SSM.SM_ID = SMC.SM_ID AND SMC.MH_ID= "+ id + " AND SMC.Role_Id= '" + roleIds + "' ORDER BY SSM.SM_SEQ ASC";
            }

            //MenuList = (IList<SubMenuViewModel>)_dbContext.Connection.Query<SubMenuViewModel>(SMQry);
            // return MenuList.ToList();
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SubMenuViewModel>(SMQry);
                return result.ToList();
            }

        }
        public async Task<MenuViewModel> GetMenuItemAsync(IList<MenuViewModel> MenuList, int mh_Id, ClaimsPrincipal ctx)
        {
 
            string roleName = await GetUserRoleNameAsync(ctx);
            string MHQry = "";
            if (roleName == "SuperAdmin")
            {
                MHQry = "SELECT DISTINCT MH.Mh_Id, Mh.Mh_Name, Mh.Mh_Seq, MH.Mh_Id, MH.Area FROM  Menu_Head MH";
            }
            else
            {
                MHQry = " SELECT DISTINCT MH.Mh_Id, Mh.Mh_Name, Mh.Mh_Seq, MH.Mh_Id, MH.Area FROM [Identity].UserRoles SRC,[Identity].Roles SR, Menu_Conf SMC, Menu_Head MH " +
                           " WHERE SRC.RoleId = SR.Id AND SMC.Role_Id = SRC.RoleId  And Mh.Mh_Id = SMC.Mh_Id  ORDER BY Mh.Mh_Id ASC ";
            }
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
               var result =   await connection.QueryAsync<MenuViewModel>(MHQry);
                return result.FirstOrDefault(x => x.Mh_Id == mh_Id);

            }
        }

        public async Task<IList<MenuViewModel>> GetMenuListAsync(IList<MenuViewModel> MenuList, ClaimsPrincipal ctx)
        {
            
            string roleName = await GetUserRoleNameAsync(ctx);
            string MHQry = ""; 
            if (roleName == "SuperAdmin")
            {
                 MHQry = "SELECT DISTINCT MH.Mh_Id, Mh.Mh_Name, Mh.Mh_Seq, MH.Mh_Id, MH.Area FROM  Menu_Head MH";
            }
            else
            {
                var userId = GetUserId(ctx);
                string roleIds =  GetUserRoleIdsAsync(userId);
                MHQry = " SELECT DISTINCT MH.Mh_Id, Mh.Mh_Name, Mh.Mh_Seq, MH.Mh_Id, MH.Area FROM [Identity].UserRoles SRC,[Identity].Roles SR, Menu_Conf SMC, Menu_Head MH " +
                            " WHERE SRC.RoleId = SR.Id AND SMC.Role_Id = SRC.RoleId  And Mh.Mh_Id = SMC.Mh_Id AND SMC.Role_Id = '"+ roleIds + "'  ORDER BY Mh.Mh_Id ASC ";
            }

            using(var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<MenuViewModel>(MHQry);
                return result.ToList();
            }
        }
        public async Task<IList<SubMenuViewModel>> GetSubMenuAsync(int parentId, ClaimsPrincipal ctx)
        {
            string SMQry = " SELECT DISTINCT SSM.SM_NAME,SSM.SM_ID,SSM.SM_SEQ,SSM.SM_CSS_CLASS,SSM.URL,SMC.Mh_Id FROM[Identity].UserRoles SRC,[Identity].Roles SR, Menu_Conf SMC,Sub_Menu SSM " +
                           " WHERE SRC.RoleId = SR.Id AND SMC.Role_Id = SRC.RoleId AND SSM.SM_ID = SMC.SM_ID AND SMC.Mh_Id = "+ parentId + "  ORDER BY SSM.SM_SEQ ASC";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SubMenuViewModel>(SMQry);
                return result.ToList();
            }
        }



        private async Task<int> GetMenuHeadAsync(string roleIds)
        {
            var query = "SELECT * FROM Menu_Head WHERE Role_Id = '" + roleIds + "";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<string>(query, new { Id = roleIds });
                return Convert.ToInt32(result);
            }
        }


        public string GetUserRoleIdsAsync(string userId)
        {
            //var userId = GetUserId(ctx);
            //var query ="SELECT distinct U.UserName FROM[Identity].UserRoles UR, [Identity].Users U WHERE Ur.UserId = u.Id AND UserId = '"+ userId + "'";
            var query = "SELECT distinct UR.RoleId FROM[Identity].UserRoles UR, [Identity].Users U,[Identity].Roles R WHERE Ur.UserId = u.Id And ur.RoleId = r.Id AND UserId = '" + userId + "'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result =  connection.QuerySingleOrDefault<string>(query, new { Id = userId });
                return result;
            }
        }
        private async Task<string> GetUserRoleNameAsync(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            //var query ="SELECT distinct U.UserName FROM[Identity].UserRoles UR, [Identity].Users U WHERE Ur.UserId = u.Id AND UserId = '"+ userId + "'";
            var query = "SELECT distinct R.Name FROM[Identity].UserRoles UR, [Identity].Users U,[Identity].Roles R WHERE Ur.UserId = u.Id And ur.RoleId = r.Id AND UserId = '" + userId + "'";
            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<string>(query, new { Id = userId });
                return result;
            }
        }
        private string GetUserId(ClaimsPrincipal user)
        {
            return ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IEnumerable<SubMenuViewModel>> GetSubMenus( ClaimsPrincipal principal)
        {
            string SMQry = " SELECT DISTINCT SSM.SM_NAME,SSM.SM_ID,SSM.SM_SEQ,SSM.SM_CSS_CLASS,SSM.URL FROM[Identity].UserRoles SRC, " +
                            " [Identity].Roles SR, MENU_CONF SMC,SUB_MENU SSM  WHERE SRC.RoleId = SR.Id AND SMC.Role_Id = SRC.RoleId AND SSM.SM_ID = SMC.SM_ID ORDER BY SSM.SM_SEQ ASC ";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<SubMenuViewModel>(SMQry);
                return result.ToList();
            }
        }

        public async Task<IList<SubMenuViewModel>> GetMenuAsync(IList<MenuViewModel> menuList, IList<SubMenuViewModel> subMenuList, int parentId, ClaimsPrincipal ctx)
        {
            var menus = await GetMenuListAsync(menuList, ctx);


            if (!menus.Any())
            {
                return new List<SubMenuViewModel>();
            }

            var vmList = new List<SubMenuViewModel>();
            foreach (var item in menus)
            {
                var menu = await GetMenuItemAsync(menuList, item.Mh_Id, ctx);
                var vm = new SubMenuViewModel();
                vm.Mh_Id = menu.Mh_Id;
                vm.Mh_Name = menu.Mh_Name;
                vm.Area = menu.Area;
                vm.Children = await GetChildrenMenuAsync(subMenuList, item.Mh_Id, ctx);
                vmList.Add(vm);
            }
            return vmList;
        }


    }
}
