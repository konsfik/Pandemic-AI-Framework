using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// Player discards a city card to move to the city named of the card
    /// </summary>
    [Serializable]
    public class PD_PMA_DirectFlight : PD_MapMovementAction_Base
    {
        public PD_CityCard CityCardToDiscard { get; private set; }

        [JsonConstructor]
        public PD_PMA_DirectFlight(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation,
            PD_CityCard cityCardToDiscard
            ) : base(
                player, 
                initialLocation, 
                targetLocation
                )
        {
            CityCardToDiscard = cityCardToDiscard;
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PMA_DirectFlight(Player, InitialLocation, TargetLocation, CityCardToDiscard);
        }

        // private constructor, for custom deep copy purposes only
        private PD_PMA_DirectFlight(
            PD_PMA_DirectFlight actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy(),
                actionToCopy.InitialLocation.GetCustomDeepCopy(),
                actionToCopy.TargetLocation.GetCustomDeepCopy()
                )
        {
            CityCardToDiscard = actionToCopy.CityCardToDiscard.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_DirectFlight(this);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: DIRECT_FLIGHT | card: {1} | {2} -> {3}",
                Player.Name,
                CityCardToDiscard.City.Name,
                InitialLocation.Name,
                TargetLocation.Name
                );

            return actionDescription;
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PMA_DirectFlight)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            else if (this.InitialLocation != other.InitialLocation)
            {
                return false;
            }
            else if (this.TargetLocation != other.TargetLocation)
            {
                return false;
            }
            else if (this.CityCardToDiscard != other.CityCardToDiscard)
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
            hash = hash * 31 + InitialLocation.GetHashCode();
            hash = hash * 31 + TargetLocation.GetHashCode();
            hash = hash * 31 + CityCardToDiscard.GetHashCode();

            return hash;
        }

        #endregion
    }
}