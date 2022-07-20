using OgameWrapper.Model;
using RestSharp;
using System.Net;

namespace OgameWrapper.Services
{
    internal static class ServiceFactory
    {
        internal static RestClient HttpClient
        {
            get
            {
                // TODO : use configuration settings
                var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:103.0) Gecko/20100101 Firefox/103.0";
                var proxyAddress = "";
                var proxyPort = 0;
                NetworkCredential? proxyCredentials = null;

                RestClient client = new()
                {
                    UserAgent = userAgent,
                    CookieContainer = new(),
                    Timeout = 30000,
                };

                client.AddDefaultHeader("Accept", "gzip, deflate, br");

                if (!string.IsNullOrEmpty(proxyAddress) && proxyPort > 0)
                {
                    client.Proxy = new WebProxy($"{proxyAddress}:{proxyPort}")
                    {
                        Credentials = proxyCredentials,
                    };
                }

                return client;
            }
        }
    }
}
