using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.Department
{
    public class CreateDepartmentRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
