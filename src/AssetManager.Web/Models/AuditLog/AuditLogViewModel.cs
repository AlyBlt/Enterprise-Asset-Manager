using AssetManager.Application.DTOs.AuditLog;

namespace AssetManager.Web.Models.AuditLog
{
    public class AuditLogViewModel
    {
        public IEnumerable<AuditLogResponseDto> Logs { get; set; } = [];
        public string PageTitle { get; set; } = "System Audit Logs";
    }
}
