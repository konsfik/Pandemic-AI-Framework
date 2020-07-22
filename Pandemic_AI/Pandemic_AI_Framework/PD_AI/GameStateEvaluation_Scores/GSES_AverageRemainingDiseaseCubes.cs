using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class GSES_AverageRemainingDiseaseCubes : GameStateEvaluationScore
    {
        public override double CalculateScore(PD_Game gameState)
        {
            return PD_AI_GameStateEvaluation_HelpMethods.Calculate_Average_Percent_AvailableDiseaseCubes_Min_1(gameState);
        }
    }
}
