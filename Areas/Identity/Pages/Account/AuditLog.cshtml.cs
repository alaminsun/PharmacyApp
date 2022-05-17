

using Microsoft.AspNetCore.Mvc.RazorPages;
using PhramacyApp.DbContexts;
using PhramacyApp.Interfaces;
using PhramacyApp.Interfaces.Repositories;
using PhramacyApp.Interfaces.Shared;
using PhramacyApp.Models;
using System.Collections.Generic;

using System.Threading.Tasks;


namespace PharmacyApp.Areas.Identity.Pages.Account
{
    public class AuditLogModel : PageModel
    {
        //private readonly IMediator _mediator;
        private readonly IAuthenticatedUserService _userService;
        public List<AuditLogResponse> AuditLogResponses;
        private IViewRenderService _viewRenderer;
        private readonly ILogRepository _logRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;

        public AuditLogModel(ILogRepository logRepository, IAuthenticatedUserService userService, IViewRenderService viewRenderer)
        {
            _logRepository = logRepository;
            _userService = userService;
            _viewRenderer = viewRenderer;
        }



        public async Task OnGet()
        {
            //var response = await ;
            //AuditLogResponses = response.Data;
            string userId = _userService.UserId;
            //var response = await _logRepository.GetAllLogss(userId);
            var response = await _logRepository.GetAuditLogsAsync(userId);
            AuditLogResponses = response;
        }
    }
}