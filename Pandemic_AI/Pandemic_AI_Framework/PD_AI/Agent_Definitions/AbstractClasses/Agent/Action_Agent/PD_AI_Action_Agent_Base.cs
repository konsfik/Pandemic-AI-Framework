using System.Collections;
using System.Collections.Generic;
using System;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_AI_Action_Agent_Base : PD_AI_Agent_Base
    {
        public PD_AI_Action_Agent_Base()
        {

        }

        public abstract PD_GameAction_Base GetNextAction(
            Random randomness_provider,
            PD_Game game
            );
    }
}