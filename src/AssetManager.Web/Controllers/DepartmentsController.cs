using AssetManager.Application.Features.Department.Commands.CreateDepartment;
using AssetManager.Application.Features.Department.Commands.UpdateDepartment;
using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManager.Web.Controllers;

[Authorize]
public class DepartmentsController(IDepartmentApiService departmentApiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var departments = await departmentApiService.GetAllAsync();
        var model = new DepartmentIndexViewModel { Departments = departments };
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View(new DepartmentCreateViewModel());

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(DepartmentCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await departmentApiService.CreateAsync(new CreateDepartmentCommand(
        model.Name,
        model.Description
        ));

        if (result) return RedirectToAction(nameof(Index));

        ModelState.AddModelError("", "Error while creating department.");
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await departmentApiService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var department = await departmentApiService.GetByIdAsync(id);

        if (department == null) return NotFound();

        var viewModel = new DepartmentDetailsViewModel
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            UserCount = department.UserCount
        };

        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Editor")]
    public async Task<IActionResult> Edit(int id)
    {
        // API'den tekil departmanı çekiyoruz
        var department = await departmentApiService.GetByIdAsync(id);

        if (department == null) return NotFound();

        var model = new DepartmentUpdateViewModel
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description
        };

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Editor")]
    public async Task<IActionResult> Edit(DepartmentUpdateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // ViewModel'den Command'e map ediyoruz
        var command = new UpdateDepartmentCommand(model.Id, model.Name, model.Description);

        var result = await departmentApiService.UpdateAsync(command);

        if (result) return RedirectToAction(nameof(Index));

        ModelState.AddModelError("", "An error occurred while updating the department.");
        return View(model);
    }
}