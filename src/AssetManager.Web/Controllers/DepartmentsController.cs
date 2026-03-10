using AssetManager.Application.DTOs.Department;
using AssetManager.Web.Interfaces;
using AssetManager.Web.Models.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        var result = await departmentApiService.CreateAsync(new CreateDepartmentRequestDto 
        { Name = model.Name,
          Description = model.Description
        });

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
        var departments = await departmentApiService.GetAllAsync();
        var department = departments.FirstOrDefault(d => d.Id == id);

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
}