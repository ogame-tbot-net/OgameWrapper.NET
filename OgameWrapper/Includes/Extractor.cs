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
        internal static CelestialTypes GetCurrentCelestialType(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-planet-type']");
            if (element != null && element.GetAttribute("content") != null)
            {
                switch (element.GetAttribute("content"))
                {
                    case "planet":
                        return CelestialTypes.Planet;
                    case "moon":
                        return CelestialTypes.Moon;
                    default:
                        return CelestialTypes.Planet;
                }
            }
            return CelestialTypes.Planet;
        }
        internal static Coordinate? GetCurrentCelestialCoordinates(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector("meta[name='ogame-planet-coordinates']");
            if (element != null && element.GetAttribute("content") != null)
            {
                var tokens = element.GetAttribute("content").Split(':');
                return new(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]), GetCurrentCelestialType(content));
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
        internal static PlayerClasses GetPlayerClass(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var element = doc.QuerySelector(".sprite.characterclass.medium");
            if (element != null)
            {
                if (element.ClassList.Any(c => c == "warrior"))
                {
                    return PlayerClasses.General;
                }
                else if (element.ClassList.Any(c => c == "explorer"))
                {
                    return PlayerClasses.Discoverer;
                }
                else if (element.ClassList.Any(c => c == "miner"))
                {
                    return PlayerClasses.Collector;
                }
                return PlayerClasses.NoClass;
            }
            return PlayerClasses.NoClass;
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
            if (element != null)
            {
                foreach (var el in element.Children)
                {
                    if (el.ClassList.Any(c => c == "commander") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        output.Commander = true;
                    }
                    if (el.ClassList.Any(c => c == "admiral") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        output.Admiral = true;
                    }
                    if (el.ClassList.Any(c => c == "engineer") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        output.Engineer = true;
                    }
                    if (el.ClassList.Any(c => c == "geologist") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        output.Geologist = true;
                    }
                    if (el.ClassList.Any(c => c == "technocrat") && el.ClassList.Any(c2 => c2 == "on"))
                    {
                        output.Technocrat = true;
                    }
                }
            }
            return output;
        }
        internal static List<Celestial> GetCelestials(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var output = new List<Celestial>();
            var element = doc.QuerySelector("#planetList");
            if (element != null)
            {
                foreach (var el in element.Children.Where(e => e.ClassList.Any(c => c == "smallplanet")))
                {
                    var planet = new Planet();
                    planet.ID = int.Parse(el.Id.Substring(7));
                    var planetElement = el.Children.First();
                    planet.Img = planetElement.Children.Single(el => el.ClassList.Any(c => c == "planetPic")).GetAttribute("src");
                    var details = planetElement.GetAttribute("title");
                    var detailsDoc = parser.ParseDocument(details);
                    planet.Name = details.Substring(3, details.IndexOf('[') - 4);
                    var coordString = details.Substring(details.IndexOf('[') + 1, details.IndexOf(']') - details.IndexOf('[') - 1);
                    var coordTokens = coordString.Split(':');
                    planet.Coordinate = new(
                        int.Parse(coordTokens[0]),
                        int.Parse(coordTokens[1]),
                        int.Parse(coordTokens[2]),
                        CelestialTypes.Planet
                    );
                    if (IsV8(content)) {
                        planet.Diameter = int.Parse(details.Substring(details.IndexOf("<br/>") + 5, details.IndexOf("km") - (details.IndexOf("<br/>") + 5)).Replace(".", string.Empty));
                        var fieldsString = details.Substring(details.IndexOf('(') + 1, details.IndexOf(')') - details.IndexOf('(') - 1).Replace("<span class='overmark' >", string.Empty).Replace("</span>", string.Empty);
                        planet.Fields = new Fields
                        {
                            Built = int.Parse(fieldsString.Substring(0, fieldsString.IndexOf('/'))),
                            Total = int.Parse(fieldsString.Substring(fieldsString.IndexOf('/') + 1, fieldsString.Length - (fieldsString.IndexOf('/') + 1)))
                        };
                        var temperatureString = details.Substring(details.IndexOf("<br>") + 4, details.LastIndexOf("°C") - (details.IndexOf("<br>") + 4));
                        var minString = temperatureString.Substring(temperatureString.IndexOf(temperatureString.First(c => c == '-' || char.IsDigit(c))), temperatureString.IndexOf("°C")).Replace("°C", string.Empty).Trim();
                        var maxString = temperatureString.Substring(temperatureString.LastIndexOf(" "), temperatureString.Length - temperatureString.LastIndexOf(" ")).Replace("°C", string.Empty).Trim();
                        planet.Temperature = new Temperature
                        {
                            Min = int.Parse(minString),
                            Max = int.Parse(maxString)
                        };
                    }
                    else if (IsV9(content))
                    {
                        Lifeforms lifeform = Lifeforms.None;
                        string lifeformString = details.Substring(details.IndexOf("<br/>") + 5, (details.NthIndexOf("<br/>", 2) - details.IndexOf("<br/>") - 5));
                        Enum.TryParse<Lifeforms>(lifeformString.Substring(lifeformString.IndexOf(":") + 2), out lifeform);
                        planet.Lifeform = lifeform;
                        planet.Diameter = int.Parse(details.Substring(details.NthIndexOf("<br/>", 2) + 5, details.IndexOf("km") - (details.NthIndexOf("<br/>", 2) + 5)).Replace(".", string.Empty));
                        var fieldsString = details.Substring(details.IndexOf('(') + 1, details.IndexOf(')') - details.IndexOf('(') - 1).Replace("<span class='overmark' >", string.Empty).Replace("</span>", string.Empty);
                        planet.Fields = new Fields
                        {
                            Built = int.Parse(fieldsString.Substring(0, fieldsString.IndexOf('/'))),
                            Total = int.Parse(fieldsString.Substring(fieldsString.IndexOf('/') + 1, fieldsString.Length - (fieldsString.IndexOf('/') + 1)))
                        };
                        var temperatureString = details.Substring(details.IndexOf("<br>") + 4, details.LastIndexOf("°C") - (details.IndexOf("<br>") + 4));
                        var minString = temperatureString.Substring(temperatureString.IndexOf(temperatureString.First(c => c == '-' || char.IsDigit(c))), temperatureString.IndexOf("°C")).Replace("°C", string.Empty).Trim();
                        var maxString = temperatureString.Substring(temperatureString.LastIndexOf(" "), temperatureString.Length - temperatureString.LastIndexOf(" ")).Replace("°C", string.Empty).Trim();
                        planet.Temperature = new Temperature
                        {
                            Min = int.Parse(minString),
                            Max = int.Parse(maxString)
                        };
                    }

                    var moonElement = el.Children.FirstOrDefault(el => el.ClassList.Any(c => c == "moonlink"));
                    if (moonElement != null)
                    {
                        var moon = new Moon();                        
                        details = moonElement.GetAttribute("title");
                        detailsDoc = parser.ParseDocument(details);
                        moon.ID = int.Parse(details.Substring(details.IndexOf("cp=") + 3, 8));
                        moon.Name = details.Substring(3, details.IndexOf('[') - 4);
                        moon.Img = moonElement.Children.Single(el => el.ClassList.Any(c => c == "icon-moon")).GetAttribute("src");
                        coordString = details.Substring(details.IndexOf('[') + 1, details.IndexOf(']') - details.IndexOf('[') - 1);
                        coordTokens = coordString.Split(':');
                        moon.Coordinate = new(
                            int.Parse(coordTokens[0]),
                            int.Parse(coordTokens[1]),
                            int.Parse(coordTokens[2]),
                            CelestialTypes.Moon
                        );
                        moon.Diameter = int.Parse(details.Substring(details.IndexOf("<br>") + 4, details.IndexOf("km") - (details.IndexOf("<br>") + 4)).Replace(".", string.Empty));
                        var moonFieldsString = details.Substring(details.IndexOf('(') + 1, details.IndexOf(')') - details.IndexOf('(') - 1).Replace("<span class='overmark' >", string.Empty).Replace("</span>", string.Empty);
                        moon.Fields = new Fields
                        {
                            Built = int.Parse(moonFieldsString.Substring(0, moonFieldsString.IndexOf('/'))),
                            Total = int.Parse(moonFieldsString.Substring(moonFieldsString.IndexOf('/') + 1, moonFieldsString.Length - (moonFieldsString.IndexOf('/') + 1)))
                        };

                        planet.Moon = moon;
                        output.Add(moon);
                    }

                    output.Add(planet);
                }
            }
            return output
                .OrderBy(c => c.Coordinate.Galaxy)
                .ThenBy(c => c.Coordinate.System)
                .ThenBy(c => c.Coordinate.Position)
                .ThenByDescending(c => c.Coordinate.Type == CelestialTypes.Planet)
                .ToList();
        }

        internal static Techs GetTechs(string content)
        {
            Dictionary<Buildables, int> decodedJson = new();
            try
            {
                decodedJson = JsonConvert.DeserializeObject<Dictionary<Buildables, int>>(content);
            }
            catch (Exception e) { }
            return new Techs
            {
                Buildings = new Buildings
                {
                    MetalMine = decodedJson.GetValueOrDefault(Buildables.MetalMine, 0),
                    CrystalMine = decodedJson.GetValueOrDefault(Buildables.CrystalMine, 0),
                    DeuteriumSynthesizer = decodedJson.GetValueOrDefault(Buildables.DeuteriumSynthesizer, 0),
                    SolarPlant = decodedJson.GetValueOrDefault(Buildables.SolarPlant, 0),
                    FusionReactor = decodedJson.GetValueOrDefault(Buildables.FusionReactor, 0),
                    MetalStorage = decodedJson.GetValueOrDefault(Buildables.MetalStorage, 0),
                    CrystalStorage = decodedJson.GetValueOrDefault(Buildables.CrystalStorage, 0),
                    DeuteriumTank = decodedJson.GetValueOrDefault(Buildables.DeuteriumTank, 0),
                    SolarSatellite = decodedJson.GetValueOrDefault(Buildables.SolarSatellite, 0)
                },
                Facilities = new Facilities
                {
                    RoboticsFactory = decodedJson.GetValueOrDefault(Buildables.RoboticsFactory, 0),
                    ResearchLab = decodedJson.GetValueOrDefault(Buildables.ResearchLab, 0),
                    Shipyard = decodedJson.GetValueOrDefault(Buildables.Shipyard, 0),
                    MissileSilo = decodedJson.GetValueOrDefault(Buildables.MissileSilo, 0),
                    NaniteFactory = decodedJson.GetValueOrDefault(Buildables.NaniteFactory, 0),
                    SpaceDock = decodedJson.GetValueOrDefault(Buildables.SpaceDock, 0),
                    Terraformer = decodedJson.GetValueOrDefault(Buildables.Terraformer, 0),
                    AllianceDepot = decodedJson.GetValueOrDefault(Buildables.AllianceDepot, 0),
                    LunarBase = decodedJson.GetValueOrDefault(Buildables.LunarBase, 0),
                    SensorPhalanx = decodedJson.GetValueOrDefault(Buildables.SensorPhalanx, 0),
                    JumpGate = decodedJson.GetValueOrDefault(Buildables.JumpGate, 0)
                },
                Ships = new Ships
                {
                    LightFighter = decodedJson.GetValueOrDefault(Buildables.LightFighter, 0),
                    HeavyFighter = decodedJson.GetValueOrDefault(Buildables.HeavyFighter, 0),
                    Cruiser = decodedJson.GetValueOrDefault(Buildables.Cruiser, 0),
                    Pathfinder = decodedJson.GetValueOrDefault(Buildables.Pathfinder, 0),
                    Battleship = decodedJson.GetValueOrDefault(Buildables.Battleship, 0),
                    Battlecruiser = decodedJson.GetValueOrDefault(Buildables.Battlecruiser, 0),
                    Bomber = decodedJson.GetValueOrDefault(Buildables.Bomber, 0),
                    Destroyer = decodedJson.GetValueOrDefault(Buildables.Destroyer, 0),
                    Reaper = decodedJson.GetValueOrDefault(Buildables.Reaper, 0),
                    Deathstar = decodedJson.GetValueOrDefault(Buildables.Deathstar, 0),
                    SmallCargo = decodedJson.GetValueOrDefault(Buildables.SmallCargo, 0),
                    LargeCargo = decodedJson.GetValueOrDefault(Buildables.LargeCargo, 0),
                    Recycler = decodedJson.GetValueOrDefault(Buildables.Recycler, 0),
                    ColonyShip = decodedJson.GetValueOrDefault(Buildables.ColonyShip, 0),
                    EspionageProbe = decodedJson.GetValueOrDefault(Buildables.EspionageProbe, 0),
                    Crawler = decodedJson.GetValueOrDefault(Buildables.Crawler, 0),
                    SolarSatellite = decodedJson.GetValueOrDefault(Buildables.SolarSatellite, 0)
                },
                Defences = new Defences
                {
                    RocketLauncher = decodedJson.GetValueOrDefault(Buildables.RocketLauncher, 0),
                    LightLaser = decodedJson.GetValueOrDefault(Buildables.LightLaser, 0),
                    HeavyLaser = decodedJson.GetValueOrDefault(Buildables.HeavyLaser, 0),
                    IonCannon = decodedJson.GetValueOrDefault(Buildables.IonCannon, 0),
                    GaussCannon = decodedJson.GetValueOrDefault(Buildables.GaussCannon, 0),
                    PlasmaTurret = decodedJson.GetValueOrDefault(Buildables.PlasmaTurret, 0),
                    SmallShieldDome = decodedJson.GetValueOrDefault(Buildables.SmallShieldDome, 0),
                    LargeShieldDome = decodedJson.GetValueOrDefault(Buildables.LargeShieldDome, 0),
                    AntiBallisticMissiles = decodedJson.GetValueOrDefault(Buildables.AntiBallisticMissiles, 0),
                    InterplanetaryMissiles = decodedJson.GetValueOrDefault(Buildables.InterplanetaryMissiles, 0)
                },
                Researches = new Researches
                {
                    EnergyTechnology = decodedJson.GetValueOrDefault(Buildables.EnergyTechnology, 0),
                    LaserTechnology = decodedJson.GetValueOrDefault(Buildables.LaserTechnology, 0),
                    IonTechnology = decodedJson.GetValueOrDefault(Buildables.IonTechnology, 0),
                    HyperspaceTechnology = decodedJson.GetValueOrDefault(Buildables.HyperspaceTechnology, 0),
                    PlasmaTechnology = decodedJson.GetValueOrDefault(Buildables.PlasmaTechnology, 0),
                    EspionageTechnology = decodedJson.GetValueOrDefault(Buildables.EspionageTechnology, 0),
                    ComputerTechnology = decodedJson.GetValueOrDefault(Buildables.ComputerTechnology, 0),
                    Astrophysics = decodedJson.GetValueOrDefault(Buildables.Astrophysics, 0),
                    IntergalacticResearchNetwork = decodedJson.GetValueOrDefault(Buildables.IntergalacticResearchNetwork, 0),
                    GravitonTechnology = decodedJson.GetValueOrDefault(Buildables.GravitonTechnology, 0),
                    CombustionDrive = decodedJson.GetValueOrDefault(Buildables.CombustionDrive, 0),
                    ImpulseDrive = decodedJson.GetValueOrDefault(Buildables.ImpulseDrive, 0),
                    HyperspaceDrive = decodedJson.GetValueOrDefault(Buildables.HyperspaceDrive, 0),
                    WeaponsTechnology = decodedJson.GetValueOrDefault(Buildables.WeaponsTechnology, 0),
                    ShieldingTechnology = decodedJson.GetValueOrDefault(Buildables.ShieldingTechnology, 0),
                    ArmourTechnology = decodedJson.GetValueOrDefault(Buildables.ArmourTechnology, 0)
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
                Darkmatter = resProd.Darkmatter.Available
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

            return new ResourcesProduction
            (
                new Resource
                {
                    Available = decodedJson.resources.metal.amount,
                    StorageCapacity = decodedJson.resources.metal.storage,
                    CurrentProduction = long.Parse(CleanString(metalTooltip.QuerySelector("table tr:nth-child(3) td").TextContent)),
                    DenCapacity = long.Parse(CleanString(metalTooltip.QuerySelector("table tr:nth-child(4) td").TextContent))
                },
                new Resource
                {
                    Available = decodedJson.resources.crystal.amount,
                    StorageCapacity = decodedJson.resources.crystal.storage,
                    CurrentProduction = long.Parse(CleanString(crystalTooltip.QuerySelector("table tr:nth-child(3) td").TextContent)),
                    DenCapacity = long.Parse(CleanString(crystalTooltip.QuerySelector("table tr:nth-child(4) td").TextContent))
                },
                new Resource
                {
                    Available = decodedJson.resources.deuterium.amount,
                    StorageCapacity = decodedJson.resources.deuterium.storage,
                    CurrentProduction = long.Parse(CleanString(deuteriumTooltip.QuerySelector("table tr:nth-child(3) td").TextContent)),
                    DenCapacity = long.Parse(CleanString(deuteriumTooltip.QuerySelector("table tr:nth-child(4) td").TextContent))
                },
                new Energy
                {
                    Available = decodedJson.resources.energy.amount,
                    Consumption = long.Parse(CleanString(energyTooltip.QuerySelector("table tr:nth-child(2) td").TextContent)),
                    CurrentProduction = long.Parse(CleanString(energyTooltip.QuerySelector("table tr:nth-child(3) td").TextContent))
                },
                new Darkmatter
                {
                    Available = decodedJson.resources.darkmatter.amount,
                    Purchased = long.Parse(CleanString(darkmatterTooltip.QuerySelector("table tr:nth-child(2) td").TextContent)),
                    Found = long.Parse(CleanString(darkmatterTooltip.QuerySelector("table tr:nth-child(3) td").TextContent))
                }
            );
        }
        internal static ResourceSettings GetResourcesSettings(string content)
        {
            var parser = new HtmlParser();
            var doc = parser.ParseDocument(content);
            var tokens = doc.QuerySelectorAll("option").Where(e => e.Attributes.Any(a => a.Name == "selected")).ToArray();
            return new ResourceSettings
            {
                MetalMine = (ResourceSettingsPercents)int.Parse(tokens[0].GetAttribute("value")),
                CrystalMine = (ResourceSettingsPercents)int.Parse(tokens[1].GetAttribute("value")),
                DeuteriumSynthesizer = (ResourceSettingsPercents)int.Parse(tokens[2].GetAttribute("value")),
                SolarPlant = (ResourceSettingsPercents)int.Parse(tokens[3].GetAttribute("value")),
                FusionReactor = (ResourceSettingsPercents)int.Parse(tokens[4].GetAttribute("value")),
                SolarSatellite = (ResourceSettingsPercents)int.Parse(tokens[5].GetAttribute("value")),
                Crawler = (ResourceSettingsPercents)int.Parse(tokens[6].GetAttribute("value")),
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
            var slots = new Slots
            {
                InUse = int.Parse(fleetTokens[0]),
                Total = int.Parse(fleetTokens[1]),
                ExpInUse = int.Parse(expTokens[0]),
                ExpTotal = int.Parse(expTokens[1])
            };
            return slots;
        }
    }
}
