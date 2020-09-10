using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_OperationsExpert_Flight :
        PD_Action,
        IEquatable<PA_OperationsExpert_Flight>,
        I_Player_Action,
        I_Movement_Action,
        I_Uses_Card
    {
        public int Player { get; private set; }

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
        public PA_OperationsExpert_Flight(
            int player,
            int toCity,
            int usedCard
            )
        {
            this.Player = player;
            this.ToCity = toCity;
            this.UsedCard = usedCard;
        }

        /// <summary>
        /// private constructor, for deepcopy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PA_OperationsExpert_Flight(
            PA_OperationsExpert_Flight actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.ToCity = actionToCopy.ToCity;
            this.UsedCard = actionToCopy.UsedCard;
        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PA_OperationsExpert_Flight(this);
        }
        #endregion


        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
#if DEBUG
            int current_player = game.GQ_CurrentPlayer();
            if (game.GQ_CurrentPlayer_Role() != PD_Player_Roles.Operations_Expert)
            {
                throw new System.Exception("player is not an operations expert!");
            }
            else if (game.GQ_Is_City_ResearchStation(game.GQ_CurrentPlayer_Location()) == false)
            {
                throw new System.Exception("current player location is not a research station!");
            }
            else if (game.GQ_CityCardsInCurrentPlayerHand().Contains(UsedCard) == false)
            {
                throw new System.Exception("player dows not own this card!");
            }
#endif

            game.GO_PlayerDiscardsPlayerCard(
                Player,
                UsedCard
                );

            game.GO_MovePawn_ToCity(
                Player,
                ToCity
                );

            game.game_state_counter.operations_expert_flight_used_this_turn = true;

            game.Medic_MoveTreat(ToCity);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: OPERATIONS_EXPERT_FLIGHT | card:{1} | to City_{2}",
                Player.ToString(),
                UsedCard.ToString(),
                ToCity.ToString()
                );

            return actionDescription;
        }

        #region equality overrides
        public bool Equals(PA_OperationsExpert_Flight other)
        {
            if (this.Player != other.Player)
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

        public override bool Equals(PD_Action other)
        {
            if (other is PA_OperationsExpert_Flight other_action)
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
            if (other is PA_OperationsExpert_Flight other_action)
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
            hash = hash * 31 + UsedCard;

            return hash;
        }



        #endregion
    }
}
