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
    public class PD_PlayerTests
    {
        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            PD_Player player = new PD_Player(0, "playerName");

            Assert.IsTrue(player.Equals(player) == true);
        }

        //[TestMethod()]
        //public void ComparisonWithSelf_Test()
        //{
        //    PD_Player player = new PD_Player(0, "playerName");

        //    Assert.IsTrue(player == player);
        //}

        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");

            Assert.IsTrue(player1a.Equals(player1b) == true);
        }

        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");

            Assert.IsTrue(player1a == player1b);
        }

        [TestMethod()]
        public void EqualityWithOther_Test()
        {
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");

            Assert.IsTrue(player1.Equals(player2) == false);
        }

        [TestMethod()]
        public void ComparisonWithOther_Test()
        {
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");

            Assert.IsTrue(player1 != player2);
        }
    }
}