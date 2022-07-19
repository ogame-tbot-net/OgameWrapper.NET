using Newtonsoft.Json;
using RestSharp;
using System.Net;
using OgameWrapper.Model;
using OgameWrapper.Includes;
using JsonFlatFileDataStore;
using System.Text.RegularExpressions;

namespace OgameWrapper
{
    public class OgameWrapperClient
    {
        public bool IsLoggedIn { get; private set; }

        public IDocumentCollection<Celestial> Celestials { get; private set; }

        public ServerInfo ServerInfo { get; private set; }

        private RestClient Client { get; set; }

        private Credentials Credentials { get; set; }

        private DataStore Database { get; set; }

        private string LastPage { get; set; } = string.Empty;

        private string LastUrl { get; set; } = string.Empty;

        private const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36";

        private string UniverseBaseUrl
        {
            get
            {
                return $"https://s{Credentials.Number}-{Credentials.Language.ToLower()}.ogame.gameforge.com/game/index.php?";
            }
        }

        public OgameWrapperClient(Credentials credentials, string? userAgent = null, Proxy? proxy = null)
        {
            IsLoggedIn = false;
            Database = new DataStore("data.json");

            if (Database.TryGetItem<Credentials>("credentials", out var creds) && creds.Token != null)
            {
                // NOTE : we might want to check if credentials.Email matches creds.Email to avoid wrong token use on account change
                Credentials = new(credentials.Email, credentials.Password, credentials.Language, credentials.Number)
                {
                    Token = creds.Token
                };
            }
            else
            {
                Credentials = credentials;
                if (credentials == null)
                {
                    throw new ArgumentNullException(nameof(credentials));
                }
            }

            Client = new()
            {
                UserAgent = userAgent ?? DEFAULT_USER_AGENT,
                CookieContainer = new(),
                Timeout = 30000
            };

            Client.AddDefaultHeader("Accept", "gzip, deflate, br");

            if (proxy != null)
            {
                SetProxy(proxy);
            }

            Celestials = Database.GetCollection<Celestial>("Celestials");

            if (Database.TryGetItem<ServerInfo>("serverInfo", out var si))
            {
                ServerInfo = si;
            }
            else
            {
                ServerInfo = new();
                Database.InsertItem<ServerInfo>("serverInfo", ServerInfo);
            }
        }

        private async Task<int?> GetPlayerIdFromLobby()
        {
            RestRequest request = new("https://lobby.ogame.gameforge.com/api/users/me/accounts");
            var response = await ExecuteRequest<List<AccountInfo>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            // NOTE : note sure we need `GetServerInfo`, we have server number in `Credentials.Number`

            var server = await GetServerInfo();
            if (server != null)
            {
                var account = response.Data
                    .Where(a => a.Server.Language == Credentials.Language)
                    .Where(a => a.Server.Number == server.Number)
                    .FirstOrDefault() ?? null;

                return account?.Id;
            }
            
            return null;
        }

        private async Task<List<ServerInfo>> GetServers()
        {
            RestRequest request = new("https://lobby.ogame.gameforge.com/api/servers");
            var response = await ExecuteRequest<List<ServerInfo>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new();
            }
            return response.Data;
        }

        public async Task<ServerInfo?> GetServerInfo()
        {
            var servers = await GetServers();
            var thisServer = servers
                .Where(s => s.Language == Credentials.Language)
                .Where(s => s.Number == Credentials.Number)
                .FirstOrDefault(new ServerInfo());

            Database.ReplaceItem("serverInfo", thisServer, true);

            return thisServer;
        }

        public void SetProxy(Proxy proxy)
        {
            Client.Proxy = new WebProxy(proxy.Address + ":" + proxy.Port.ToString())
            {
                Credentials = proxy.Credentials ?? null,
            };
        }

