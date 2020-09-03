using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ApplyEpidemicCard : PD_GameAction_Base, I_Auto_Action, I_Player_Action
    {
        public PD_Player Player { get; protected set; }

        #region constructors
        [JsonConstructor]
        public PD_ApplyEpidemicCard(
            PD_Player player
            )
        {
            this.Player = player.GetCustomDeepCopy();
        }

        // private constructor, for custom deep copy purposes only
        private PD_ApplyEpidemicCard(
            PD_ApplyEpidemicCard actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_ApplyEpidemicCard(this);
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_ApplyEpidemicCard(
                randomness_provider,
                Player
                );
        }

        public override string GetDescription()
        {
            return String.Format("{0}: EPIDEMIC.", Player.Name);
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_ApplyEpidemicCard)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            else {
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