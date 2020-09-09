using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    ////////////////////////////////////////////////////////////////////////////////////
    /// CARDS' REPRESENTATION
    /// - Infection cards are represented as integers that range from 0 to 47
    ///     (same as the cities that they relate to)
    /// - City cards are represented as integers that range from 0 to 47
    ///     (same as the cities that they relate to)
    /// - Epidemic cards are represented as integers that range from 128 to 134
    /// - Special event cards are represented as integers that range from 160 to ...
    ////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// a minimal representation of the state of pandemic,
    /// which is temporarily used for serialization and deserialization of the game.
    /// </summary>

    [Serializable]
    public class PD_MiniGame : ICustomDeepCopyable<PD_MiniGame>
    {
        #region properties

        public long unique_id;

        // game - settings:
        public int settings___number_of_players;
        public int settings___game_difficulty;
        public int settings___maximum_viable_outbreaks;
        public int settings___maximum_player_hand_size;
        public Dictionary<int, int> settings___initial_hand_size__per__number_of_players;
        public Dictionary<int, int> settings___epidemic_cards__per__game_difficulty;
        public Dictionary<int, int> settings___infection_rate__per__epidemics;

        // general - data
        public List<int> players;

        // player - roles
        public int[] role__per__player;

        // map - data
        public int map___number_of_cities;
        public List<int> map___cities;
        public Dictionary<int, string> map___name__per__city;
        public Dictionary<int, PD_Point> map___position__per__city;
        public Dictionary<int, int> map___infection_type__per__city;
        public Dictionary<int, List<int>> map___neighbors__per__city;

        public Dictionary<int, bool> map_elements___research_station__per__city;
        public Dictionary<int, int> map_elements___location__per__player;
        public Dictionary<int, Dictionary<int, int>> map_elements___infection_cubes__per__type__per__city;

        // game elements
        public int map_elements___available_research_stations;
        public Dictionary<int, int> map_elements___available_infection_cubes__per__type;

        // state - counters:
        public int state_counter___current_state;

        public int state_counter___current_turn;
        public int state_counter___current_player;
        public int state_counter___current_player_action_index;

        public int state_counter___number_of_outbreaks;
        public int state_counter___number_of_epidemics;
        public Dictionary<int, int> state_counter___disease_states;

        // flags
        public bool flag___insufficient_disease_cubes_during_infection;
        public bool flag___insufficient_player_cards_to_draw;
        public bool flag___operations_expert_flight_used_this_turn;

        // card - containers:
        public List<List<int>> cards___divided_deck_of_infection_cards;
        public List<int> cards___active_infection_cards;
        public List<int> cards___deck_of_discarded_infection_cards;
        public List<List<int>> cards___divided_deck_of_player_cards;
        public List<int> cards___deck_of_discarded_player_cards;
        public Dictionary<int, List<int>> cards___player_cards__per__player;

        #endregion

        [JsonConstructor]
        public PD_MiniGame(
            long unique_id,

            // game - settings:
            int settings___number_of_players,
            int settings___game_difficulty,
            int settings___maximum_viable_outbreaks,
            int settings___maximum_player_hand_size,
            Dictionary<int, int> settings___initial_hand_size__per__number_of_players,
            Dictionary<int, int> settings___epidemic_cards__per__game_difficulty,
            Dictionary<int, int> settings___infection_rate__per__epidemics,

            // general - data
            List<int> players,

            // player - roles
            int[] role__per__player,

            // map
            int map___number_of_cities,
            List<int> map___cities,
            Dictionary<int, string> map___name__per__city,
            Dictionary<int, PD_Point> map___position__per__city,
            Dictionary<int, int> map___infection_type__per__city,
            Dictionary<int, List<int>> map___neighbors__per__city,

            // map - elements
            Dictionary<int, bool> map_elements___research_station__per__city,
            Dictionary<int, int> map_elements___location__per__player,
            Dictionary<int, Dictionary<int, int>> map_elements___infection_cubes__per__type__per__city,
            int map_elements___available_research_stations,
            Dictionary<int, int> map_elements___available_infection_cubes__per__type,

            // state - counters:
            int state_counter___current_state,

            int state_counter___current_turn,
            int state_counter___current_player,
            int state_counter___current_player_action_index,

            int state_counter___number_of_outbreaks,
            int state_counter___number_of_epidemics,
            Dictionary<int, int> state_counter___disease_states,

            // flags
            bool flag___insufficient_disease_cubes_during_infection,
            bool flag___insufficient_player_cards_to_draw,
            bool flag___operations_expert_flight_used_this_turn,

            // initial card - containers:
            List<List<int>> cards___divided_deck_of_infection_cards,
            List<int> cards___active_infection_cards,
            List<int> cards___deck_of_discarded_infection_cards,
            List<List<int>> cards___divided_deck_of_player_cards,
            List<int> cards___deck_of_discarded_player_cards,
            Dictionary<int, List<int>> cards___player_cards__per__player

            )
        {
            this.unique_id = unique_id;

            // game - settings:
            this.settings___number_of_players = settings___number_of_players;
            this.settings___game_difficulty = settings___game_difficulty;
            this.settings___maximum_viable_outbreaks = settings___maximum_viable_outbreaks;
            this.settings___maximum_player_hand_size = settings___maximum_player_hand_size;
            this.settings___initial_hand_size__per__number_of_players
                = settings___initial_hand_size__per__number_of_players.CustomDeepCopy();
            this.settings___epidemic_cards__per__game_difficulty
                = settings___epidemic_cards__per__game_difficulty.CustomDeepCopy();
            this.settings___infection_rate__per__epidemics
                = settings___infection_rate__per__epidemics;

            // general - data
            this.players = players.CustomDeepCopy();

            // player - roles
            this.role__per__player = role__per__player.CustomDeepCopy();

            // map - data
            this.map___number_of_cities = map___number_of_cities;
            this.map___cities = map___cities.CustomDeepCopy();
            this.map___name__per__city = map___name__per__city.CustomDeepCopy();
            this.map___position__per__city
                = map___position__per__city.CustomDeepCopy();
            this.map___infection_type__per__city
                = map___infection_type__per__city.CustomDeepCopy();
            this.map___neighbors__per__city
                = map___neighbors__per__city.CustomDeepCopy();
            this.map_elements___research_station__per__city
                = map_elements___research_station__per__city.CustomDeepCopy();
            this.map_elements___location__per__player
                = map_elements___location__per__player.CustomDeepCopy();
            this.map_elements___infection_cubes__per__type__per__city
                = map_elements___infection_cubes__per__type__per__city.CustomDeepCopy();

            // game elements
            this.map_elements___available_research_stations 
                = map_elements___available_research_stations;
            this.map_elements___available_infection_cubes__per__type 
                = map_elements___available_infection_cubes__per__type;

            // state - counters:
            this.state_counter___current_state 
                = state_counter___current_state;

            this.state_counter___current_turn 
                = state_counter___current_turn;
            this.state_counter___current_player 
                = state_counter___current_player;
            this.state_counter___current_player_action_index
                = state_counter___current_player_action_index;

            this.state_counter___number_of_outbreaks 
                = state_counter___number_of_outbreaks;
            this.state_counter___number_of_epidemics 
                = state_counter___number_of_epidemics;
            this.state_counter___disease_states 
                = state_counter___disease_states.CustomDeepCopy();

            // flags
            this.flag___insufficient_disease_cubes_during_infection
                = flag___insufficient_disease_cubes_during_infection;
            this.flag___insufficient_player_cards_to_draw
                = flag___insufficient_player_cards_to_draw;
            this.flag___operations_expert_flight_used_this_turn
                = flag___operations_expert_flight_used_this_turn;

            // initial card - containers:
            this.cards___divided_deck_of_infection_cards
                = cards___divided_deck_of_infection_cards.CustomDeepCopy();
            this.cards___active_infection_cards
                = cards___active_infection_cards.CustomDeepCopy();
            this.cards___deck_of_discarded_infection_cards
                = cards___deck_of_discarded_infection_cards.CustomDeepCopy();
            this.cards___divided_deck_of_player_cards
                = cards___divided_deck_of_player_cards.CustomDeepCopy();
            this.cards___deck_of_discarded_player_cards
                = cards___deck_of_discarded_player_cards.CustomDeepCopy();
            this.cards___player_cards__per__player
                = cards___player_cards__per__player.CustomDeepCopy();
        }

        public PD_MiniGame()
        {

        }

        private PD_MiniGame(PD_MiniGame mini_game_to_copy)
        {
            throw new NotImplementedException();
        }

        public PD_MiniGame GetCustomDeepCopy()
        {
            return new PD_MiniGame(this);
        }

        #region equality overrides
        public bool Equals(PD_MiniGame other)
        {
            if (this.unique_id != other.unique_id)
            {
                return false;
            }

            // game - settings:
            else if (this.settings___number_of_players
                != other.settings___number_of_players) return false;
            else if (this.settings___game_difficulty
                != other.settings___game_difficulty) return false;
            else if (this.settings___maximum_viable_outbreaks
                != other.settings___maximum_viable_outbreaks) return false;
            else if (this.settings___maximum_player_hand_size
                != other.settings___maximum_player_hand_size) return false;
            else if (this.settings___initial_hand_size__per__number_of_players
                .Dictionary_Equals(other.settings___initial_hand_size__per__number_of_players) == false) return false;
            else if (this.settings___epidemic_cards__per__game_difficulty
                .Dictionary_Equals(other.settings___epidemic_cards__per__game_difficulty) == false) return false;
            else if (this.settings___infection_rate__per__epidemics
                .Dictionary_Equals(other.settings___infection_rate__per__epidemics) == false) return false;

            // general - data
            else if (this.players
                .List_Equals(other.players) == false) return false;

            // player - roles
            else if (this.role__per__player
                .Array_Equal_S(other.role__per__player) == false) return false;

            // map - data
            else if (this.map___number_of_cities
                != other.map___number_of_cities) return false;
            else if (this.map___cities
                .List_Equals(other.map___cities) == false) return false;
            else if (this.map___name__per__city
                .Dictionary_Equals(other.map___name__per__city) == false) return false;
            else if (this.map___position__per__city
                .Dictionary_Equals(other.map___position__per__city) == false) return false;
            else if (this.map___infection_type__per__city
                .Dictionary_Equal_S(other.map___infection_type__per__city) == false) return false;
            else if (this.map___neighbors__per__city
                .Dictionary_Equals(other.map___neighbors__per__city) == false) return false;

            else if (this.map_elements___research_station__per__city
                .Dictionary_Equal_S(other.map_elements___research_station__per__city) == false) return false;
            else if (this.map_elements___location__per__player
                .Dictionary_Equal_S(other.map_elements___location__per__player) == false) return false;
            else if (this.map_elements___infection_cubes__per__type__per__city
                .Dictionary_Equals(other.map_elements___infection_cubes__per__type__per__city) == false) return false;

            // game elements
            else if (this.map_elements___available_research_stations
                != other.map_elements___available_research_stations) return false;
            else if (this.map_elements___available_infection_cubes__per__type
                .Dictionary_Equal_S(other.map_elements___available_infection_cubes__per__type) == false) return false;

            // state counters
            else if (this.state_counter___current_state
                != other.state_counter___current_state) return false;

            else if (this.state_counter___current_turn
                != other.state_counter___current_turn) return false;
            else if (this.state_counter___current_player
                != other.state_counter___current_player) return false;
            else if (this.state_counter___current_player_action_index
                != other.state_counter___current_player_action_index) return false;

            else if (this.state_counter___number_of_outbreaks
                != other.state_counter___number_of_outbreaks) return false;
            else if (this.state_counter___number_of_epidemics
                != other.state_counter___number_of_epidemics) return false;
            else if (this.state_counter___disease_states
                .Dictionary_Equal_S(other.state_counter___disease_states) == false) return false;

            // flags
            else if (this.flag___insufficient_disease_cubes_during_infection
                != other.flag___insufficient_disease_cubes_during_infection) return false;
            else if (this.flag___insufficient_player_cards_to_draw
                != other.flag___insufficient_player_cards_to_draw) return false;
            else if (this.flag___operations_expert_flight_used_this_turn
                != other.flag___operations_expert_flight_used_this_turn) return false;

            // initial card - containers:
            else if (this.cards___divided_deck_of_infection_cards
                .List_Equals(other.cards___divided_deck_of_infection_cards) == false) return false;
            else if (this.cards___active_infection_cards
                .List_Equals(other.cards___active_infection_cards) == false) return false;
            else if (this.cards___deck_of_discarded_infection_cards
                .List_Equals(other.cards___deck_of_discarded_infection_cards) == false) return false;
            else if (this.cards___divided_deck_of_player_cards
                .List_Equals(other.cards___divided_deck_of_player_cards) == false) return false;
            else if (this.cards___deck_of_discarded_player_cards
                .List_Equals(other.cards___deck_of_discarded_player_cards) == false) return false;
            else if (this.cards___player_cards__per__player
                .Dictionary_Equals(other.cards___player_cards__per__player) == false) return false;

            else return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_MiniGame other_mini_game)
            {
                return Equals(other_mini_game);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 7;

            hash = (hash * 13) + (int)unique_id;

            // settings
            hash = (hash * 13) + settings___number_of_players;
            hash = (hash * 13) + settings___game_difficulty;
            hash = (hash * 13) + settings___maximum_viable_outbreaks;
            hash = (hash * 13) + settings___maximum_player_hand_size;
            hash = (hash * 13) + settings___initial_hand_size__per__number_of_players.Custom_HashCode();
            hash = (hash * 13) + settings___epidemic_cards__per__game_difficulty.Custom_HashCode();
            hash = (hash * 13) + settings___infection_rate__per__epidemics.Custom_HashCode();

            hash = (hash * 13) + players.Custom_HashCode();

            hash = (hash * 13) + role__per__player.Custom_HashCode();

            // map - data
            hash = (hash * 13) + map___number_of_cities;
            hash = (hash * 13) + map___cities.Custom_HashCode();
            hash = (hash * 13) + map___name__per__city.Custom_HashCode();
            hash = (hash * 13) + map___position__per__city.Custom_HashCode();
            hash = (hash * 13) + map___infection_type__per__city.Custom_HashCode();
            hash = (hash * 13) + map___neighbors__per__city.Custom_HashCode();

            hash = (hash * 13) + map_elements___research_station__per__city.Custom_HashCode();
            hash = (hash * 13) + map_elements___location__per__player.Custom_HashCode();
            hash = (hash * 13) + map_elements___infection_cubes__per__type__per__city.Custom_HashCode();

            // game elements
            hash = (hash * 13) + map_elements___available_research_stations;
            hash = (hash * 13) + map_elements___available_infection_cubes__per__type.Custom_HashCode();

            // state - counters:
            hash = (hash * 13) + state_counter___current_state;

            hash = (hash * 13) + state_counter___current_turn;
            hash = (hash * 13) + state_counter___current_player;
            hash = (hash * 13) + state_counter___current_player_action_index;

            hash = (hash * 13) + state_counter___number_of_outbreaks;
            hash = (hash * 13) + state_counter___number_of_epidemics;
            hash = (hash * 13) + state_counter___disease_states.Custom_HashCode();

            // flags
            hash = (hash * 13) + cards___divided_deck_of_infection_cards.Custom_HashCode();
            hash = (hash * 13) + cards___active_infection_cards.Custom_HashCode();
            hash = (hash * 13) + cards___deck_of_discarded_infection_cards.Custom_HashCode();
            hash = (hash * 13) + cards___divided_deck_of_player_cards.Custom_HashCode();
            hash = (hash * 13) + cards___deck_of_discarded_player_cards.Custom_HashCode();
            hash = (hash * 13) + cards___player_cards__per__player.Custom_HashCode();


            return hash;
        }



        public static bool operator ==(PD_MiniGame mg1, PD_MiniGame mg2)
        {
            if (Object.ReferenceEquals(mg1, null))
            {
                if (Object.ReferenceEquals(mg2, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // c1 is not null
            {
                if (Object.ReferenceEquals(mg2, null)) // c2 is null
                {
                    return false;
                }
            }
            return mg1.Equals(mg2);
        }

        public static bool operator !=(PD_MiniGame mg1, PD_MiniGame mg2)
        {
            return !(mg1 == mg2);
        }
        #endregion
    }
}
