using System.Reflection;

namespace OgameWrapper.Model
{
    public record Coordinate
    {
        public uint Galaxy { get; init; }

        public uint System { get; init; }

        public uint Position { get; init; }

        public CelestialType Type { get; init; }

        public override string ToString()
        {
            return $"[{GetCelestialCode()}:{Galaxy}:{System}:{Position}]";
        }

        private string GetCelestialCode()
        {
            return Type switch
            {
                CelestialType.Planet => "P",
                CelestialType.Debris => "DF",
                CelestialType.Moon => "M",
                CelestialType.DeepSpace => "DS",
                _ => "",
            };
        }

        public bool IsSame(Coordinate coord)
        {
            return Galaxy == coord.Galaxy
                && System == coord.System
                && Position == coord.Position
                && Type == coord.Type;
        }
    }

    public record Fields
    {
        public uint Built { get; init; }

        public uint Total { get; init; }

        public uint Free
        {
            get
            {
                return Total - Built;
            }
        }

        public bool IsFull
        {
            get
            {
                return Built == Total;
            }
        }

        public override string ToString()
        {
            return Built.ToString() + "/" + Total.ToString();
        }
    }

    public record Temperature
    {
        public int Min { get; init; }

        public int Max { get; init; }

        public double Average
        {
            get
            {
                return (Min + Max) / 2d;
            }
        }

        public override string ToString()
        {
            return Min.ToString() + "°C ~ " + Max.ToString() + "°C";
        }
    }

    public record ResourceSettings
    {
        public ResourceSettingsPercent MetalMine { get; set; }

        public ResourceSettingsPercent CrystalMine { get; set; }

        public ResourceSettingsPercent DeuteriumSynthesizer { get; set; }

        public ResourceSettingsPercent SolarPlant { get; set; }

        public ResourceSettingsPercent FusionReactor { get; set; }

        public ResourceSettingsPercent SolarSatellite { get; set; }

        public ResourceSettingsPercent Crawler { get; set; }
    }

    public record Celestial
    {
        public uint Id { get; init; }

        public string Img { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public uint Diameter { get; init; }

        public uint Activity { get; init; }

        public Coordinate Coordinate { get; init; } = new();

        public Fields Fields { get; init; } = new();

        public Resources Resources { get; set; } = new();

        public Ships Ships { get; set; } = new();

        public Defences Defences { get; set; } = new();

        public Buildings Buildings { get; set; } = new();

        public Facilities Facilities { get; set; } = new();

        public List<Production> Productions { get; init; } = new();

        public Constructions Constructions { get; init; } = new();

        public ResourceSettings ResourceSettings { get; set; } = new();

        public ResourcesProduction ResourcesProduction { get; set; } = new();

        public GalaxyDebris Debris { get; init; } = new();

        public override string ToString()
        {
            return $"{Name} {Coordinate}";
        }

        public bool HasProduction()
        {
            if (Productions == null)
            {
                return false;
            }

            return Productions.Count != 0;
        }

        internal bool HasConstruction()
        {
            if (Constructions == null)
            {
                return false;
            }

            return Constructions.BuildingId != (int)Buildable.Null;
        }

        public bool HasCoords(Coordinate coords)
        {
            if (Coordinate == null)
            {
                return false;
            }

            return coords.Galaxy == Coordinate.Galaxy
                && coords.System == Coordinate.System
                && coords.Position == Coordinate.Position
                && coords.Type == Coordinate.Type;
        }

        public uint GetLevel(Buildable building)
        {
            var output = 0u;

            if (building == Buildable.Null)
            {
                return output;
            }

            if (Buildings == null)
            {
                return output;
            }

            foreach (PropertyInfo prop in Buildings.GetType().GetProperties())
            {
                if (prop.Name == building.ToString())
                {
                    output = (uint)prop.GetValue(Buildings);
                }
            }

            if (output == 0)
            {
                if (Facilities == null)
                {
                    return output;
                }

                foreach (PropertyInfo prop in Facilities.GetType().GetProperties())
                {
                    if (prop.Name == building.ToString())
                    {
                        output = (uint)prop.GetValue(Facilities);
                    }
                }
            }

            return output;
        }

        public async Task GetTechs(OgameClient client)
        {
            var techs = await client.GetTechs(this);
            Buildings = techs.Buildings;
            Facilities = techs.Facilities;
            Ships = techs.Ships;
            Defences = techs.Defences;
        }

        public async Task GetBuildings(OgameClient client)
        {
            var techs = await client.GetTechs(this);
            Buildings = techs.Buildings;
        }

        public async Task GetFacilities(OgameClient client)
        {
            var techs = await client.GetTechs(this);
            Facilities = techs.Facilities;
        }

        public async Task GetShips(OgameClient client)
        {
            var techs = await client.GetTechs(this);
            Ships = techs.Ships;
        }

        public async Task GetDefences(OgameClient client)
        {
            var techs = await client.GetTechs(this);
            Defences = techs.Defences;
        }

        public async Task GetResources(OgameClient client)
        {
            var resources = await client.GetResources(this);
            Resources = resources;
        }
    }

