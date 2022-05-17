using AspNetCoreHero.EntityFrameworkCore.AuditTrail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhramacyApp.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<int> UpdateAsync(int id, T entity);
        Task<int> DeleteAsync(int id);
    }
}
