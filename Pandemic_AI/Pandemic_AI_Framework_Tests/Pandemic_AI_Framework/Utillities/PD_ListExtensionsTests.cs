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
        public void ShuffleAllSubListsElementsTest()
        {
            List<List<int>> listOfLists = new List<List<int>>();
            List<int> listOfInts1 = new List<int>() { 0, 1, 2, 3 };
            List<int> listOfInts2 = new List<int>() { 4, 5, 6, 7 };
            List<int> listOfInts3 = new List<int>() { 8, 9, 10, 11 };
            listOfLists.Add(listOfInts1);
            listOfLists.Add(listOfInts2);
            listOfLists.Add(listOfInts3);
            listOfLists.ShuffleAllSubListsElements();
        }

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

        [TestMethod()]
        public void GetNonSamePairsTest()
        {
            List<int> originalList = new List<int>() { 1, 2, 3 };
            List<List<int>> pairsList = originalList.GetNonSamePairs();
            List<List<int>> testPairsList = new List<List<int>>() {
                new List<int>(){ 1, 2},
                new List<int>(){ 1, 3},
                new List<int>(){ 2, 1},
                new List<int>(){ 2, 3},
                new List<int>(){ 3, 1},
                new List<int>(){ 3, 2}
            };
            bool allgood = true;

            for (int i = 0; i < pairsList.Count; i++)
            {
                var pair = pairsList[i];
                var testPair = testPairsList[i];
                if (pair[0] != testPair[0]) allgood = false;
                if (pair[1] != testPair[1]) allgood = false;
            }
            Assert.IsTrue(allgood);
        }
    }
}