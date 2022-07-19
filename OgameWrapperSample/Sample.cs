
using OgameWrapper.Model;
using OgameWrapper.Includes;
using System;
using System.Net;

namespace OgameWrapper.Sample
{
    public static class Sample
    {
        private static OgameWrapperClient ogameClient;

        static async Task Main(string[] args)
        {
            Credentials credentials = new("em@i.l", "password", "en", 132);
            ogameClient = new(credentials);

            var login = await ogameClient.Login();
            
            if (login)
            {
                var time1 = DateTime.Now;
                var isVacationMode = await ogameClient.IsInVacationMode();
                var time2 = DateTime.Now;
                Console.WriteLine("IsInVacationMode: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var isUnderAttack = await ogameClient.IsUnderAttack();
                time2 = DateTime.Now;
                Console.WriteLine("IsUnderAttack: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var playerClass = await ogameClient.GetPlayerClass();
                time2 = DateTime.Now;
                Console.WriteLine("GetPlayerClass: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var playerName = ogameClient.GetPlayerName();
                time2 = DateTime.Now;
                Console.WriteLine("GetPlayerName: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var staff = await ogameClient.GetStaff();
                time2 = DateTime.Now;
                Console.WriteLine("GetStaff: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var researches = await ogameClient.GetResearches();
                time2 = DateTime.Now;
                Console.WriteLine("GetResearches: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var slots = await ogameClient.GetSlots();
                time2 = DateTime.Now;
                Console.WriteLine("GetSlots: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var celestials = await ogameClient.GetCelestials();
                time2 = DateTime.Now;
                Console.WriteLine("GetCelestials: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                var count = celestials.Count;
                Console.WriteLine("celestials.Count: " + count.ToString());
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetTechs(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetTechs: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetResources(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetResources: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var speed = ogameClient.GetEconomySpeed();
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetEconomySpeed: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetBuildings(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetBuildings: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetFacilities(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetFacilities: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetShips(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetShips: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetDefences(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetDefences: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetResourceSettings(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetResourceSettings: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(async c => await c.GetResourcesProduction(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetResourcesProduction: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();
            }

            Console.ReadLine();
        }
            
    }
}
