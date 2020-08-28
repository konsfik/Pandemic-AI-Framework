﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class GSES_PercentCuredDiseases : GameStateEvaluationScore
    {
        public override double CalculateScore(PD_Game gameState)
        {
            int numDiseasesCured = gameState.GQ_Num_DiseasesCured();
            double percentCuredDiseases = (double)numDiseasesCured / 4;
            return percentCuredDiseases;
        }
    }
}
