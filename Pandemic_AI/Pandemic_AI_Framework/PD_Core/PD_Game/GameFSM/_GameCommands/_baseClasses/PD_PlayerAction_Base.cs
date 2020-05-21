using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_PlayerAction_Base : PD_GameAction_Base, IDescribable
    {
        public PD_Player Player { get; protected set; }

        public PD_PlayerAction_Base(PD_Player player)
        {
            Player = player;
        }
    }
}
