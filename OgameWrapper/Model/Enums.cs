using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgameWrapper.Model
{
	public enum PlayerClasses
	{
		NoClass = 0,
		Collector = 1,
		General = 2,
		Discoverer = 3
	}
	public enum AllianceClasses
	{
		NoClass = 0,
		Trader = 1,
		Researchers = 2,
		Warriors = 3
	}
	public enum CelestialTypes
	{
		Planet = 1,
		Debris = 2,
		Moon = 3,
		DeepSpace = 4
	}
    public enum Lifeforms
    {
        None = 0,
        Humans = 1,
		Rocktal = 2,
		Mecha = 3,
        Kaelesh = 4
	}
	public enum Buildables
	{
		Null = 0,
		MetalMine = 1,
		CrystalMine = 2,
		DeuteriumSynthesizer = 3,
		SolarPlant = 4,
		FusionReactor = 12,
		MetalStorage = 22,
		CrystalStorage = 23,
		DeuteriumTank = 24,
		ShieldedMetalDen = 25,
		UndergroundCrystalDen = 26,
		SeabedDeuteriumDen = 27,
		AllianceDepot = 34,
		RoboticsFactory = 14,
		Shipyard = 21,
		ResearchLab = 31,
		MissileSilo = 44,
		NaniteFactory = 15,
		Terraformer = 33,
		SpaceDock = 36,
		LunarBase = 41,
		SensorPhalanx = 42,
		JumpGate = 43,
		RocketLauncher = 401,
		LightLaser = 402,
		HeavyLaser = 403,
		GaussCannon = 404,
		IonCannon = 405,
		PlasmaTurret = 406,
		SmallShieldDome = 407,
		LargeShieldDome = 408,
		AntiBallisticMissiles = 502,
		InterplanetaryMissiles = 503,
		SmallCargo = 202,
		LargeCargo = 203,
		LightFighter = 204,
		HeavyFighter = 205,
		Cruiser = 206,
		Battleship = 207,
		ColonyShip = 208,
		Recycler = 209,
		EspionageProbe = 210,
		Bomber = 211,
		SolarSatellite = 212,
		Destroyer = 213,
		Deathstar = 214,
		Battlecruiser = 215,
		Crawler = 217,
		Reaper = 218,
		Pathfinder = 219,
		EspionageTechnology = 106,
		ComputerTechnology = 108,
		WeaponsTechnology = 109,
		ShieldingTechnology = 110,
		ArmourTechnology = 111,
		EnergyTechnology = 113,
		HyperspaceTechnology = 114,
		CombustionDrive = 115,
		ImpulseDrive = 117,
		HyperspaceDrive = 118,
		LaserTechnology = 120,
		IonTechnology = 121,
		PlasmaTechnology = 122,
		IntergalacticResearchNetwork = 123,
		Astrophysics = 124,
		GravitonTechnology = 199,
		ResidentialSector= 11101,
		BiosphereFarm = 11102,
		ResearchCentre = 11103,
		AcademyOfSciences = 11104,
		NeuroCalibrationCentre = 11105,
		HighEnergySmelting = 11106,
		FoodSilo = 11107,
		FusionPoweredProduction = 11108,
		Skyscraper = 11109,
		BiotechLab = 11110,
		Metropolis = 11111,
		PlanetaryShield = 11112,
		IntergalacticEnvoys = 11201,
		HighPerformanceExtractors = 11202,
		FusionDrives = 11203,
		StealthFieldGenerator = 11204,
		OrbitalDen = 11205,
		ResearchAI = 11206,
		HighPerformanceTerraformer = 11207,
		EnhancedProductionTechnologies = 11208,
		LightFighterMkII = 11209,
		CruiserMkII = 11210,
		ImprovedLabTechnology = 11211,
		PlasmaTerraformer = 11212,
		LowTemperatureDrives = 11213,
		BomberMkII = 11214,
		DestroyerMkII = 11215,
		BattlecruiserMkII = 11216,
		RobotAssistants = 11217,
		Supercomputer = 11218,
	}

	public enum Missions
	{
		Attack = 1,
		FederalAttack = 2,
		Transport = 3,
		Deploy = 4,
		FederalDefense = 5,
		Spy = 6,
		Colonize = 7,
		Harvest = 8,
		Destroy = 9,
		MissileAttack = 10,
		Expedition = 15,
		Trade = 16
	}

	public enum ResourceSettingsPercents
    {
		HundredFiftyPercent = 150,
		HundredFourtyPercent = 140,
		HundredThirtyPercent = 130,
		HundredTwentyPercent = 120,
		HundredTenPercent = 110,
		HundredPercent = 100,
		NinetyPercent = 90,
		EightyPercent = 80,
		SeventyPercent = 70,
		SixtyPercent = 60,
		FiftyPercent = 50,
		FourtyPercent = 40,
		ThirtyPercent = 30,
		TwentyPercent = 20,
		TenPercent = 10,
		ShutDown = 0
	}

	public static class Speeds
	{
		public const decimal FivePercent = 0.5M;
		public const decimal TenPercent = 1;
		public const decimal FifteenPercent = 1.5M;
		public const decimal TwentyPercent = 2;
		public const decimal TwentyfivePercent = 2.5M;
		public const decimal ThirtyPercent = 3;
		public const decimal ThirtyfivePercent = 3.5M;
		public const decimal FourtyPercent = 4;
		public const decimal FourtyfivePercent = 4.5M;
		public const decimal FiftyPercent = 5;
		public const decimal FiftyfivePercent = 5.5M;
		public const decimal SixtyPercent = 6;
		public const decimal SixtyfivePercent = 6.5M;
		public const decimal SeventyPercent = 7;
		public const decimal SeventyfivePercent = 7.5M;
		public const decimal EightyPercent = 8;
		public const decimal EightyfivePercent = 8.5M;
		public const decimal NinetyPercent = 9;
		public const decimal NinetyfivePercent = 9.5M;
		public const decimal HundredPercent = 10;
		public static List<decimal> GetGeneralSpeedsList()
		{
			return new()
			{
				10,
				9.5M,
				9,
				8.5M,
				8,
				7.5M,
				7,
				6.5M,
				6,
				5.5M,
				5,
				4.5M,
				4,
				3.5M,
				3,
				2.5M,
				2,
				1.5M,
				1,
				0.5M
			};
		}
		public static List<decimal> GetNonGeneralSpeedsList()
		{
			return new()
			{
				10,
				9,
				8,
				7,
				6,
				5,
				4,
				3,
				2,
				1
			};
		}
	}
}
