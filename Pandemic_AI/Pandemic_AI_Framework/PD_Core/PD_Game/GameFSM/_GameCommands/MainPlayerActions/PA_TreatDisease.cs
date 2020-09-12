using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_TreatDisease :
        PD_Action,
        IEquatable<PA_TreatDisease>,
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
        public PA_TreatDisease(
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
        private PA_TreatDisease(
            PA_TreatDisease actionToCopy
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
            else if (game.GQ_Player_Role(Player) == PD_Player_Roles.Medic)
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

            bool diseaseCured = game.GQ_Is_DiseaseCured_OR_Eradicated(TypeOfDiseaseToTreat);

            if (diseaseCured)
            {
                // remove all cubes of this type
                game.GO_Remove_All_InfectionCubes_OfType_FromCity(
                    CityToTreatDiseaseAt,
                    TypeOfDiseaseToTreat
                    );

                // check if disease is eradicated...
                int remaining_cubes_this_type
                    = game.map_elements.available_infection_cubes__per__type[TypeOfDiseaseToTreat];

                // if disease eradicated -> set marker to 2
                if (remaining_cubes_this_type == 0)
                {
                    game.game_state_counter.disease_states[TypeOfDiseaseToTreat] = 2;
                }
            }
            else
            {
                // remove only one cube of this type
                game.GO_Remove_One_InfectionCube_OfType_FromCity(
                    CityToTreatDiseaseAt,
                    TypeOfDiseaseToTreat
                    );
            }
        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PA_TreatDisease(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: TREAT_DISEASE type {1} on {2}",
                Player.ToString(),
                TypeOfDiseaseToTreat,
                CityToTreatDiseaseAt.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PA_TreatDisease other)
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

        public override bool Equals(PD_Action other)
        {
            if (other is PA_TreatDisease other_action)
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
            if (other is PA_TreatDisease other_action)
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
            hash = hash * 31 + CityToTreatDiseaseAt;
            hash = hash * 31 + TypeOfDiseaseToTreat;

            return hash;
        }



        #endregion
    }
}
