using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// A player moves to an adjacent location
    /// </summary>
    [Serializable]
    public class PA_DriveFerry :
        PD_Action,
        IEquatable<PA_DriveFerry>,
        I_Player_Action,
        I_Movement_Action
    {
        public int Player { get; private set; }

        public int ToCity { get; protected set; }

        public PA_DriveFerry(
            int player,
            int targetLocation
            )
        {
            this.Player = player;
            this.ToCity = targetLocation;
        }

        // private constructor, for custom deep copy purposes only
        private PA_DriveFerry(
            PA_DriveFerry actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.ToCity = actionToCopy.ToCity;
        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PA_DriveFerry(this);
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
#if DEBUG
            if (game.map.neighbors__per__city[game.GQ_CurrentPlayer_Location()].Contains(ToCity) == false)
            {
                throw new System.Exception("cannot go there!");
            }
#endif

            game.GO_MovePawn_ToCity(
                Player,
                ToCity
                );

            game.Medic_MoveTreat(ToCity);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: DRIVE_FERRY: to City_{1}",
                Player.ToString(),
                ToCity.ToString()
                );

            return actionDescription;
        }

        #region equality overrides
        public bool Equals(PA_DriveFerry other)
        {
            if (this.Player != other.Player)
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

        public override bool Equals(PD_Action other)
        {
            if (other is PA_DriveFerry other_action)
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
            if (other is PA_DriveFerry other_action)
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
            hash = hash * 31 + ToCity;

            return hash;
        }



        #endregion
    }
}
