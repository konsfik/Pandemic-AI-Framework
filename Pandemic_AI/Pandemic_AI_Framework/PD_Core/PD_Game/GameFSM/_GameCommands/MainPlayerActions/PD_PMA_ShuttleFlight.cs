using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PMA_ShuttleFlight :
        PD_GameAction_Base,
        IEquatable<PD_PMA_ShuttleFlight>,
        I_Player_Action,
        I_Movement_Action
    {
        public int Player { get; private set; }

        public int FromCity { get; protected set; }

        public int ToCity { get; protected set; }

        #region constructors
        /// <summary>
        /// normal && json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="initialLocation"></param>
        /// <param name="targetLocation"></param>
        [JsonConstructor]
        public PD_PMA_ShuttleFlight(
            int player,
            int initialLocation,
            int targetLocation
            )
        {
            this.Player = player;
            this.FromCity = initialLocation;
            this.ToCity = targetLocation;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_ShuttleFlight(
            PD_PMA_ShuttleFlight actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.FromCity = actionToCopy.FromCity;
            this.ToCity = actionToCopy.ToCity;
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_ShuttleFlight(this);
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
#if DEBUG
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state!");
            }
            else if (Player != game.GQ_CurrentPlayer())
            {
                throw new System.Exception("wrong player...");
            }
            else if (game.GQ_Is_City_ResearchStation(FromCity) == false)
            {
                throw new System.Exception("initial location is NOT a research station!");
            }
            else if (game.GQ_Is_City_ResearchStation(ToCity) == false)
            {
                throw new System.Exception("final location is NOT a research station!");
            }
#endif

            game.GO_MovePawnFromCityToCity(
                Player,
                FromCity,
                ToCity
                );

            game.Medic_MoveTreat(ToCity);
        }

        #region equality overrides
        public bool Equals(PD_PMA_ShuttleFlight other)
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
            if (other is PD_PMA_ShuttleFlight other_action)
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
            if (other is PD_PMA_ShuttleFlight other_action)
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

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: SHUTTLE_FLIGHT {1} -> {2}",
                Player.ToString(),
                FromCity.ToString(),
                ToCity.ToString()
                );

            return actionDescription;
        }


    }
}