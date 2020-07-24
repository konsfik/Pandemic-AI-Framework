﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// The path finder's sole purpose of existence is to provide
    /// information about the shortest paths, from city to city,
    /// taking into consideration the existing research stations 
    /// that are placed on the game's map. 
    /// </summary>
    [Serializable]
    public class PD_AI_PathFinder
    {

        private Dictionary<PD_CityEdge_Directed, List<PD_City>> _shortestPaths;


        public Dictionary<PD_CityEdge_Directed, List<PD_City>> ShortestPaths { get { return _shortestPaths; } }

        public PD_AI_PathFinder()
        {
            string game_creation_data_file_path = System.IO.Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "GameCreationData",
                    "gameCreationData.csv"
                );

            string game_creation_data = PD_IO_Utilities.ReadFile(game_creation_data_file_path);
            // generate a random game
            PD_Game dummy_game = PD_GameCreator.CreateNewGame(
                4,
                0,
                game_creation_data,
                false
                );


            ComputeShortestPaths(dummy_game.Map);
        }

        /// <summary>
        /// Shortest paths are computed at the beginning of the game, 
        /// as well as when a new research station is added. 
        /// </summary>
        private void ComputeShortestPaths(PD_Map map)
        {
            _shortestPaths = new Dictionary<PD_CityEdge_Directed, List<PD_City>>();
            foreach (var root in map.Cities)
            {
                foreach (var destination in map.Cities)
                {
                    if (root != destination)
                    {
                        List<PD_City> path = ComputeShortestPath(
                            root,
                            destination,
                            map
                            );
                        _shortestPaths.Add(new PD_CityEdge_Directed(root, destination), path);
                    }
                }
            }
        }

        /// <summary>
        /// Computes the shortest path between two cities of a given map, on demand.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private List<PD_City> ComputeShortestPath(
            PD_City root,
            PD_City destination,
            PD_Map map
            )
        {
            if (root == destination)
            {
                throw new System.Exception("root equals destination");
            }
            if (map.Cities.Any(x => x == root) == false)
            {
                throw new System.Exception("map does not contain root city");
            }
            if (map.Cities.Any(x => x == destination) == false)
            {
                throw new System.Exception("map does not contain destination city");
            }

            Dictionary<PD_City, PD_City> predecessors = new Dictionary<PD_City, PD_City>();
            PD_City dummyCity = new PD_City(-1, 0, "o", new PD_Point());
            foreach (var city in map.Cities)
            {
                predecessors.Add(city, dummyCity);
            }

            List<PD_City> visitedCities = new List<PD_City>();

            Queue<PD_City> searchQueue = new Queue<PD_City>();

            searchQueue.Enqueue(root);
            visitedCities.Add(root);
            bool foundDestination = false;

            while (searchQueue.Count > 0 && foundDestination == false)
            {
                var current = searchQueue.Dequeue();

                var neighbors = map.CityNeighbors_PerCityID[current.ID];

                var unvisitedNeighbors = neighbors.FindAll(
                    x =>
                    visitedCities.Contains(x) == false
                    );

                foreach (var neighbor in unvisitedNeighbors)
                {
                    predecessors[neighbor] = current;
                    searchQueue.Enqueue(neighbor);
                    visitedCities.Add(neighbor);

                    if (neighbor == destination)
                    {
                        foundDestination = true;
                        break;
                    }
                }
            }

            List<PD_City> shortestPath = new List<PD_City>();

            bool pathFinished = false;
            var currentPathPosition = destination;
            shortestPath.Add(currentPathPosition);
            while (pathFinished == false)
            {
                var predecessor = predecessors[currentPathPosition];
                if (predecessor != dummyCity)
                {
                    shortestPath.Add(predecessor);
                    currentPathPosition = predecessor;
                }
                else
                {
                    pathFinished = true;
                }
            }

            shortestPath.Reverse();

            return shortestPath;

        }

        public int GetPrecalculatedShortestDistance(
            PD_Game game,
            List<PD_City> researchStationCities,
            PD_City root,
            PD_City destination
            )
        {
            return GetPrecalculatedShortestPath(
                game,
                researchStationCities,
                root,
                destination
                ).Count - 1;
        }

        public List<PD_City> GetPrecalculatedShortestPath(
            PD_Game game,
            List<PD_City> researchStationCities,
            PD_City root,
            PD_City destination
            )
        {
            if (game.Map.Cities.Contains(root) == false)
            {
                throw new System.Exception("root not included in map cities");
            }
            if (game.Map.Cities.Contains(destination) == false)
            {
                throw new System.Exception("destination not included in map cities");
            }

            if (root == destination)
            {
                return new List<PD_City>() { root };
            }

            var simpleWalkPath = _shortestPaths[new PD_CityEdge_Directed(root, destination)];

            if (simpleWalkPath == null)
            {
                throw new System.Exception("null path");
            }
            if (researchStationCities.Count <= 1)
            {
                return simpleWalkPath;
            }
            else
            {
                bool rootIsResearchStation = PD_Game_Queries.GQ_Is_City_ResearchStation(game, root);
                bool destinationIsResearchStation = PD_Game_Queries.GQ_Is_City_ResearchStation(game, destination);
                if (rootIsResearchStation && destinationIsResearchStation)
                {
                    return new List<PD_City>() {
                        root,
                        destination
                    };
                }
                else if (rootIsResearchStation && !destinationIsResearchStation)
                {
                    PD_City researchStationClosestToDestination =
                        Find_RS_ClosestToCity(
                            game,
                            researchStationCities,
                            destination
                            );
                    if (researchStationClosestToDestination == root)
                    {
                        return simpleWalkPath;
                    }
                    else
                    {
                        List<PD_City> rsPath = new List<PD_City>();
                        rsPath.Add(root);
                        //rsPath.Add(_researchStationClosestToDestination);
                        rsPath.AddRange(
                            _shortestPaths[
                                new PD_CityEdge_Directed(
                                    researchStationClosestToDestination,
                                    destination
                                    )]
                            );
                        if (rsPath.Count < simpleWalkPath.Count)
                        {
                            return rsPath;
                        }
                        return simpleWalkPath;
                    }
                }
                else if (
                    rootIsResearchStation == false
                    && destinationIsResearchStation == true
                    )
                {
                    PD_City _researchStationClosestToRoot =
                        Find_RS_ClosestToCity(
                            game,
                            researchStationCities,
                            root
                            );
                    if (_researchStationClosestToRoot == destination)
                    {
                        return simpleWalkPath;
                    }
                    else
                    {
                        List<PD_City> rsPath = new List<PD_City>();
                        rsPath.AddRange(
                            _shortestPaths[new PD_CityEdge_Directed(
                                root,
                                _researchStationClosestToRoot
                                )]
                            );
                        rsPath.Add(destination);
                        if (rsPath.Count < simpleWalkPath.Count)
                        {
                            return rsPath;
                        }
                        return simpleWalkPath;
                    }
                }
                else
                // rootIsResearchStation == false
                // && destinationIsResearchStation == false
                {
                    PD_City _researchStationClosestToRoot =
                        Find_RS_ClosestToCity(
                            game,
                            researchStationCities,
                            root
                            );
                    PD_City _researchStationClosestToDestination =
                        Find_RS_ClosestToCity(
                            game,
                            researchStationCities,
                            destination
                            );

                    if (
                    _researchStationClosestToRoot == _researchStationClosestToDestination
                    )
                    {
                        return simpleWalkPath;
                    }
                    else
                    {
                        List<PD_City> part1 = _shortestPaths[new PD_CityEdge_Directed(
                            root,
                            _researchStationClosestToRoot
                            )];

                        List<PD_City> part2 = _shortestPaths[new PD_CityEdge_Directed(
                            _researchStationClosestToDestination,
                            destination
                            )];

                        List<PD_City> rsRoute = new List<PD_City>();
                        rsRoute.AddRange(part1);
                        rsRoute.AddRange(part2);

                        if (rsRoute.Count < simpleWalkPath.Count)
                        {
                            return rsRoute;
                        }

                        return simpleWalkPath;
                    }
                }
            }
        }

        public List<PD_City> Find_AllCities_Within_SpecificRange_From_ReferenceCity(
            PD_Game game,
            PD_City referenceCity,
            List<PD_City> researchStationCities,
            int range
            )
        {
            //if (range < 1)
            //{
            //    throw new System.Exception("range must be greater than zero!");
            //}
            List<PD_City> citiesWithinSpecificRange = new List<PD_City>();
            foreach (var city in game.Map.Cities)
            {
                if (city != referenceCity)
                {
                    int distanceBetween = GetPrecalculatedShortestDistance(
                        game,
                        researchStationCities,
                        referenceCity,
                        city
                        );
                    if (distanceBetween <= range)
                    {
                        citiesWithinSpecificRange.Add(city);
                    }
                }
            }
            return citiesWithinSpecificRange;
        }

        public List<PD_City> Find_AllCities_Of_SpecificDistance_From_ReferenceCity(
            PD_Game game,
            PD_City referenceCity,
            List<PD_City> researchStationCities,
            int distance
            )
        {
            //if (distance < 1)
            //{
            //    throw new System.Exception("distance must be 1 or greater");
            //}
            List<PD_City> citiesOfSpecifiedDistanceFromReferenceCity = new List<PD_City>();
            foreach (var city in game.Map.Cities)
            {
                if (city != referenceCity)
                {
                    int distanceBetween = GetPrecalculatedShortestDistance(
                        game,
                        researchStationCities,
                        referenceCity,
                        city
                        );
                    if (distanceBetween == distance)
                    {
                        citiesOfSpecifiedDistanceFromReferenceCity.Add(city);
                    }
                }
            }
            return citiesOfSpecifiedDistanceFromReferenceCity;
        }

        /// <summary>
        /// Finds all the cities whose minimum distance from ANY research station
        /// is equal or greater than a specified value (minimumAllowedDistance).
        /// </summary>
        /// <param name="game"></param>
        /// <param name="minimumAllowedDistance"></param>
        /// <returns></returns>
        public List<PD_City> FindCitiesThatAreAwayFromResearchStations(
            PD_Game game,
            int minimumAllowedDistance
            )
        {
            if (minimumAllowedDistance < 1)
            {
                throw new System.Exception("minimum allowed distance should be at least 1");
            }
            var researchStationCities = PD_Game_Queries.GQ_Find_ResearchStationCities(game);
            var allCities = game.Map.Cities;
            var nonResearhcStationCities = allCities.FindAll(
                x =>
                researchStationCities.Contains(x) == false
                );

            var citiesAwayFromResearchStations = new List<PD_City>();
            foreach (var nonRSCity in nonResearhcStationCities)
            {
                PD_City researchStationClosestToCity = Find_RS_ClosestToCity(
                    game,
                    researchStationCities,
                    nonRSCity
                    );

                int distance = GetPrecalculatedShortestDistance(
                    game,
                    researchStationCities,
                    nonRSCity,
                    researchStationClosestToCity
                    );

                if (distance >= minimumAllowedDistance)
                {
                    citiesAwayFromResearchStations.Add(nonRSCity);
                }
            }
            return citiesAwayFromResearchStations;
        }

        public PD_City Find_RS_ClosestToCity(
            PD_Game game,
            List<PD_City> researchStationCities,
            PD_City city
            )
        {
            if (researchStationCities.Contains(city))
            {
                return city;
            }
            else
            {
                var minDist = 10000000;
                var minRoute = new List<PD_City>();
                foreach (var rsCity in researchStationCities)
                {
                    var route = _shortestPaths[new PD_CityEdge_Directed(city, rsCity)];

                    if (route.Count < minDist)
                    {
                        minDist = route.Count;
                        minRoute = route;
                    }
                }
                return minRoute[minRoute.Count - 1];
            }
        }

    }
}
