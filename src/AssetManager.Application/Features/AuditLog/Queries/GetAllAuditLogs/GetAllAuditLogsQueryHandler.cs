using AssetManager.Application.DTOs.AuditLog;
using AssetManager.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Features.AuditLog.Queries.GetAllAuditLogs
{
    public class GetAllAuditLogsQueryHandler(
     IAuditLogRepository auditLogRepository,
     IMapper mapper) : IRequestHandler<GetAllAuditLogsQuery, IEnumerable<AuditLogResponseDto>>
    {
        public async Task<IEnumerable<AuditLogResponseDto>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var logs = await auditLogRepository.GetAllLogsAsync();
            return mapper.Map<IEnumerable<AuditLogResponseDto>>(logs);
        }
    }
}
