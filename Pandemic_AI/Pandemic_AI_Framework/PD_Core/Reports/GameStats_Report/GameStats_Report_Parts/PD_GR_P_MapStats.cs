using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_GR_P_MapStats : PD_Report_Part
    {
        public int NumDiseaseCubesOnMap_AllTypes { get; private set; }
        public int NumDiseaseCubesOnMap_Type0 { get; private set; }
        public int NumDiseaseCubesOnMap_Type1 { get; private set; }
        public int NumDiseaseCubesOnMap_Type2 { get; private set; }
        public int NumDiseaseCubesOnMap_Type3 { get; private set; }
        public int NumResearchStationsOnMap { get; private set; }

        public int NumCitiesWithThreeSameTypeCubes { get; private set; }
        public int NumCitiesWithTwoSameTypeCubes { get; private set; }
        public int NumCitiesWithOneSameTypeCube { get; private set; }
        public int NumCitiesWithZeroSameTypeCubes { get; private set; }

        public int NumCitiesWithThreeSameTypeCubes_Type0 { get; private set; }
        public int NumCitiesWithThreeSameTypeCubes_Type1 { get; private set; }
        public int NumCitiesWithThreeSameTypeCubes_Type2 { get; private set; }
        public int NumCitiesWithThreeSameTypeCubes_Type3 { get; private set; }

        public int NumCitiesWithTwoSameTypeCubes_Type0 { get; private set; }
        public int NumCitiesWithTwoSameTypeCubes_Type1 { get; private set; }
        public int NumCitiesWithTwoSameTypeCubes_Type2 { get; private set; }
        public int NumCitiesWithTwoSameTypeCubes_Type3 { get; private set; }

        public int NumCitiesWithOneSameTypeCube_Type0 { get; private set; }
        public int NumCitiesWithOneSameTypeCube_Type1 { get; private set; }
        public int NumCitiesWithOneSameTypeCube_Type2 { get; private set; }
        public int NumCitiesWithOneSameTypeCube_Type3 { get; private set; }

        public PD_GR_P_MapStats(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            Update(game, pathFinder);
        }

        public override void Update(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            // count number of disease cubes on the map
            // also count the same, per type

            NumDiseaseCubesOnMap_Type0 = 0;
            NumDiseaseCubesOnMap_Type1 = 0;
            NumDiseaseCubesOnMap_Type2 = 0;
            NumDiseaseCubesOnMap_Type3 = 0;
            foreach (var city in game.map.cities)
            {
                var diseaseCubesOfType_0_onCity = game.GQ_Find_InfectionCubes_OfType_OnCity(city,0);
                var diseaseCubesOfType_1_onCity = game.GQ_Find_InfectionCubes_OfType_OnCity(city,1);
                var diseaseCubesOfType_2_onCity = game.GQ_Find_InfectionCubes_OfType_OnCity(city,2);
                var diseaseCubesOfType_3_onCity = game.GQ_Find_InfectionCubes_OfType_OnCity(city,3);

                NumDiseaseCubesOnMap_Type0 += diseaseCubesOfType_0_onCity;
                NumDiseaseCubesOnMap_Type1 += diseaseCubesOfType_1_onCity;
                NumDiseaseCubesOnMap_Type2 += diseaseCubesOfType_2_onCity;
                NumDiseaseCubesOnMap_Type3 += diseaseCubesOfType_3_onCity;
            }

            NumDiseaseCubesOnMap_AllTypes =
                NumDiseaseCubesOnMap_Type0 +
                NumDiseaseCubesOnMap_Type1 +
                NumDiseaseCubesOnMap_Type2 +
                NumDiseaseCubesOnMap_Type3;


            // count the number of cities that have three, two, one or zero saame type cubes
            // allso the same, per type!
            NumCitiesWithZeroSameTypeCubes = 0;
            NumCitiesWithOneSameTypeCube = 0;
            NumCitiesWithTwoSameTypeCubes = 0;
            NumCitiesWithThreeSameTypeCubes = 0;

            NumCitiesWithOneSameTypeCube_Type0 = 0;
            NumCitiesWithOneSameTypeCube_Type1 = 0;
            NumCitiesWithOneSameTypeCube_Type2 = 0;
            NumCitiesWithOneSameTypeCube_Type3 = 0;

            NumCitiesWithTwoSameTypeCubes_Type0 = 0;
            NumCitiesWithTwoSameTypeCubes_Type1 = 0;
            NumCitiesWithTwoSameTypeCubes_Type2 = 0;
            NumCitiesWithTwoSameTypeCubes_Type3 = 0;

            NumCitiesWithThreeSameTypeCubes_Type0 = 0;
            NumCitiesWithThreeSameTypeCubes_Type1 = 0;
            NumCitiesWithThreeSameTypeCubes_Type2 = 0;
            NumCitiesWithThreeSameTypeCubes_Type3 = 0;

            foreach (var city in game.map.cities)
            {
                // type 0 cubes on that city?
                int numCubes_Type0 = game.GQ_InfectionCubes_OfType_OnCity(
                    city,
                    0
                    );
                if (numCubes_Type0 == 1)
                {
                    NumCitiesWithOneSameTypeCube++;
                    NumCitiesWithOneSameTypeCube_Type0++;
                }
                if (numCubes_Type0 == 2)
                {
                    NumCitiesWithTwoSameTypeCubes++;
                    NumCitiesWithTwoSameTypeCubes_Type0++;
                }
                if (numCubes_Type0 == 3)
                {
                    NumCitiesWithThreeSameTypeCubes++;
                    NumCitiesWithThreeSameTypeCubes_Type0++;
                }

                // type 1 cubes on that city?
                int numCubes_Type1 = game.GQ_InfectionCubes_OfType_OnCity(city,1);
                if (numCubes_Type1 == 1)
                {
                    NumCitiesWithOneSameTypeCube++;
                    NumCitiesWithOneSameTypeCube_Type1++;
                }
                if (numCubes_Type1 == 2)
                {
                    NumCitiesWithTwoSameTypeCubes++;
                    NumCitiesWithTwoSameTypeCubes_Type1++;
                }
                if (numCubes_Type1 == 3)
                {
                    NumCitiesWithThreeSameTypeCubes++;
                    NumCitiesWithThreeSameTypeCubes_Type1++;
                }

                // type 2 cubes on that city?
                int numCubes_Type2 = PD_Game_Queries.GQ_InfectionCubes_OfType_OnCity(
                    game,
                    city,
                    2
                    );
                if (numCubes_Type2 == 1)
                {
                    NumCitiesWithOneSameTypeCube++;
                    NumCitiesWithOneSameTypeCube_Type2++;
                }
                if (numCubes_Type2 == 2)
                {
                    NumCitiesWithTwoSameTypeCubes++;
                    NumCitiesWithTwoSameTypeCubes_Type2++;
                }
                if (numCubes_Type2 == 3)
                {
                    NumCitiesWithThreeSameTypeCubes++;
                    NumCitiesWithThreeSameTypeCubes_Type2++;
                }

                // type 3 cubes on that city?
                int numCubes_Type3 = PD_Game_Queries.GQ_InfectionCubes_OfType_OnCity(
                    game,
                    city,
                    3
                    );
                if (numCubes_Type3 == 1)
                {
                    NumCitiesWithOneSameTypeCube++;
                    NumCitiesWithOneSameTypeCube_Type3++;
                }
                if (numCubes_Type3 == 2)
                {
                    NumCitiesWithTwoSameTypeCubes++;
                    NumCitiesWithTwoSameTypeCubes_Type3++;
                }
                if (numCubes_Type3 == 3)
                {
                    NumCitiesWithThreeSameTypeCubes++;
                    NumCitiesWithThreeSameTypeCubes_Type3++;
                }

                if (
                    numCubes_Type0 == 0
                    && 
                    numCubes_Type1 == 0
                    && 
                    numCubes_Type2 == 0
                    && 
                    numCubes_Type3 == 0
                    )
                {
                    NumCitiesWithZeroSameTypeCubes++;
                }
            }
            
            // count number of research stations on the map
            NumResearchStationsOnMap = 0;
            foreach (var city in game.map.cities)
            {
                if(game.GQ_Is_City_ResearchStation(city))
                    NumResearchStationsOnMap++;
            }
        }
    }
}
