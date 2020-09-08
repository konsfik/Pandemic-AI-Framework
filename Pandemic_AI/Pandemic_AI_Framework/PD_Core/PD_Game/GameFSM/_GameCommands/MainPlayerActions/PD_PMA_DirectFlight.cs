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
    public class PD_PMA_DirectFlight :
        PD_GameAction_Base,
        IEquatable<PD_PMA_DirectFlight>,
        I_Player_Action,
        I_Movement_Action
    {
        public int Player { get; private set; }

        public int InitialLocation { get; protected set; }

        public int TargetLocation { get; protected set; }

        public PD_CityCard CityCardToDiscard { get; private set; }


        [JsonConstructor]
        public PD_PMA_DirectFlight(
            int player,
            int initialLocation,
            int targetLocation,
            PD_CityCard cityCardToDiscard
            )
        {
            this.Player = player;
            this.InitialLocation = initialLocation;
            this.TargetLocation = targetLocation;
            this.CityCardToDiscard = cityCardToDiscard.GetCustomDeepCopy();
        }

        // private constructor, for custom deep copy purposes only
        private PD_PMA_DirectFlight(
            PD_PMA_DirectFlight actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.InitialLocation = actionToCopy.InitialLocation;
            this.TargetLocation = actionToCopy.TargetLocation;
            this.CityCardToDiscard = actionToCopy.CityCardToDiscard.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_DirectFlight(this);
        }

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
                Player,
                InitialLocation,
                TargetLocation
                );

            game.Medic_MoveTreat(TargetLocation);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: DIRECT_FLIGHT | card: {1} | {2} -> {3}",
                Player.ToString(),
                CityCardToDiscard.City.ToString(),
                InitialLocation.ToString(),
                TargetLocation.ToString()
                );

            return actionDescription;
        }

        #region equality overrides
        public bool Equals(PD_PMA_DirectFlight other)
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
            if (other is PD_PMA_DirectFlight other_action)
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
            if (other is PD_PMA_DirectFlight other_action)
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
            hash = hash * 31 + InitialLocation;
            hash = hash * 31 + TargetLocation;
            hash = hash * 31 + CityCardToDiscard.GetHashCode();

            return hash;
        }



        #endregion
    }
}