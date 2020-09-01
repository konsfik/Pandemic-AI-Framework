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