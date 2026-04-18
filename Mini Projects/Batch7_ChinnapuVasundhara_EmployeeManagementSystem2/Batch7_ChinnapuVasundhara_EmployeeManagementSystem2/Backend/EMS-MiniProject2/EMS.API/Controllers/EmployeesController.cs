using EMS.API.DTOs;
using EMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires a valid JWT token!
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeesController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search, [FromQuery] string? department, [FromQuery] string? status,
            [FromQuery] string sortBy = "name", [FromQuery] string sortDir = "asc",
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Cap page size to 100 for safety
            pageSize = pageSize > 100 ? 100 : pageSize;
            var result = await _service.GetEmployeesAsync(search, department, status, sortBy, sortDir, page, pageSize);
            return Ok(result);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            return Ok(await _service.GetDashboardSummaryAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var emp = await _service.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        // --- ADMIN ONLY ENDPOINTS BELOW ---

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(EmployeeRequestDto request)
        {
            if (await _service.EmailExistsAsync(request.Email))
                return Conflict(new { message = "Email already exists." });

            var newEmp = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newEmp.Id }, newEmp);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, EmployeeRequestDto request)
        {
            if (await _service.EmailExistsAsync(request.Email, id))
                return Conflict(new { message = "Email is already in use by another employee." });

            var updatedEmp = await _service.UpdateAsync(id, request);
            if (updatedEmp == null) return NotFound();

            return Ok(updatedEmp);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}