using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// Class that offers the functionality to create a new game!
    /// </summary>
    public static class PD_GameCreator
    {
        public static PD_Game CreateNewGame(
            int numberOfPlayers,        // should be 2 to 4
            int gameDifficultyLevel,    // should be 0 to 2
            string gameCreationData,
            bool autoGameSetup
            )
        {
            if (numberOfPlayers < 2 || numberOfPlayers > 4)
            {
                throw new System.Exception("The number of players should be between 2 and 4");
            }
            if (gameDifficultyLevel < 0 || gameDifficultyLevel > 2)
            {
                throw new System.Exception("The difficulty should be between 0 and 2");
            }

            char dataSeparator = ',';
            char listSeparator = ';';
            int numberOfResearchStations = 6;
            int numberOfAvailableInfectionCubesPerCity = 2;

            Dictionary<int, string> _playerIDsNames = new Dictionary<int, string>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                _playerIDsNames.Add(
                    i,
                    "Player " + i.ToString()
                    );
            }

            List<PD_Player> players = CreatePlayers(_playerIDsNames);

            List<PD_City> cities = CreateCitiesFromCityNodesData(
                gameCreationData,
                dataSeparator,
                listSeparator
                );

            List<PD_CityEdge> edges = CreateEdgesFromCityNodesData(
                gameCreationData,
                cities,
                dataSeparator,
                listSeparator
                );

            List<PD_CityCard> cityCards = CreateCityCardsFromCities(cities);
            List<PD_InfectionCard> infectionCards = CreateInfectionCardsFromCities(cities);
            List<PD_EpidemicCard> epidemicCards = CreateEpidemicCards(6);
            List<PD_ME_PlayerPawn> playerPawns = CreatePlayerPawns();
            List<PD_Role_Card> roleCards = CreateRoleCards();
            List<PD_ME_InfectionCube> infectionCubes = CreateInfectionCubesFromCities
                (
                cities,
                numberOfAvailableInfectionCubesPerCity
                );
            List<PD_ME_ResearchStation> researchStations = CreateResearchStations
                (
                numberOfResearchStations
                );

            PD_Game newGame = new PD_Game(
                gameDifficultyLevel,
                players,
                cities,
                edges,

                cityCards,
                infectionCards,
                epidemicCards,

                playerPawns,
                roleCards,

                researchStations,
                infectionCubes
                );

            if (autoGameSetup)
            {
                if (newGame.GameFSM.CurrentState.GetType() == typeof(PD_GS_Idle))
                {
                    var availableCommands = newGame.CurrentAvailablePlayerActions;
                    var selectedCommand = availableCommands[0];
                    if (selectedCommand.GetType() == typeof(PD_GA_SetupGame_Random))
                    {
                        newGame.ApplySpecificPlayerAction(selectedCommand);
                    }
                }
            }

            return newGame;
        }

        public static PD_Game CreateNewGame_SpecificRoles(
            string gameCreationData
            )
        {
            // settings
            int numberOfPlayers = 4;
            int gameDifficultyLevel = 0;


            char dataSeparator = ',';
            char listSeparator = ';';
            int numberOfResearchStations = 6;
            int numberOfAvailableInfectionCubesPerCity = 2;

            Dictionary<int, string> _playerIDsNames = new Dictionary<int, string>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                _playerIDsNames.Add(
                    i,
                    "Player " + i.ToString()
                    );
            }

            List<PD_Player> players = CreatePlayers(_playerIDsNames);

            List<PD_City> cities = CreateCitiesFromCityNodesData(
                gameCreationData,
                dataSeparator,
                listSeparator
                );

            List<PD_CityEdge> edges = CreateEdgesFromCityNodesData(
                gameCreationData,
                cities,
                dataSeparator,
                listSeparator
                );

            List<PD_CityCard> cityCards = CreateCityCardsFromCities(cities);
            List<PD_InfectionCard> infectionCards = CreateInfectionCardsFromCities(cities);
            List<PD_EpidemicCard> epidemicCards = CreateEpidemicCards(6);
            List<PD_ME_PlayerPawn> playerPawns = CreatePlayerPawns();
            List<PD_Role_Card> roleCards = CreateRoleCards();
            List<PD_ME_InfectionCube> infectionCubes = CreateInfectionCubesFromCities
                (
                cities,
                numberOfAvailableInfectionCubesPerCity
                );
            List<PD_ME_ResearchStation> researchStations = CreateResearchStations
                (
                numberOfResearchStations
                );

            PD_Game newGame = new PD_Game(
                gameDifficultyLevel,
                players,
                cities,
                edges,

                cityCards,
                infectionCards,
                epidemicCards,

                playerPawns,
                roleCards,

                researchStations,
                infectionCubes
                );

            if (newGame.GameFSM.CurrentState.GetType() == typeof(PD_GS_Idle))
            {
                var availableCommands = newGame.CurrentAvailablePlayerActions;
                var selectedCommand = availableCommands[0];
                if (selectedCommand.GetType() == typeof(PD_GA_SetupGame_Random))
                {
                    newGame.ApplySpecificPlayerAction(selectedCommand);
                }
            }

            // fix player roles here...

            // player 0 is operations expert
            newGame.RoleCardsPerPlayerID[0] = roleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Operations_Expert
                );
            newGame.PlayerPawnsPerPlayerID[0] = playerPawns.Find(
                x =>
                x.Role == PD_Player_Roles.Operations_Expert
                );

            // player 1 is medic
            newGame.RoleCardsPerPlayerID[1] = roleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Medic
                );
            newGame.PlayerPawnsPerPlayerID[1] = playerPawns.Find(
                x =>
                x.Role == PD_Player_Roles.Medic
                );

            // player 2 is scientist
            newGame.RoleCardsPerPlayerID[2] = roleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Scientist
                );
            newGame.PlayerPawnsPerPlayerID[2] = playerPawns.Find(
                x =>
                x.Role == PD_Player_Roles.Scientist
                );

            // player 3 is researcher
            newGame.RoleCardsPerPlayerID[3] = roleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Researcher
                );
            newGame.PlayerPawnsPerPlayerID[3] = playerPawns.Find(
                x =>
                x.Role == PD_Player_Roles.Researcher
                );


            // reevaluate available player actions
            newGame.UpdateAvailablePlayerActions();

            return newGame;
        }
        //public static PD_Game CreateNewGame(
        //    PD_Specific_GameSetup_Definition gameSetup
        //    )
        //{
        //    PD_Game game = new PD_Game()
        //}

        private static List<PD_City> GetNeighborCities(PD_City thisCity, List<PD_CityEdge> edges)
        {
            // find all edges that include this city 
            List<PD_CityEdge> cityEdges = edges.FindAll(
                x =>
                x.ContainsCity(thisCity)
                );

            // all neighbor ids...
            List<PD_City> neighbors = new List<PD_City>();
            foreach (var edge in cityEdges)
            {
                PD_City neighbor = edge.GetOtherCity(thisCity);
                neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private static List<PD_Player> CreatePlayers(Dictionary<int, string> playerIDsNames)
        {
            List<PD_Player> players = new List<PD_Player>();

            foreach (var pair in playerIDsNames)
            {
                PD_Player player = new PD_Player(pair.Key, pair.Value);
                players.Add(player);
            }

            return players;
        }

        private static List<PD_City> CreateCitiesFromCityNodesData(
            string data,
            char dataSeparator,
            char listSeparator
            )
        {
            List<PD_City> cities = new List<PD_City>();

            //Debug.Log(dataFile.text);
            string fullText = data;
            string[] lines = fullText.Split('\n');

            // skip first line
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] entries = line.Split(dataSeparator);
                int cityID = Int32.Parse(entries[0]);
                string cityName = entries[1];
                int cityType = Int32.Parse(entries[2]);

                string positionString = entries[4];
                //Debug.Log(positionString);
                string[] positionStringSplit = positionString.Split(listSeparator);
                float positionX = (float)Int32.Parse(positionStringSplit[0]);
                float positionY = (float)Int32.Parse(positionStringSplit[1]);
                PD_Point cityPosition = new PD_Point(positionX, positionY);

                PD_City city = new PD_City(cityID, cityType, cityName, cityPosition);

                cities.Add(city);
            }

            return cities;
        }

        private static List<PD_CityEdge> CreateEdgesFromCityNodesData(
            string dataFile,
            List<PD_City> cities,
            char dataSeparator,
            char listSeparator
            )
        {
            List<PD_CityEdge> edges = new List<PD_CityEdge>();

            string fullText = dataFile;
            string[] lines = fullText.Split('\n');

            // skip first line
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] entries = line.Split(dataSeparator);
                int cityID = Int32.Parse(entries[0]);

                string neighborIDsString = entries[3];
                string[] neighborIDsStringSplit =
                    neighborIDsString.Split(listSeparator);
                List<int> neighborIDs = new List<int>();
                foreach (string idString in neighborIDsStringSplit)
                {
                    int id = Int32.Parse(idString);
                    neighborIDs.Add(id);
                }

                foreach (var neighborID in neighborIDs)
                {
                    PD_City city1 = cities.Find(
                        x =>
                        x.ID == cityID
                        );

                    PD_City city2 = cities.Find(
                        x =>
                        x.ID == neighborID
                        );

                    PD_CityEdge edge = new PD_CityEdge(city1, city2);

                    if (!edges.Contains(edge))
                    {
                        edges.Add(edge);
                    }
                }
            }

            return edges;
        }

        private static List<PD_CityCard> CreateCityCardsFromCities(List<PD_City> cities)
        {
            List<PD_CityCard> cityCards = new List<PD_CityCard>();
            foreach (var city in cities)
            {
                PD_CityCard cc = new PD_CityCard(city.ID, city);
                cityCards.Add(cc);
            }
            return cityCards;
        }

        private static List<PD_InfectionCard> CreateInfectionCardsFromCities(List<PD_City> cities)
        {
            List<PD_InfectionCard> infectionCards = new List<PD_InfectionCard>();
            foreach (var city in cities)
            {
                PD_InfectionCard ic = new PD_InfectionCard(city.ID, city);
                infectionCards.Add(ic);
            }
            return infectionCards;
        }

        private static List<PD_EpidemicCard> CreateEpidemicCards(int numberOfCards)
        {
            List<PD_EpidemicCard> epidemicCardsList = new List<PD_EpidemicCard>();
            for (int i = 0; i < numberOfCards; i++)
            {
                PD_EpidemicCard card = new PD_EpidemicCard(i);
                epidemicCardsList.Add(card);
            }

            return epidemicCardsList;
        }

        private static List<PD_ME_PlayerPawn> CreatePlayerPawns()
        {
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

            return playerPawns;
        }

        public static List<PD_Role_Card> CreateRoleCards()
        {
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
            return roleCards;
        }

        private static List<PD_ME_ResearchStation> CreateResearchStations(int numberOfResearchStations)
        {
            List<PD_ME_ResearchStation> researchStations = new List<PD_ME_ResearchStation>();
            for (int i = 0; i < numberOfResearchStations; i++)
            {
                PD_ME_ResearchStation rs = new PD_ME_ResearchStation(i);
                researchStations.Add(rs);
            }
            return researchStations;
        }

        private static List<PD_ME_InfectionCube> CreateInfectionCubesFromCities(List<PD_City> cities, int cubesPerCity)
        {
            List<PD_ME_InfectionCube> infectionCubes = new List<PD_ME_InfectionCube>();

            int infectionCubeID = 0;
            foreach (var city in cities)
            {
                for (int i = 0; i < cubesPerCity; i++)
                {
                    PD_ME_InfectionCube ic = new PD_ME_InfectionCube(infectionCubeID, city.Type);
                    infectionCubes.Add(ic);
                    infectionCubeID++;
                }
            }

            return infectionCubes;
        }



    }
}