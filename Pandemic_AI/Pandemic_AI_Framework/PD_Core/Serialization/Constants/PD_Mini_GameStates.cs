using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public static class PD_Mini__GameState
    {
        public const int IDLE = 0;
        public const int MAIN_PLAYER_ACTIONS = 1;
        public const int DISCARDING_DURING_MAIN_PLAYER_ACTIONS = 2;
        public const int DRAWING_NEW_PLAYER_CARDS = 3;
        public const int DISCARDING_AFTER_DRAWING = 4;
        public const int APPLYING_EPIDEMIC_CARDS = 5;
        public const int DRAWING_NEW_INFECTION_CARDS = 6;
        public const int APPLYING_INFECTION_CARDS = 7;
        public const int GAME_LOST = 8;
        public const int GAME_WON = 9;

    }         
}
