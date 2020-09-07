using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_AI_CardEvaluation_Utilities
    {
        #region num cards table
        // these methods are the very basic ones
        // for calculating all the evaluations.
        // they just count the number of cards that are in the hands of each player.

        /// <summary>
        /// Returns a table that just ccounts the number of cards in the players hands, per player and per type.
        /// The table is a 2D table, where rows are of the same type, and columns are of the same player.
        /// Info can be accessed as: 
        /// numCards = numCardsTable[typeIndex, playerIndex]
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static int[,] NumCardsTable(
            PD_Game game
            )
        {
            int numPlayers = game.Players.Count;
            int numTypes = 4;

            int[,] numCardsTable = new int[numTypes, numPlayers];

            for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
            {
                var player = game.Players[playerIndex];
                var cityCardsInPlayerHand =
                    game.GQ_CityCardsInPlayerHand(
                        player
                        );
                for (int typeIndex = 0; typeIndex < numTypes; typeIndex++)
                {
                    int numCards_ThisType = cityCardsInPlayerHand.FindAll(
                        x =>
                        game.Map.infection_type__per__city[x.City] == typeIndex
                        ).Count;

                    numCardsTable[typeIndex, playerIndex] = numCards_ThisType;
                }
            }

            return numCardsTable;
        }


        #endregion

        #region percent complete sets of cards
        public static double[,] Calculate_Percent_CompleteSetsOfCards_Table(
            PD_Game game,
            int[,] numCardsTable
            )
        {
            int numTypes = numCardsTable.Height();
            int numPlayers = numCardsTable.Width();

            double[,] percent_CompleteSetsOfCards_Table = new double[numTypes, numPlayers];

            for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
            {
                for (int typeIndex = 0; typeIndex < numTypes; typeIndex++)
                {
                    PD_Player player = game.Players[playerIndex];
                    bool isPlayerScientist = game.GQ_Find_Player_Role(player) == PD_Player_Roles.Scientist;

                    int numCards = numCardsTable[typeIndex, playerIndex];
                    int numCards_SetComplete = isPlayerScientist ? 4 : 5;

                    double percent_SetCompleteness = (double)numCards / (double)numCards_SetComplete;
                    if (percent_SetCompleteness > 1.0)
                    {
                        percent_SetCompleteness = 1.0;
                    }

                    percent_CompleteSetsOfCards_Table[typeIndex, playerIndex] = percent_SetCompleteness;
                }
            }

            return percent_CompleteSetsOfCards_Table;
        }

        #endregion

        #region group ability to cure diseases Table
        public static double[,] Calculate_Percent_AbilityToCureDiseases_Table(
            PD_Game game,
            double[,] percentCompleteSetsOfCards_Table,
            bool squared
            )
        {
            double[,] groupAbilityToCureDiseases_Table = percentCompleteSetsOfCards_Table.CustomDeepCopy();

            int numTypes = groupAbilityToCureDiseases_Table.Height();
            int numPlayers = groupAbilityToCureDiseases_Table.Width();

            for (int type = 0; type < numTypes; type++)
            {
                bool isDiseaseCured = game.GQ_Is_DiseaseCured_OR_Eradicated(type);
                if (isDiseaseCured)
                {
                    for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
                    {
                        groupAbilityToCureDiseases_Table[type, playerIndex] = 1;
                    }
                }
            }

            if (squared)
            {
                groupAbilityToCureDiseases_Table = groupAbilityToCureDiseases_Table.Squared();
            }

            return groupAbilityToCureDiseases_Table;
        }
        #endregion

        public static double Calculate_PercentCuredDiseases_Gradient(
            PD_Game game,
            bool squared
            )
        {
            int[,] numCardsTable = NumCardsTable(
                game
                );

            double[,] percentComplete_Table = Calculate_Percent_CompleteSetsOfCards_Table(game, numCardsTable);

            double[,] groupAbilityToCureDiseases_Table = Calculate_Percent_AbilityToCureDiseases_Table(
                game,
                percentComplete_Table,
                squared
                );

            double[,] finalTable = groupAbilityToCureDiseases_Table.CustomDeepCopy();

            int numTypes = groupAbilityToCureDiseases_Table.Height();
            int numPlayers = groupAbilityToCureDiseases_Table.Width();

            for (int type = 0; type < numTypes; type++)
            {
                bool isDiseaseCured = game.GQ_Is_DiseaseCured_OR_Eradicated(type);
                if (isDiseaseCured)
                {
                    for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
                    {
                        finalTable[type, playerIndex] = 1.3;
                    }
                }
                else
                {
                    for (int playerIndex = 0; playerIndex < numPlayers; playerIndex++)
                    {
                        finalTable[type, playerIndex] = groupAbilityToCureDiseases_Table[type, playerIndex];
                    }
                }
            }

            double[,] normalized_Table = finalTable.Divided_By(1.3);

            if (squared)
            {
                normalized_Table = normalized_Table.Squared();
            }

            return
                normalized_Table
                .RowMax_PerRow()
                .Average();
        }

        #region GroupAbilityToCureDiseases
        public static double Calculate_Percent_AbilityToCureDiseases(
            PD_Game game,
            double[,] groupAbilityToCureDiseases_Table
            )
        {
            return
                groupAbilityToCureDiseases_Table
                .RowMax_PerRow()
                .Average();
        }

        public static double Calculate_Percent_AbilityToCureDiseases(
            PD_Game game,
            bool squared
            )
        {
            int[,] numCardsTable = NumCardsTable(
                game
                );
            double[,] percentComplete_SetsOfCards_Table = Calculate_Percent_CompleteSetsOfCards_Table(
                game,
                numCardsTable
                );
            double[,] groupAbilityToCureDiseases_Table = Calculate_Percent_AbilityToCureDiseases_Table(
                game,
                percentComplete_SetsOfCards_Table,
                squared
                );
            return
                groupAbilityToCureDiseases_Table
                .RowMax_PerRow()
                .Average();
        }
        #endregion

        #region effect of player actions
        public static int[,] NumCardsTable_AfterApplying_PlayerAction(
            PD_Game game,
            int[,] initialNumCardsTable,
            PD_GameAction_Base action
            )
        {
            int[,] hypothetical_NumCardsTable = initialNumCardsTable.CustomDeepCopy();
            if (action.GetType() == typeof(PD_PA_ShareKnowledge_GiveCard))
            {
                PD_PA_ShareKnowledge_GiveCard shareKnowledge_Give_Action = (PD_PA_ShareKnowledge_GiveCard)action;
                int cardType = game.GQ_City_InfectionType(shareKnowledge_Give_Action.CityCardToGive.City);
                int giver_Index = game.Players.IndexOf(shareKnowledge_Give_Action.Player);
                int taker_Index = game.Players.IndexOf(shareKnowledge_Give_Action.OtherPlayer);

                hypothetical_NumCardsTable[cardType, giver_Index] -= 1;
                hypothetical_NumCardsTable[cardType, taker_Index] += 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PA_ShareKnowledge_GiveCard_ResearcherGives))
            {
                PD_PA_ShareKnowledge_GiveCard_ResearcherGives shareKnowledge_Give_Action =
                    (PD_PA_ShareKnowledge_GiveCard_ResearcherGives)action;
                int cardType = game.GQ_City_InfectionType(shareKnowledge_Give_Action.CityCardToGive.City);
                int giver_Index = game.Players.IndexOf(shareKnowledge_Give_Action.Player);
                int taker_Index = game.Players.IndexOf(shareKnowledge_Give_Action.OtherPlayer);

                hypothetical_NumCardsTable[cardType, giver_Index] -= 1;
                hypothetical_NumCardsTable[cardType, taker_Index] += 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PA_ShareKnowledge_TakeCard))
            {
                PD_PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = (PD_PA_ShareKnowledge_TakeCard)action;
                int cardType = game.GQ_City_InfectionType(shareKnowledge_Take_Action.CityCardToTake.City);
                int taker_Index = game.Players.IndexOf(shareKnowledge_Take_Action.Player);
                int giver_Index = game.Players.IndexOf(shareKnowledge_Take_Action.OtherPlayer);

                hypothetical_NumCardsTable[cardType, giver_Index] -= 1;
                hypothetical_NumCardsTable[cardType, taker_Index] += 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PA_ShareKnowledge_TakeCard_FromResearcher))
            {
                PD_PA_ShareKnowledge_TakeCard_FromResearcher shareKnowledge_Take_Action =
                    (PD_PA_ShareKnowledge_TakeCard_FromResearcher)action;
                int cardType = game.GQ_City_InfectionType(shareKnowledge_Take_Action.CityCardToTake.City);
                int taker_Index = game.Players.IndexOf(shareKnowledge_Take_Action.Player);
                int giver_Index = game.Players.IndexOf(shareKnowledge_Take_Action.OtherPlayer);

                hypothetical_NumCardsTable[cardType, giver_Index] -= 1;
                hypothetical_NumCardsTable[cardType, taker_Index] += 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PMA_DirectFlight))
            {
                PD_PMA_DirectFlight directFlightAction =
                    (PD_PMA_DirectFlight)action;
                int cardType = game.GQ_City_InfectionType(directFlightAction.CityCardToDiscard.City);
                int playerIndex = game.Players.IndexOf(directFlightAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PMA_CharterFlight))
            {
                PD_PMA_CharterFlight charterFlightAction =
                    (PD_PMA_CharterFlight)action;
                int cardType = game.GQ_City_InfectionType(charterFlightAction.CityCardToDiscard.City);
                int playerIndex = game.Players.IndexOf(charterFlightAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PMA_OperationsExpert_Flight))
            {
                PD_PMA_OperationsExpert_Flight operationsExpertFlightAction =
                    (PD_PMA_OperationsExpert_Flight)action;
                int cardType = game.GQ_City_InfectionType(operationsExpertFlightAction.CityCardToDiscard.City);
                int playerIndex = game.Players.IndexOf(operationsExpertFlightAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PMA_OperationsExpert_Flight))
            {
                PD_PMA_OperationsExpert_Flight operationsExpertFlightAction =
                    (PD_PMA_OperationsExpert_Flight)action;
                int cardType = game.GQ_City_InfectionType(operationsExpertFlightAction.CityCardToDiscard.City);
                int playerIndex = game.Players.IndexOf(operationsExpertFlightAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PA_BuildResearchStation))
            {
                PD_PA_BuildResearchStation buildResearchStationAction =
                    (PD_PA_BuildResearchStation)action;
                int cardType = game.GQ_City_InfectionType(buildResearchStationAction.Used_CityCard.City);
                int playerIndex = game.Players.IndexOf(buildResearchStationAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PA_MoveResearchStation))
            {
                PD_PA_MoveResearchStation moveResearchStationAction =
                    (PD_PA_MoveResearchStation)action;
                int cardType = game.GQ_City_InfectionType(moveResearchStationAction.Used_CityCard.City);
                int playerIndex = game.Players.IndexOf(moveResearchStationAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PA_Discard_DuringMainPlayerActions))
            {
                PD_PA_Discard_DuringMainPlayerActions discardAction =
                    (PD_PA_Discard_DuringMainPlayerActions)action;
                int cardType = game.GQ_City_InfectionType(((PD_CityCard)discardAction.PlayerCardToDiscard).City);
                int playerIndex = game.Players.IndexOf(discardAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else if (action.GetType() == typeof(PD_PA_Discard_AfterDrawing))
            {
                PD_PA_Discard_AfterDrawing discardAction =
                    (PD_PA_Discard_AfterDrawing)action;
                int cardType = game.GQ_City_InfectionType(((PD_CityCard)discardAction.PlayerCardToDiscard).City);
                int playerIndex = game.Players.IndexOf(discardAction.Player);
                hypothetical_NumCardsTable[cardType, playerIndex] -= 1;

                return hypothetical_NumCardsTable;
            }
            else
            {
                return hypothetical_NumCardsTable;
            }
        }

        public static int[,] NumCardsTable_AfterApplying_ListOfActions(
            PD_Game game,
            int[,] initialNumCardsTable,
            List<PD_GameAction_Base> actions
            )
        {
            int[,] result = initialNumCardsTable.CustomDeepCopy();
            foreach (var action in actions)
            {
                result = NumCardsTable_AfterApplying_PlayerAction(
                    game,
                    result,
                    action
                    );
            }
            return result;
        }

        public static double Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
            PD_Game game,
            PD_GameAction_Base action,
            bool squared
            )
        {
            int[,] current_NumCardsTable = NumCardsTable(game);
            double[,] current_Percent_Complete_SetsOfCards = Calculate_Percent_CompleteSetsOfCards_Table(
                game,
                current_NumCardsTable
                );
            double[,] current_GroupAbilityToCureDiseases_Table = Calculate_Percent_AbilityToCureDiseases_Table(
                game,
                current_Percent_Complete_SetsOfCards,
                squared
                );
            double current_GroupAbilityToCureDiseases = Calculate_Percent_AbilityToCureDiseases(
                game,
                current_GroupAbilityToCureDiseases_Table
                );

            int[,] supposed_NumCardsTable = NumCardsTable_AfterApplying_PlayerAction(
                game,
                current_NumCardsTable,
                action
                );
            double[,] supposed_Percent_Complete_SetsOfCards = Calculate_Percent_CompleteSetsOfCards_Table(
                game,
                supposed_NumCardsTable
                );
            double[,] supposed_GroupAbilityToCureDiseases_Table = Calculate_Percent_AbilityToCureDiseases_Table(
                game,
                supposed_Percent_Complete_SetsOfCards,
                squared
                );
            double supposed_GroupAbilityToCureDiseases = Calculate_Percent_AbilityToCureDiseases(
                game,
                supposed_GroupAbilityToCureDiseases_Table
                );

            return supposed_GroupAbilityToCureDiseases - current_GroupAbilityToCureDiseases;
        }

        public static double Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
            PD_Game game,
            List<PD_GameAction_Base> actions,
            bool squared
            )
        {
            double effect = 0;
            foreach (var action in actions)
            {
                effect += Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
                    game,
                    action,
                    squared
                    );
            }
            return effect;
        }
        #endregion
    }
}
