using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.Asset
{
    public class CreateAssetRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } // Entity'deki Value'ya gidecek
        public string Category { get; set; } = string.Empty; 
        public string SerialNumber { get; set; } = string.Empty;
    }
}
