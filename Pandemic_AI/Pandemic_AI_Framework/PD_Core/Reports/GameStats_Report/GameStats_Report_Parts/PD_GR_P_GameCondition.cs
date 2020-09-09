﻿using System;
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
            GameOngoing = game.GQ_Is_Ongoing() ? 1 : 0;
            GameWon = game.GQ_Is_Won() ? 1 : 0;
            GameLost = game.GQ_Is_Lost() ? 1 : 0;
            Lost_Outbreaks = game.GQ_SS_DeadlyOutbreaks() ? 1 : 0;
            Lost_DiseaseCubes =
                game.game_state_counter.insufficient_disease_cubes_for_infection ? 1 : 0;
            Lost_PlayerCards =
                game.game_state_counter.insufficient_player_cards_to_draw ? 1 : 0;
        }
    }
}
