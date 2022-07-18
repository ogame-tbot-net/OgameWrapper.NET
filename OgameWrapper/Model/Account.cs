using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgameWrapper.Model
{
    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public int Number { get; set; }
        public string? Token { get; set; }
        public Credentials(string email, string password, string language, int number)
        {
            Email = email;
            Password = password;
            Language = language;
            Number = number;
        }
    }


    public class Server
    {
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }
    }

    public class Detail
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class Sitting
    {
        [JsonProperty(PropertyName = "shared")]
        public bool Generated { get; set; }

        [JsonProperty(PropertyName = "endTime")]
        public DateTime? EndTime { get; set; }

        [JsonProperty(PropertyName = "cooldownTime")]
        public DateTime? CooldownTime { get; set; }
    }

    public class Trading
    {
        [JsonProperty(PropertyName = "trading")]
        public bool Generated { get; set; }

        [JsonProperty(PropertyName = "cooldownTime")]
        public DateTime? CooldownTime { get; set; }
    }

    public class AccountInfo
    {
        [JsonProperty(PropertyName = "server")]
        public Server Server { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "gameAccountId")]
        public int GameAccountId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "lastPlayed")]
        public DateTime LastPlayed { get; set; }

        [JsonProperty(PropertyName = "lastLogin")]
        public DateTime LastLogin { get; set; }

        [JsonProperty(PropertyName = "blocked")]
        public bool Blocked { get; set; }

        [JsonProperty(PropertyName = "bannedUntil")]
        public DateTime? BannedUntil { get; set; }

        [JsonProperty(PropertyName = "bannedReason")]
        public string BannedReason { get; set; }

        [JsonProperty(PropertyName = "details")]
        public List<Detail> Details { get; set; }

        [JsonProperty(PropertyName = "sitting")]
        public Sitting Sitting { get; set; }

        [JsonProperty(PropertyName = "trading")]
        public Trading Trading { get; set; }
    }

    public class Settings
    {
        [JsonProperty(PropertyName = "aks")]
        public int Aks;

        [JsonProperty(PropertyName = "wreckField")]
        public int WreckField;

        [JsonProperty(PropertyName = "serverLabel")]
        public string ServerLabel;

        [JsonProperty(PropertyName = "economySpeed")]
        public int EconomySpeed;

        [JsonProperty(PropertyName = "planetFields")]
        public int PlanetFields;

        [JsonProperty(PropertyName = "universeSize")]
        public int UniverseSize;

        [JsonProperty(PropertyName = "fleetSpeedWar")]
        public int FleetSpeedWar;

        [JsonProperty(PropertyName = "serverCategory")]
        public string ServerCategory;

        [JsonProperty(PropertyName = "fleetSpeedHolding")]
        public int FleetSpeedHolding;

        [JsonProperty(PropertyName = "fleetSpeedPeaceful")]
        public int FleetSpeedPeaceful;

        [JsonProperty(PropertyName = "espionageProbeRaids")]
        public int EspionageProbeRaids;

        [JsonProperty(PropertyName = "premiumValidationGift")]
        public int PremiumValidationGift;

        [JsonProperty(PropertyName = "debrisFieldFactorShips")]
        public int DebrisFieldFactorShips;

        [JsonProperty(PropertyName = "researchDurationDivisor")]
        public double ResearchDurationDivisor;

        [JsonProperty(PropertyName = "debrisFieldFactorDefence")]
        public int DebrisFieldFactorDefence;
    }

    public class ServerInfo
    {
        [JsonProperty(PropertyName = "language")]
        public string Language;

        [JsonProperty(PropertyName = "number")]
        public int Number;

        [JsonProperty(PropertyName = "name")]
        public string Name;

        [JsonProperty(PropertyName = "playerCount")]
        public int PlayerCount;

        [JsonProperty(PropertyName = "playersOnline")]
        public int PlayersOnline;

        [JsonProperty(PropertyName = "opened")]
        public DateTime Opened;

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate;

        [JsonProperty(PropertyName = "endDate")]
        public object EndDate;

        [JsonProperty(PropertyName = "serverClosed")]
        public int ServerClosed;

        [JsonProperty(PropertyName = "prefered")]
        public int Prefered;

        [JsonProperty(PropertyName = "signupClosed")]
        public int SignupClosed;

        [JsonProperty(PropertyName = "settings")]
        public Settings Settings;
    }
}
