using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ApplyEpidemicCard : PD_AutoAction_Base
    {
        public PD_ApplyEpidemicCard(
            PD_Player player
            ) : base(
                player
                )
        {

        }

        public override void Execute(PD_Game game)
        {
            game.Com_ApplyEpidemicCard(Player);
        }

        // private constructor, for custom deep copy purposes only
        private PD_ApplyEpidemicCard(
            PD_ApplyEpidemicCard actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {

        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_ApplyEpidemicCard(this);
        }

        public override string GetDescription()
        {
            return String.Format("{0}: EPIDEMIC.", Player.Name);
        }


    }
}