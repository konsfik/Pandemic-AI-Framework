using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GR_P_GameCondition : PD_Report_Part
    {
        public int GameOngoing { get; private set; }
        public int GameWon { get; private set; }
        public int GameLost { get; private set; }
        public int Lost_Outbreaks { get; private set; }
        public int Lost_DiseaseCubes { get; private set; }
        public int Lost_PlayerCards { get; private set; }

        public PD_GR_P_GameCondition(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            Update(game, pathFinder);
        }

        public override void Update(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            GameOngoing = PD_Game_Queries.GQ_Is_Ongoing(game) ? 1 : 0;
            GameWon = PD_Game_Queries.GQ_Is_Won(game) ? 1 : 0;
            GameLost = PD_Game_Queries.GQ_Is_Lost(game) ? 1 : 0;
            Lost_Outbreaks = PD_Game_Queries.GQ_SS_DeadlyOutbreaks(game) ? 1 : 0;
            Lost_DiseaseCubes =
                game.GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection ? 1 : 0;
            Lost_PlayerCards =
                game.GameStateCounter.NotEnoughPlayerCardsToDraw ? 1 : 0;
        }
    }
}
