using AssetManager.Application.DTOs.Asset;
using AssetManager.Domain.Enums;

namespace AssetManager.Web.Models.Asset;

public class AssetListViewModel
{
    public IEnumerable<AssetResponseDto> Assets { get; set; } = [];
    public int TotalCount => Assets.Count();
    public string PageTitle { get; set; } = "Asset Inventory";

    // Ekranda "Durum" kısmını renkli göstermek için yardımcı bir metod
    public string GetStatusClass(AssetStatus status) => status switch
    {
        AssetStatus.Active => "bg-info",
        AssetStatus.InStock => "bg-success",
        AssetStatus.Assigned => "bg-primary",
        AssetStatus.Lost => "bg-danger",
        AssetStatus.InRepair => "bg-warning text-dark",
        AssetStatus.Retired => "bg-secondary",
        _ => "bg-dark"
    };
}