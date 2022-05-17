using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.EntityFrameworkCore.AuditTrail.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhramacyApp.DbContexts;
using PhramacyApp.Interfaces;
using PhramacyApp.Interfaces.Repositories;
using PhramacyApp.Interfaces.Shared;
using PhramacyApp.Models;

namespace PhramacyApp.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration configuration;

        public LogRepository(IConfiguration configuration, ApplicationDbContext dbContext, IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
            this.configuration = configuration;
        }

        public async Task AddLogAsync(string action, string userId)
        {

            var audit = new Audit()
            {
                Type = action,
                UserId = userId,
                DateTime = _dateTimeService.NowUtc
            };
            await _dbContext.AddAsync(audit);

            await _dbContext.SaveChangesAsync();

        }

        //public async Task<List<AuditLogResponse>> GetAuditLogsAsync(List<Audit> audits, string userId)
        //{
        //    var logs = await _dbContext.audits.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
        //    var mappedLogs = logs;
        //    return mappedLogs.;
        //}

        public async Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId)
        {
            List<AuditLogResponse> loglist = new List<AuditLogResponse>();
            var query = "SELECT TOP 250 * FROM AuditLogs Where UserId = '" + userId + "' order by Id Desc";

            using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<AuditLogResponse>(query);
                foreach (var model in result)
                {
                    AuditLogResponse log = new AuditLogResponse();

                    log.Id = model.Id;
                    log.UserId = model.UserId;
                    log.Type = model.Type;
                    log.TableName = model.TableName;
                    log.DateTime = model.DateTime;
                    log.OldValues = model.OldValues;
                    log.NewValues = model.NewValues;
                    log.AffectedColumns = model.AffectedColumns;
                    log.PrimaryKey = model.PrimaryKey;
                    loglist.Add(log);
                }
                return result.ToList();
            }
            //var logs = await _dbContext.AuditLogs.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            //var model = logs;

            //return PartialView("_ViewAll", userList);
            //var logs = await _dbContext.Entities.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            //var mappedLogs = _mapper.Map<List<AuditLogResponse>>(logs);
            //return loglist;
        }

        //public async Task<List<AuditLogResponse>> GetAuditLogsAsync(string userId)
        //{
        //    var query = "SELECT TOP 250 * FROM AuditLogs Where UserId = '" + userId + "' order by Id Desc";

        //    using (var connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<AuditLogResponse>(query);
        //        return result.ToList();
        //    }
        //}
        //public async Task<List<AuditLogResponse>> GetAllLogss(string userId)
        //{
        //    var query = "SELECT TOP 250 * FROM AuditLogs Where UserId = '" + userId + "' order by Id Desc";

        //    using (var connection = _dbContext.Connection)
        //    {
        //        var logs = await connection.QueryAsync<AuditLogResponse>(query);
        //        return logs.ToList();
        //    }
        //}
    }


}