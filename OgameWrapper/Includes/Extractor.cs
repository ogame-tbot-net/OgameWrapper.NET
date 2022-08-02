using System.Text.RegularExpressions;
using System.Web;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using OgameWrapper.Model;

namespace OgameWrapper.Includes
{
    internal static class Extractor
    {
        internal static string GetPlayerName(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-player-name']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return element.GetAttribute("content");
            }
            return string.Empty;
        }
        internal static int GetPlayerID(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-player-id']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return int.Parse(element.GetAttribute("content"));
            }
            return 0;
        }
        internal static string GetServerName(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-universe']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return element.GetAttribute("content");
            }
            return string.Empty;
        }
        internal static int GetCurrentCelestialID(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-planet-id']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return int.Parse(element.GetAttribute("content"));
            }
            return 0;
        }
        internal static string GetCurrentCelestialName(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-planet-name']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return element.GetAttribute("content");
            }
            return string.Empty;
        }
        internal static CelestialType GetCurrentCelestialType(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-planet-type']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return element.GetAttribute("content") switch
                {
                    "planet" => CelestialType.Planet,
                    "moon" => CelestialType.Moon,
                    _ => CelestialType.Planet,
                };
            }
            return CelestialType.Planet;
        }
        internal static Coordinate? GetCurrentCelestialCoordinates(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-planet-coordinates']");
            if (element != null && element.GetAttribute("content") != null)
            {
                var tokens = element.GetAttribute("content").Split(':');
                return new()
                {
                    Galaxy = uint.Parse(tokens[0]),
                    System = uint.Parse(tokens[1]),
                    Position = uint.Parse(tokens[2]),
                    Type = GetCurrentCelestialType(content),
                };
            }
            return null;
        }
        internal static bool IsInVacationMode(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            if (doc.QuerySelector("div#advice-bar a") != null && doc.QuerySelector("div#advice-bar a").GetAttribute("href") != null)
            {
                var url = new Uri(doc.QuerySelector("div#advice-bar a").GetAttribute("href"));
                var query = HttpUtility.ParseQueryString(url.Query);
                if (query.AllKeys.Any(q => q.Contains("component")) && query.AllKeys.Any(q => q.Contains("selectedTab")) && query.AllKeys.Any(q => q.Contains("openGroup")))
                {
                    if (query.GetValues("component").First() == "preferences" && query.GetValues("selectedTab").First() == "3" && query.GetValues("openGroup").First() == "0")
                        return true;
                }
            }
            return false;
        }
        internal static string GetOgameVersion(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-version']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return element.GetAttribute("content");
            }
            return "";
        }
        internal static bool IsV9(string content)
        {
            return GetOgameVersion(content).StartsWith("9");
        }
        internal static bool IsV8(string content)
        {
            return GetOgameVersion(content).StartsWith("8");
        }
        internal static int GetEconomySpeed(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-universe-speed']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return int.Parse(element.GetAttribute("content"));
            }
            return 1;
        }
        internal static int GetFleetSpeedPeaceful(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-universe-speed-fleet-peaceful']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return int.Parse(element.GetAttribute("content"));
            }
            return 1;
        }
        internal static int GetFleetSpeedWar(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-universe-speed-fleet-war']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return int.Parse(element.GetAttribute("content"));
            }
            return 1;
        }
        internal static int GetFleetSpeedHolding(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-universe-speed-fleet-holding']");
            if (element != null && element.GetAttribute("content") != null)
            {
                return int.Parse(element.GetAttribute("content"));
            }
            return 1;
        }
        internal static PlayerClass GetPlayerClass(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector(".sprite.characterclass.medium");
            if (element != null)
            {
                if (element.ClassList.Any(c => c == "warrior"))
                {
                    return PlayerClass.General;
                }
                else if (element.ClassList.Any(c => c == "explorer"))
                {
                    return PlayerClass.Discoverer;
                }
                else if (element.ClassList.Any(c => c == "miner"))
                {
                    return PlayerClass.Collector;
                }
                return PlayerClass.NoClass;
            }
            return PlayerClass.NoClass;
        }
        internal static bool IsUnderAttack(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("#attack_alert");
            if (element != null)
            {
                if (element.ClassList.Any(c => c == "soon"))
                {
                    return true;
                }
                else if (element.ClassList.Any(c => c == "noAttack"))
                {
                    return false;
                }
                return false;
            }
            return false;
        }
        internal static Staff GetStaff(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var output = new Staff();
            var element = doc.QuerySelector("#officers");

            var commander = false;
            var admiral = false;
            var engineer = false;
            var geologist = false;
            var technocrat = false;

            if (element != null)
            {
                foreach (var el in element.Children)
                {
                    if (el.ClassList.Any(c => c == "commander") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        commander = true;
                    }
                    if (el.ClassList.Any(c => c == "admiral") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        admiral = true;
                    }
                    if (el.ClassList.Any(c => c == "engineer") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        engineer = true;
                    }
                    if (el.ClassList.Any(c => c == "geologist") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        geologist = true;
                    }
                    if (el.ClassList.Any(c => c == "technocrat") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        technocrat = true;
                    }
                }
            }
            return new()
            {
                Commander = commander,
                Admiral = admiral,
                Engineer = engineer,
                Geologist = geologist,
                Technocrat = technocrat,
            };
        }
        internal static List<Celestial> GetCelestials(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var output = new List<Celestial>();
            var element = doc.QuerySelector("#planetList");
            if (element == null)
            {
                return output;
            }

            foreach (var el in element.Children.Where(e => e.ClassList.Any(c => c == "smallplanet")))
            {
                var planetId = uint.Parse(el.Id.Substring(7));
                var planetElement = el.Children.First();
                var planetImg = planetElement.Children.Single(el => el.ClassList.Any(c => c == "planetPic")).GetAttribute("src");
                var details = planetElement.GetAttribute("title");
                var detailsDoc = parser.ParseDocument(details);
                var planetName = details.Substring(3, details.IndexOf('[') - 4);
                var coordString = details.Substring(details.IndexOf('[') + 1, details.IndexOf(']') - details.IndexOf('[') - 1);
                var coordTokens = coordString.Split(':');
                Coordinate planetCoordinate = new()
                {
                    Galaxy = uint.Parse(coordTokens[0]),
                    System = uint.Parse(coordTokens[1]),
                    Position = uint.Parse(coordTokens[2]),
                    Type = CelestialType.Planet,
                };

                var planetDiameter = 0u;
                Fields planetFields = new();
                Temperature planetTemperature = new();
                Lifeform lifeform = Lifeform.None;

                if (IsV8(content))
                {
                    planetDiameter = uint.Parse(details.Substring(details.IndexOf("<br/>") + 5, details.IndexOf("km") - (details.IndexOf("<br/>") + 5)).Replace(".", string.Empty));
                    var fieldsString = details.Substring(details.IndexOf('(') + 1, details.IndexOf(')') - details.IndexOf('(') - 1).Replace("<span class='overmark' >", string.Empty).Replace("</span>", string.Empty);
                    planetFields = new()
                    {
                        Built = uint.Parse(fieldsString.Substring(0, fieldsString.IndexOf('/'))),
                        Total = uint.Parse(fieldsString.Substring(fieldsString.IndexOf('/') + 1, fieldsString.Length - (fieldsString.IndexOf('/') + 1)))
                    };

                    var temperatureString = details.Substring(details.IndexOf("<br>") + 4, details.LastIndexOf("°C") - (details.IndexOf("<br>") + 4));
                    var minString = temperatureString.Substring(temperatureString.IndexOf(temperatureString.First(c => c == '-' || char.IsDigit(c))), temperatureString.IndexOf("°C")).Replace("°C", string.Empty).Trim();
                    var maxString = temperatureString.Substring(temperatureString.LastIndexOf(" "), temperatureString.Length - temperatureString.LastIndexOf(" ")).Replace("°C", string.Empty).Trim();
                    planetTemperature = new()
                    {
                        Min = int.Parse(minString),
                        Max = int.Parse(maxString)
                    };
                }
                else if (IsV9(content))
                {
                    string lifeformString = details.Substring(details.IndexOf("<br/>") + 5, (details.NthIndexOf("<br/>", 2) - details.IndexOf("<br/>") - 5));
                    Enum.TryParse(lifeformString.Substring(lifeformString.IndexOf(":") + 2), out lifeform);

                    planetDiameter = uint.Parse(details.Substring(details.NthIndexOf("<br/>", 2) + 5, details.IndexOf("km") - (details.NthIndexOf("<br/>", 2) + 5)).Replace(".", string.Empty));
                    var fieldsString = details.Substring(details.IndexOf('(') + 1, details.IndexOf(')') - details.IndexOf('(') - 1).Replace("<span class='overmark' >", string.Empty).Replace("</span>", string.Empty);
                    planetFields = new()
                    {
                        Built = uint.Parse(fieldsString.Substring(0, fieldsString.IndexOf('/'))),
                        Total = uint.Parse(fieldsString.Substring(fieldsString.IndexOf('/') + 1, fieldsString.Length - (fieldsString.IndexOf('/') + 1)))
                    };

                    var temperatureString = details.Substring(details.IndexOf("<br>") + 4, details.LastIndexOf("°C") - (details.IndexOf("<br>") + 4));
                    var minString = temperatureString.Substring(temperatureString.IndexOf(temperatureString.First(c => c == '-' || char.IsDigit(c))), temperatureString.IndexOf("°C")).Replace("°C", string.Empty).Trim();
                    var maxString = temperatureString.Substring(temperatureString.LastIndexOf(" "), temperatureString.Length - temperatureString.LastIndexOf(" ")).Replace("°C", string.Empty).Trim();
                    planetTemperature = new()
                    {
                        Min = int.Parse(minString),
                        Max = int.Parse(maxString)
                    };
                }

                Moon moon = new();

                var moonElement = el.Children.FirstOrDefault(el => el.ClassList.Any(c => c == "moonlink"));
                if (moonElement != null)
                {
                    details = moonElement.GetAttribute("title");
                    detailsDoc = parser.ParseDocument(details);
                    var moonId = uint.Parse(details.Substring(details.IndexOf("cp=") + 3, 8));
                    var moonName = details.Substring(3, details.IndexOf('[') - 4);
                    var moonImg = moonElement.Children.Single(el => el.ClassList.Any(c => c == "icon-moon")).GetAttribute("src");

                    coordString = details.Substring(details.IndexOf('[') + 1, details.IndexOf(']') - details.IndexOf('[') - 1);
                    coordTokens = coordString.Split(':');
                    Coordinate moonCoordinate = new()
                    {
                        Galaxy = uint.Parse(coordTokens[0]),
                        System = uint.Parse(coordTokens[1]),
                        Position = uint.Parse(coordTokens[2]),
                        Type = CelestialType.Moon,
                    };

                    var moonDiameter = uint.Parse(details.Substring(details.IndexOf("<br>") + 4, details.IndexOf("km") - (details.IndexOf("<br>") + 4)).Replace(".", string.Empty));
                    var moonFieldsString = details.Substring(details.IndexOf('(') + 1, details.IndexOf(')') - details.IndexOf('(') - 1).Replace("<span class='overmark' >", string.Empty).Replace("</span>", string.Empty);
                    Fields moonFields = new()
                    {
                        Built = uint.Parse(moonFieldsString.Substring(0, moonFieldsString.IndexOf('/'))),
                        Total = uint.Parse(moonFieldsString.Substring(moonFieldsString.IndexOf('/') + 1, moonFieldsString.Length - (moonFieldsString.IndexOf('/') + 1)))
                    };

                    moon = new()
                    {
                        Id = moonId,
                        Name = moonName,
                        Img = moonImg,
                        Coordinate = moonCoordinate,
                        Diameter = moonDiameter,
                        Fields = moonFields,
                    };

                    output.Add(moon);
                }

                Planet planet = new()
                {
                    Id = planetId,
                    Name = planetName,
                    Img = planetImg,
                    Coordinate = planetCoordinate,
                    Diameter = planetDiameter,
                    Fields = planetFields,
                    Moon = moon,
                    Lifeform = lifeform,
                };
                output.Add(planet);
            }

            return output
                .OrderBy(c => c.Coordinate.Galaxy)
                .ThenBy(c => c.Coordinate.System)
                .ThenBy(c => c.Coordinate.Position)
                .ThenByDescending(c => c.Coordinate.Type == CelestialType.Planet)
                .ToList();
        }

        internal static Techs GetTechs(string content)
        {
            Dictionary<Buildable, uint> decodedJson = new();

            try
            {
                decodedJson = JsonConvert.DeserializeObject<Dictionary<Buildable, uint>>(content);
            }
            catch { }

            return new Techs
            {
                Buildings = new()
                {
                    MetalMine = decodedJson.GetValueOrDefault(Buildable.MetalMine, 0u),
                    CrystalMine = decodedJson.GetValueOrDefault(Buildable.CrystalMine, 0u),
                    DeuteriumSynthesizer = decodedJson.GetValueOrDefault(Buildable.DeuteriumSynthesizer, 0u),
                    SolarPlant = decodedJson.GetValueOrDefault(Buildable.SolarPlant, 0u),
                    FusionReactor = decodedJson.GetValueOrDefault(Buildable.FusionReactor, 0u),
                    MetalStorage = decodedJson.GetValueOrDefault(Buildable.MetalStorage, 0u),
                    CrystalStorage = decodedJson.GetValueOrDefault(Buildable.CrystalStorage, 0u),
                    DeuteriumTank = decodedJson.GetValueOrDefault(Buildable.DeuteriumTank, 0u),
                    SolarSatellite = decodedJson.GetValueOrDefault(Buildable.SolarSatellite, 0u)
                },
                Facilities = new()
                {
                    RoboticsFactory = decodedJson.GetValueOrDefault(Buildable.RoboticsFactory, 0u),
                    ResearchLab = decodedJson.GetValueOrDefault(Buildable.ResearchLab, 0u),
                    Shipyard = decodedJson.GetValueOrDefault(Buildable.Shipyard, 0u),
                    MissileSilo = decodedJson.GetValueOrDefault(Buildable.MissileSilo, 0u),
                    NaniteFactory = decodedJson.GetValueOrDefault(Buildable.NaniteFactory, 0u),
                    SpaceDock = decodedJson.GetValueOrDefault(Buildable.SpaceDock, 0u),
                    Terraformer = decodedJson.GetValueOrDefault(Buildable.Terraformer, 0u),
                    AllianceDepot = decodedJson.GetValueOrDefault(Buildable.AllianceDepot, 0u),
                    LunarBase = decodedJson.GetValueOrDefault(Buildable.LunarBase, 0u),
                    SensorPhalanx = decodedJson.GetValueOrDefault(Buildable.SensorPhalanx, 0u),
                    JumpGate = decodedJson.GetValueOrDefault(Buildable.JumpGate, 0u)
                },
                Ships = new()
                {
                    LightFighter = decodedJson.GetValueOrDefault(Buildable.LightFighter, 0u),
                    HeavyFighter = decodedJson.GetValueOrDefault(Buildable.HeavyFighter, 0u),
                    Cruiser = decodedJson.GetValueOrDefault(Buildable.Cruiser, 0u),
                    Pathfinder = decodedJson.GetValueOrDefault(Buildable.Pathfinder, 0u),
                    Battleship = decodedJson.GetValueOrDefault(Buildable.Battleship, 0u),
                    Battlecruiser = decodedJson.GetValueOrDefault(Buildable.Battlecruiser, 0u),
                    Bomber = decodedJson.GetValueOrDefault(Buildable.Bomber, 0u),
                    Destroyer = decodedJson.GetValueOrDefault(Buildable.Destroyer, 0u),
                    Reaper = decodedJson.GetValueOrDefault(Buildable.Reaper, 0u),
                    Deathstar = decodedJson.GetValueOrDefault(Buildable.Deathstar, 0u),
                    SmallCargo = decodedJson.GetValueOrDefault(Buildable.SmallCargo, 0u),
                    LargeCargo = decodedJson.GetValueOrDefault(Buildable.LargeCargo, 0u),
                    Recycler = decodedJson.GetValueOrDefault(Buildable.Recycler, 0u),
                    ColonyShip = decodedJson.GetValueOrDefault(Buildable.ColonyShip, 0u),
                    EspionageProbe = decodedJson.GetValueOrDefault(Buildable.EspionageProbe, 0u),
                    Crawler = decodedJson.GetValueOrDefault(Buildable.Crawler, 0u),
                    SolarSatellite = decodedJson.GetValueOrDefault(Buildable.SolarSatellite, 0u)
                },
                Defences = new()
                {
                    RocketLauncher = decodedJson.GetValueOrDefault(Buildable.RocketLauncher, 0u),
                    LightLaser = decodedJson.GetValueOrDefault(Buildable.LightLaser, 0u),
                    HeavyLaser = decodedJson.GetValueOrDefault(Buildable.HeavyLaser, 0u),
                    IonCannon = decodedJson.GetValueOrDefault(Buildable.IonCannon, 0u),
                    GaussCannon = decodedJson.GetValueOrDefault(Buildable.GaussCannon, 0u),
                    PlasmaTurret = decodedJson.GetValueOrDefault(Buildable.PlasmaTurret, 0u),
                    SmallShieldDome = decodedJson.GetValueOrDefault(Buildable.SmallShieldDome, 0u),
                    LargeShieldDome = decodedJson.GetValueOrDefault(Buildable.LargeShieldDome, 0u),
                    AntiBallisticMissiles = decodedJson.GetValueOrDefault(Buildable.AntiBallisticMissiles, 0u),
                    InterplanetaryMissiles = decodedJson.GetValueOrDefault(Buildable.InterplanetaryMissiles, 0u)
                },
                Researches = new()
                {
                    EnergyTechnology = decodedJson.GetValueOrDefault(Buildable.EnergyTechnology, 0u),
                    LaserTechnology = decodedJson.GetValueOrDefault(Buildable.LaserTechnology, 0u),
                    IonTechnology = decodedJson.GetValueOrDefault(Buildable.IonTechnology, 0u),
                    HyperspaceTechnology = decodedJson.GetValueOrDefault(Buildable.HyperspaceTechnology, 0u),
                    PlasmaTechnology = decodedJson.GetValueOrDefault(Buildable.PlasmaTechnology, 0u),
                    EspionageTechnology = decodedJson.GetValueOrDefault(Buildable.EspionageTechnology, 0u),
                    ComputerTechnology = decodedJson.GetValueOrDefault(Buildable.ComputerTechnology, 0u),
                    Astrophysics = decodedJson.GetValueOrDefault(Buildable.Astrophysics, 0u),
                    IntergalacticResearchNetwork = decodedJson.GetValueOrDefault(Buildable.IntergalacticResearchNetwork, 0u),
                    GravitonTechnology = decodedJson.GetValueOrDefault(Buildable.GravitonTechnology, 0u),
                    CombustionDrive = decodedJson.GetValueOrDefault(Buildable.CombustionDrive, 0u),
                    ImpulseDrive = decodedJson.GetValueOrDefault(Buildable.ImpulseDrive, 0u),
                    HyperspaceDrive = decodedJson.GetValueOrDefault(Buildable.HyperspaceDrive, 0u),
                    WeaponsTechnology = decodedJson.GetValueOrDefault(Buildable.WeaponsTechnology, 0u),
                    ShieldingTechnology = decodedJson.GetValueOrDefault(Buildable.ShieldingTechnology, 0u),
                    ArmourTechnology = decodedJson.GetValueOrDefault(Buildable.ArmourTechnology, 0u)
                },
                LifeformBuildings = new()
                {
                    ResidentialSector = decodedJson.GetValueOrDefault(Buildable.ResidentialSector, 0u),
                    BiosphereFarm = decodedJson.GetValueOrDefault(Buildable.BiosphereFarm, 0u),
                    ResearchCentre = decodedJson.GetValueOrDefault(Buildable.ResearchCentre, 0u),
                    AcademyOfSciences = decodedJson.GetValueOrDefault(Buildable.AcademyOfSciences, 0u),
                    NeuroCalibrationCentre = decodedJson.GetValueOrDefault(Buildable.NeuroCalibrationCentre, 0u),
                    HighEnergySmelting = decodedJson.GetValueOrDefault(Buildable.HighEnergySmelting, 0u),
                    FoodSilo = decodedJson.GetValueOrDefault(Buildable.FoodSilo, 0u),
                    FusionPoweredProduction = decodedJson.GetValueOrDefault(Buildable.FusionPoweredProduction, 0u),
                    Skyscraper = decodedJson.GetValueOrDefault(Buildable.Skyscraper, 0u),
                    BiotechLab = decodedJson.GetValueOrDefault(Buildable.BiotechLab, 0u),
                    Metropolis = decodedJson.GetValueOrDefault(Buildable.Metropolis, 0u),
                    PlanetaryShield = decodedJson.GetValueOrDefault(Buildable.PlanetaryShield, 0u)
                },
                LifeformResearches = new()
                {
                    IntergalacticEnvoys = decodedJson.GetValueOrDefault(Buildable.IntergalacticEnvoys, 0u),
                    HighPerformanceExtractors = decodedJson.GetValueOrDefault(Buildable.HighPerformanceExtractors, 0u),
                    FusionDrives = decodedJson.GetValueOrDefault(Buildable.FusionDrives, 0u),
                    StealthFieldGenerator = decodedJson.GetValueOrDefault(Buildable.StealthFieldGenerator, 0u),
                    OrbitalDen = decodedJson.GetValueOrDefault(Buildable.OrbitalDen, 0u),
                    ResearchAI = decodedJson.GetValueOrDefault(Buildable.ResearchAI, 0u),
                    HighPerformanceTerraformer = decodedJson.GetValueOrDefault(Buildable.HighPerformanceTerraformer, 0u),
                    EnhancedProductionTechnologies = decodedJson.GetValueOrDefault(Buildable.EnhancedProductionTechnologies, 0u),
                    LightFighterMkII = decodedJson.GetValueOrDefault(Buildable.LightFighterMkII, 0u),
                    CruiserMkII = decodedJson.GetValueOrDefault(Buildable.CruiserMkII, 0u),
                    ImprovedLabTechnology = decodedJson.GetValueOrDefault(Buildable.ImprovedLabTechnology, 0u),
                    PlasmaTerraformer = decodedJson.GetValueOrDefault(Buildable.PlasmaTerraformer, 0u),
                    LowTemperatureDrives = decodedJson.GetValueOrDefault(Buildable.LowTemperatureDrives, 0u),
                    BomberMkII = decodedJson.GetValueOrDefault(Buildable.BomberMkII, 0u),
                    DestroyerMkII = decodedJson.GetValueOrDefault(Buildable.DestroyerMkII, 0u),
                    BattlecruiserMkII = decodedJson.GetValueOrDefault(Buildable.BattlecruiserMkII, 0u),
                    RobotAssistants = decodedJson.GetValueOrDefault(Buildable.RobotAssistants, 0u),
                    Supercomputer = decodedJson.GetValueOrDefault(Buildable.Supercomputer, 0u),
                }
            };
        }
        internal static Resources GetResources(string content)
        {
            var resProd = GetResourcesProduction(content);
            return new Resources
            {
                Metal = resProd.Metal.Available,
                Crystal = resProd.Crystal.Available,
                Deuterium = resProd.Deuterium.Available,
                Energy = resProd.Energy.Available,
                Darkmatter = resProd.Darkmatter.Available,
                Population = resProd.Population.Available,
                Food = resProd.Food.Available
            };
        }
        private static string CleanString(string stringToClean)
        {
            return stringToClean
                .Replace("<span class='overmark' >", string.Empty)
                .Replace("<span class='middlemark' >", string.Empty)
                .Replace("<span class=\"\" >", string.Empty)
                .Replace("</span>", string.Empty)
                .Replace(".", string.Empty)
                .Replace(",", string.Empty);
        }
        internal static ResourcesProduction GetResourcesProduction(string content)
        {
            dynamic decodedJson = JsonConvert.DeserializeObject(content);
            var parser = new HtmlParser();
            IHtmlDocument metalTooltip = parser.ParseDocument(decodedJson.resources.metal.tooltip.ToString());
            IHtmlDocument crystalTooltip = parser.ParseDocument(decodedJson.resources.crystal.tooltip.ToString());
            IHtmlDocument deuteriumTooltip = parser.ParseDocument(decodedJson.resources.deuterium.tooltip.ToString());
            IHtmlDocument energyTooltip = parser.ParseDocument(decodedJson.resources.energy.tooltip.ToString());
            IHtmlDocument darkmatterTooltip = parser.ParseDocument(decodedJson.resources.darkmatter.tooltip.ToString());

            Resource metal = new()
            {
                Available = decodedJson.resources.metal.amount,
                StorageCapacity = decodedJson.resources.metal.storage,
                CurrentProduction = ulong.Parse(CleanString(metalTooltip.QuerySelector("table tr:nth-child(3) td").TextContent)),
                DenCapacity = ulong.Parse(CleanString(metalTooltip.QuerySelector("table tr:nth-child(4) td").TextContent))
            };
            Resource crystal = new()
            {
                Available = decodedJson.resources.crystal.amount,
                StorageCapacity = decodedJson.resources.crystal.storage,
                CurrentProduction = ulong.Parse(CleanString(crystalTooltip.QuerySelector("table tr:nth-child(3) td").TextContent)),
                DenCapacity = ulong.Parse(CleanString(crystalTooltip.QuerySelector("table tr:nth-child(4) td").TextContent))
            };
            Resource deuterium = new()
            {
                Available = decodedJson.resources.deuterium.amount,
                StorageCapacity = decodedJson.resources.deuterium.storage,
                CurrentProduction = ulong.Parse(CleanString(deuteriumTooltip.QuerySelector("table tr:nth-child(3) td").TextContent)),
                DenCapacity = ulong.Parse(CleanString(deuteriumTooltip.QuerySelector("table tr:nth-child(4) td").TextContent))
            };
            Energy energy = new()
            {
                Available = decodedJson.resources.energy.amount,
                Consumption = ulong.Parse(CleanString(energyTooltip.QuerySelector("table tr:nth-child(2) td").TextContent)),
                CurrentProduction = ulong.Parse(CleanString(energyTooltip.QuerySelector("table tr:nth-child(3) td").TextContent))
            };
            Darkmatter darkmatter = new()
            {
                Available = decodedJson.resources.darkmatter.amount,
                Purchased = ulong.Parse(CleanString(darkmatterTooltip.QuerySelector("table tr:nth-child(2) td").TextContent)),
                Found = ulong.Parse(CleanString(darkmatterTooltip.QuerySelector("table tr:nth-child(3) td").TextContent))
            };
            Population population = new()
            {
                Available = (ulong)Math.Round(float.Parse((string)(decodedJson.resources.population.amount))),
                StorageCapacity = decodedJson.resources.population.storage,
                SafeCapacity = decodedJson.resources.population.safeCapacity,
                GrowthRate = decodedJson.resources.population.growthRate,
                CapableToFeed = decodedJson.resources.population.capableToFeed,
                NeedFood = decodedJson.resources.population.needFood,
                SingleFoodConsumption = decodedJson.resources.population.singleFoodConsumption
            };
            Food food = new()
            {
                Available = decodedJson.resources.food.amount,
                StorageCapacity = decodedJson.resources.food.storage,
                CapableToFeed = decodedJson.resources.food.capableToFeed,
                CurrentProduction = decodedJson.resources.food.production,
                ExtraProduction = decodedJson.resources.food.extraproduction,
                Consumption = decodedJson.resources.food.consumption,
            };
            return new()
            {
                Metal = metal,
                Crystal = crystal,
                Deuterium = deuterium,
                Energy = energy,
                Darkmatter = darkmatter,
                Population = population,
                Food = food
            };
        }
        internal static ResourceSettings GetResourcesSettings(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var tokens = doc.QuerySelectorAll("option").Where(e => e.Attributes.Any(a => a.Name == "selected")).ToArray();
            return new ResourceSettings
            {
                MetalMine = (ResourceSettingsPercent)int.Parse(tokens[0].GetAttribute("value")),
                CrystalMine = (ResourceSettingsPercent)int.Parse(tokens[1].GetAttribute("value")),
                DeuteriumSynthesizer = (ResourceSettingsPercent)int.Parse(tokens[2].GetAttribute("value")),
                SolarPlant = (ResourceSettingsPercent)int.Parse(tokens[3].GetAttribute("value")),
                FusionReactor = (ResourceSettingsPercent)int.Parse(tokens[4].GetAttribute("value")),
                SolarSatellite = (ResourceSettingsPercent)int.Parse(tokens[5].GetAttribute("value")),
                Crawler = (ResourceSettingsPercent)int.Parse(tokens[6].GetAttribute("value")),
            };
        }
        internal static Slots GetSlots(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var searchString = doc.QuerySelector("#slots").TextContent;
            var r = Regex.Matches(searchString, "[0-9]+/[0-9]+");
            var fleetTokens = r[0].Value.Split('/');
            var expTokens = r[1].Value.Split('/');
            return new()
            {
                InUse = uint.Parse(fleetTokens[0]),
                Total = uint.Parse(fleetTokens[1]),
                ExpInUse = uint.Parse(expTokens[0]),
                ExpTotal = uint.Parse(expTokens[1])
            };
        }
    }
}
