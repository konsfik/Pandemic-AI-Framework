using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_Operators
    {
        public static void GO_Randomize_HiddenState(
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
            game.cards.divided_deck_of_infection_cards.ShuffleAllSubListsElements(randomness_provider);
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
            int num_SubLists = game.cards.divided_deck_of_player_cards.Count;
            List<int> subLists_Sizes = new List<int>();
            List<bool> subLists_IncludeEpidemic = new List<bool>();
            foreach (List<int> subList in game.cards.divided_deck_of_player_cards)
            {
                if (subList.Any(x => x >= 128))
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
            var allPlayerCards_IncludingEpidemics = game.cards.divided_deck_of_player_cards.DrawAllElementsOfAllSubListsAsOneList();
            var playerCityCards = allPlayerCards_IncludingEpidemics.FindAll(
                x =>
                x < game.map.number_of_cities
                );
            var epidemicCards = allPlayerCards_IncludingEpidemics.FindAll(
                x =>
                x >= 128
                );

            // compose the new randomized list of lists
            playerCityCards.Shuffle(randomness_provider);
            for (int subList_Index = 0; subList_Index < num_SubLists; subList_Index++)
            {
                int sublist_Size = subLists_Sizes[subList_Index];
                bool subList_IncludesEpidemic = subLists_IncludeEpidemic[subList_Index];
                List<int> newSubList = new List<int>();
                for (int item_Index = 0; item_Index < sublist_Size; item_Index++)
                {
                    newSubList.Add(playerCityCards.DrawOneRandom(randomness_provider));
                }
                if (subList_IncludesEpidemic)
                {
                    newSubList.Add(epidemicCards.DrawOneRandom(randomness_provider));
                }
                newSubList.Shuffle(randomness_provider);
                game.cards.divided_deck_of_player_cards.Add(newSubList);
            }
        }

        public static void GO_PlaceAllPawnsOnAtlanta(
            this PD_Game game
            )
        {
            int atlanta = 0;
            foreach (var player in game.players)
            {
                game.map_elements.location__per__player[player] = atlanta;
            }
        }

        /// <summary>
        /// The process of infecting a city
        /// </summary>
        /// <param name="game"></param>
        /// <param name="target_city"></param>
        /// <param name="infection_size"></param>
        /// <param name="existingReport"></param>
        /// <param name="infectionDuringGameSetup"></param>
        /// <returns></returns>
        public static PD_InfectionReport GO_InfectCity(
            this PD_Game game,
            int target_city,
            int infection_size,
            PD_InfectionReport existingReport,
            bool infectionDuringGameSetup
            )
        {
            int infection_type = existingReport.InfectionType;

            if (infectionDuringGameSetup == false)
            {

                int medic_location = game.GQ_Medic_Location();

                bool currentDiseaseIsCured = game.GQ_Is_DiseaseCured_OR_Eradicated(infection_type);

                bool medicIsAtInfectionLocation = medic_location == target_city;

                if (
                    currentDiseaseIsCured
                    &&
                    medicIsAtInfectionLocation
                    )
                {
                    existingReport.AddInfectionPreventedByMedic(target_city);
                    return existingReport;
                }
            }

            existingReport.AddInfectedCity(target_city);

            int occupied_infection_positions_on_city =
                game.GQ_Num_InfectionCubes_OfType_OnCity(target_city, infection_type);

            int remaining_infection_positions_on_city =
                3 - occupied_infection_positions_on_city;

            int available_infection_cubes_this_type =
                game.GQ_Num_AvailableInfectionCubes_OfType(infection_type);

            bool enough_available_infection_cubes =
                available_infection_cubes_this_type >= infection_size;

            bool city_causes_outbreak =
                remaining_infection_positions_on_city < infection_size;

            // CASE 1: CITY DOES NOT CAUSE AN OUTBREAK
            if (city_causes_outbreak == false)
            {
                // NOT ENOUGH CUBES FOR THIS INFECTION... GAME LOST
                if (enough_available_infection_cubes == false)
                {
                    // place the remaining inactive cubes, either way...
                    for (int i = 0; i < available_infection_cubes_this_type; i++)
                    {
                        GO_Place_InfectionCube_OnCity(game, target_city, infection_type);
                    }
                    // game lost...
                    existingReport.SetFailureReason(InfectionFailureReasons.notEnoughDiseaseCubes);
                    return existingReport;
                }

                // ENOUGH CUBES TO COMPLETE THIS INFECTION, JUST PROCEED!
                for (int i = 0; i < infection_size; i++)
                {
                    GO_Place_InfectionCube_OnCity(game, target_city, infection_type);
                }
                existingReport.AddUsedCubes(infection_size);
                return existingReport;

            }

            // CASE 2: CITY DOES CAUSE AN OUTBREAK!!!
            bool enoughCubesToFillUpCity = available_infection_cubes_this_type >= remaining_infection_positions_on_city;

            existingReport.AddCityThatCausedOutbreak(target_city);

            if (enoughCubesToFillUpCity == false)
            {
                // place the remaining inactive cubes, either way...
                for (int i = 0; i < available_infection_cubes_this_type; i++)
                {
                    GO_Place_InfectionCube_OnCity(game, target_city, infection_type);
                }
                // game lost
                existingReport.SetFailureReason(InfectionFailureReasons.notEnoughDiseaseCubes);
                return existingReport;
            }

            // fill up this city
            for (int i = 0; i < remaining_infection_positions_on_city; i++)
            {
                GO_Place_InfectionCube_OnCity(
                    game,
                    target_city,
                    existingReport.InfectionType
                    );
            }
            existingReport.AddUsedCubes(remaining_infection_positions_on_city);

            game.game_state_counter.IncreaseOutbreaksCounter();
            if (game.GQ_SS_DeadlyOutbreaks() == true)
            {
                existingReport.SetFailureReason(InfectionFailureReasons.maximumOutbreaksSurpassed);
                return existingReport;
            }

            var neighbors = game.map.neighbors__per__city[target_city]; // cityToInfect.AdjacentCities;

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

        public static void GO_Place_InfectionCube_OnCity(
            this PD_Game game,
            int city,
            int infection_type
            )
        {
            game.map_elements.available_infection_cubes__per__type[infection_type] -= 1;
            game.map_elements.infections__per__type__per__city[city][infection_type] += 1;
        }

        public static void GO_Place_ResearchStation_OnCity(
            this PD_Game game,
            int city
            )
        {
            game.map_elements.available_research_stations--;
            game.map_elements.research_stations__per__city[city] = true;
        }

        public static void GO_MovePawn_ToCity(
            this PD_Game game,
            int player,
            int targetCity
            )
        {
            game.map_elements.location__per__player[player] = targetCity;
        }

        public static void GO_PlayerDiscardsPlayerCard(
            this PD_Game game,
            int player,
            int playerCardToDiscard
            )
        {
            game.cards.player_hand__per__player[player].Remove(playerCardToDiscard);
            game.cards.deck_of_discarded_player_cards.Add(playerCardToDiscard);
        }

        public static void GO_Remove_One_InfectionCube_OfType_FromCity(
            this PD_Game game,
            int city,
            int treatType
            )
        {
            // remove the cube from the city
            game.map_elements.infections__per__type__per__city[city][treatType]--;
            // put the cubes back in the inactive container
            game.map_elements.available_infection_cubes__per__type[treatType]++;
        }

        public static void GO_Remove_All_InfectionCubes_OfType_FromCity(
            this PD_Game game,
            int city,
            int diseaseType
            )
        {
            // count the cubes
            int num_cubes = game.map_elements.infections__per__type__per__city[city][diseaseType];

            // remove the cubes from the city
            game.map_elements.infections__per__type__per__city[city][diseaseType] -= num_cubes;
            // put the cubes back in the inactive container
            game.map_elements.available_infection_cubes__per__type[diseaseType] += num_cubes;
        }

    }
}
