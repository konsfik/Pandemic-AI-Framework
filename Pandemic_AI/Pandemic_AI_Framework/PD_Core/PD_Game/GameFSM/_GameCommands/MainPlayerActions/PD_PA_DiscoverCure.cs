using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_DiscoverCure : PD_GameAction_Base, I_Player_Action
    {
        public PD_Player Player { get; private set; }
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
        public PD_PA_DiscoverCure(
            PD_Player player,
            PD_City cityOfResearchStation,
            List<PD_CityCard> cityCardsToDiscard,
            int typeOfDiseaseToCure
            )
        {
            this.Player = player;
            this.CityCardsToDiscard = cityCardsToDiscard;
            this.CityOfResearchStation = cityOfResearchStation;
            this.TypeOfDiseaseToCure = typeOfDiseaseToCure;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_DiscoverCure(
            PD_PA_DiscoverCure actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.CityCardsToDiscard = actionToCopy.CityCardsToDiscard.CustomDeepCopy();
            this.CityOfResearchStation = actionToCopy.CityOfResearchStation.GetCustomDeepCopy();
            this.TypeOfDiseaseToCure = actionToCopy.TypeOfDiseaseToCure;
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            // discard the cards
            foreach (var cityCard in CityCardsToDiscard)
            {
                game.GO_PlayerDiscardsPlayerCard(
                    Player,
                    cityCard
                    );
            }

            // discover the cure for the disease here...
            game.GameStateCounter.CureDisease(TypeOfDiseaseToCure);

            game.Medic_AutoTreat_AfterDiscoverCure(TypeOfDiseaseToCure);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_DiscoverCure(this);
        }

        public override string GetDescription()
        {
            string description = String.Format(
                "{0}: DISCOVER_CURE of type {1} on {2}",
                Player.Name,
                TypeOfDiseaseToCure,
                CityOfResearchStation.Name
                );

            return description;
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PA_DiscoverCure)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            else if (this.CityOfResearchStation != other.CityOfResearchStation)
            {
                return false;
            }
            else if (this.TypeOfDiseaseToCure != other.TypeOfDiseaseToCure)
            {
                return false;
            }
            else if (this.CityCardsToDiscard.Count != other.CityCardsToDiscard.Count)
            {
                return false;
            }

            for (int i = 0; i < CityCardsToDiscard.Count; i++)
            {
                if (this.CityCardsToDiscard[i] != other.CityCardsToDiscard[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player.GetHashCode();
            hash = hash * 31 + CityOfResearchStation.GetHashCode();
            hash = hash * 31 + TypeOfDiseaseToCure.GetHashCode();
            hash = hash * 31 + CityCardsToDiscard.Count;
            foreach (var city in CityCardsToDiscard)
            {
                hash = hash * 31 + city.GetHashCode();
            }

            return hash;
        }

        #endregion
    }
}
