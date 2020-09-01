using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic_AI_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework.Tests
{
    [TestClass()]
    public class PD_AI_PathFinderTests
    {
        [TestMethod()]
        public void GetPrecalculatedShortestDistanceTest1()
        {
            Random randomness_provider = new Random();

            PD_Game game = PD_Game.Create(
                randomness_provider,
                4, 
                0, 
                true
                );
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();

            // get reference to all available cities
            var Atlanta = game.Map.Cities.Find(x => x.Name == "Atlanta");
            var Chicago = game.Map.Cities.Find(x => x.Name == "Chicago");
            var Essen = game.Map.Cities.Find(x => x.Name == "Essen");
            var London = game.Map.Cities.Find(x => x.Name == "London");
            var Madrid = game.Map.Cities.Find(x => x.Name == "Madrid");
            var Milan = game.Map.Cities.Find(x => x.Name == "Milan");
            var Montreal = game.Map.Cities.Find(x => x.Name == "Montreal");
            var NewYork = game.Map.Cities.Find(x => x.Name == "New York");
            var Paris = game.Map.Cities.Find(x => x.Name == "Paris");
            var SanFrancisco = game.Map.Cities.Find(x => x.Name == "San Francisco");
            var StPetersburg = game.Map.Cities.Find(x => x.Name == "St. Petersburg");
            var Washington = game.Map.Cities.Find(x => x.Name == "Washington");

            var Bogota = game.Map.Cities.Find(x => x.Name == "Bogota");
            var BuenosAires = game.Map.Cities.Find(x => x.Name == "Buenos Aires");
            var Johannesburg = game.Map.Cities.Find(x => x.Name == "Johannesburg");
            var Khartoum = game.Map.Cities.Find(x => x.Name == "Khartoum");
            var Kinshasha = game.Map.Cities.Find(x => x.Name == "Kinshasha");
            var Lagos = game.Map.Cities.Find(x => x.Name == "Lagos");
            var Lima = game.Map.Cities.Find(x => x.Name == "Lima");
            var LosAngeles = game.Map.Cities.Find(x => x.Name == "Los Angeles");
            var MexicoCity = game.Map.Cities.Find(x => x.Name == "Mexico City");
            var Miami = game.Map.Cities.Find(x => x.Name == "Miami");
            var Santiago = game.Map.Cities.Find(x => x.Name == "Santiago");
            var SaoPaolo = game.Map.Cities.Find(x => x.Name == "Sao Paolo");

            var Algiers = game.Map.Cities.Find(x => x.Name == "Algiers");
            var Baghdad = game.Map.Cities.Find(x => x.Name == "Baghdad");
            var Cairo = game.Map.Cities.Find(x => x.Name == "Cairo");
            var Chennai = game.Map.Cities.Find(x => x.Name == "Chennai");
            var Delhi = game.Map.Cities.Find(x => x.Name == "Delhi");
            var Istanbul = game.Map.Cities.Find(x => x.Name == "Istanbul");
            var Karachi = game.Map.Cities.Find(x => x.Name == "Karachi");
            var Kolkata = game.Map.Cities.Find(x => x.Name == "Kolkata");
            var Moscow = game.Map.Cities.Find(x => x.Name == "Moscow");
            var Mumbai = game.Map.Cities.Find(x => x.Name == "Mumbai");
            var Riyadh = game.Map.Cities.Find(x => x.Name == "Riyadh");
            var Tehran = game.Map.Cities.Find(x => x.Name == "Tehran");

            var Bangkok = game.Map.Cities.Find(x => x.Name == "Bangkok");
            var Beijing = game.Map.Cities.Find(x => x.Name == "Beijing");
            var HiChiMinhCity = game.Map.Cities.Find(x => x.Name == "Ho Chi Minh City");
            var HongKong = game.Map.Cities.Find(x => x.Name == "Hong Kong");
            var Jakarta = game.Map.Cities.Find(x => x.Name == "Jakarta");
            var Manila = game.Map.Cities.Find(x => x.Name == "Manila");
            var Osaka = game.Map.Cities.Find(x => x.Name == "Osaka");
            var Seoul = game.Map.Cities.Find(x => x.Name == "Seoul");
            var Shanghai = game.Map.Cities.Find(x => x.Name == "Shanghai");
            var Sydney = game.Map.Cities.Find(x => x.Name == "Sydney");
            var Taipei = game.Map.Cities.Find(x => x.Name == "Taipei");
            var Tokyo = game.Map.Cities.Find(x => x.Name == "Tokyo");


            List<PD_City> researchStations = new List<PD_City>();

            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, Washington
                    ) == 1
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, NewYork
                    ) == 2
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, London
                    ) == 3
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, Essen
                    ) == 4
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, Milan
                    ) == 5
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, Istanbul
                    ) == 5
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, Baghdad
                    ) == 6
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, Karachi
                    ) == 7
                );


            researchStations = new List<PD_City>() {
                Atlanta,
                Paris
            };

            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game,
                    researchStations,
                    Chicago,
                    Milan
                    ) == 3
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Chicago, Algiers
                    ) == 3
                );

            researchStations = new List<PD_City>() {
                Atlanta,
                Paris,
                Baghdad
            };

            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Chicago, Tehran
                    ) == 3
                );
            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Chicago, Delhi
                    ) == 4
                );

            researchStations = new List<PD_City>() {
                NewYork,
                London,
                Paris,
                Madrid
            };

            Assert.IsTrue(
                pathFinder.GetPrecalculatedShortestDistance(
                    game, researchStations,
                    Atlanta, Kolkata
                    ) == 5
                );
        }
    }
}