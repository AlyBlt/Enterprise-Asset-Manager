using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Core.Entities
{
    public class AuditLogEntity
    {
        public int Id { get; set; }
        public string? UserName { get; set; } // İşlemi yapan kullanıcı
        public string Action { get; set; } = string.Empty; // Create, Update, Delete
        public string EntityName { get; set; } = string.Empty; // Asset
        public string EntityId { get; set; } = string.Empty; // İşlem gören varlığın ID'si
        public string Details { get; set; } = string.Empty; // "Serial number changed from X to Y"
        public string IpAddress { get; set; } = string.Empty;
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }
}
