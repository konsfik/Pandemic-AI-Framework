using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_GR_P_GameSettings : PD_Report_Part
    {
        public int NumPlayers { get; private set; }
        public int GameDifficulty { get; private set; }
        public string Player1_Role { get; private set; }
        public string Player2_Role { get; private set; }
        public string Player3_Role { get; private set; }
        public string Player4_Role { get; private set; }

        public PD_GR_P_GameSettings(
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
            NumPlayers = game.GameStateCounter.NumberOfPlayers;
            GameDifficulty = game.GameSettings.GameDifficultyLevel;

            Player1_Role = game.RoleCardsPerPlayerID[game.Players[0]].ToString();
            Player2_Role = game.RoleCardsPerPlayerID[game.Players[1]].ToString();

            if (game.Players.Count >= 3)
            {
                Player3_Role = game.RoleCardsPerPlayerID[game.Players[2]].ToString();
            }
            else {
                Player3_Role = "";
            }

            if (game.Players.Count >= 4)
            {
                Player4_Role = game.RoleCardsPerPlayerID[game.Players[3]].ToString();
            }
            else
            {
                Player4_Role = "";
            }
        }


    }
}
