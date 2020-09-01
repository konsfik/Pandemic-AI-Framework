using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public List<int> unassigned_player_roles;
        public int[] role__per__player;

        // map - data
        public int _map___number_of_cities;
        public List<int> _map___cities;
        public Dictionary<int, string> _map___name__per__city;
        public Dictionary<int, PD_Point> _map___position__per__city;
        public Dictionary<int, int> _map___infection_type__per__city;
        public Dictionary<int, List<int>> _map___neighbors__per__city;

        public Dictionary<int, bool> _map___research_station__per__city;
        public Dictionary<int, int> _map___location__per__player;
        public Dictionary<int, Dictionary<int, int>> _map___infection_cubes__per__type__per__city;

        // game elements
        public int available_research_stations;
        public Dictionary<int, int> available_infection_cubes__per__type;

        // state - counters:
        public int state_counter___current_state;

        public int state_counter___current_turn;
        public int state_counter___current_player;
        public int state_counter___current_player_action_index;

        public int state_counter___number_of_outbreaks;
        public int state_counter___number_of_epidemics;
        public Dictionary<int, int> state_counter___disease_states;

        // flags
        public bool NotEnoughDiseaseCubesToCompleteAnInfection;
        public bool NotEnoughPlayerCardsToDraw;
        public bool operations_expert_flight_used_this_turn;

        // initial card - containers:
        public List<List<int>> _cards___divided_deck_of_infection_cards;
        public List<int> _cards___active_infection_cards;
        public List<int> _cards___deck_of_discarded_infection_cards;
        public List<List<int>> _cards___divided_deck_of_player_cards;
        public List<int> _cards___deck_of_discarded_player_cards;
        public Dictionary<int, List<int>> _cards___player_cards__per__player;



        #endregion

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


    }
}
