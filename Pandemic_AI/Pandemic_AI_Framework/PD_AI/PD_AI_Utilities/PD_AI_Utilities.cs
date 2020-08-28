using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    public static class PD_AI_Utilities
    {
        

        /// <summary>
        /// Returns a dictionary of Infection Types per City,
        /// where the 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="min_SameType_InfectionCubes"></param>
        /// <returns></returns>
        public static Dictionary<PD_City, List<int>> Find_InfectionTypesPerCity_MinCubesSameType(
            PD_Game game,
            int min_SameType_InfectionCubes
            )
        {
            if (min_SameType_InfectionCubes < 1)
            {
                throw new System.Exception("minimum cubes must be at least 1");
            }

            var infectedCities_MinCubes_AnyType =
                game.GQ_Find_InfectionTypePerCity_MinCubes(min_SameType_InfectionCubes);

            if (infectedCities_MinCubes_AnyType.Count == 0) return new Dictionary<PD_City, List<int>>();

            var infectionTypesPerCity_MinSameCubes = new Dictionary<PD_City, List<int>>();

            foreach (var city in infectedCities_MinCubes_AnyType)
            {
                List<int> infectionCubeTypes = game.GQ_Find_InfectionCubeTypes_OnCity(city);
                foreach (var type in infectionCubeTypes)
                {
                    int numCubesOfThisTypeOnThisCity = game.MapElements.InfectionCubesPerCityID[city.ID].FindAll(
                        x =>
                        x.Type == type
                        ).Count;
                    if (numCubesOfThisTypeOnThisCity > min_SameType_InfectionCubes)
                    {
                        if (infectionTypesPerCity_MinSameCubes.Keys.Contains(city))
                        {
                            infectionTypesPerCity_MinSameCubes[city].Add(type);
                        }
                        else
                        {
                            infectionTypesPerCity_MinSameCubes.Add(city, new List<int>() { type });
                        }
                    }
                }
            }

            return infectionTypesPerCity_MinSameCubes;
        }

        public static List<PD_City> Find_InfectedCities(
            PD_Game game,
            int minimumNumberOfAnyTypeInfectionCubes,
            int minimumNumberOfSameTypeInfectionCubes
            )
        {
            var infectedCities = new List<PD_City>();

            foreach (var city in game.Map.Cities)
            {
                if (
                    game.MapElements.InfectionCubesPerCityID[city.ID].Count > minimumNumberOfAnyTypeInfectionCubes
                    )
                {
                    var infectionCubesOnThisCity = game.MapElements.InfectionCubesPerCityID[city.ID];
                    List<int> infectionCubeTypes = new List<int>();
                    foreach (var ic in infectionCubesOnThisCity)
                    {
                        if (infectionCubeTypes.Contains(ic.Type) == false)
                        {
                            infectionCubeTypes.Add(ic.Type);
                        }
                    }
                    List<List<PD_ME_InfectionCube>> groups = new List<List<PD_ME_InfectionCube>>();
                    foreach (var type in infectionCubeTypes)
                    {
                        var cubesOfThisType = infectionCubesOnThisCity.FindAll(
                            x =>
                            x.Type == type
                            );
                        groups.Add(cubesOfThisType);
                    }
                    if (groups.Any(x => x.Count > minimumNumberOfSameTypeInfectionCubes))
                    {
                        infectedCities.Add(city);
                    }
                }
            }

            return infectedCities;
        }
    }
}
