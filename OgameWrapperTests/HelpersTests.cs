using NUnit.Framework;
using OgameWrapper.Includes;
using OgameWrapper.Model;

namespace OgameWrapperTests
{
    public class HelpersTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test_CalcShipCapacity_NoClass()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 0, PlayerClass.NoClass, 0), 5000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 0, PlayerClass.NoClass, 0), 25000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 0, PlayerClass.NoClass, 0), 50);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 0, PlayerClass.NoClass, 0), 100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 0, PlayerClass.NoClass, 0), 800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 0, PlayerClass.NoClass, 0), 1500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 0, PlayerClass.NoClass, 0), 7500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 0, PlayerClass.NoClass, 0), 20000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 0, PlayerClass.NoClass, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 0, PlayerClass.NoClass, 0), 500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 0, PlayerClass.NoClass, 0), 2000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 0, PlayerClass.NoClass, 0), 1000000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 0, PlayerClass.NoClass, 0), 750);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 0, PlayerClass.NoClass, 0), 10000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 0, PlayerClass.NoClass, 0), 10000);
        }

        [Test]
        public void Test_CalcShipCapacity_Collector()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 0, PlayerClass.Collector, 0), 6250);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 0, PlayerClass.Collector, 0), 31250);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 0, PlayerClass.Collector, 0), 50);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 0, PlayerClass.Collector, 0), 100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 0, PlayerClass.Collector, 0), 800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 0, PlayerClass.Collector, 0), 1500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 0, PlayerClass.Collector, 0), 7500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 0, PlayerClass.Collector, 0), 20000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 0, PlayerClass.Collector, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 0, PlayerClass.Collector, 0), 500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 0, PlayerClass.Collector, 0), 2000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 0, PlayerClass.Collector, 0), 1000000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 0, PlayerClass.Collector, 0), 750);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 0, PlayerClass.Collector, 0), 10000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 0, PlayerClass.Collector, 0), 10000);
        }

        [Test]
        public void Test_CalcShipCapacity_General()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 0, PlayerClass.General, 0), 5000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 0, PlayerClass.General, 0), 25000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 0, PlayerClass.General, 0), 50);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 0, PlayerClass.General, 0), 100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 0, PlayerClass.General, 0), 800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 0, PlayerClass.General, 0), 1500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 0, PlayerClass.General, 0), 7500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 0, PlayerClass.General, 0), 24000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 0, PlayerClass.General, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 0, PlayerClass.General, 0), 500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 0, PlayerClass.General, 0), 2000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 0, PlayerClass.General, 0), 1000000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 0, PlayerClass.General, 0), 750);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 0, PlayerClass.General, 0), 10000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 0, PlayerClass.General, 0), 12000);
        }

        [Test]
        public void Test_CalcShipCapacity_Discoverer()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 0, PlayerClass.Discoverer, 0), 5000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 0, PlayerClass.Discoverer, 0), 25000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 0, PlayerClass.Discoverer, 0), 50);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 0, PlayerClass.Discoverer, 0), 100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 0, PlayerClass.Discoverer, 0), 800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 0, PlayerClass.Discoverer, 0), 1500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 0, PlayerClass.Discoverer, 0), 7500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 0, PlayerClass.Discoverer, 0), 20000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 0, PlayerClass.Discoverer, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 0, PlayerClass.Discoverer, 0), 500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 0, PlayerClass.Discoverer, 0), 2000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 0, PlayerClass.Discoverer, 0), 1000000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 0, PlayerClass.Discoverer, 0), 750);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 0, PlayerClass.Discoverer, 0), 10000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 0, PlayerClass.Discoverer, 0), 10000);
        }

        [Test]
        public void Test_CalcShipCapacity_NoClass_HyperspaceLevel8()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 8, PlayerClass.NoClass, 0), 7000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 8, PlayerClass.NoClass, 0), 35000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 8, PlayerClass.NoClass, 0), 70);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 8, PlayerClass.NoClass, 0), 140);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 8, PlayerClass.NoClass, 0), 1120);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 8, PlayerClass.NoClass, 0), 2100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 8, PlayerClass.NoClass, 0), 10500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 8, PlayerClass.NoClass, 0), 28000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 8, PlayerClass.NoClass, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 8, PlayerClass.NoClass, 0), 700);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 8, PlayerClass.NoClass, 0), 2800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 8, PlayerClass.NoClass, 0), 1400000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 8, PlayerClass.NoClass, 0), 1050);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 8, PlayerClass.NoClass, 0), 14000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 8, PlayerClass.NoClass, 0), 14000);
        }

        [Test]
        public void Test_CalcShipCapacity_Collector_HyperspaceLevel8()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 8, PlayerClass.Collector, 0), 8250);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 8, PlayerClass.Collector, 0), 41250);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 8, PlayerClass.Collector, 0), 70);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 8, PlayerClass.Collector, 0), 140);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 8, PlayerClass.Collector, 0), 1120);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 8, PlayerClass.Collector, 0), 2100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 8, PlayerClass.Collector, 0), 10500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 8, PlayerClass.Collector, 0), 28000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 8, PlayerClass.Collector, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 8, PlayerClass.Collector, 0), 700);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 8, PlayerClass.Collector, 0), 2800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 8, PlayerClass.Collector, 0), 1400000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 8, PlayerClass.Collector, 0), 1050);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 8, PlayerClass.Collector, 0), 14000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 8, PlayerClass.Collector, 0), 14000);
        }

        [Test]
        public void Test_CalcShipCapacity_General_HyperspaceLevel8()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 8, PlayerClass.General, 0), 7000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 8, PlayerClass.General, 0), 35000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 8, PlayerClass.General, 0), 70);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 8, PlayerClass.General, 0), 140);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 8, PlayerClass.General, 0), 1120);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 8, PlayerClass.General, 0), 2100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 8, PlayerClass.General, 0), 10500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 8, PlayerClass.General, 0), 32000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 8, PlayerClass.General, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 8, PlayerClass.General, 0), 700);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 8, PlayerClass.General, 0), 2800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 8, PlayerClass.General, 0), 1400000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 8, PlayerClass.General, 0), 1050);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 8, PlayerClass.General, 0), 14000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 8, PlayerClass.General, 0), 16000);
        }

        [Test]
        public void Test_CalcShipCapacity_Discoverer_HyperspaceLevel8()
        {
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.SmallCargo, 8, PlayerClass.Discoverer, 0), 7000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LargeCargo, 8, PlayerClass.Discoverer, 0), 35000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.LightFighter, 8, PlayerClass.Discoverer, 0), 70);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.HeavyFighter, 8, PlayerClass.Discoverer, 0), 140);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Cruiser, 8, PlayerClass.Discoverer, 0), 1120);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battleship, 8, PlayerClass.Discoverer, 0), 2100);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.ColonyShip, 8, PlayerClass.Discoverer, 0), 10500);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Recycler, 8, PlayerClass.Discoverer, 0), 28000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.EspionageProbe, 8, PlayerClass.Discoverer, 0), 0);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Bomber, 8, PlayerClass.Discoverer, 0), 700);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Destroyer, 8, PlayerClass.Discoverer, 0), 2800);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Deathstar, 8, PlayerClass.Discoverer, 0), 1400000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Battlecruiser, 8, PlayerClass.Discoverer, 0), 1050);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Reaper, 8, PlayerClass.Discoverer, 0), 14000);
            Assert.AreEqual(Helpers.CalcShipCapacity(Buildable.Pathfinder, 8, PlayerClass.Discoverer, 0), 14000);
        }
    }
}