    public record Moon : Celestial
    {
        public bool HasLunarFacilities(Facilities facilities)
        {
            if (Facilities == null)
            {
                return false;
            }

            return Facilities.LunarBase >= facilities.LunarBase
                && Facilities.SensorPhalanx >= facilities.SensorPhalanx
                && Facilities.JumpGate >= facilities.JumpGate
                && Facilities.Shipyard >= facilities.Shipyard
                && Facilities.RoboticsFactory >= facilities.RoboticsFactory;
        }
    }

    public record Planet : Celestial
    {
        public Lifeform Lifeform { get; init; }

        public LifeformBuildings LifeformBuildings { get; set; } = new();

        public LifeformResearches LifeformResearches { get; set; } = new();

        public bool Administrator { get; init; }

        public bool Inactive { get; init; }

        public bool Vacation { get; init; }

        public bool StrongPlayer { get; init; }

        public bool Newbie { get; init; }

        public bool HonorableTarget { get; init; }

        public bool Banned { get; init; }

        public Player Player { get; init; } = new();

        public Alliance Alliance { get; init; } = new();

        public Temperature Temperature { get; init; } = new();

        public Moon Moon { get; init; } = new();

        public async Task GetResourcesProduction(OgameClient client)
        {
            var resourcesProduction = await client.GetResourcesProduction(this);
            ResourcesProduction = resourcesProduction;
        }

        public async Task GetResourceSettings(OgameClient client)
        {
            var resourcesSettings = await client.GetResourceSettings(this);
            ResourceSettings = resourcesSettings;
        }

        public async Task GetLifeformBuildings(OgameClient client)
        {
            var lifeformBuildings = await client.GetLifeformBuildings(this);
            LifeformBuildings = lifeformBuildings;
        }

        public async Task GetLifeformResearches(OgameClient client)
        {
            var lifeformResearches = await client.GetLifeformResearches(this);
            LifeformResearches = lifeformResearches;
        }

        public bool HasMines(Buildings buildings)
        {
            if (Buildings == null)
            {
                return false;
            }

            return Buildings.MetalMine >= buildings.MetalMine
                && Buildings.CrystalMine >= buildings.CrystalMine
                && Buildings.DeuteriumSynthesizer >= buildings.DeuteriumSynthesizer;
        }
    }
    
    public record ServerData
    {
        public string Name { get; init; } = string.Empty;

        public uint Number { get; init; }

        public string Language { get; init; } = string.Empty;

        public string Timezone { get; init; } = string.Empty;

        public string TimezoneOffset { get; init; } = string.Empty;

        public string Domain { get; init; } = string.Empty;

        public string Version { get; init; } = string.Empty;

        public uint Speed { get; init; }

        public uint SpeedFleet { get; init; }

        public uint SpeedFleetPeaceful { get; init; }

