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
    public class PD_CityEdgeTests
    {

        [TestMethod()]
        public void ContainsCityIDTest()
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
                "city2",
                new PD_Point(0, 0)
                );

            PD_CityEdge edge = new PD_CityEdge(city1, city2);
            Assert.IsTrue(edge.ContainsCity(city1));
            Assert.IsTrue(edge.ContainsCity(city2));
        }

        #region equalityTests

        [TestMethod()]
        public void EqualsTest()
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
                "city2",
                new PD_Point(0, 0)
                );

            PD_City city3 = new PD_City(
                3,
                0,
                "city3",
                new PD_Point(0, 0)
                );

            PD_CityEdge edge1 = new PD_CityEdge(city1, city2);
            PD_CityEdge edge2 = new PD_CityEdge(city1, city3);
            PD_CityEdge edge3 = new PD_CityEdge(city1, city2);
            Assert.IsTrue(edge1.Equals(edge3) == true);
            Assert.IsTrue(edge1.Equals(edge2) == false);
        }

        #endregion
    }
}