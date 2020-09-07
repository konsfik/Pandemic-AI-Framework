using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ApplyEpidemicCard :
        PD_GameAction_Base,
        IEquatable<PD_ApplyEpidemicCard>,
        I_Auto_Action,
        I_Player_Action
    {
        public PD_Player Player { get; protected set; }

        #region constructors
        [JsonConstructor]
        public PD_ApplyEpidemicCard(
            PD_Player player
            )
        {
            this.Player = player.GetCustomDeepCopy();
        }

        // private constructor, for custom deep copy purposes only
        private PD_ApplyEpidemicCard(
            PD_ApplyEpidemicCard actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_ApplyEpidemicCard(this);
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
#if DEBUG
            if (Player != game.GQ_CurrentPlayer())
            {
                throw new System.Exception("wrong player!");
            }
            else if ((game.GameFSM.CurrentState is PD_GS_ApplyingEpidemicCard) == false)
            {
                throw new System.Exception("wrong state!");
            }
#endif
            // 0. discard the epidemic card...
            var allCards_InPlayerHand = game.Cards.PlayerCardsPerPlayerID[Player.ID];
            var epidemicCard = allCards_InPlayerHand.Find(
                x =>
                x.GetType() == typeof(PD_EpidemicCard)
                );
            game.Cards.PlayerCardsPerPlayerID[Player.ID].Remove(epidemicCard);
            game.Cards.DeckOfDiscardedPlayerCards.Add(epidemicCard);

            // 1. move the infection rate marker by one position
            game.GameStateCounter.IncreaseEpidemicsCounter();

            // 2. Infect: Draw the bottom card from the Infection Deck. 
            // Unless its disease color has been eradicated, put 3 disease cubes of that color on the named city.
            // If the city already has cubes of this color, do not add 3 cubes to it.
            // Instead, add just enough cubes so that it has 3 cubes of this color and then an outbreak of this disease occurs in the city(see Outbreaks below). 
            // Discard this card to the Infection Discard Pile.
            var cardFromBottom = game.Cards.DividedDeckOfInfectionCards.DrawFirstElementOfFirstSubList();

            int epidemicInfectionType = game.GQ_City_InfectionType(cardFromBottom.City);
            bool diseaseTypeEradicated = game.GQ_Is_Disease_Eradicated(epidemicInfectionType);

            if (diseaseTypeEradicated == false)
            {
                // actually apply the epidemic infection here...

                PD_InfectionReport initialReport = new PD_InfectionReport(
                    false, // not game setup...
                    Player,
                    cardFromBottom.City,
                    epidemicInfectionType,
                    3
                    );

                // apply infection of this city here
                PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                    game,
                    cardFromBottom.City,
                    3,
                    initialReport,
                    false
                    );

                game.InfectionReports.Add(finalReport);

                if (finalReport.FailureReason == InfectionFailureReasons.notEnoughDiseaseCubes)
                {
                    game.GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection = true;
                }
            }

            // put card in discarded infection cards pile
            game.Cards.DeckOfDiscardedInfectionCards.Add(cardFromBottom);

            // 3. intensify: Reshuffle just the cards in the Infection Discard Pile 
            // and place them on top of the Infection Deck.
            game.Cards.DeckOfDiscardedInfectionCards.Shuffle(randomness_provider);
            game.Cards.DividedDeckOfInfectionCards.Add(
                game.Cards.DeckOfDiscardedInfectionCards.DrawAll()
                );
        }

        public override string GetDescription()
        {
            return String.Format("{0}: EPIDEMIC.", Player.Name);
        }

        #region equality overrides
        public bool Equals(PD_ApplyEpidemicCard other)
        {
            if (this.Player != other.Player)
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
            if (other is PD_ApplyEpidemicCard other_action)
            {
                return Equals(other_action);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_ApplyEpidemicCard other_action)
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

            return hash;
        }





        #endregion
    }
}