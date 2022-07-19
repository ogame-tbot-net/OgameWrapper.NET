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

    public record Server
    {
        public string Language { get; init; } = string.Empty;

        public int Number { get; init; } = 0;
    }

    public record Detail
    {
        public string Type { get; init; } = string.Empty;

        public string Title { get; init; } = string.Empty;

        public string Value { get; init; } = string.Empty;
    }

    public class Sitting
    {
        public bool Generated { get; init; } = false;

        public DateTime? EndTime { get; init; } = null;

        public DateTime? CooldownTime { get; init; } = null;
    }

    public class Trading
    {
        public bool Generated { get; init; } = false;

        public DateTime? CooldownTime { get; init; } = null;
    }

    public class AccountInfo
    {
        public Server Server { get; init; } = new();

        public int Id { get; init; } = 0;

        public int GameAccountId { get; init; } = 0;

        public string Name { get; init; } = string.Empty;

        public DateTime LastPlayed { get; init; } = DateTime.MinValue;

        public DateTime LastLogin { get; init; } = DateTime.MinValue;

        public bool Blocked { get; init; } = false;

        public DateTime? BannedUntil { get; init; } = null;

        public string BannedReason { get; init; } = string.Empty;

        public List<Detail> Details { get; init; } = new();

        public Sitting Sitting { get; init; } = new();

        public Trading Trading { get; init; } = new();
    }

    public class Settings
    {
        public bool Aks { get; init; } = false;

        public bool WreckField { get; init; } = false;

        public string ServerLabel { get; init; } = string.Empty;

        public int EconomySpeed { get; init; } = 0;

        public int PlanetFields { get; init; } = 0;

        public int UniverseSize { get; init; } = 0;

        public int FleetSpeedWar { get; init; } = 0;

        public string ServerCategory { get; init; } = string.Empty;

        public int FleetSpeedHolding { get; init; } = 0;

        public int FleetSpeedPeaceful { get; init; } = 0;

        public bool EspionageProbeRaids { get; init; } = false;

        public int PremiumValidationGift { get; init; } = 0;

        public int DebrisFieldFactorShips { get; init; } = 0;

        public double ResearchDurationDivisor { get; init; } = 0;

        public int DebrisFieldFactorDefence { get; init; } = 0;
    }

    public class ServerInfo
    {
        public string Language { get; init; } = string.Empty;

        public int Number { get; init; } = 0;

        public string AccountGroup { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public int PlayerCount { get; init; } = 0;

        public int PlayersOnline { get; init; } = 0;

        public DateTime Opened { get; init; } = DateTime.MinValue;

        public DateTime StartDate { get; init; } = DateTime.MinValue;

        public DateTime? EndDate { get; init; } = null;

        public bool ServerClosed { get; init; } = false;

        public bool Prefered { get; init; } = false;

        public bool SignupClosed { get; init; } = false;

        public bool MultiLanguage { get; init; } = false;

        public List<string> AvailableOn { get; init; } = new();

        public Settings Settings { get; init; } = new();
    }
}
