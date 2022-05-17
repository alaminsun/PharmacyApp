
using Microsoft.EntityFrameworkCore;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.ExternalLoginModel;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.EntityFrameworkCore.AuditTrail;

using PhramacyApp.Interfaces.Shared;
using PhramacyApp.Models.Contexts;
using System.Threading.Tasks;
using System.Threading;
using AspNetCoreHero.Abstractions.Domain;

namespace PhramacyApp.DbContexts
{
    public class ApplicationDbContext : AuditableContext, IApplicationDbContext
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        private readonly IDbConnection connection;
        public ApplicationDbContext(IConfiguration configuration,DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ApplicationConnection");
            connection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection"));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IDbConnection Connection => new SqlConnection(_connectionString);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            if (_authenticatedUser.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_authenticatedUser.UserId);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);
        }

       
    }
}