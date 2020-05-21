using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_MainAction_Base : PD_PlayerAction_Base
    {
        public PD_MainAction_Base(PD_Player player) : base(player)
        {

        }
    }
}