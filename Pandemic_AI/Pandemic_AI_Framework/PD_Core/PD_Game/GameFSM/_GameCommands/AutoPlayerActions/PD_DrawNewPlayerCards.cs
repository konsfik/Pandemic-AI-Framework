using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_DrawNewPlayerCards : PD_AutoAction_Base
    {
        public PD_DrawNewPlayerCards(
            PD_Player player
            ) : base(
                player
                )
        {

        }

        public override void Execute(PD_Game game)
        {
            game.Com_DrawNewPlayerCards(Player);
        }

        // private constructor, for custom deep copy purposes only
        private PD_DrawNewPlayerCards(
            PD_DrawNewPlayerCards actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {

        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_DrawNewPlayerCards(this);
        }

        public override string GetDescription()
        {
            return String.Format("{0}: DRAW Player Cards.", Player.Name);
        }
    }
}