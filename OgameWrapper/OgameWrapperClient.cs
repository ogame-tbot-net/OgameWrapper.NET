using Newtonsoft.Json;
using RestSharp;
using System.Net;
using OgameCaptchaSolver;
using MihaZupan;
using OgameWrapper.Model;
using OgameWrapper.Includes;
using JsonFlatFileDataStore;

namespace OgameWrapper
{
    public class OgameWrapperClient
    {
        private RestClient Client { get; set; }
        private Credentials Credentials { get; set; }
        public bool IsLoggedIn { get; private set; }
        public DataStore Database {get; private set; }
        public IDocumentCollection<Celestial> Celestials { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public string LastPage { get; set; }
        public string LastUrl { get; set; }
        public string IndexPhp
        {
            get
            {
                return $"https://s{Credentials.Number}-{Credentials.Language.ToLower()}.ogame.gameforge.com/game/index.php?";
            }
        }
        public int? PlayerId { get
            {
                return GetPlayerId();
            } }

        private int? GetPlayerId()
        {
            RestRequest request = new()
            {
                Method = Method.GET,
                Resource = "https://lobby.ogame.gameforge.com/api/users/me/accounts"
            };
            var response = ExecuteRequest(request);
            if (response.StatusCode != HttpStatusCode.OK || response.Content == string.Empty || response.Content == null)
            {
                return null;
            }
            else
            {
                List<AccountInfo> content = JsonConvert.DeserializeObject<List<AccountInfo>>(response.Content);
                var thisServer = GetServerInfo();
                if (thisServer != null)
                {
                    var thisAccount = content
                        .Where(a => a.Server.Language == Credentials.Language)
                        .Where(a => a.Server.Number == thisServer.Number)
                        .FirstOrDefault() ?? null;
                    if (thisAccount != null)
                        return thisAccount.Id;
                    else return null;
                }
                else return null;
            }
        }
        private List<ServerInfo> GetServers()
        {
            RestRequest request = new()
            {
                Method = Method.GET,
                Resource = "https://lobby.ogame.gameforge.com/api/servers"
            };
            var response = ExecuteRequest(request);
            if (response.StatusCode != HttpStatusCode.OK || response.Content == string.Empty || response.Content == null)
            {
                return new();
            }
            else
            {
                List<ServerInfo> content = JsonConvert.DeserializeObject<List<ServerInfo>>(response.Content);
                return content;
            }
        }
        public ServerInfo? GetServerInfo()
        {
            var servers = GetServers();
            var thisServer = servers
                .Where(s => s.Language == Credentials.Language)
                .Where(s => s.Number == Credentials.Number)
                .FirstOrDefault() ?? null;

            Database.ReplaceItem<ServerInfo>("serverInfo", thisServer, true);

            return thisServer;
        }
        public OgameWrapperClient(Credentials? credentials = null, string? userAgent = null, Proxy? proxy = null)
        {
            IsLoggedIn = false;
            Database = new DataStore("data.json");
            if (Database.TryGetItem<Credentials>("credentials", out var creds) && creds.Token != null)
            {
                Credentials = new(credentials.Email, credentials.Password, credentials.Language, credentials.Number);
                Credentials.Token = creds.Token;
            }
            else
            {
                Credentials = credentials;
                if (credentials == null)
                {
                    throw new ArgumentNullException(nameof(credentials));
                }
            }

            Client = new RestClient();
            if (userAgent != null)
            {
                SetUserAgent(userAgent);
            }
            else
            {
                SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36");
            }
            if (proxy != null)
            {
                SetProxy(proxy);
            }
            Client.AddDefaultHeader("Accept", "gzip, deflate, br");
            Client.Timeout = 30000;
            Client.CookieContainer = new CookieContainer();
            
            Celestials = Database.GetCollection<Celestial>("Celestials");
            if (Database.TryGetItem<ServerInfo>("serverInfo", out var si)) {
                ServerInfo = si;
            }
            else
            {
                ServerInfo = new();
                Database.InsertItem<ServerInfo>("serverInfo", ServerInfo);
            }
        }
        public void SetProxy(Proxy proxy)
        {
            WebProxy webProxy = new(proxy.Address + ":" + proxy.Port.ToString());
            if (proxy.Credentials != null)
            {
                webProxy.Credentials = proxy.Credentials;
            }
            Client.Proxy = webProxy;
        }
        public void SetUserAgent(string userAgent)
        {
            Client.UserAgent = userAgent;
        }
        private IRestResponse ExecuteRequest(RestRequest request)
        {
            var response = Client.Execute(request);
            if (response.ResponseUri.Host.ToString() != new Uri(request.Resource).Host.ToString())
            {
                IsLoggedIn = false;
                Console.WriteLine("Not logged in!");
                InvalidateCredentials();
                Login();
                return ExecuteRequest(request);
            }
            else if (response.StatusCode == HttpStatusCode.Conflict && response.Headers.Any(h => h.Name == "gf-challenge-id"))
            {
                Console.WriteLine("Captcha required");
                string challengeId = response.Headers.Single(h => h.Name == "gf-challenge-id").Value.ToString().Substring(0, 36);
                var result = false;
                while (!result)
                    result = SolveCaptcha(challengeId);
                if (result)
                    Console.WriteLine("Captcha solved!");
                Credentials.Token = GetBearerToken();
                return ExecuteRequest(request);
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                IsLoggedIn = false;
                InvalidateCredentials();
                throw new Exception();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                IsLoggedIn = false;
                InvalidateCredentials();
                Login();
                return ExecuteRequest(request);
            }
            else
            {
                if (response.ResponseUri.AbsoluteUri.Contains($"ogame.gameforge.com/game/"))
                {
                    LastPage = response.Content;
                    LastUrl = response.ResponseUri.AbsoluteUri;
                }                    
                return response;
            }
        }
        public bool Login()
        {
            RestRequest request;
            request = new RestRequest()
            {
                Method = Method.GET,
                Resource = "https://lobby.ogame.gameforge.com/"
            };
            ExecuteRequest(request);

            string token = String.Empty;
            if (Credentials.Token != null)
                token = Credentials.Token;
            else
                token = GetBearerToken();

            Client.RemoveDefaultParameter("authorization");
            Client.AddDefaultHeader("authorization", "Bearer " + token);

            request = new()
            {
                Method = Method.GET,
                Resource = "https://lobby.ogame.gameforge.com/api/users/me/loginLink"
            };
            request.AddParameter("id", PlayerId, ParameterType.GetOrPost);
            request.AddParameter("server[language]", Credentials.Language, ParameterType.GetOrPost);
            request.AddParameter("server[number]", Credentials.Number.ToString(), ParameterType.GetOrPost);
            request.AddParameter("clickedButton", "account_list", ParameterType.GetOrPost);
            var response = ExecuteRequest(request);
            if (response.StatusCode != HttpStatusCode.OK || response.Content == string.Empty || response.Content == null)
            {
                IsLoggedIn = false;
                Console.WriteLine("Not logged in!");
                InvalidateCredentials();
                return Login();
            }
            else
            {
                Console.WriteLine("Logging in...");
                dynamic loginLink = JsonConvert.DeserializeObject(response.Content);
                response = ExecuteRequest(new RestRequest()
                {
                    Method = Method.GET,
                    Resource = loginLink.url.ToString()
                });
                response = ExecuteRequest(new RestRequest()
                {
                    Method = Method.GET,
                    Resource = IndexPhp + "page=ingame"
                });
                IsLoggedIn = true;
                Console.WriteLine("Login succesful!");
                return true;
            }
        }

        public string GetBearerToken()
        {
            RestRequest request;
            IRestResponse response;
            dynamic content;

            string gameEnvironmentId;
            string platformGameId;

            request = new RestRequest()
            {
                Method = Method.GET,
                Resource = "https://lobby.ogame.gameforge.com/config/configuration.js"
            };
            response = ExecuteRequest(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unable to get https://lobby.ogame.gameforge.com/config/configuration.js");
            }
            else
            {
                gameEnvironmentId = response.Content.Substring(response.Content.IndexOf("gameEnvironmentId") + 20, 36);
                platformGameId = response.Content.Substring(response.Content.IndexOf("platformGameId") + 17, 36);
            }

            request = new RestRequest()
            {
                Method = Method.POST,
                Resource = "https://gameforge.com/api/v1/auth/thin/sessions"
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                identity = Credentials.Email,
                password = Credentials.Password,
                gfLang = "en",
                locale = "en_EN",
                gameEnvironmentId = gameEnvironmentId,
                platformGameId = platformGameId,
                autoGameAccountCreation = false
            });
            response = ExecuteRequest(request);
            if (response.StatusCode != HttpStatusCode.Created || response.Content == string.Empty || response.Content == null)
            {
                IsLoggedIn = false;
                Console.WriteLine($"Error {response.StatusCode}: {response.ErrorMessage}");
                InvalidateCredentials();
                return "";
            }
            else
            {
                content = JsonConvert.DeserializeObject(response.Content);
                Client.CookieContainer.Add(new Cookie("gf-token-production", content.token.ToString(), "/", ".gameforge.com"));

                Credentials.Token = content.token;
                SaveCredentials();

                return content.token;
            }
        }
        public bool SolveCaptcha(string challengeId)
        {
            var output = OgameCaptchaSolver.OgameCaptchaSolver.SolveCaptcha(challengeId, Client);
            return output;
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

        public bool IsInVacationMode()
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp
            };
            ExecuteRequest(request);
            return Extractor.IsInVacationMode(LastPage);
        }

