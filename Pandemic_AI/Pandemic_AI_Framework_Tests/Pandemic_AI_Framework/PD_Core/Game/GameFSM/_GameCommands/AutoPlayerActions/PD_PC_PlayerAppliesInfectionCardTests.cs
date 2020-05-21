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
    public class PD_PC_PlayerAppliesInfectionCardTests
    {
        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            string data = DataUtilities.Read_GameCreationData();
            PD_Game game = PD_GameCreator.CreateNewGame(4, 0, data, true);
            PD_Player player = new PD_Player(0, "playerName");
            PD_InfectionCard infectionCard = game.GameElementReferences.InfectionCards[0];

            PD_ApplyInfectionCard command =
                new PD_ApplyInfectionCard(
                    player,
                    infectionCard
                    );

            Assert.IsTrue(command.Equals(command) == true);
        }

        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            string data = DataUtilities.Read_GameCreationData();
            PD_Game game = PD_GameCreator.CreateNewGame(4, 0, data, true);
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");
            PD_InfectionCard infectionCard1a = game.GameElementReferences.InfectionCards[0];
            PD_InfectionCard infectionCard1b = game.GameElementReferences.InfectionCards[0];

            PD_ApplyInfectionCard command1a =
                new PD_ApplyInfectionCard(
                    player1a,
                    infectionCard1a
                    );

            PD_ApplyInfectionCard command1b =
                new PD_ApplyInfectionCard(
                    player1b,
                    infectionCard1b
                    );

            Assert.IsTrue(command1a.Equals(command1b) == true);
        }

        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            string data = DataUtilities.Read_GameCreationData();
            PD_Game game = PD_GameCreator.CreateNewGame(4, 0, data, true);
            PD_Player player1a = new PD_Player(0, "playerName");
            PD_Player player1b = new PD_Player(0, "playerName");
            PD_InfectionCard infectionCard1a = game.GameElementReferences.InfectionCards[0];
            PD_InfectionCard infectionCard1b = game.GameElementReferences.InfectionCards[0];

            PD_ApplyInfectionCard command1a =
                new PD_ApplyInfectionCard(
                    player1a,
                    infectionCard1a
                    );

            PD_ApplyInfectionCard command1b =
                new PD_ApplyInfectionCard(
                    player1b,
                    infectionCard1b
                    );

            Assert.IsTrue(command1a == command1b);
        }

        [TestMethod()]
        public void EqualityWithOther_Test()
        {
            string data = DataUtilities.Read_GameCreationData();
            PD_Game game = PD_GameCreator.CreateNewGame(4, 0, data, true);
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");
            PD_InfectionCard infectionCard1 = game.GameElementReferences.InfectionCards[0];
            PD_InfectionCard infectionCard2 = game.GameElementReferences.InfectionCards[1];

            PD_ApplyInfectionCard command1 =
                new PD_ApplyInfectionCard(
                    player1,
                    infectionCard1
                    );

            PD_ApplyInfectionCard command2 =
                new PD_ApplyInfectionCard(
                    player2,
                    infectionCard2
                    );

            Assert.IsTrue(command1.Equals(command2) == false);
        }

        [TestMethod()]
        public void ComparisonWithOther_Test()
        {
            string data = DataUtilities.Read_GameCreationData();
            PD_Game game = PD_GameCreator.CreateNewGame(4, 0, data, true);
            PD_Player player1 = new PD_Player(0, "playerName1");
            PD_Player player2 = new PD_Player(1, "playerName2");
            PD_InfectionCard infectionCard1 = game.GameElementReferences.InfectionCards[0];
            PD_InfectionCard infectionCard2 = game.GameElementReferences.InfectionCards[1];

            PD_ApplyInfectionCard command1 =
                new PD_ApplyInfectionCard(
                    player1,
                    infectionCard1
                    );

            PD_ApplyInfectionCard command2 =
                new PD_ApplyInfectionCard(
                    player2,
                    infectionCard2
                    );

            Assert.IsTrue(command1 != command2);
        }
    }
}