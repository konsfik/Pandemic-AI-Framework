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
    public class PD_AI_CardEvaluation_Utilities_Tests
    {
        [TestMethod()]
        public void Calculate_NumCardsTable_Test()
        {
            string data = DataUtilities.Read_GameCreationData();

            //string data = DataUtilities.ReadGameData("gameCreationData.csv");
            PD_Game game = PD_GameCreator.CreateNewGame(4, 0, data, true);
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder(game);

            int[,] numCardsTable = PD_AI_CardEvaluation_Utilities.NumCardsTable(game);
            int numTypes = numCardsTable.Height();
            int numPlayers = numCardsTable.Width();
            for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
            {
                for (int type = 0; type < numTypes; type++)
                {
                    int numCards = numCardsTable[type, playerIndex];

                    PD_Player player = game.Players[playerIndex];
                    List<PD_CityCard> playerCards = PD_Game_Queries.GQ_Find_CityCardsInPlayerHand(game, player);
                    List<PD_CityCard> playerCardsOfThisType = playerCards.FindAll(
                        x => 
                        x.Type == type
                        );

                    Assert.IsTrue(
                        playerCardsOfThisType.Count == numCards
                        );
                }
            }
        }
    }
}