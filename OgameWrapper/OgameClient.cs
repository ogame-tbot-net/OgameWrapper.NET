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

    public class OgameClient
    {
        private Account Account { get; init; }

        private LobbyClient LobbyClient { get; init; }

        private RestClient HttpClient { get; init; }

        private string ServerHost
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

        public async Task<bool> IsInVacationMode()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.IsInVacationMode(response.Content);
        }

        public async Task<string> GetPlayerName()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetPlayerName(response.Content);
        }

        public async Task<int> GetPlayerId()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetPlayerID(response.Content);
        }

        public async Task<string> GetServerName()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetServerName(response.Content);
        }

        public async Task<int> GetCurrentCelestialId()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetCurrentCelestialID(response.Content);
        }

        public async Task<string> GetCurrentCelestialName()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetCurrentCelestialName(response.Content);
        }

        public async Task<Coordinate?> GetCurrentCelestialCoordinates()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetCurrentCelestialCoordinates(response.Content);
        }

        public async Task<int> GetEconomySpeed()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetEconomySpeed(response.Content);
        }

        public async Task<int> GetFleetSpeedPeaceful()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetFleetSpeedPeaceful(response.Content);
        }

        public async Task<int> GetFleetSpeedWar()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetFleetSpeedWar(response.Content);
        }

        public async Task<int> GetFleetSpeedHolding()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetFleetSpeedHolding(response.Content);
        }

        public async Task<PlayerClasses> GetPlayerClass()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetPlayerClass(response.Content);
        }

        public async Task<bool> IsUnderAttack()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.IsUnderAttack(response.Content);
        }

        public async Task<Staff> GetStaff()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetStaff(response.Content);
        }

        public async Task<List<Celestial>> GetCelestials()
        {
            RestRequest request = new(Page.Overview.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetCelestials(response.Content);
        }

        public async Task<Researches> GetResearches()
        {
            RestRequest request = new(Page.FetchTechs.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetTechs(response.Content).Researches;
        }

        public async Task<Techs> GetTechs(Celestial celestial)
        {
            var url = string.Format(Page.FetchCelestialTechs.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetTechs(response.Content);
        }

        public async Task<Buildings> GetBuildings(Celestial celestial)
        {
            var techs = await GetTechs(celestial);
            return techs.Buildings;
        }

        public async Task<Facilities> GetFacilities(Celestial celestial)
        {
            var techs = await GetTechs(celestial);
            return techs.Facilities;
        }

        public async Task<LifeformBuildings?> GetLifeformBuildings(Celestial celestial)
        {
            var techs = await GetTechs(celestial);
            return techs.LifeformBuildings;
        }

        public async Task<LifeformResearches?> GetLifeformResearches(Planet planet)
        {
            var techs = await GetTechs(planet);
            return techs.LifeformResearches;
        }

        public async Task<Ships> GetShips(Celestial celestial)
        {
            var techs = await GetTechs(celestial);
            return techs.Ships;
        }

        public async Task<Defences> GetDefences(Celestial celestial)
        {
            var techs = await GetTechs(celestial);
            return techs.Defences;
        }

        public async Task<Resources> GetResources(Celestial celestial)
        {
            var url = string.Format(Page.FetchCelestialResources.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetResources(response.Content);
        }

        public async Task<ResourceSettings> GetResourceSettings(Celestial celestial)
        {
            var url = string.Format(Page.ResourceSettings.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetResourcesSettings(response.Content);
        }

        public async Task<ResourcesProduction> GetResourcesProduction(Celestial celestial)
        {
            var url = string.Format(Page.FetchCelestialResources.Value, celestial.ID);
            RestRequest request = new(url);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetResourcesProduction(response.Content);
        }

        public async Task<Slots> GetSlots()
        {
            RestRequest request = new(Page.FleetDispatch.Value);
            var response = await ExecuteRequestAsync(request);
            return Extractor.GetSlots(response.Content);
        }

        private Task<IRestResponse<object>> ExecuteRequestAsync(IRestRequest request)
        {
            return ExecuteRequestAsync<object>(request);
        }

        private async Task<IRestResponse<T>> ExecuteRequestAsync<T>(IRestRequest request)
        {
            var response = await HttpClient.ExecuteAsync<T>(request);

            var uri = response.ResponseUri;
            if (uri.Host != ServerHost || response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await Login();

                return await ExecuteRequestAsync<T>(request);
            }

            return response;
        }
    }
}
