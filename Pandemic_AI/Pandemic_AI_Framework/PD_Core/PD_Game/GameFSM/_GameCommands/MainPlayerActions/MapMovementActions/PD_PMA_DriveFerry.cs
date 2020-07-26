using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// A player moves to an adjacent location
    /// </summary>
    [Serializable]
    public class PD_PMA_DriveFerry : PD_MapMovementAction_Base
    {

        public PD_PMA_DriveFerry(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation
            ) : base(player, initialLocation, targetLocation)
        {

        }

        // private constructor, for custom deep copy purposes only
        private PD_PMA_DriveFerry(
            PD_PMA_DriveFerry actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy(),
                actionToCopy.InitialLocation.GetCustomDeepCopy(),
                actionToCopy.TargetLocation.GetCustomDeepCopy()
                )
        {

        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PMA_DriveFerry(Player, InitialLocation, TargetLocation);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_DriveFerry(this);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: DRIVE_FERRY: {1} -> {2}",
                Player.Name,
                InitialLocation.Name,
                TargetLocation.Name
                );

            return actionDescription;
        }
    }
}
