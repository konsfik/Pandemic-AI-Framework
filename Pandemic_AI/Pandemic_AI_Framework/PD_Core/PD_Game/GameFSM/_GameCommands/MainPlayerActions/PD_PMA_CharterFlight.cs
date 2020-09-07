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
    public class PD_PMA_CharterFlight :
        PD_GameAction_Base,
        IEquatable<PD_PMA_CharterFlight>,
        I_Player_Action,
        I_Movement_Action
    {
        public PD_Player Player { get; private set; }

        public int InitialLocation { get; protected set; }

        public int TargetLocation { get; protected set; }

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
            int initialLocation,
            int targetLocation,
            PD_CityCard cityCardToDiscard
            )
        {
            this.Player = player;
            this.InitialLocation = initialLocation;
            this.TargetLocation = targetLocation;
            this.CityCardToDiscard = cityCardToDiscard;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_CharterFlight(
            PD_PMA_CharterFlight actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.InitialLocation = actionToCopy.InitialLocation;
            this.TargetLocation = actionToCopy.TargetLocation;
            this.CityCardToDiscard = actionToCopy.CityCardToDiscard.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.GO_PlayerDiscardsPlayerCard(
                Player,
                CityCardToDiscard
                );

            game.GO_MovePawnFromCityToCity(
                game.PlayerPawnsPerPlayerID[Player.ID],
                InitialLocation,
                TargetLocation
                );

            game.Medic_MoveTreat(TargetLocation);
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
                CityCardToDiscard.City.ToString(),
                InitialLocation.ToString(),
                TargetLocation.ToString()
                );

            return actionDescription;
        }

        #region equality overrides
        public bool Equals(PD_PMA_CharterFlight other)
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
            else if (this.CityCardToDiscard != other.CityCardToDiscard)
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
            if (other is PD_PMA_CharterFlight other_action)
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
            if (other is PD_PMA_CharterFlight other_action)
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
            hash = hash * 31 + CityCardToDiscard.GetHashCode();

            return hash;
        }



        #endregion
    }
}