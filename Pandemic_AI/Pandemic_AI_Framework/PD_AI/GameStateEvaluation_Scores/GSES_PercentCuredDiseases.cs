using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class GSES_PercentCuredDiseases : GameStateEvaluationScore
    {
        public override double CalculateScore(PD_Game gameState)
        {
            int numDiseasesCured = PD_Game_Queries.GQ_Count_Num_DiseasesCured(gameState);
            double percentCuredDiseases = (double)numDiseasesCured / 4;
            return percentCuredDiseases;
        }
    }
}
