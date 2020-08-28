using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// a minimal representation of the state of pandemic,
    /// which is temporarily used for serialization and deserialization of the game.
    /// </summary>
    public enum PD_Mini__Disease_State
    {
        ACTIVE,
        CURED,
        ERADICATED
    }

    public enum PD_Mini__Player_Roles
    {
        UNDEFINED,
        Contingency_Planner,
        Operations_Expert,
        Dispatcher,
        Quarantine_Specialist,
        Researcher,
        Medic,
        Scientist
    }

    public enum PD_Mini__Game_States
    {
        IDLE,
        MAIN_PLAYER_ACTIONS,
        DISCARDING_DURING_MAIN_PLAYER_ACTIONS,
        DRAWING_NEW_PLAYER_CARDS,
        DISCARDING_AFTER_DRAWING,
        APPLYING_EPIDEMIC_CARDS,
        DRAWING_NEW_INFECTION_CARDS,
        APPLYING_INFECTION_CARDS,
        GAME_LOST_CARDS,
        GAME_LOST_DISEASE_CUBES,
        GAME_LOST_OUTBREAKS,
        GAME_WON
    }

    [Serializable]
    public class Pandemic_Mini_State : ICustomDeepCopyable<Pandemic_Mini_State>
    {
        #region properties

        // game - settings:
        public int number_of_players;
        public int game_difficulty;
        public int maximum_viable_outbreaks;
        public int maximum_player_hand_size;
        public Dictionary<int, int> initial_hand_size__per__number_of_players;
        public Dictionary<int, int> epidemic_cards__per__game_difficulty;
        public Dictionary<int, int> infection_rate__per__epidemics;

        // general - data
        public List<int> players;

        // player - roles
        public List<PD_Mini__Player_Roles> unassigned_player_roles;
        public PD_Mini__Player_Roles[] role__per__player;

        // map - data
        public int number_of_cities;
        public int[] cities;
        public int[][] neighbors__per__city;
        public bool[] research_station__per__city;

        public int[] infection_type__per__city;
        public int[] location__per__player;

        // game elements
        public int available_research_stations;
        public int[] available_infection_cubes__per__type;
        public int[][] infection_cubes__per__type__per__city;

        // state - counters:
        public int current_turn;
        public int current_player;
        public int current_player_action_index;

        public int number_of_outbreaks;
        public int number_of_epidemics;
        public PD_Mini__Disease_State[] disease_states;

        // flags
        public bool NotEnoughDiseaseCubesToCompleteAnInfection;
        public bool NotEnoughPlayerCardsToDraw;
        public bool operations_expert_flight_used_this_turn;

        // initial card - containers:
        public List<PD_InfectionCard> initial_container__infection_cards;
        public List<PD_EpidemicCard> initial_container__epidemic_cards;
        public List<PD_CityCard> initial_container__city_cards;

        public List<List<PD_InfectionCard>> divided_deck_of_infection_cards;
        public List<PD_InfectionCard> active_infection_cards;
        public List<PD_InfectionCard> deck_of_discarded_infection_cards;
        public List<List<PD_Card_Base>> divided_deck_of_player_cards;
        public List<PD_Card_Base> deck_of_discarded_player_cards;
        public Dictionary<int, List<PD_Card_Base>> player_cards__per__player;

        PD_Mini__Game_States game_state;

        #endregion


        public Pandemic_Mini_State GetCustomDeepCopy()
        {
            throw new NotImplementedException();
        }

        public static Pandemic_Mini_State From_Normal_State(PD_Game game)
        {
            Pandemic_Mini_State minified_game = new Pandemic_Mini_State();

            // game - settings:
            minified_game.number_of_players = game.GameStateCounter.NumberOfPlayers;
            minified_game.game_difficulty = game.GameSettings.GameDifficultyLevel;
            minified_game.maximum_viable_outbreaks = game.GameSettings.MaximumViableOutbreaks;
            minified_game.maximum_player_hand_size = game.GameSettings.MaximumNumberOfPlayerCardsPerPlayer;
            minified_game.initial_hand_size__per__number_of_players =
                game.GameSettings.NumberOfInitialCardsPerNumberOfPlayers.CustomDeepCopy();
            minified_game.epidemic_cards__per__game_difficulty =
                game.GameSettings.NumberOfEpidemicCardsPerDifficultyLevel.CustomDeepCopy();
            minified_game.infection_rate__per__epidemics =
                game.GameSettings.InfectionRatesPerEpidemicsCounter.CustomDeepCopy();

            // general - data
            minified_game.players = new List<int>();
            for (int p = 0; p < minified_game.number_of_players; p++) {
                minified_game.players.Add(p);
            }

            // player - roles
            minified_game.unassigned_player_roles = new List<PD_Mini__Player_Roles>();
            foreach (var role_card in game.Cards.InactiveRoleCards) {
                PD_Player_Roles role = role_card.Role;
                switch (role) {
                    case PD_Player_Roles.None:
                        minified_game.unassigned_player_roles.Add(PD_Mini__Player_Roles.UNDEFINED);
                        break;
                    case PD_Player_Roles.Medic:
                        minified_game.unassigned_player_roles.Add(PD_Mini__Player_Roles.Medic);
                        break;
                    case PD_Player_Roles.Operations_Expert:
                        minified_game.unassigned_player_roles.Add(PD_Mini__Player_Roles.Operations_Expert);
                        break;
                    case PD_Player_Roles.Researcher:
                        minified_game.unassigned_player_roles.Add(PD_Mini__Player_Roles.Researcher);
                        break;
                    case PD_Player_Roles.Scientist:
                        minified_game.unassigned_player_roles.Add(PD_Mini__Player_Roles.Scientist);
                        break;
                }
            }
            minified_game.role__per__player = new PD_Mini__Player_Roles[minified_game.number_of_players];
            foreach (int p in minified_game.players) {
                var game_player = game.Players[p];
                PD_Player_Roles game_player_role = game.GQ_Find_Player_Role(game_player);
                switch (game_player_role)
                {
                    case PD_Player_Roles.None:
                        minified_game.role__per__player[p] = PD_Mini__Player_Roles.UNDEFINED;
                        break;
                    case PD_Player_Roles.Medic:
                        minified_game.role__per__player[p] = PD_Mini__Player_Roles.Medic;
                        break;
                    case PD_Player_Roles.Operations_Expert:
                        minified_game.role__per__player[p] = PD_Mini__Player_Roles.Operations_Expert;
                        break;
                    case PD_Player_Roles.Researcher:
                        minified_game.role__per__player[p] = PD_Mini__Player_Roles.Researcher;
                        break;
                    case PD_Player_Roles.Scientist:
                        minified_game.role__per__player[p] = PD_Mini__Player_Roles.Scientist;
                        break;
                }
            }

            // map - data
            minified_game.number_of_cities = game.Map.Cities.Count;

            minified_game.cities = new int[minified_game.number_of_cities];
            for (int i = 0; i < minified_game.number_of_cities; i++)
            {
                minified_game.cities[i] = i;
            }

            minified_game.neighbors__per__city = new int[minified_game.number_of_cities][];
            foreach (int city in minified_game.cities)
            {
                var neighbors = game.Map.CityNeighbors_PerCityID[city];
                minified_game.neighbors__per__city[city] = new int[neighbors.Count];
                for (int n = 0; n < neighbors.Count; n++)
                {
                    var neighbor = neighbors[n];
                    minified_game.neighbors__per__city[city][n] = neighbor.ID;
                }
            }

            minified_game.research_station__per__city = new bool[minified_game.number_of_cities];
            foreach (int c in minified_game.cities)
            {
                var city = game.Map.Cities[c];
                if (game.GQ_Is_City_ResearchStation(city))
                {
                    minified_game.research_station__per__city[c] = true;
                }
                else
                {
                    minified_game.research_station__per__city[c] = false;
                }
            }


            minified_game.infection_type__per__city = new int[minified_game.number_of_cities];
            for (int c = 0; c < minified_game.number_of_cities; c++)
            {
                minified_game.infection_type__per__city[c] = game.Map.Cities[c].Type;
            }

            

            minified_game.location__per__player = new int[minified_game.number_of_players];

            return minified_game;
        }

    }
}
