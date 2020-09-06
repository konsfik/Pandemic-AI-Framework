using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class PD_MiniGame_BuildRS_Action
    {
        public int player;
        public int city_card;
        public int city;

        public PD_MiniGame_BuildRS_Action(int player, int city_card, int city)
        {
            this.player = player;
            this.city_card = city_card;
            this.city = city;
        }
    }
}
