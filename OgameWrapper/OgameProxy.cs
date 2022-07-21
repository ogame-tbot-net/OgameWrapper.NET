using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OgameWrapper
{
    public class OgameProxy
    {
        private HttpListener Listener { get; init; } = new();

        private const string COOKIE_BANNER_NAME = "gf-cookie-consent-4449562312";
        private const string COOKIE_BANNER_VALUE = "|0|1";
        
        public OgameProxy(string host = "localhost", int port = 1337)
        {
            Listener.Prefixes.Add($"http://{host}:{port}/");
        }

        public async void Start(OgameClient ogameClient)
        {
            try
            {
                Listener.Start();

                while (true)
                {
                    var context = await Listener.GetContextAsync();

                    using BinaryWriter stream = new(context.Response.OutputStream);

                    var response = await ogameClient.ExecuteRequest(context.Request);

                    if (response.ProtocolVersion != null)
                    {
                        context.Response.ProtocolVersion = response.ProtocolVersion;
                    }
                    context.Response.StatusCode = (int)response.StatusCode;
                    context.Response.ContentType = response.ContentType;
                    if (response.ContentLength > 0)
                    {
                        context.Response.ContentLength64 = response.ContentLength;
                    }

                    WebHeaderCollection headers = new();
                    foreach (var header in response.Headers)
                    {
                        if (string.IsNullOrEmpty(header.Name) || string.IsNullOrEmpty((string?)header.Value))
                        {
                            continue;
                        }

                        headers.Add(header.Name, header.Value.ToString());
                    }
                    context.Response.Headers = headers;

                    // Remove GRPD cookie banner
                    if (!context.Request.Cookies.Any(cookie => cookie.Name == COOKIE_BANNER_NAME))
                    {
                        context.Response.SetCookie(new(COOKIE_BANNER_NAME, COOKIE_BANNER_VALUE, "/"));
                    }

                    var responseBytes = response.RawBytes;

                    // Replace server host with proxy host
                    if (response.Content.Contains(ogameClient.ServerHost))
                    {
                        if (context.Request.Url != null)
                        {
                            var newHost = context.Request.Url.Authority;
                            var responseText = response.Content
                                .Replace(ogameClient.ServerHost, newHost)
                                .Replace($"https://{newHost}", $"http://{newHost}")
                                .Replace($"https:\\/\\/{newHost}", $"http:\\/\\/{newHost}");

                            responseBytes = Encoding.UTF8.GetBytes(responseText);
                        }
                    }

                    // TODO : make stream async
                    stream.Write(responseBytes);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
