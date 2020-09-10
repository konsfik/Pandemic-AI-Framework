using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_MoveResearchStation : 
        PD_Action, 
        IEquatable<PA_MoveResearchStation>,
        I_Player_Action
    {
        public int Player { get; private set; }
        public int Used_CityCard { get; private set; }
        public int Move_RS_From { get; private set; }
        public int Move_RS_To { get; private set; }
        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="used_CityCard"></param>
        /// <param name="move_RS_From"></param>
        /// <param name="move_RS_To"></param>
        [JsonConstructor]
        public PA_MoveResearchStation(
            int player,
            int used_CityCard,
            int move_RS_From,
            int move_RS_To
            )
        {
            this.Player = player;
            this.Used_CityCard = used_CityCard;
            this.Move_RS_From = move_RS_From;
            this.Move_RS_To = move_RS_To;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PA_MoveResearchStation(
            PA_MoveResearchStation actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.Used_CityCard = actionToCopy.Used_CityCard;
            this.Move_RS_From = actionToCopy.Move_RS_From;
            this.Move_RS_To = actionToCopy.Move_RS_To;
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
            else if (game.GQ_NumInactiveResearchStations() > 0)
            {
                throw new System.Exception("there are inactive research stations available!");
            }
            else if (Player != game.GQ_CurrentPlayer())
            {
                throw new System.Exception("wrong player!");
            }
            else if (Used_CityCard != game.GQ_CurrentPlayer_Location())
            {
                throw new System.Exception("city card does not match current player position");
            }
            else if (Move_RS_To != game.GQ_CurrentPlayer_Location())
            {
                throw new System.Exception("selected city does not match current player position");
            }
#endif
            game.GO_PlayerDiscardsPlayerCard(
                Player,
                Used_CityCard
                );

            game.map_elements.research_stations__per__city[Move_RS_From] = false;
            game.map_elements.research_stations__per__city[Move_RS_To] = true;

        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PA_MoveResearchStation(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: MOVE_RESEARCH_STATION from {1} to {2}",
                Player.ToString(),
                Move_RS_From.ToString(),
                Move_RS_To.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PA_MoveResearchStation other)
        {
            if (this.Player != other.Player)
            {
                return false;
            }
            if (this.Used_CityCard != other.Used_CityCard)
            {
                return false;
            }
            if (this.Move_RS_From != other.Move_RS_From)
            {
                return false;
            }
            if (this.Move_RS_To != other.Move_RS_To)
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
            if (other is PA_MoveResearchStation other_action)
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
            if (other is PA_MoveResearchStation other_action)
            {
                return Equals(other_action);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player;
            hash = hash * 31 + Used_CityCard;
            hash = hash * 31 + Move_RS_From;
            hash = hash * 31 + Move_RS_To;

            return hash;
        }

        

        #endregion
    }
}