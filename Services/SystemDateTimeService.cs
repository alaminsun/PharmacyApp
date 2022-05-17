
using PhramacyApp.Interfaces.Shared;
using System;

namespace PharmacyApp.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}