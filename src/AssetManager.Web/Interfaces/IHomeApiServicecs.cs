using AssetManager.Web.Models.Home;

namespace AssetManager.Web.Interfaces
{
    public interface IHomeApiService
    {
        Task<SystemInfoViewModel?> GetSystemInfoAsync();
        Task<bool> IsApiHealthyAsync(); //API ayakta mı kontrolü
    }
}
