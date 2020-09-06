using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class PD_MiniGame_ApplyInfection_Action
    {
        public int player;
        public int infection_card;

        public PD_MiniGame_ApplyInfection_Action(int player, int infection_card)
        {
            this.player = player;
            this.infection_card = infection_card;
        }
    }
}
