using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// A class that is used to gather the most important data from a game that has is either finished or still running...
    /// This class also provides helper methods to convert 
    /// </summary>
    [Serializable]
    public class PD_GameStats_Report : PD_Report_Part
    {
        public PD_GR_P_GameSettings GameSettings { get; private set; }
        public PD_GR_P_GameCondition GameCondition { get; private set; }
        public PD_GR_P_GameScores GameScores { get; private set; }
        public PD_GR_P_GameStateEvaluationMetrics GameStateEvaluationMetrics { get; private set; }
        public PD_GR_P_ActionStats ActionStats { get; private set; }
        public PD_GR_P_MapStats MapStats { get; private set; }

        public PD_GameStats_Report(
            PD_Game game, 
            PD_AI_PathFinder pathFinder
            )
        {
            GameSettings = new PD_GR_P_GameSettings(game, pathFinder);
            GameCondition = new PD_GR_P_GameCondition(game, pathFinder);
            GameScores = new PD_GR_P_GameScores(game, pathFinder);
            ActionStats = new PD_GR_P_ActionStats(game, pathFinder);
            GameStateEvaluationMetrics = new PD_GR_P_GameStateEvaluationMetrics(game, pathFinder);
            MapStats = new PD_GR_P_MapStats(game, pathFinder);

            Update(game, pathFinder);
        }

        public override void Update(PD_Game game, PD_AI_PathFinder pathFinder)
        {
            GameSettings.Update(game, pathFinder);
            GameCondition.Update(game, pathFinder);
            GameScores.Update(game, pathFinder);
            ActionStats.Update(game, pathFinder);
            GameStateEvaluationMetrics.Update(game, pathFinder);
            MapStats.Update(game, pathFinder);
        }
    }
}
