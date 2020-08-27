using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class GSE_Aggregated_WinOrDefeat_Adjusted : PD_GameStateEvaluator
    {
        public List<GameStateEvaluationScore> Scores { get; private set; }

        public GSE_Aggregated_WinOrDefeat_Adjusted(
            List<GameStateEvaluationScore> scores
            )
        {
            Scores = scores;
        }

        public override double EvaluateGameState(PD_Game game)
        {
            double scoreSum = 0;
            foreach (var score in Scores)
            {
                scoreSum += score.CalculateScore(game);
            }

            double scoreAverage = scoreSum / Scores.Count;

            if (PD_Game_Queries.GQ_Is_Won(game))
            {
                return 1;
            }
            else if (PD_Game_Queries.GQ_Is_Ongoing(game))
            {
                return scoreAverage;
            }
            else
            {
                return 0.1 * scoreAverage;
            }
        }
    }
}
