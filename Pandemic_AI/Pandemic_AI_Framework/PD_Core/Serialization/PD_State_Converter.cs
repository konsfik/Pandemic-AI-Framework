﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    public static class PD_State_Converter
    {
        public static PD_MiniGame To_MiniGame(this PD_Game game)
        {
            return MiniGame__From__Game(game);
        }

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

        public static PD_MiniGame MiniGame__From__Game(PD_Game game)
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
            mini_game.map___number_of_cities = game.Map.Cities.Count;
            mini_game.map___cities = new List<int>();
            mini_game.map___name__per__city = new Dictionary<int, string>();
            mini_game.map___position__per__city = new Dictionary<int, PD_Point>();
            mini_game.map___infection_type__per__city = new Dictionary<int, int>();
            mini_game.map___neighbors__per__city = new Dictionary<int, List<int>>();
            mini_game.map___research_station__per__city = new Dictionary<int, bool>();
            mini_game.map___location__per__player = new Dictionary<int, int>();
            for (int c = 0; c < mini_game.map___number_of_cities; c++)
            {
                // cities
                mini_game.map___cities.Add(c);
                // name__per__city
                mini_game.map___name__per__city.Add(
                    c,
                    game.Map.Cities[c].Name
                    );
                // position__per__city
                mini_game.map___position__per__city.Add(
                    c,
                    game.Map.Cities[c].Position.GetCustomDeepCopy()
                    );
                // infection_type__per__city
                mini_game.map___infection_type__per__city.Add(
                    c,
                    game.Map.Cities[c].Type
                    );
                // neighbors__per__city
                List<int> neighbors = new List<int>();
                foreach (PD_City nei in game.Map.CityNeighbors_PerCityID[c])
                {
                    neighbors.Add(nei.ID);
                }
                mini_game.map___neighbors__per__city.Add(
                    c,
                    neighbors
                    );
                // research_station__per__city
                if (game.GQ_Is_City_ResearchStation(game.Map.Cities[c]))
                {
                    mini_game.map___research_station__per__city.Add(c, true);
                }
                else
                {
                    mini_game.map___research_station__per__city.Add(c, false);
                }
            }
            // location__per__player
            foreach (int p in mini_game.players)
            {
                var game_player = game.Players[p];
                var game_player_location = game.GQ_PlayerLocation(game_player);
                mini_game.map___location__per__player[p] = game_player_location.ID;
            }
            // infection_cubes__per__type__per__city
            mini_game.map___infection_cubes__per__type__per__city =
                new Dictionary<int, Dictionary<int, int>>();
            foreach (int c in mini_game.map___cities)
            {
                mini_game.map___infection_cubes__per__type__per__city.Add(
                    c,
                    new Dictionary<int, int>()
                    );
            }
            foreach (var city in game.Map.Cities)
            {
                for (int t = 0; t < 4; t++)
                {
                    int num_cubes__this_type__that_city = game.GQ_Find_InfectionCubes_OfType_OnCity(city, t).Count;
                    mini_game.map___infection_cubes__per__type__per__city[city.ID].Add(
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
            mini_game.cards___divided_deck_of_infection_cards = new List<List<int>>();
            foreach (var group in game.Cards.DividedDeckOfInfectionCards)
            {
                List<int> mini_group = new List<int>();
                foreach (var card in group)
                {
                    int mini_infection_card = card.City.ID;
                    mini_group.Add(mini_infection_card);
                }
                mini_game.cards___divided_deck_of_infection_cards.Add(mini_group);
            }

            // active_infection_cards
            mini_game.cards___active_infection_cards = new List<int>();
            foreach (var card in game.Cards.ActiveInfectionCards)
            {
                int mini_card = card.City.ID;
                mini_game.cards___active_infection_cards.Add(mini_card);
            }

            // deck_of_discarded_infection_cards
            mini_game.cards___deck_of_discarded_infection_cards = new List<int>();
            foreach (var card in game.Cards.DeckOfDiscardedInfectionCards)
            {
                int mini_card = card.City.ID;
                mini_game.cards___deck_of_discarded_infection_cards.Add(mini_card);
            }

            // divided_deck_of_player_cards
            mini_game.cards___divided_deck_of_player_cards = new List<List<int>>();
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
                mini_game.cards___divided_deck_of_player_cards.Add(mini_group);
            }

            // deck_of_discarded_player_cards
            mini_game.cards___deck_of_discarded_player_cards = new List<int>();
            foreach (var player_card in game.Cards.DeckOfDiscardedPlayerCards)
            {
                if (player_card is PD_CityCard cc)
                {
                    int mini_city_card = cc.City.ID;
                    mini_game.cards___deck_of_discarded_player_cards.Add(mini_city_card);
                }
                else if (player_card is PD_EpidemicCard ec)
                {
                    int mini_epidemic_card = ec.ID + 128;
                    mini_game.cards___deck_of_discarded_player_cards.Add(mini_epidemic_card);
                }
            }

            mini_game.cards___player_cards__per__player = new Dictionary<int, List<int>>();
            foreach (var player in game.Players)
            {
                List<PD_PlayerCardBase> game__player_hand = game.GQ_PlayerHand(player);
                List<int> mini_game__player_hand = new List<int>();
                foreach (var card in game__player_hand)
                {
                    int mini_game__player_card = MiniGame_PlayerCard__FROM__Game_PlayerCard(card);
                    mini_game__player_hand.Add(mini_game__player_card);
                }
                mini_game.cards___player_cards__per__player.Add(player.ID, mini_game__player_hand);
            }



            return mini_game;
        }

        public static PD_Game Game__From__MiniGame(PD_MiniGame mini_game)
        {

            // game settings...
            int game_difficulty_level = mini_game.settings___game_difficulty;
            PD_GameSettings game_settings = new PD_GameSettings(game_difficulty_level);

            // game FSM
            PD_GameFSM game_fsm;
            switch (mini_game.state_counter___current_state)
            {
                case PD_MiniGame__GameState.IDLE:
                    game_fsm = new PD_GameFSM(new PD_GS_Idle());
                    break;
                case PD_MiniGame__GameState.MAIN_PLAYER_ACTIONS:
                    game_fsm = new PD_GameFSM(new PD_GS_ApplyingMainPlayerActions());
                    break;
                case PD_MiniGame__GameState.DISCARDING_DURING_PLAY:
                    game_fsm = new PD_GameFSM(new PD_GS_Discarding_DuringMainPlayerActions());
                    break;
                case PD_MiniGame__GameState.DRAWING_NEW_PLAYER_CARDS:
                    game_fsm = new PD_GameFSM(new PD_GS_DrawingNewPlayerCards());
                    break;
                case PD_MiniGame__GameState.DISCARDING_AFTER_DRAWING:
                    game_fsm = new PD_GameFSM(new PD_GS_Discarding_AfterDrawing());
                    break;
                case PD_MiniGame__GameState.APPLYING_EPIDEMIC_CARDS:
                    game_fsm = new PD_GameFSM(new PD_GS_ApplyingEpidemicCard());
                    break;
                case PD_MiniGame__GameState.DRAWING_NEW_INFECTION_CARDS:
                    game_fsm = new PD_GameFSM(new PD_GS_DrawingNewInfectionCards());
                    break;
                case PD_MiniGame__GameState.APPLYING_INFECTION_CARDS:
                    game_fsm = new PD_GameFSM(new PD_GS_ApplyingInfectionCards());
                    break;
                case PD_MiniGame__GameState.GAME_LOST:
                    game_fsm = new PD_GameFSM(new PD_GS_GameLost());
                    break;
                case PD_MiniGame__GameState.GAME_WON:
                    game_fsm = new PD_GameFSM(new PD_GS_GameWon());
                    break;
                default:
                    throw new System.Exception("incorrect state!");
            }

            // game state counter...
            PD_GameStateCounter game_state_counter = new PD_GameStateCounter(
                mini_game.settings___number_of_players,
                mini_game.state_counter___current_player_action_index,
                mini_game.state_counter___current_player,
                mini_game.state_counter___current_turn,
                mini_game.state_counter___disease_states.CustomDeepCopy(),
                mini_game.state_counter___number_of_outbreaks,
                mini_game.state_counter___number_of_epidemics,
                mini_game.flag___NotEnoughDiseaseCubesToCompleteAnInfection,
                mini_game.flag___NotEnoughPlayerCardsToDraw
                );

            ////////////////////////////////////////////////////////////
            /// players
            ////////////////////////////////////////////////////////////
            List<PD_Player> players = new List<PD_Player>();
            foreach (int p in mini_game.players)
            {
                PD_Player player = new PD_Player(p, "Player " + p.ToString());
            }

            ////////////////////////////////////////////////////////////
            /// map
            ////////////////////////////////////////////////////////////

            // cities
            List<PD_City> cities = new List<PD_City>();
            foreach (int c in mini_game.map___cities)
            {
                PD_City city = new PD_City(
                    c,
                    mini_game.map___infection_type__per__city[c],
                    mini_game.map___name__per__city[c],
                    mini_game.map___position__per__city[c]
                    );
                cities.Add(city);
            }

            // neighbors:
            Dictionary<int, List<PD_City>> neighbors__per__city = new Dictionary<int, List<PD_City>>();
            foreach (int c in mini_game.map___cities)
            {
                List<int> mini_neighbors = mini_game.map___neighbors__per__city[c];
                List<PD_City> neighbors = new List<PD_City>();
                foreach (int n in mini_neighbors)
                {
                    neighbors.Add(cities[n]);
                }
                neighbors__per__city.Add(c, neighbors);
            }

            // edges:
            List<PD_CityEdge> edges = new List<PD_CityEdge>();
            foreach (PD_City city in cities)
            {
                List<PD_City> neighbors = neighbors__per__city[city.ID];
                foreach (PD_City neighbor in neighbors)
                {
                    PD_CityEdge edge = new PD_CityEdge(city, neighbor);
                    if (edges.Contains(edge) == false)
                    {
                        edges.Add(edge);
                    }
                }
            }
            // map
            PD_Map map = new PD_Map(
                cities,
                edges,
                neighbors__per__city
                );


            ////////////////////////////////////////////////////////////
            /// game element references...
            ////////////////////////////////////////////////////////////

            // city cards
            List<PD_CityCard> city_cards = new List<PD_CityCard>();
            foreach (int c in mini_game.map___cities)
            {
                PD_CityCard city_card = new PD_CityCard(c, cities[c]);
            }

            // infection cards
            List<PD_InfectionCard> infection_cards = new List<PD_InfectionCard>();
            foreach (int c in mini_game.map___cities)
            {
                PD_InfectionCard infection_card = new PD_InfectionCard(c, cities[c]);
            }

            // epidemic cards
            List<PD_EpidemicCard> epidemic_cards = new List<PD_EpidemicCard>();
            for (int i = 0; i < 6; i++)
            {
                PD_EpidemicCard epidemic_card = new PD_EpidemicCard(i);
                epidemic_cards.Add(epidemic_card);
            }

            // player pawns
            List<PD_ME_PlayerPawn> player_pawns = new List<PD_ME_PlayerPawn>();
            player_pawns.Add(
                new PD_ME_PlayerPawn(PD_MiniGame__PlayerRole.Operations_Expert, PD_Player_Roles.Operations_Expert)
                );
            player_pawns.Add(
                new PD_ME_PlayerPawn(PD_MiniGame__PlayerRole.Researcher, PD_Player_Roles.Researcher)
                );
            player_pawns.Add(
                new PD_ME_PlayerPawn(PD_MiniGame__PlayerRole.Medic, PD_Player_Roles.Medic)
                );
            player_pawns.Add(
                new PD_ME_PlayerPawn(PD_MiniGame__PlayerRole.Scientist, PD_Player_Roles.Scientist)
                );

            // role cards
            List<PD_Role_Card> role_cards = new List<PD_Role_Card>();
            role_cards.Add(
                new PD_Role_Card(PD_MiniGame__PlayerRole.Operations_Expert, PD_Player_Roles.Operations_Expert)
                );
            role_cards.Add(
                new PD_Role_Card(PD_MiniGame__PlayerRole.Researcher, PD_Player_Roles.Researcher)
                );
            role_cards.Add(
                new PD_Role_Card(PD_MiniGame__PlayerRole.Medic, PD_Player_Roles.Medic)
                );
            role_cards.Add(
                new PD_Role_Card(PD_MiniGame__PlayerRole.Scientist, PD_Player_Roles.Scientist)
                );

            // research stations
            List<PD_ME_ResearchStation> research_stations = new List<PD_ME_ResearchStation>();
            for (int i = 0; i < 6; i++)
            {
                research_stations.Add(new PD_ME_ResearchStation(i));
            }

            // infection cubes
            List<PD_ME_InfectionCube> infection_cubes = new List<PD_ME_InfectionCube>();
            for (int i = 0; i < 24; i++)
            {
                for (int t = 0; t < 4; t++)
                {
                    int index = t * 24 + i;
                    PD_ME_InfectionCube ic = new PD_ME_InfectionCube(
                        index,
                        t
                        );
                    infection_cubes.Add(ic);
                }
            }

            // game element references...
            PD_GameElementReferences game_element_references = new PD_GameElementReferences(
                city_cards,
                infection_cards,
                epidemic_cards,
                player_pawns,
                role_cards,
                research_stations,
                infection_cubes
                );

            ////////////////////////////////////////////////////////////
            /// map elements
            ////////////////////////////////////////////////////////////

            // inactive player pawns
            List<PD_ME_PlayerPawn> inactive_player_pawns = new List<PD_ME_PlayerPawn>();
            foreach (int ur in mini_game.unassigned_player_roles)
            {
                switch (ur)
                {
                    case PD_MiniGame__PlayerRole.UNDEFINED:
                        inactive_player_pawns.Add(player_pawns.Find(x => x.Role == PD_Player_Roles.None));
                        break;
                    case PD_MiniGame__PlayerRole.Operations_Expert:
                        inactive_player_pawns.Add(player_pawns.Find(x => x.Role == PD_Player_Roles.Operations_Expert));
                        break;
                    case PD_MiniGame__PlayerRole.Researcher:
                        inactive_player_pawns.Add(player_pawns.Find(x => x.Role == PD_Player_Roles.Researcher));
                        break;
                    case PD_MiniGame__PlayerRole.Medic:
                        inactive_player_pawns.Add(player_pawns.Find(x => x.Role == PD_Player_Roles.Medic));
                        break;
                    case PD_MiniGame__PlayerRole.Scientist:
                        inactive_player_pawns.Add(player_pawns.Find(x => x.Role == PD_Player_Roles.Scientist));
                        break;
                }
            }

            // player pawns per city id
            Dictionary<int, List<PD_ME_PlayerPawn>> player_pawns__per__city_id = new Dictionary<int, List<PD_ME_PlayerPawn>>();
            foreach (int c in mini_game.map___cities)
            {
                player_pawns__per__city_id.Add(c, new List<PD_ME_PlayerPawn>());
            }
            //foreach (int p in mini_game.players) {
            //    int player_location = mini_game._map___location__per__player[p];
            //    int mini_player_role = mini_game.role__per__player[p];

            //    PD_ME_PlayerPawn player_pawn = 
            //}

            //PD_MapElements map_elements = new PD_MapElements(
            //    inactive_player_pawns,

            //    );


            //PD_Game game = new PD_Game(
            //    0,
            //    DateTime.UtcNow,
            //    DateTime.UtcNow,
            //    game_settings,
            //    game_fsm,
            //    game_state_counter,
            //    players,
            //    map,
            //    game_element_references,
            //    );

            return null;
        }

        public static int MiniGame_PlayerCard__FROM__Game_PlayerCard(PD_PlayerCardBase card)
        {
            if (card is PD_CityCard cc)
            {
                return cc.City.ID;
            }
            else if (card is PD_EpidemicCard ec)
            {
                return ec.ID + 128;
            }
            else if (card is PD_InfectionCard ic)
            {
                return ic.City.ID;
            }
            else
            {
                throw new System.Exception("incorrect card type");
            }
        }
    }
}
