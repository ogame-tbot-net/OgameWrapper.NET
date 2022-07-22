namespace OgameWrapper.Sample
{
    public static class CaptchaCollector
    {
        static async Task Main(string[] args)
        {
            var flag = false;
            while (!flag)
            {
                try
                {
                    var email = CreateString(5, 5, 2);
                    var password = CreateString(5, 5, 2);
                    LobbyClient lobbyClient = new(email, password);

                    await lobbyClient.Login();
                    flag = true;
                }
                catch (Exception ex)
                {
                    var rand = new Random();
                    decimal interval = rand.Next(450000, 900000);
                    Console.WriteLine($"Limit hit, waiting about {(int)Math.Round(interval / 1000 / 60)} minutes");
                    Thread.Sleep((int)Math.Round(interval));
                }
            }

            Console.ReadLine();
        }

        static string CreateString(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijkmnopqrstuvwxyz";
            string uppers = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string number = "23456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);
        }
    }
}