        public string GetPlayerName()
        {
            return Extractor.GetPlayerName(LastPage);
        }
        
        public int GetPlayerID()
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

        public PlayerClasses GetPlayerClass()
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp
            };
            ExecuteRequest(request);
            return Extractor.GetPlayerClass(LastPage);
        }
        public bool IsUnderAttack()
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp
            };
            ExecuteRequest(request);
            return Extractor.IsUnderAttack(LastPage);
        }
        public Staff GetStaff()
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp
            };
            ExecuteRequest(request);
            return Extractor.GetStaff(LastPage);
        }
        public List<Celestial> GetCelestials()
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp
            };
            ExecuteRequest(request);
            return Extractor.GetCelestials(LastPage);
        }
        public Researches GetResearches()
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp + "page=fetchTechs&ajax=1&asJson=1"
            };
            ExecuteRequest(request);
            var researches = Extractor.GetTechs(LastPage).Researches;
            return researches;
        }
        public Techs GetTechs(Celestial celestial)
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp + "page=fetchTechs&ajax=1&asJson=1&cp=" + celestial.ID
            };
            ExecuteRequest(request);
            var techs = Extractor.GetTechs(LastPage);
            return techs;
        }
        public Buildings GetBuildings(Celestial celestial)
        {
            var techs = GetTechs(celestial);
            return techs.Buildings;
        }
        public Facilities GetFacilities(Celestial celestial)
        {
            var techs = GetTechs(celestial);
            return techs.Facilities;
        }
        public LifeformBuildings? GetLifeformBuildings(Celestial celestial)
        {
            var techs = GetTechs(celestial);
            return techs.LifeformBuildings;
        }
        public LifeformResearches? GetLifeformResearches(Planet celestial)
        {
            var techs = GetTechs(celestial);
            return techs.LifeformResearches;
        }
        public Ships GetShips(Celestial celestial)
        {
            var techs = GetTechs(celestial);
            return techs.Ships;
        }
        public Defences GetDefences(Celestial celestial)
        {
            var techs = GetTechs(celestial);
            return techs.Defences;
        }
        public Resources GetResources(Celestial celestial)
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp + "page=fetchResources&ajax=1&asJson=1&cp=" + celestial.ID
            };
            ExecuteRequest(request);
            var res = Extractor.GetResources(LastPage);
            return res;
        }
        public ResourceSettings GetResourceSettings(Planet celestial)
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp + "page=resourceSettings&cp=" + celestial.ID
            };
            ExecuteRequest(request);
            var res = Extractor.GetResourcesSettings(LastPage);
            return res;
        }
        public ResourcesProduction GetResourcesProduction(Planet celestial)
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp + "page=fetchResources&ajax=1&asJson=1&cp=" + celestial.ID
            };
            ExecuteRequest(request);
            var res = Extractor.GetResourcesProduction(LastPage);
            return res;
        }
        public Slots GetSlots()
        {
            var request = new RestRequest()
            {
                Method = Method.GET,
                Resource = IndexPhp + "page=ingame&component=fleetdispatch"
            };
            var result = ExecuteRequest(request);
            var slots = Extractor.GetSlots(result.Content);
            return slots;
        }
    }    
}