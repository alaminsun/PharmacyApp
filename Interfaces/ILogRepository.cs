
using AspNetCoreHero.EntityFrameworkCore.AuditTrail.Models;
using PhramacyApp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces.Repositories
{
    public interface ILogRepository 
    {
        Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId);
        Task AddLogAsync(string action, string userId);
    }
}