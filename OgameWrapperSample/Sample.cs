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

            // var researches = await ogameClient.GetResearches();
            // Console.WriteLine(researches);

            Console.WriteLine("Stating proxy...");

            OgameProxy proxy = new("localhost", 1337);
            proxy.Start(ogameClient);

            Console.WriteLine("Proxy started!");

            // await lobbyClient.Logout();

            Console.ReadLine();
        }
    }
}
