using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public partial class PD_Game : ICustomDeepCopyable<PD_Game>
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
        public Dictionary<int, PD_ME_PlayerPawn> PlayerPawnsPerPlayerID { get; private set; }
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

            // generate the default cities...
            List<PD_City> cities = new List<PD_City>()
            {
                new PD_City(0,  0, "Atlanta"          , new PD_Point( 379,  837  )),
                new PD_City(1,  0, "Chicago"          , new PD_Point( 330,  934  )),
                new PD_City(2,  0, "Essen"            , new PD_Point( 942,  1026 )),
                new PD_City(3,  0, "London"           , new PD_Point( 809,  1002 )),
                new PD_City(4,  0, "Madrid"           , new PD_Point( 790,  872  )),
                new PD_City(5,  0, "Milan"            , new PD_Point( 997,  963  )),
                new PD_City(6,  0, "Montreal"         , new PD_Point( 462,  939  )),
                new PD_City(7,  0, "New York"         , new PD_Point( 564,  924  )),
                new PD_City(8,  0, "Paris"            , new PD_Point( 910,  938  )),
                new PD_City(9,  0, "San Francisco"    , new PD_Point( 161,  885  )),
                new PD_City(10, 0, "St. Petersburg"   , new PD_Point( 1089, 1054 )),
                new PD_City(11, 0, "Washington"       , new PD_Point( 519,  844  )),

                new PD_City(12, 1, "Bogota"           , new PD_Point( 452,  595  )),
                new PD_City(13, 1, "Buenos Aires"     , new PD_Point( 561,  331  )),
                new PD_City(14, 1, "Johannesburg"     , new PD_Point( 1055, 392  )),
                new PD_City(15, 1, "Khartoum"         , new PD_Point( 1065, 643  )),
                new PD_City(16, 1, "Kinshasha"        , new PD_Point( 980,  526  )),
                new PD_City(17, 1, "Lagos"            , new PD_Point( 896,  619  )),
                new PD_City(18, 1, "Lima"             , new PD_Point( 405,  452  )),
                new PD_City(19, 1, "Los Angeles"      , new PD_Point( 188,  753  )),
                new PD_City(20, 1, "Mexico City"      , new PD_Point( 311,  709  )),
                new PD_City(21, 1, "Miami"            , new PD_Point( 463,  729  )),
                new PD_City(22, 1, "Santiago"         , new PD_Point( 423,  305  )),
                new PD_City(23, 1, "Sao Paolo"        , new PD_Point( 638,  432  )),

                new PD_City(24, 2, "Algiers"          , new PD_Point( 938,  800  )),
                new PD_City(25, 2, "Baghdad"          , new PD_Point( 1150, 818  )),
                new PD_City(26, 2, "Cairo"            , new PD_Point( 1037, 773  )),
                new PD_City(27, 2, "Chennai"          , new PD_Point( 1392, 605  )),
                new PD_City(28, 2, "Delhi"            , new PD_Point( 1373, 812  )),
                new PD_City(29, 2, "Istanbul"         , new PD_Point( 1056, 887  )),
                new PD_City(30, 2, "Karachi"          , new PD_Point( 1276, 775  )),
                new PD_City(31, 2, "Kolkata"          , new PD_Point( 1466, 780  )),
                new PD_City(32, 2, "Moscow"           , new PD_Point( 1160, 964  )),
                new PD_City(33, 2, "Mumbai"           , new PD_Point( 1289, 677  )),
                new PD_City(34, 2, "Riyadh"           , new PD_Point( 1165, 699  )),
                new PD_City(35, 2, "Tehran"           , new PD_Point( 1250, 901  )),

                new PD_City(36, 3, "Bangkok"          , new PD_Point( 1486, 668  )),
                new PD_City(37, 3, "Beijing"          , new PD_Point( 1543, 931  )),
                new PD_City(38, 3, "Ho Chi Minh City" , new PD_Point( 1567, 574  )),
                new PD_City(39, 3, "Hong Kong"        , new PD_Point( 1563, 727  )),
                new PD_City(40, 3, "Jakarta"          , new PD_Point( 1486, 490  )),
                new PD_City(41, 3, "Manila"           , new PD_Point( 1697, 582  )),
                new PD_City(42, 3, "Osaka"            , new PD_Point( 1764, 786  )),
                new PD_City(43, 3, "Seoul"            , new PD_Point( 1660, 938  )),
                new PD_City(44, 3, "Shanghai"         , new PD_Point( 1551, 839  )),
                new PD_City(45, 3, "Sydney"           , new PD_Point( 1773, 310  )),
                new PD_City(46, 3, "Taipei"           , new PD_Point( 1668, 746  )),
                new PD_City(47, 3, "Tokyo"            , new PD_Point( 1753, 885  )),
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

            List<PD_CityEdge> city_edges = new List<PD_CityEdge>();
            for (int c = 0; c < cities.Count; c++)
            {
                PD_City city = cities[c];
                foreach (int n in neighbors__per__city[c])
                {
                    PD_City neighbor = cities[n];
                    city_edges.Add(
                        new PD_CityEdge(city, neighbor)
                        );
                }
            }

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

            List<PD_ME_PlayerPawn> playerPawns = new List<PD_ME_PlayerPawn>() {
                new PD_ME_PlayerPawn(
                    2,
                    PD_Player_Roles.Operations_Expert
                    ),
                new PD_ME_PlayerPawn(
                    4,
                    PD_Player_Roles.Researcher
                    ),
                new PD_ME_PlayerPawn(
                    5,
                    PD_Player_Roles.Medic
                    ),
                new PD_ME_PlayerPawn(
                    6,
                    PD_Player_Roles.Scientist
                    )
            };

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

            List<PD_ME_ResearchStation> research_stations = new List<PD_ME_ResearchStation>();
            for (int i = 0; i < 6; i++)
            {
                PD_ME_ResearchStation rs = new PD_ME_ResearchStation(i);
                research_stations.Add(rs);
            }

            List<PD_ME_InfectionCube> infection_cubes = new List<PD_ME_InfectionCube>();
            int infectionCubeID = 0;
            foreach (var city in cities)
            {
                for (int i = 0; i < 2; i++)
                {
                    PD_ME_InfectionCube ic = new PD_ME_InfectionCube(infectionCubeID, city.Type);
                    infection_cubes.Add(ic);
                    infectionCubeID++;
                }
            }

            PD_Game new_game = new PD_Game(
                game_difficulty,
                players,
                cities,
                city_edges,
                allCityCards,
                all_infection_cards,
                all_epidemic_cards,
                playerPawns,
                roleCards,
                research_stations,
                infection_cubes
                );

            if (auto_game_setup) {
                new_game.ApplySpecificPlayerAction(
                    randomness_provider,
                    new_game.CurrentAvailablePlayerActions[0]
                    );
            }

            return new_game;
        }

        /// <summary>
        /// NORMAL CONSTRUCTOR!
        /// </summary>
        /// <param name="gameDifficultyLevel"></param>
        /// <param name="players"></param>
        /// <param name="cities"></param>
        /// <param name="edges"></param>
        /// <param name="allCityCards"></param>
        /// <param name="allInfectionCards"></param>
        /// <param name="allEpidemicCards"></param>
        /// <param name="allPlayerPawns"></param>
        /// <param name="allResearchStations"></param>
        /// <param name="allInfectionCubes"></param>
        public PD_Game(
            int gameDifficultyLevel,
            List<PD_Player> players,
            List<PD_City> cities,
            List<PD_CityEdge> edges,

            List<PD_CityCard> allCityCards,
            List<PD_InfectionCard> allInfectionCards,
            List<PD_EpidemicCard> allEpidemicCards,

            List<PD_ME_PlayerPawn> allPlayerPawns,
            List<PD_Role_Card> allRoleCards,
            List<PD_ME_ResearchStation> allResearchStations,
            List<PD_ME_InfectionCube> allInfectionCubes
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

            Map = new PD_Map(cities, edges);

            // game element references
            GameElementReferences = new PD_GameElementReferences(
                allCityCards,
                allInfectionCards,
                allEpidemicCards,

                allPlayerPawns,
                allRoleCards,

                allResearchStations,
                allInfectionCubes
                );

            MapElements = new PD_MapElements(Map.Cities);
            Cards = new PD_GameCards(Players, allRoleCards);

            PlayerPawnsPerPlayerID = new Dictionary<int, PD_ME_PlayerPawn>();
            foreach (var player in Players)
            {
                PlayerPawnsPerPlayerID.Add(player.ID, null);
            }

            RoleCardsPerPlayerID = new Dictionary<int, PD_Role_Card>();
            foreach (var player in players)
            {
                RoleCardsPerPlayerID.Add(player.ID, null);
            }

            PlayerActionsHistory = new List<PD_GameAction_Base>();
            InfectionReports = new List<PD_InfectionReport>();

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

            Dictionary<int, PD_ME_PlayerPawn> playerPawnsPerPlayerID,
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

            PlayerPawnsPerPlayerID = playerPawnsPerPlayerID.CustomDeepCopy();
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

            PlayerPawnsPerPlayerID = gameToCopy.PlayerPawnsPerPlayerID.CustomDeepCopy();
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
            MapElements.InactiveResearchStations.AddRange(
                new List<PD_ME_ResearchStation>(GameElementReferences.ResearchStations));

            // 1.2. separate the infection cubes by color (type) in their containers
            for (int i = 0; i < 4; i++)
            {
                List<PD_ME_InfectionCube> infectionCubesByType =
                    GameElementReferences.InfectionCubes.FindAll(
                        x =>
                        x.Type == i
                    );
                MapElements.InactiveInfectionCubesPerType.Add(i, infectionCubesByType);
            }

            // 1.3. place research station on atlanta
            PD_City atlanta = this.GQ_Find_CityByName("Atlanta");
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

                    PD_City city = infectionCard.City;

                    PD_InfectionReport report = new PD_InfectionReport(
                        true,
                        firstPlayer,
                        city,
                        city.Type,
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
            MapElements.InactivePlayerPawns.AddRange(GameElementReferences.PlayerPawns);

            foreach (var player in Players)
            {
                var pawn = MapElements.InactivePlayerPawns.DrawOneRandom(randomness_provider);
                var roleCard = GameElementReferences.RoleCards.Find(
                    x =>
                    x.Role == pawn.Role
                    );
                Cards.InactiveRoleCards.Remove(roleCard);
                PlayerPawnsPerPlayerID[player.ID] = pawn;
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
            PD_City city
            )
        {
            // if player is a medic:
            if (this.GQ_CurrentPlayer_Role() == PD_Player_Roles.Medic)
            {
                List<int> typesOfInfectionCubesOnTargetLocation = this.GQ_Find_InfectionCubeTypes_OnCity(
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
            PD_City city,
            int treat_Type
            )
        {
            if (this.GQ_Count_Num_InfectionCubes_OfType_OnCity(city, treat_Type) == 0)
            {
                throw new System.Exception("City does not have any infection cubes of the requested type");
            }

            if (this.GQ_CurrentPlayer_Role() != PD_Player_Roles.Medic)
            {
                throw new System.Exception(
                    "Only Medic can perform this type of action.");
            }

            if (this.GQ_Is_DiseaseCured_OR_Eradicated(treat_Type))
            {
                // remove all cubes of this type
                PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                    this,
                    city,
                    treat_Type
                    );

                // check if disease is eradicated...
                var remainingCubesOfThisType = new List<PD_ME_InfectionCube>();
                foreach (var someCity in Map.Cities)
                {
                    var cubesOfThisTypeOnSomeCity = MapElements.InfectionCubesPerCityID[someCity.ID].FindAll(
                        x =>
                        x.Type == treat_Type
                        );
                    remainingCubesOfThisType.AddRange(cubesOfThisTypeOnSomeCity);
                }

                // if disease eradicated -> set marker to 2
                if (remainingCubesOfThisType.Count == 0)
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
            PD_City medicLocation = this.GQ_Find_Medic_Location();
            if (medicLocation != null)
            {
                List<int> infectionCubeTypes_OnMedicLocation =
                    this.GQ_Find_InfectionCubeTypes_OnCity(medicLocation);

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

        public void Com_ApplyEpidemicCard(
            Random randomness_provider,
            PD_Player player
            )
        {
            // 0. discard the epidemic card...
            var allCards_InPlayerHand = Cards.PlayerCardsPerPlayerID[player.ID];
            var epidemicCard = allCards_InPlayerHand.Find(
                x =>
                x.GetType() == typeof(PD_EpidemicCard)
                );
            Cards.PlayerCardsPerPlayerID[player.ID].Remove(epidemicCard);
            Cards.DeckOfDiscardedPlayerCards.Add(epidemicCard);

            // 1. move the infection rate marker by one position
            GameStateCounter.IncreaseEpidemicsCounter();

            // 2. Infect: Draw the bottom card from the Infection Deck. 
            // Unless its disease color has been eradicated, put 3 disease cubes of that color on the named city.
            // If the city already has cubes of this color, do not add 3 cubes to it.
            // Instead, add just enough cubes so that it has 3 cubes of this color and then an outbreak of this disease occurs in the city(see Outbreaks below). 
            // Discard this card to the Infection Discard Pile.
            var cardFromBottom = Cards.DividedDeckOfInfectionCards.DrawFirstElementOfFirstSubList();

            int epidemicInfectionType = cardFromBottom.Type;
            bool diseaseTypeEradicated = this.GQ_Is_Disease_Eradicated(epidemicInfectionType);

            if (diseaseTypeEradicated == false)
            {
                // actually apply the epidemic infection here...

                PD_InfectionReport initialReport = new PD_InfectionReport(
                    false, // not game setup...
                    player,
                    cardFromBottom.City,
                    cardFromBottom.Type,
                    3
                    );

                // apply infection of this city here
                PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                    this,
                    cardFromBottom.City,
                    3,
                    initialReport,
                    false
                    );

                InfectionReports.Add(finalReport);

                if (finalReport.FailureReason == InfectionFailureReasons.notEnoughDiseaseCubes)
                {
                    GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection = true;
                }
            }

            // put card in discarded infection cards pile
            Cards.DeckOfDiscardedInfectionCards.Add(cardFromBottom);

            // 3. intensify: Reshuffle just the cards in the Infection Discard Pile 
            // and place them on top of the Infection Deck.
            Cards.DeckOfDiscardedInfectionCards.Shuffle(randomness_provider);
            Cards.DividedDeckOfInfectionCards.Add(
                Cards.DeckOfDiscardedInfectionCards.DrawAll()
                );
        }

        public void Com_Discard_AfterDrwing(
            PD_Player player,
            PD_PlayerCardBase playerCardToDiscard
            )
        {
            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(this, player, playerCardToDiscard);
        }

        public void Com_Discard_DuringMainPlayerActions(
            PD_Player player,
            PD_PlayerCardBase playerCardToDiscard
            )
        {
            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(this, player, playerCardToDiscard);
        }

        public void Com_DrawNewInfectionCards(PD_Player player)
        {
            int numberOfInfectionCardsToDraw =
                GameSettings.InfectionRatesPerEpidemicsCounter[GameStateCounter.EpidemicsCounter];

            var infectionCards = new List<PD_InfectionCard>();

            for (int i = 0; i < numberOfInfectionCardsToDraw; i++)
            {
                infectionCards.Add(Cards.DividedDeckOfInfectionCards.DrawLastElementOfLastSubList());
            }

            Cards.ActiveInfectionCards.AddRange(infectionCards);

        }

        public void Com_ApplyInfectionCard(PD_Player player, PD_InfectionCard infectionCard)
        {
            int infectionType = infectionCard.Type;
            bool diseaseEradicated = this.GQ_Is_Disease_Eradicated(infectionType);

            if (diseaseEradicated == false)
            {
                PD_InfectionReport initialReport = new PD_InfectionReport(
                    false, // not game setup
                    player,
                    infectionCard.City,
                    infectionCard.Type,
                    1
                    );

                PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                    this,
                    infectionCard.City,
                    1,
                    initialReport,
                    false
                    );

                InfectionReports.Add(finalReport);

                if (finalReport.FailureReason == InfectionFailureReasons.notEnoughDiseaseCubes)
                {
                    GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection = true;
                }
            }

            // remove the infection card from the active infection cards pile
            Cards.ActiveInfectionCards.Remove(infectionCard);
            Cards.DeckOfDiscardedInfectionCards.Add(infectionCard);

        }
        #endregion

        public void UpdateAvailablePlayerActions()
        {
            CurrentAvailablePlayerActions = this.FindAvailable_PlayerActions();
        }

        public void ApplySpecificPlayerAction(
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
                    ApplySpecificPlayerAction(
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

        public void ApplySpecificMacro(
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
                            ApplySpecificPlayerAction(
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
                        ApplySpecificPlayerAction(
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
                        ApplySpecificPlayerAction(
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

        public void OverrideStartTime()
        {
            StartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// This method is called when entering the GameLost or the GameWon states
        /// </summary>
        public void OverrideEndTime()
        {
            EndTime = DateTime.UtcNow;
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
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_Game)otherObject;

            if (this.GameSettings.Equals(other.GameSettings) == false)
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
            else if (this.PlayerPawnsPerPlayerID.Dictionary_Equal(other.PlayerPawnsPerPlayerID) == false)
            {
                return false;
            }
            else if (this.RoleCardsPerPlayerID.Dictionary_Equal(other.RoleCardsPerPlayerID) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + GameSettings.GetHashCode();


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
