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
    public class PD_PC_MA_A5a_BuildResearchStationTests
    {
        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            PD_Player player = new PD_Player(0, "playerName");
            PD_City city = new PD_City(
                0,0,"cityName",new PD_Point(0,0)
                );
            PD_CityCard cityCard = new PD_CityCard(
                0,city
                );
            PD_PA_BuildResearchStation command =
                new PD_PA_BuildResearchStation(
                    player,
                    cityCard,
                    city
                    );

            Assert.IsTrue(command.Equals(command) == true);
        }

        //[TestMethod()]
        //public void ComparisonWithSelf_Test()
        //{
        //    string data = DataUtilities.ReadGameData("gameCreationData.csv");
        //    PD_Game game = PD_GameCreator.CreateNewGame(4, 0, data, true);
        //    PD_Player player = new PD_Player(0, "playerName");
        //    PD_City city = new PD_City(
        //        0, 0, "cityName", new PD_Point(0, 0)
        //        );
        //    PD_CityCard cityCard = new PD_CityCard(
        //        0, city
        //        );
        //    PD_PA_BuildResearchStation command =
        //        new PD_PA_BuildResearchStation(
        //            player,
        //            cityCard,
        //            city
        //            );

        //    Assert.IsTrue(command == command);
        //}

        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");
            PD_City city1a = new PD_City(
                0, 0, "cityName", new PD_Point(0, 0)
                );
            PD_City city1b = new PD_City(
                0, 0, "cityName", new PD_Point(0, 0)
                );
            PD_CityCard cityCard1a = new PD_CityCard(
                0, city1a
                );
            PD_CityCard cityCard1b = new PD_CityCard(
                0, city1b
                );

            PD_PA_BuildResearchStation command1a =
                new PD_PA_BuildResearchStation(
                    player1a,
                    cityCard1a,
                    city1a
                    );

            PD_PA_BuildResearchStation command1b =
                new PD_PA_BuildResearchStation(
                    player1b,
                    cityCard1b,
                    city1b
                    );

            Assert.IsTrue(command1a.Equals(command1b) == true);
        }

        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");
            PD_City city1a = new PD_City(
                0, 0, "cityName", new PD_Point(0, 0)
                );
            PD_City city1b = new PD_City(
                0, 0, "cityName", new PD_Point(0, 0)
                );
            PD_CityCard cityCard1a = new PD_CityCard(
                0, city1a
                );
            PD_CityCard cityCard1b = new PD_CityCard(
                0, city1b
                );

            PD_PA_BuildResearchStation command1a =
                new PD_PA_BuildResearchStation(
                    player1a,
                    cityCard1a,
                    city1a
                    );

            PD_PA_BuildResearchStation command1b =
                new PD_PA_BuildResearchStation(
                    player1b,
                    cityCard1b,
                    city1b
                    );

            Assert.IsTrue(command1a == command1b);
        }

        [TestMethod()]
        public void EqualityWithOther_Test()
        {
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");
            PD_City city1 = new PD_City(
                0, 0, "cityName1", new PD_Point(0, 0)
                );
            PD_City city2 = new PD_City(
                1, 1, "cityName2", new PD_Point(0, 0)
                );
            PD_CityCard cityCard1 = new PD_CityCard(
                0, city1
                );
            PD_CityCard cityCard2 = new PD_CityCard(
                1, city2
                );

            PD_PA_BuildResearchStation command1 =
                new PD_PA_BuildResearchStation(
                    player1,
                    cityCard1,
                    city1
                    );

            PD_PA_BuildResearchStation command2 =
                new PD_PA_BuildResearchStation(
                    player2,
                    cityCard2,
                    city2
                    );

            Assert.IsTrue(command1.Equals(command2) == false);
        }

        [TestMethod()]
        public void ComparisonWithOther_Test()
        {
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");
            PD_City city1 = new PD_City(
                0, 0, "cityName1", new PD_Point(0, 0)
                );
            PD_City city2 = new PD_City(
                1, 1, "cityName2", new PD_Point(0, 0)
                );
            PD_CityCard cityCard1 = new PD_CityCard(
                0, city1
                );
            PD_CityCard cityCard2 = new PD_CityCard(
                1, city2
                );

            PD_PA_BuildResearchStation command1 =
                new PD_PA_BuildResearchStation(
                    player1,
                    cityCard1,
                    city1
                    );

            PD_PA_BuildResearchStation command2 =
                new PD_PA_BuildResearchStation(
                    player2,
                    cityCard2,
                    city2
                    );

            Assert.IsTrue(command1 != command2);
        }
    }
}