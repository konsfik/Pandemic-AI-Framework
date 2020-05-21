﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameCards : ICustomDeepCopyable<PD_GameCards>
    {

        public List<List<PD_InfectionCard>> DividedDeckOfInfectionCards { get; private set; }
        public List<PD_InfectionCard> ActiveInfectionCards { get; private set; }
        public List<PD_InfectionCard> DeckOfDiscardedInfectionCards { get; private set; }
        public List<List<PD_PlayerCardBase>> DividedDeckOfPlayerCards { get; private set; }
        public List<PD_PlayerCardBase> DeckOfDiscardedPlayerCards { get; private set; }
        public Dictionary<int, List<PD_PlayerCardBase>> PlayerCardsPerPlayerID { get; private set; }
        public List<PD_Role_Card> InactiveRoleCards { get; private set; }

        #region constructors
        public PD_GameCards(
            List<PD_Player> players,
            List<PD_Role_Card> allRoleCards
            )
        {
            DividedDeckOfInfectionCards = new List<List<PD_InfectionCard>>();
            ActiveInfectionCards = new List<PD_InfectionCard>();
            DeckOfDiscardedInfectionCards = new List<PD_InfectionCard>();

            DividedDeckOfPlayerCards = new List<List<PD_PlayerCardBase>>();
            DeckOfDiscardedPlayerCards = new List<PD_PlayerCardBase>();
            PlayerCardsPerPlayerID = new Dictionary<int, List<PD_PlayerCardBase>>();
            foreach (var player in players)
                PlayerCardsPerPlayerID.Add(player.ID, new List<PD_PlayerCardBase>());
            InactiveRoleCards = allRoleCards.CustomDeepCopy();
        }

        /// <summary>
        /// special constructor, for use with the JSON serializer
        /// </summary>
        /// <param name="dividedDeckOfInfectionCards"></param>
        /// <param name="activeInfectionCards"></param>
        /// <param name="deckOfDiscardedInfectionCards"></param>
        /// <param name="dividedDeckOfPlayerCards"></param>
        /// <param name="deckOfDiscardedPlayerCards"></param>
        /// <param name="playerCardsPerPlayerID"></param>
        [JsonConstructor]
        private PD_GameCards(
            List<List<PD_InfectionCard>> dividedDeckOfInfectionCards,
            List<PD_InfectionCard> activeInfectionCards,
            List<PD_InfectionCard> deckOfDiscardedInfectionCards,
            List<List<PD_PlayerCardBase>> dividedDeckOfPlayerCards,
            List<PD_PlayerCardBase> deckOfDiscardedPlayerCards,
            Dictionary<int, List<PD_PlayerCardBase>> playerCardsPerPlayerID,
            List<PD_Role_Card> inactiveRoleCards
            )
        {
            DividedDeckOfInfectionCards = dividedDeckOfInfectionCards;
            ActiveInfectionCards = activeInfectionCards;
            DeckOfDiscardedInfectionCards = deckOfDiscardedInfectionCards;
            DividedDeckOfPlayerCards = dividedDeckOfPlayerCards;
            DeckOfDiscardedPlayerCards = deckOfDiscardedPlayerCards;
            PlayerCardsPerPlayerID = playerCardsPerPlayerID;
            InactiveRoleCards = inactiveRoleCards;
        }

        private PD_GameCards(
            PD_GameCards gameCardsToCopy
            )
        {
            DividedDeckOfInfectionCards = gameCardsToCopy.DividedDeckOfInfectionCards.CustomDeepCopy();
            ActiveInfectionCards = gameCardsToCopy.ActiveInfectionCards.CustomDeepCopy();
            DeckOfDiscardedInfectionCards = gameCardsToCopy.DeckOfDiscardedInfectionCards.CustomDeepCopy();
            DividedDeckOfPlayerCards = gameCardsToCopy.DividedDeckOfPlayerCards.CustomDeepCopy();
            DeckOfDiscardedPlayerCards = gameCardsToCopy.DeckOfDiscardedPlayerCards.CustomDeepCopy();
            PlayerCardsPerPlayerID = gameCardsToCopy.PlayerCardsPerPlayerID.CustomDeepCopy();
            InactiveRoleCards = gameCardsToCopy.InactiveRoleCards.CustomDeepCopy();
        }
        #endregion

        public PD_GameCards GetCustomDeepCopy()
        {
            return new PD_GameCards(this);
        }
    }
}
