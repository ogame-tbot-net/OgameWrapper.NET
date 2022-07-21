using OgameWrapper.Includes;
using OgameWrapper.Model;
using OgameWrapper.Services;
using RestSharp;
using System.Net;

namespace OgameWrapper
{
    internal class Page
    {
        public readonly string Value;

        public Page(string value)
        {
            Value = value;
        }

        public static Page LobbyLogin = new("/game/lobbylogin.php?id={0}&token={1}");
        public static Page Overview = new("/game/index.php?page=ingame&component=overview");
        public static Page Rewards = new("/game/index.php?page=rewards");
        public static Page Supplies = new("/game/index.php?page=ingame&component=supplies");
        public static Page ResourceSettings = new("/game/index.php?page=ingame&component=resourceSettings&cp={0}");
        public static Page Facilities = new("/game/index.php?page=ingame&component=facilities");
        public static Page TraderOverview = new("/game/index.php?page=ingame&component=traderOverview");
        public static Page Research = new("/game/index.php?page=ingame&component=research");
        public static Page Shipyard = new("/game/index.php?page=ingame&component=shipyard");
        public static Page Defenses = new("/game/index.php?page=ingame&component=defenses");
        public static Page FleetDispatch = new("/game/index.php?page=ingame&component=fleetdispatch");
        public static Page Movement = new("/game/index.php?page=ingame&component=movement");
        public static Page Galaxy = new("/game/index.php?page=ingame&component=galaxy");
        public static Page Alliance = new("/game/index.php?page=ingame&component=alliance");
        public static Page Premium = new("/game/index.php?page=ingame&component=premium");
        public static Page Shop = new("/game/index.php?page=ingame&component=shop");

        public static Page FetchCelestialResources = new("/game/index.php?page=fetchResources&ajax=1&cp={0}");
        public static Page FetchTechs = new("/game/index.php?page=fetchTechs&ajax=1");
        public static Page FetchCelestialTechs = new("/game/index.php?page=fetchTechs&ajax=1&cp={0}");
    }

    internal class CacheEntry
    {
        public Uri Uri { get; set; } = new("about:blank");

        public DateTime Date { get; set; } = DateTime.MinValue;

        public IRestResponse<object> Response { get; set; } = new RestResponse<object>();
    }

    public class OgameClient
    {
        private const uint CACHE_TIME_SECONDS = 60;

        private Account Account { get; init; }

        private LobbyClient LobbyClient { get; init; }

        private RestClient HttpClient { get; init; }

        private Dictionary<string, CacheEntry> Cache { get; init; }

        private static readonly List<string> ForwardHeaders = new()
        {
            "x-requested-with"
        };

        public string ServerHost
        {
            get
            {
                return $"s{Account.Server.Number}-{Account.Server.Language}.ogame.gameforge.com";
            }
        }

        public OgameClient(LobbyClient lobbyClient, Account account)
        {
            LobbyClient = lobbyClient;
            Account = account;

            HttpClient = ServiceFactory.HttpClient;
            HttpClient.BaseUrl = new($"https://{ServerHost}");

            Cache = new();
        }

        public async Task Login()
        {
            var token = await LobbyClient.GetServerToken(Account);

            var url = string.Format(Page.LobbyLogin.Value, Account.GameAccountId, token);
            RestRequest request = new(url);

            var response = await HttpClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to login to server : Invalid status code {response.StatusCode}.");
            }

            var uri = response.ResponseUri;
            if (uri.Host != ServerHost)
            {
                throw new Exception($"Unable to login to server : Invalid host {uri.Host}.");
            }
        }

        public async Task<IRestResponse> ExecuteRequest(HttpListenerRequest inputRequest)
        {
            if (inputRequest.Url == null)
            {
                return new RestResponse();
            }

            var uri = inputRequest.Url;
            var method = StringMethodToMethod(inputRequest.HttpMethod);

            RestRequest request = new(uri.PathAndQuery, method);

            if (request.Method == Method.POST && inputRequest.ContentType != null)
            {
                using StreamReader stream = new(inputRequest.InputStream, inputRequest.ContentEncoding);
                var body = await stream.ReadToEndAsync();
                request.AddParameter(inputRequest.ContentType, body, ParameterType.RequestBody);
            }

            foreach (Cookie cookie in inputRequest.Cookies)
            {
                request.AddCookie(cookie.Name, cookie.Value);
            }

            foreach (string headerName in inputRequest.Headers)
            {
                if (ForwardHeaders.Contains(headerName.ToLower()))
                {
                    var headerValue = inputRequest.Headers[headerName];
                    if (headerValue == null)
                    {
                        continue;
                    }
                    request.AddHeader(headerName, headerValue);
                }
            }

            return await ExecuteRequestAsync(request);
        }

        public async Task<bool> IsInVacationMode(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.IsInVacationMode(response.Content);
        }

        public async Task<string> GetPlayerName(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetPlayerName(response.Content);
        }

        public async Task<int> GetPlayerId(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetPlayerID(response.Content);
        }

        public async Task<string> GetServerName(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetServerName(response.Content);
        }

        public async Task<int> GetCurrentCelestialId(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetCurrentCelestialID(response.Content);
        }

