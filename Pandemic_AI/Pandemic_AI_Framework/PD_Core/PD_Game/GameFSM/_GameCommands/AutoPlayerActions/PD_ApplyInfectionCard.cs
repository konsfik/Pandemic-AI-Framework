using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ApplyInfectionCard : PD_GameAction_Base, I_Auto_Action, I_Player_Action
    {
        public PD_Player Player { get; protected set; }

        public PD_InfectionCard InfectionCardToApply { get; private set; }

        [JsonConstructor]
        public PD_ApplyInfectionCard(
            PD_Player player,
            PD_InfectionCard infectionCardToApply
            )
        {
            this.Player = player.GetCustomDeepCopy();
            this.InfectionCardToApply = infectionCardToApply.GetCustomDeepCopy();
        }

        // private constructor, for custom deep copy purposes only
        private PD_ApplyInfectionCard(
            PD_ApplyInfectionCard actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.InfectionCardToApply = actionToCopy.InfectionCardToApply.GetCustomDeepCopy();
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_ApplyInfectionCard(Player, InfectionCardToApply);
        }

        

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_ApplyInfectionCard(this);
        }

        public override string GetDescription()
        {
            return Player.Name + ": INFECTION " + InfectionCardToApply.City.Name;
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_ApplyInfectionCard)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            else if (this.InfectionCardToApply != other.InfectionCardToApply) {
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
            hash = hash * 31 + InfectionCardToApply.GetHashCode();

            return hash;
        }

        #endregion
    }
}