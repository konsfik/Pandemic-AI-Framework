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
        public long UniqueID { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public PD_GameSettings GameSettings { get; private set; }
        public PD_GameFSM GameFSM { get; private set; }

        // STATE - RELATED
        public PD_GameStateCounter GameStateCounter { get; private set; }

        // REFERENCES
        public List<PD_Player> Players { get; private set; }
        public PD_Map Map { get; private set; }
        public PD_GameElementReferences GameElementReferences { get; private set; }

        // CONTAINERS
        public PD_GameCards Cards { get; private set; }
        public PD_MapElements MapElements { get; private set; }

        // OWNERSHIPS
        public Dictionary<int, PD_Role_Card> RoleCardsPerPlayerID { get; private set; }

        // GAME HISTORY
        public List<PD_GameAction_Base> PlayerActionsHistory { get; private set; }
        public List<PD_InfectionReport> InfectionReports { get; private set; }

        public List<PD_GameAction_Base> CurrentAvailablePlayerActions { get; private set; }
        public List<PD_MacroAction> CurrentAvailableMacros { get; private set; }

        #endregion

        #region constructors

        public static PD_Game Create(
            Random randomness_provider,
            int number_of_players,
            int game_difficulty,
            bool auto_game_setup
            )
        {
            Dictionary<int, string> playerIDsNames = new Dictionary<int, string>() {
                {0,"Player 0"},
                {1,"Player 1"},
                {2,"Player 2"},
                {3,"Player 3"}
            };

            List<PD_Player> players = new List<PD_Player>();
            for (int i = 0; i < number_of_players; i++)
            {
                players.Add(new PD_Player(i, playerIDsNames[i]));
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


            List<PD_CityCard> allCityCards = new List<PD_CityCard>();
            for (int i = 0; i < 48; i++)
            {
                allCityCards.Add(new PD_CityCard(i, cities[i]));
            }

            List<PD_InfectionCard> all_infection_cards = new List<PD_InfectionCard>();
            for (int i = 0; i < 48; i++)
            {
                all_infection_cards.Add(new PD_InfectionCard(i, cities[i]));
            }

            List<PD_EpidemicCard> all_epidemic_cards = new List<PD_EpidemicCard>();
            for (int i = 0; i < 6; i++)
            {
                all_epidemic_cards.Add(new PD_EpidemicCard(i));
            }

            List<PD_Role_Card> roleCards = new List<PD_Role_Card>() {
                new PD_Role_Card(
                    2,
                    PD_Player_Roles.Operations_Expert
                    ),
                new PD_Role_Card(
                    4,
                    PD_Player_Roles.Researcher
                    ),
                new PD_Role_Card(
                    5,
                    PD_Player_Roles.Medic
                    ),
                new PD_Role_Card(
                    6,
                    PD_Player_Roles.Scientist
                    ),
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
            List<PD_Player> players,

            // map - related
            List<int> cities,
            Dictionary<int, string> name__per__city,
            Dictionary<int, PD_Point> position__per__city,
            Dictionary<int, int> infection_type__per__city,
            Dictionary<int, List<int>> neighbors_per_city,

            List<PD_CityCard> allCityCards,
            List<PD_InfectionCard> allInfectionCards,
            List<PD_EpidemicCard> allEpidemicCards,

            List<PD_Role_Card> allRoleCards,
            int number_of_research_stations
            )
        {
            CurrentAvailableMacros = new List<PD_MacroAction>();

            StartTime = DateTime.UtcNow;
            UniqueID = DateTime.UtcNow.Ticks;

            GameSettings = new PD_GameSettings(gameDifficultyLevel);

            GameStateCounter = new PD_GameStateCounter(
                players.Count,
                0,
                0
                );

            GameFSM = new PD_GameFSM(this);

            Players = players;

            UpdateAvailablePlayerActions();

            Map = new PD_Map(
                cities.Count,
                cities,
                name__per__city,
                position__per__city,
                infection_type__per__city,
                neighbors_per_city);

            // game element references
            GameElementReferences = new PD_GameElementReferences(
                allCityCards,
                allInfectionCards,
                allEpidemicCards,
                allRoleCards
                );

            MapElements = new PD_MapElements(players, cities);
            Cards = new PD_GameCards(Players, allRoleCards);


            RoleCardsPerPlayerID = new Dictionary<int, PD_Role_Card>();
            foreach (var player in players)
            {
                RoleCardsPerPlayerID.Add(player.ID, null);
            }

            PlayerActionsHistory = new List<PD_GameAction_Base>();
            InfectionReports = new List<PD_InfectionReport>();

            UpdateAvailablePlayerActions();
        }

        [JsonConstructor]
        public PD_Game(
            long uniqueID,
            DateTime startTime,
            DateTime endTime,

            PD_GameSettings gameSettings,
            PD_GameFSM gameFSM,
            PD_GameStateCounter gameStateCounter,

            List<PD_Player> players,
            PD_Map map,

            PD_GameElementReferences gameElementReferences,
            PD_MapElements mapElements,
            PD_GameCards cards,

            Dictionary<int, PD_Role_Card> roleCardsPerPlayerID,

            List<PD_GameAction_Base> playerActionsHistory,
            List<PD_InfectionReport> infectionReports,

            List<PD_GameAction_Base> currentAvailablePlayerActions,
            List<PD_MacroAction> currentAvailableMacros
            )
        {
            UniqueID = uniqueID;
            StartTime = startTime;
            EndTime = endTime;

            GameSettings = gameSettings.GetCustomDeepCopy();
            GameFSM = gameFSM.GetCustomDeepCopy();
            GameStateCounter = gameStateCounter.GetCustomDeepCopy();

            Players = players.CustomDeepCopy();
            Map = map.GetCustomDeepCopy();

            GameElementReferences = gameElementReferences.GetCustomDeepCopy();
            MapElements = mapElements.GetCustomDeepCopy();
            Cards = cards.GetCustomDeepCopy();

            RoleCardsPerPlayerID = roleCardsPerPlayerID.CustomDeepCopy();

            PlayerActionsHistory = playerActionsHistory.CustomDeepCopy();
            InfectionReports = infectionReports.CustomDeepCopy();

            CurrentAvailablePlayerActions = currentAvailablePlayerActions.CustomDeepCopy();
            CurrentAvailableMacros = currentAvailableMacros.CustomDeepCopy();
        }

        /// <summary>
        /// private constructor, for deep copy purposes, only!
        /// </summary>
        /// <param name="gameToCopy"></param>
        private PD_Game(
            PD_Game gameToCopy
            )
        {
            UniqueID = gameToCopy.UniqueID;
            StartTime = gameToCopy.StartTime;
            EndTime = gameToCopy.EndTime;

            GameSettings = gameToCopy.GameSettings.GetCustomDeepCopy();
            GameFSM = gameToCopy.GameFSM.GetCustomDeepCopy();
            GameStateCounter = gameToCopy.GameStateCounter.GetCustomDeepCopy();

            Players = gameToCopy.Players.CustomDeepCopy();
            Map = gameToCopy.Map.GetCustomDeepCopy();

            GameElementReferences = gameToCopy.GameElementReferences.GetCustomDeepCopy();
            MapElements = gameToCopy.MapElements.GetCustomDeepCopy();
            Cards = gameToCopy.Cards.GetCustomDeepCopy();

            RoleCardsPerPlayerID = gameToCopy.RoleCardsPerPlayerID.CustomDeepCopy();

            PlayerActionsHistory = gameToCopy.PlayerActionsHistory.CustomDeepCopy();
            InfectionReports = gameToCopy.InfectionReports.CustomDeepCopy();

            CurrentAvailablePlayerActions = gameToCopy.CurrentAvailablePlayerActions.CustomDeepCopy();
            CurrentAvailableMacros = gameToCopy.CurrentAvailableMacros.CustomDeepCopy();
        }

        #endregion

        #region command methods
        public void Com_SetupGame_Random(Random randomness_provider)
        {
            // 1. Set out board and pieces
            // 1.1. put research stations in research stations container
            MapElements.inactive_research_stations = 6;

            // 1.2. separate the infection cubes by color (type) in their containers
            for (int i = 0; i < 4; i++)
            {
                MapElements.inactive_infection_cubes__per__type[i] = 24;
            }

            // 1.3. place research station on atlanta
            int atlanta = this.GQ_Find_CityByName("Atlanta");
            PD_Game_Operators.GO_PlaceResearchStationOnCity(
                this, atlanta);

            // 2. Place outbreaks and cure markers
            // put outbreaks counter to position zero
            GameStateCounter.ResetOutbreaksCounter();

            // place cure markers vial side up
            GameStateCounter.InitializeCureMarkerStates();

            // 3. place infection marker and infect 9 cities
            // 3.1. place the infection marker (epidemics counter) on the lowest position
            GameStateCounter.ResetEpidemicsCounter();

            // 3.2. Infect the first cities - process
            // 3.2.1. put all infection cards in the divided deck of infection cards...
            Cards.DividedDeckOfInfectionCards.Add(new List<PD_InfectionCard>(GameElementReferences.InfectionCards));

            // 3.2.2 shuffle the infection cards deck...
            Cards.DividedDeckOfInfectionCards.ShuffleAllSubListsElements(randomness_provider);

            // 3.2.3 actually infect the cities.. 
            var firstPlayer = Players[0];
            for (int num_InfectionCubes_ToPlace = 3; num_InfectionCubes_ToPlace > 0; num_InfectionCubes_ToPlace--)
            {
                for (int city_Counter = 0; city_Counter < 3; city_Counter++)
                {
                    var infectionCard = Cards.DividedDeckOfInfectionCards.DrawLastElementOfLastSubList();

                    int city = infectionCard.City;
                    int city_type = Map.infection_type__per__city[city];

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

                    Cards.DeckOfDiscardedInfectionCards.Add(infectionCard);
                }
            }

            // 4. Give each player cards and a pawn
            // 4.1. Assign random roles and pawns to players
            List<PD_Player_Roles> available_roles = new List<PD_Player_Roles>() {
                PD_Player_Roles.Operations_Expert,
                PD_Player_Roles.Researcher,
                PD_Player_Roles.Medic,
                PD_Player_Roles.Scientist
            };
            foreach (var player in Players)
            {
                var roleCard = Cards.InactiveRoleCards.DrawOneRandom(randomness_provider);
                RoleCardsPerPlayerID[player.ID] = roleCard;
            }

            // 4.2. Deal cards to players: initial hands
            var playerCardsTempList = new List<PD_PlayerCardBase>();
            playerCardsTempList.AddRange(GameElementReferences.CityCards.Cast<PD_PlayerCardBase>().ToList());
            Cards.DividedDeckOfPlayerCards.Add(playerCardsTempList);
            Cards.DividedDeckOfPlayerCards.ShuffleAllSubListsElements(randomness_provider);

            int numPlayers = Players.Count;
            int numCardsToDealPerPlayer = GameSettings.GetNumberOfInitialCardsToDealPlayers(numPlayers);
            foreach (var player in Players)
            {
                for (int i = 0; i < numCardsToDealPerPlayer; i++)
                {
                    Cards.PlayerCardsPerPlayerID[player.ID].Add(Cards.DividedDeckOfPlayerCards.DrawLastElementOfLastSubList());
                }
            }

            // 5. Prepare the player deck
            // 5.1. get the necessary number of epidemic cards
            int numEpidemicCards = GameSettings.GetNumberOfEpidemicCardsToUseInGame();

            var epidemicCardsTempList = new List<PD_EpidemicCard>(GameElementReferences.EpidemicCards);
            while (epidemicCardsTempList.Count > numEpidemicCards)
            {
                epidemicCardsTempList.DrawOneRandom(randomness_provider);
            }

            // divide the player cards deck in as many sub decks as necessary
            var allPlayerCardsList = Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            //int numberOfPlayerCardsPerSubDeck = allPlayerCardsList.Count / numEpidemicCards;

            int numCards = allPlayerCardsList.Count;
            int numSubDecks = numEpidemicCards;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            // create the sub decks
            List<List<PD_PlayerCardBase>> temporaryDividedList = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numEpidemicCards; i++)
            {
                var subDeck = new List<PD_PlayerCardBase>();
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
            foreach (var subList in temporaryDividedList)
            {
                subList.Add(epidemicCardsTempList.DrawOneRandom(randomness_provider));
            }

            // shuffle all the sublists!
            temporaryDividedList.ShuffleAllSubListsElements(randomness_provider);

            // set the player cards deck as necessary
            Cards.DividedDeckOfPlayerCards.Clear();
            foreach (var sublist in temporaryDividedList)
            {
                Cards.DividedDeckOfPlayerCards.Add(sublist);
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
            PD_Player player,
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
                    = MapElements.inactive_infection_cubes__per__type[treat_Type];

                // if disease eradicated -> set marker to 2
                if (remaining_cubes_this_type == 0)
                {
                    GameStateCounter.CureMarkersStates[treat_Type] = 2;
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
            int medicLocation = this.GQ_Find_Medic_Location();
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

            GameFSM.OnCommand(randomness_provider, this, playerAction);

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
            if (GameFSM.CurrentState.GetType() == typeof(PD_GS_ApplyingMainPlayerActions))
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
                    int numRemainingActionsThisRound = 4 - GameStateCounter.CurrentPlayerActionIndex;
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
            UniqueID = unique_id;
        }

        public void OverrideStartTime()
        {
            StartTime = DateTime.UtcNow;
        }

        public void OverrideStartTime(DateTime date_time)
        {
            StartTime = date_time;
        }

        /// <summary>
        /// This method is called when entering the GameLost or the GameWon states
        /// </summary>
        public void OverrideEndTime()
        {
            EndTime = DateTime.UtcNow;
        }

        public void OverrideEndTime(DateTime date_time)
        {
            EndTime = date_time;
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
            if (this.UniqueID != other.UniqueID)
            {
                return false;
            }
            else if (this.StartTime != other.StartTime)
            {
                return false;
            }
            else if (this.EndTime != other.EndTime)
            {
                return false;
            }
            else if (this.GameSettings.Equals(other.GameSettings) == false)
            {
                return false;
            }
            else if (this.GameFSM.Equals(other.GameFSM) == false)
            {
                return false;
            }
            else if (this.GameStateCounter.Equals(other.GameStateCounter) == false)
            {
                return false;
            }
            else if (this.Players.List_Equals(other.Players) == false)
            {
                return false;
            }
            else if (this.Map.Equals(other.Map) == false)
            {
                return false;
            }
            else if (this.GameElementReferences.Equals(other.GameElementReferences) == false)
            {
                return false;
            }
            else if (this.Cards.Equals(other.Cards) == false)
            {
                return false;
            }
            else if (this.MapElements.Equals(other.MapElements) == false)
            {
                return false;
            }
            else if (this.RoleCardsPerPlayerID.Dictionary_Equals(other.RoleCardsPerPlayerID) == false)
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

            hash = hash * 31 + UniqueID.GetHashCode();
            hash = hash * 31 + StartTime.GetHashCode();
            hash = hash * 31 + EndTime.GetHashCode();

            hash = hash * 31 + GameSettings.GetHashCode();
            hash = hash * 31 + GameFSM.GetHashCode();

            hash = hash * 31 + GameStateCounter.GetHashCode();

            hash = hash * 31 + Players.Custom_HashCode();
            hash = hash * 31 + Map.GetHashCode();
            hash = hash * 31 + GameElementReferences.GetHashCode();

            hash = hash * 31 + Cards.GetHashCode();
            hash = hash * 31 + MapElements.GetHashCode();

            hash = hash * 31 + RoleCardsPerPlayerID.Custom_HashCode();

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
