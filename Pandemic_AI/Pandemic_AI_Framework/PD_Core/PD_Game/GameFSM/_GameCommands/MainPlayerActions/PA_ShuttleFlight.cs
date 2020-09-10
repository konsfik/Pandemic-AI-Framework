using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_ShuttleFlight :
        PD_Action,
        IEquatable<PA_ShuttleFlight>,
        I_Player_Action,
        I_Movement_Action
    {
        public int Player { get; private set; }

        public int ToCity { get; protected set; }

        #region constructors
        /// <summary>
        /// normal && json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="initialLocation"></param>
        /// <param name="targetLocation"></param>
        [JsonConstructor]
        public PA_ShuttleFlight(
            int player,
            int targetLocation
            )
        {
            this.Player = player;
            this.ToCity = targetLocation;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PA_ShuttleFlight(
            PA_ShuttleFlight actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.ToCity = actionToCopy.ToCity;
        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PA_ShuttleFlight(this);
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
            else if (game.GQ_Is_City_ResearchStation(game.GQ_CurrentPlayer_Location()) == false)
            {
                throw new System.Exception("current player location is NOT a research station!");
            }
            else if (game.GQ_Is_City_ResearchStation(ToCity) == false)
            {
                throw new System.Exception("final location is NOT a research station!");
            }
#endif

            game.GO_MovePawn_ToCity(
                Player,
                ToCity
                );

            game.Medic_MoveTreat(ToCity);
        }

        #region equality overrides
        public bool Equals(PA_ShuttleFlight other)
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
            if (other is PA_ShuttleFlight other_action)
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
            if (other is PA_ShuttleFlight other_action)
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

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: SHUTTLE_FLIGHT to City_{1}",
                Player.ToString(),
                ToCity.ToString()
                );

            return actionDescription;
        }


    }
}