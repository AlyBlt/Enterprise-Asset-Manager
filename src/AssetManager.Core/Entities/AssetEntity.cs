using AssetManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Core.Entities
{
    public class AssetEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Laptop, Monitor, Software License vb.
        public string SerialNumber { get; set; } = string.Empty;
        public decimal Value { get; set; } // Varlığın maddi değeri 
        public AssetStatus Status { get; set; } = AssetStatus.InStock;

        // İlişki: Varlık bir kullanıcıya zimmetlenebilir
        public int? AssignedUserId { get; set; }
        public AppUserEntity? AssignedUser { get; set; }
    }
}
