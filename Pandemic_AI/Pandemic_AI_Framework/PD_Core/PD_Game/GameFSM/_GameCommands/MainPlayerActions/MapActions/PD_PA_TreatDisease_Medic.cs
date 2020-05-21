using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_TreatDisease_Medic : PD_MapAction_Base
    {
        public PD_City CityToTreatDiseaseAt { get; private set; }
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
            PD_Player player,
            PD_City cityToTreatDiseaseAt,
            int typeOfDiseaseToTreat
            ) : base(
                player
                )
        {
            CityToTreatDiseaseAt = cityToTreatDiseaseAt;
            TypeOfDiseaseToTreat = typeOfDiseaseToTreat;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_TreatDisease_Medic(
            PD_PA_TreatDisease_Medic actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            CityToTreatDiseaseAt = actionToCopy.CityToTreatDiseaseAt.GetCustomDeepCopy();
            TypeOfDiseaseToTreat = actionToCopy.TypeOfDiseaseToTreat;
        }
        #endregion

        public override void Execute(PD_Game game)
        {
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
                Player.Name,
                TypeOfDiseaseToTreat,
                CityToTreatDiseaseAt.Name
                );
        }
    }
}
