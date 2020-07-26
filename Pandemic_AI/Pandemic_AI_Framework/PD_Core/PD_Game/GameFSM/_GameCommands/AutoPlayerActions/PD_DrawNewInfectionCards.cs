﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_DrawNewInfectionCards : PD_AutoAction_Base
    {
        public PD_DrawNewInfectionCards(
            PD_Player player
            ) : base(
                player
                )
        {

        }

        public override void Execute(
            Random randomness_provider, 
            PD_Game game
            )
        {
            game.Com_DrawNewInfectionCards(Player);
        }

        // private constructor, for custom deep copy purposes only
        private PD_DrawNewInfectionCards(
            PD_DrawNewInfectionCards actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {

        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_DrawNewInfectionCards(this);
        }

        public override string GetDescription()
        {
            return String.Format("{0}: DRAW Infection cards", Player.Name);
        }
    }
}