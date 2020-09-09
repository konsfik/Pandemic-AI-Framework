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
        public int Player { get; private set; }

        public int FromCity { get; protected set; }

        public int ToCity { get; protected set; }

        public PD_PMA_DriveFerry(
            int player,
            int initialLocation,
            int targetLocation
            )
        {
            this.Player = player;
            this.FromCity = initialLocation;
            this.ToCity = targetLocation;
        }

        // private constructor, for custom deep copy purposes only
        private PD_PMA_DriveFerry(
            PD_PMA_DriveFerry actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.FromCity = actionToCopy.FromCity;
            this.ToCity = actionToCopy.ToCity;
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
            game.GO_MovePawnFromCityToCity(
                Player,
                FromCity,
                ToCity
                );

            game.Medic_MoveTreat(ToCity);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: DRIVE_FERRY: {1} -> {2}",
                Player.ToString(),
                FromCity.ToString(),
                ToCity.ToString()
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
            else if (this.FromCity != other.FromCity)
            {
                return false;
            }
            else if (this.ToCity != other.ToCity)
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

            hash = hash * 31 + Player;
            hash = hash * 31 + FromCity;
            hash = hash * 31 + ToCity;

            return hash;
        }



        #endregion
    }
}
