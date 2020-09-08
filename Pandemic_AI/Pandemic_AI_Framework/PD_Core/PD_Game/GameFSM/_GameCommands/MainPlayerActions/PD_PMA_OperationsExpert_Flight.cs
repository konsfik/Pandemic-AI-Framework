using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PMA_OperationsExpert_Flight :
        PD_GameAction_Base,
        IEquatable<PD_PMA_OperationsExpert_Flight>,
        I_Player_Action,
        I_Movement_Action
    {
        public int Player { get; private set; }

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
        public PD_PMA_OperationsExpert_Flight(
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

        /// <summary>
        /// private constructor, for deepcopy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_OperationsExpert_Flight(
            PD_PMA_OperationsExpert_Flight actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.InitialLocation = actionToCopy.InitialLocation;
            this.TargetLocation = actionToCopy.TargetLocation;
            this.CityCardToDiscard = actionToCopy.CityCardToDiscard.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_OperationsExpert_Flight(this);
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
                Player,
                InitialLocation,
                TargetLocation
                );

            game.Medic_MoveTreat(TargetLocation);
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: OPERATIONS_EXPERT_FLIGHT | card:{1} | {2} -> {3}",
                Player.ToString(),
                CityCardToDiscard.City.ToString(),
                InitialLocation.ToString(),
                TargetLocation.ToString()
                );

            return actionDescription;
        }

        #region equality overrides
        public bool Equals(PD_PMA_OperationsExpert_Flight other)
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
            if (other is PD_PMA_OperationsExpert_Flight other_action)
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
            if (other is PD_PMA_OperationsExpert_Flight other_action)
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
