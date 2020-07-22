using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class GSES_MultipliedRemainingDiseaseCubes : GameStateEvaluationScore
    {
        public override double CalculateScore(PD_Game gameState)
        {
            return PD_AI_GameStateEvaluation_HelpMethods.Calculate_Multiplied_Percent_AvailableDiseaseCubes_Min_1(gameState);
        }
    }
}
