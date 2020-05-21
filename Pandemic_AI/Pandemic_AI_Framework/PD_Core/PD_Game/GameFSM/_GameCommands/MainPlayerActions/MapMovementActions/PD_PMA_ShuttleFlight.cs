using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PMA_ShuttleFlight : PD_MapMovementAction_Base
    {
        #region constructors
        /// <summary>
        /// normal && json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="initialLocation"></param>
        /// <param name="targetLocation"></param>
        [JsonConstructor]
        public PD_PMA_ShuttleFlight(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation
            ) : base(player, initialLocation, targetLocation)
        {

        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_ShuttleFlight(
            PD_PMA_ShuttleFlight actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy(),
                actionToCopy.InitialLocation.GetCustomDeepCopy(),
                actionToCopy.TargetLocation.GetCustomDeepCopy()
                )
        {

        }
        #endregion

        public override void Execute(PD_Game game)
        {
            game.Com_PMA_ShuttleFlight(Player, InitialLocation, TargetLocation);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_ShuttleFlight(this);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: SHUTTLE_FLIGHT {1} -> {2}",
                Player.Name,
                InitialLocation.Name,
                TargetLocation.Name
                );

            return actionDescription;
        }
    }
}