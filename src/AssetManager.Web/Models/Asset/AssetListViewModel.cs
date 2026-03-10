using AssetManager.Application.DTOs.Asset;

namespace AssetManager.Web.Models.Asset;

public class AssetListViewModel
{
    public IEnumerable<AssetResponseDto> Assets { get; set; } = [];
    public int TotalCount => Assets.Count();
    public string PageTitle { get; set; } = "Asset Inventory";

    // Ekranda "Durum" kısmını renkli göstermek için yardımcı bir metod
    public string GetStatusClass(string status) => status switch
    {
        "Active" => "badge-success",
        "Archived" => "badge-danger",
        _ => "badge-secondary"
    };
}