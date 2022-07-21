using OgameWrapper.Services;
using OgameCaptchaSolver;
using RestSharp;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;

namespace OgameWrapper
{
    internal record Configuration
    {
        public string GameEnvironmentId { get; init; } = string.Empty;

        public string PlatformGameId { get; init; } = string.Empty;
    }

    internal record LobbySession
    {
        public string Token { get; init; } = string.Empty;

        public bool IsPlatformLogin { get; init; } = default;

        public bool IsGameAccountMigrated { get; init; } = default;

        public string PlatformUserId { get; init; } = string.Empty;

        public bool IsGameAccountCreated { get; init; } = default;

        public bool HasUnmigratedGameAccounts { get; init; } = default;

        public bool IsValid
        {
            get
            {
                return Token.Length > 0;
            }
        }
    }

    // TODO : fix properties when Restsharp is updated
    internal record LobbySessionRequest
    {
        [JsonPropertyName("autoGameAccountCreation")]
        public bool autoGameAccountCreation { get; set; } = false;

        [JsonPropertyName("identity")]
        public string identity { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string password { get; set; } = string.Empty;

        [JsonPropertyName("gfLang")]
        public string gfLang { get; set; } = string.Empty;

        [JsonPropertyName("locale")]
        public string locale { get; set; } = string.Empty;

        [JsonPropertyName("gameEnvironmentId")]
        public string gameEnvironmentId { get; set; } = string.Empty;

        [JsonPropertyName("platformGameId")]
        public string platformGameId { get; set; } = string.Empty;
    }

    internal record ServerSettings
    {
        public bool Aks { get; init; } = false;

        public bool WreckField { get; init; } = false;

        public string ServerLabel { get; init; } = string.Empty;

        public uint EconomySpeed { get; init; } = 0;

        public uint PlanetFields { get; init; } = 0;

        public uint UniverseSize { get; init; } = 0;

        public uint FleedSpeedWar { get; init; } = 0;

        public string ServerCategory { get; init; } = string.Empty;

        public uint FleedSpeedHolding { get; init; } = 0;

        public uint FleedSpeedPeaceful { get; init; } = 0;

        public bool EspionageProbeRaids { get; init; } = false;

        public uint PremiumValidationGift { get; init; } = 0;

        public uint DebrisFieldFactorShips { get; init; } = 0;

        public uint ResearchDurationDivisor { get; init; } = 0;

        public uint DebrisFieldFactorDefence { get; init; } = 0;
    }

    public record Server
    {
        public string Language { get; init; } = string.Empty;

        public uint Number { get; init; } = 0;

        public string AccountGroup { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public uint PlayerCount { get; init; } = 0;

        public uint PlayersOnline { get; init; } = 0;

        public DateTime Opened { get; init; } = DateTime.MinValue;

        public DateTime StartDate { get; init; } = DateTime.MinValue;

        public DateTime? EndDate { get; init; } = null;

        public bool ServerClosed { get; init; } = false;

        public bool Prefered { get; init; } = false;

        public bool SignupClosed { get; init; } = false;

        public bool MultiLanguage { get; init; } = false;

        public List<string> AvailableOn = new();
    }

    public record Me
    {
        public uint Id { get; init; } = 0;

        public uint UserId { get; init; } = 0;

        public string GameforgeAccountId { get; init; } = string.Empty;

        public bool Validated { get; init; } = false;

        public bool Portable { get; init; } = false;

        public bool UnlinkedAccounts { get; init; } = false;

        public bool MigrationRequired { get; init; } = false;

        public string Email { get; init; } = string.Empty;

        public string UnportableName { get; init; } = string.Empty;

        public string Mhash { get; init; } = string.Empty;
    }

    public record AccountServer
    {
        public string Language { get; init; } = string.Empty;

        public uint Number { get; init; } = 0;
    }

    public record AccountDetail
    {
        public string Type { get; init; } = string.Empty;

        public string Title { get; init; } = string.Empty;

        public string Value { get; init; } = string.Empty;
    }

    public record AccountSitting
    {
        public bool Shared { get; init; } = false;

        public DateTime? EndTime { get; init; } = null;

        public DateTime? CooldownTime { get; init; } = null;
    }

    public record AccountTrading
    {
        public bool Trading { get; init; } = false;

        public DateTime? CooldownTime { get; init; } = null;
    }

    public record Account
    {
        public AccountServer Server { get; init; } = new();

        public uint Id { get; init; } = 0;

        public uint GameAccountId { get; init; } = 0;

        public string Name { get; init; } = string.Empty;

        public string AccountGroup { get; init; } = string.Empty;

        public DateTime LastPlayed { get; init; } = DateTime.MinValue;

        public DateTime LastLogin { get; init; } = DateTime.MinValue;

        public bool Blocked { get; init; } = false;

        public DateTime? BannedUntil { get; init; } = null;

        public string? BannedReason { get; init; } = null;

        public List<AccountDetail> Details { get; init; } = new();

        public AccountSitting Sitting { get; init; } = new();

        public AccountTrading Trading { get; init; } = new();
    }

    public record ServerLoginResponse
    {
        public string Url { get; init; } = string.Empty;
    }

    public class LobbyClient
    {
        private RestClient HttpClient { get; init; }

        private LobbySession Session { get; set; }

        private string Email { get; init; }

        private string Password { get; init; }

        public bool IsLoggedIn
        {
            get
            {
                return Session.IsValid;
            }
        }

        public LobbyClient(string email, string password)
        {
            Email = email;
            Password = password;

            HttpClient = ServiceFactory.HttpClient;
            Session = new();
        }

