using PhramacyApp.Areas.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PhramacyApp.Services
{
    public interface IProductService
    {
        public Task<DataTableResponse<MedicineModel>> GetProductsAsync(DataTableRequest request);
    }
}
