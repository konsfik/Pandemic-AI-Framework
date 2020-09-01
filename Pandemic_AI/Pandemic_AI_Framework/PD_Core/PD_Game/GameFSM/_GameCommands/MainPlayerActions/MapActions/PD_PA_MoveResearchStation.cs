using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_MoveResearchStation : PD_GameAction_Base, I_Player_Action
    {
        public PD_Player Player { get; private set; }
        public PD_CityCard Used_CityCard { get; private set; }
        public PD_City Move_RS_From { get; private set; }
        public PD_City Move_RS_To { get; private set; }
        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="used_CityCard"></param>
        /// <param name="move_RS_From"></param>
        /// <param name="move_RS_To"></param>
        [JsonConstructor]
        public PD_PA_MoveResearchStation(
            PD_Player player,
            PD_CityCard used_CityCard,
            PD_City move_RS_From,
            PD_City move_RS_To
            )
        {
            this.Player = player;
            this.Used_CityCard = used_CityCard;
            this.Move_RS_From = move_RS_From;
            this.Move_RS_To = move_RS_To;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_MoveResearchStation(
            PD_PA_MoveResearchStation actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.Used_CityCard = actionToCopy.Used_CityCard.GetCustomDeepCopy();
            this.Move_RS_From = actionToCopy.Move_RS_From.GetCustomDeepCopy();
            this.Move_RS_To = actionToCopy.Move_RS_To.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            var cityCardsInPlayerHand = game.Cards.PlayerCardsPerPlayerID[Player.ID].FindAll(
                x =>
                x.GetType() == typeof(PD_CityCard)
                ).Cast<PD_CityCard>().ToList();

            var cityCardToDiscard = cityCardsInPlayerHand.Find(
                x =>
                x.Name == Move_RS_To.Name
                );

            game.GO_PlayerDiscardsPlayerCard(
                Player, 
                cityCardToDiscard
                );

            var researchStationToMove = game
                .MapElements
                .ResearchStationsPerCityID[Move_RS_From.ID].DrawLast();

            game.MapElements.ResearchStationsPerCityID[Move_RS_To.ID].Add(researchStationToMove);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_MoveResearchStation(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: MOVE_RESEARCH_STATION from {1} to {2}",
                Player.Name,
                Move_RS_From.Name,
                Move_RS_To.Name
                );
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PA_MoveResearchStation)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            if (this.Used_CityCard != other.Used_CityCard)
            {
                return false;
            }
            if (this.Move_RS_From != other.Move_RS_From)
            {
                return false;
            }
            if (this.Move_RS_To != other.Move_RS_To)
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
            hash = hash * 31 + Used_CityCard.GetHashCode();
            hash = hash * 31 + Move_RS_From.GetHashCode();
            hash = hash * 31 + Move_RS_To.GetHashCode();

            return hash;
        }

        #endregion
    }
}