using AssetManager.Application.Features.Department.Commands.CreateDepartment;
using AssetManager.Application.Features.Department.Commands.DeleteDepartment;
using AssetManager.Application.Features.Department.Queries.GetAllDepartments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.API.Controllers;

[Authorize]
[Route("api/departments")]
public class DepartmentsController : BaseController
{
    // Tüm departmanları listele
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllDepartmentsQuery());
        return Ok(result);
    }

    // Yeni departman ekle (Sadece Admin)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command)
    {
        var result = await Mediator.Send(command);
        return result
            ? Ok(new { message = "Department is added." })
            : BadRequest();
    }

    // Departman sil (Sadece Admin)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteDepartmentCommand(id));
        return result
            ? Ok(new { message = "Department is deleted." })
            : NotFound();
    }
}