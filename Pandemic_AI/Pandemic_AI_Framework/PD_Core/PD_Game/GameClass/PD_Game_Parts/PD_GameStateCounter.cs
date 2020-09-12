using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameStateCounter :
        PD_GameParts_Base,
        IDescribable,
        ICustomDeepCopyable<PD_GameStateCounter>
    {
        public int number_of_players;
        public int player_action_index;
        public int player_index;
        public int turn_index;
        public Dictionary<int, int> disease_states;
        public int outbreaks_counter;
        public int epidemics_counter;

        public bool insufficient_disease_cubes_for_infection;
        public bool insufficient_player_cards_to_draw;
        public bool operations_expert_flight_used_this_turn;

        #region constructors
        // normal constructor
        public PD_GameStateCounter(
            int numberOfPlayers
            )
        {
            number_of_players = numberOfPlayers;
            player_index = 0;
            turn_index = 0;
            outbreaks_counter = 0;
            epidemics_counter = 0;
            insufficient_disease_cubes_for_infection = false;
            insufficient_player_cards_to_draw = false;
            operations_expert_flight_used_this_turn = false;
        }

        // custom private constructor for deep copy purposes!
        [JsonConstructor]
        public PD_GameStateCounter(
            int number_of_players,
            int player_action_index,
            int player_index,
            int turn_index,
            Dictionary<int, int> disease_states,
            int outbreaks_counter,
            int epidemics_counter,
            bool insufficient_disease_cubes_for_infection,
            bool insufficient_player_cards_to_draw,
            bool operations_expert_flight_used_this_turn
            )
        {
            this.number_of_players = number_of_players;
            this.player_action_index = player_action_index;
            this.player_index = player_index;
            this.turn_index = turn_index;
            this.disease_states = disease_states.CustomDeepCopy();
            this.outbreaks_counter = outbreaks_counter;
            this.epidemics_counter = epidemics_counter;
            this.insufficient_disease_cubes_for_infection
                = insufficient_disease_cubes_for_infection;
            this.insufficient_player_cards_to_draw = insufficient_player_cards_to_draw;
            this.operations_expert_flight_used_this_turn = operations_expert_flight_used_this_turn;
        }

        // private constructor for deepcopy purposes only
        private PD_GameStateCounter(
            PD_GameStateCounter gameStateCounterToCopy
            )
        {
            this.number_of_players = gameStateCounterToCopy.number_of_players;
            this.player_action_index = gameStateCounterToCopy.player_action_index;
            this.player_index = gameStateCounterToCopy.player_index;
            this.turn_index = gameStateCounterToCopy.turn_index;
            this.disease_states =
                gameStateCounterToCopy.disease_states.CustomDeepCopy();
            this.outbreaks_counter = gameStateCounterToCopy.outbreaks_counter;
            this.epidemics_counter = gameStateCounterToCopy.epidemics_counter;
            this.insufficient_disease_cubes_for_infection = gameStateCounterToCopy.insufficient_disease_cubes_for_infection;
            this.insufficient_player_cards_to_draw = gameStateCounterToCopy.insufficient_player_cards_to_draw;
            this.operations_expert_flight_used_this_turn = gameStateCounterToCopy.operations_expert_flight_used_this_turn;
        }

        public PD_GameStateCounter GetCustomDeepCopy()
        {
            return new PD_GameStateCounter(this);
        }
        #endregion

        #region various methods

        public void ResetPlayerActionIndex()
        {
            player_action_index = 0;
        }

        public void IncreasePlayerActionIndex()
        {
            player_action_index++;
        }

        public void IncreaseCurrentPlayerIndex()
        {
            player_index++;
            turn_index++;
            if (player_index >= number_of_players)
            {
                player_index = 0;
            }
        }

        public void InitializeCureMarkerStates()
        {
            disease_states = new Dictionary<int, int>() {
                {0, PD_DiseaseStates.Active},
                {1, PD_DiseaseStates.Active},
                {2, PD_DiseaseStates.Active},
                {3, PD_DiseaseStates.Active}
            };
        }

        public void CureDisease(int diseaseTypeToCure)
        {
            if (disease_states[diseaseTypeToCure] != 0)
            {
                throw new System.Exception("trying to cure a cured disease");
            }
            disease_states[diseaseTypeToCure] = PD_DiseaseStates.Cured;
        }

        public void EradicateDisease(int diseaseTypeToEradicate)
        {
            disease_states[diseaseTypeToEradicate] = PD_DiseaseStates.Eradicated;
        }

        public void ResetOutbreaksCounter()
        {
            outbreaks_counter = 0;
        }

        public void IncreaseOutbreaksCounter()
        {
            outbreaks_counter++;
        }

        public void ResetEpidemicsCounter()
        {
            epidemics_counter = 0;
        }

        public void IncreaseEpidemicsCounter()
        {
            epidemics_counter++;
        }

        public string GetDescription()
        {
            string report = "";
            report += "GAME STATE COUNTER:\n";

            var myProperties = this.GetType().GetProperties();

            for (int i = 0; i < myProperties.Length; i++)
            {
                var property = myProperties[i];
                if (property.GetValue(this) != null)
                {
                    report += String.Format(
                        "{0} : {1}\n",
                        property.Name,
                        property.GetValue(this).ToString()
                        );
                }
            }

            return report;
        }
        #endregion

        #region equalityOverride
        public bool Equals(PD_GameStateCounter other)
        {
            if (this.number_of_players != other.number_of_players)
            {
                return false;
            }
            if (this.player_action_index != other.player_action_index)
            {
                return false;
            }
            if (this.player_index != other.player_index)
            {
                return false;
            }
            if (this.turn_index != other.turn_index)
            {
                return false;
            }
            if (this.disease_states.Dictionary_Equals(other.disease_states) == false)
            {
                return false;
            }
            if (this.outbreaks_counter != other.outbreaks_counter)
            {
                return false;
            }
            if (this.epidemics_counter != other.epidemics_counter)
            {
                return false;
            }

            if (this.insufficient_disease_cubes_for_infection != other.insufficient_disease_cubes_for_infection)
            {
                return false;
            }
            if (this.insufficient_player_cards_to_draw != other.insufficient_player_cards_to_draw)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GameStateCounter other_game_state_counter)
            {
                return Equals(other_game_state_counter);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + number_of_players;
            hash = (hash * 13) + player_action_index;
            hash = (hash * 13) + player_index;
            hash = (hash * 13) + turn_index;
            hash = (hash * 13) + disease_states.Custom_HashCode();
            hash = (hash * 13) + outbreaks_counter;
            hash = (hash * 13) + epidemics_counter;

            hash = (hash * 13) + (insufficient_disease_cubes_for_infection ? 1 : 0);
            hash = (hash * 13) + (insufficient_player_cards_to_draw ? 1 : 0);
            hash = (hash * 13) + (operations_expert_flight_used_this_turn ? 1 : 0);

            return hash;
        }


        #endregion
    }
}