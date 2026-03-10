namespace AssetManager.Web.Models.Home
{
    public class SystemInfoViewModel
    {
        public string Student { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string ServerTimeUtc { get; set; } = string.Empty;
        public bool IsApiOnline { get; set; } // Dashboard'da yeşil/kırmızı ışık için
    }
}
