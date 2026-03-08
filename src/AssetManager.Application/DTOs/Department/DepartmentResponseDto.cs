using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.Department
{
    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserCount { get; set; } 
    }
}