        public async Task<string> GetCurrentCelestialName(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetCurrentCelestialName(response.Content);
        }

        public async Task<Coordinate?> GetCurrentCelestialCoordinates(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetCurrentCelestialCoordinates(response.Content);
        }

        public async Task<int> GetEconomySpeed(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetEconomySpeed(response.Content);
        }

        public async Task<int> GetFleetSpeedPeaceful(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetFleetSpeedPeaceful(response.Content);
        }

        public async Task<int> GetFleetSpeedWar(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetFleetSpeedWar(response.Content);
        }

        public async Task<int> GetFleetSpeedHolding(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetFleetSpeedHolding(response.Content);
        }

        public async Task<PlayerClasses> GetPlayerClass(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetPlayerClass(response.Content);
        }

        public async Task<bool> IsUnderAttack(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.IsUnderAttack(response.Content);
        }

        public async Task<Staff> GetStaff(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetStaff(response.Content);
        }

        public async Task<List<Celestial>> GetCelestials(bool useCache = true)
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetCelestials(response.Content);
        }

        public async Task<Researches> GetResearches(bool useCache = true)
        {
            RestRequest request = new(Page.FetchTechs.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetTechs(response.Content).Researches;
        }

        public async Task<Techs> GetTechs(Celestial celestial, bool useCache = true)
        {
            var url = string.Format(Page.FetchCelestialTechs.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetTechs(response.Content);
        }

        public async Task<Buildings> GetBuildings(Celestial celestial, bool useCache = true)
        {
            var techs = await GetTechs(celestial, useCache);
            return techs.Buildings;
        }

        public async Task<Facilities> GetFacilities(Celestial celestial, bool useCache = true)
        {
            var techs = await GetTechs(celestial, useCache);
            return techs.Facilities;
        }

        public async Task<LifeformBuildings?> GetLifeformBuildings(Celestial celestial, bool useCache = true)
        {
            var techs = await GetTechs(celestial, useCache);
            return techs.LifeformBuildings;
        }

        public async Task<LifeformResearches?> GetLifeformResearches(Planet planet, bool useCache = true)
        {
            var techs = await GetTechs(planet, useCache);
            return techs.LifeformResearches;
        }

        public async Task<Ships> GetShips(Celestial celestial, bool useCache = true)
        {
            var techs = await GetTechs(celestial, useCache);
            return techs.Ships;
        }

        public async Task<Defences> GetDefences(Celestial celestial, bool useCache = true)
        {
            var techs = await GetTechs(celestial, useCache);
            return techs.Defences;
        }

        public async Task<Resources> GetResources(Celestial celestial, bool useCache = true)
        {
            var url = string.Format(Page.FetchCelestialResources.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetResources(response.Content);
        }

        public async Task<ResourceSettings> GetResourceSettings(Celestial celestial, bool useCache = true)
        {
            var url = string.Format(Page.ResourceSettings.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetResourcesSettings(response.Content);
        }

        public async Task<ResourcesProduction> GetResourcesProduction(Celestial celestial, bool useCache = true)
        {
            var url = string.Format(Page.FetchCelestialResources.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetResourcesProduction(response.Content);
        }

        public async Task<Slots> GetSlots(bool useCache = true)
        {
            RestRequest request = new(Page.FleetDispatch.Value);
            var response = await ExecuteRequestAsync(request, useCache);
            return Extractor.GetSlots(response.Content);
        }

        private Task<IRestResponse<object>> ExecuteRequestAsync(IRestRequest request, bool useCache = false)
        {
            return ExecuteRequestAsync<object>(request, useCache);
        }

        private async Task<IRestResponse<T>> ExecuteRequestAsync<T>(IRestRequest request, bool useCache = false)
        {
            var uri = HttpClient.BuildUri(request);

            if (useCache && request.Method == Method.GET)
            {
                if (Cache.ContainsKey(uri.PathAndQuery))
                {
                    var cachedResponse = Cache[uri.PathAndQuery];
                    if (cachedResponse.Date.AddSeconds(CACHE_TIME_SECONDS) > DateTime.UtcNow)
                    {
                        return (IRestResponse<T>) cachedResponse.Response;
                    }
                }
            }

            var response = await HttpClient.ExecuteAsync<T>(request);
            var responseUri = response.ResponseUri;
            if (responseUri.Host != ServerHost || response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Login();

                return await ExecuteRequestAsync<T>(request);
            }

            if (useCache && request.Method == Method.GET)
            {
                Cache[uri.PathAndQuery] = new()
                {
                    Uri = uri,
                    Date = DateTime.UtcNow,
                    Response = (IRestResponse<object>) response,
                };
            }

            return response;
        }

        private static Method StringMethodToMethod(string method)
        {
            return method switch
            {
                "GET" => Method.GET,
                "POST" => Method.POST,
                "PUT" => Method.PUT,
                "DELETE" => Method.DELETE,
                "HEAD" => Method.HEAD,
                "OPTIONS" => Method.OPTIONS,
                "PATCH" => Method.PATCH,
                "MERGE" => Method.MERGE,
                "COPY" => Method.COPY,
                _ => Method.GET
            };
        }

    }
}
