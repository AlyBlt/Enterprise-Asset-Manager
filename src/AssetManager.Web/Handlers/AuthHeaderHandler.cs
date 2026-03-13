using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;

namespace AssetManager.Web.Handlers;

public class AuthHeaderHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var context = httpContextAccessor.HttpContext;
            if (context != null)
            {
                // Çıkış yap
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Login'e yönlendir (Kullanıcı "Boş Liste" yerine Login sayfasını görür)
                context.Response.Redirect("/Account/Login");
            }
        }

        return response;
    }
}