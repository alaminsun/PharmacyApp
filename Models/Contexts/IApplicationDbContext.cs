using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PhramacyApp.Models.Contexts
{
    public interface IApplicationDbContext
    {
        //bool HasChanges { get; }

        //EntityEntry Entry(object entity);
        public IDbConnection Connection { get; }  
        //DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        //Task GetMenuItemsAsync(ClaimsPrincipal user);
    }
}