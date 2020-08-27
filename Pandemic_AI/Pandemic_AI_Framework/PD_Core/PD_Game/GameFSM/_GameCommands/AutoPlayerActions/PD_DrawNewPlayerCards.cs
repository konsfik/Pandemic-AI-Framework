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

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_DrawNewPlayerCards)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player.GetHashCode();

            return hash;
        }

        #endregion
    }
}