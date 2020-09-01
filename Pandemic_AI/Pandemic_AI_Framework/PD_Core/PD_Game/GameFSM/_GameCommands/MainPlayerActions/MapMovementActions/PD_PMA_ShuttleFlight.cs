using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PMA_ShuttleFlight : PD_GameAction_Base, I_Player_Action, I_Movement_Action
    {
        public PD_Player Player { get; private set; }

        public PD_City InitialLocation { get; protected set; }

        public PD_City TargetLocation { get; protected set; }

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
            )
        {
            this.Player = player;
            this.InitialLocation = initialLocation;
            this.TargetLocation = targetLocation;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_ShuttleFlight(
            PD_PMA_ShuttleFlight actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.InitialLocation = actionToCopy.InitialLocation.GetCustomDeepCopy();
            this.TargetLocation = actionToCopy.TargetLocation.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.GO_MovePawnFromCityToCity(
                game.PlayerPawnsPerPlayerID[Player.ID],
                InitialLocation,
                TargetLocation
                );

            game.Medic_MoveTreat(TargetLocation);
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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PMA_ShuttleFlight)otherObject;

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