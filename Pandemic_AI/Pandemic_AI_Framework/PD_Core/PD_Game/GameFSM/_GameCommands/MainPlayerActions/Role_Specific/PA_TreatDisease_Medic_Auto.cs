﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_TreatDisease_Medic_Auto :
        PD_GameAction_Base,
        IEquatable<PA_TreatDisease_Medic_Auto>,
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
        public PA_TreatDisease_Medic_Auto(
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
        private PA_TreatDisease_Medic_Auto(
            PA_TreatDisease_Medic_Auto actionToCopy
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
            game.Com_PA_TreatDisease_Medic(Player, CityToTreatDiseaseAt, TypeOfDiseaseToTreat);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PA_TreatDisease_Medic_Auto(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: TREAT_DISEASE_MEDIC_AUTO type {1} on {2}",
                Player.ToString(),
                TypeOfDiseaseToTreat,
                CityToTreatDiseaseAt.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PA_TreatDisease_Medic_Auto other)
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
            if (other is PA_TreatDisease_Medic_Auto other_action)
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
            if (other is PA_TreatDisease_Medic_Auto other_action)
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
