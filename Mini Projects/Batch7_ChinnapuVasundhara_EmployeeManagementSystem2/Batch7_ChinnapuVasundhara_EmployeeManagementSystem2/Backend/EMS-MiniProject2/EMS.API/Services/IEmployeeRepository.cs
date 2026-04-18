using EMS.API.Models;

namespace EMS.API.Services
{
    public interface IEmployeeRepository
    {
        // Returns IQueryable so the Service layer can append Search/Filter/Sort before executing the SQL
        IQueryable<Employee> GetAllAsQueryable();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task SaveChangesAsync();
    }
}