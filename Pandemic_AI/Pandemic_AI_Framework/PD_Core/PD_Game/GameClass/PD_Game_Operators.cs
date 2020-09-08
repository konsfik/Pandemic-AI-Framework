using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_Operators
    {
        public static void GO_RandomizeGame(
            this PD_Game game,
            Random randomness_provider
            )
        {
            game.GO_Randomize_PlayerCards_Deck(randomness_provider);
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);
        }

        public static void GO_Randomize_InfectionCards_Deck(
            this PD_Game game,
            Random randomness_provider
            )
        {
            game.Cards.DividedDeckOfInfectionCards.ShuffleAllSubListsElements(randomness_provider);
        }

        /// <summary>
        /// Randomizes the current divided deck of player cards of a game.
        /// </summary>
        /// <param name="game"></param>
        public static void GO_Randomize_PlayerCards_Deck(
            this PD_Game game,
            Random randomness_provider
            )
        {
            // gather primary info:
            int num_SubLists = game.Cards.DividedDeckOfPlayerCards.Count;
            List<int> subLists_Sizes = new List<int>();
            List<bool> subLists_IncludeEpidemic = new List<bool>();
            foreach (List<PD_PlayerCardBase> subList in game.Cards.DividedDeckOfPlayerCards)
            {
                if (subList.Any(x => x is PD_EpidemicCard))
                {
                    subLists_IncludeEpidemic.Add(true);
                    subLists_Sizes.Add(subList.Count - 1);
                }
                else
                {
                    subLists_IncludeEpidemic.Add(false);
                    subLists_Sizes.Add(subList.Count);
                }
            }

            // get the material
            var allPlayerCards_IncludingEpidemics = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            var playerCityCards = allPlayerCards_IncludingEpidemics.FindAll(
                x =>
                x.GetType() == typeof(PD_CityCard)
                );
            var epidemicCards = allPlayerCards_IncludingEpidemics.FindAll(
                x =>
                x.GetType() == typeof(PD_EpidemicCard)
                );

            // compose the new randomized list of lists
            playerCityCards.Shuffle(randomness_provider);
            for (int subList_Index = 0; subList_Index < num_SubLists; subList_Index++)
            {
                int sublist_Size = subLists_Sizes[subList_Index];
                bool subList_IncludesEpidemic = subLists_IncludeEpidemic[subList_Index];
                List<PD_PlayerCardBase> newSubList = new List<PD_PlayerCardBase>();
                for (int item_Index = 0; item_Index < sublist_Size; item_Index++)
                {
                    newSubList.Add(playerCityCards.DrawOneRandom(randomness_provider));
                }
                if (subList_IncludesEpidemic)
                {
                    newSubList.Add(epidemicCards.DrawOneRandom(randomness_provider));
                }
                newSubList.Shuffle(randomness_provider);
                game.Cards.DividedDeckOfPlayerCards.Add(newSubList);
            }
        }

        public static void GO_PlaceAllPawnsOnAtlanta(
            this PD_Game game
            )
        {
            int atlanta = game.GQ_Find_CityByName("Atlanta");
            foreach (var player in game.Players)
            {
                game.MapElements.location__per__player[player] = atlanta;
            }
        }

        /// <summary>
        /// The act of infecting a city.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="cityToInfect"></param>
        /// <param name="num_CubesToPlace"></param>
        /// <param name="existingReport"></param>
        /// <returns></returns>
        public static PD_InfectionReport GO_InfectCity(
            this PD_Game game,
            int cityToInfect,
            int num_CubesToPlace,
            PD_InfectionReport existingReport,
            bool infectionDuringGameSetup
            )
        {
            int currentInfectionType = existingReport.InfectionType;

            if (infectionDuringGameSetup == false)
            {

                int medic_location = game.GQ_Find_Medic_Location();

                bool currentDiseaseIsCured = game.GQ_Is_DiseaseCured_OR_Eradicated(currentInfectionType);

                bool medicIsAtInfectionLocation = medic_location == cityToInfect;

                if (
                    currentDiseaseIsCured
                    &&
                    medicIsAtInfectionLocation
                    )
                {
                    existingReport.AddInfectionPreventedByMedic(cityToInfect);
                    return existingReport;
                }
            }

            existingReport.AddInfectedCity(cityToInfect);

            int occupied_infection_positions =
                game.GQ_InfectionCubes_OfType_OnCity(
                    cityToInfect,
                    currentInfectionType
                    );

            int free_infection_positions =
                3 - occupied_infection_positions;

            int available_cubes_of_type =
                game.Num_InactiveInfectionCubes_OfType(
                    currentInfectionType
                    );

            bool enoughInactiveCubes =
                available_cubes_of_type >= num_CubesToPlace;

            bool cityCausesOutbreak =
                free_infection_positions < num_CubesToPlace;

            // CASE 1: CITY DOES NOT CAUSE AN OUTBREAK
            if (cityCausesOutbreak == false)
            {
                // NOT ENOUGH CUBES FOR THIS INFECTION... GAME LOST
                if (enoughInactiveCubes == false)
                {
                    // place the remaining inactive cubes, either way...
                    for (int i = 0; i < available_cubes_of_type; i++)
                    {
                        GO_PA_PlaceInfectionCubeOnCity(game, cityToInfect, currentInfectionType);
                    }
                    // game lost...
                    existingReport.SetFailureReason(InfectionFailureReasons.notEnoughDiseaseCubes);
                    return existingReport;
                }

                // ENOUGH CUBES TO COMPLETE THIS INFECTION, JUST PROCEED!
                for (int i = 0; i < num_CubesToPlace; i++)
                {
                    GO_PA_PlaceInfectionCubeOnCity(game, cityToInfect, currentInfectionType);
                }
                existingReport.AddUsedCubes(num_CubesToPlace);
                return existingReport;

            }

            // CASE 2: CITY DOES CAUSE AN OUTBREAK!!!
            bool enoughCubesToFillUpCity = available_cubes_of_type >= free_infection_positions;

            existingReport.AddCityThatCausedOutbreak(cityToInfect);

            if (enoughCubesToFillUpCity == false)
            {
                // place the remaining inactive cubes, either way...
                for (int i = 0; i < available_cubes_of_type; i++)
                {
                    GO_PA_PlaceInfectionCubeOnCity(game, cityToInfect, currentInfectionType);
                }
                // game lost
                existingReport.SetFailureReason(InfectionFailureReasons.notEnoughDiseaseCubes);
                return existingReport;
            }

            // fill up this city
            for (int i = 0; i < free_infection_positions; i++)
            {
                GO_PA_PlaceInfectionCubeOnCity(
                    game,
                    cityToInfect,
                    existingReport.InfectionType
                    );
            }
            existingReport.AddUsedCubes(free_infection_positions);

            game.GameStateCounter.IncreaseOutbreaksCounter();
            if (game.GQ_SS_DeadlyOutbreaks() == true)
            {
                existingReport.SetFailureReason(InfectionFailureReasons.maximumOutbreaksSurpassed);
                return existingReport;
            }

            var neighbors = game.Map.neighbors__per__city[cityToInfect]; // cityToInfect.AdjacentCities;

            var neighborsThatHaveNotCausedAnOutbreak = neighbors.FindAll(
                x =>
                existingReport.CitiesThatHaveCausedOutbreaks.Any(xx => xx == x) == false
                );

            foreach (var neighbor in neighborsThatHaveNotCausedAnOutbreak)
            {
                existingReport = GO_InfectCity(
                    game,
                    neighbor,
                    1,
                    existingReport,
                    infectionDuringGameSetup
                    );

                if (existingReport.FailureReason != InfectionFailureReasons.none)
                {
                    return existingReport;
                }
            }
            return existingReport;

        }

        public static void GO_PA_PlaceInfectionCubeOnCity(
            this PD_Game game,
            int city,
            int infectionCubeType
            )
        {
            game.MapElements.inactive_infection_cubes__per__type[infectionCubeType] -= 1;
            game.MapElements.infections__per__type__per__city[city][infectionCubeType] += 1;
        }

        public static void GO_PlaceResearchStationOnCity(
            this PD_Game game,
            int city
            )
        {
            game.MapElements.inactive_research_stations--;
            game.MapElements.research_stations__per__city[city] = true;
        }

        public static void GO_MovePawnFromCityToCity(
            this PD_Game game,
            int player,
            int initialCity,
            int targetCity
            )
        {
            game.MapElements.location__per__player[player] = targetCity;
        }

        public static void GO_PlayerDiscardsPlayerCard(
            this PD_Game game,
            int player,
            PD_PlayerCardBase playerCardToDiscard
            )
        {
            game.Cards.PlayerCardsPerPlayerID[player].Remove(playerCardToDiscard);
            game.Cards.DeckOfDiscardedPlayerCards.Add(playerCardToDiscard);
        }

        public static void GO_Remove_One_InfectionCube_OfType_FromCity(
            this PD_Game game,
            int city,
            int treatType
            )
        {
            // remove the cube from the city
            game.MapElements.infections__per__type__per__city[city][treatType]--;
            // put the cubes back in the inactive container
            game.MapElements.inactive_infection_cubes__per__type[treatType]++;
        }

        public static void GO_Remove_All_InfectionCubes_OfType_FromCity(
            this PD_Game game,
            int city,
            int diseaseType
            )
        {
            // count the cubes
            int num_cubes = game.MapElements.infections__per__type__per__city[city][diseaseType];

            // remove the cubes from the city
            game.MapElements.infections__per__type__per__city[city][diseaseType] -= num_cubes;
            // put the cubes back in the inactive container
            game.MapElements.inactive_infection_cubes__per__type[diseaseType] += num_cubes;
        }

    }
}
