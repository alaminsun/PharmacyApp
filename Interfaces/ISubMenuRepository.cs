using PhramacyApp.Areas.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces.Repositories
{
    public interface ISubMenuRepository : IGenericRepository<SubMenuViewModel>
    {
        Task<IEnumerable<SubMenuViewModel>> GetAllSubMenuByMenuId(string Role_Id, int mhId);
    }
}
