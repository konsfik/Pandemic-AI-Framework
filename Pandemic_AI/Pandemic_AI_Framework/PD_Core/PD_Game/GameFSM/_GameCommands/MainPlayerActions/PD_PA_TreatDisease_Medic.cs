using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_TreatDisease_Medic : 
        PD_GameAction_Base, 
        IEquatable<PD_PA_TreatDisease_Medic>,
        I_Player_Action
    {
        public int Player { get; private set; }
        public int CityToTreatDiseaseAt { get; private set; }
        public int TypeOfDiseaseToTreat { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cityToTreatDiseaseAt"></param>
        /// <param name="typeOfDiseaseToTreat"></param>
        [JsonConstructor]
        public PD_PA_TreatDisease_Medic(
            int player,
            int cityToTreatDiseaseAt,
            int typeOfDiseaseToTreat
            )
        {
            Player = player;
            CityToTreatDiseaseAt = cityToTreatDiseaseAt;
            TypeOfDiseaseToTreat = typeOfDiseaseToTreat;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_TreatDisease_Medic(
            PD_PA_TreatDisease_Medic actionToCopy
            )
        {
            Player = actionToCopy.Player;
            CityToTreatDiseaseAt = actionToCopy.CityToTreatDiseaseAt;
            TypeOfDiseaseToTreat = actionToCopy.TypeOfDiseaseToTreat;
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
                throw new System.Exception("wrong player!");
            }
            else if (game.GQ_Find_Player_Role(Player) != PD_Player_Roles.Medic)
            {
                throw new System.Exception("wrong player role!");
            }
            else if (CityToTreatDiseaseAt != game.GQ_PlayerLocation(Player))
            {
                throw new System.Exception("wrong player location");
            }
            else if (
                game.GQ_InfectionCubeTypes_OnCity(CityToTreatDiseaseAt)
                .Contains(TypeOfDiseaseToTreat) == false)
            {
                throw new System.Exception("the selected city does not have cubes of this type!");
            }
#endif
            game.Com_PA_TreatDisease_Medic(Player, CityToTreatDiseaseAt, TypeOfDiseaseToTreat);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_TreatDisease_Medic(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: TREAT_DISEASE_MEDIC type {1} on {2}",
                Player.ToString(),
                TypeOfDiseaseToTreat,
                CityToTreatDiseaseAt.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PD_PA_TreatDisease_Medic other)
        {
            if (this.Player != other.Player)
            {
                return false;
            }
            else if (this.CityToTreatDiseaseAt != other.CityToTreatDiseaseAt)
            {
                return false;
            }
            else if (this.TypeOfDiseaseToTreat != other.TypeOfDiseaseToTreat)
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
            if (other is PD_PA_TreatDisease_Medic other_action)
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
            if (other is PD_PA_TreatDisease_Medic other_action)
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
            hash = hash * 31 + CityToTreatDiseaseAt;
            hash = hash * 31 + TypeOfDiseaseToTreat;

            return hash;
        }

        

        #endregion
    }
}
