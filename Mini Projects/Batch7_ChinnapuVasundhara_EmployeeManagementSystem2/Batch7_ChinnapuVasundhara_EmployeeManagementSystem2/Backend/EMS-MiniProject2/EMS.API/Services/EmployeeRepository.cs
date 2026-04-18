using EMS.API.Data;
using EMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.API.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Employee> GetAllAsQueryable() => _context.Employees.AsQueryable();

        public async Task<Employee?> GetByIdAsync(int id) => await _context.Employees.FindAsync(id);

        public async Task AddAsync(Employee employee) => await _context.Employees.AddAsync(employee);

        public Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            return Task.CompletedTask;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _context.Employees.Where(e => e.Email == email);
            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}