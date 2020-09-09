﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_AI_GameStateEvaluation_HelpMethods
    {
        #region named game state evaluation scores
        // named scores, for ease of access / reference
        public static double Optimistic_Score_1(PD_Game game)
        {
            // percent cured diseases
            return Calculate_Percent_CuredDiseases(game);
        }

        public static double Optimistic_Score_2(PD_Game game)
        {
            // percent cured diseases - gradient
            return Calculate_Percent_CuredDiseases_Gradient(game, false);
        }

        public static double Pessimistic_Score_1(PD_Game game)
        {
            // average percent available disease cubes - min1
            return Calculate_Average_Percent_AvailableDiseaseCubes_Min_1(game);
        }

        public static double Pessimistic_Score_2(PD_Game game)
        {
            // minimum percent available disease cubes - min1
            return Calculate_Minimum_PercentAvailableDiseaseCubes_Min_1(game);
        }

        public static double Pessimistic_Score_3(PD_Game game)
        {
            // multiplied percent available disease cubes - min1
            return Calculate_Multiplied_Percent_AvailableDiseaseCubes_Min_1(game);
        }

        public static double Pessimistic_Score_4(PD_Game game)
        {
            // percent remaining outbreaks - min 1
            return Calculate_PercentRemainingOutbreaks_Min_1(game);
        }
        #endregion

        public static double Calculate_Percent_CuredDiseases(
            PD_Game game
            )
        {
            int numDiseasesCured = game.GQ_Num_Cured_or_Eradicated_DiseaseTypes();
            double percentageDiseasesCured = (double)numDiseasesCured / 4;
            return percentageDiseasesCured;
        }

        public static double Calculate_Percent_CuredDiseases_Gradient(
            PD_Game game,
            bool squared
            )
        {
            return PD_AI_CardEvaluation_Utilities.Calculate_PercentCuredDiseases_Gradient(
                game,
                squared
                );
        }

        public static double Calculate_Percent_AbilityToCureDiseases(
            PD_Game game,
            bool squared
            )
        {
            return PD_AI_CardEvaluation_Utilities.Calculate_Percent_AbilityToCureDiseases(
                game,
                squared
                );
        }

        public static double Calculate_PercentRemainingPlayerCards(
            PD_Game game
            )
        {
            int numAllCityCardsInGame = game.map.number_of_cities;
            int numAllEpidemicCardsInGame = game.game_settings.GetNumberOfEpidemicCardsToUseInGame();
            int numAllCardsInGame = numAllCityCardsInGame + numAllEpidemicCardsInGame;

            int numPlayers = game.game_state_counter.number_of_players;
            int initialNumberOfCardsPerPlayer = game.game_settings.GetNumberOfInitialCardsToDealPlayers(numPlayers);
            int initiallyDealtCards = numPlayers * initialNumberOfCardsPerPlayer;

            int initialCardsInDeck = numAllCardsInGame - initiallyDealtCards;

            int currentCardsInDeck = game.cards.divided_deck_of_player_cards.GetNumberOfElementsOfAllSubLists();

            double percentage = (double)currentCardsInDeck / (double)initialCardsInDeck;
            return percentage;
        }

        public static double Calculate_PercentRemainingOutbreaks_Min_1(
            PD_Game game
            )
        {
            int currentNumOutbreaks = game.game_state_counter.outbreaks_counter;

            // the + 1 is important! It ensures that no negative values can be returned from this score.
            int maxNumOutbreaks = game.game_settings.maximum_viable_outbreaks + 1;

            int remainingOutbreaks = maxNumOutbreaks - currentNumOutbreaks;

            return (double)remainingOutbreaks / (double)maxNumOutbreaks;
        }

        #region multiplied percent available disease cubes


        public static double Calculate_Multiplied_Percent_AvailableDiseaseCubes_Min_0(
            PD_Game game
            )
        {
            double mp = 1.0;
            for (int i = 0; i < 4; i++)
            {
                mp *= Calculate_PercentAvailableDiseaseCubes_SpecificType_Min_0(game, i);
            }
            return mp;
        }

        public static double Calculate_PercentAvailableDiseaseCubes_SpecificType_Min_0(
            PD_Game game,
            int infectionType
            )
        {
            if (infectionType > 4 || infectionType < 0)
            {
                throw new System.Exception("infection type must be between 0 and 4");
            }

            int maxNumAvailableCubesOfAnyType = 24;
            int numAvailableCubesThisType 
                = game.map_elements.inactive_infection_cubes__per__type[infectionType];
            return (double)numAvailableCubesThisType / (double)maxNumAvailableCubesOfAnyType;
        }

        public static double Calculate_Multiplied_Percent_AvailableDiseaseCubes_Min_1(
            PD_Game game
            )
        {
            double multipliedPercent = 1.0;
            for (int i = 0; i < 4; i++)
            {
                multipliedPercent *= Calculate_PercentAvailableDiseaseCubes_SpecificType_Min_1(game, i);
            }
            return multipliedPercent;
        }

        /// <summary>
        /// returns the percentage, considering the lowest value to be 1!
        /// This is important when multiplying various scores together.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="infectionType"></param>
        /// <returns></returns>
        public static double Calculate_PercentAvailableDiseaseCubes_SpecificType_Min_1(
            PD_Game game,
            int infectionType
            )
        {
            if (infectionType > 4 || infectionType < 0)
            {
                throw new System.Exception("infection type must be between 0 and 4");
            }

            int maxNumAvailableCubesOfAnyType = 25; // 24 + 1

            int numAvailableCubesThisType 
                = game.map_elements.inactive_infection_cubes__per__type[infectionType] + 1;

            return (double)numAvailableCubesThisType / (double)maxNumAvailableCubesOfAnyType;
        }
        #endregion

        public static double Calculate_Minimum_PercentAvailableDiseaseCubes_Min_0(PD_Game game)
        {
            int numCubesPerType = 24;
            List<int> inactiveCubesPerType = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int numInactiveCubesThisType 
                    = game.map_elements.inactive_infection_cubes__per__type[i];
                inactiveCubesPerType.Add(numInactiveCubesThisType);
            }
            int minValue = inactiveCubesPerType.Min();
            double percentage = (double)minValue / (double)numCubesPerType;
            return percentage;
        }

        public static double Calculate_Minimum_PercentAvailableDiseaseCubes_Min_1(PD_Game game)
        {
            int numCubesPerType = 25;
            List<int> inactiveCubesPerType = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int numInactiveCubesThisType 
                    = game.map_elements.inactive_infection_cubes__per__type[i] + 1;
                inactiveCubesPerType.Add(numInactiveCubesThisType);
            }
            int minValue = inactiveCubesPerType.Min();
            double percentage = (double)minValue / (double)numCubesPerType;
            return percentage;
        }

        public static double Calculate_Average_Percent_AvailableDiseaseCubes_Min_0(PD_Game game)
        {
            double sum = 0.0;
            for (int i = 0; i < 4; i++)
            {
                sum += Calculate_PercentAvailableDiseaseCubes_SpecificType_Min_0(game, i);
            }
            return sum / 4;
        }

        public static double Calculate_Average_Percent_AvailableDiseaseCubes_Min_1(PD_Game game)
        {
            double sum = 0.0;
            for (int i = 0; i < 4; i++)
            {
                sum += Calculate_PercentAvailableDiseaseCubes_SpecificType_Min_1(game, i);
            }
            return sum / 4;
        }

        public static double Calculate_PercentAvailableDiseaseCubes_AllTypes(PD_Game game)
        {
            int numAllCubes = 96;
            int allInactiveCubes = 0;
            for (int i = 0; i < 4; i++)
            {
                int numInactiveCubesThisType 
                    = game.map_elements.inactive_infection_cubes__per__type[i];
                allInactiveCubes += numInactiveCubesThisType;
            }
            double percentage = (double)allInactiveCubes / (double)numAllCubes;
            return percentage;
        }

        public static double Calculate_PercentInfectedCities(PD_Game game, int xCubes)
        {
            if (xCubes < 1)
            {
                throw new System.Exception("minimum must be 1");
            }
            // since there are 96 cubes in the game, the worst case scenario 
            // is that these cubes are all places by three on exactly 96 / 3 = 32 cities,
            // three cubes per city (3 * 32 = 96);
            int maxPossible = 96 / xCubes;

            int numInfCitiesWith_atLeast_X_Cubes = 0;

            foreach (int city in game.map.cities)
            {
                for (int t = 0; t < 4; t++)
                {
                    int numCubesOfTypeOnCity
                        = game.map_elements.infections__per__type__per__city[city][t];
                    if (numCubesOfTypeOnCity >= xCubes)
                    {
                        // if a city has three cubes of two different types, then it counts as two cities!!
                        numInfCitiesWith_atLeast_X_Cubes += 1;
                    }
                }
            }

            double result =
                (double)numInfCitiesWith_atLeast_X_Cubes
                /
                (double)maxPossible;

            return result;


        }


    }
}
