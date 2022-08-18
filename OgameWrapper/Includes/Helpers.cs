using System.Reflection;
using OgameWrapper.Model;

namespace OgameWrapper.Includes
{
    public static class Helpers
    {
        public static bool ShouldSleep(DateTime time, DateTime goToSleep, DateTime wakeUp)
        {
            if (time >= goToSleep)
            {
                if (time >= wakeUp)
                {
                    if (goToSleep >= wakeUp)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (time >= wakeUp)
                {
                    return false;
                }
                else
                {
                    if (goToSleep >= wakeUp)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static ulong CalcShipCapacity(Buildable buildable, uint hyperspaceTech, PlayerClass playerClass, uint probeCargo = 0)
        {
            ulong baseCargo;
            var bonus = (hyperspaceTech * 5ul);

            switch (buildable)
            {
                case Buildable.SmallCargo:
                    baseCargo = 5000;
                    if (playerClass == PlayerClass.Collector)
                    {
                        bonus += 25;
                    }
                    break;
                case Buildable.LargeCargo:
                    baseCargo = 25000;
                    if (playerClass == PlayerClass.Collector)
                    {
                        bonus += 25;
                    }
                    break;
                case Buildable.LightFighter:
                    baseCargo = 50;
                    break;
                case Buildable.HeavyFighter:
                    baseCargo = 100;
                    break;
                case Buildable.Cruiser:
                    baseCargo = 800;
                    break;
                case Buildable.Battleship:
                    baseCargo = 1500;
                    break;
                case Buildable.ColonyShip:
                    baseCargo = 7500;
                    break;
                case Buildable.Recycler:
                    baseCargo = 20000;
                    if (playerClass == PlayerClass.General)
                    {
                        bonus += 20;
                    }
                    break;
                case Buildable.EspionageProbe:
                    baseCargo = probeCargo;
                    break;
                case Buildable.Bomber:
                    baseCargo = 500;
                    break;
                case Buildable.Destroyer:
                    baseCargo = 2000;
                    break;
                case Buildable.Deathstar:
                    baseCargo = 1000000;
                    break;
                case Buildable.Battlecruiser:
                    baseCargo = 750;
                    break;
                case Buildable.Reaper:
                    baseCargo = 10000;
                    break;
                case Buildable.Pathfinder:
                    baseCargo = 10000;
                    if (playerClass == PlayerClass.General)
                    {
                        bonus += 20;
                    }
                    break;
                default:
                    return 0;
            }

            return baseCargo * (bonus + 100) / 100;
        }

        public static ulong CalcShipFuelCapacity(Buildable buildable, uint probeCargo = 0)
        {
            ulong baseCargo;
            switch (buildable)
            {
                case Buildable.SmallCargo:
                    baseCargo = 5000;
                    break;
                case Buildable.LargeCargo:
                    baseCargo = 25000;
                    break;
                case Buildable.LightFighter:
                    baseCargo = 50;
                    break;
                case Buildable.HeavyFighter:
                    baseCargo = 100;
                    break;
                case Buildable.Cruiser:
                    baseCargo = 800;
                    break;
                case Buildable.Battleship:
                    baseCargo = 1500;
                    break;
                case Buildable.ColonyShip:
                    baseCargo = 7500;
                    break;
                case Buildable.Recycler:
                    baseCargo = 20000;
                    break;
                case Buildable.EspionageProbe:
                    baseCargo = probeCargo;
                    break;
                case Buildable.Bomber:
                    baseCargo = 750;
                    break;
                case Buildable.Destroyer:
                    baseCargo = 2000;
                    break;
                case Buildable.Deathstar:
                    baseCargo = 1000000;
                    break;
                case Buildable.Battlecruiser:
                    baseCargo = 750;
                    break;
                case Buildable.Reaper:
                    baseCargo = 7000;
                    break;
                case Buildable.Pathfinder:
                    baseCargo = 10000;
                    break;
                default:
                    return 0;
            }
            return baseCargo;
        }

        public static ulong CalcFleetCapacity(Ships fleet, uint hyperspaceTech, PlayerClass playerClass, uint probeCargo = 0)
        {
            var total = 0ul;
            foreach (PropertyInfo prop in fleet.GetType().GetProperties())
            {
                var qty = (ulong)prop.GetValue(fleet, null);
                if (qty == 0)
                {
                    continue;
                }

                if (Enum.TryParse(prop.Name, out Buildable buildable))
                {
                    var oneCargo = CalcShipCapacity(buildable, hyperspaceTech, playerClass, probeCargo);
                    total += oneCargo * qty;
                }
            }
            return total;
        }

        public static ulong CalcFleetFuelCapacity(Ships fleet, uint probeCargo = 0)
        {
            var total = 0ul;
            foreach (PropertyInfo prop in fleet.GetType().GetProperties())
            {
                var qty = (ulong)prop.GetValue(fleet, null);
                if (qty == 0)
                {
                    continue;
                }

                if (Enum.TryParse(prop.Name, out Buildable buildable))
                {
                    var oneCargo = CalcShipFuelCapacity(buildable, probeCargo);
                    total += oneCargo * qty;
                }
            }
            return total;
        }

        public static ulong CalcShipSpeed(Buildable buildable, Researches researches, PlayerClass playerClass)
        {
            return CalcShipSpeed(buildable, researches.CombustionDrive, researches.ImpulseDrive, researches.HyperspaceDrive, playerClass);
        }

        public static ulong CalcShipSpeed(Buildable buildable, uint combustionDrive, uint impulseDrive, uint hyperspaceDrive, PlayerClass playerClass)
        {
            ulong baseSpeed;
            ulong bonus = combustionDrive;
            switch (buildable)
            {
                case Buildable.SmallCargo:
                    baseSpeed = 5000;
                    if (impulseDrive >= 5)
                    {
                        baseSpeed = 10000;
                        bonus = impulseDrive * 2;
                    }
                    if (playerClass == PlayerClass.Collector)
                        bonus += 10;
                    break;
                case Buildable.LargeCargo:
                    baseSpeed = 7500;
                    if (playerClass == PlayerClass.Collector)
                        bonus += 10;
                    break;
                case Buildable.LightFighter:
                    baseSpeed = 12500;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.HeavyFighter:
                    baseSpeed = 10000;
                    bonus = impulseDrive * 2;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.Cruiser:
                    baseSpeed = 15000;
                    bonus = impulseDrive * 2;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.Battleship:
                    baseSpeed = 10000;
                    bonus = hyperspaceDrive * 3;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.ColonyShip:
                    bonus = impulseDrive * 2;
                    baseSpeed = 2500;
                    break;
                case Buildable.Recycler:
                    baseSpeed = 2000;
                    if (impulseDrive >= 17)
                    {
                        baseSpeed = 4000;
                        bonus = impulseDrive * 2;
                    }
                    if (hyperspaceDrive >= 15)
                    {
                        baseSpeed = 6000;
                        bonus = hyperspaceDrive * 3;
                    }
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.EspionageProbe:
                    baseSpeed = 100000000;
                    break;
                case Buildable.Bomber:
                    baseSpeed = 4000;
                    bonus = impulseDrive * 2;
                    if (hyperspaceDrive >= 8)
                    {
                        baseSpeed = 5000;
                        bonus = hyperspaceDrive * 3;
                    }
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.Destroyer:
                    baseSpeed = 5000;
                    bonus = hyperspaceDrive * 3;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.Deathstar:
                    baseSpeed = 100;
                    bonus = hyperspaceDrive * 3;
                    break;
                case Buildable.Battlecruiser:
                    baseSpeed = 10000;
                    bonus = hyperspaceDrive * 3;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.Reaper:
                    baseSpeed = 10000;
                    bonus = hyperspaceDrive * 3;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                case Buildable.Pathfinder:
                    baseSpeed = 10000;
                    bonus = hyperspaceDrive * 3;
                    if (playerClass == PlayerClass.General)
                        bonus += 10;
                    break;
                default:
                    return 0;
            }

            return (ulong)Math.Round((baseSpeed * (bonus + 10) / 10d), MidpointRounding.ToZero);
        }

        public static ulong CalcSlowestSpeed(Ships fleet, Researches researches, PlayerClass playerClass)
        {
            return CalcSlowestSpeed(fleet, researches.CombustionDrive, researches.ImpulseDrive, researches.HyperspaceDrive, playerClass);
        }

        public static ulong CalcSlowestSpeed(Ships fleet, uint combustionDrive, uint impulseDrive, uint hyperspaceDrive, PlayerClass playerClass)
        {
            var lowest = ulong.MaxValue;
            foreach (PropertyInfo prop in fleet.GetType().GetProperties())
            {
                var qty = (ulong)prop.GetValue(fleet, null);
                if (qty == 0)
                {
                    continue;
                }

                if (Enum.TryParse(prop.Name, out Buildable buildable))
                {
                    if (buildable == Buildable.SolarSatellite || buildable == Buildable.Crawler)
                    {
                        continue;
                    }

                    var speed = CalcShipSpeed(buildable, combustionDrive, impulseDrive, hyperspaceDrive, playerClass);
                    if (speed < lowest)
                    {
                        lowest = speed;
                    }
                }
            }
            return lowest;
        }

        public static ulong CalcFleetSpeed(Ships fleet, Researches researches, PlayerClass playerClass)
        {
            return CalcFleetSpeed(fleet, researches.CombustionDrive, researches.ImpulseDrive, researches.HyperspaceDrive, playerClass);
        }

        public static ulong CalcFleetSpeed(Ships fleet, uint combustionDrive, uint impulseDrive, uint hyperspaceDrive, PlayerClass playerClass)
        {
            ulong minSpeed = 0;
            foreach (PropertyInfo prop in fleet.GetType().GetProperties())
            {
                var qty = (ulong)prop.GetValue(fleet, null);
                if (qty == 0)
                {
                    continue;
                }

                if (Enum.TryParse<Buildable>(prop.Name, out Buildable buildable))
                {
                    var thisSpeed = CalcShipSpeed(buildable, combustionDrive, impulseDrive, hyperspaceDrive, playerClass);
                    if (thisSpeed < minSpeed)
                        minSpeed = thisSpeed;
                }
            }
            return minSpeed;
        }

        public static ulong CalcShipConsumption(Buildable buildable, Researches researches, ServerData serverData, PlayerClass playerClass)
        {
            return CalcShipConsumption(buildable, researches.ImpulseDrive, researches.HyperspaceDrive, serverData.GlobalDeuteriumSaveFactor, playerClass);
        }

        public static ulong CalcShipConsumption(Buildable buildable, uint impulseDrive, uint hyperspaceDrive, double deuteriumSaveFactor, PlayerClass playerClass)
        {
            ulong baseConsumption;
            switch (buildable)
            {
                case Buildable.SmallCargo:
                    baseConsumption = 20;
                    if (impulseDrive >= 5)
                        baseConsumption *= 2;
                    break;
                case Buildable.LargeCargo:
                    baseConsumption = 50;
                    break;
                case Buildable.LightFighter:
                    baseConsumption = 20;
                    break;
                case Buildable.HeavyFighter:
                    baseConsumption = 75;
                    break;
                case Buildable.Cruiser:
                    baseConsumption = 300;
                    break;
                case Buildable.Battleship:
                    baseConsumption = 500;
                    break;
                case Buildable.ColonyShip:
                    baseConsumption = 1000;
                    break;
                case Buildable.Recycler:
                    baseConsumption = 2000;
                    if (hyperspaceDrive >= 15)
                        baseConsumption *= 3;
                    else if (impulseDrive >= 17)
                        baseConsumption *= 2;
                    break;
                case Buildable.EspionageProbe:
                    baseConsumption = 1;
                    break;
                case Buildable.Bomber:
                    baseConsumption = 700;
                    if (hyperspaceDrive >= 8)
                        baseConsumption *= 3 / 2;
                    break;
                case Buildable.Destroyer:
                    baseConsumption = 1000;
                    break;
                case Buildable.Deathstar:
                    baseConsumption = 1;
                    break;
                case Buildable.Battlecruiser:
                    baseConsumption = 250;
                    break;
                case Buildable.Reaper:
                    baseConsumption = 1100;
                    break;
                case Buildable.Pathfinder:
                    baseConsumption = 300;
                    break;
                default:
                    return 0;
            }

            var fuelConsumption = deuteriumSaveFactor * baseConsumption;
            if (playerClass == PlayerClass.General)
            {
                fuelConsumption /= 2d;
            }
            fuelConsumption = Math.Round(fuelConsumption);
            if (fuelConsumption < 1)
            {
                return 1;
            }

            return (ulong)fuelConsumption;
        }

        public static ulong CalcFlightTime(Coordinate origin, Coordinate destination, Ships ships, Mission mission, double speed, Researches researches, ServerData serverData, PlayerClass playerClass)
        {
            var fleetSpeed = mission switch
            {
                Mission.Attack or Mission.FederalAttack or Mission.Destroy or Mission.Spy or Mission.Harvest => serverData.SpeedFleetWar,
                Mission.FederalDefense => serverData.SpeedFleetHolding,
                _ => serverData.SpeedFleetPeaceful,
            };

            return CalcFlightTime(origin, destination, ships, speed, researches.CombustionDrive, researches.ImpulseDrive, researches.HyperspaceDrive, serverData.Galaxies, serverData.Systems, serverData.DonutGalaxy, serverData.DonutSystem, fleetSpeed, playerClass);
        }

        public static ulong CalcFlightTime(Coordinate origin, Coordinate destination, Ships ships, double speed, uint combustionDrive, uint impulseDrive, uint hyperspaceDrive, uint numberOfGalaxies, uint numberOfSystems, bool donutGalaxies, bool donutSystems, uint fleetSpeed, PlayerClass playerClass)
        {
            var slowestShipSpeed = CalcSlowestSpeed(ships, combustionDrive, impulseDrive, hyperspaceDrive, playerClass);
            var distance = CalcDistance(origin, destination, numberOfGalaxies, numberOfSystems, donutGalaxies, donutSystems);
            return (ulong)Math.Round(((35000d / speed) * Math.Sqrt(distance * 10d / slowestShipSpeed) + 10d) / fleetSpeed);
        }

        public static ulong CalcFuelConsumption(Coordinate origin, Coordinate destination, Ships ships, Mission mission, ulong flightTime, Researches researches, ServerData serverData, PlayerClass playerClass)
        {
            var fleetSpeed = mission switch
            {
                Mission.Attack or Mission.FederalAttack or Mission.Destroy or Mission.Harvest or Mission.Spy => serverData.SpeedFleetWar,
                Mission.FederalDefense => serverData.SpeedFleetHolding,
                _ => serverData.SpeedFleetPeaceful,
            };
            return CalcFuelConsumption(origin, destination, ships, flightTime, researches.CombustionDrive, researches.ImpulseDrive, researches.HyperspaceDrive, serverData.Galaxies, serverData.Systems, serverData.DonutGalaxy, serverData.DonutSystem, fleetSpeed, serverData.GlobalDeuteriumSaveFactor, playerClass);
        }

        public static ulong CalcFuelConsumption(Coordinate origin, Coordinate destination, Ships ships, ulong flightTime, uint combustionDrive, uint impulseDrive, uint hyperspaceDrive, uint numberOfGalaxies, uint numberOfSystems, bool donutGalaxies, bool donutSystems, uint fleetSpeed, double deuteriumSaveFactor, PlayerClass playerClass)
        {
            var distance = CalcDistance(origin, destination, numberOfGalaxies, numberOfSystems, donutGalaxies, donutSystems);
            var tempFuel = 0d;
            foreach (PropertyInfo prop in ships.GetType().GetProperties())
            {
                var qty = (ulong)prop.GetValue(ships, null);
                if (qty == 0)
                {
                    continue;
                }

                if (Enum.TryParse(prop.Name, out Buildable buildable))
                {
                    var tempSpeed = 35000d / ((flightTime * fleetSpeed) - 10d) * (double)Math.Sqrt(distance * 10d / CalcShipSpeed(buildable, combustionDrive, impulseDrive, hyperspaceDrive, playerClass));
                    var shipConsumption = CalcShipConsumption(buildable, impulseDrive, hyperspaceDrive, deuteriumSaveFactor, playerClass);
                    var thisFuel = (shipConsumption * qty * distance) / 35000d * Math.Pow(((double)tempSpeed / (double)10) + (double)1, 2);
                    tempFuel += thisFuel;
                }
            }
            return (ulong)(1 + Math.Round(tempFuel));
        }

        public static FleetPrediction CalcFleetPrediction(Coordinate origin, Coordinate destination, Ships ships, Mission mission, double speed, Researches researches, ServerData serverData, PlayerClass playerClass)
        {
            var time = CalcFlightTime(origin, destination, ships, mission, speed, researches, serverData, playerClass);
            var fuel = CalcFuelConsumption(origin, destination, ships, mission, time, researches, serverData, playerClass);
            return new()
            {
                Fuel = fuel,
                Time = time,
            };
        }

        public static FleetPrediction CalcFleetPrediction(Celestial origin, Coordinate destination, Ships ships, Mission mission, double speed, Researches researches, ServerData serverData, PlayerClass playerClass)
        {
            return CalcFleetPrediction(origin.Coordinate, destination, ships, mission, speed, researches, serverData, playerClass);
        }

        public static ulong CalcDistance(Coordinate origin, Coordinate destination, uint galaxiesNumber, uint systemsNumber = 499, bool donutGalaxy = true, bool donutSystem = true)
        {
            if (origin.Galaxy != destination.Galaxy)
            {
                return CalcGalaxyDistance(origin, destination, galaxiesNumber, donutGalaxy);
            }

            if (origin.System != destination.System)
            {
                return CalcSystemDistance(origin, destination, systemsNumber, donutSystem);
            }

            if (origin.Position != destination.Position)
            {
                return CalcPlanetDistance(origin, destination);
            }

            return 5;
        }

        public static ulong CalcDistance(Coordinate origin, Coordinate destination, ServerData serverData)
        {
            return CalcDistance(origin, destination, serverData.Galaxies, serverData.Systems, serverData.DonutGalaxy, serverData.DonutSystem);
        }

        private static ulong CalcGalaxyDistance(Coordinate origin, Coordinate destination, uint galaxiesNumber, bool donutGalaxy = true)
        {
            if (!donutGalaxy)
                return 20000 * (ulong)Math.Abs(origin.Galaxy - destination.Galaxy);

            if (origin.Galaxy > destination.Galaxy)
                return 20000 * Math.Min((origin.Galaxy - destination.Galaxy), ((destination.Galaxy + galaxiesNumber) - origin.Galaxy));

            return 20000 * Math.Min((destination.Galaxy - origin.Galaxy), ((origin.Galaxy + galaxiesNumber) - destination.Galaxy));
        }

        private static ulong CalcSystemDistance(Coordinate origin, Coordinate destination, uint systemsNumber, bool donutSystem = true)
        {
            if (!donutSystem)
                return 2700 + 95 * (ulong)Math.Abs(origin.System - destination.System);

            if (origin.System > destination.System)
                return 2700 + 95 * Math.Min((origin.System - destination.System), ((destination.System + systemsNumber) - origin.System));

            return 2700 + 95 * Math.Min((destination.System - origin.System), ((origin.System + systemsNumber) - destination.System));

        }

        private static ulong CalcPlanetDistance(Coordinate origin, Coordinate destination)
        {
            return 1000 + 5 * (ulong)Math.Abs(destination.Position - origin.Position);
        }

        public static ulong CalcEnergyProduction(Buildable buildable, uint level, uint energyTechnology = 0, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasEngineer = false, bool hasStaff = false)
        {
            ulong prod = 0;
            if (buildable == Buildable.SolarPlant)
            {
                prod = (ulong)Math.Round(20 * level * Math.Pow(1.1, level) * ratio);
            }
            else if (buildable == Buildable.FusionReactor)
            {
                prod = (ulong)Math.Round(30 * level * Math.Pow(1.05 + (0.01 * energyTechnology), level) * ratio);
            }

            if (hasEngineer)
            {
                prod += (ulong)Math.Round(prod * 0.1);
            }
            if (hasStaff)
            {
                prod += (ulong)Math.Round(prod * 0.02);
            }
            if (playerClass == PlayerClass.Collector)
            {
                prod += (ulong)Math.Round(prod * 0.1);
            }

            return prod;
        }

        public static ulong CalcEnergyProduction(Buildable buildable, uint level, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasEngineer = false, bool hasStaff = false)
        {
            return CalcEnergyProduction(buildable, level, researches.EnergyTechnology, ratio, playerClass, hasEngineer, hasStaff);
        }

        public static ulong CalcMetalProduction(uint level, uint position, uint speedFactor, double ratio = 1, uint plasma = 0, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            uint baseProd = position switch
            {
                6 => (uint)Math.Round(30 + (30 * 0.17)),
                7 => (uint)Math.Round(30 + (30 * 0.23)),
                8 => (uint)Math.Round(30 + (30 * 0.35)),
                9 => (uint)Math.Round(30 + (30 * 0.23)),
                10 => (uint)Math.Round(30 + (30 * 0.17)),
                _ => 30,
            };
            baseProd *= speedFactor;
            if (level == 0)
                return baseProd;
            int prod = (int)Math.Round((float)(baseProd * level * Math.Pow(1.1, level)));
            int plasmaProd = (int)Math.Round(prod * 0.01 * plasma);
            int geologistProd = 0;
            if (hasGeologist)
            {
                geologistProd = (int)Math.Round(prod * 0.1);
            }
            int staffProd = 0;
            if (hasStaff)
            {
                staffProd = (int)Math.Round(prod * 0.02);
            }
            int classProd = 0;
            if (playerClass == PlayerClass.Collector)
            {
                classProd = (int)Math.Round(prod * 0.25);
            }
            return (ulong)Math.Round((prod + plasmaProd + geologistProd + staffProd + classProd) * ratio, 0);
        }

        public static ulong CalcMetalProduction(Buildings buildings, uint position, uint speedFactor, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            return CalcMetalProduction(buildings.MetalMine, position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
        }

        public static ulong CalcMetalProduction(Planet planet, uint speedFactor, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            return CalcMetalProduction(planet.Buildings.MetalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
        }

        public static ulong CalcCrystalProduction(uint level, uint position, uint speedFactor, double ratio = 1, uint plasma = 0, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            ulong baseProd = position switch
            {
                1 => (ulong)Math.Round(20 + (20 * 0.3)),
                2 => (ulong)Math.Round(20 + (20 * 0.2)),
                3 => (ulong)Math.Round(20 + (20 * 0.1)),
                _ => 20,
            };
            baseProd *= speedFactor;
            if (level == 0)
                return baseProd;
            int prod = (int)Math.Round((float)(baseProd * level * Math.Pow(1.1, level)));
            int plasmaProd = (int)Math.Round(prod * 0.0066 * plasma);
            int geologistProd = 0;
            if (hasGeologist)
            {
                geologistProd = (int)Math.Round(prod * 0.1);
            }
            int staffProd = 0;
            if (hasStaff)
            {
                staffProd = (int)Math.Round(prod * 0.02);
            }
            int classProd = 0;
            if (playerClass == PlayerClass.Collector)
            {
                classProd = (int)Math.Round(prod * 0.25);
            }
            return (ulong)Math.Round((prod + plasmaProd + geologistProd + staffProd + classProd) * ratio, 0);
        }

        public static ulong CalcCrystalProduction(Buildings buildings, uint position, uint speedFactor, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            return CalcCrystalProduction(buildings.CrystalMine, position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
        }

        public static ulong CalcCrystalProduction(Planet planet, uint speedFactor, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            return CalcCrystalProduction(planet.Buildings.CrystalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
        }

        public static ulong CalcDeuteriumProduction(uint level, double temp, uint speedFactor, double ratio = 1, uint plasma = 0, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            if (level == 0)
                return 0;
            var baseProd = 10 * speedFactor;
            int prod = (int)Math.Round((float)(baseProd * level * Math.Pow(1.1, level) * ((-0.004 * temp) + 1.36)));
            int plasmaProd = (int)Math.Round(prod * 0.0033 * plasma);
            int geologistProd = 0;
            if (hasGeologist)
            {
                geologistProd = (int)Math.Round(prod * 0.1);
            }
            int staffProd = 0;
            if (hasStaff)
            {
                staffProd = (int)Math.Round(prod * 0.02);
            }
            int classProd = 0;
            if (playerClass == PlayerClass.Collector)
            {
                classProd = (int)Math.Round(prod * 0.25);
            }
            return (ulong)Math.Round((prod + plasmaProd + geologistProd + staffProd + classProd) * ratio, 0);
        }

        public static ulong CalcDeuteriumProduction(Buildings buildings, Temperature temp, uint speedFactor, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            return CalcDeuteriumProduction(buildings.CrystalMine, temp.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
        }

        public static ulong CalcDeuteriumProduction(Planet planet, uint speedFactor, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            return CalcDeuteriumProduction(planet.Buildings.CrystalMine, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
        }

        public static Resources CalcPlanetHourlyProduction(Planet planet, uint speedFactor, Researches researches, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            return new()
            {
                Metal = CalcMetalProduction(planet, speedFactor, researches, ratio, playerClass, hasGeologist, hasStaff),
                Crystal = CalcCrystalProduction(planet, speedFactor, researches, ratio, playerClass, hasGeologist, hasStaff),
                Deuterium = CalcDeuteriumProduction(planet, speedFactor, researches, ratio, playerClass, hasGeologist, hasStaff)
            };
        }

        public static Resources CalcPrice(Buildable buildable, uint level)
        {
            var metal = 0ul;
            var crystal = 0ul;
            var deuterium = 0ul;
            var energy = 0ul;

            switch (buildable)
            {
                case Buildable.MetalMine:
                    metal = (ulong)Math.Round(60 * Math.Pow(1.5, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    crystal = (ulong)Math.Round(15 * Math.Pow(1.5, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    // MidpointRounding set to "ToNegativeInfinity" because in all cases that i try (metal 51 crystal 44) the result is always the lower integer
                    // Formula: 10 * Mine Level * (1.1 ^ Mine Level)
                    energy = (ulong)Math.Round((10 * level * (Math.Pow(1.1, level))), 0, MidpointRounding.ToPositiveInfinity);
                    break;
                case Buildable.CrystalMine:
                    metal = (ulong)Math.Round(48 * Math.Pow(1.6, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    crystal = (ulong)Math.Round(24 * Math.Pow(1.6, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    // MidpointRounding set to "ToNegativeInfinity" because in all cases that i try (metal 51 crystal 44) the result is always the lower integer
                    // Formula: 10 * Mine Level * (1.1 ^ Mine Level)
                    energy = (ulong)Math.Round((10 * level * (Math.Pow(1.1, level))), 0, MidpointRounding.ToPositiveInfinity);
                    break;
                case Buildable.DeuteriumSynthesizer:
                    metal = (ulong)Math.Round(225 * Math.Pow(1.5, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    crystal = (ulong)Math.Round(75 * Math.Pow(1.5, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    // MidpointRounding set to "ToNegativeInfinity" because in all cases that i try (metal 51 crystal 44) the result is always the lower integer
                    // Formula: 20 * Mine Level * (1.1 ^ Mine Level)
                    energy = (ulong)Math.Round((20 * level * (Math.Pow(1.1, level))), 0, MidpointRounding.ToPositiveInfinity);
                    break;
                case Buildable.SolarPlant:
                    metal = (ulong)Math.Round(75 * Math.Pow(1.5, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    crystal = (ulong)Math.Round(30 * Math.Pow(1.5, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    break;
                case Buildable.FusionReactor:
                    metal = (ulong)Math.Round(900 * Math.Pow(1.8, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    crystal = (ulong)Math.Round(360 * Math.Pow(1.8, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    deuterium = (ulong)Math.Round(180 * Math.Pow(1.8, (level - 1)), 0, MidpointRounding.ToPositiveInfinity);
                    break;
                case Buildable.MetalStorage:
                    metal = (ulong)(500 * Math.Pow(2, level));
                    break;
                case Buildable.CrystalStorage:
                    metal = (ulong)(500 * Math.Pow(2, level));
                    crystal = (ulong)(250 * Math.Pow(2, level));
                    break;
                case Buildable.DeuteriumTank:
                    metal = (ulong)(500 * Math.Pow(2, level));
                    crystal = (ulong)(500 * Math.Pow(2, level));
                    break;
                case Buildable.ShieldedMetalDen:
                    break;
                case Buildable.UndergroundCrystalDen:
                    break;
                case Buildable.SeabedDeuteriumDen:
                    break;
                case Buildable.AllianceDepot:
                    metal = (ulong)(20000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(40000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.RoboticsFactory:
                    metal = (ulong)(400 * Math.Pow(2, level - 1));
                    crystal = (ulong)(120 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(200 * Math.Pow(2, level - 1));
                    break;
                case Buildable.Shipyard:
                    metal = (ulong)(400 * Math.Pow(2, level - 1));
                    crystal = (ulong)(200 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(100 * Math.Pow(2, level - 1));
                    break;
                case Buildable.ResearchLab:
                    metal = (ulong)(200 * Math.Pow(2, level - 1));
                    crystal = (ulong)(400 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(200 * Math.Pow(2, level - 1));
                    break;
                case Buildable.MissileSilo:
                    metal = (ulong)(20000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(20000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(1000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.NaniteFactory:
                    metal = (ulong)(1000000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(500000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(100000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.Terraformer:
                    crystal = (ulong)(50000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(100000 * Math.Pow(2, level - 1));
                    energy = (ulong)(1000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.SpaceDock:
                    metal = (ulong)(200 * Math.Pow(5, level - 1));
                    deuterium = (ulong)(50 * Math.Pow(5, level - 1));
                    energy = (ulong)Math.Round(50 * Math.Pow(2.5, level - 1), 0, MidpointRounding.ToPositiveInfinity);
                    break;
                case Buildable.LunarBase:
                    metal = (ulong)(20000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(40000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(20000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.SensorPhalanx:
                    metal = (ulong)(20000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(40000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(20000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.JumpGate:
                    metal = (ulong)(2000000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(4000000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(2000000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.RocketLauncher:
                    metal = (ulong)(2000 * level);
                    break;
                case Buildable.LightLaser:
                    metal = (ulong)(1500 * level);
                    crystal = (ulong)(500 * level);
                    break;
                case Buildable.HeavyLaser:
                    metal = (ulong)(6000 * level);
                    crystal = (ulong)(2000 * level);
                    break;
                case Buildable.GaussCannon:
                    metal = (ulong)(20000 * level);
                    crystal = (ulong)(15000 * level);
                    deuterium = (ulong)(2000 * level);
                    break;
                case Buildable.IonCannon:
                    metal = (ulong)(5000 * level);
                    crystal = (ulong)(3000 * level);
                    break;
                case Buildable.PlasmaTurret:
                    metal = (ulong)(50000 * level);
                    crystal = (ulong)(50000 * level);
                    deuterium = (ulong)(30000 * level);
                    break;
                case Buildable.SmallShieldDome:
                    metal = (ulong)(10000 * level);
                    crystal = (ulong)(10000 * level);
                    break;
                case Buildable.LargeShieldDome:
                    metal = (ulong)(50000 * level);
                    crystal = (ulong)(50000 * level);
                    break;
                case Buildable.AntiBallisticMissiles:
                    metal = (ulong)(8000 * level);
                    deuterium = (ulong)(2000 * level);
                    break;
                case Buildable.InterplanetaryMissiles:
                    metal = (ulong)(12500 * level);
                    crystal = (ulong)(2500 * level);
                    deuterium = (ulong)(10000 * level);
                    break;
                case Buildable.SmallCargo:
                    metal = (ulong)(2000 * level);
                    crystal = (ulong)(2000 * level);
                    break;
                case Buildable.LargeCargo:
                    metal = (ulong)(6000 * level);
                    crystal = (ulong)(6000 * level);
                    break;
                case Buildable.LightFighter:
                    metal = (ulong)(3000 * level);
                    crystal = (ulong)(1000 * level);
                    break;
                case Buildable.HeavyFighter:
                    metal = (ulong)(6000 * level);
                    crystal = (ulong)(4000 * level);
                    break;
                case Buildable.Cruiser:
                    metal = (ulong)(20000 * level);
                    crystal = (ulong)(7000 * level);
                    deuterium = (ulong)(2000 * level);
                    break;
                case Buildable.Battleship:
                    metal = (ulong)(35000 * level);
                    crystal = (ulong)(15000 * level);
                    break;
                case Buildable.ColonyShip:
                    metal = (ulong)(10000 * level);
                    crystal = (ulong)(20000 * level);
                    deuterium = (ulong)(10000 * level);
                    break;
                case Buildable.Recycler:
                    metal = (ulong)(10000 * level);
                    crystal = (ulong)(6000 * level);
                    deuterium = (ulong)(2000 * level);
                    break;
                case Buildable.EspionageProbe:
                    crystal = (ulong)(1000 * level);
                    break;
                case Buildable.Bomber:
                    metal = (ulong)(50000 * level);
                    crystal = (ulong)(25000 * level);
                    deuterium = (ulong)(15000 * level);
                    break;
                case Buildable.SolarSatellite:
                    crystal = (ulong)(2000 * level);
                    deuterium = (ulong)(500 * level);
                    break;
                case Buildable.Destroyer:
                    metal = (ulong)(60000 * level);
                    crystal = (ulong)(50000 * level);
                    deuterium = (ulong)(15000 * level);
                    break;
                case Buildable.Deathstar:
                    metal = (ulong)(5000000 * level);
                    crystal = (ulong)(4000000 * level);
                    deuterium = (ulong)(1000000 * level);
                    break;
                case Buildable.Battlecruiser:
                    metal = (ulong)(30000 * level);
                    crystal = (ulong)(40000 * level);
                    deuterium = (ulong)(15000 * level);
                    break;
                case Buildable.Crawler:
                    metal = (ulong)(2000 * level);
                    crystal = (ulong)(2000 * level);
                    deuterium = (ulong)(1000 * level);
                    break;
                case Buildable.Reaper:
                    metal = (ulong)(85000 * level);
                    crystal = (ulong)(55000 * level);
                    deuterium = (ulong)(20000 * level);
                    break;
                case Buildable.Pathfinder:
                    metal = (ulong)(8000 * level);
                    crystal = (ulong)(15000 * level);
                    deuterium = (ulong)(8000 * level);
                    break;
                case Buildable.EspionageTechnology:
                    metal = (ulong)(200 * Math.Pow(2, level - 1));
                    crystal = (ulong)(1000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(200 * Math.Pow(2, level - 1));
                    break;
                case Buildable.ComputerTechnology:
                    crystal = (ulong)(400 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(600 * Math.Pow(2, level - 1));
                    break;
                case Buildable.WeaponsTechnology:
                    metal = (ulong)(800 * Math.Pow(2, level - 1));
                    crystal = (ulong)(200 * Math.Pow(2, level - 1));
                    break;
                case Buildable.ShieldingTechnology:
                    metal = (ulong)(200 * Math.Pow(2, level - 1));
                    crystal = (ulong)(600 * Math.Pow(2, level - 1));
                    break;
                case Buildable.ArmourTechnology:
                    metal = (ulong)(1000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.EnergyTechnology:
                    crystal = (ulong)(800 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(400 * Math.Pow(2, level - 1));
                    break;
                case Buildable.HyperspaceTechnology:
                    crystal = (ulong)(4000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(2000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.CombustionDrive:
                    metal = (ulong)(400 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(600 * Math.Pow(2, level - 1));
                    break;
                case Buildable.ImpulseDrive:
                    metal = (ulong)(2000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(4000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(600 * Math.Pow(2, level - 1));
                    break;
                case Buildable.HyperspaceDrive:
                    metal = (ulong)(10000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(20000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(6000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.LaserTechnology:
                    metal = (ulong)(200 * Math.Pow(2, level - 1));
                    crystal = (ulong)(100 * Math.Pow(2, level - 1));
                    break;
                case Buildable.IonTechnology:
                    metal = (ulong)(1000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(300 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(100 * Math.Pow(2, level - 1));
                    break;
                case Buildable.PlasmaTechnology:
                    metal = (ulong)(2000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(4000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(1000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.IntergalacticResearchNetwork:
                    metal = (ulong)(240000 * Math.Pow(2, level - 1));
                    crystal = (ulong)(400000 * Math.Pow(2, level - 1));
                    deuterium = (ulong)(160000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.Astrophysics:
                    metal = (ulong)(4000 * Math.Pow(1.75, level - 1));
                    crystal = (ulong)(8000 * Math.Pow(1.75, level - 1));
                    deuterium = (ulong)(4000 * Math.Pow(1.75, level - 1));
                    break;
                case Buildable.GravitonTechnology:
                    energy = (ulong)(300000 * Math.Pow(2, level - 1));
                    break;
                case Buildable.Null:
                default:
                    break;
            }

            return new()
            {
                Metal = metal,
                Crystal = crystal,
                Deuterium = deuterium,
                Energy = energy,
            };
        }

        public static uint CalcCumulativeLabLevel(List<Celestial> celestials, Researches researches)
        {
            uint output = 0;

            if (celestials == null || celestials.Any(c => c.Facilities == null))
            {
                return 0;
            }

            try
            {
                output = (uint)celestials
                    .Where(c => c.Coordinate.Type == CelestialType.Planet)
                    .OrderByDescending(c => c.Facilities.ResearchLab)
                    .Take((int)researches.IntergalacticResearchNetwork + 1)
                    .Sum(c => c.Facilities.ResearchLab);
            }
            catch
            {
                output = 0;
            }
            return output;
        }

        public static ulong CalcProductionTime(Buildable buildable, uint level, ServerData serverData, Facilities facilities, uint cumulativeLabLevel = 0)
        {
            return CalcProductionTime(buildable, level, facilities, serverData.Speed, cumulativeLabLevel);
        }

        public static ulong CalcProductionTime(Buildable buildable, uint level, Facilities facilities, uint speed = 1, uint cumulativeLabLevel = 1, bool isDiscoverer = false, bool hasTechnocrat = false)
        {
            var output = 1d;
            var structuralIntegrity = CalcPrice(buildable, level).StructuralIntegrity;

            switch (buildable)
            {
                case Buildable.MetalMine:
                case Buildable.CrystalMine:
                case Buildable.DeuteriumSynthesizer:
                case Buildable.SolarPlant:
                case Buildable.FusionReactor:
                case Buildable.MetalStorage:
                case Buildable.CrystalStorage:
                case Buildable.DeuteriumTank:
                case Buildable.ShieldedMetalDen:
                case Buildable.UndergroundCrystalDen:
                case Buildable.SeabedDeuteriumDen:
                case Buildable.AllianceDepot:
                case Buildable.RoboticsFactory:
                case Buildable.Shipyard:
                case Buildable.ResearchLab:
                case Buildable.MissileSilo:
                case Buildable.NaniteFactory:
                case Buildable.Terraformer:
                case Buildable.SpaceDock:
                case Buildable.LunarBase:
                case Buildable.SensorPhalanx:
                case Buildable.JumpGate:
                    output = structuralIntegrity / (2500d * (1d + facilities.RoboticsFactory) * speed * Math.Pow(2, facilities.NaniteFactory));
                    break;

                case Buildable.RocketLauncher:
                case Buildable.LightLaser:
                case Buildable.HeavyLaser:
                case Buildable.GaussCannon:
                case Buildable.IonCannon:
                case Buildable.PlasmaTurret:
                case Buildable.SmallShieldDome:
                case Buildable.LargeShieldDome:
                case Buildable.AntiBallisticMissiles:
                case Buildable.InterplanetaryMissiles:
                case Buildable.SmallCargo:
                case Buildable.LargeCargo:
                case Buildable.LightFighter:
                case Buildable.HeavyFighter:
                case Buildable.Cruiser:
                case Buildable.Battleship:
                case Buildable.ColonyShip:
                case Buildable.Recycler:
                case Buildable.EspionageProbe:
                case Buildable.Bomber:
                case Buildable.SolarSatellite:
                case Buildable.Destroyer:
                case Buildable.Deathstar:
                case Buildable.Battlecruiser:
                case Buildable.Crawler:
                case Buildable.Reaper:
                case Buildable.Pathfinder:
                    output = structuralIntegrity / (2500d * (1d + facilities.Shipyard) * speed * Math.Pow(2, facilities.NaniteFactory));
                    break;

                case Buildable.EspionageTechnology:
                case Buildable.ComputerTechnology:
                case Buildable.WeaponsTechnology:
                case Buildable.ShieldingTechnology:
                case Buildable.ArmourTechnology:
                case Buildable.EnergyTechnology:
                case Buildable.HyperspaceTechnology:
                case Buildable.CombustionDrive:
                case Buildable.ImpulseDrive:
                case Buildable.HyperspaceDrive:
                case Buildable.LaserTechnology:
                case Buildable.IonTechnology:
                case Buildable.PlasmaTechnology:
                case Buildable.IntergalacticResearchNetwork:
                case Buildable.Astrophysics:
                case Buildable.GravitonTechnology:
                    if (cumulativeLabLevel == 0)
                    {
                        cumulativeLabLevel = facilities.ResearchLab;
                    }
                    output = structuralIntegrity / (1000d * (1d + cumulativeLabLevel) * speed);
                    if (isDiscoverer)
                    {
                        output = output * 3d / 4d;
                    }
                    if (hasTechnocrat)
                    {
                        output = output * 3d / 4d;
                    }
                    break;

                case Buildable.Null:
                default:
                    break;
            }

            return (ulong)Math.Round(output * 3600d, 0, MidpointRounding.ToPositiveInfinity);
        }

        public static ulong CalcMaxBuildableNumber(Buildable buildable, Resources resources)
        {
            var output = 0ul;
            Resources oneItemCost = CalcPrice(buildable, 1);

            var maxPerMet = ulong.MaxValue;
            var maxPerCry = ulong.MaxValue;
            var maxPerDeut = ulong.MaxValue;

            if (oneItemCost.Metal > 0)
                maxPerMet = (ulong)Math.Round(resources.Metal / (double)oneItemCost.Metal, 0, MidpointRounding.ToZero);
            if (oneItemCost.Crystal > 0)
                maxPerCry = (ulong)Math.Round(resources.Crystal / (double)oneItemCost.Crystal, 0, MidpointRounding.ToZero);
            if (oneItemCost.Deuterium > 0)
                maxPerDeut = (ulong)Math.Round(resources.Deuterium / (double)oneItemCost.Deuterium, 0, MidpointRounding.ToZero);

            output = Math.Min(maxPerMet, Math.Min(maxPerCry, maxPerDeut));
            if (output == ulong.MaxValue)
                output = 0;

            return output;
        }

        public static uint GetNextLevel(Celestial planet, Buildable buildable, bool isCollector = false, bool hasEngineer = false, bool hasFullStaff = false)
        {
            var output = 0u;
            if (buildable == Buildable.SolarSatellite)
            {
                if (planet is Planet)
                {
                    output = (uint)CalcNeededSolarSatellites(planet as Planet, (ulong)Math.Abs((double)planet.Resources.Energy), isCollector, hasEngineer, hasFullStaff); // TOFIX : careful on overflow, returns ulong
                }
            }

            if (output == 0 && planet is Planet)
            {
                foreach (PropertyInfo prop in planet.Buildings.GetType().GetProperties())
                {
                    if (prop.Name == buildable.ToString())
                    {
                        output = (uint)prop.GetValue(planet.Buildings) + 1;
                    }
                }
            }

            if (output == 0)
            {
                foreach (PropertyInfo prop in planet.Facilities.GetType().GetProperties())
                {
                    if (prop.Name == buildable.ToString())
                    {
                        output = (uint)prop.GetValue(planet.Facilities) + 1;
                    }
                }
            }

            return output;
        }

        public static uint GetNextLevel(Researches researches, Buildable buildable)
        {
            var output = 0u;
            foreach (PropertyInfo prop in researches.GetType().GetProperties())
            {
                if (prop.Name == buildable.ToString())
                {
                    output = (uint)prop.GetValue(researches) + 1;
                }
            }
            return output;
        }

        public static ulong CalcDepositCapacity(uint level)
        {
            return 5000 * (ulong)(2.5d * Math.Pow(Math.E, (20d * level / 33d)));
        }

        public static bool ShouldBuildMetalStorage(Planet planet, uint maxLevel, uint speedFactor, Researches researches, uint hours = 12, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool forceIfFull = false)
        {
            var metalProduction = CalcMetalProduction(planet, speedFactor, researches, ratio, playerClass, hasGeologist, hasStaff);
            var metalCapacity = CalcDepositCapacity(planet.Buildings.MetalStorage);
            if (forceIfFull && planet.Resources.Metal >= metalCapacity && GetNextLevel(planet, Buildable.MetalStorage) < maxLevel)
                return true;
            if (metalCapacity < hours * metalProduction && GetNextLevel(planet, Buildable.MetalStorage) < maxLevel)
                return true;
            else
                return false;
        }

        public static bool ShouldBuildCrystalStorage(Planet planet, uint maxLevel, uint speedFactor, Researches researches, uint hours = 12, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool forceIfFull = false)
        {
            var crystalProduction = CalcCrystalProduction(planet, speedFactor, researches, ratio, playerClass, hasGeologist, hasStaff);
            var crystalCapacity = CalcDepositCapacity(planet.Buildings.CrystalStorage);
            if (forceIfFull && planet.Resources.Crystal >= crystalCapacity && GetNextLevel(planet, Buildable.CrystalStorage) < maxLevel)
                return true;
            if (crystalCapacity < hours * crystalProduction && GetNextLevel(planet, Buildable.CrystalStorage) < maxLevel)
                return true;
            else
                return false;
        }

        public static bool ShouldBuildDeuteriumTank(Planet planet, uint maxLevel, uint speedFactor, Researches researches, uint hours = 12, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool forceIfFull = false)
        {
            var deuteriumProduction = CalcDeuteriumProduction(planet, speedFactor, researches, ratio, playerClass, hasGeologist, hasStaff);
            var deuteriumCapacity = CalcDepositCapacity(planet.Buildings.DeuteriumTank);
            if (forceIfFull && planet.Resources.Deuterium >= deuteriumCapacity && GetNextLevel(planet, Buildable.DeuteriumTank) < maxLevel)
                return true;
            if (deuteriumCapacity < hours * deuteriumProduction && GetNextLevel(planet, Buildable.DeuteriumTank) < maxLevel)
                return true;
            else
                return false;
        }

        public static bool ShouldBuildEnergySource(Planet planet)
        {
            if (planet.Resources.Energy < 0)
                return true;
            else
                return false;
        }

        public static Buildable GetNextEnergySourceToBuild(Planet planet, uint maxSolarPlant, uint maxFusionReactor)
        {
            if (planet.Buildings.SolarPlant < maxSolarPlant)
                return Buildable.SolarPlant;
            if (planet.Buildings.FusionReactor < maxFusionReactor)
                return Buildable.FusionReactor;
            return Buildable.SolarSatellite;
        }

        public static uint GetSolarSatelliteOutput(Planet planet, bool isCollector = false, bool hasEngineer = false, bool hasFullStaff = false)
        {
            var production = (planet.Temperature.Average + 160d) / 6d;
            var collectorProd = 0d;
            var engineerProd = 0d;
            var staffProd = 0d;
            if (isCollector)
                collectorProd = 0.1d * production;
            if (hasEngineer)
                engineerProd = 0.1d * production;
            if (hasFullStaff)
                staffProd = 0.02d * production;
            return (uint)Math.Round(production + collectorProd + engineerProd + staffProd);
        }

        public static ulong CalcNeededSolarSatellites(Planet planet, ulong requiredEnergy = 0, bool isCollector = false, bool hasEngineer = false, bool hasFullStaff = false)
        {
            if (requiredEnergy == 0)
            {
                if (planet.Resources.Energy > 0)
                {
                    return 0;
                }

                return (ulong)Math.Round((float)(Math.Abs((double)planet.Resources.Energy) / GetSolarSatelliteOutput(planet, isCollector, hasEngineer, hasFullStaff)), MidpointRounding.ToPositiveInfinity);
            }

            return (ulong)Math.Round((float)(Math.Abs((double)requiredEnergy) / GetSolarSatelliteOutput(planet, isCollector, hasEngineer, hasFullStaff)), MidpointRounding.ToPositiveInfinity);
        }

        public static Buildable GetNextMineToBuild(Planet planet, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, bool optimizeForStart = true)
        {
            if (optimizeForStart && (planet.Buildings.MetalMine < 10 || planet.Buildings.CrystalMine < 7 || planet.Buildings.DeuteriumSynthesizer < 5))
            {
                if (planet.Buildings.MetalMine <= planet.Buildings.CrystalMine + 2)
                {
                    return Buildable.MetalMine;
                }
                else if (planet.Buildings.CrystalMine <= planet.Buildings.DeuteriumSynthesizer + 2)
                {
                    return Buildable.CrystalMine;
                }

                return Buildable.DeuteriumSynthesizer;
            }


            Dictionary<Buildable, ulong> dic = new();

            var mines = new List<Buildable> { Buildable.MetalMine, Buildable.CrystalMine, Buildable.DeuteriumSynthesizer };
            foreach (var mine in mines)
            {
                switch (mine)
                {
                    case Buildable.MetalMine when GetNextLevel(planet, mine) > maxMetalMine:
                        continue;
                    case Buildable.CrystalMine when GetNextLevel(planet, mine) > maxCrystalMine:
                        continue;
                    case Buildable.DeuteriumSynthesizer when GetNextLevel(planet, mine) > maxDeuteriumSynthetizer:
                        continue;
                }

                dic.Add(mine, CalcPrice(mine, GetNextLevel(planet, mine)).ConvertedDeuterium);
            }

            if (dic.Count == 0)
            {
                return Buildable.Null;
            }

            return dic
                .OrderBy(m => m.Value)
                .ToDictionary(m => m.Key, m => m.Value)
                .FirstOrDefault()
                .Key;
        }

        public static Buildable GetNextMineToBuild(Planet planet, Researches researches, uint speedFactor = 1, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool optimizeForStart = true, double maxDaysOfInvestmentReturn = 36500)
        {
            if (optimizeForStart && (planet.Buildings.MetalMine < 10 || planet.Buildings.CrystalMine < 7 || planet.Buildings.DeuteriumSynthesizer < 5))
            {
                if (planet.Buildings.MetalMine <= planet.Buildings.CrystalMine + 2 && planet.Buildings.MetalMine < maxMetalMine)
                    return Buildable.MetalMine;
                else if (planet.Buildings.CrystalMine <= planet.Buildings.DeuteriumSynthesizer + 2 && planet.Buildings.CrystalMine < maxCrystalMine)
                    return Buildable.CrystalMine;
                else if (planet.Buildings.DeuteriumSynthesizer < maxDeuteriumSynthetizer)
                    return Buildable.DeuteriumSynthesizer;
            }

            var mines = new List<Buildable> { Buildable.MetalMine, Buildable.CrystalMine, Buildable.DeuteriumSynthesizer };
            Dictionary<Buildable, double> dic = new();
            foreach (var mine in mines)
            {
                if (mine == Buildable.MetalMine && GetNextLevel(planet, mine) > maxMetalMine)
                    continue;
                if (mine == Buildable.CrystalMine && GetNextLevel(planet, mine) > maxCrystalMine)
                    continue;
                if (mine == Buildable.DeuteriumSynthesizer && GetNextLevel(planet, mine) > maxDeuteriumSynthetizer)
                    continue;

                dic.Add(mine, CalcDaysOfInvestmentReturn(planet, mine, researches, speedFactor, ratio, playerClass, hasGeologist, hasStaff));
            }
            if (dic.Count == 0)
                return Buildable.Null;

            dic = dic.OrderBy(m => m.Value)
                .ToDictionary(m => m.Key, m => m.Value);
            var bestMine = dic.FirstOrDefault().Key;

            if (maxDaysOfInvestmentReturn >= CalcDaysOfInvestmentReturn(planet, bestMine, researches, speedFactor, ratio, playerClass, hasGeologist, hasStaff))
                return bestMine;
            else
                return Buildable.Null;
        }

        public static double CalcROI(Planet planet, Buildable buildable, Researches researches, uint speedFactor = 1, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            double currentProd;
            double nextLevelProd;
            ulong cost;

            switch (buildable)
            {
                case Buildable.MetalMine:
                    currentProd = CalcMetalProduction(planet.Buildings.MetalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d;
                    nextLevelProd = CalcMetalProduction(planet.Buildings.MetalMine + 1, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d;
                    cost = CalcPrice(buildable, GetNextLevel(planet, buildable)).ConvertedDeuterium;
                    break;
                case Buildable.CrystalMine:
                    currentProd = CalcCrystalProduction(planet.Buildings.CrystalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d;
                    nextLevelProd = CalcCrystalProduction(planet.Buildings.CrystalMine + 1, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d;
                    cost = CalcPrice(buildable, GetNextLevel(planet, buildable)).ConvertedDeuterium;
                    break;
                case Buildable.DeuteriumSynthesizer:
                    currentProd = CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
                    nextLevelProd = CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer + 1, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff);
                    cost = CalcPrice(buildable, GetNextLevel(planet, buildable)).ConvertedDeuterium;
                    break;
                default:
                    return 0;
            }

            var delta = nextLevelProd - currentProd;
            return delta / cost;
        }

        public static double CalcDaysOfInvestmentReturn(Planet planet, Buildable buildable, Researches researches, uint speedFactor = 1, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            if (buildable == Buildable.MetalMine || buildable == Buildable.CrystalMine || buildable == Buildable.DeuteriumSynthesizer)
            {
                var currentOneDayProd = 1d;
                var nextOneDayProd = 1d;

                switch (buildable)
                {
                    case Buildable.MetalMine:
                        currentOneDayProd = CalcMetalProduction(planet.Buildings.MetalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d * 24d;
                        nextOneDayProd = CalcMetalProduction(planet.Buildings.MetalMine + 1, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d * 24d;
                        break;
                    case Buildable.CrystalMine:
                        currentOneDayProd = CalcCrystalProduction(planet.Buildings.CrystalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d * 24d;
                        nextOneDayProd = CalcCrystalProduction(planet.Buildings.CrystalMine + 1, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d * 24d;
                        break;
                    case Buildable.DeuteriumSynthesizer:
                        currentOneDayProd = CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) * 24d;
                        nextOneDayProd = CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer + 1, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) * 24d;
                        break;
                    default:

                        break;
                }

                var cost = CalcPrice(buildable, GetNextLevel(planet, buildable)).ConvertedDeuterium;
                var delta = nextOneDayProd - currentOneDayProd;
                return cost / delta;
            }

            return double.MaxValue;
        }

        public static double CalcNextDaysOfInvestmentReturn(Planet planet, Researches researches, uint speedFactor = 1, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            var metalCost = CalcPrice(Buildable.MetalMine, GetNextLevel(planet, Buildable.MetalMine)).ConvertedDeuterium;
            var currentOneDayMetalProd = CalcMetalProduction(planet.Buildings.MetalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d * 24d;
            var nextOneDayMetalProd = CalcMetalProduction(planet.Buildings.MetalMine + 1, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d * 24d;
            var metalDOIR = metalCost / (nextOneDayMetalProd - currentOneDayMetalProd);

            var crystalCost = CalcPrice(Buildable.CrystalMine, GetNextLevel(planet, Buildable.CrystalMine)).ConvertedDeuterium;
            var currentOneDayCrystalProd = CalcCrystalProduction(planet.Buildings.CrystalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d * 24d;
            var nextOneDayCrystalProd = CalcCrystalProduction(planet.Buildings.CrystalMine + 1, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d * 24d;
            var crystalDOIR = crystalCost / (nextOneDayCrystalProd - currentOneDayCrystalProd);

            var deuteriumCost = CalcPrice(Buildable.DeuteriumSynthesizer, GetNextLevel(planet, Buildable.DeuteriumSynthesizer)).ConvertedDeuterium;
            var currentOneDayDeuteriumProd = CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) * 24;
            var nextOneDayDeuteriumProd = CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer + 1, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) * 24;
            var deuteriumDOIR = deuteriumCost / (nextOneDayDeuteriumProd - currentOneDayDeuteriumProd);

            return Math.Min(double.IsNaN(deuteriumDOIR) ? float.MaxValue : deuteriumDOIR, Math.Min(double.IsNaN(crystalDOIR) ? float.MaxValue : crystalDOIR, double.IsNaN(metalDOIR) ? float.MaxValue : metalDOIR));
        }

        public static double CalcNextPlasmaTechDOIR(List<Planet> planets, Researches researches, uint speedFactor = 1, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            var nextPlasmaLevel = researches.PlasmaTechnology + 1;
            var nextPlasmaCost = CalcPrice(Buildable.PlasmaTechnology, nextPlasmaLevel).ConvertedDeuterium;

            var currentProd = 0d;
            var nextProd = 0d;

            foreach (var planet in planets)
            {
                currentProd += (ulong)Math.Round(CalcMetalProduction(planet.Buildings.MetalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d * 24d, 0);
                currentProd += (ulong)Math.Round(CalcCrystalProduction(planet.Buildings.CrystalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d * 24d, 0);
                currentProd += CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) * 24;

                nextProd += (ulong)Math.Round(CalcMetalProduction(planet.Buildings.MetalMine, planet.Coordinate.Position, speedFactor, ratio, nextPlasmaLevel, playerClass, hasGeologist, hasStaff) / 2.5d * 24d, 0);
                nextProd += (ulong)Math.Round(CalcCrystalProduction(planet.Buildings.CrystalMine, planet.Coordinate.Position, speedFactor, ratio, nextPlasmaLevel, playerClass, hasGeologist, hasStaff) / 1.5d * 24d, 0);
                nextProd += CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer, planet.Temperature.Average, speedFactor, ratio, nextPlasmaLevel, playerClass, hasGeologist, hasStaff) * 24;
            }

            var delta = nextProd - currentProd;
            return nextPlasmaCost / delta;
        }

        public static double CalcNextAstroDOIR(List<Planet> planets, Researches researches, uint speedFactor = 1, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false)
        {
            var nextAstroCost = CalcPrice(Buildable.Astrophysics, researches.Astrophysics + 1).ConvertedDeuterium;
            if (researches.Astrophysics % 2 != 0)
            {
                nextAstroCost += CalcPrice(Buildable.Astrophysics, researches.Astrophysics + 2).ConvertedDeuterium;
            }

            var averageMetal = (uint)Math.Round(planets.Average(p => p.Buildings.MetalMine), 0);
            var metalCost = 0ul;
            for (var i = 1u; i <= averageMetal; i++)
            {
                metalCost += CalcPrice(Buildable.MetalMine, i).ConvertedDeuterium;
            }

            var averageCrystal = (uint)Math.Round(planets.Average(p => p.Buildings.CrystalMine), 0);
            var crystalCost = 0ul;
            for (var i = 1u; i <= averageCrystal; i++)
            {
                crystalCost += CalcPrice(Buildable.CrystalMine, i).ConvertedDeuterium;
            }

            var averageDeuterium = (uint)Math.Round(planets.Average(p => p.Buildings.DeuteriumSynthesizer), 0);
            var deuteriumCost = 0ul;
            for (var i = 1u; i <= averageDeuterium; i++)
            {
                deuteriumCost += CalcPrice(Buildable.DeuteriumSynthesizer, i).ConvertedDeuterium;
            }

            var totalCost = nextAstroCost + metalCost + crystalCost + deuteriumCost;

            var dailyProd = 0ul;
            foreach (var planet in planets)
            {
                dailyProd += (ulong)Math.Round(CalcMetalProduction(planet.Buildings.MetalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 2.5d * 24d, 0);
                dailyProd += (ulong)Math.Round(CalcCrystalProduction(planet.Buildings.CrystalMine, planet.Coordinate.Position, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) / 1.5d * 24d, 0);
                dailyProd += CalcDeuteriumProduction(planet.Buildings.DeuteriumSynthesizer, planet.Temperature.Average, speedFactor, ratio, researches.PlasmaTechnology, playerClass, hasGeologist, hasStaff) * 24;
            }
            var nextDailyProd = dailyProd + (ulong)Math.Round((float)dailyProd / (float)planets.Count, 0);

            var delta = nextDailyProd - dailyProd;
            return totalCost / delta;
        }

        public static Buildable GetNextMineToBuild(List<Celestial> celestials, Researches researches, Buildings maxBuildings, Facilities maxFacilities, PlayerClass playerClass, Staff staff, ServerData serverData, AutoMinerSettings settings, double ratio = 1)
        {
            var dic = new Dictionary<Planet, double>();
            foreach (var c in celestials.Where(c2 => c2 is Planet))
            {
                dic.Add(c as Planet, CalcNextDaysOfInvestmentReturn(c as Planet, researches, serverData.Speed, ratio, playerClass, staff.Geologist, staff.IsFull));
            }
            return GetNextMineToBuild(dic.OrderBy(dic => dic.Value).First().Key, researches, maxBuildings, maxFacilities, playerClass, staff, serverData, settings, ratio);
        }

        public static Buildable GetNextBuildingToBuild(Planet planet, Researches researches, Buildings maxBuildings, Facilities maxFacilities, PlayerClass playerClass, Staff staff, ServerData serverData, AutoMinerSettings settings, double ratio = 1)
        {
            Buildable buildableToBuild = Buildable.Null;
            if (ShouldBuildTerraformer(planet, researches, maxFacilities.Terraformer))
                buildableToBuild = Buildable.Terraformer;
            if (buildableToBuild == Buildable.Null && ShouldBuildEnergySource(planet))
                buildableToBuild = GetNextEnergySourceToBuild(planet, maxBuildings.SolarPlant, maxBuildings.FusionReactor);
            if (buildableToBuild == Buildable.Null)
                buildableToBuild = GetNextDepositToBuild(planet, researches, maxBuildings, playerClass, staff, serverData, settings, ratio);
            if (buildableToBuild == Buildable.Null)
                buildableToBuild = GetNextFacilityToBuild(planet, researches, maxBuildings, maxFacilities, playerClass, staff, serverData, settings, ratio);
            if (buildableToBuild == Buildable.Null)
                buildableToBuild = GetNextMineToBuild(planet, researches, maxBuildings, maxFacilities, playerClass, staff, serverData, settings, ratio);
            if (buildableToBuild == Buildable.Null)
                buildableToBuild = GetNextFacilityToBuild(planet, researches, maxBuildings, maxFacilities, playerClass, staff, serverData, settings, ratio, true);

            return buildableToBuild;
        }

        public static Buildable GetNextDepositToBuild(Planet planet, Researches researches, Buildings maxBuildings, PlayerClass playerClass, Staff staff, ServerData serverData, AutoMinerSettings settings, double ratio = 1)
        {
            Buildable depositToBuild = Buildable.Null;
            if (
                settings.OptimizeForStart &&
                planet.Buildings.MetalMine < 13 &&
                planet.Buildings.CrystalMine < 12 &&
                planet.Buildings.DeuteriumSynthesizer < 10 &&
                planet.Buildings.SolarPlant < 13 &&
                planet.Buildings.FusionReactor < 5 &&
                planet.Facilities.RoboticsFactory < 5 &&
                planet.Facilities.Shipyard < 5 &&
                planet.Facilities.ResearchLab < 5
            )
            {
                return depositToBuild;
            }

            if (depositToBuild == Buildable.Null && ShouldBuildDeuteriumTank(planet, maxBuildings.DeuteriumTank, serverData.Speed, researches, settings.DepositHours, ratio, playerClass, staff.Geologist, staff.IsFull, settings.BuildDepositIfFull))
                depositToBuild = Buildable.DeuteriumTank;
            if (depositToBuild == Buildable.Null && ShouldBuildCrystalStorage(planet, maxBuildings.CrystalStorage, serverData.Speed, researches, settings.DepositHours, ratio, playerClass, staff.Geologist, staff.IsFull, settings.BuildDepositIfFull))
                depositToBuild = Buildable.CrystalStorage;
            if (depositToBuild == Buildable.Null && ShouldBuildMetalStorage(planet, maxBuildings.MetalStorage, serverData.Speed, researches, settings.DepositHours, ratio, playerClass, staff.Geologist, staff.IsFull, settings.BuildDepositIfFull))
                depositToBuild = Buildable.MetalStorage;

            return depositToBuild;
        }
        
        public static Buildable GetNextFacilityToBuild(Planet planet, Researches researches, Buildings maxBuildings, Facilities maxFacilities, PlayerClass playerClass, Staff staff, ServerData serverData, AutoMinerSettings settings, double ratio = 1, bool force = false)
        {
            Buildable facilityToBuild = Buildable.Null;
            if (settings.PrioritizeRobotsAndNanites)
            {
                if (planet.Facilities.RoboticsFactory < 10 && planet.Facilities.RoboticsFactory < maxFacilities.RoboticsFactory)
                    facilityToBuild = Buildable.RoboticsFactory;
                else if (planet.Facilities.RoboticsFactory >= 10 && researches.ComputerTechnology >= 10 && planet.Facilities.NaniteFactory < maxFacilities.NaniteFactory && !planet.HasProduction())
                    facilityToBuild = Buildable.NaniteFactory;
            }

            if (facilityToBuild == Buildable.Null && ShouldBuildSpaceDock(planet, researches, maxFacilities.SpaceDock, serverData.Speed, maxBuildings.MetalMine, maxBuildings.CrystalMine, maxBuildings.DeuteriumSynthesizer, 1, playerClass, staff.Geologist, staff.IsFull, settings.OptimizeForStart, settings.MaxDaysOfInvestmentReturn, force))
                facilityToBuild = Buildable.SpaceDock;
            if (facilityToBuild == Buildable.Null && ShouldBuildNanites(planet, researches, maxFacilities.NaniteFactory, serverData.Speed, maxBuildings.MetalMine, maxBuildings.CrystalMine, maxBuildings.DeuteriumSynthesizer, 1, playerClass, staff.Geologist, staff.IsFull, settings.OptimizeForStart, settings.MaxDaysOfInvestmentReturn, force) && !planet.HasProduction())
                facilityToBuild = Buildable.NaniteFactory;
            if (facilityToBuild == Buildable.Null && ShouldBuildRoboticFactory(planet, researches, maxFacilities.RoboticsFactory, serverData.Speed, maxBuildings.MetalMine, maxBuildings.CrystalMine, maxBuildings.DeuteriumSynthesizer, 1, playerClass, staff.Geologist, staff.IsFull, settings.OptimizeForStart, settings.MaxDaysOfInvestmentReturn, force))
                facilityToBuild = Buildable.RoboticsFactory;
            if (facilityToBuild == Buildable.Null && ShouldBuildShipyard(planet, researches, maxFacilities.Shipyard, serverData.Speed, maxBuildings.MetalMine, maxBuildings.CrystalMine, maxBuildings.DeuteriumSynthesizer, 1, playerClass, staff.Geologist, staff.IsFull, settings.OptimizeForStart, settings.MaxDaysOfInvestmentReturn, force) && !planet.HasProduction())
                facilityToBuild = Buildable.Shipyard;
            if (facilityToBuild == Buildable.Null && ShouldBuildResearchLab(planet, researches, maxFacilities.ResearchLab, serverData.Speed, serverData.ResearchDurationDivisor, maxBuildings.MetalMine, maxBuildings.CrystalMine, maxBuildings.DeuteriumSynthesizer, 1, playerClass, staff.Geologist, staff.IsFull, settings.OptimizeForStart, settings.MaxDaysOfInvestmentReturn, force) && planet.Constructions.ResearchId == 0)
                facilityToBuild = Buildable.ResearchLab;
            if (facilityToBuild == Buildable.Null && ShouldBuildMissileSilo(planet, researches, maxFacilities.MissileSilo, serverData.Speed, maxBuildings.MetalMine, maxBuildings.CrystalMine, maxBuildings.DeuteriumSynthesizer, 1, playerClass, staff.Geologist, staff.IsFull, settings.OptimizeForStart, settings.MaxDaysOfInvestmentReturn, force))
                facilityToBuild = Buildable.MissileSilo;

            return facilityToBuild;
        }

        public static Buildable GetNextMineToBuild(Planet planet, Researches researches, Buildings maxBuildings, Facilities maxFacilities, PlayerClass playerClass, Staff staff, ServerData serverData, AutoMinerSettings settings, double ratio = 1)
        {
            return GetNextMineToBuild(planet, researches, serverData.Speed, maxBuildings.MetalMine, maxBuildings.CrystalMine, maxBuildings.DeuteriumSynthesizer, ratio, playerClass, staff.Geologist, staff.IsFull, settings.OptimizeForStart, settings.MaxDaysOfInvestmentReturn);
        }

        public static Buildable GetNextLunarFacilityToBuild(Moon moon, Researches researches, Facilities maxLunarFacilities)
        {
            return GetNextLunarFacilityToBuild(moon, researches, maxLunarFacilities.LunarBase, maxLunarFacilities.RoboticsFactory, maxLunarFacilities.SensorPhalanx, maxLunarFacilities.JumpGate, maxLunarFacilities.Shipyard);
        }

        public static Buildable GetNextLunarFacilityToBuild(Moon moon, Researches researches, uint maxLunarBase = 8, uint maxRoboticsFactory = 8, uint maxSensorPhalanx = 6, uint maxJumpGate = 1, uint maxShipyard = 0)
        {
            Buildable lunarFacilityToBuild = Buildable.Null;

            if (ShouldBuildLunarBase(moon, maxLunarBase))
                lunarFacilityToBuild = Buildable.LunarBase;
            if (lunarFacilityToBuild == Buildable.Null && ShouldBuildRoboticFactory(moon, researches, maxRoboticsFactory))
                lunarFacilityToBuild = Buildable.RoboticsFactory;
            if (lunarFacilityToBuild == Buildable.Null && ShouldBuildJumpGate(moon, researches, maxJumpGate))
                lunarFacilityToBuild = Buildable.JumpGate;
            if (lunarFacilityToBuild == Buildable.Null && ShouldBuildSensorPhalanx(moon, maxSensorPhalanx))
                lunarFacilityToBuild = Buildable.SensorPhalanx;
            if (lunarFacilityToBuild == Buildable.Null && ShouldBuildShipyard(moon, researches, maxShipyard))
                lunarFacilityToBuild = Buildable.Shipyard;

            return lunarFacilityToBuild;
        }

        public static bool ShouldBuildRoboticFactory(Celestial celestial, Researches researches, uint maxLevel = 10, uint speedFactor = 1, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool optimizeForStart = true, double maxDaysOfInvestmentReturn = 36500, bool force = false)
        {
            var nextRobotsLevel = GetNextLevel(celestial, Buildable.RoboticsFactory);

            if (celestial is Planet)
            {
                var nextMine = GetNextMineToBuild(celestial as Planet, researches, speedFactor, maxMetalMine, maxCrystalMine, maxDeuteriumSynthetizer, ratio, playerClass, hasGeologist, hasStaff, optimizeForStart, maxDaysOfInvestmentReturn);
                var nextMineLevel = GetNextLevel(celestial, nextMine);
                var nextMinePrice = CalcPrice(nextMine, nextMineLevel);
                var nextMineTime = CalcProductionTime(nextMine, nextMineLevel, celestial.Facilities, speedFactor);

                var nextRobotsPrice = CalcPrice(Buildable.RoboticsFactory, nextRobotsLevel);
                var nextRobotsTime = CalcProductionTime(Buildable.RoboticsFactory, nextRobotsLevel, celestial.Facilities, speedFactor);

                return nextRobotsLevel <= maxLevel &&
                    (nextMinePrice.ConvertedDeuterium > nextRobotsPrice.ConvertedDeuterium || force);
            }

            return nextRobotsLevel <= maxLevel &&
                celestial.Fields.Free > 1;
        }

        public static bool ShouldBuildShipyard(Celestial celestial, Researches researches, uint maxLevel = 12, uint speedFactor = 1, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool optimizeForStart = true, double maxDaysOfInvestmentReturn = 36500, bool force = false)
        {
            var nextShipyardLevel = GetNextLevel(celestial as Planet, Buildable.Shipyard);

            if (celestial is Planet)
            {
                var nextMine = GetNextMineToBuild(celestial as Planet, researches, speedFactor, maxMetalMine, maxCrystalMine, maxDeuteriumSynthetizer, ratio, playerClass, hasGeologist, hasStaff, optimizeForStart, maxDaysOfInvestmentReturn);
                var nextMineLevel = GetNextLevel(celestial, nextMine);
                var nextMinePrice = CalcPrice(nextMine, nextMineLevel);

                var nextShipyardPrice = CalcPrice(Buildable.Shipyard, nextShipyardLevel);

                return nextShipyardLevel <= maxLevel &&
                    (nextMinePrice.ConvertedDeuterium > nextShipyardPrice.ConvertedDeuterium || force) &&
                    celestial.Facilities.RoboticsFactory >= 2;
            }

            return nextShipyardLevel <= maxLevel &&
                celestial.Fields.Free > 1;
        }

        public static bool ShouldBuildResearchLab(Planet celestial, Researches researches, uint maxLevel = 12, uint speedFactor = 1, double researchDurationDivisor = 2, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool optimizeForStart = true, double maxDaysOfInvestmentReturn = 36500, bool force = false)
        {
            var nextMine = GetNextMineToBuild(celestial, researches, speedFactor, maxMetalMine, maxCrystalMine, maxDeuteriumSynthetizer, ratio, playerClass, hasGeologist, hasStaff, optimizeForStart, maxDaysOfInvestmentReturn);
            var nextMineLevel = GetNextLevel(celestial, nextMine);
            var nextMinePrice = CalcPrice(nextMine, nextMineLevel);

            var nextLabLevel = GetNextLevel(celestial, Buildable.ResearchLab);
            var nextLabPrice = CalcPrice(Buildable.ResearchLab, nextLabLevel);

            var nextResearch = GetNextResearchToBuild(celestial, researches, new()); // TOFIX : why was "slots" null?
            var nextResearchLevel = GetNextLevel(researches, nextResearch);
            var nextResearchTime = CalcProductionTime(nextResearch, nextResearchLevel, celestial.Facilities, (uint)Math.Round(speedFactor * researchDurationDivisor));
            var nextLabTime = CalcProductionTime(Buildable.ResearchLab, nextLabLevel, celestial.Facilities, speedFactor);

            return nextLabLevel <= maxLevel &&
                (nextMinePrice.ConvertedDeuterium > nextLabPrice.ConvertedDeuterium || force);
        }

        public static bool ShouldBuildMissileSilo(Planet celestial, Researches researches, uint maxLevel = 6, uint speedFactor = 1, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool optimizeForStart = true, double maxDaysOfInvestmentReturn = 36500, bool force = false)
        {
            var nextMine = GetNextMineToBuild(celestial, researches, speedFactor, maxMetalMine, maxCrystalMine, maxDeuteriumSynthetizer, ratio, playerClass, hasGeologist, hasStaff, optimizeForStart, maxDaysOfInvestmentReturn);
            var nextMineLevel = GetNextLevel(celestial, nextMine);
            var nextMinePrice = CalcPrice(nextMine, nextMineLevel);

            var nextSiloLevel = GetNextLevel(celestial, Buildable.MissileSilo);
            var nextSiloPrice = CalcPrice(Buildable.MissileSilo, nextSiloLevel);

            return nextSiloLevel <= maxLevel &&
                (nextMinePrice.ConvertedDeuterium > nextSiloPrice.ConvertedDeuterium || force) &&
                celestial.Facilities.Shipyard >= 1;
        }

        public static bool ShouldBuildNanites(Planet celestial, Researches researches, uint maxLevel = 10, uint speedFactor = 1, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool optimizeForStart = true, double maxDaysOfInvestmentReturn = 36500, bool force = false)
        {
            var nextMine = GetNextMineToBuild(celestial, researches, speedFactor, maxMetalMine, maxCrystalMine, maxDeuteriumSynthetizer, ratio, playerClass, hasGeologist, hasStaff, optimizeForStart, maxDaysOfInvestmentReturn);
            var nextMineLevel = GetNextLevel(celestial, nextMine);
            var nextMinePrice = CalcPrice(nextMine, nextMineLevel);
            var nextMineTime = CalcProductionTime(nextMine, nextMineLevel, celestial.Facilities, speedFactor);

            var nextNanitesLevel = GetNextLevel(celestial, Buildable.NaniteFactory);
            var nextNanitesPrice = CalcPrice(Buildable.NaniteFactory, nextNanitesLevel);
            var nextNanitesTime = CalcProductionTime(Buildable.NaniteFactory, nextNanitesLevel, celestial.Facilities, speedFactor);

            return nextNanitesLevel <= maxLevel &&
                (nextMinePrice.ConvertedDeuterium > nextNanitesPrice.ConvertedDeuterium || nextNanitesTime < nextMineTime || force) &&
                celestial.Facilities.RoboticsFactory >= 10 &&
                researches.ComputerTechnology >= 10;
        }

        public static bool ShouldBuildTerraformer(Planet celestial, Researches researches, uint maxLevel = 10)
        {
            if (researches.EnergyTechnology < 12)
            {
                return false;
            }

            var nextLevel = GetNextLevel(celestial, Buildable.Terraformer);
            return celestial.Fields.Free == 1 &&
                nextLevel <= maxLevel;
        }

        public static bool ShouldBuildSpaceDock(Planet celestial, Researches researches, uint maxLevel = 10, uint speedFactor = 1, uint maxMetalMine = 100, uint maxCrystalMine = 100, uint maxDeuteriumSynthetizer = 100, double ratio = 1, PlayerClass playerClass = PlayerClass.NoClass, bool hasGeologist = false, bool hasStaff = false, bool optimizeForStart = true, double maxDaysOfInvestmentReturn = 36500, bool force = false)
        {
            var nextMine = GetNextMineToBuild(celestial, researches, speedFactor, maxMetalMine, maxCrystalMine, maxDeuteriumSynthetizer, ratio, playerClass, hasGeologist, hasStaff, optimizeForStart, maxDaysOfInvestmentReturn);
            var nextMineLevel = GetNextLevel(celestial, nextMine);
            var nextMinePrice = CalcPrice(nextMine, nextMineLevel);

            var nextSpaceDockLevel = GetNextLevel(celestial, Buildable.SpaceDock);
            var nextSpaceDockPrice = CalcPrice(Buildable.SpaceDock, nextSpaceDockLevel);

            return nextSpaceDockLevel <= maxLevel &&
                (nextMinePrice.ConvertedDeuterium > nextSpaceDockPrice.ConvertedDeuterium || force) &&
                celestial.ResourcesProduction.Energy.CurrentProduction >= nextSpaceDockPrice.Energy &&
                celestial.Facilities.Shipyard >= 2;
        }

        public static bool ShouldBuildLunarBase(Moon moon, uint maxLevel = 8)
        {
            var nextLunarBaseLevel = GetNextLevel(moon, Buildable.LunarBase);

            return nextLunarBaseLevel <= maxLevel &&
                moon.Fields.Free == 1;
        }

        public static bool ShouldBuildSensorPhalanx(Moon moon, uint maxLevel = 7)
        {
            var nextSensorPhalanxLevel = GetNextLevel(moon, Buildable.SensorPhalanx);

            return nextSensorPhalanxLevel <= maxLevel &&
                moon.Facilities.LunarBase >= 1 &&
                moon.Fields.Free > 1;
        }

        public static bool ShouldBuildJumpGate(Moon moon, Researches researches, uint maxLevel = 1)
        {
            var nextJumpGateLevel = GetNextLevel(moon, Buildable.JumpGate);

            return nextJumpGateLevel <= maxLevel &&
                moon.Facilities.LunarBase >= 1 &&
                researches.HyperspaceTechnology >= 7 &&
                moon.Fields.Free > 1;
        }

        public static bool ShouldResearchEnergyTech(List<Planet> planets, uint energyTech, uint maxEnergyTech = 25, PlayerClass playerClass = PlayerClass.NoClass, bool hasEngineer = false, bool hasStaff = false)
        {
            if (energyTech >= maxEnergyTech)
            {
                return false;
            }

            if (!planets.Any(p => p.Buildings.FusionReactor >= 1))
            {
                return false;
            }

            var avgFusion = (uint)Math.Round(planets.Where(p => p.Buildings.FusionReactor > 0).Average(p => p.Buildings.FusionReactor));
            var energyProd = (ulong)Math.Round(planets.Where(p => p.Buildings.FusionReactor > 0).Average(p => (double)CalcEnergyProduction(Buildable.FusionReactor, p.Buildings.FusionReactor, energyTech, 1, playerClass, hasEngineer, hasStaff)));
            var avgEnergyProd = CalcEnergyProduction(Buildable.FusionReactor, avgFusion, energyTech, 1, playerClass, hasEngineer, hasStaff);

            var fusionCost = CalcPrice(Buildable.FusionReactor, avgFusion + 1).ConvertedDeuterium * (uint)planets.Where(p => p.Buildings.FusionReactor > 0).Count();
            var fusionEnergy = CalcEnergyProduction(Buildable.FusionReactor, avgFusion + 1, energyTech, 1, playerClass, hasEngineer, hasStaff);
            var fusionRatio = fusionEnergy / (double)fusionCost;
            var energyTechCost = CalcPrice(Buildable.EnergyTechnology, energyTech + 1).ConvertedDeuterium;
            var energyTechEnergy = CalcEnergyProduction(Buildable.FusionReactor, avgFusion, energyTech + 1, 1, playerClass, hasEngineer, hasStaff);
            var energyTechRatio = energyTechEnergy / (double)energyTechCost;

            return energyTechRatio >= fusionRatio;
        }

        public static bool ShouldResearchEnergyTech(List<Planet> planets, Researches researches, uint maxEnergyTech = 25, PlayerClass playerClass = PlayerClass.NoClass, bool hasEngineer = false, bool hasStaff = false)
        {
            return ShouldResearchEnergyTech(planets, researches.EnergyTechnology, maxEnergyTech, playerClass, hasEngineer, hasStaff);
        }

        public static Buildable GetNextResearchToBuild(Planet celestial, Researches researches, Slots slots, bool prioritizeRobotsAndNanitesOnNewPlanets = false, uint maxEnergyTechnology = 20, uint maxLaserTechnology = 12, uint maxIonTechnology = 5, uint maxHyperspaceTechnology = 20, uint maxPlasmaTechnology = 20, uint maxCombustionDrive = 19, uint maxImpulseDrive = 17, uint maxHyperspaceDrive = 15, uint maxEspionageTechnology = 8, uint maxComputerTechnology = 20, uint maxAstrophysics = 23, uint maxIntergalacticResearchNetwork = 12, uint maxWeaponsTechnology = 25, uint maxShieldingTechnology = 25, uint maxArmourTechnology = 25, bool optimizeForStart = true, bool ensureExpoSlots = true)
        {
            if (optimizeForStart)
            {
                if (researches.EnergyTechnology == 0 && celestial.Facilities.ResearchLab > 0 && researches.EnergyTechnology < maxEnergyTechnology)
                    return Buildable.EnergyTechnology;
                if (researches.CombustionDrive < 2 && celestial.Facilities.ResearchLab > 0 && researches.EnergyTechnology >= 1 && researches.CombustionDrive < maxCombustionDrive)
                    return Buildable.CombustionDrive;
                if (researches.EspionageTechnology < 4 && celestial.Facilities.ResearchLab >= 3 && researches.EspionageTechnology < maxEspionageTechnology)
                    return Buildable.EspionageTechnology;
                if (researches.ImpulseDrive < 3 && celestial.Facilities.ResearchLab >= 2 && researches.EnergyTechnology >= 1 && researches.ImpulseDrive < maxImpulseDrive)
                    return Buildable.ImpulseDrive;
                if (researches.ComputerTechnology < 1 && celestial.Facilities.ResearchLab >= 1 && researches.ComputerTechnology < maxComputerTechnology)
                    return Buildable.ComputerTechnology;
                if (researches.Astrophysics == 0 && celestial.Facilities.ResearchLab >= 3 && researches.EspionageTechnology >= 4 && researches.ImpulseDrive >= 3 && researches.Astrophysics < maxAstrophysics)
                    return Buildable.Astrophysics;
                if (researches.EnergyTechnology >= 1 && researches.EnergyTechnology < 3 && celestial.Facilities.ResearchLab > 0 && researches.EnergyTechnology < maxEnergyTechnology)
                    return Buildable.EnergyTechnology;
                if (researches.ShieldingTechnology < 2 && celestial.Facilities.ResearchLab > 5 && researches.EnergyTechnology >= 3 && researches.ShieldingTechnology < maxShieldingTechnology)
                    return Buildable.ShieldingTechnology;
                if (researches.CombustionDrive >= 2 && researches.CombustionDrive < 6 && celestial.Facilities.ResearchLab > 0 && researches.EnergyTechnology >= 1 && researches.CombustionDrive < maxComputerTechnology)
                    return Buildable.CombustionDrive;
                if (prioritizeRobotsAndNanitesOnNewPlanets && researches.ComputerTechnology < 10 && celestial.Facilities.ResearchLab >= 1 && researches.ComputerTechnology < maxComputerTechnology)
                    return Buildable.ComputerTechnology;
            }

            if (ensureExpoSlots && slots.ExpTotal + 1 > slots.Total && researches.ComputerTechnology < maxComputerTechnology)
            {
                return Buildable.ComputerTechnology;
            }

            List<Buildable> researchesList = new()
            {
                Buildable.EnergyTechnology,
                Buildable.LaserTechnology,
                Buildable.IonTechnology,
                Buildable.HyperspaceTechnology,
                Buildable.PlasmaTechnology,
                Buildable.CombustionDrive,
                Buildable.ImpulseDrive,
                Buildable.HyperspaceDrive,
                Buildable.EspionageTechnology,
                Buildable.ComputerTechnology,
                Buildable.Astrophysics,
                Buildable.IntergalacticResearchNetwork,
                Buildable.WeaponsTechnology,
                Buildable.ShieldingTechnology,
                Buildable.ArmourTechnology,
                Buildable.GravitonTechnology
            };

            Dictionary<Buildable, ulong> dic = new();
            foreach (Buildable research in researchesList)
            {
                switch (research)
                {
                    case Buildable.EnergyTechnology:
                        if (celestial.Facilities.ResearchLab < 1)
                            continue;
                        if (GetNextLevel(researches, research) > maxEnergyTechnology)
                            continue;
                        break;
                    case Buildable.LaserTechnology:
                        if (celestial.Facilities.ResearchLab < 1)
                            continue;
                        if (researches.EnergyTechnology < 2)
                            continue;
                        if (GetNextLevel(researches, research) > maxLaserTechnology)
                            continue;
                        break;
                    case Buildable.IonTechnology:
                        if (celestial.Facilities.ResearchLab < 4)
                            continue;
                        if (researches.EnergyTechnology < 4 || researches.LaserTechnology < 5)
                            continue;
                        if (GetNextLevel(researches, research) > maxIonTechnology)
                            continue;
                        break;
                    case Buildable.HyperspaceTechnology:
                        if (celestial.Facilities.ResearchLab < 7)
                            continue;
                        if (researches.EnergyTechnology < 5 || researches.ShieldingTechnology < 5)
                            continue;
                        if (GetNextLevel(researches, research) > maxHyperspaceTechnology)
                            continue;
                        break;
                    case Buildable.PlasmaTechnology:
                        if (celestial.Facilities.ResearchLab < 4)
                            continue;
                        if (researches.EnergyTechnology < 8 || researches.LaserTechnology < 10 || researches.IonTechnology < 5)
                            continue;
                        if (GetNextLevel(researches, research) > maxPlasmaTechnology)
                            continue;
                        break;
                    case Buildable.CombustionDrive:
                        if (celestial.Facilities.ResearchLab < 1)
                            continue;
                        if (researches.EnergyTechnology < 1)
                            continue;
                        if (GetNextLevel(researches, research) > maxCombustionDrive)
                            continue;
                        break;
                    case Buildable.ImpulseDrive:
                        if (celestial.Facilities.ResearchLab < 2)
                            continue;
                        if (researches.EnergyTechnology < 1)
                            continue;
                        if (GetNextLevel(researches, research) > maxImpulseDrive)
                            continue;
                        break;
                    case Buildable.HyperspaceDrive:
                        if (celestial.Facilities.ResearchLab < 7)
                            continue;
                        if (researches.HyperspaceTechnology < 3)
                            continue;
                        if (GetNextLevel(researches, research) > maxHyperspaceDrive)
                            continue;
                        break;
                    case Buildable.EspionageTechnology:
                        if (celestial.Facilities.ResearchLab < 3)
                            continue;
                        if (GetNextLevel(researches, research) > maxEspionageTechnology)
                            continue;
                        break;
                    case Buildable.ComputerTechnology:
                        if (celestial.Facilities.ResearchLab < 1)
                            continue;
                        if (GetNextLevel(researches, research) > maxComputerTechnology)
                            continue;
                        break;
                    case Buildable.Astrophysics:
                        if (celestial.Facilities.ResearchLab < 3)
                            continue;
                        if (researches.EspionageTechnology < 4 || researches.ImpulseDrive < 3)
                            continue;
                        if (GetNextLevel(researches, research) > maxAstrophysics)
                            continue;
                        break;
                    case Buildable.IntergalacticResearchNetwork:
                        if (celestial.Facilities.ResearchLab < 10)
                            continue;
                        if (researches.ComputerTechnology < 8 || researches.HyperspaceTechnology < 8)
                            continue;
                        if (GetNextLevel(researches, research) > maxIntergalacticResearchNetwork)
                            continue;
                        break;
                    case Buildable.WeaponsTechnology:
                        if (celestial.Facilities.ResearchLab < 4)
                            continue;
                        if (GetNextLevel(researches, research) > maxWeaponsTechnology)
                            continue;
                        break;
                    case Buildable.ShieldingTechnology:
                        if (celestial.Facilities.ResearchLab < 6)
                            continue;
                        if (researches.EnergyTechnology < 3)
                            continue;
                        if (GetNextLevel(researches, research) > maxShieldingTechnology)
                            continue;
                        break;
                    case Buildable.ArmourTechnology:
                        if (celestial.Facilities.ResearchLab < 2)
                            continue;
                        if (GetNextLevel(researches, research) > maxArmourTechnology)
                            continue;
                        break;
                    case Buildable.GravitonTechnology:
                        if (celestial.Facilities.ResearchLab < 12)
                            continue;
                        if (celestial.ResourcesProduction.Energy.CurrentProduction < CalcPrice(Buildable.GravitonTechnology, GetNextLevel(researches, Buildable.GravitonTechnology)).Energy)
                            continue;
                        break;

                }

                dic.Add(research, CalcPrice(research, GetNextLevel(researches, research)).ConvertedDeuterium);
            }
            if (dic.Count == 0)
                return Buildable.Null;

            dic = dic.OrderBy(m => m.Value)
                .ToDictionary(m => m.Key, m => m.Value);
            return dic.FirstOrDefault().Key;
        }

        public static uint CalcMaxPlanets(uint astrophysics)
        {
            return (uint)Math.Round((astrophysics + 3d) / 2d, 0, MidpointRounding.ToZero);
        }

        public static uint CalcMaxPlanets(Researches researches)
        {
            return researches == null ? 1u : CalcMaxPlanets(researches.Astrophysics);
        }

        public static ulong CalcMaxCrawlers(Planet planet, PlayerClass userClass)
        {
            return 8ul * (planet.Buildings.MetalMine + planet.Buildings.CrystalMine + planet.Buildings.DeuteriumSynthesizer);
        }
    }
}
