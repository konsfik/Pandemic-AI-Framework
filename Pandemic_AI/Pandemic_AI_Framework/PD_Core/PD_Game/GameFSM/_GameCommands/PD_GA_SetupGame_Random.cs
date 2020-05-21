using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GA_SetupGame_Random : PD_GameAction_Base
    {
        public PD_GA_SetupGame_Random()
        {
            
        }

        public override void Execute(PD_Game game)
        {
            game.Com_SetupGame_Random();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_GA_SetupGame_Random();
        }

        public override string GetDescription()
        {
            string description = "Game Setup Begins";
            return description;
        }
    }
}