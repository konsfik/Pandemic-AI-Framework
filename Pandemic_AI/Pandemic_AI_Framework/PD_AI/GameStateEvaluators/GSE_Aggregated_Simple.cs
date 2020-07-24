using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class GSE_Aggregated_Simple : PD_GameStateEvaluator_Base
    {
        public List<GameStateEvaluationScore> Scores { get; private set; }

        public GSE_Aggregated_Simple(
            List<GameStateEvaluationScore> scores
            )
        {
            Scores = scores;
        }

        /// <summary>
        /// The game state evaluation is calculated as the average value
        /// of all the game state evaluation scores.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public override double EvaluateGameState(PD_Game game)
        {
            double scoreSum = 0;
            foreach (var score in Scores)
            {
                scoreSum += score.CalculateScore(game);
            }

            double scoreAverage = scoreSum / Scores.Count;

            return scoreAverage;
        }
    }
}
