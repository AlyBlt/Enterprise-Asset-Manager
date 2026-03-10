using System.ComponentModel.DataAnnotations;
using AssetManager.Application.DTOs.Department;

namespace AssetManager.Web.Models.Department;

public class DepartmentIndexViewModel
{
    public IEnumerable<DepartmentResponseDto> Departments { get; set; } = [];
    public string PageTitle { get; set; } = "Department Management";
}