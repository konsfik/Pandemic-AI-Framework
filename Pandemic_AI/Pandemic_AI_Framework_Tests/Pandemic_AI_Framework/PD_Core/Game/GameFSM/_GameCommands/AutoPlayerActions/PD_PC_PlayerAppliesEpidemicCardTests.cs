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
    public class PD_PC_PlayerAppliesEpidemicCardTests
    {
        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            PD_Player player = new PD_Player(0, "playerName");

            PD_ApplyEpidemicCard command =
                new PD_ApplyEpidemicCard(
                    player
                    );

            Assert.IsTrue(command.Equals(command) == true);
        }

        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");

            PD_ApplyEpidemicCard command1a =
                new PD_ApplyEpidemicCard(
                    player1a
                    );

            PD_ApplyEpidemicCard command1b =
                new PD_ApplyEpidemicCard(
                    player1b
                    );

            Assert.IsTrue(command1a.Equals(command1b) == true);
        }

        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");

            PD_ApplyEpidemicCard command1a =
                new PD_ApplyEpidemicCard(
                    player1a
                    );

            PD_ApplyEpidemicCard command1b =
                new PD_ApplyEpidemicCard(
                    player1b
                    );

            Assert.IsTrue(command1a == command1b);
        }

        [TestMethod()]
        public void EqualityWithOther_Test()
        {
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");

            PD_ApplyEpidemicCard command1 =
                new PD_ApplyEpidemicCard(
                    player1
                    );

            PD_ApplyEpidemicCard command2 =
                new PD_ApplyEpidemicCard(
                    player2
                    );

            Assert.IsTrue(command1.Equals(command2) == false);
        }

        [TestMethod()]
        public void ComparisonWithOther_Test()
        {
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");

            PD_ApplyEpidemicCard command1 =
                new PD_ApplyEpidemicCard(
                    player1
                    );

            PD_ApplyEpidemicCard command2 =
                new PD_ApplyEpidemicCard(
                    player2
                    );

            Assert.IsTrue(command1 != command2);
        }
    }
}