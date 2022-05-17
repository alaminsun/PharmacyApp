using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<CustomerModel>
    {
    }
}