        public uint SpeedFleetWar { get; init; }

        public uint SpeedFleetHolding { get; init; }

        public uint Galaxies { get; init; }

        public uint Systems { get; init; }

        public bool ACS { get; init; }

        public bool RapidFire { get; init; }

        public bool DefToTF { get; init; }

        public double DebrisFactor { get; init; }

        public double DebrisFactorDef { get; init; }

        public double RepairFactor { get; init; }

        public uint NewbieProtectionLimit { get; init; }

        public uint NewbieProtectionHigh { get; init; }

        public double TopScore { get; init; }

        public uint BonusFields { get; init; }

        public bool DonutGalaxy { get; init; }

        public bool DonutSystem { get; init; }

        public bool WfEnabled { get; init; }

        public uint WfMinimumRessLost { get; init; }

        public uint WfMinimumLossPercentage { get; init; }

        public uint WfBasicPercentageRepairable { get; init; }

        public double GlobalDeuteriumSaveFactor { get; init; }

        public uint Bashlimit { get; init; }

        public uint ProbeCargo { get; init; }

        public double ResearchDurationDivisor { get; init; }

        public uint DarkMatterNewAcount { get; init; }

        public uint CargoHyperspaceTechMultiplier { get; init; }
    }

    public record UserInfo
    {
        public uint PlayerId { get; init; }

        public string PlayerName { get; init; } = string.Empty;

        public ulong Points { get; init; }

        public ulong Rank { get; init; }

        public ulong Total { get; init; }

        public ulong HonourPoints { get; init; }

        public PlayerClass Class { get; init; }
    }

    public record Resources
    {
        public ulong Metal { get; init; }

        public ulong Crystal { get; init; }

        public ulong Deuterium { get; init; }

        public ulong Energy { get; init; }

        public ulong Darkmatter { get; init; }

        public ulong Population { get; init; }

        public ulong Food { get; init; }

        public ulong ConvertedDeuterium
        {
            get
            {
                return GetConvertedDeuterium();
            }
        }

        public ulong GetConvertedDeuterium(double metalRatio = 2.5, double crystalRatio = 1.5)
        {
            return (ulong)Math.Round((Metal / metalRatio) + (Crystal / crystalRatio) + Deuterium, 0, MidpointRounding.ToPositiveInfinity);
        }

        public ulong TotalResources
        {
            get
            {
                return Metal + Crystal + Deuterium;
            }
        }

        public ulong StructuralIntegrity
        {
            get
            {
                return Metal + Crystal;
            }
        }

        public override string ToString()
        {
            if (Energy != 0 && Darkmatter != 0)
            {
                return $"M: {Metal:N0} C: {Crystal:N0} D: {Deuterium:N0} E: {Energy:N0} DM: {Darkmatter:N0}";
            }

            return $"M: {Metal:N0} C: {Crystal:N0} D: {Deuterium:N0}";
        }

        public bool IsEnoughFor(Resources cost, uint times = 1, Resources? resToLeave = null)
        {
            var tempMet = Metal;
            var tempCry = Crystal;
            var tempDeut = Deuterium;

            if (resToLeave != null)
            {
                tempMet -= resToLeave.Metal;
                tempCry -= resToLeave.Crystal;
                tempDeut -= resToLeave.Deuterium;
            }

            return cost.Metal * times <= tempMet && cost.Crystal * times <= tempCry && cost.Deuterium * times <= tempDeut;
        }

        public bool IsEmpty()
        {
            return Metal == 0 && Crystal == 0 && Deuterium == 0;
        }

        public static Resources operator +(Resources a, Resources b)
        {
            return new()
            {
                Metal = a.Metal + b.Metal,
                Crystal = a.Crystal + b.Crystal,
                Deuterium = a.Deuterium + b.Deuterium,
            };
        }