        public async Task Login()
        {
            if (IsLoggedIn)
            {
                return;
            }

            RestRequest request = new("https://lobby.ogame.gameforge.com/");

            var response = await HttpClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to load lobby page : Invalid status code {response.StatusCode}.");
            }

            var session = await GetSession();
            Session = session;
        }

        public async Task Logout()
        {
            if (!IsLoggedIn)
            {
                return;
            }

            RestRequest logoutRequest = new("https://lobby.ogame.gameforge.com/api/users/me/logout", Method.PUT);

            var logoutResponse = await ExecuteAuthenticatedAsync<object>(logoutRequest);
            if (logoutResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to logout : Invalid status code {logoutResponse.StatusCode}.");
            }

            RestRequest sessionRequest = new("https://gameforge.com/api/v1/auth/sessions", Method.DELETE);

            var sessionResponse = await ExecuteAuthenticatedAsync<object>(sessionRequest);
            if (sessionResponse.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception($"Unable to delete session : Invalid status code {sessionResponse.StatusCode}.");
            }

            Session = new();
        }

        public async Task<string> GetServerToken(Account account)
        {
            RestRequest request = new("https://lobby.ogame.gameforge.com/api/users/me/loginLink");
            request.AddHeader("Referer", "https://lobby.ogame.gameforge.com/en_GB/hub");

            request.AddParameter("id", account.GameAccountId);
            request.AddParameter("server[language]", account.Server.Language);
            request.AddParameter("server[number]", account.Server.Number);
            request.AddParameter("clickedButton", "account_list");

            var response = await ExecuteAuthenticatedAsync<ServerLoginResponse>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to login to server : Invalid status code {response.StatusCode}.");
            }

            var uri = new Uri(response.Data.Url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var token = query.Get("token");
            if (token == null)
            {
                throw new Exception("Unable to get server token.");
            }

            return token;
        }

        public async Task<List<Server>> GetServers()
        {
            var request = new RestRequest("https://lobby.ogame.gameforge.com/api/servers");
            request.AddHeader("Referer", "https://lobby.ogame.gameforge.com/en_GB/");

            var response = await HttpClient.ExecuteAsync<List<Server>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get servers : Invalid status code {response.StatusCode}.");
            }

            return response.Data;
        }

        public async Task<Account> GetAccount(string language, uint number)
        {
            var accounts = await GetAccounts();

            AccountServer userServer = new()
            {
                Language = language,
                Number = number,
            };

            if (!accounts.Exists(account => account.Server == userServer))
            {
                throw new Exception($"Unknown server {userServer}.");
            }

            return accounts.First(account => account.Server == userServer);
        }

        public async Task<Me> GetMe()
        {
            var request = new RestRequest("https://lobby.ogame.gameforge.com/api/users/me");
            request.AddHeader("Referer", "https://lobby.ogame.gameforge.com/en_GB/hub");

            var response = await ExecuteAuthenticatedAsync<Me>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get me : Invalid status code {response.StatusCode}.");
            }

            return response.Data;
        }

        public async Task<List<Account>> GetAccounts()
        {
            var request = new RestRequest("https://lobby.ogame.gameforge.com/api/users/me/accounts");
            request.AddHeader("Referer", "https://lobby.ogame.gameforge.com/en_GB/hub");

            var response = await ExecuteAuthenticatedAsync<List<Account>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get accounts : Invalid status code {response.StatusCode}.");
            }
            return response.Data;
        }

        private async Task<LobbySession> GetSession()
        {
            var configuration = await GetConfiguration();

            RestRequest request = new("https://gameforge.com/api/v1/auth/thin/sessions", Method.POST);
            request.AddHeader("Referer", "https://lobby.ogame.gameforge.com/");
            request.AddHeader("Content-Type", "application/json");

            LobbySessionRequest requestBody = new()
            {
                identity = Email,
                password = Password,
                gfLang = "en",
                locale = "en_GB",
                gameEnvironmentId = configuration.GameEnvironmentId,
                platformGameId = configuration.PlatformGameId,
            };

            request.AddJsonBody(requestBody);

            var response = await HttpClient.ExecuteAsync<LobbySession>(request);

            // Check if captcha is present
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var header = response.Headers.First(header => header.Name == "gf-challenge-id");
                if (header == null || header.Value == null)
                {
                    throw new Exception("Cannot get catpcha challenge id.");
                }

                string challengeId = (string)header.Value;
                challengeId = challengeId.Split(';')[0];
                var solved = OgameCaptchaSolver.OgameCaptchaSolver.SolveCaptcha(challengeId, HttpClient);
                if (!solved)
                {
                    throw new Exception("Cannot solve catpcha.");
                }

                return await GetSession();
            }

            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Unable to get connect to lobby : Invalid status code {response.StatusCode}.");
            }

            return response.Data;
        }

        private async Task<Configuration> GetConfiguration()
        {
            RestRequest request = new("https://lobby.ogame.gameforge.com/config/configuration.js");
            var response = await HttpClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Unable to get lobby configuration : Invalid status code {response.StatusCode}.");
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

        private async Task<IRestResponse<T>> ExecuteAuthenticatedAsync<T>(IRestRequest request)
        {
            request.AddHeader("Authorization", $"Bearer {Session.Token}");

            var response = await HttpClient.ExecuteAsync<T>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session = new();

                await Login();

                return await ExecuteAuthenticatedAsync<T>(request);
            }

            return response;
        }
    }
}