        private async Task<IRestResponse<T>> ExecuteRequest<T>(RestRequest request)
        {
            var response = await Client.ExecuteAsync<T>(request);

            if (response.ResponseUri.Host != new Uri(request.Resource).Host)
            {
                IsLoggedIn = false;
                Console.WriteLine("Not logged in!");
                InvalidateCredentials();
                await Login();
                return await ExecuteRequest<T>(request);
            }
            
            if (response.StatusCode == HttpStatusCode.Conflict && response.Headers.Any(h => h.Name == "gf-challenge-id"))
            {
                Console.WriteLine("Captcha required");
                string challengeId = response.Headers.Single(h => h.Name == "gf-challenge-id").Value.ToString().Substring(0, 36);
                var result = false;
                while (!result)
                {
                    result = SolveCaptcha(challengeId);
                }
                if (result)
                {
                    Console.WriteLine("Captcha solved!");
                }
                Credentials.Token = await GetBearerToken();
                return await ExecuteRequest<T>(request);
            }
            
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                IsLoggedIn = false;
                InvalidateCredentials();
                throw new Exception();
            }
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                IsLoggedIn = false;
                InvalidateCredentials();
                await Login();
                return await ExecuteRequest<T>(request);
            }

            if (response.ResponseUri.AbsoluteUri.Contains($"ogame.gameforge.com/game/"))
            {
                LastPage = response.Content;
                LastUrl = response.ResponseUri.AbsoluteUri;
            }

            return response;
        }

        public record LoginResponse
        {
            [JsonProperty("url")]
            public string Url { get; init; } = string.Empty;
        }

        public async Task<bool> Login()
        {
            RestRequest lobbyRequest = new("https://lobby.ogame.gameforge.com/");
            await ExecuteRequest<object>(lobbyRequest);

            string token = Credentials.Token ?? await GetBearerToken();
            Client.RemoveDefaultParameter("Authorization");
            Client.AddDefaultHeader("Authorization", "Bearer " + token);

            var playerId = await GetPlayerIdFromLobby();

            RestRequest loginRequest = new("https://lobby.ogame.gameforge.com/api/users/me/loginLink");
            loginRequest.AddParameter("id", playerId);
            loginRequest.AddParameter("server[language]", Credentials.Language);
            loginRequest.AddParameter("server[number]", Credentials.Number);
            loginRequest.AddParameter("clickedButton", "account_list");

            var response = await ExecuteRequest<LoginResponse>(loginRequest);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                IsLoggedIn = false;
                Console.WriteLine("Not logged in!");
                InvalidateCredentials();
                return await Login();
            }

            Console.WriteLine("Logging in...");
            await ExecuteRequest<object>(new(response.Data.Url));
            await ExecuteRequest<object>(new(UniverseBaseUrl + "page=ingame"));
            IsLoggedIn = true;
            Console.WriteLine("Login succesful!");

            return true;
        }

        public record Configuration
        {
            public string GameEnvironmentId { get; init; } = string.Empty;

            public string PlatformGameId { get; init; } = string.Empty;
        }

        private async Task<Configuration?> GetConfiguration()
        {
            RestRequest request = new("https://lobby.ogame.gameforge.com/config/configuration.js");
            var response = await Client.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var content = response.Content;
            var gameEnvironmentId = new Regex("\"gameEnvironmentId\":\"([a-f0-9-]*)\"").Match(content).Groups.Values.Last().Value;
            var platformGameId = new Regex("\"platformGameId\":\"([a-f0-9-]*)\"").Match(content).Groups.Values.Last().Value;

            return new()
            {
                GameEnvironmentId = gameEnvironmentId,
                PlatformGameId = platformGameId,
            };
        }

        public record SessionsResponse
        {
            public string Token { get; init; } = string.Empty;

            public bool IsPlatformLogin { get; init; } = default;

            public bool IsGameAccountMigrated { get; init; } = default;

            public string PlatformUserId { get; init; } = string.Empty;

            public bool IsGameAccountCreated { get; init; } = default;

            public bool HasUnmigratedGameAccounts { get; init; } = default;
        }

        public async Task<string> GetBearerToken()
        {
            var configuration = await GetConfiguration();
            if (configuration == null)
            {
                return string.Empty;
            }

            RestRequest request = new("https://gameforge.com/api/v1/auth/thin/sessions", Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                identity = Credentials.Email,
                password = Credentials.Password,
                gfLang = "en", // TODO : use configuration lang / locale
                locale = "en_EN", // TODO : use configuration lang / locale
                gameEnvironmentId = configuration.GameEnvironmentId,
                platformGameId = configuration.PlatformGameId,
                autoGameAccountCreation = false
            });

            var response = await Client.ExecuteAsync<SessionsResponse>(request);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                IsLoggedIn = false;
                Console.WriteLine($"Error {response.StatusCode}: {response.ErrorMessage}");
                InvalidateCredentials();
                return string.Empty;
            }

            var token = response.Data.Token;

            if (Client.CookieContainer != null)
            {
                Client.CookieContainer.Add(new Cookie("gf-token-production", token, "/", ".gameforge.com"));
            }

            Credentials.Token = token;

            SaveCredentials();

            return token;
        }

        public bool SolveCaptcha(string challengeId)
        {
            return OgameCaptchaSolver.OgameCaptchaSolver.SolveCaptcha(challengeId, Client);
        }

        public void InvalidateCredentials()
        {
            Credentials.Token = null;
            Database.DeleteItem("credentials");
            Client.CookieContainer = new();
            Client.RemoveDefaultParameter("authorization");
        }

        public void SaveCredentials()
        {
            Database.ReplaceItem<Credentials>("credentials", Credentials, true);
        }

        public async Task<bool> IsInVacationMode()
        {
            RestRequest request = new(UniverseBaseUrl);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.IsInVacationMode(LastPage));
        }

        public string GetPlayerName()
        {
            return Extractor.GetPlayerName(LastPage);
        }

        public int GetPlayerId()
        {
            return Extractor.GetPlayerID(LastPage);
        }

        public string GetServerName()
        {
            return Extractor.GetServerName(LastPage);
        }

        public int GetCurrentCelestialID()
        {
            return Extractor.GetCurrentCelestialID(LastPage);
        }

        public string GetCurrentCelestialName()
        {
            return Extractor.GetCurrentCelestialName(LastPage);
        }

        public Coordinate GetCurrentCelestialCoordinates()
        {
            return Extractor.GetCurrentCelestialCoordinates(LastPage);
        }

        public int GetEconomySpeed()
        {
            return Extractor.GetEconomySpeed(LastPage);
        }

        public int GetFleetSpeedPeaceful()
        {
            return Extractor.GetFleetSpeedPeaceful(LastPage);
        }

        public int GetFleetSpeedWar()
        {
            return Extractor.GetFleetSpeedWar(LastPage);
        }

        public int GetFleetSpeedHolding()
        {
            return Extractor.GetFleetSpeedHolding(LastPage);
        }

        public async Task<PlayerClasses> GetPlayerClass()
        {
            RestRequest request = new(UniverseBaseUrl);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetPlayerClass(LastPage));
        }

        public async Task<bool> IsUnderAttack()
        {
            RestRequest request = new(UniverseBaseUrl);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.IsUnderAttack(LastPage));
        }

        public async Task<Staff> GetStaff()
        {
            RestRequest request = new(UniverseBaseUrl);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetStaff(LastPage));
        }

        public async Task<List<Celestial>> GetCelestials()
        {
            RestRequest request = new(UniverseBaseUrl);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetCelestials(LastPage));
        }

        public async Task<Researches> GetResearches()
        {
            RestRequest request = new(UniverseBaseUrl + "page=fetchTechs&ajax=1&asJson=1");
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetTechs(LastPage).Researches);
        }

        public async Task<Techs> GetTechs(Celestial celestial)
        {
            RestRequest request = new(UniverseBaseUrl + "page=fetchTechs&ajax=1&asJson=1&cp=" + celestial.ID);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetTechs(LastPage));
        }

        public async Task<Buildings> GetBuildings(Celestial celestial)
        {
            return await GetTechs(celestial)
                .ContinueWith(_ => _.Result.Buildings);
        }

        public async Task<Facilities> GetFacilities(Celestial celestial)
        {
            return await GetTechs(celestial)
                .ContinueWith(_ => _.Result.Facilities);
        }

        public async Task<Ships> GetShips(Celestial celestial)
        {
            return await GetTechs(celestial)
                .ContinueWith(_ => _.Result.Ships);
        }

        public async Task<Defences> GetDefences(Celestial celestial)
        {
            return await GetTechs(celestial)
                .ContinueWith(_ => _.Result.Defences);
        }

        public async Task<Resources> GetResources(Celestial celestial)
        {
            RestRequest request = new(UniverseBaseUrl + "page=fetchResources&ajax=1&asJson=1&cp=" + celestial.ID);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetResources(LastPage));
        }

        public async Task<ResourceSettings> GetResourceSettings(Celestial celestial)
        {
            RestRequest request = new(UniverseBaseUrl + "page=resourceSettings&cp=" + celestial.ID);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetResourcesSettings(LastPage));
        }

        public async Task<ResourcesProduction> GetResourcesProduction(Celestial celestial)
        {
            RestRequest request = new(UniverseBaseUrl + "page=fetchResources&ajax=1&asJson=1&cp=" + celestial.ID);
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetResourcesProduction(LastPage));
        }

        public async Task<Slots> GetSlots()
        {
            RestRequest request = new(UniverseBaseUrl + "page=ingame&component=fleetdispatch");
            return await ExecuteRequest<object>(request)
                .ContinueWith(_ => Extractor.GetSlots(_.Result.Content));
        }
    }
}