        public static Resources operator -(Resources a, Resources b)
        {
            return new()
            {
                Metal = Math.Max(a.Metal - b.Metal, 0),
                Crystal = Math.Max(a.Crystal - b.Crystal, 0),
                Deuterium = Math.Max(a.Deuterium - b.Deuterium, 0),
            };
        }
    }

    public record Buildings : IBuildable
    {
        public uint MetalMine { get; init; }

        public uint CrystalMine { get; init; }

        public uint DeuteriumSynthesizer { get; init; }

        public uint SolarPlant { get; init; }

        public uint FusionReactor { get; init; }

        public uint SolarSatellite { get; init; }

        public uint MetalStorage { get; init; }

        public uint CrystalStorage { get; init; }

        public uint DeuteriumTank { get; init; }

        public uint GetLevel(Buildable buildable)
        {
            uint output = 0;
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString() && prop.GetValue(this) != null)
                {
                    output = (uint)prop.GetValue(this);
                }
            }
            return output;
        }

        public Buildings SetLevel(Buildable buildable, uint level)
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString() && prop != null)
                {
                    prop.SetValue(this, level);
                }
            }
            return this;
        }
    }

    public record Facilities : IBuildable
    {
        public uint RoboticsFactory { get; init; }

        public uint Shipyard { get; init; }

        public uint ResearchLab { get; init; }

        public uint AllianceDepot { get; init; }

        public uint MissileSilo { get; init; }

        public uint NaniteFactory { get; init; }

        public uint Terraformer { get; init; }

        public uint SpaceDock { get; init; }

        public uint LunarBase { get; init; }

        public uint SensorPhalanx { get; init; }

        public uint JumpGate { get; init; }
    }

    public record LifeformBuildings : IBuildable
    {
        public uint ResidentialSector { get; init; }

        public uint BiosphereFarm { get; init; }

        public uint ResearchCentre { get; init; }

        public uint AcademyOfSciences { get; init; }

        public uint NeuroCalibrationCentre { get; init; }

        public uint HighEnergySmelting { get; init; }

        public uint FoodSilo { get; init; }

        public uint FusionPoweredProduction { get; init; }

        public uint Skyscraper { get; init; }

        public uint BiotechLab { get; init; }

        public uint Metropolis { get; init; }

        public uint PlanetaryShield { get; init; }
    }

    public interface IBuildable
    {
        public uint GetLevel(Buildable buildable)
        {
            uint output = 0;
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString() && prop.GetValue(this) != null)
                {
                    output = (uint)prop.GetValue(this);
                }
            }
            return output;
        }

        public IBuildable SetLevel(Buildable buildable, uint level)
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString() && prop != null)
                {
                    prop.SetValue(this, level);
                }
            }
            return this;
        }
    }

    public record Defences : IAmountable
    {
        public ulong RocketLauncher { get; init; }

        public ulong LightLaser { get; init; }

        public ulong HeavyLaser { get; init; }

        public ulong GaussCannon { get; init; }

        public ulong IonCannon { get; init; }

        public ulong PlasmaTurret { get; init; }

        public ulong SmallShieldDome { get; init; }

        public ulong LargeShieldDome { get; init; }

        public ulong AntiBallisticMissiles { get; init; }

        public ulong InterplanetaryMissiles { get; init; }
    }

    public interface IAmountable
    {
        public int GetAmount(Buildable buildable)
        {
            int output = 0;
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString() && prop.GetValue(this) != null)
                {
                    output = (int)prop.GetValue(this);
                }
            }
            return output;
        }
        public bool SetAmount(Buildable buildable, long number)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString())
                {
                    prop.SetValue(this, number);
                    return true;
                }
            }
            return false;
        }
    }

    public record Ships : IAmountable
    {
        public ulong LightFighter { get; init; }

        public ulong HeavyFighter { get; init; }

        public ulong Cruiser { get; init; }

        public ulong Battleship { get; init; }

        public ulong Battlecruiser { get; init; }

        public ulong Bomber { get; init; }

        public ulong Destroyer { get; init; }

        public ulong Deathstar { get; init; }

        public ulong SmallCargo { get; init; }

        public ulong LargeCargo { get; init; }

        public ulong ColonyShip { get; init; }

        public ulong Recycler { get; init; }

        public ulong EspionageProbe { get; init; }

        public ulong SolarSatellite { get; init; }

        public ulong Crawler { get; init; }

        public ulong Reaper { get; init; }

        public ulong Pathfinder { get; init; }

        public Ships(Ships ships)
        {
            LightFighter = ships.LightFighter;
            HeavyFighter = ships.HeavyFighter;
            Cruiser = ships.Cruiser;
            Battleship = ships.Battleship;
            Battlecruiser = ships.Battlecruiser;
            Bomber = ships.Bomber;
            Destroyer = ships.Destroyer;
            Deathstar = ships.Deathstar;
            SmallCargo = ships.SmallCargo;
            LargeCargo = ships.LargeCargo;
            ColonyShip = ships.ColonyShip;
            Recycler = ships.Recycler;
            EspionageProbe = ships.EspionageProbe;
            SolarSatellite = ships.SolarSatellite;
            Crawler = ships.Crawler;
            Reaper = ships.Reaper;
            Pathfinder = ships.Pathfinder;
        }

        public ulong GetFleetPoints()
        {
            var output = 0ul;
            output += LightFighter * 4;
            output += HeavyFighter * 10;
            output += Cruiser * 29;
            output += Battleship * 60;
            output += Battlecruiser * 85;
            output += Bomber * 90;
            output += Destroyer * 125;
            output += Deathstar * 10000;
            output += SmallCargo * 4;
            output += LargeCargo * 12;
            output += ColonyShip * 40;
            output += Recycler * 18;
            output += EspionageProbe * 1;
            output += Reaper * 160;
            output += Pathfinder * 31;
            return output;
        }

        public bool HasMovableFleet()
        {
            return LightFighter != 0
                || HeavyFighter != 0
                || Cruiser != 0
                || Battleship != 0
                || Battlecruiser != 0
                || Bomber != 0
                || Destroyer != 0
                || Deathstar != 0
                || SmallCargo != 0
                || LargeCargo != 0
                || ColonyShip != 0
                || Recycler != 0
                || EspionageProbe != 0
                || SolarSatellite != 0
                || Crawler != 0
                || Reaper != 0
                || Pathfinder != 0;
        }

        public Ships GetMovableShips()
        {
            return new(this)
            {
                SolarSatellite = 0,
                Crawler = 0,
            };
        }

        public Ships Add(Buildable buildable, ulong quantity)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString() && prop.GetValue(this) != null)
                {
                    prop.SetValue(this, (ulong)prop.GetValue(this) + quantity);
                }
            }
            return this;
        }

        public Ships Remove(Buildable buildable, ulong quantity)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString() && prop.GetValue(this) != null)
                {
                    var val = (ulong)prop.GetValue(this);
                    if (val >= quantity)
                        prop.SetValue(this, val);
                    else
                        prop.SetValue(this, 0);
                }
            }
            return this;
        }

        public bool HasAtLeast(Ships ships, ulong times = 1)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.GetValue(this) != null && prop.GetValue(ships) != null && (ulong)prop.GetValue(this) * times < (ulong)prop.GetValue(ships))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public record FleetPrediction
    {
        public ulong Time { get; set; }

        public ulong Fuel { get; set; }
    }

    public record Fleet
    {
        public Mission Mission { get; init; } = Mission.None;

        public bool ReturnFlight { get; init; }

        public bool InDeepSpace { get; init; }

        public uint Id { get; init; }

        public Resources Resources { get; init; } = new();

        public Coordinate Origin { get; init; } = new();

        public Coordinate Destination { get; init; } = new();

        public Ships Ships { get; init; } = new();

        public DateTime StartTime { get; init; }

        public DateTime ArrivalTime { get; init; }

        public DateTime? BackTime { get; init; }

        public int ArriveIn { get; init; }

        public int? BackIn { get; init; }

        public int? UnionId { get; init; }

        public int TargetPlanetId { get; init; }
    }

    public record AttackerFleet
    {
        public uint Id { get; init; }

        public Mission MissionType { get; init; } = Mission.None;

        public Coordinate Origin { get; init; } = new();

        public Coordinate Destination { get; init; } = new();

        public string DestinationName { get; init; } = string.Empty;

        public DateTime ArrivalTime { get; init; }

        public int ArriveIn { get; init; }

        public string AttackerName { get; init; } = string.Empty;

        public int AttackerId { get; init; }

        public int UnionId { get; init; }

        public int Missiles { get; init; }

        public Ships Ships { get; init; } = new();

        public bool IsOnlyProbes()
        {
            if (Ships.EspionageProbe == 0)
            {
                return false;
            }

            return Ships.Battlecruiser == 0
                && Ships.Battleship == 0
                && Ships.Bomber == 0
                && Ships.ColonyShip == 0
                && Ships.Cruiser == 0
                && Ships.Deathstar == 0
                && Ships.Destroyer == 0
                && Ships.HeavyFighter == 0
                && Ships.LargeCargo == 0
                && Ships.LightFighter == 0
                && Ships.Pathfinder == 0
                && Ships.Reaper == 0
                && Ships.Recycler == 0
                && Ships.SmallCargo == 0;
        }
    }

    public record Slots
    {
        public uint InUse { get; init; }

        public uint Total { get; init; }

        public uint ExpInUse { get; init; }

        public uint ExpTotal { get; init; }

        public uint Free
        {
            get
            {
                return Total - InUse;
            }
        }

        public uint ExpFree
        {
            get
            {
                return ExpTotal - ExpInUse;
            }
        }
    }

    public record Researches : IBuildable
    {
        public uint EnergyTechnology { get; init; }

        public uint LaserTechnology { get; init; }

        public uint IonTechnology { get; init; }

        public uint HyperspaceTechnology { get; init; }

        public uint PlasmaTechnology { get; init; }

        public uint CombustionDrive { get; init; }

        public uint ImpulseDrive { get; init; }

        public uint HyperspaceDrive { get; init; }

        public uint EspionageTechnology { get; init; }

        public uint ComputerTechnology { get; init; }

        public uint Astrophysics { get; init; }

        public uint IntergalacticResearchNetwork { get; init; }

        public uint GravitonTechnology { get; init; }

        public uint WeaponsTechnology { get; init; }

        public uint ShieldingTechnology { get; init; }

        public uint ArmourTechnology { get; init; }
    }

    public record LifeformResearches : IBuildable
    {
        public uint IntergalacticEnvoys { get; init; }

        public uint HighPerformanceExtractors { get; init; }

        public uint FusionDrives { get; init; }

        public uint StealthFieldGenerator { get; init; }

        public uint OrbitalDen { get; init; }

        public uint ResearchAI { get; init; }

        public uint HighPerformanceTerraformer { get; init; }

        public uint EnhancedProductionTechnologies { get; init; }

        public uint LightFighterMkII { get; init; }

        public uint CruiserMkII { get; init; }

        public uint ImprovedLabTechnology { get; init; }

        public uint PlasmaTerraformer { get; init; }

        public uint LowTemperatureDrives { get; init; }

        public uint BomberMkII { get; init; }

        public uint DestroyerMkII { get; init; }

        public uint BattlecruiserMkII { get; init; }

        public uint RobotAssistants { get; init; }

        public uint Supercomputer { get; init; }
    }

    public record Production
    {
        public uint Id { get; init; }

        public ulong Nbr { get; init; }
    }

    public record Constructions
    {
        public uint BuildingId { get; init; }

        public uint BuildingCountdown { get; init; }

        public uint ResearchId { get; init; }

        public uint ResearchCountdown { get; init; }
    }

    public record GalaxyDebris : Debris
    {
        public ulong RecyclersNeeded { get; init; }
    }

    public record ExpeditionDebris : Debris
    {
        public ulong PathfindersNeeded { get; init; }
    }

    public record Debris
    {
        public ulong Metal { get; init; }

        public ulong Crystal { get; init; }

        public Resources Resources
        {
            get
            {
                return new()
                {
                    Metal = Metal,
                    Crystal = Crystal,
                };
            }
        }
    }

    public record Player
    {
        public uint Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public uint Rank { get; init; }

        public bool IsBandit { get; init; }

        public bool IsStarlord { get; init; }

        public PlayerClass Class { get; init; }
    }

    public record Alliance
    {
        public uint Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public uint Rank { get; init; }

        public uint Member { get; init; }

        public AllianceClass Class { get; init; }
    }

    public record GalaxyInfo
    {
        public uint Galaxy { get; init; }

        public uint System { get; init; }

        public List<Planet> Planets { get; init; } = new();

        public ExpeditionDebris ExpeditionDebris { get; init; } = new();
    }

    public record FleetHypotesis
    {
        public Celestial Origin { get; set; } = new();

        public Coordinate Destination { get; set; } = new();

        public Ships Ships { get; set; } = new();

        public Mission Mission { get; set; }

        public decimal Speed { get; set; }

        public ulong Duration { get; set; }

        public ulong Fuel { get; set; }
    }

    public record Staff
    {
        public bool Commander { get; init; }

        public bool Admiral { get; init; }

        public bool Engineer { get; init; }

        public bool Geologist { get; init; }

        public bool Technocrat { get; init; }

        public bool IsFull
        {
            get
            {
                return Commander && Admiral && Engineer && Geologist && Technocrat;
            }
        }
    }

    public record Resource : BaseResource
    {
        public ulong StorageCapacity { get; init; }

        public ulong CurrentProduction { get; init; }

        public ulong DenCapacity { get; init; }
    }

    public record Energy : BaseResource
    {
        public ulong CurrentProduction { get; init; }

        public ulong Consumption { get; init; }
    }

    public record Darkmatter : BaseResource
    {
        public ulong Purchased { get; init; }

        public ulong Found { get; init; }
    }

    public record Population : BaseResource
    {
        public ulong StorageCapacity { get; init; }

        public ulong SafeCapacity { get; init; }

        public double GrowthRate { get; init; }

        public ulong CapableToFeed { get; init; }

        public ulong NeedFood { get; init; }

        public double SingleFoodConsumption { get; init; }
    }

    public record Food : BaseResource
    {
        public ulong StorageCapacity { get; init; }

        public double CapableToFeed { get; init; }

        public ulong CurrentProduction { get; init; }

        public ulong ExtraProduction { get; init; }

        public ulong Consumption { get; init; }
    }

    public abstract record BaseResource
    {
        public ulong Available { get; init; }
    }

    public record ResourcesProduction
    {
        public Resource Metal { get; init; } = new();

        public Resource Crystal { get; init; } = new();

        public Resource Deuterium { get; init; } = new();

        public Energy Energy { get; init; } = new();

        public Darkmatter Darkmatter { get; init; } = new();

        public Population Population { get; init; } = new();

        public Food Food { get; init; } = new();
    }

    public record Techs
    {
        public Buildings Buildings { get; init; } = new();

        public Facilities Facilities { get; init; } = new();

        public Ships Ships { get; init; } = new();

        public Defences Defences { get; init; } = new();

        public Researches Researches { get; init; } = new();

        public LifeformBuildings LifeformBuildings { get; init; } = new();

        public LifeformResearches LifeformResearches { get; init; } = new();
    }

    public record AutoMinerSettings
    {
        public bool OptimizeForStart { get; init; }

        public bool PrioritizeRobotsAndNanites { get; init; }

        public double MaxDaysOfInvestmentReturn { get; init; }

        public uint DepositHours { get; init; }

        public bool BuildDepositIfFull { get; init; }

        public ulong DeutToLeaveOnMoons { get; init; }
    }
}
