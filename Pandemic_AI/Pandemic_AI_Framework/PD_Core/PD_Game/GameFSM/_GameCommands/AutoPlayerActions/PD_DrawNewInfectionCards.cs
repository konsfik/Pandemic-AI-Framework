﻿using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_DrawNewInfectionCards : 
        PD_GameAction_Base, 
        IEquatable<PD_DrawNewInfectionCards>,
        I_Auto_Action, 
        I_Player_Action
    {
        public int Player { get; protected set; }

        [JsonConstructor]
        public PD_DrawNewInfectionCards(
            int player
            )
        {
            this.Player = player;
        }

        // private constructor, for custom deep copy purposes only
        private PD_DrawNewInfectionCards(
            PD_DrawNewInfectionCards actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_DrawNewInfectionCards(this);
        }

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
            else if ((game.game_FSM.CurrentState is PD_GS_DrawingNewInfectionCards) == false)
            {
                throw new System.Exception("wrong state!");
            }
#endif
            int numberOfInfectionCardsToDraw =
                game.game_settings.infection_rate__per__number_of_epidemics[
                    game.game_state_counter.epidemics_counter];

            var infectionCards = new List<int>();

            for (int i = 0; i < numberOfInfectionCardsToDraw; i++)
            {
                infectionCards.Add(
                    game.cards.divided_deck_of_infection_cards.DrawLastElementOfLastSubList());
            }

            game.cards.active_infection_cards.AddRange(infectionCards);
        }

        public override string GetDescription()
        {
            return String.Format("{0}: DRAW Infection cards", Player.ToString());
        }

        #region equality overrides
        public bool Equals(PD_DrawNewInfectionCards other)
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
            if (other is PD_DrawNewInfectionCards other_action)
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
            if (other is PD_DrawNewInfectionCards other_action)
            {
                return Equals(other_action);
            }
            else {
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