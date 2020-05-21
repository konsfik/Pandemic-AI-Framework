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
    public class PD_Card_BaseTests
    {
        [TestMethod()]
        public void TestEqualityBetweenSameCityCard() {

        }

        [TestMethod()]
        public void TestEqualityForVariousSubTypes()
        {
            // test city card
            PD_City city1a =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_City city1b =
                new PD_City(
                    0,
                    0,
                    "cityName",
                    new PD_Point(0, 0)
                    );

            PD_City city2 =
                new PD_City(
                    1,
                    0,
                    "cityName2",
                    new PD_Point(1, 1)
                    );

            PD_CityCard cityCard1a =
                new PD_CityCard(
                    0,
                    city1a
                    );

            PD_CityCard cityCard1b =
                new PD_CityCard(
                    0,
                    city1b
                    );

            PD_CityCard cityCard2 =
                new PD_CityCard(
                    0,
                    city2
                    );

            // compare a card to itself
            Assert.IsTrue(
                cityCard1a.Equals(cityCard1a) == true
                );
            //Assert.IsTrue(
            //    cityCard1a == cityCard1a
            //    );

            // compare a card to its same other
            Assert.IsTrue(
                cityCard1a.Equals(cityCard1b) == true
                );
            Assert.IsTrue(
                cityCard1a == cityCard1b
                );

            // compare two city cards that are not equal
            Assert.IsTrue(
                cityCard1a.Equals(cityCard2) == false
                );
            Assert.IsTrue(
                cityCard1a != cityCard2
                );

        }

        //[TestMethod()]
        //public void PD_Card_BaseTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetDescriptionTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void EqualsTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void EqualsTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetHashCodeTest()
        //{
        //    Assert.Fail();
        //}
    }
}