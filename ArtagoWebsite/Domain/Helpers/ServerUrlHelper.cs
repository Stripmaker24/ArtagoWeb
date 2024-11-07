using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Domain.Helpers
{
    public static class ServerUrlHelper
    {
        public static string GetServerUrl(this IServer server)
        {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDev)
            {
                return "https://localhost:7208";
            }
            string url = server.Features?.Get<IServerAddressesFeature>()?.Addresses?.LastOrDefault() ?? string.Empty;
            url = url.Replace(":80", string.Empty).Replace(":443", string.Empty);

            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            return url;
        }
    }
}
