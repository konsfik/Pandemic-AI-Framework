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
    public class PD_CityCardTests
    {
        [TestMethod()]
        public void ListContains_Test()
        {
            PD_City city =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard =
                new PD_CityCard(
                    0,
                    city
                    );

            List<PD_CityCard> list = new List<PD_CityCard>();

            list.Add(cityCard);

            Assert.IsTrue(list.Contains(cityCard) == true);
        }

        [TestMethod()]
        public void BaseListContains_Test()
        {
            PD_City city =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard =
                new PD_CityCard(
                    0,
                    city
                    );

            List<PD_Card_Base> baseList = new List<PD_Card_Base>();

            baseList.Add(cityCard);

            Assert.IsTrue(baseList.Contains(cityCard) == true);
        }

        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            PD_City city =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard =
                new PD_CityCard(
                    0,
                    city
                    );

            Assert.IsTrue(cityCard.Equals(cityCard) == true);
        }

        //[TestMethod()]
        //public void ComparisonWithSelf_Test()
        //{
        //    PD_City city =
        //        new PD_City(
        //            0,
        //            0,
        //            "cityName",
        //            new PD_Point(0, 0)
        //            );

        //    PD_CityCard cityCard =
        //        new PD_CityCard(
        //            0,
        //            city
        //            );

        //    Assert.IsTrue(cityCard == cityCard);
        //}

        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            PD_City city1a =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard1a =
                new PD_CityCard(
                    0,
                    city1a
                    );

            PD_City city1b =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard1b =
                new PD_CityCard(
                    0,
                    city1b
                    );

            Assert.IsTrue(cityCard1a.Equals(cityCard1b) == true);
        }

        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            PD_City city1a =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard1a =
                new PD_CityCard(
                    0,
                    city1a
                    );

            PD_City city1b =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard1b =
                new PD_CityCard(
                    0,
                    city1b
                    );

            Assert.IsTrue(cityCard1a == cityCard1b);
        }

        [TestMethod()]
        public void EqualityWithOther_Test()
        {
            PD_City city1 =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard1 =
                new PD_CityCard(
                    0,
                    city1
                    );

            PD_City city2 =
                new PD_City(
                    1,
                    1,
                    "cityName2",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard2 =
                new PD_CityCard(
                    1,
                    city2
                    );

            Assert.IsTrue(cityCard1.Equals(cityCard2) == false);
        }

        [TestMethod()]
        public void ComparisonWithOther_Test()
        {
            PD_City city1 =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard1 =
                new PD_CityCard(
                    0,
                    city1
                    );

            PD_City city2 =
                new PD_City(
                    1,
                    1,
                    "cityName2",
                    new PD_Point(0, 0)
                    );

            PD_CityCard cityCard2 =
                new PD_CityCard(
                    0,
                    city2
                    );

            Assert.IsTrue(cityCard1 != cityCard2);
        }
    }
}