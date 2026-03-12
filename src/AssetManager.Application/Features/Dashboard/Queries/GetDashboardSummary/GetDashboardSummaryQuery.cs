using AssetManager.Application.DTOs.Dashboard;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.Dashboard.Queries.GetDashboardSummary
{
    public record GetDashboardSummaryQuery() : IRequest<DashboardSummaryDto>;
}
