using AssetManager.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }
}
