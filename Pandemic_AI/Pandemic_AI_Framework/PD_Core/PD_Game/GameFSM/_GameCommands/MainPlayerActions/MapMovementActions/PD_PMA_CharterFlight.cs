using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// Player Discards the City card that matches the city you are in to move to any city.
    /// </summary>
    [Serializable]
    public class PD_PMA_CharterFlight : PD_MapMovementAction_Base
    {
        public PD_CityCard CityCardToDiscard { get; private set; }

        #region constructors
        /// <summary>
        /// normal && json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="initialLocation"></param>
        /// <param name="targetLocation"></param>
        /// <param name="cityCardToDiscard"></param>
        [JsonConstructor]
        public PD_PMA_CharterFlight(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation,
            PD_CityCard cityCardToDiscard
            ) : base(player, initialLocation, targetLocation)
        {
            CityCardToDiscard = cityCardToDiscard;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_CharterFlight(
            PD_PMA_CharterFlight actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy(),
                actionToCopy.InitialLocation.GetCustomDeepCopy(),
                actionToCopy.TargetLocation.GetCustomDeepCopy()
                )
        {
            CityCardToDiscard = actionToCopy.CityCardToDiscard.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(PD_Game game)
        {
            game.Com_PMA_CharterFlight(Player, InitialLocation, TargetLocation, CityCardToDiscard);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_CharterFlight(this);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: CHARTER_FLIGHT | card: {1} | {2} -> {3}",
                Player.Name,
                CityCardToDiscard.City.Name,
                InitialLocation.Name,
                TargetLocation.Name
                );

            return actionDescription;
        }
    }
}