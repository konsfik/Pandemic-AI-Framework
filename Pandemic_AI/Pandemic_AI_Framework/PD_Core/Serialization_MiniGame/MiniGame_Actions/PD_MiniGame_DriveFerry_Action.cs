using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class PD_MiniGame_DriveFerry_Action : PD_MiniGame_Action
    {
        public int player;
        public int from_city;
        public int to_city;

        public PD_MiniGame_DriveFerry_Action(
            int player, 
            int from_city,
            int to_city
            )
        {
            this.player = player;
            this.from_city = from_city;
            this.to_city = to_city;
        }
    }
}
