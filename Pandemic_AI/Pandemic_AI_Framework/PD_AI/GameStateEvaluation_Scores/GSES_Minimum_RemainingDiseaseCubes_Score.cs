using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class GSES_Minimum_RemainingDiseaseCubes_Score : GameStateEvaluationScore_Base
    {
        public override double CalculateScore(PD_Game gameState)
        {
            return PD_AI_GameStateEvaluation_HelpMethods.Calculate_Minimum_PercentAvailableDiseaseCubes_Min_1(gameState);
        }
    }
}
