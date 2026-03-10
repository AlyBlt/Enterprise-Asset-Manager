using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.Dashboard
{
    public class RecentActivityDto
    {
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty; // Create, Delete, Assign vb.
    }
}
