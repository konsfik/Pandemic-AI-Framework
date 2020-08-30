using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_MainAction_Base : PD_GameAction_Base, I_Player_Action
    {
        public PD_Player Player { get; protected set; }
        public PD_MainAction_Base(PD_Player player)
        {
            Player = player;
        }

    }
}