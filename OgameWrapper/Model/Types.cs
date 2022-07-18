using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OgameWrapper.Model
{
	public class Coordinate
	{
		public Coordinate(int galaxy = 1, int system = 1, int position = 1, CelestialTypes type = CelestialTypes.Planet)
		{
			Galaxy = galaxy;
			System = system;
			Position = position;
			Type = type;
		}
		public int Galaxy { get; set; }
		public int System { get; set; }
		public int Position { get; set; }
		public CelestialTypes Type { get; set; }

		public override string ToString()
		{
			return $"[{GetCelestialCode()}:{Galaxy}:{System}:{Position}]";
		}

		private string GetCelestialCode()
		{
			return Type switch
			{
				CelestialTypes.Planet => "P",
				CelestialTypes.Debris => "DF",
				CelestialTypes.Moon => "M",
				CelestialTypes.DeepSpace => "DS",
				_ => "",
			};
		}

		public bool IsSame(Coordinate otherCoord)
		{
			return Galaxy == otherCoord.Galaxy
				&& System == otherCoord.System
				&& Position == otherCoord.Position
				&& Type == otherCoord.Type;
		}
	}

	public class Fields
	{
		public int Built { get; set; }
		public int Total { get; set; }
		public int Free
		{
			get
			{
				return Total - Built;
			}
		}
		public bool IsFull {
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

	public class Temperature
	{
		public int Min { get; set; }
		public int Max { get; set; }
		public float Average
		{
			get
			{
				return (float)(Min + Max) / 2;
			}
		}
		public override string ToString()
		{
			return Min.ToString() + "°C ~ " + Max.ToString() + "°C";
		}
	}

	public class ResourceSettings
	{
		public ResourceSettingsPercents? MetalMine { get; set; }
		public ResourceSettingsPercents CrystalMine { get; set; }
		public ResourceSettingsPercents DeuteriumSynthesizer { get; set; }
		public ResourceSettingsPercents SolarPlant { get; set; }
		public ResourceSettingsPercents FusionReactor { get; set; }
		public ResourceSettingsPercents? SolarSatellite { get; set; }
		public ResourceSettingsPercents? Crawler { get; set; }
	}

	public class Celestial
	{
		public int ID { get; set; }
		public string? Img { get; set; }
		public string? Name { get; set; }
		public int? Diameter { get; set; }
		public int? Activity { get; set; }
		public Coordinate? Coordinate { get; set; }
		public Fields? Fields { get; set; }
		public Resources? Resources { get; set; }
		public Ships? Ships { get; set; }
		public Defences? Defences { get; set; }
		public Buildings? Buildings { get; set; }
		public Facilities? Facilities { get; set; }
		public List<Production>? Productions { get; set; }
		public Constructions? Constructions { get; set; }
		public ResourceSettings? ResourceSettings { get; set; }
		public ResourcesProduction? ResourcesProduction { get; set; }
		public GalaxyDebris? Debris { get; set; }

		public override string ToString()
		{
			return $"{Name} {Coordinate}";
		}

		public bool HasProduction()
		{
			if (Productions == null) return false;
			return Productions.Count != 0;
		}

		internal bool HasConstruction()
		{
			if (Constructions == null) return false;
			return Constructions.BuildingID != (int)Buildables.Null;
		}

		public bool HasCoords(Coordinate coords)
		{
			if (Coordinate == null) return false;
			return coords.Galaxy == Coordinate.Galaxy
				&& coords.System == Coordinate.System
				&& coords.Position == Coordinate.Position
				&& coords.Type == Coordinate.Type;
		}

		public int? GetLevel(Buildables building)
		{
			int? output = 0;
			if (building == Buildables.Null) return output;
			if (Buildings == null) return output;
			foreach (PropertyInfo prop in Buildings.GetType().GetProperties())
			{
				if (prop.Name == building.ToString())
				{
					output = (int?)prop.GetValue(Buildings);
				}
			}
			if (output == 0)
			{
				if (Facilities == null) return output;
				foreach (PropertyInfo prop in Facilities.GetType().GetProperties())
				{
					if (prop.Name == building.ToString())
					{
						output = (int?)prop.GetValue(Facilities);
					}
				}
			}
			return output;
		}
		public void GetTechs(OgameWrapperClient client)
		{
			var techs = client.GetTechs(this);
			Buildings = techs.Buildings;
			Facilities = techs.Facilities;
			Ships = techs.Ships;
			Defences = techs.Defences;
		}
		public void GetBuildings(OgameWrapperClient client)
		{
			var techs = client.GetTechs(this);
			Buildings = techs.Buildings;
		}
		public void GetFacilities(OgameWrapperClient client)
		{
			var techs = client.GetTechs(this);
			Facilities = techs.Facilities;
		}
		public void GetShips(OgameWrapperClient client)
		{
			var techs = client.GetTechs(this);
			Ships = techs.Ships;
		}
		public void GetDefences(OgameWrapperClient client)
		{
			var techs = client.GetTechs(this);
			Defences = techs.Defences;
		}
		public void GetResources(OgameWrapperClient client)
        {
			var resources = client.GetResources(this);
			Resources = resources;
        }
		public void GetResourcesProduction(OgameWrapperClient client)
		{
			var resourcesProduction = client.GetResourcesProduction(this);
			ResourcesProduction = resourcesProduction;
		}
		public void GetResourceSettings(OgameWrapperClient client)
        {
			var resourcesSettings = client.GetResourceSettings(this);
			ResourceSettings = resourcesSettings;
        }
	}

	public class Moon : Celestial
	{
		public bool HasLunarFacilities(Facilities facilities)
		{
			if (Facilities == null) return false;
			return Facilities.LunarBase >= facilities.LunarBase
				&& Facilities.SensorPhalanx >= facilities.SensorPhalanx
				&& Facilities.JumpGate >= facilities.JumpGate
				&& Facilities.Shipyard >= facilities.Shipyard
				&& Facilities.RoboticsFactory >= facilities.RoboticsFactory;
		}
	}

	public class Planet : Celestial
	{
		public bool Administrator { get; set; }
		public bool Inactive { get; set; }
		public bool Vacation { get; set; }
		public bool StrongPlayer { get; set; }
		public bool Newbie { get; set; }
		public bool HonorableTarget { get; set; }
		public bool Banned { get; set; }
		public Player? Player { get; set; }
		public Alliance? Alliance { get; set; }
		public Temperature? Temperature { get; set; }
		public Moon? Moon { get; set; }

		public bool HasMines(Buildings buildings)
		{
			if (Buildings == null) return false;
			return Buildings.MetalMine >= buildings.MetalMine
				&& Buildings.CrystalMine >= buildings.CrystalMine
				&& Buildings.DeuteriumSynthesizer >= buildings.DeuteriumSynthesizer;
		}
	}
	public class ServerData
	{
		public string Name { get; set; }
		public int Number { get; set; }
		public string Language { get; set; }
		public string Timezone { get; set; }
		public string TimezoneOffset { get; set; }
		public string Domain { get; set; }
		public string Version { get; set; }
		public int Speed { get; set; }
		public int SpeedFleet { get; set; }
		public int SpeedFleetPeaceful { get; set; }
		public int SpeedFleetWar { get; set; }
		public int SpeedFleetHolding { get; set; }
		public int Galaxies { get; set; }
		public int Systems { get; set; }
		public bool ACS { get; set; }
		public bool RapidFire { get; set; }
		public bool DefToTF { get; set; }
		public float DebrisFactor { get; set; }
		public float DebrisFactorDef { get; set; }
		public float RepairFactor { get; set; }
		public int NewbieProtectionLimit { get; set; }
		public int NewbieProtectionHigh { get; set; }
		public long TopScore { get; set; }
		public int BonusFields { get; set; }
		public bool DonutGalaxy { get; set; }
		public bool DonutSystem { get; set; }
		public bool WfEnabled { get; set; }
		public int WfMinimumRessLost { get; set; }
		public int WfMinimumLossPercentage { get; set; }
		public int WfBasicPercentageRepairable { get; set; }
		public float GlobalDeuteriumSaveFactor { get; set; }
		public int Bashlimit { get; set; }
		public int ProbeCargo { get; set; }
		public int ResearchDurationDivisor { get; set; }
		public int DarkMatterNewAcount { get; set; }
		public int CargoHyperspaceTechMultiplier { get; set; }
	}

	public class UserInfo
	{
		public int PlayerID { get; set; }
		public string? PlayerName { get; set; }
		public long Points { get; set; }
		public long Rank { get; set; }
		public long Total { get; set; }
		public long HonourPoints { get; set; }
        public PlayerClasses Class { get; set; }
	}

	public class Resources
	{
		public Resources(long metal = 0, long crystal = 0, long deuterium = 0, long? energy = null, long? darkmatter = null)
		{
			Metal = metal;
			Crystal = crystal;
			Deuterium = deuterium;
			Energy = energy;
			Darkmatter = darkmatter;
		}
		public long Metal { get; set; }
		public long Crystal { get; set; }
		public long Deuterium { get; set; }
		public long? Energy { get; set; }
		public long? Darkmatter { get; set; }        
        public long ConvertedDeuterium
		{
			get
            {
				return GetConvertedDeuterium();
            }
		}
		public long GetConvertedDeuterium(double metalRatio = 2.5, double crystalRatio = 1.5)
		{
			return (long)Math.Round((Metal / metalRatio) + (Crystal / crystalRatio) + Deuterium, 0, MidpointRounding.ToPositiveInfinity);
		}

		public long TotalResources
		{
			get
			{
				return Metal + Crystal + Deuterium;
			}
		}

		public long StructuralIntegrity
		{
			get
			{
				return Metal + Crystal;
			}
		}

		public override string ToString()
		{
			if (Energy != null && Darkmatter != null)
				return $"M: {Metal:N0} C: {Crystal:N0} D: {Deuterium:N0} E: {Energy:N0} DM: {Darkmatter:N0}";
			else
				return $"M: {Metal:N0} C: {Crystal:N0} D: {Deuterium:N0}";
		}

		public bool IsEnoughFor(Resources cost, int times = 1, Resources? resToLeave = null)
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

		public Resources Sum(Resources resourcesToSum)
		{
			Resources output = new();
			output.Metal = Metal + resourcesToSum.Metal;
			output.Crystal = Crystal + resourcesToSum.Crystal;
			output.Deuterium = Deuterium + resourcesToSum.Deuterium;

			return output;
		}

		public Resources Difference(Resources resourcesToSubtract)
		{
			Resources output = new();
			output.Metal = Metal - resourcesToSubtract.Metal;
			if (output.Metal < 0)
				output.Metal = 0;
			output.Crystal = Crystal - resourcesToSubtract.Crystal;
			if (output.Crystal < 0)
				output.Crystal = 0;
			output.Deuterium = Deuterium - resourcesToSubtract.Deuterium;
			if (output.Deuterium < 0)
				output.Deuterium = 0;

			return output;
		}
	}

	public class Buildings : IBuildable
	{
		public int MetalMine { get; set; }
		public int CrystalMine { get; set; }
		public int DeuteriumSynthesizer { get; set; }
		public int SolarPlant { get; set; }
		public int FusionReactor { get; set; }
		public int SolarSatellite { get; set; }
		public int MetalStorage { get; set; }
		public int CrystalStorage { get; set; }
		public int DeuteriumTank { get; set; }
		public Buildings(
			int metalMine = 0,
			int crystalMine = 0,
			int deuteriumSynthesizer = 0,
			int solarPlant = 0,
			int fusionReactor = 0,
			int solarSatellite = 0,
			int metalStorage = 0,
			int crystalStorage = 0,
			int deuteriumTank = 0
		)
        {
			MetalMine = metalMine;
			CrystalMine = crystalMine;
			DeuteriumSynthesizer = deuteriumSynthesizer;
			SolarPlant = solarPlant;
			FusionReactor = fusionReactor;
			SolarSatellite = solarSatellite;
			MetalStorage = metalStorage;
			CrystalStorage = crystalStorage;
			DeuteriumTank = deuteriumTank;
        }

		public override string ToString()
		{
			return $"M: {MetalMine} C: {CrystalMine} D: {DeuteriumSynthesizer} S: {SolarPlant} F: {FusionReactor}";
		}

		public int GetLevel(Buildables buildable)
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

		public Buildings SetLevel(Buildables buildable, int level)
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

	public class Facilities : IBuildable
	{
		public int RoboticsFactory { get; set; }
		public int Shipyard { get; set; }
		public int? ResearchLab { get; set; }
		public int? AllianceDepot { get; set; }
		public int? MissileSilo { get; set; }
		public int? NaniteFactory { get; set; }
		public int? Terraformer { get; set; }
		public int? SpaceDock { get; set; }
		public int? LunarBase { get; set; }
		public int? SensorPhalanx { get; set; }
		public int? JumpGate { get; set; }
		public Facilities(
			int roboticsFactory = 0,
			int shipyard = 0,
			int? researchLab = 0,
			int? allianceDepot = 0,
			int? missileSilo = 0,
			int? naniteFactory = 0,
			int? terraformer = 0,
			int? spaceDock = 0,
			int? lunarBase = 0,
			int? sensorPhalanx = null,
			int? jumpGate = null
		)
		{
			RoboticsFactory = roboticsFactory;
			Shipyard = shipyard;
			ResearchLab = researchLab;
			AllianceDepot = allianceDepot;
			MissileSilo = missileSilo;
			NaniteFactory = naniteFactory;
			Terraformer = terraformer;
			SpaceDock = spaceDock;
			LunarBase = lunarBase;
			SensorPhalanx = sensorPhalanx;
			JumpGate = jumpGate;
		}

		public override string ToString()
		{
			return $"R: {RoboticsFactory} S: {Shipyard} L: {ResearchLab} M: {MissileSilo} N: {NaniteFactory}";
		}
	}

	public interface IBuildable
	{
		public int GetLevel(Buildables buildable)
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

		public IBuildable SetLevel(Buildables buildable, int level)
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

	public class Defences : IAmountable
	{
		public long RocketLauncher { get; set; }
		public long LightLaser { get; set; }
		public long HeavyLaser { get; set; }
		public long GaussCannon { get; set; }
		public long IonCannon { get; set; }
		public long PlasmaTurret { get; set; }
		public long SmallShieldDome { get; set; }
		public long LargeShieldDome { get; set; }
		public long AntiBallisticMissiles { get; set; }
		public long InterplanetaryMissiles { get; set; }
		public Defences(
			long rocketLauncher = 0,
			long lightLaser = 0,
			long heavyLaser = 0,
			long gaussCannon = 0,
			long ionCannon = 0,
			long plasmaTurret = 0,
			long smallShieldDome = 0,
			long largeShieldDome = 0,
			long antiBallisticMissiles = 0,
			long interplanetaryMissiles = 0
        )
        {
			RocketLauncher = rocketLauncher;
			LightLaser = lightLaser;
			HeavyLaser = heavyLaser;
			GaussCannon = gaussCannon;
			IonCannon = ionCannon;
			PlasmaTurret = plasmaTurret;
			SmallShieldDome = smallShieldDome;
			LargeShieldDome = largeShieldDome;
			AntiBallisticMissiles = antiBallisticMissiles;
			InterplanetaryMissiles= interplanetaryMissiles;
		}
	}

	public interface IAmountable {
		public int GetAmount(Buildables buildable)
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
		public bool SetAmount(Buildables buildable, long number)
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

	public class Ships : IAmountable
	{
		public long LightFighter { get; set; }
		public long HeavyFighter { get; set; }
		public long Cruiser { get; set; }
		public long Battleship { get; set; }
		public long Battlecruiser { get; set; }
		public long Bomber { get; set; }
		public long Destroyer { get; set; }
		public long Deathstar { get; set; }
		public long SmallCargo { get; set; }
		public long LargeCargo { get; set; }
		public long ColonyShip { get; set; }
		public long Recycler { get; set; }
		public long EspionageProbe { get; set; }
		public long SolarSatellite { get; set; }
		public long Crawler { get; set; }
		public long Reaper { get; set; }
		public long Pathfinder { get; set; }

		public Ships(
			long lightFighter = 0,
			long heavyFighter = 0,
			long cruiser = 0,
			long battleship = 0,
			long battlecruiser = 0,
			long bomber = 0,
			long destroyer = 0,
			long deathstar = 0,
			long smallCargo = 0,
			long largeCargo = 0,
			long colonyShip = 0,
			long recycler = 0,
			long espionageProbe = 0,
			long solarSatellite = 0,
			long crawler = 0,
			long reaper = 0,
			long pathfinder = 0
		)
		{
			LightFighter = lightFighter;
			HeavyFighter = heavyFighter;
			Cruiser = cruiser;
			Battleship = battleship;
			Battlecruiser = battlecruiser;
			Bomber = bomber;
			Destroyer = destroyer;
			Deathstar = deathstar;
			SmallCargo = smallCargo;
			LargeCargo = largeCargo;
			ColonyShip = colonyShip;
			Recycler = recycler;
			EspionageProbe = espionageProbe;
			SolarSatellite = solarSatellite;
			Crawler = crawler;
			Reaper = reaper;
			Pathfinder = pathfinder;
		}

		public long GetFleetPoints()
		{
			long output = 0;
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
			Ships tempShips = this;
			tempShips.SolarSatellite = 0;
			tempShips.Crawler = 0;
			return tempShips;
		}

		public Ships Add(Buildables buildable, long quantity)
		{
			foreach (PropertyInfo prop in this.GetType().GetProperties())
			{
				if (prop.Name == buildable.ToString() && prop.GetValue(this) != null)
				{
					prop.SetValue(this, (long)prop.GetValue(this) + quantity);
				}
			}
			return this;
		}

		public Ships Remove(Buildables buildable, int quantity)
		{
			foreach (PropertyInfo prop in this.GetType().GetProperties())
			{
				if (prop.Name == buildable.ToString() && prop.GetValue(this) != null)
				{
					long val = (long)prop.GetValue(this);
					if (val >= quantity)
						prop.SetValue(this, val);
					else
						prop.SetValue(this, 0);
				}
			}
			return this;
		}

		public bool HasAtLeast(Ships ships, long times = 1)
		{
			foreach (PropertyInfo prop in this.GetType().GetProperties())
			{
				if (prop.GetValue(this) != null && prop.GetValue(ships) != null && (long)prop.GetValue(this) * times < (long)prop.GetValue(ships))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			string output = "";
			foreach (PropertyInfo prop in this.GetType().GetProperties())
			{
				if (prop.GetValue(this) == null || (long)prop.GetValue(this) == 0)
					continue;
				output += $"{prop.Name}: {prop.GetValue(this)}; ";
			}
			return output[0..^2];
		}
	}

	public class FleetPrediction
	{
		public long Time { get; set; }
		public long Fuel { get; set; }
	}

	public class Fleet
	{
		public Missions Mission { get; set; }
		public bool ReturnFlight { get; set; }
		public bool InDeepSpace { get; set; }
		public int ID { get; set; }
		public Resources Resources { get; set; }
		public Coordinate Origin { get; set; }
		public Coordinate Destination { get; set; }
		public Ships Ships { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime ArrivalTime { get; set; }
		public DateTime? BackTime { get; set; }
		public int ArriveIn { get; set; }
		public int? BackIn { get; set; }
		public int? UnionID { get; set; }
		public int TargetPlanetID { get; set; }
	}

	public class AttackerFleet
	{
		public int ID { get; set; }
		public Missions MissionType { get; set; }
		public Coordinate Origin { get; set; }
		public Coordinate Destination { get; set; }
		public string DestinationName { get; set; }
		public DateTime ArrivalTime { get; set; }
		public int ArriveIn { get; set; }
		public string AttackerName { get; set; }
		public int AttackerID { get; set; }
		public int UnionID { get; set; }
		public int Missiles { get; set; }
		public Ships Ships { get; set; }

		public bool IsOnlyProbes()
		{
			if (Ships.EspionageProbe != 0)
			{
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
			else
				return false;
		}
	}
	public class Slots
	{
		public int InUse { get; set; }
		public int Total { get; set; }
		public int ExpInUse { get; set; }
		public int ExpTotal { get; set; }
		public int Free
		{
			get
			{
				return Total - InUse;
			}
		}
		public int ExpFree
		{
			get
			{
				return ExpTotal - ExpInUse;
			}
		}
	}

	public class Researches : IBuildable
	{
		public int EnergyTechnology { get; set; }
		public int LaserTechnology { get; set; }
		public int IonTechnology { get; set; }
		public int HyperspaceTechnology { get; set; }
		public int PlasmaTechnology { get; set; }
		public int CombustionDrive { get; set; }
		public int ImpulseDrive { get; set; }
		public int HyperspaceDrive { get; set; }
		public int EspionageTechnology { get; set; }
		public int ComputerTechnology { get; set; }
		public int Astrophysics { get; set; }
		public int IntergalacticResearchNetwork { get; set; }
		public int GravitonTechnology { get; set; }
		public int WeaponsTechnology { get; set; }
		public int ShieldingTechnology { get; set; }
		public int ArmourTechnology { get; set; }
		public Researches(
			int energyTechnology = 0,
			int laserTechnology = 0,
			int ionTechnology = 0,
			int hyperspaceTechnology = 0,
			int plasmaTechnology = 0,
			int combustionDrive = 0,
			int impulseDrive = 0,
			int hyperspaceDrive = 0,
			int espionageTechnology = 0,
			int computerTechnology = 0,
			int astrophysics = 0,
			int intergalacticResearchNetwork = 0,
			int gravitonTechnology = 0,
			int weaponsTechnology = 0,
			int shieldingTechnology = 0,
			int armourTechnology = 0
		)
        {
			EnergyTechnology = energyTechnology;
			LaserTechnology = laserTechnology;
			IonTechnology = ionTechnology;
			HyperspaceTechnology = hyperspaceTechnology;
			PlasmaTechnology = plasmaTechnology;
			CombustionDrive = combustionDrive;
			ImpulseDrive = impulseDrive;
			HyperspaceDrive = hyperspaceDrive;
			EspionageTechnology = espionageTechnology;
			ComputerTechnology = computerTechnology;
			Astrophysics = astrophysics;
			IntergalacticResearchNetwork = intergalacticResearchNetwork;
			GravitonTechnology = gravitonTechnology;
			WeaponsTechnology = weaponsTechnology;
			ShieldingTechnology = shieldingTechnology;
			ArmourTechnology = armourTechnology;
		}
	}

	public class Production
	{
		public int ID { get; set; }
		public int Nbr { get; set; }
	}

	public class Constructions
	{
		public int BuildingID { get; set; }
		public int BuildingCountdown { get; set; }
		public int ResearchID { get; set; }
		public int ResearchCountdown { get; set; }
	}

	public class GalaxyDebris : Debris
	{
		public long RecyclersNeeded { get; set; }
	}

	public class ExpeditionDebris : Debris
	{
		public long PathfindersNeeded { get; set; }		
	}
	public class Debris
    {
		public long Metal { get; set; }
		public long Crystal { get; set; }
		public Resources Resources
		{
			get
			{
				return new Resources
				{
					Metal = Metal,
					Crystal = Crystal,
					Deuterium = 0,
					Darkmatter = null,
					Energy = null
				};
			}
		}
	}

	public class Player
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int Rank { get; set; }
		public bool IsBandit { get; set; }
		public bool IsStarlord { get; set; }
		public PlayerClasses Class { get; set; }
	}

	public class Alliance
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int Rank { get; set; }
		public int Member { get; set; }
		public AllianceClasses Class { get; set; }
	}

	public class GalaxyInfo
	{
		public int Galaxy { get; set; }
		public int System { get; set; }
		public List<Planet>? Planets { get; set; }
		public ExpeditionDebris? ExpeditionDebris { get; set; }
	}

	public class FleetHypotesis
	{
		public Celestial Origin { get; set; }
		public Coordinate Destination { get; set; }
		public Ships Ships { get; set; }
		public Missions Mission { get; set; }
		public decimal Speed { get; set; }
		public long Duration { get; set; }
		public long Fuel { get; set; }
	}

	public class Staff
	{
		public Staff()
		{
			Commander = false;
			Admiral = false;
			Engineer = false;
			Geologist = false;
			Technocrat = false;
		}
		public bool Commander { get; set; }
		public bool Admiral { get; set; }
		public bool Engineer { get; set; }
		public bool Geologist { get; set; }
		public bool Technocrat { get; set; }
		public bool IsFull
		{
			get
			{
				return Commander && Admiral && Engineer && Geologist && Technocrat;
			}
		}
	}

	public class Resource : BaseResource
	{
		public long StorageCapacity { get; set; }
		public long CurrentProduction { get; set; }
		public long DenCapacity { get; set; }
	}

	public class Energy : BaseResource
	{
		public long CurrentProduction { get; set; }
		public long Consumption { get; set; }
	}

	public class Darkmatter : BaseResource
	{
		public long Purchased { get; set; }
		public long Found { get; set; }
	}

	public abstract class BaseResource
    {
		public long Available { get; set; }
	}

	public class ResourcesProduction
	{
		public Resource Metal { get; set; }
		public Resource Crystal { get; set; }
		public Resource Deuterium { get; set; }
		public Energy Energy { get; set; }
		public Darkmatter Darkmatter { get; set; }
		public ResourcesProduction(
			Resource metal,
			Resource crystal,
			Resource deuterium,
			Energy energy,
			Darkmatter darkmatter
		)
        {
			Metal = metal;
			Crystal = crystal;
			Deuterium = deuterium;
			Energy = energy;
			Darkmatter = darkmatter;
        }
	}

	public class Techs
    {
		public Buildings Buildings { get; set; }
		public Facilities Facilities { get; set; }
		public Ships Ships { get; set; }
		public Defences Defences { get; set; }
		public Researches Researches { get; set; }
    }

	public class AutoMinerSettings
	{
		public bool OptimizeForStart { get; set; }
		public bool PrioritizeRobotsAndNanites { get; set; }
		public float MaxDaysOfInvestmentReturn { get; set; }
		public int DepositHours { get; set; }
		public bool BuildDepositIfFull { get; set; }
		public int DeutToLeaveOnMoons { get; set; }

		public AutoMinerSettings()
		{
			OptimizeForStart = true;
			PrioritizeRobotsAndNanites = false;
			MaxDaysOfInvestmentReturn = 36500;
			DepositHours = 6;
			BuildDepositIfFull = false;
			DeutToLeaveOnMoons = 1000000;
		}
	}
}
