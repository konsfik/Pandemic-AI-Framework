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
        //[TestMethod()]
        //public void GetPrecalculatedShortestDistanceTest1()
        //{
        //    Random randomness_provider = new Random();

        //    PD_Game game = PD_Game.Create(
        //        randomness_provider,
        //        4, 
        //        0, 
        //        true
        //        );
        //    PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();

        //    // get reference to all available cities
        //    var Atlanta = game.Map.cities[0];
        //    var Chicago = game.Map.cities[1];
        //    var Essen = game.Map.cities[2];
        //    var London = game.Map.cities[3];
        //    var Madrid = game.Map.cities[4];
        //    var Milan = game.Map.cities[5];
        //    var Montreal = game.Map.cities[6];
        //    var NewYork = game.Map.cities[7];
        //    var Paris = game.Map.cities[8];
        //    var SanFrancisco = game.Map.cities.Find(x => x.Name == "San Francisco");
        //    var StPetersburg = game.Map.cities.Find(x => x.Name == "St. Petersburg");
        //    var Washington = game.Map.cities.Find(x => x.Name == "Washington");

        //    var Bogota = game.Map.cities.Find(x => x.Name == "Bogota");
        //    var BuenosAires = game.Map.cities.Find(x => x.Name == "Buenos Aires");
        //    var Johannesburg = game.Map.cities.Find(x => x.Name == "Johannesburg");
        //    var Khartoum = game.Map.cities.Find(x => x.Name == "Khartoum");
        //    var Kinshasha = game.Map.cities.Find(x => x.Name == "Kinshasha");
        //    var Lagos = game.Map.cities.Find(x => x.Name == "Lagos");
        //    var Lima = game.Map.cities.Find(x => x.Name == "Lima");
        //    var LosAngeles = game.Map.cities.Find(x => x.Name == "Los Angeles");
        //    var MexicoCity = game.Map.cities.Find(x => x.Name == "Mexico City");
        //    var Miami = game.Map.cities.Find(x => x.Name == "Miami");
        //    var Santiago = game.Map.cities.Find(x => x.Name == "Santiago");
        //    var SaoPaolo = game.Map.cities.Find(x => x.Name == "Sao Paolo");

        //    var Algiers = game.Map.cities.Find(x => x.Name == "Algiers");
        //    var Baghdad = game.Map.cities.Find(x => x.Name == "Baghdad");
        //    var Cairo = game.Map.cities.Find(x => x.Name == "Cairo");
        //    var Chennai = game.Map.cities.Find(x => x.Name == "Chennai");
        //    var Delhi = game.Map.cities.Find(x => x.Name == "Delhi");
        //    var Istanbul = game.Map.cities.Find(x => x.Name == "Istanbul");
        //    var Karachi = game.Map.cities.Find(x => x.Name == "Karachi");
        //    var Kolkata = game.Map.cities.Find(x => x.Name == "Kolkata");
        //    var Moscow = game.Map.cities.Find(x => x.Name == "Moscow");
        //    var Mumbai = game.Map.cities.Find(x => x.Name == "Mumbai");
        //    var Riyadh = game.Map.cities.Find(x => x.Name == "Riyadh");
        //    var Tehran = game.Map.cities.Find(x => x.Name == "Tehran");

        //    var Bangkok = game.Map.cities.Find(x => x.Name == "Bangkok");
        //    var Beijing = game.Map.cities.Find(x => x.Name == "Beijing");
        //    var HiChiMinhCity = game.Map.cities.Find(x => x.Name == "Ho Chi Minh City");
        //    var HongKong = game.Map.cities.Find(x => x.Name == "Hong Kong");
        //    var Jakarta = game.Map.cities.Find(x => x.Name == "Jakarta");
        //    var Manila = game.Map.cities.Find(x => x.Name == "Manila");
        //    var Osaka = game.Map.cities.Find(x => x.Name == "Osaka");
        //    var Seoul = game.Map.cities.Find(x => x.Name == "Seoul");
        //    var Shanghai = game.Map.cities.Find(x => x.Name == "Shanghai");
        //    var Sydney = game.Map.cities.Find(x => x.Name == "Sydney");
        //    var Taipei = game.Map.cities.Find(x => x.Name == "Taipei");
        //    var Tokyo = game.Map.cities.Find(x => x.Name == "Tokyo");


        //    List<int> researchStations = new List<int>();

        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, Washington
        //            ) == 1
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, NewYork
        //            ) == 2
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, London
        //            ) == 3
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, Essen
        //            ) == 4
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, Milan
        //            ) == 5
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, Istanbul
        //            ) == 5
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, Baghdad
        //            ) == 6
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, Karachi
        //            ) == 7
        //        );


        //    researchStations = new List<int>() {
        //        Atlanta,
        //        Paris
        //    };

        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game,
        //            researchStations,
        //            Chicago,
        //            Milan
        //            ) == 3
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Chicago, Algiers
        //            ) == 3
        //        );

        //    researchStations = new List<int>() {
        //        Atlanta,
        //        Paris,
        //        Baghdad
        //    };

        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Chicago, Tehran
        //            ) == 3
        //        );
        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Chicago, Delhi
        //            ) == 4
        //        );

        //    researchStations = new List<int>() {
        //        NewYork,
        //        London,
        //        Paris,
        //        Madrid
        //    };

        //    Assert.IsTrue(
        //        pathFinder.GetPrecalculatedShortestDistance(
        //            game, researchStations,
        //            Atlanta, Kolkata
        //            ) == 5
        //        );
        //}
    }
}