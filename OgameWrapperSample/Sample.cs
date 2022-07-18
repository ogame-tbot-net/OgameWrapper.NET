
using OgameWrapper.Model;
using OgameWrapper.Includes;
using System;
using System.Net;

namespace OgameWrapper.Sample
{
    public static class Sample
    {
        private static OgameWrapperClient ogameClient;

        static void Main(string[] args)
        {
            Credentials credentials = new("em@i.l", "password", "en", 132);
            ogameClient = new(credentials);

            var login = ogameClient.Login();
            
            if (login)
            {
                var time1 = DateTime.Now;
                var isVacationMode = ogameClient.IsInVacationMode();
                var time2 = DateTime.Now;
                Console.WriteLine("IsInVacationMode: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var isUnderAttack = ogameClient.IsUnderAttack();
                time2 = DateTime.Now;
                Console.WriteLine("IsUnderAttack: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var playerClass = ogameClient.GetPlayerClass();
                time2 = DateTime.Now;
                Console.WriteLine("GetPlayerClass: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var playerName = ogameClient.GetPlayerName();
                time2 = DateTime.Now;
                Console.WriteLine("GetPlayerName: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var staff = ogameClient.GetStaff();
                time2 = DateTime.Now;
                Console.WriteLine("GetStaff: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var researches = ogameClient.GetResearches();
                time2 = DateTime.Now;
                Console.WriteLine("GetResearches: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var slots = ogameClient.GetSlots();
                time2 = DateTime.Now;
                Console.WriteLine("GetSlots: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                var celestials = ogameClient.GetCelestials();
                time2 = DateTime.Now;
                Console.WriteLine("GetCelestials: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                var count = celestials.Count;
                Console.WriteLine("celestials.Count: " + count.ToString());
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(c => c.GetTechs(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetTechs: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(c => c.GetResources(ogameClient));
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
                celestials.ForEach(c => c.GetBuildings(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetBuildings: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(c => c.GetFacilities(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetFacilities: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(c => c.GetShips(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetShips: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(c => c.GetDefences(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetDefences: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(c => c.GetResourceSettings(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetResourceSettings: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();

                time1 = DateTime.Now;
                celestials.ForEach(c => c.GetResourcesProduction(ogameClient));
                time2 = DateTime.Now;
                Console.WriteLine("Celestial.GetResourcesProduction: " + time2.Subtract(time1).TotalMilliseconds + "ms");
                Console.WriteLine("per celestial: " + (time2.Subtract(time1).TotalMilliseconds / count) + "ms");
                Console.WriteLine();
            }

            Console.ReadLine();
        }
            
    }
}
