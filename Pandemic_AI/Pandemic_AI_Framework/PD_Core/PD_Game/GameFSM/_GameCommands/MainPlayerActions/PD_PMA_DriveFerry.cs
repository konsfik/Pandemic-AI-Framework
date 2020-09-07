using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// A player moves to an adjacent location
    /// </summary>
    [Serializable]
    public class PD_PMA_DriveFerry :
        PD_GameAction_Base,
        IEquatable<PD_PMA_DriveFerry>,
        I_Player_Action,
        I_Movement_Action
    {
        public PD_Player Player { get; private set; }

        public int InitialLocation { get; protected set; }

        public int TargetLocation { get; protected set; }

        public PD_PMA_DriveFerry(
            PD_Player player,
            int initialLocation,
            int targetLocation
            )
        {
            this.Player = player.GetCustomDeepCopy();
            this.InitialLocation = initialLocation;
            this.TargetLocation = targetLocation;
        }

        // private constructor, for custom deep copy purposes only
        private PD_PMA_DriveFerry(
            PD_PMA_DriveFerry actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.InitialLocation = actionToCopy.InitialLocation;
            this.TargetLocation = actionToCopy.TargetLocation;
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
                InitialLocation.ToString(),
                TargetLocation.ToString()
                );

            return actionDescription;
        }

        #region equality overrides
        public bool Equals(PD_PMA_DriveFerry other)
        {
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

        public override bool Equals(PD_GameAction_Base other)
        {
            if (other is PD_PMA_DriveFerry other_action)
            {
                return Equals(other_action);
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object other)
        {
            if (other is PD_PMA_DriveFerry other_action)
            {
                return Equals(other_action);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player.GetHashCode();
            hash = hash * 31 + InitialLocation;
            hash = hash * 31 + TargetLocation;

            return hash;
        }



        #endregion
    }
}
