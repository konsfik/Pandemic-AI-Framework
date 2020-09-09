using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    public static class PD_State_Converter
    {

        public static string To_Json_String(
            this PD_MiniGame mini_game,
            Newtonsoft.Json.Formatting formatting,
            Newtonsoft.Json.TypeNameHandling type_name_handling,
            Newtonsoft.Json.PreserveReferencesHandling preserve_references_handling
            )
        {
            return JsonConvert.SerializeObject(
                mini_game,
                formatting,
                new JsonSerializerSettings
                {
                    TypeNameHandling = type_name_handling,
                    PreserveReferencesHandling = preserve_references_handling
                }
            );
        }

        public static PD_MiniGame Convert_To_MiniGame(
            this PD_Game game
            )
        {
            PD_MiniGame mini_game = new PD_MiniGame();

            // game - id
            mini_game.unique_id = game.unique_id;

            // game - settings:
            mini_game.settings___number_of_players = game.game_state_counter.number_of_players;
            mini_game.settings___game_difficulty = game.game_settings.game_difficulty_level;
            mini_game.settings___maximum_viable_outbreaks = game.game_settings.maximum_viable_outbreaks;
            mini_game.settings___maximum_player_hand_size = game.game_settings.maximum_player_hand_size;
            mini_game.settings___initial_hand_size__per__number_of_players =
                game.game_settings.initial_hand_size__per__number_of_players.CustomDeepCopy();
            mini_game.settings___epidemic_cards__per__game_difficulty =
                game.game_settings.number_of_epidemic_cards__per__difficulty_level.CustomDeepCopy();
            mini_game.settings___infection_rate__per__epidemics =
                game.game_settings.infection_rate__per__number_of_epidemics.CustomDeepCopy();

            // general - data
            mini_game.players = new List<int>();
            for (int p = 0; p < mini_game.settings___number_of_players; p++)
            {
                mini_game.players.Add(p);
            }

            // player - roles
            mini_game.role__per__player = new int[mini_game.settings___number_of_players];
            foreach (int p in mini_game.players)
            {
                var game_player = game.players[p];
                int game_player_role = game.GQ_Player_Role(game_player);
                switch (game_player_role)
                {
                    case PD_Player_Roles.None:
                        mini_game.role__per__player[p] = PD_MiniGame__PlayerRole.UNDEFINED;
                        break;
                    case PD_Player_Roles.Medic:
                        mini_game.role__per__player[p] = PD_MiniGame__PlayerRole.Medic;
                        break;
                    case PD_Player_Roles.Operations_Expert:
                        mini_game.role__per__player[p] = PD_MiniGame__PlayerRole.Operations_Expert;
                        break;
                    case PD_Player_Roles.Researcher:
                        mini_game.role__per__player[p] = PD_MiniGame__PlayerRole.Researcher;
                        break;
                    case PD_Player_Roles.Scientist:
                        mini_game.role__per__player[p] = PD_MiniGame__PlayerRole.Scientist;
                        break;
                }
            }

            //////////////////////////////////
            // map - data
            //////////////////////////////////

            // number_of_cities
            mini_game.map___number_of_cities = game.map.number_of_cities;
            mini_game.map___cities = new List<int>();
            mini_game.map___name__per__city = new Dictionary<int, string>();
            mini_game.map___position__per__city = new Dictionary<int, PD_Point>();
            mini_game.map___infection_type__per__city = new Dictionary<int, int>();
            mini_game.map___neighbors__per__city = new Dictionary<int, List<int>>();
            mini_game.map_elements___research_station__per__city = new Dictionary<int, bool>();
            mini_game.map_elements___location__per__player = new Dictionary<int, int>();

            foreach (int c in game.map.cities)
            {
                // cities
                mini_game.map___cities.Add(c);
                // name__per__city
                mini_game.map___name__per__city.Add(
                    c,
                    game.map.name__per__city[c]
                    );
                // position__per__city
                mini_game.map___position__per__city.Add(
                    c,
                    game.map.position__per__city[c].GetCustomDeepCopy()
                    );
                // infection_type__per__city
                mini_game.map___infection_type__per__city.Add(
                    c,
                    game.map.infection_type__per__city[c]
                    );
                // neighbors__per__city
                List<int> neighbors = new List<int>();
                foreach (int nei in game.map.neighbors__per__city[c])
                {
                    neighbors.Add(nei);
                }
                mini_game.map___neighbors__per__city.Add(
                    c,
                    neighbors
                    );
                // research_station__per__city
                if (game.GQ_Is_City_ResearchStation(game.map.cities[c]))
                {
                    mini_game.map_elements___research_station__per__city.Add(c, true);
                }
                else
                {
                    mini_game.map_elements___research_station__per__city.Add(c, false);
                }
            }
            // location__per__player
            foreach (int p in mini_game.players)
            {
                var game_player = game.players[p];
                var game_player_location = game.GQ_PlayerLocation(game_player);
                mini_game.map_elements___location__per__player[p] = game_player_location;
            }
            // infection_cubes__per__type__per__city
            mini_game.map_elements___infection_cubes__per__type__per__city =
                new Dictionary<int, Dictionary<int, int>>();
            foreach (int c in mini_game.map___cities)
            {
                mini_game.map_elements___infection_cubes__per__type__per__city.Add(
                    c,
                    new Dictionary<int, int>()
                    );
            }
            foreach (int city in game.map.cities)
            {
                for (int t = 0; t < 4; t++)
                {
                    int num_cubes__this_type__that_city
                        = game.GQ_InfectionCubes_OfType_OnCity(city, t);
                    mini_game.map_elements___infection_cubes__per__type__per__city[city].Add(
                        t,
                        num_cubes__this_type__that_city
                        );
                }
            }

            /////////////////////////////////////////////////
            // game elements
            /////////////////////////////////////////////////

            // availablee research stations
            mini_game.map_elements___available_research_stations
                = game.map_elements.inactive_research_stations;
            // available infection cubes per type
            mini_game.map_elements___available_infection_cubes__per__type = new Dictionary<int, int>();
            for (int t = 0; t < 4; t++)
            {
                mini_game.map_elements___available_infection_cubes__per__type.Add(
                    t, game.Num_InactiveInfectionCubes_OfType(t)
                    );
            }


            /////////////////////////////////////////////////
            // state counters
            /////////////////////////////////////////////////

            if (game.game_FSM.CurrentState is PD_GS_Idle)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.IDLE;
            }
            else if (game.game_FSM.CurrentState is PD_GS_ApplyingMainPlayerActions)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.MAIN_PLAYER_ACTIONS;
            }
            else if (game.game_FSM.CurrentState is PD_GS_Discarding_DuringMainPlayerActions)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DISCARDING_DURING_PLAY;
            }
            else if (game.game_FSM.CurrentState is PD_GS_DrawingNewPlayerCards)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DRAWING_NEW_PLAYER_CARDS;
            }
            else if (game.game_FSM.CurrentState is PD_GS_Discarding_AfterDrawing)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DISCARDING_AFTER_DRAWING;
            }
            else if (game.game_FSM.CurrentState is PD_GS_ApplyingEpidemicCard)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.APPLYING_EPIDEMIC_CARDS;
            }
            else if (game.game_FSM.CurrentState is PD_GS_DrawingNewInfectionCards)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DRAWING_NEW_INFECTION_CARDS;
            }
            else if (game.game_FSM.CurrentState is PD_GS_ApplyingInfectionCards)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.APPLYING_INFECTION_CARDS;
            }
            else if (game.game_FSM.CurrentState is PD_GS_GameLost)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.GAME_LOST;
            }
            else if (game.game_FSM.CurrentState is PD_GS_GameWon)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.GAME_WON;
            }
            else
            {
                throw new System.Exception("state not found!");
            }

            mini_game.state_counter___current_turn = game.game_state_counter.turn_index;
            mini_game.state_counter___current_player = game.game_state_counter.player_index;
            mini_game.state_counter___current_player_action_index = game.game_state_counter.player_action_index;
            mini_game.state_counter___number_of_outbreaks = game.game_state_counter.outbreaks_counter;
            mini_game.state_counter___number_of_epidemics = game.game_state_counter.epidemics_counter;
            mini_game.state_counter___disease_states = new Dictionary<int, int>();
            for (int t = 0; t < 4; t++)
            {
                if (game.game_state_counter.disease_states[t] == 0)
                {
                    mini_game.state_counter___disease_states.Add(t, PD_MiniGame__DiseaseState.ACTIVE);
                }
                else if (game.game_state_counter.disease_states[t] == 1)
                {
                    mini_game.state_counter___disease_states.Add(t, PD_MiniGame__DiseaseState.CURED);
                }
                else if (game.game_state_counter.disease_states[t] == 2)
                {
                    mini_game.state_counter___disease_states.Add(t, PD_MiniGame__DiseaseState.ERADICATED);
                }
            }


            /////////////////////////////////////////////////
            // cards
            /////////////////////////////////////////////////

            // divided_deck_of_infection_cards
            mini_game.cards___divided_deck_of_infection_cards = game.cards.divided_deck_of_infection_cards.CustomDeepCopy();

            // active_infection_cards
            mini_game.cards___active_infection_cards = game.cards.active_infection_cards.CustomDeepCopy();

            // deck_of_discarded_infection_cards
            mini_game.cards___deck_of_discarded_infection_cards = game.cards.deck_of_discarded_infection_cards.CustomDeepCopy();

            // divided_deck_of_player_cards
            mini_game.cards___divided_deck_of_player_cards = game.cards.divided_deck_of_player_cards.CustomDeepCopy();

            // deck_of_discarded_player_cards
            mini_game.cards___deck_of_discarded_player_cards = game.cards.deck_of_discarded_player_cards.CustomDeepCopy();

            // player cards per player
            mini_game.cards___player_cards__per__player = game.cards.player_hand__per__player.CustomDeepCopy();



            return mini_game;
        }

        public static PD_Game Convert_To_Game(
            this PD_MiniGame mini_game
            )
        {
            // game settings...
            int game_difficulty_level = mini_game.settings___game_difficulty;
            PD_GameSettings GAME_SETTINGS = new PD_GameSettings(game_difficulty_level);

            // game FSM
            PD_GameFSM GAME_FSM;
            switch (mini_game.state_counter___current_state)
            {
                case PD_MiniGame__GameState.IDLE:
                    GAME_FSM = new PD_GameFSM(new PD_GS_Idle());
                    break;
                case PD_MiniGame__GameState.MAIN_PLAYER_ACTIONS:
                    GAME_FSM = new PD_GameFSM(new PD_GS_ApplyingMainPlayerActions());
                    break;
                case PD_MiniGame__GameState.DISCARDING_DURING_PLAY:
                    GAME_FSM = new PD_GameFSM(new PD_GS_Discarding_DuringMainPlayerActions());
                    break;
                case PD_MiniGame__GameState.DRAWING_NEW_PLAYER_CARDS:
                    GAME_FSM = new PD_GameFSM(new PD_GS_DrawingNewPlayerCards());
                    break;
                case PD_MiniGame__GameState.DISCARDING_AFTER_DRAWING:
                    GAME_FSM = new PD_GameFSM(new PD_GS_Discarding_AfterDrawing());
                    break;
                case PD_MiniGame__GameState.APPLYING_EPIDEMIC_CARDS:
                    GAME_FSM = new PD_GameFSM(new PD_GS_ApplyingEpidemicCard());
                    break;
                case PD_MiniGame__GameState.DRAWING_NEW_INFECTION_CARDS:
                    GAME_FSM = new PD_GameFSM(new PD_GS_DrawingNewInfectionCards());
                    break;
                case PD_MiniGame__GameState.APPLYING_INFECTION_CARDS:
                    GAME_FSM = new PD_GameFSM(new PD_GS_ApplyingInfectionCards());
                    break;
                case PD_MiniGame__GameState.GAME_LOST:
                    GAME_FSM = new PD_GameFSM(new PD_GS_GameLost());
                    break;
                case PD_MiniGame__GameState.GAME_WON:
                    GAME_FSM = new PD_GameFSM(new PD_GS_GameWon());
                    break;
                default:
                    throw new System.Exception("incorrect state!");
            }

            // game state counter...
            PD_GameStateCounter GAME_STATE_COUNTER = new PD_GameStateCounter(
                mini_game.settings___number_of_players,
                mini_game.state_counter___current_player_action_index,
                mini_game.state_counter___current_player,
                mini_game.state_counter___current_turn,
                mini_game.state_counter___disease_states.CustomDeepCopy(),
                mini_game.state_counter___number_of_outbreaks,
                mini_game.state_counter___number_of_epidemics,
                mini_game.flag___insufficient_disease_cubes_during_infection,
                mini_game.flag___insufficient_player_cards_to_draw
                );

            ////////////////////////////////////////////////////////////
            /// players
            ////////////////////////////////////////////////////////////
            List<int> PLAYERS = mini_game.players.CustomDeepCopy();

            ////////////////////////////////////////////////////////////
            /// map
            ////////////////////////////////////////////////////////////


            // map
            PD_Map MAP = new PD_Map(
                mini_game.map___number_of_cities,
                mini_game.map___cities,
                mini_game.map___name__per__city,
                mini_game.map___position__per__city,
                mini_game.map___infection_type__per__city,
                mini_game.map___neighbors__per__city
                );


            ////////////////////////////////////////////////////////////
            /// game element references...
            ////////////////////////////////////////////////////////////

            // city cards
            List<int> city_cards = new List<int>();
            foreach (int cc in mini_game.map___cities)
            {
                city_cards.Add(cc);
            }

            // infection cards
            List<int> infection_cards = new List<int>();
            foreach (int ic in mini_game.map___cities)
            {
                infection_cards.Add(ic);
            }

            // epidemic cards
            List<int> epidemic_cards = new List<int>();
            for (int ec = 0; ec < 6; ec++)
            {
                epidemic_cards.Add(128 + ec);
            }

            // role cards
            List<int> role_cards = new List<int>();
            role_cards.Add(PD_Player_Roles.Operations_Expert);
            role_cards.Add(PD_Player_Roles.Researcher);
            role_cards.Add(PD_Player_Roles.Medic);
            role_cards.Add(PD_Player_Roles.Scientist);


            Dictionary<int, int> ROLE_CARDS__PER__PLAYER_ID = new Dictionary<int, int>();
            foreach (int p in mini_game.players)
            {
                int mini_player_role = mini_game.role__per__player[p];
                int player_role = PlayerRole__From__MiniGamePlayerRole(mini_player_role);

                ROLE_CARDS__PER__PLAYER_ID.Add(p, player_role);
            }


            ////////////////////////////////////////////////////////////
            /// map elements
            ////////////////////////////////////////////////////////////
            ///

            // location per player
            Dictionary<int, int> location__per__player = new Dictionary<int, int>();
            foreach (int p in mini_game.players)
            {
                int location = mini_game.map_elements___location__per__player[p];
                location__per__player.Add(p, location);
            }


            // inactive_infection_cubes_per_type
            Dictionary<int, int> inactive_infection_cubes_per_type
                = mini_game.map_elements___available_infection_cubes__per__type.CustomDeepCopy();

            // infection_cubes_per_city_id
            Dictionary<int, Dictionary<int, int>> infections__per__type__per__city
                = mini_game.map_elements___infection_cubes__per__type__per__city.CustomDeepCopy();

            // inactive_research_stations
            int inactive_research_stations = mini_game.map_elements___available_research_stations;

            // research_stations_per_city
            Dictionary<int, bool> research_stations_per_city = mini_game.map_elements___research_station__per__city.CustomDeepCopy();

            // map elements...
            PD_MapElements MAP_ELEMENTS = new PD_MapElements(
                location__per__player,
                inactive_infection_cubes_per_type,
                infections__per__type__per__city,
                inactive_research_stations,
                research_stations_per_city
                );


            ////////////////////////////////////////////////////////////
            /// CARDS
            ////////////////////////////////////////////////////////////

            // divided_deck_of_infection_cards
            List<List<int>> divided_deck_of_infection_cards
                = new List<List<int>>();
            foreach (List<int> mini_cards_group in mini_game.cards___divided_deck_of_infection_cards)
            {
                List<int> cards_group = new List<int>();
                foreach (int mini_infection_card in mini_cards_group)
                {
                    cards_group.Add(mini_infection_card);
                }
                divided_deck_of_infection_cards.Add(cards_group);
            }

            // active_infection_cards
            List<int> active_infection_cards
                = new List<int>();
            foreach (int mini_card in mini_game.cards___active_infection_cards)
            {
                active_infection_cards.Add(mini_card);
            }

            // deck_of_discarded_infection_cards
            List<int> deck_of_discarded_infection_cards
                = new List<int>();
            foreach (int mini_card in mini_game.cards___deck_of_discarded_infection_cards)
            {
                deck_of_discarded_infection_cards.Add(mini_card);
            }

            // divided_deck_of_player_cards
            List<List<int>> divided_deck_of_player_cards = mini_game.cards___divided_deck_of_player_cards.CustomDeepCopy();

            // deck_of_dicarded_player_cards
            List<int> deck_of_dicarded_player_cards = mini_game.cards___deck_of_discarded_player_cards.CustomDeepCopy();

            // player_cards_per_player
            Dictionary<int, List<int>> player_cards_per_player = mini_game.cards___player_cards__per__player.CustomDeepCopy();

            List<int> all_role_cards = new List<int>() {
                PD_Player_Roles.Operations_Expert,
                PD_Player_Roles.Researcher,
                PD_Player_Roles.Medic,
                PD_Player_Roles.Scientist
            };

            List<int> inactive_role_cards = all_role_cards.CustomDeepCopy();
            foreach (int player in mini_game.players)
            {
                int mini_role = mini_game.role__per__player[player];
                int game_role = PlayerRole__From__MiniGamePlayerRole(mini_role);
                inactive_role_cards.Remove(game_role);
            }

            PD_GameCards CARDS = new PD_GameCards(
                divided_deck_of_infection_cards,
                active_infection_cards,
                deck_of_discarded_infection_cards,
                divided_deck_of_player_cards,
                deck_of_dicarded_player_cards,
                player_cards_per_player
                );


            PD_Game game = new PD_Game(
                mini_game.unique_id,                        // unique id
                DateTime.UtcNow,                            // start time
                DateTime.UtcNow,                            // end time
                GAME_SETTINGS,                              // game settings
                GAME_FSM,                                   // game fsm
                GAME_STATE_COUNTER,                         // game state counter
                PLAYERS,                                    // players
                MAP,                                        // map
                MAP_ELEMENTS,                               // map elements
                CARDS,                                      // cards
                ROLE_CARDS__PER__PLAYER_ID,                 // role cards per player id     
                new List<PD_GameAction_Base>(),             // player actions history
                new List<PD_InfectionReport>(),             // infection reports
                new List<PD_GameAction_Base>(),             // current aavailable player actions
                new List<PD_MacroAction>()                  // current available player actions
                );

            game.UpdateAvailablePlayerActions();

            return game;
        }

        public static int MiniGame_PlayerCard__FROM__Game_PlayerCard(
            int card
            )
        {
            return card;
        }

        public static int Game_PlayerCard__FROM__MiniGame_PlayerCard(
            int mini_game_player_card
            )
        {
            return mini_game_player_card;
        }

        public static int PlayerRole__From__MiniGamePlayerRole(int mini_game__player_role)
        {
            switch (mini_game__player_role)
            {
                case PD_MiniGame__PlayerRole.UNDEFINED:
                    return PD_Player_Roles.None;
                case PD_MiniGame__PlayerRole.Contingency_Planner:
                    return PD_Player_Roles.None;
                case PD_MiniGame__PlayerRole.Operations_Expert:
                    return PD_Player_Roles.Operations_Expert;
                case PD_MiniGame__PlayerRole.Dispatcher:
                    return PD_Player_Roles.None;
                case PD_MiniGame__PlayerRole.Quarantine_Specialist:
                    return PD_Player_Roles.None;
                case PD_MiniGame__PlayerRole.Researcher:
                    return PD_Player_Roles.Researcher;
                case PD_MiniGame__PlayerRole.Medic:
                    return PD_Player_Roles.Medic;
                case PD_MiniGame__PlayerRole.Scientist:
                    return PD_Player_Roles.Scientist;
                default:
                    return PD_Player_Roles.None;
            }
        }

        public static int MiniGame_PlayerRole__From__PlayerRole(int player_role)
        {
            switch (player_role)
            {
                case PD_Player_Roles.None:
                    return PD_MiniGame__PlayerRole.UNDEFINED;
                case PD_Player_Roles.Operations_Expert:
                    return PD_MiniGame__PlayerRole.Operations_Expert;
                case PD_Player_Roles.Researcher:
                    return PD_MiniGame__PlayerRole.Researcher;
                case PD_Player_Roles.Medic:
                    return PD_MiniGame__PlayerRole.Medic;
                case PD_Player_Roles.Scientist:
                    return PD_MiniGame__PlayerRole.Scientist;
                default:
                    return PD_MiniGame__PlayerRole.UNDEFINED;
            }
        }
    }
}
