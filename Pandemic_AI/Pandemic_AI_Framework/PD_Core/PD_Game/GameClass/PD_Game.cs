using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_Game : ICustomDeepCopyable<PD_Game>
    {
        #region properties
        public long unique_id;
        public DateTime start_time;
        public DateTime end_time;

        public PD_GameSettings game_settings;
        public PD_GameFSM game_FSM;

        // STATE - RELATED
        public PD_GameStateCounter game_state_counter;

        public List<int> players;
        public Dictionary<int, int> role__per__player;

        public PD_Map map;


        // CONTAINERS
        public PD_GameCards cards;
        public PD_MapElements map_elements;

        // GAME HISTORY
        public List<PD_GameAction_Base> PlayerActionsHistory;
        public List<PD_InfectionReport> InfectionReports;

        public List<PD_GameAction_Base> CurrentAvailablePlayerActions;
        public List<PD_MacroAction> CurrentAvailableMacros;

        #endregion

        #region constructors

        public static PD_Game Create(
            Random randomness_provider,
            int number_of_players,
            int game_difficulty,
            bool auto_game_setup
            )
        {

            List<int> players = new List<int>();
            for (int i = 0; i < number_of_players; i++)
            {
                players.Add(i);
            }

            List<int> cities = new List<int>() {
                0,   1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11,
                12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23,
                24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35,
                36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47
            };

            Dictionary<int, string> name__per__city = new Dictionary<int, string>() {
                {0,   "Atlanta"          },
                {1,   "Chicago"          },
                {2,   "Essen"            },
                {3,   "London"           },
                {4,   "Madrid"           },
                {5,   "Milan"            },
                {6,   "Montreal"         },
                {7,   "New York"         },
                {8,   "Paris"            },
                {9,   "San Francisco"    },
                {10,  "St. Petersburg"   },
                {11,  "Washington"       },

                {12,  "Bogota"           },
                {13,  "Buenos Aires"     },
                {14,  "Johannesburg"     },
                {15,  "Khartoum"         },
                {16,  "Kinshasha"        },
                {17,  "Lagos"            },
                {18,  "Lima"             },
                {19,  "Los Angeles"      },
                {20,  "Mexico City"      },
                {21,  "Miami"            },
                {22,  "Santiago"         },
                {23,  "Sao Paolo"        },

                {24,  "Algiers"          },
                {25,  "Baghdad"          },
                {26,  "Cairo"            },
                {27,  "Chennai"          },
                {28,  "Delhi"            },
                {29,  "Istanbul"         },
                {30,  "Karachi"          },
                {31,  "Kolkata"          },
                {32,  "Moscow"           },
                {33,  "Mumbai"           },
                {34,  "Riyadh"           },
                {35,  "Tehran"           },

                {36,  "Bangkok"          },
                {37,  "Beijing"          },
                {38,  "Ho Chi Minh City" },
                {39,  "Hong Kong"        },
                {40,  "Jakarta"          },
                {41,  "Manila"           },
                {42,  "Osaka"            },
                {43,  "Seoul"            },
                {44,  "Shanghai"         },
                {45,  "Sydney"           },
                {46,  "Taipei"           },
                {47,  "Tokyo"            }
            };

            Dictionary<int, PD_Point> position__per__city = new Dictionary<int, PD_Point>() {
                {0,   new PD_Point( 379,  837  ) },
                {1,   new PD_Point( 330,  934  ) },
                {2,   new PD_Point( 942,  1026 ) },
                {3,   new PD_Point( 809,  1002 ) },
                {4,   new PD_Point( 790,  872  ) },
                {5,   new PD_Point( 997,  963  ) },
                {6,   new PD_Point( 462,  939  ) },
                {7,   new PD_Point( 564,  924  ) },
                {8,   new PD_Point( 910,  938  ) },
                {9,   new PD_Point( 161,  885  ) },
                {10,  new PD_Point( 1089, 1054 ) },
                {11,  new PD_Point( 519,  844  ) },

                {12,  new PD_Point( 452,  595  ) },
                {13,  new PD_Point( 561,  331  ) },
                {14,  new PD_Point( 1055, 392  ) },
                {15,  new PD_Point( 1065, 643  ) },
                {16,  new PD_Point( 980,  526  ) },
                {17,  new PD_Point( 896,  619  ) },
                {18,  new PD_Point( 405,  452  ) },
                {19,  new PD_Point( 188,  753  ) },
                {20,  new PD_Point( 311,  709  ) },
                {21,  new PD_Point( 463,  729  ) },
                {22,  new PD_Point( 423,  305  ) },
                {23,  new PD_Point( 638,  432  ) },

                {24,  new PD_Point( 938,  800  ) },
                {25,  new PD_Point( 1150, 818  ) },
                {26,  new PD_Point( 1037, 773  ) },
                {27,  new PD_Point( 1392, 605  ) },
                {28,  new PD_Point( 1373, 812  ) },
                {29,  new PD_Point( 1056, 887  ) },
                {30,  new PD_Point( 1276, 775  ) },
                {31,  new PD_Point( 1466, 780  ) },
                {32,  new PD_Point( 1160, 964  ) },
                {33,  new PD_Point( 1289, 677  ) },
                {34,  new PD_Point( 1165, 699  ) },
                {35,  new PD_Point( 1250, 901  ) },

                {36,  new PD_Point( 1486, 668  ) },
                {37,  new PD_Point( 1543, 931  ) },
                {38,  new PD_Point( 1567, 574  ) },
                {39,  new PD_Point( 1563, 727  ) },
                {40,  new PD_Point( 1486, 490  ) },
                {41,  new PD_Point( 1697, 582  ) },
                {42,  new PD_Point( 1764, 786  ) },
                {43,  new PD_Point( 1660, 938  ) },
                {44,  new PD_Point( 1551, 839  ) },
                {45,  new PD_Point( 1773, 310  ) },
                {46,  new PD_Point( 1668, 746  ) },
                {47,  new PD_Point( 1753, 885  ) }
            };

            Dictionary<int, int> infection_type__per__city = new Dictionary<int, int>() {
                {0,   0},
                {1,   0},
                {2,   0},
                {3,   0},
                {4,   0},
                {5,   0},
                {6,   0},
                {7,   0},
                {8,   0},
                {9,   0},
                {10,  0},
                {11,  0},

                {12,  1},
                {13,  1},
                {14,  1},
                {15,  1},
                {16,  1},
                {17,  1},
                {18,  1},
                {19,  1},
                {20,  1},
                {21,  1},
                {22,  1},
                {23,  1},

                {24,  2},
                {25,  2},
                {26,  2},
                {27,  2},
                {28,  2},
                {29,  2},
                {30,  2},
                {31,  2},
                {32,  2},
                {33,  2},
                {34,  2},
                {35,  2},

                {36,  3},
                {37,  3},
                {38,  3},
                {39,  3},
                {40,  3},
                {41,  3},
                {42,  3},
                {43,  3},
                {44,  3},
                {45,  3},
                {46,  3},
                {47,  3}
            };

            Dictionary<int, List<int>> neighbors__per__city = new Dictionary<int, List<int>>() {
                { 0,  new List<int>(){  11, 21, 1              } },
                { 1,  new List<int>(){  9,  0,  6              } },
                { 2,  new List<int>(){  3,  8,  5,  10         } },
                { 3,  new List<int>(){  7,  4,  8,  2          } },
                { 4,  new List<int>(){  7,  3,  8,  24, 23     } },
                { 5,  new List<int>(){  29, 8,  2              } },
                { 6,  new List<int>(){  11, 7,  1              } },
                { 7,  new List<int>(){  6,  11, 3,  4          } },
                { 8,  new List<int>(){  3,  4,  2,  5,  24     } },
                { 9,  new List<int>(){  1,  19, 47, 41         } },
                { 10, new List<int>(){  32, 29, 2              } },
                { 11, new List<int>(){  0,  21, 6,  7          } },

                { 12, new List<int>(){  21, 20, 18, 13, 23     } },
                { 13, new List<int>(){  12, 23                 } },
                { 14, new List<int>(){  16, 15                 } },
                { 15, new List<int>(){  17, 16, 14, 26         } },
                { 16, new List<int>(){  17, 15, 14             } },
                { 17, new List<int>(){  23, 15, 16             } },
                { 18, new List<int>(){  12, 20, 22             } },
                { 19, new List<int>(){  20, 45, 9,  1          } },
                { 20, new List<int>(){  21, 1,  19, 12         } },
                { 21, new List<int>(){  0,  11, 20, 12         } },
                { 22, new List<int>(){  18                     } },
                { 23, new List<int>(){  12, 13, 17, 4          } },

                { 24, new List<int>(){  8,  4,  29, 26         } },
                { 25, new List<int>(){  29, 34, 26, 30, 35     } },
                { 26, new List<int>(){  24, 15, 34, 25, 29     } },
                { 27, new List<int>(){  33, 28, 31, 36, 40     } },
                { 28, new List<int>(){  30, 35, 33, 27, 31     } },
                { 29, new List<int>(){  5,  10, 32, 25, 26, 24 } },
                { 30, new List<int>(){  28, 25, 35, 34, 33     } },
                { 31, new List<int>(){  36, 39, 28, 27         } },
                { 32, new List<int>(){  10, 29, 35             } },
                { 33, new List<int>(){  30, 27, 28             } },
                { 34, new List<int>(){  25, 26, 30             } },
                { 35, new List<int>(){  32, 25, 28, 30         } },

                { 36, new List<int>(){  31, 27,39,38,40        } },
                { 37, new List<int>(){  44, 43                 } },
                { 38, new List<int>(){  36, 39, 41, 40         } },
                { 39, new List<int>(){  31, 36, 46, 44, 38, 41 } },
                { 40, new List<int>(){  38, 36, 27, 45         } },
                { 41, new List<int>(){  45, 9,  39, 38, 46     } },
                { 42, new List<int>(){  47, 46                 } },
                { 43, new List<int>(){  47, 37, 44             } },
                { 44, new List<int>(){  37, 47, 43, 46, 39     } },
                { 45, new List<int>(){  40, 41, 19             } },
                { 46, new List<int>(){  39, 42, 41, 44         } },
                { 47, new List<int>(){  42, 9, 43, 44          } },
            };


            List<int> allCityCards = new List<int>();
            for (int cc = 0; cc < 48; cc++)
            {
                allCityCards.Add(cc);
            }

            List<int> all_infection_cards = new List<int>();
            for (int i = 0; i < 48; i++)
            {
                all_infection_cards.Add(i);
            }

            List<int> all_epidemic_cards = new List<int>();
            for (int ec = 0; ec < 6; ec++)
            {
                all_epidemic_cards.Add(128 + ec);
            }

            List<int> roleCards = new List<int>() {
                PD_Player_Roles.Operations_Expert,
                PD_Player_Roles.Researcher,
                PD_Player_Roles.Medic,
                PD_Player_Roles.Scientist
            };

            int number_of_research_stations = 6;

            PD_Game new_game = new PD_Game(
                game_difficulty,
                players,
                cities,
                name__per__city,
                position__per__city,
                infection_type__per__city,
                neighbors__per__city,
                allCityCards,
                all_infection_cards,
                all_epidemic_cards,
                roleCards,
                number_of_research_stations
                );



            if (auto_game_setup)
            {
                new_game.Apply_Action(
                    randomness_provider,
                    new_game.CurrentAvailablePlayerActions[0]
                    );
            }

            return new_game;
        }

        // normal constructor
        public PD_Game(
            int gameDifficultyLevel,
            List<int> players,

            // map - related
            List<int> cities,
            Dictionary<int, string> name__per__city,
            Dictionary<int, PD_Point> position__per__city,
            Dictionary<int, int> infection_type__per__city,
            Dictionary<int, List<int>> neighbors_per_city,

            List<int> allCityCards,
            List<int> allInfectionCards,
            List<int> allEpidemicCards,

            List<int> allRoleCards,
            int number_of_research_stations
            )
        {
            CurrentAvailableMacros = new List<PD_MacroAction>();

            start_time = DateTime.UtcNow;
            unique_id = DateTime.UtcNow.Ticks;

            game_settings = new PD_GameSettings(gameDifficultyLevel);

            game_state_counter = new PD_GameStateCounter(
                players.Count,
                0,
                0
                );

            game_FSM = new PD_GameFSM(this);

            this.players = players;

            UpdateAvailablePlayerActions();

            map = new PD_Map(
                cities.Count,
                cities,
                name__per__city,
                position__per__city,
                infection_type__per__city,
                neighbors_per_city);

            map_elements = new PD_MapElements(players, cities);
            cards = new PD_GameCards(this.players);


            role__per__player = new Dictionary<int, int>();
            foreach (var player in players)
            {
                role__per__player.Add(player, PD_Player_Roles.None);
            }

            PlayerActionsHistory = new List<PD_GameAction_Base>();
            InfectionReports = new List<PD_InfectionReport>();

            UpdateAvailablePlayerActions();
        }

        [JsonConstructor]
        public PD_Game(
            long unique_id,
            DateTime start_time,
            DateTime end_time,

            PD_GameSettings game_settings,
            PD_GameFSM game_FSM,
            PD_GameStateCounter game_state_counter,

            List<int> players,
            PD_Map map,

            PD_MapElements map_elements,
            PD_GameCards cards,

            Dictionary<int, int> role__per__player,

            List<PD_GameAction_Base> playerActionsHistory,
            List<PD_InfectionReport> infectionReports,

            List<PD_GameAction_Base> currentAvailablePlayerActions,
            List<PD_MacroAction> currentAvailableMacros
            )
        {
            this.unique_id = unique_id;
            this.start_time = start_time;
            this.end_time = end_time;

            this.game_settings = game_settings.GetCustomDeepCopy();
            this.game_FSM = game_FSM.GetCustomDeepCopy();
            this.game_state_counter = game_state_counter.GetCustomDeepCopy();

            this.players = players.CustomDeepCopy();
            this.map = map.GetCustomDeepCopy();

            this.map_elements = map_elements.GetCustomDeepCopy();
            this.cards = cards.GetCustomDeepCopy();

            this.role__per__player = role__per__player.CustomDeepCopy();

            this.PlayerActionsHistory = playerActionsHistory.CustomDeepCopy();
            this.InfectionReports = infectionReports.CustomDeepCopy();

            this.CurrentAvailablePlayerActions = currentAvailablePlayerActions.CustomDeepCopy();
            this.CurrentAvailableMacros = currentAvailableMacros.CustomDeepCopy();
        }

        /// <summary>
        /// private constructor, for deep copy purposes, only!
        /// </summary>
        /// <param name="gameToCopy"></param>
        private PD_Game(
            PD_Game gameToCopy
            )
        {
            unique_id = gameToCopy.unique_id;
            start_time = gameToCopy.start_time;
            end_time = gameToCopy.end_time;

            game_settings = gameToCopy.game_settings.GetCustomDeepCopy();
            game_FSM = gameToCopy.game_FSM.GetCustomDeepCopy();
            game_state_counter = gameToCopy.game_state_counter.GetCustomDeepCopy();

            players = gameToCopy.players.CustomDeepCopy();
            map = gameToCopy.map.GetCustomDeepCopy();

            map_elements = gameToCopy.map_elements.GetCustomDeepCopy();
            cards = gameToCopy.cards.GetCustomDeepCopy();

            role__per__player = gameToCopy.role__per__player.CustomDeepCopy();

            PlayerActionsHistory = gameToCopy.PlayerActionsHistory.CustomDeepCopy();
            InfectionReports = gameToCopy.InfectionReports.CustomDeepCopy();

            CurrentAvailablePlayerActions = gameToCopy.CurrentAvailablePlayerActions.CustomDeepCopy();
            CurrentAvailableMacros = gameToCopy.CurrentAvailableMacros.CustomDeepCopy();
        }

        #endregion

        #region command methods
        public void Com_SetupGame_Random(Random randomness_provider)
        {
            List<int> initial_container__city_cards = new List<int>();
            List<int> initial_container__infection_cards = new List<int>();
            foreach (int city in map.cities) {
                initial_container__city_cards.Add(city);
                initial_container__infection_cards.Add(city);
            }
            List<int> initial_container__epidemic_cards = new List<int>();
            for (int i = 0; i < 6; i++) {
                initial_container__epidemic_cards.Add(128 + i);
            }


            // 1. Set out board and pieces
            // 1.1. put research stations in research stations container
            map_elements.inactive_research_stations = 6;

            // 1.2. separate the infection cubes by color (type) in their containers
            for (int i = 0; i < 4; i++)
            {
                map_elements.inactive_infection_cubes__per__type[i] = 24;
            }

            // 1.3. place research station on atlanta
            int atlanta = this.GQ_Find_CityByName("Atlanta");
            PD_Game_Operators.GO_PlaceResearchStationOnCity(
                this, atlanta);

            // 2. Place outbreaks and cure markers
            // put outbreaks counter to position zero
            game_state_counter.ResetOutbreaksCounter();

            // place cure markers vial side up
            game_state_counter.InitializeCureMarkerStates();

            // 3. place infection marker and infect 9 cities
            // 3.1. place the infection marker (epidemics counter) on the lowest position
            game_state_counter.ResetEpidemicsCounter();

            // 3.2. Infect the first cities - process
            // 3.2.1. put all infection cards in the divided deck of infection cards...
            cards.divided_deck_of_infection_cards.Add(initial_container__infection_cards.DrawAll());

            // 3.2.2 shuffle the infection cards deck...
            cards.divided_deck_of_infection_cards.ShuffleAllSubListsElements(randomness_provider);

            // 3.2.3 actually infect the cities.. 
            var firstPlayer = players[0];
            for (int num_InfectionCubes_ToPlace = 3; num_InfectionCubes_ToPlace > 0; num_InfectionCubes_ToPlace--)
            {
                for (int city_Counter = 0; city_Counter < 3; city_Counter++)
                {
                    var infectionCard = cards.divided_deck_of_infection_cards.DrawLastElementOfLastSubList();

                    int city = infectionCard;
                    int city_type = map.infection_type__per__city[city];

                    PD_InfectionReport report = new PD_InfectionReport(
                        true,
                        firstPlayer,
                        city,
                        city_type,
                        num_InfectionCubes_ToPlace
                        );

                    PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                        this,
                        city,
                        num_InfectionCubes_ToPlace,
                        report,
                        true
                        );

                    InfectionReports.Add(finalReport);

                    cards.deck_of_discarded_infection_cards.Add(infectionCard);
                }
            }

            // 4. Give each player cards and a pawn
            // 4.1. Assign random roles and pawns to players
            List<int> available_roles = new List<int>() {
                PD_Player_Roles.Operations_Expert,
                PD_Player_Roles.Researcher,
                PD_Player_Roles.Medic,
                PD_Player_Roles.Scientist
            };
            foreach (int player in players)
            {
                int roleCard = available_roles.DrawOneRandom(randomness_provider);
                role__per__player[player] = roleCard;
            }

            // 4.2. Deal cards to players: initial hands
            cards.divided_deck_of_player_cards.Add(initial_container__city_cards.DrawAll());
            cards.divided_deck_of_player_cards.ShuffleAllSubListsElements(randomness_provider);

            int numPlayers = players.Count;
            int numCardsToDealPerPlayer = game_settings.GetNumberOfInitialCardsToDealPlayers(numPlayers);
            foreach (var player in players)
            {
                for (int i = 0; i < numCardsToDealPerPlayer; i++)
                {
                    cards.player_hand__per__player[player].Add(cards.divided_deck_of_player_cards.DrawLastElementOfLastSubList());
                }
            }

            // 5. Prepare the player deck
            // 5.1. get the necessary number of epidemic cards
            int numEpidemicCards = game_settings.GetNumberOfEpidemicCardsToUseInGame();

            // divide the player cards deck in as many sub decks as necessary
            var allPlayerCardsList = cards.divided_deck_of_player_cards.DrawAllElementsOfAllSubListsAsOneList();
            //int numberOfPlayerCardsPerSubDeck = allPlayerCardsList.Count / numEpidemicCards;

            int numCards = allPlayerCardsList.Count;
            int numSubDecks = numEpidemicCards;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            // create the sub decks
            List<List<int>> temporaryDividedList = new List<List<int>>();
            for (int i = 0; i < numEpidemicCards; i++)
            {
                var subDeck = new List<int>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    subDeck.Add(allPlayerCardsList.DrawOneRandom(randomness_provider));
                }
                temporaryDividedList.Add(subDeck);
            }
            // add the remaining cards
            for (int i = 0; i < remainingCardsNumber; i++)
            {
                int deckIndex = (temporaryDividedList.Count - 1) - i;
                temporaryDividedList[deckIndex].Add(allPlayerCardsList.DrawOneRandom(randomness_provider));
            }
            // insert the epidemic cards
            foreach (List<int> subList in temporaryDividedList)
            {
                int epidemic_card = initial_container__epidemic_cards.DrawFirst();
                subList.Add(epidemic_card);
            }

            // shuffle all the sublists!
            temporaryDividedList.ShuffleAllSubListsElements(randomness_provider);

            // set the player cards deck as necessary
            cards.divided_deck_of_player_cards.Clear();
            foreach (var sublist in temporaryDividedList)
            {
                cards.divided_deck_of_player_cards.Add(sublist);
            }

            //// place all pawns on atlanta
            PD_Game_Operators.GO_PlaceAllPawnsOnAtlanta(this);

            UpdateAvailablePlayerActions();
        }

        public void Medic_MoveTreat(
            int city
            )
        {
            // if player is a medic:
            if (this.GQ_CurrentPlayer_Role() == PD_Player_Roles.Medic)
            {
                List<int> typesOfInfectionCubesOnTargetLocation = this.GQ_InfectionCubeTypes_OnCity(
                    city
                    );
                foreach (var type in typesOfInfectionCubesOnTargetLocation)
                {
                    // if this type has been cured:
                    if (this.GQ_Is_DiseaseCured_OR_Eradicated(type))
                    {
                        PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                            this,
                            city,
                            type
                            );
                        // create the supposed (auto) action
                        PD_PA_TreatDisease_Medic_Auto actionToStore = new PD_PA_TreatDisease_Medic_Auto(
                            this.GQ_CurrentPlayer(),
                            city,
                            type
                            );

                        // store the action in the game actions history
                        PlayerActionsHistory.Add(actionToStore);
                    }

                }
            }
        }

        public void Com_PA_TreatDisease_Medic(
            int player,
            int city,
            int treat_Type
            )
        {
            if (this.GQ_Is_DiseaseCured_OR_Eradicated(treat_Type))
            {
                // remove all cubes of this type
                PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                    this,
                    city,
                    treat_Type
                    );

                // check if disease is eradicated...
                int remaining_cubes_this_type
                    = map_elements.inactive_infection_cubes__per__type[treat_Type];

                // if disease eradicated -> set marker to 2
                if (remaining_cubes_this_type == 0)
                {
                    game_state_counter.disease_states[treat_Type] = 2;
                }
            }
            else
            {
                // remove all cubes of this type
                PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                    this,
                    city,
                    treat_Type
                    );
            }

        }

        public void Medic_AutoTreat_AfterDiscoverCure(int curedDiseaaseType)
        {
            int medicLocation = this.GQ_Medic_Location();
            if (medicLocation != -1)
            {
                List<int> infectionCubeTypes_OnMedicLocation =
                    this.GQ_InfectionCubeTypes_OnCity(medicLocation);

                if (infectionCubeTypes_OnMedicLocation.Contains(curedDiseaaseType))
                {
                    // remove all cubes of this type from medic's location
                    PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                        this,
                        medicLocation,
                        curedDiseaaseType
                        );
                }
            }
        }
        #endregion

        public void UpdateAvailablePlayerActions()
        {
            CurrentAvailablePlayerActions = this.FindAvailable_PlayerActions();
        }

        public void Apply_Action(
            Random randomness_provider,
            PD_GameAction_Base playerAction
            )
        {
            CurrentAvailableMacros = new List<PD_MacroAction>();

            if (CurrentAvailablePlayerActions.Contains(playerAction) == false)
            {
                throw new System.Exception("non applicable action");
            }

            PlayerActionsHistory.Add(playerAction);

            game_FSM.OnCommand(randomness_provider, this, playerAction);

            UpdateAvailablePlayerActions();

            // after an action is applied, see if the next action is auto action
            // and if so, apply it automatically.
            if (
                CurrentAvailablePlayerActions != null
                && CurrentAvailablePlayerActions.Count > 0
                )
            {
                if (CurrentAvailablePlayerActions[0] is I_Auto_Action)
                {
                    Apply_Action(
                        randomness_provider,
                        CurrentAvailablePlayerActions[0]
                        );
                }
            }
        }

        public List<PD_MacroAction> GetAvailableMacros(PD_AI_PathFinder pathFinder)
        {
            if (
                CurrentAvailableMacros == null
                ||
                CurrentAvailableMacros.Count == 0
                )
            {
                var researchStationCities = this.GQ_ResearchStationCities();
                CurrentAvailableMacros = PD_MacroActionsSynthesisSystem.FindAll_Macros(
                    this,
                    pathFinder,
                    researchStationCities
                    );
                return new List<PD_MacroAction>(CurrentAvailableMacros);
            }

            return new List<PD_MacroAction>(CurrentAvailableMacros);
        }

        public void Apply_Macro_Action(
            Random randomness_provider,
            PD_MacroAction macro
            )
        {
            if (game_FSM.CurrentState.GetType() == typeof(PD_GS_ApplyingMainPlayerActions))
            {
                if (
                    macro.MacroAction_Type == PD_MacroAction_Type.Walk_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.Stay_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Scientist_Macro
                    )
                {
                    var executablePart = new List<PD_GameAction_Base>();
                    int numRemainingActionsThisRound = 4 - game_state_counter.player_action_index;
                    for (int i = 0; i < numRemainingActionsThisRound; i++)
                    {
                        if (i < macro.Count_Total_Length())
                        {
                            executablePart.Add(macro.Actions_All[i].GetCustomDeepCopy());
                        }
                    }

                    foreach (var command in executablePart)
                    {
                        if (CurrentAvailablePlayerActions.Contains(command))
                        {
                            Apply_Action(
                                randomness_provider,
                                command
                                );
                        }
                        else
                        {
                            Console.WriteLine(macro.GetDescription());
                            Console.WriteLine("problem: " + command.GetDescription());
                            throw new System.Exception("command cannot be applied!");
                        }
                    }
                }
                else
                {
                    throw new System.Exception("wrong macro type");
                }
            }
            else if (this.GQ_IsInState_DiscardAfterDrawing())
            {
                PD_MacroAction_Type mat = macro.MacroAction_Type;
                if (mat == PD_MacroAction_Type.Discard_AfterDrawing_Macro)
                {
                    var action = macro.Actions_All[0];
                    if (CurrentAvailablePlayerActions.Contains(action))
                    {
                        //Console.WriteLine("applying command: " + action.GetDescription());
                        Apply_Action(
                            randomness_provider,
                            action
                            );
                    }
                    else
                    {
                        throw new System.Exception("command cannot be applied!");
                    }
                }
                else
                {
                    throw new System.Exception("wrong macro type");
                }
            }
            else if (this.GQ_IsInState_DiscardDuringMainPlayerActions())
            {
                PD_MacroAction_Type mat = macro.MacroAction_Type;
                if (mat == PD_MacroAction_Type.Discard_DuringMainPlayerActions_Macro)
                {
                    var action = macro.Actions_All[0];
                    if (CurrentAvailablePlayerActions.Contains(action))
                    {
                        //Console.WriteLine("applying command: " + action.GetDescription());
                        Apply_Action(
                            randomness_provider,
                            action
                            );
                    }
                    else
                    {
                        throw new System.Exception("command cannot be applied!");
                    }
                }
                else
                {
                    throw new System.Exception("wrong macro type");
                }
            }
        }

        public void OverrideUniqueID(long unique_id)
        {
            this.unique_id = unique_id;
        }

        public void OverrideStartTime()
        {
            start_time = DateTime.UtcNow;
        }

        public void OverrideStartTime(DateTime date_time)
        {
            start_time = date_time;
        }

        /// <summary>
        /// This method is called when entering the GameLost or the GameWon states
        /// </summary>
        public void OverrideEndTime()
        {
            end_time = DateTime.UtcNow;
        }

        public void OverrideEndTime(DateTime date_time)
        {
            end_time = date_time;
        }

        public PD_Game Request_Fair_ForwardModel(Random randomness_provider)
        {
            PD_Game gameCopy = this.GetCustomDeepCopy();
            gameCopy.GO_RandomizeGame(randomness_provider);
            return gameCopy;
        }

        public PD_Game Request_Unfair_ForwardModel()
        {
            PD_Game gameCopy = this.GetCustomDeepCopy();
            return gameCopy;
        }

        public PD_Game GetCustomDeepCopy()
        {
            return new PD_Game(this);
        }

        #region equality overrides
        public bool Equals(PD_Game other)
        {
            if (this.unique_id != other.unique_id)
            {
                return false;
            }
            else if (this.start_time != other.start_time)
            {
                return false;
            }
            else if (this.end_time != other.end_time)
            {
                return false;
            }
            else if (this.game_settings.Equals(other.game_settings) == false)
            {
                return false;
            }
            else if (this.game_FSM.Equals(other.game_FSM) == false)
            {
                return false;
            }
            else if (this.game_state_counter.Equals(other.game_state_counter) == false)
            {
                return false;
            }
            else if (this.players.List_Equals(other.players) == false)
            {
                return false;
            }
            else if (this.map.Equals(other.map) == false)
            {
                return false;
            }
            else if (this.cards.Equals(other.cards) == false)
            {
                return false;
            }
            else if (this.map_elements.Equals(other.map_elements) == false)
            {
                return false;
            }
            else if (this.role__per__player.Dictionary_Equals(other.role__per__player) == false)
            {
                return false;
            }
            else if (this.PlayerActionsHistory.List_Equals(other.PlayerActionsHistory) == false)
            {
                return false;
            }
            else if (this.InfectionReports.List_Equals(other.InfectionReports) == false)
            {
                return false;
            }
            else if (this.CurrentAvailablePlayerActions.List_Equals(other.CurrentAvailablePlayerActions) == false)
            {
                return false;
            }
            else if (this.CurrentAvailableMacros.List_Equals(other.CurrentAvailableMacros) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_Game other_game)
            {
                return Equals(other_game);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + unique_id.GetHashCode();
            hash = hash * 31 + start_time.GetHashCode();
            hash = hash * 31 + end_time.GetHashCode();

            hash = hash * 31 + game_settings.GetHashCode();
            hash = hash * 31 + game_FSM.GetHashCode();

            hash = hash * 31 + game_state_counter.GetHashCode();

            hash = hash * 31 + players.Custom_HashCode();
            hash = hash * 31 + map.GetHashCode();

            hash = hash * 31 + cards.GetHashCode();
            hash = hash * 31 + map_elements.GetHashCode();

            hash = hash * 31 + role__per__player.Custom_HashCode();

            hash = hash * 31 + PlayerActionsHistory.Custom_HashCode();
            hash = hash * 31 + InfectionReports.Custom_HashCode();

            hash = hash * 31 + CurrentAvailablePlayerActions.Custom_HashCode();
            hash = hash * 31 + CurrentAvailableMacros.Custom_HashCode();

            return hash;
        }

        public static bool operator ==(PD_Game c1, PD_Game c2)
        {
            if (Object.ReferenceEquals(c1, null))
            {
                if (Object.ReferenceEquals(c2, null))
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
                if (Object.ReferenceEquals(c2, null)) // c2 is null
                {
                    return false;
                }
            }
            // c1 is not null && c2 is not null
            // -> actually check equality
            return c1.Equals(c2);
        }

        public static bool operator !=(PD_Game c1, PD_Game c2)
        {
            return !(c1 == c2);
        }

        #endregion
    }
}
