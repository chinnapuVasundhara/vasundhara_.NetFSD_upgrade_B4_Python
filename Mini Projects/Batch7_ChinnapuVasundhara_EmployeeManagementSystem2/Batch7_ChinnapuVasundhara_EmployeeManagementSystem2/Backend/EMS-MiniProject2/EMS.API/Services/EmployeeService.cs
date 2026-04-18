using EMS.API.DTOs;
using EMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.API.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<EmployeeResponseDto>> GetEmployeesAsync(
            string? search, string? department, string? status,
            string? sortBy, string? sortDir, int page, int pageSize)
        {
            var query = _repository.GetAllAsQueryable();

            // 1. Search (Matches FirstName + LastName combined, or Email)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                query = query.Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(term) ||
                                         e.Email.ToLower().Contains(term));
            }

            // 2. Filter (Exact match for Department and Status)
            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(e => e.Department == department);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(e => e.Status == status);
            }

            // 3. Sort (Executes as ORDER BY in SQL)
            bool isDesc = sortDir?.ToLower() == "desc";
            query = (sortBy?.ToLower()) switch
            {
                "salary" => isDesc ? query.OrderByDescending(e => e.Salary) : query.OrderBy(e => e.Salary),
                "joindate" => isDesc ? query.OrderByDescending(e => e.JoinDate) : query.OrderBy(e => e.JoinDate),
                // Default sort by Name (LastName then FirstName)
                _ => isDesc ? query.OrderByDescending(e => e.LastName).ThenByDescending(e => e.FirstName)
                            : query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
            };

            // 4. Pagination (Skip/Take executes in SQL)
            var totalCount = await query.CountAsync(); // Count before skipping
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // 5. Map to DTO
            var dtos = items.Select(MapToResponseDto).ToList();

            return new PagedResult<EmployeeResponseDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<object> GetDashboardSummaryAsync()
        {
            var query = _repository.GetAllAsQueryable();

            // KPIs computed via SQL COUNT()
            var total = await query.CountAsync();
            var active = await query.CountAsync(e => e.Status == "Active");
            var inactive = await query.CountAsync(e => e.Status == "Inactive");

            // Unique departments count
            var depts = await query.Select(e => e.Department).Distinct().CountAsync();

            // Department Breakdown computed via SQL GROUP BY
            var breakdown = await query.GroupBy(e => e.Department)
                .Select(g => new {
                    Department = g.Key,
                    Count = g.Count(),
                    Percentage = total == 0 ? 0 : Math.Round((double)g.Count() / total * 100)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            // Last 5 added employees
            var recent = await query.OrderByDescending(e => e.CreatedAt)
                                    .Take(5)
                                    .Select(e => MapToResponseDto(e))
                                    .ToListAsync();

            return new { total, active, inactive, departments = depts, breakdown, recent };
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var emp = await _repository.GetByIdAsync(id);
            return emp == null ? null : MapToResponseDto(emp);
        }

        public async Task<EmployeeResponseDto> CreateAsync(EmployeeRequestDto dto)
        {
            var emp = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary = dto.Salary,
                JoinDate = dto.JoinDate,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(emp);
            await _repository.SaveChangesAsync();
            return MapToResponseDto(emp);
        }

        public async Task<EmployeeResponseDto?> UpdateAsync(int id, EmployeeRequestDto dto)
        {
            var emp = await _repository.GetByIdAsync(id);
            if (emp == null) return null;

            emp.FirstName = dto.FirstName;
            emp.LastName = dto.LastName;
            emp.Email = dto.Email;
            emp.Phone = dto.Phone;
            emp.Department = dto.Department;
            emp.Designation = dto.Designation;
            emp.Salary = dto.Salary;
            emp.JoinDate = dto.JoinDate;
            emp.Status = dto.Status;
            emp.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(emp);
            await _repository.SaveChangesAsync();
            return MapToResponseDto(emp);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var emp = await _repository.GetByIdAsync(id);
            if (emp == null) return false;

            await _repository.DeleteAsync(emp);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _repository.EmailExistsAsync(email, excludeId);
        }

        // Helper method to keep DTO mapping consistent and clean
        private static EmployeeResponseDto MapToResponseDto(Employee e)
        {
            return new EmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Department = e.Department,
                Designation = e.Designation,
                Salary = e.Salary,
                JoinDate = e.JoinDate.ToString("yyyy-MM-dd"),
                Status = e.Status
            };
        }
    }
}