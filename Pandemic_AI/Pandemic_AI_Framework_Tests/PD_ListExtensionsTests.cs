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
    public class PD_ListExtensionsTests
    {

        [TestMethod()]
        public void GetNumberOfElementsOfAllSubLists_Test1()
        {
            List<List<int>> listOfLists1 = new List<List<int>>()
            {
                new List<int>() { 0, 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 0, 0, 0 }
            };

            Assert.IsTrue(listOfLists1.GetNumberOfElementsOfAllSubLists() == 12);
            Assert.IsTrue(listOfLists1[0].Count == 5);
            Assert.IsTrue(listOfLists1[1].Count == 4);
            Assert.IsTrue(listOfLists1[2].Count == 3);
        }

        [TestMethod()]
        public void GetNumberOfElementsOfAllSubLists_Test2()
        {
            List<List<int>> listOfLists1 = new List<List<int>>()
            {
                new List<int>() { 0, 0, 0, 0, 0 }
            };

            Assert.IsTrue(listOfLists1.GetNumberOfElementsOfAllSubLists() == 5);
            Assert.IsTrue(listOfLists1[0].Count == 5);
        }

    }
}