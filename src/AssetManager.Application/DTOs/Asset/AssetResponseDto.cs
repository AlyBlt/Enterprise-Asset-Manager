using AssetManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.Asset
{
    public class AssetResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public AssetStatus Status { get; set; }
        public string? AssignedUserName { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
    }
}
