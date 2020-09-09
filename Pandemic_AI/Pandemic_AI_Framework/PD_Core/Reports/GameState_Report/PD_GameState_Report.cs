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
            GameID = game.unique_id;
            CurrentTurn = game.game_state_counter.turn_index;
            CurrentPlayerAction = game.game_state_counter.player_action_index;
            CurrentGameStep = CurrentTurn * 4 + CurrentPlayerAction;
            CurrentGameState = game.game_FSM.CurrentState.GetType().Name.ToString();

            long gameDurationTicks = DateTime.UtcNow.Ticks - game.start_time.Ticks;
            GameDurMillis = (float)gameDurationTicks / 10000.0f;
        }
    }
}
