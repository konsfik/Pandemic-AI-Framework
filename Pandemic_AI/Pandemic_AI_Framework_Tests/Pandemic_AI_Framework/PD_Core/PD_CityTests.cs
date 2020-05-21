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
    public class PD_CityTests
    {
        [TestMethod()]
        public void GetCustomDeepCopyTest()
        {
            PD_City city1 = new PD_City(
                0,
                0,
                "city1Name",
                new PD_Point(0, 0)
                );

            PD_City city1Copy = city1.GetCustomDeepCopy();

            Assert.IsTrue(city1 == city1Copy);
        }

        /// <summary>
        /// Compares a city with itself 
        /// using the Equals() method
        /// </summary>
        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            PD_City city = new PD_City(
                0,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            Assert.IsTrue(
                city.Equals(city)
                );
        }

        /// <summary>
        /// Compares a city with itself 
        /// using "=="
        /// </summary>
        //[TestMethod()]
        //public void ComparisonWithSelf_Test()
        //{
        //    PD_City city = new PD_City(
        //        0,
        //        0,
        //        "city1",
        //        new PD_Point(0, 0)
        //        );

        //    Assert.IsTrue(
        //        city == city
        //        );
        //}

        /// <summary>
        /// Compares a city with another instance which has same properties
        /// using the Equals() method
        /// </summary>
        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            PD_City city1a = new PD_City(
                0,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            PD_City city1b = new PD_City(
                0,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            Assert.IsTrue(
                city1a.Equals(city1b)
                );
        }

        /// <summary>
        /// Compares a city with another instance which has same properties
        /// using "=="
        /// </summary>
        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            PD_City city1a = new PD_City(
                0,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            PD_City city1b = new PD_City(
                0,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            Assert.IsTrue(
                city1a == city1b
                );
        }

        /// <summary>
        /// Compares a city with another instance which has different properties
        /// using the Equals() method
        /// </summary>
        [TestMethod()]
        public void EqualityWithDifferentOther_Test()
        {
            PD_City city1 = new PD_City(
                0,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            PD_City city2 = new PD_City(
                1,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            Assert.IsTrue(
                city1.Equals(city2) == false
                );
        }

        /// <summary>
        /// Compares a city with another instance which has different properties
        /// using the Equals() method
        /// </summary>
        [TestMethod()]
        public void ComparisonWithDifferentOther_Test()
        {
            PD_City city1 = new PD_City(
                0,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            PD_City city2 = new PD_City(
                1,
                0,
                "city1",
                new PD_Point(0, 0)
                );

            Assert.IsTrue(
                city1 != city2
                );
        }

    }
}