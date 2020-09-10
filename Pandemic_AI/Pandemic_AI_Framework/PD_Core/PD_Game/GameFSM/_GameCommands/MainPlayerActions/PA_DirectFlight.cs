using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// Player discards a city card to move to the city named of the card
    /// </summary>
    [Serializable]
    public class PA_DirectFlight :
        PD_GameAction_Base,
        IEquatable<PA_DirectFlight>,
        I_Player_Action,
        I_Movement_Action,
        I_Uses_Card
    {
        public int Player { get; private set; }

        public int FromCity { get; protected set; }

        public int ToCity { get; protected set; }

        public int UsedCard { get; private set; }


        [JsonConstructor]
        public PA_DirectFlight(
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

        // private constructor, for custom deep copy purposes only
        private PA_DirectFlight(
            PA_DirectFlight actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.FromCity = actionToCopy.FromCity;
            this.ToCity = actionToCopy.ToCity;
            this.UsedCard = actionToCopy.UsedCard;
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PA_DirectFlight(this);
        }

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

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: DIRECT_FLIGHT | card: {1} | {2} -> {3}",
                Player.ToString(),
                UsedCard.ToString(),
                FromCity.ToString(),
                ToCity.ToString()
                );

            return actionDescription;
        }

        #region equality overrides
        public bool Equals(PA_DirectFlight other)
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
            if (other is PA_DirectFlight other_action)
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
            if (other is PA_DirectFlight other_action)
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