using PhramacyApp.Areas.Master.Models;
using PhramacyApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IMedicineRepository _medicineRepository;

        public ProductService(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public async Task<DataTableResponse<MedicineModel>> GetProductsAsync(DataTableRequest request)
        {
            var req = new ProductListRequest()
            {
                PageNo = request.Start,
                PageSize = request.Length,
                SortColumn = request.Order[0].Column,
                SortDirection = request.Order[0].Dir,
                SearchValue = request.Search != null ? request.Search.Value.Trim() : ""
            };

            var products = await _medicineRepository.GetProductsAsync(req);

            return new DataTableResponse<MedicineModel>()
            {
                Draw = request.Draw,
                RecordsTotal = products[0].TotalCount,
                RecordsFiltered = products[0].FilteredCount,
                Data = products.ToArray(),
                Error = ""
            };

        }
    }
}
