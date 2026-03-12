using AssetManager.Application.DTOs.AuditLog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.AuditLog.Queries.GetAllAuditLogs
{
    public record GetAllAuditLogsQuery() : IRequest<IEnumerable<AuditLogResponseDto>>;
}
