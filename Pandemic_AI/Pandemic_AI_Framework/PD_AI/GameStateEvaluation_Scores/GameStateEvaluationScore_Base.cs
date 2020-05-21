using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public abstract class GameStateEvaluationScore_Base
    {
        public abstract double CalculateScore(PD_Game gameState);
    }
}
