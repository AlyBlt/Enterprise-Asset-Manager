using System.Net.Http.Headers;

public abstract class BaseApiService
{
    protected readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    protected void AddAuthorizationHeader()
    {
        // NOT: Dashboard gibi herkese açık veya login sonrası verilerde 
        // bu metodu servis içindeki istekten hemen önce çağırmayı unutma.
        var token = _httpContextAccessor.HttpContext?.User.FindFirst("AccessToken")?.Value;
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}