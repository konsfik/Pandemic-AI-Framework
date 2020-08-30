using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PMA_OperationsExpert_Flight : PD_MainAction_Base, I_Movement_Action
    {
        public PD_City InitialLocation { get; protected set; }

        public PD_City TargetLocation { get; protected set; }

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
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation,
            PD_CityCard cityCardToDiscard

            ) : base(
                player
                )
        {
            this.InitialLocation = initialLocation;
            this.TargetLocation = targetLocation;
            this.CityCardToDiscard = cityCardToDiscard;
        }

        /// <summary>
        /// private constructor, for deepcopy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PMA_OperationsExpert_Flight(
            PD_PMA_OperationsExpert_Flight actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            this.InitialLocation = actionToCopy.InitialLocation.GetCustomDeepCopy();
            this.TargetLocation = actionToCopy.TargetLocation.GetCustomDeepCopy();
            this.CityCardToDiscard = actionToCopy.CityCardToDiscard.GetCustomDeepCopy();
        }
        #endregion


        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PMA_OperationsExpertFlight(
                Player,
                InitialLocation,
                TargetLocation,
                CityCardToDiscard
                );
        }

        public override string GetDescription()
        {
            string actionDescription = String.Format(
                "{0}: OPERATIONS_EXPERT_FLIGHT | card:{1} | {2} -> {3}",
                Player.Name,
                CityCardToDiscard.City.Name,
                InitialLocation.Name,
                TargetLocation.Name
                );

            return actionDescription;
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PMA_OperationsExpert_Flight(this);
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PMA_OperationsExpert_Flight)otherObject;

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

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player.GetHashCode();
            hash = hash * 31 + InitialLocation.GetHashCode();
            hash = hash * 31 + TargetLocation.GetHashCode();
            hash = hash * 31 + CityCardToDiscard.GetHashCode();

            return hash;
        }

        #endregion
    }
}
