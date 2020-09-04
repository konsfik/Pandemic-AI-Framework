using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_DiscoverCure_Scientist :
        PD_GameAction_Base,
        IEquatable<PD_PA_DiscoverCure_Scientist>,
        I_Player_Action
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
        public PD_PA_DiscoverCure_Scientist(
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
        private PD_PA_DiscoverCure_Scientist(
            PD_PA_DiscoverCure_Scientist actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.CityCardsToDiscard = actionToCopy.CityCardsToDiscard.CustomDeepCopy();
            this.CityOfResearchStation = actionToCopy.CityOfResearchStation.GetCustomDeepCopy();
            this.TypeOfDiseaseToCure = actionToCopy.TypeOfDiseaseToCure;
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_DiscoverCure_Scientist(this);
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
#if DEBUG
            PD_City current_player_location = game.GQ_CurrentPlayer_Location();

            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state!");
            }
            else if (game.GQ_Is_DiseaseCured_OR_Eradicated(TypeOfDiseaseToCure))
            {
                throw new System.Exception("disease is already cured!");
            }
            else if (game.GQ_Is_City_ResearchStation(current_player_location) == false)
            {
                throw new System.Exception("current player location is not a research station");
            }
            else if (game.GQ_Is_City_ResearchStation(CityOfResearchStation) == false)
            {
                throw new System.Exception("selected city is not a research station");
            }
            else if (current_player_location != CityOfResearchStation)
            {
                throw new System.Exception("selected city is not the player's location");
            }
            List<PD_CityCard> city_cards_in_player_hand = game.GQ_CityCardsInCurrentPlayerHand();
            foreach (var city_card in CityCardsToDiscard)
            {
                if (city_cards_in_player_hand.Contains(city_card) == false)
                {
                    throw new System.Exception("player does not have this card: " + city_card.ToString());
                }
            }
#endif
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

        #region equality overrides
        public bool Equals(PD_PA_DiscoverCure_Scientist other)
        {
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
            else if (CityCardsToDiscard.List_Equals(other.CityCardsToDiscard) == false)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(PD_GameAction_Base other)
        {
            if (other is PD_PA_DiscoverCure_Scientist other_action)
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
            if (other is PD_PA_DiscoverCure_Scientist other_action)
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

            hash = hash * 31 + Player.GetHashCode();
            hash = hash * 31 + CityOfResearchStation.GetHashCode();
            hash = hash * 31 + TypeOfDiseaseToCure.GetHashCode();
            hash = hash * 31 + CityCardsToDiscard.Custom_HashCode();

            return hash;
        }



        #endregion
    }
}
