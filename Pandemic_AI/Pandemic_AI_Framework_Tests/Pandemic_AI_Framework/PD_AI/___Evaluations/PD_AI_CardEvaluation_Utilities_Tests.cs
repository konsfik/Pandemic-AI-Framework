﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic_AI_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework.Tests
{
    [TestClass()]
    public class PD_AI_CardEvaluation_Utilities_Tests
    {
        [TestMethod()]
        public void Calculate_NumCardsTable_Test()
        {
            Random randomness_provider = new Random();

            //string data = DataUtilities.ReadGameData("gameCreationData.csv");
            PD_Game game = PD_Game.Create(
                randomness_provider,
                4, 
                0, 
                true
                );
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();

            int[,] numCardsTable = PD_AI_CardEvaluation_Utilities.NumCardsTable(game);
            int numTypes = numCardsTable.Height();
            int numPlayers = numCardsTable.Width();
            for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
            {
                for (int type = 0; type < numTypes; type++)
                {
                    int numCards = numCardsTable[type, playerIndex];

                    int player = game.players[playerIndex];
                    List<int> playerCards = game.GQ_CityCardsInPlayerHand(player);
                    List<int> playerCardsOfThisType = playerCards.FindAll(
                        x => 
                        game.Map.infection_type__per__city[x] == type
                        );

                    Assert.IsTrue(
                        playerCardsOfThisType.Count == numCards
                        );
                }
            }
        }
    }
}