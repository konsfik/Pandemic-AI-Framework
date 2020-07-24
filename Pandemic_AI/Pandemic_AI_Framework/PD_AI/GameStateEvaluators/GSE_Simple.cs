using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class GSE_Simple : PD_GameStateEvaluator_Base
    {
        private GameStateEvaluationScore _score_calculator;

        public GSE_Simple(GameStateEvaluationScore score_calculator)
        {
            _score_calculator = score_calculator;
        }

        public override double EvaluateGameState(PD_Game game)
        {
            return _score_calculator.CalculateScore(game);
        }
    }
}
