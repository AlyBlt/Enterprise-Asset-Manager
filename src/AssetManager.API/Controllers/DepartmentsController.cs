using AssetManager.Application.DTOs.Department;
using AssetManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentsController(IDepartmentService departmentService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await departmentService.GetAllDepartmentsAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequestDto request)
        {
            var result = await departmentService.CreateDepartmentAsync(request);
            return result ? Ok(new { message = "Department is added." }) : BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await departmentService.DeleteDepartmentAsync(id);
            return result ? Ok(new { message = "Department is deleted." }) : NotFound();
        }
    }
}
