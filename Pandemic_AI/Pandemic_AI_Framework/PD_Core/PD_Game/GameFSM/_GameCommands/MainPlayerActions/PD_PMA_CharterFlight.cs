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
        I_Movement_Action,
        I_Uses_Card
    {
        public int Player { get; private set; }

        public int FromCity { get; protected set; }

        public int ToCity { get; protected set; }

        public int UsedCard { get; private set; }



        #region constructors
        /// <summary>
        /// normal && json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="fromCity"></param>
        /// <param name="toCity"></param>
        /// <param name="usedCard"></param>
        [JsonConstructor]
        public PD_PMA_CharterFlight(
            int player,
            int fromCity,
            int toCity,
            int usedCard
            )
        {
            this.Player = player;
            this.FromCity = fromCity;
            this.ToCity = toCity;
            this.UsedCard = usedCard;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_CharterFlight(
            PD_PMA_CharterFlight actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.FromCity = actionToCopy.FromCity;
            this.ToCity = actionToCopy.ToCity;
            this.UsedCard = actionToCopy.UsedCard;
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.GO_PlayerDiscardsPlayerCard(
                Player,
                UsedCard
                );

            game.GO_MovePawn_ToCity(
                Player,
                ToCity
                );

            game.Medic_MoveTreat(ToCity);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_CharterFlight(this);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: CHARTER_FLIGHT | card: {1} | {2} -> {3}",
                Player.ToString(),
                UsedCard.ToString(),
                FromCity.ToString(),
                ToCity.ToString()
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
            else if (this.FromCity != other.FromCity)
            {
                return false;
            }
            else if (this.ToCity != other.ToCity)
            {
                return false;
            }
            else if (this.UsedCard != other.UsedCard)
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

            hash = hash * 31 + Player;
            hash = hash * 31 + FromCity;
            hash = hash * 31 + ToCity;
            hash = hash * 31 + UsedCard;

            return hash;
        }



        #endregion
    }
}