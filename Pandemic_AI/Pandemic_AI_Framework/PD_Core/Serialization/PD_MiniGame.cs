using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
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


        public PD_MiniGame GetCustomDeepCopy()
        {
            throw new NotImplementedException();
        }

        public static PD_MiniGame From_Normal_State(PD_Game game)
        {
            PD_MiniGame mini_game = new PD_MiniGame();

            // game - id
            mini_game.unique_id = game.UniqueID;

            // game - settings:
            mini_game.settings___number_of_players = game.GameStateCounter.NumberOfPlayers;
            mini_game.settings___game_difficulty = game.GameSettings.GameDifficultyLevel;
            mini_game.settings___maximum_viable_outbreaks = game.GameSettings.MaximumViableOutbreaks;
            mini_game.settings___maximum_player_hand_size = game.GameSettings.MaximumNumberOfPlayerCardsPerPlayer;
            mini_game.settings___initial_hand_size__per__number_of_players =
                game.GameSettings.NumberOfInitialCardsPerNumberOfPlayers.CustomDeepCopy();
            mini_game.settings___epidemic_cards__per__game_difficulty =
                game.GameSettings.NumberOfEpidemicCardsPerDifficultyLevel.CustomDeepCopy();
            mini_game.settings___infection_rate__per__epidemics =
                game.GameSettings.InfectionRatesPerEpidemicsCounter.CustomDeepCopy();

            // general - data
            mini_game.players = new List<int>();
            for (int p = 0; p < mini_game.settings___number_of_players; p++)
            {
                mini_game.players.Add(p);
            }

            // player - roles
            mini_game.unassigned_player_roles = new List<int>();
            foreach (var role_card in game.Cards.InactiveRoleCards)
            {
                PD_Player_Roles role = role_card.Role;
                switch (role)
                {
                    case PD_Player_Roles.None:
                        mini_game.unassigned_player_roles.Add(PD_MiniGame__PlayerRole.UNDEFINED);
                        break;
                    case PD_Player_Roles.Medic:
                        mini_game.unassigned_player_roles.Add(PD_MiniGame__PlayerRole.Medic);
                        break;
                    case PD_Player_Roles.Operations_Expert:
                        mini_game.unassigned_player_roles.Add(PD_MiniGame__PlayerRole.Operations_Expert);
                        break;
                    case PD_Player_Roles.Researcher:
                        mini_game.unassigned_player_roles.Add(PD_MiniGame__PlayerRole.Researcher);
                        break;
                    case PD_Player_Roles.Scientist:
                        mini_game.unassigned_player_roles.Add(PD_MiniGame__PlayerRole.Scientist);
                        break;
                }
            }
            mini_game.role__per__player = new int[mini_game.settings___number_of_players];
            foreach (int p in mini_game.players)
            {
                var game_player = game.Players[p];
                PD_Player_Roles game_player_role = game.GQ_Find_Player_Role(game_player);
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
            mini_game._map___number_of_cities = game.Map.Cities.Count;
            mini_game._map___cities = new List<int>();
            mini_game._map___name__per__city = new Dictionary<int, string>();
            mini_game._map___position__per__city = new Dictionary<int, PD_Point>();
            mini_game._map___infection_type__per__city = new Dictionary<int, int>();
            mini_game._map___neighbors__per__city = new Dictionary<int, List<int>>();
            mini_game._map___research_station__per__city = new Dictionary<int, bool>();
            mini_game._map___location__per__player = new Dictionary<int, int>();
            for (int c = 0; c < mini_game._map___number_of_cities; c++)
            {
                // cities
                mini_game._map___cities.Add(c);
                // name__per__city
                mini_game._map___name__per__city.Add(
                    c,
                    game.Map.Cities[c].Name
                    );
                // position__per__city
                mini_game._map___position__per__city.Add(
                    c,
                    game.Map.Cities[c].Position.GetCustomDeepCopy()
                    );
                // infection_type__per__city
                mini_game._map___infection_type__per__city.Add(
                    c,
                    game.Map.Cities[c].Type
                    );
                // neighbors__per__city
                List<int> neighbors = new List<int>();
                foreach (PD_City nei in game.Map.CityNeighbors_PerCityID[c])
                {
                    neighbors.Add(nei.ID);
                }
                mini_game._map___neighbors__per__city.Add(
                    c,
                    neighbors
                    );
                // research_station__per__city
                if (game.GQ_Is_City_ResearchStation(game.Map.Cities[c]))
                {
                    mini_game._map___research_station__per__city.Add(c, true);
                }
                else
                {
                    mini_game._map___research_station__per__city.Add(c, false);
                }
            }
            // location__per__player
            foreach (int p in mini_game.players)
            {
                var game_player = game.Players[p];
                var game_player_location = game.GQ_PlayerLocation(game_player);
                mini_game._map___location__per__player[p] = game_player_location.ID;
            }
            // infection_cubes__per__type__per__city
            mini_game._map___infection_cubes__per__type__per__city =
                new Dictionary<int, Dictionary<int, int>>();
            foreach (int c in mini_game._map___cities)
            {
                mini_game._map___infection_cubes__per__type__per__city.Add(
                    c,
                    new Dictionary<int, int>()
                    );
            }
            foreach (var city in game.Map.Cities)
            {
                for (int t = 0; t < 4; t++)
                {
                    int num_cubes__this_type__that_city = game.GQ_Find_InfectionCubes_OfType_OnCity(city, t).Count;
                    mini_game._map___infection_cubes__per__type__per__city[city.ID].Add(
                        t,
                        num_cubes__this_type__that_city
                        );
                }
            }

            /////////////////////////////////////////////////
            // game elements
            /////////////////////////////////////////////////

            // availablee research stations
            mini_game.available_research_stations = game.MapElements.InactiveResearchStations.Count;
            // available infection cubes per type
            mini_game.available_infection_cubes__per__type = new Dictionary<int, int>();
            for (int t = 0; t < 4; t++)
            {
                mini_game.available_infection_cubes__per__type.Add(
                    t, game.Num_InactiveInfectionCubes_OfType(t)
                    );
            }


            /////////////////////////////////////////////////
            // state counters
            /////////////////////////////////////////////////

            if (game.GameFSM.CurrentState is PD_GS_Idle)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.IDLE;
            }
            else if (game.GameFSM.CurrentState is PD_GS_ApplyingMainPlayerActions)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.MAIN_PLAYER_ACTIONS;
            }
            else if (game.GameFSM.CurrentState is PD_GS_Discarding_DuringMainPlayerActions)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DISCARDING_DURING_PLAY;
            }
            else if (game.GameFSM.CurrentState is PD_GS_DrawingNewPlayerCards)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DRAWING_NEW_PLAYER_CARDS;
            }
            else if (game.GameFSM.CurrentState is PD_GS_Discarding_AfterDrawing)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DISCARDING_AFTER_DRAWING;
            }
            else if (game.GameFSM.CurrentState is PD_GS_ApplyingEpidemicCard)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.APPLYING_EPIDEMIC_CARDS;
            }
            else if (game.GameFSM.CurrentState is PD_GS_DrawingNewInfectionCards)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.DRAWING_NEW_INFECTION_CARDS;
            }
            else if (game.GameFSM.CurrentState is PD_GS_ApplyingInfectionCards)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.APPLYING_INFECTION_CARDS;
            }
            else if (game.GameFSM.CurrentState is PD_GS_GameLost)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.GAME_LOST;
            }
            else if (game.GameFSM.CurrentState is PD_GS_GameWon)
            {
                mini_game.state_counter___current_state = PD_MiniGame__GameState.GAME_WON;
            }
            else
            {
                throw new System.Exception("state not found!");
            }

            mini_game.state_counter___current_turn = game.GameStateCounter.CurrentTurnIndex;
            mini_game.state_counter___current_player = game.GameStateCounter.CurrentPlayerIndex;
            mini_game.state_counter___current_player_action_index = game.GameStateCounter.CurrentPlayerActionIndex;
            mini_game.state_counter___number_of_outbreaks = game.GameStateCounter.OutbreaksCounter;
            mini_game.state_counter___number_of_epidemics = game.GameStateCounter.EpidemicsCounter;
            mini_game.state_counter___disease_states = new Dictionary<int, int>();
            for (int t = 0; t < 4; t++)
            {
                if (game.GameStateCounter.CureMarkersStates[t] == 0)
                {
                    mini_game.state_counter___disease_states.Add(t, PD_MiniGame__DiseaseState.ACTIVE);
                }
                else if (game.GameStateCounter.CureMarkersStates[t] == 1)
                {
                    mini_game.state_counter___disease_states.Add(t, PD_MiniGame__DiseaseState.CURED);
                }
                else if (game.GameStateCounter.CureMarkersStates[t] == 2)
                {
                    mini_game.state_counter___disease_states.Add(t, PD_MiniGame__DiseaseState.ERADICATED);
                }
            }


            /////////////////////////////////////////////////
            // cards
            /////////////////////////////////////////////////

            // divided_deck_of_infection_cards
            mini_game._cards___divided_deck_of_infection_cards = new List<List<int>>();
            foreach (var group in game.Cards.DividedDeckOfInfectionCards)
            {
                List<int> mini_group = new List<int>();
                foreach (var card in group)
                {
                    int mini_infection_card = card.City.ID;
                    mini_group.Add(mini_infection_card);
                }
                mini_game._cards___divided_deck_of_infection_cards.Add(mini_group);
            }

            // active_infection_cards
            mini_game._cards___active_infection_cards = new List<int>();
            foreach (var card in game.Cards.ActiveInfectionCards)
            {
                int mini_card = card.City.ID;
                mini_game._cards___active_infection_cards.Add(mini_card);
            }

            // deck_of_discarded_infection_cards
            mini_game._cards___deck_of_discarded_infection_cards = new List<int>();
            foreach (var card in game.Cards.DeckOfDiscardedInfectionCards)
            {
                int mini_card = card.City.ID;
                mini_game._cards___deck_of_discarded_infection_cards.Add(mini_card);
            }

            // divided_deck_of_player_cards
            mini_game._cards___divided_deck_of_player_cards = new List<List<int>>();
            foreach (var group in game.Cards.DividedDeckOfPlayerCards)
            {
                List<int> mini_group = new List<int>();
                foreach (var card in group)
                {
                    if (card is PD_CityCard cc)
                    {
                        int mini_card = cc.City.ID;
                        mini_group.Add(mini_card);
                    }
                    else if (card is PD_EpidemicCard ec)
                    {
                        int mini_card = ec.ID + 128;
                        mini_group.Add(mini_card);
                    }
                }
                mini_game._cards___divided_deck_of_player_cards.Add(mini_group);
            }

            // deck_of_discarded_player_cards
            mini_game._cards___deck_of_discarded_player_cards = new List<int>();
            foreach (var player_card in game.Cards.DeckOfDiscardedPlayerCards)
            {
                if (player_card is PD_CityCard cc)
                {
                    int mini_city_card = cc.City.ID;
                    mini_game._cards___deck_of_discarded_player_cards.Add(mini_city_card);
                }
                else if (player_card is PD_EpidemicCard ec)
                {
                    int mini_epidemic_card = ec.ID + 128;
                    mini_game._cards___deck_of_discarded_player_cards.Add(mini_epidemic_card);
                }
            }

            mini_game._cards___player_cards__per__player = new Dictionary<int, List<int>>();
            foreach (var player in game.Players)
            {
                List<PD_PlayerCardBase> player_hand = game.GQ_PlayerHand(player);
                List<int> mini_player_hand = new List<int>();
                foreach (var card in player_hand)
                {
                    if (card is PD_CityCard cc)
                    {
                        int mini_city_card = cc.City.ID;
                        mini_player_hand.Add(mini_city_card);
                    }
                    else if (card is PD_EpidemicCard ec)
                    {
                        int mini_epidemic_card = ec.ID + 128;
                        mini_player_hand.Add(mini_epidemic_card);
                    }
                }
                mini_game._cards___player_cards__per__player.Add(player.ID, mini_player_hand);
            }



            return mini_game;
        }

    }
}
