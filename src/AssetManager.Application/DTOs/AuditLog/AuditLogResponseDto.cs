using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.AuditLog
{
    public class AuditLogResponseDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public DateTime TimestampUtc { get; set; }
    }
}
