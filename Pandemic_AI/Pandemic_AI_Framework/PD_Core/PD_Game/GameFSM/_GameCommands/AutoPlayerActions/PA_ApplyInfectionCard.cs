using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_ApplyInfectionCard : 
        PD_Action, 
        IEquatable<PA_ApplyInfectionCard>,
        I_Auto_Action, 
        I_Player_Action
    {
        public int Player { get; protected set; }

        public int InfectionCardToApply { get; private set; }

        [JsonConstructor]
        public PA_ApplyInfectionCard(
            int player,
            int infectionCardToApply
            )
        {
            this.Player = player;
            this.InfectionCardToApply = infectionCardToApply;
        }

        // private constructor, for custom deep copy purposes only
        private PA_ApplyInfectionCard(
            PA_ApplyInfectionCard actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
            this.InfectionCardToApply = actionToCopy.InfectionCardToApply;
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
            else if ((game.game_FSM.CurrentState is PD_GS_ApplyingInfectionCards) == false)
            {
                throw new System.Exception("wrong state!");
            }
#endif
            int infectionType = game.GQ_City_InfectionType(InfectionCardToApply);
            bool diseaseEradicated = game.GQ_Is_Disease_Eradicated(infectionType);

            if (diseaseEradicated == false)
            {
                PD_InfectionReport initialReport = new PD_InfectionReport(
                    false, // not game setup
                    Player,
                    InfectionCardToApply,
                    infectionType,
                    1
                    );

                PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                    game,
                    InfectionCardToApply,
                    1,
                    initialReport,
                    false
                    );

                game.InfectionReports.Add(finalReport);

                if (finalReport.FailureReason == InfectionFailureReasons.notEnoughDiseaseCubes)
                {
                    game.game_state_counter.insufficient_disease_cubes_for_infection = true;
                }
            }

            // remove the infection card from the active infection cards pile
            game.cards.active_infection_cards.Remove(InfectionCardToApply);
            game.cards.deck_of_discarded_infection_cards.Add(InfectionCardToApply);
        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PA_ApplyInfectionCard(this);
        }

        public override string GetDescription()
        {
            return Player.ToString() + ": INFECTION " + InfectionCardToApply.ToString();
        }

        #region equality overrides

        public bool Equals(PA_ApplyInfectionCard other)
        {
            if (this.Player != other.Player)
            {
                return false;
            }
            else if (this.InfectionCardToApply != other.InfectionCardToApply)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(PD_Action other)
        {
            if (other is PA_ApplyInfectionCard other_action)
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
            if (other is PA_ApplyInfectionCard other_action)
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
            hash = hash * 31 + InfectionCardToApply.GetHashCode();

            return hash;
        }

        #endregion
    }
}