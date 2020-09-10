using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_ApplyEpidemicCard :
        PD_GameAction_Base,
        IEquatable<PA_ApplyEpidemicCard>,
        I_Auto_Action,
        I_Player_Action
    {
        public int Player { get; protected set; }

        #region constructors
        [JsonConstructor]
        public PA_ApplyEpidemicCard(
            int player
            )
        {
            this.Player = player;
        }

        // private constructor, for custom deep copy purposes only
        private PA_ApplyEpidemicCard(
            PA_ApplyEpidemicCard actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PA_ApplyEpidemicCard(this);
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
            else if ((game.game_FSM.CurrentState is PD_GS_ApplyingEpidemicCard) == false)
            {
                throw new System.Exception("wrong state!");
            }
#endif
            // 0. discard the epidemic card...
            var allCards_InPlayerHand = game.cards.player_hand__per__player[Player];
            var epidemicCard = allCards_InPlayerHand.Find(
                x =>
                x >= 128
                );
            game.cards.player_hand__per__player[Player].Remove(epidemicCard);
            game.cards.deck_of_discarded_player_cards.Add(epidemicCard);

            // 1. move the infection rate marker by one position
            game.game_state_counter.IncreaseEpidemicsCounter();

            // 2. Infect: Draw the bottom card from the Infection Deck. 
            // Unless its disease color has been eradicated, put 3 disease cubes of that color on the named city.
            // If the city already has cubes of this color, do not add 3 cubes to it.
            // Instead, add just enough cubes so that it has 3 cubes of this color and then an outbreak of this disease occurs in the city(see Outbreaks below). 
            // Discard this card to the Infection Discard Pile.
            var cardFromBottom = game.cards.divided_deck_of_infection_cards.DrawFirstElementOfFirstSubList();

            int epidemicInfectionType = game.GQ_City_InfectionType(cardFromBottom);
            bool diseaseTypeEradicated = game.GQ_Is_Disease_Eradicated(epidemicInfectionType);

            if (diseaseTypeEradicated == false)
            {
                // actually apply the epidemic infection here...

                PD_InfectionReport initialReport = new PD_InfectionReport(
                    false, // not game setup...
                    Player,
                    cardFromBottom,
                    epidemicInfectionType,
                    3
                    );

                // apply infection of this city here
                PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                    game,
                    cardFromBottom,
                    3,
                    initialReport,
                    false
                    );

                game.InfectionReports.Add(finalReport);

                if (finalReport.FailureReason == InfectionFailureReasons.notEnoughDiseaseCubes)
                {
                    game.game_state_counter.insufficient_disease_cubes_for_infection = true;
                }
            }

            // put card in discarded infection cards pile
            game.cards.deck_of_discarded_infection_cards.Add(cardFromBottom);

            // 3. intensify: Reshuffle just the cards in the Infection Discard Pile 
            // and place them on top of the Infection Deck.
            game.cards.deck_of_discarded_infection_cards.Shuffle(randomness_provider);
            game.cards.divided_deck_of_infection_cards.Add(
                game.cards.deck_of_discarded_infection_cards.DrawAll()
                );
        }

        public override string GetDescription()
        {
            return String.Format("{0}: EPIDEMIC.", Player.ToString());
        }

        #region equality overrides
        public bool Equals(PA_ApplyEpidemicCard other)
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
            if (other is PA_ApplyEpidemicCard other_action)
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
            if (otherObject is PA_ApplyEpidemicCard other_action)
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