using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_DiscoverCure_Scientist : PD_MapAction_Base
    {
        public List<PD_CityCard> CityCardsToDiscard { get; private set; }
        public PD_City CityOfResearchStation { get; private set; }
        public int TypeOfDiseaseToCure { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cityOfResearchStation"></param>
        /// <param name="cityCardsToDiscard"></param>
        /// <param name="typeOfDiseaseToCure"></param>
        [JsonConstructor]
        public PD_PA_DiscoverCure_Scientist(
            PD_Player player,
            PD_City cityOfResearchStation,
            List<PD_CityCard> cityCardsToDiscard,
            int typeOfDiseaseToCure
            ) : base(player)
        {
            if (cityCardsToDiscard.Count != 4)
            {
                throw new System.Exception("There should be 4 cards in that list");
            }
            CityCardsToDiscard = cityCardsToDiscard;
            CityOfResearchStation = cityOfResearchStation;
            TypeOfDiseaseToCure = typeOfDiseaseToCure;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_DiscoverCure_Scientist(
            PD_PA_DiscoverCure_Scientist actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            CityCardsToDiscard = actionToCopy.CityCardsToDiscard.CustomDeepCopy();
            CityOfResearchStation = actionToCopy.CityOfResearchStation.GetCustomDeepCopy();
            TypeOfDiseaseToCure = actionToCopy.TypeOfDiseaseToCure;
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PA_DiscoverCure_Scientist(Player, CityCardsToDiscard, TypeOfDiseaseToCure);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_DiscoverCure_Scientist(this);
        }

        public override string GetDescription()
        {
            string description = String.Format(
                "{0}: DISCOVER_CURE_SCIENTIST of type {1} on {2}",
                Player.Name,
                TypeOfDiseaseToCure,
                CityOfResearchStation.Name
                );

            return description;
        }
    }
}
