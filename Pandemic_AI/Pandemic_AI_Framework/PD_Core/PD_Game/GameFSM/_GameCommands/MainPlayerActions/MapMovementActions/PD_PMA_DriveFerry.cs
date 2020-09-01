using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// A player moves to an adjacent location
    /// </summary>
    [Serializable]
    public class PD_PMA_DriveFerry : PD_GameAction_Base, I_Player_Action, I_Movement_Action
    {
        public PD_Player Player { get; private set; }

        public PD_City InitialLocation { get; protected set; }

        public PD_City TargetLocation { get; protected set; }

        public PD_PMA_DriveFerry(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation
            )
        {
            this.Player = player;
            this.InitialLocation = initialLocation;
            this.TargetLocation = targetLocation;
        }

        // private constructor, for custom deep copy purposes only
        private PD_PMA_DriveFerry(
            PD_PMA_DriveFerry actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.InitialLocation = actionToCopy.InitialLocation.GetCustomDeepCopy();
            this.TargetLocation = actionToCopy.TargetLocation.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_DriveFerry(this);
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            PD_Game_Operators.GO_MovePawnFromCityToCity(
                game,
                game.PlayerPawnsPerPlayerID[Player.ID],
                InitialLocation,
                TargetLocation
                );

            game.Medic_MoveTreat(TargetLocation);
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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PMA_DriveFerry)otherObject;

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

            return hash;
        }

        #endregion
    }
}
