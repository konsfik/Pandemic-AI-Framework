using System;
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
        public Dictionary<int, Dictionary<int, List<int>>> shortest_path__per__destination__per__origin;

        public PD_AI_PathFinder()
        {
            Random randomness_provider = new Random();

            // generate a random game
            PD_Game dummy_game = PD_Game.Create(
                randomness_provider,
                4,
                0,
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
            shortest_path__per__destination__per__origin = new Dictionary<int, Dictionary<int, List<int>>>();
            foreach (int root in map.cities)
            {
                shortest_path__per__destination__per__origin
                    .Add(root, new Dictionary<int, List<int>>());
                foreach (var destination in map.cities)
                {
                    var path = ComputeShortestPath(
                        root,
                        destination,
                        map
                        );
                    shortest_path__per__destination__per__origin[root].Add(
                        destination,
                        path
                        );
                }
            }

        }

        /// <summary>
        /// Computes the shortest path between two cities of a given map, on demand.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private List<int> ComputeShortestPath(
            int root,
            int destination,
            PD_Map map
            )
        {
            if (root == destination)
            {
                return new List<int>() { root };
            }

            Dictionary<int, int> predecessors = new Dictionary<int, int>();
            int dummyCity = -1;
            foreach (int city in map.cities)
            {
                predecessors.Add(city, dummyCity);
            }

            List<int> visitedCities = new List<int>();

            Queue<int> searchQueue = new Queue<int>();

            searchQueue.Enqueue(root);
            visitedCities.Add(root);
            bool foundDestination = false;

            while (searchQueue.Count > 0 && foundDestination == false)
            {
                var current = searchQueue.Dequeue();

                var neighbors = map.neighbors__per__city[current];

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

            List<int> shortestPath = new List<int>();

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
            List<int> researchStationCities,
            int root,
            int destination
            )
        {
            int shortest_path_size = GetPrecalculatedShortestPath(
                game,
                researchStationCities,
                root,
                destination
                ).Count;

            return shortest_path_size - 1;
        }

        public List<int> GetPrecalculatedShortestPath(
            PD_Game game,
            List<int> researchStationCities,
            int root,
            int destination
            )
        {
            var simpleWalkPath = shortest_path__per__destination__per__origin[root][destination];

            if (researchStationCities.Count <= 1)
            {
                return simpleWalkPath;
            }
            else
            {
                bool rootIsResearchStation = game.GQ_Is_City_ResearchStation(root);
                bool destinationIsResearchStation = game.GQ_Is_City_ResearchStation(destination);
                if (rootIsResearchStation && destinationIsResearchStation)
                {
                    if (root == destination)
                    {
                        return new List<int>() { 
                            root 
                        };
                    }
                    else
                    {
                        return new List<int>() {
                            root,
                            destination
                        };
                    }
                }
                else if (rootIsResearchStation && !destinationIsResearchStation)
                {
                    int researchStationClosestToDestination =
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
                        List<int> rsPath = new List<int>();
                        rsPath.Add(root);
                        //rsPath.Add(_researchStationClosestToDestination);
                        rsPath.AddRange(
                            shortest_path__per__destination__per__origin
                            [researchStationClosestToDestination]
                            [destination]
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
                    int _researchStationClosestToRoot =
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
                        List<int> rsPath = new List<int>();
                        rsPath.AddRange(
                            shortest_path__per__destination__per__origin
                            [root]
                            [_researchStationClosestToRoot]
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
                    int _researchStationClosestToRoot =
                        Find_RS_ClosestToCity(
                            game,
                            researchStationCities,
                            root
                            );
                    int _researchStationClosestToDestination =
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
                        List<int> part1 =
                            shortest_path__per__destination__per__origin
                            [root]
                            [_researchStationClosestToRoot];

                        List<int> part2 =
                            shortest_path__per__destination__per__origin
                            [_researchStationClosestToDestination]
                            [destination];

                        List<int> rsRoute = new List<int>();
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

        public List<int> Find_AllCities_Within_SpecificRange_From_ReferenceCity(
            PD_Game game,
            int referenceCity,
            List<int> researchStationCities,
            int range
            )
        {
            //if (range < 1)
            //{
            //    throw new System.Exception("range must be greater than zero!");
            //}
            List<int> citiesWithinSpecificRange = new List<int>();
            foreach (int city in game.Map.cities)
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

        public List<int> Find_AllCities_Of_SpecificDistance_From_ReferenceCity(
            PD_Game game,
            int referenceCity,
            List<int> researchStationCities,
            int distance
            )
        {
            //if (distance < 1)
            //{
            //    throw new System.Exception("distance must be 1 or greater");
            //}
            List<int> citiesOfSpecifiedDistanceFromReferenceCity = new List<int>();
            foreach (int city in game.Map.cities)
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
        public List<int> FindCitiesThatAreAwayFromResearchStations(
            PD_Game game,
            int minimumAllowedDistance
            )
        {
            if (minimumAllowedDistance < 1)
            {
                throw new System.Exception("minimum allowed distance should be at least 1");
            }
            var researchStationCities = game.GQ_ResearchStationCities();
            var allCities = game.Map.cities;
            var nonResearhcStationCities = allCities.FindAll(
                x =>
                researchStationCities.Contains(x) == false
                );

            var citiesAwayFromResearchStations = new List<int>();
            foreach (int nonRSCity in nonResearhcStationCities)
            {
                int researchStationClosestToCity = Find_RS_ClosestToCity(
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

        public int Find_RS_ClosestToCity(
            PD_Game game,
            List<int> researchStationCities,
            int city
            )
        {
            if (researchStationCities.Contains(city))
            {
                return city;
            }
            else
            {
                var minDist = 10000000;
                var minRoute = new List<int>();
                foreach (var rsCity in researchStationCities)
                {
                    var route =
                        shortest_path__per__destination__per__origin
                        [city]
                        [rsCity];

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
