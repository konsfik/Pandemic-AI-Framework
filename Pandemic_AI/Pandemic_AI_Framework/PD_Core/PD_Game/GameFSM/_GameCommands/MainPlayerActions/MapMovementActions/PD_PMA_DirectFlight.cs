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

        public override void Execute(PD_Game game)
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
    }
}