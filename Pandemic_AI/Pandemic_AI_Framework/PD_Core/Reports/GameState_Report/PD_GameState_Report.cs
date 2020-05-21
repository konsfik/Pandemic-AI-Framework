using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_GameState_Report : PD_Report_Part
    {
        public long GameID { get; private set; }
        public int Repetition { get; private set; }
        public int CurrentTurn { get; private set; }
        public int CurrentPlayerAction { get; private set; }
        public int CurrentGameStep { get; private set; }
        public string CurrentGameState { get; private set; }
        public float GameDurMillis { get; private set; }

        public PD_GameState_Report(PD_Game game, PD_AI_PathFinder pathFinder)
        {
            Repetition = 0;
            Update(game, pathFinder);
        }

        public void Update(PD_Game game, PD_AI_PathFinder pathFinder, int repetition)
        {
            Repetition = repetition;
            Update(game, pathFinder);
        }

        public override void Update(PD_Game game, PD_AI_PathFinder pathFinder)
        {
            GameID = game.UniqueID;
            CurrentTurn = game.GameStateCounter.CurrentTurnIndex;
            CurrentPlayerAction = game.GameStateCounter.CurrentPlayerActionIndex;
            CurrentGameStep = CurrentTurn * 4 + CurrentPlayerAction;
            CurrentGameState = game.GameFSM.CurrentState.GetType().Name.ToString();

            long gameDurationTicks = DateTime.UtcNow.Ticks - game.StartTime.Ticks;
            GameDurMillis = (float)gameDurationTicks / 10000.0f;
        }
    }
}
