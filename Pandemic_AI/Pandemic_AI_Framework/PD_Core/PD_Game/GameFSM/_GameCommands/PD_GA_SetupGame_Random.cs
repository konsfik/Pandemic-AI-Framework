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

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_SetupGame_Random(randomness_provider);
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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        #endregion
    }
}