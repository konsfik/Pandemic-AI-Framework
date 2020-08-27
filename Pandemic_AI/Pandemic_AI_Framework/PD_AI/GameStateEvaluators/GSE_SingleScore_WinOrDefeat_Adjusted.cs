using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class GSE_SingleScore_WinOrDefeat_Adjusted : PD_GameStateEvaluator
    {
        private GameStateEvaluationScore _score_calculator;

        public GSE_SingleScore_WinOrDefeat_Adjusted(GameStateEvaluationScore score_calculator)
        {
            _score_calculator = score_calculator;
        }

        public override double EvaluateGameState(PD_Game game)
        {
            double score = _score_calculator.CalculateScore(game);

            if (PD_Game_Queries.GQ_Is_Won(game))
            {
                return 1;
            }
            else if (PD_Game_Queries.GQ_Is_Ongoing(game))
            {
                return score;
            }
            else
            {
                return 0.1 * score;
            }
        }
    }
}
