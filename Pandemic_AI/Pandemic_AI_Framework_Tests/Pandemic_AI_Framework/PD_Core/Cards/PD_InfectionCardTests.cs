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
    public class PD_InfectionCardTests
    {
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

            PD_InfectionCard infectionCard =
                new PD_InfectionCard(
                    0,
                    city
                    );

            Assert.IsTrue(infectionCard.Equals(infectionCard) == true);
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

        //    PD_InfectionCard infectionCard =
        //        new PD_InfectionCard(
        //            0,
        //            city
        //            );

        //    Assert.IsTrue(infectionCard == infectionCard);
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

            PD_InfectionCard infectionCard1a =
                new PD_InfectionCard(
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

            PD_InfectionCard infectionCard1b =
                new PD_InfectionCard(
                    0,
                    city1b
                    );

            Assert.IsTrue(infectionCard1a.Equals(infectionCard1b) == true);
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

            PD_InfectionCard infectionCard1a =
                new PD_InfectionCard(
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

            PD_InfectionCard infectionCard1b =
                new PD_InfectionCard(
                    0,
                    city1b
                    );

            Assert.IsTrue(infectionCard1a == infectionCard1b);
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

            PD_InfectionCard infectionCard1 =
                new PD_InfectionCard(
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

            PD_InfectionCard infectionCard2 =
                new PD_InfectionCard(
                    1,
                    city2
                    );

            Assert.IsTrue(infectionCard1.Equals(infectionCard2) == false);
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

            PD_InfectionCard infectionCard1 =
                new PD_InfectionCard(
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

            PD_InfectionCard infectionCard2 =
                new PD_InfectionCard(
                    0,
                    city2
                    );

            Assert.IsTrue(infectionCard1 != infectionCard2);
        }
    }
}