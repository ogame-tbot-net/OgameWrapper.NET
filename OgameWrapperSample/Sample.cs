namespace OgameWrapper.Sample
{
    public static class Sample
    {
        static async Task Main(string[] args)
        {
            var lobbyEmail = "em@i.l";
            var lobbyPassword = "password";
            var pioneers = true;
            var serverLanguage = "en";
            var serverNumber = 132u;

            Console.WriteLine("Connecting to lobby...");

            LobbyClient lobbyClient = new(lobbyEmail, lobbyPassword, pioneers);
            await lobbyClient.Login();

            Console.WriteLine("Connected to lobby!");

            /*
            Console.WriteLine("Creating account...");

            var servers = await lobbyClient.GetServers();
            var server = servers.First(s => s.Name == "DevToolUniverse");

            var account = await lobbyClient.CreateServerAccount(server);

            Console.WriteLine("Account created!");
            */

            Console.WriteLine("Getting account...");

            var account = await lobbyClient.GetAccount(serverLanguage, serverNumber);

            Console.WriteLine($"Found {account}");

            Console.WriteLine("Connecting to server...");

            OgameClient ogameClient = new(lobbyClient, account);
            await ogameClient.Login();

            Console.WriteLine("Connected to server!");

            // await ogameClient.SelectInitialPlayerClass(Model.PlayerClasses.General);

            // var researches = await ogameClient.GetResearches();
            // Console.WriteLine(researches);

            var celestials = await ogameClient.GetCelestials();
            var celestial = celestials.First();
            if (celestial != null)
            {
                /*
                var isInVacationMode = await ogameClient.IsInVacationMode();
                var playerName = await ogameClient.GetPlayerName();
                var playerId = await ogameClient.GetPlayerId();
                var serverName = await ogameClient.GetServerName();
                var currentCelestialId = await ogameClient.GetCurrentCelestialId();
                var currentCelestialName = await ogameClient.GetCurrentCelestialName();
                var currentCelestialCoordinates = await ogameClient.GetCurrentCelestialCoordinates();
                var economySpeed = await ogameClient.GetEconomySpeed();
                var fleedSpeedPeaceful = await ogameClient.GetFleetSpeedPeaceful();
                var fleedSpeedWar = await ogameClient.GetFleetSpeedWar();
                var fleedSpeedHolding = await ogameClient.GetFleetSpeedHolding();
                var playerClass = await ogameClient.GetPlayerClass();
                var isUnderAttack = await ogameClient.IsUnderAttack();
                var staff = await ogameClient.GetStaff();
                var researches = await ogameClient.GetResearches();
                var techs = await ogameClient.GetTechs(celestial);
                var buildings = await ogameClient.GetBuildings(celestial);
                var facilities = await ogameClient.GetFacilities(celestial);
                var lifeformBuildings = await ogameClient.GetLifeformBuildings(celestial);
                var lifeformResearches = await ogameClient.GetLifeformResearches(celestial);
                */
                var ships = await ogameClient.GetShips(celestial);
                // var defences = await ogameClient.GetDefences(celestial);

                var resources = await ogameClient.GetResources(celestial);
                var resourceSettings = await ogameClient.GetResourceSettings(celestial);
                var resourcesProduction = await ogameClient.GetResourcesProduction(celestial);
                var slots = await ogameClient.GetSlots();

                if (true) { }
            }

            Console.WriteLine("Stating proxy...");

            OgameProxy proxy = new("localhost", 1337);
            proxy.Start(ogameClient);

            Console.WriteLine("Proxy started!");

            // await lobbyClient.Logout();

            Console.ReadLine();
        }
    }
}
