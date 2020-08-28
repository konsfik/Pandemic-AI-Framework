using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pandemic_AI_Framework
{
    public class GSES_GradientCuredDiseases : GameStateEvaluationScore
    {
        public override double CalculateScore(PD_Game gameState)
        {
            int[,] numCardsTable = PD_AI_CardEvaluation_Utilities.NumCardsTable(
                gameState
                );

            double[,] percentComplete_Table = 
                PD_AI_CardEvaluation_Utilities.Calculate_Percent_CompleteSetsOfCards_Table(
                    gameState, 
                    numCardsTable
                    );

            double[,] groupAbilityToCureDiseases_Table = 
                PD_AI_CardEvaluation_Utilities.Calculate_Percent_AbilityToCureDiseases_Table(
                    gameState,
                    percentComplete_Table,
                    false
                    );

            double[,] finalTable = groupAbilityToCureDiseases_Table.CustomDeepCopy();

            int numTypes = groupAbilityToCureDiseases_Table.Height();
            int numPlayers = groupAbilityToCureDiseases_Table.Width();

            for (int type = 0; type < numTypes; type++)
            {
                bool isDiseaseCured = gameState.GQ_Is_DiseaseCured_OR_Eradicated(type);
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

            //if (squared)
            //{
            //    normalized_Table = normalized_Table.Squared();
            //}

            return 
                normalized_Table
                .RowMax_PerRow()
                .Average();
        }
    }
}
