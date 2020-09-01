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
                if (
                    subList.Any(
                        x =>
                        x.GetType() == typeof(PD_EpidemicCard)
                        )
                    )
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
            PD_City atlanta = game.GQ_Find_CityByName("Atlanta");
            foreach (var player in game.Players)
            {
                game.MapElements.PlayerPawnsPerCityID[atlanta.ID].Add(
                    game.GQ_Find_PlayerPawn(player)
                    );
            }
        }

        public static void GO_Randomize_PlayerRoles(
            this PD_Game game,
            Random randomness_provider
            )
        {
            List<PD_Role_Card> all_Role_Cards = PD_GameCreator.CreateRoleCards();
            foreach (var player in game.Players)
            {
                PD_Role_Card selected_Role_Card = all_Role_Cards.DrawOneRandom(randomness_provider);
                // set the player role card and pawn
                game.RoleCardsPerPlayerID[player.ID] = selected_Role_Card;
                game.PlayerPawnsPerPlayerID[player.ID] = game.GameElementReferences.PlayerPawns.Find(
                    x =>
                    x.Role == selected_Role_Card.Role
                    );
            }

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
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
            PD_City cityToInfect,
            int num_CubesToPlace,
            PD_InfectionReport existingReport,
            bool infectionDuringGameSetup
            )
        {
            int currentInfectionType = existingReport.InfectionType;

            if (infectionDuringGameSetup == false)
            {

                PD_City medic_location = game.GQ_Find_Medic_Location();

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

            int num_CubesOfType_AlreadyOnCity =
                game.GQ_Count_Num_InfectionCubes_OfType_OnCity(
                    cityToInfect,
                    currentInfectionType
                    );

            int num_CubesOfType_ThatCanBePlacedOnCity =
                3 - num_CubesOfType_AlreadyOnCity;

            int num_InactiveInfectionCubes_OfType =
                game.Num_InactiveInfectionCubes_OfType(
                    currentInfectionType
                    );

            bool enoughInactiveCubes =
                num_InactiveInfectionCubes_OfType >= num_CubesToPlace;

            bool cityCausesOutbreak =
                num_CubesOfType_ThatCanBePlacedOnCity < num_CubesToPlace;

            // CASE 1: CITY DOES NOT CAUSE AN OUTBREAK
            if (cityCausesOutbreak == false)
            {
                // NOT ENOUGH CUBES FOR THIS INFECTION... GAME LOST
                if (enoughInactiveCubes == false)
                {
                    // place the remaining inactive cubes, either way...
                    for (int i = 0; i < num_InactiveInfectionCubes_OfType; i++)
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
            bool enoughCubesToFillUpCity = num_InactiveInfectionCubes_OfType >= num_CubesOfType_ThatCanBePlacedOnCity;

            existingReport.AddCityThatCausedOutbreak(cityToInfect);

            if (enoughCubesToFillUpCity == false)
            {
                // place the remaining inactive cubes, either way...
                for (int i = 0; i < num_InactiveInfectionCubes_OfType; i++)
                {
                    GO_PA_PlaceInfectionCubeOnCity(game, cityToInfect, currentInfectionType);
                }
                // game lost
                existingReport.SetFailureReason(InfectionFailureReasons.notEnoughDiseaseCubes);
                return existingReport;
            }

            // fill up this city
            for (int i = 0; i < num_CubesOfType_ThatCanBePlacedOnCity; i++)
            {
                GO_PA_PlaceInfectionCubeOnCity(
                    game,
                    cityToInfect,
                    existingReport.InfectionType
                    );
            }
            existingReport.AddUsedCubes(num_CubesOfType_ThatCanBePlacedOnCity);

            game.GameStateCounter.IncreaseOutbreaksCounter();
            if (game.GQ_SS_DeadlyOutbreaks() == true)
            {
                existingReport.SetFailureReason(InfectionFailureReasons.maximumOutbreaksSurpassed);
                return existingReport;
            }

            var neighbors = game.Map.CityNeighbors_PerCityID[cityToInfect.ID]; // cityToInfect.AdjacentCities;

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
            PD_City city,
            int infectionCubeType
            )
        {
            PD_ME_InfectionCube infectionCubeToAdd =
                game.MapElements.InactiveInfectionCubesPerType[infectionCubeType].DrawLast();
            game.MapElements.InfectionCubesPerCityID[city.ID].Add(infectionCubeToAdd);
        }

        public static void GO_PlaceResearchStationOnCity(
            this PD_Game game,
            PD_City city
            )
        {
            PD_ME_ResearchStation researchStation = game.MapElements.InactiveResearchStations.DrawLast();
            game.MapElements.ResearchStationsPerCityID[city.ID].Add(researchStation);
        }

        public static void GO_MovePawnFromCityToCity(
            this PD_Game game,
            PD_ME_PlayerPawn pawn,
            PD_City initialCity,
            PD_City targetCity
            )
        {
            game.MapElements.PlayerPawnsPerCityID[initialCity.ID].Remove(pawn);
            game.MapElements.PlayerPawnsPerCityID[targetCity.ID].Add(pawn);
        }

        public static void GO_PlayerDiscardsPlayerCard(
            this PD_Game game,
            PD_Player player,
            PD_PlayerCardBase playerCardToDiscard
            )
        {
            game.Cards.PlayerCardsPerPlayerID[player.ID].Remove(playerCardToDiscard);
            game.Cards.DeckOfDiscardedPlayerCards.Add(playerCardToDiscard);
        }

        public static void GO_Remove_One_InfectionCube_OfType_FromCity(
            this PD_Game game,
            PD_City city,
            int treatType
            )
        {
            if (
                game.MapElements.InfectionCubesPerCityID[city.ID]
                .Any(
                    x =>
                    x.Type == treatType
                    ) == false
                )
            {
                throw new System.Exception("the city does not have any cubes of this type");
            }

            var cubeToRemove = game.MapElements.InfectionCubesPerCityID[city.ID]
                .FindAll(
                    x =>
                    x.Type == treatType
                    ).GetFirst();

            // remove the cube from the city
            game.MapElements.InfectionCubesPerCityID[city.ID].Remove(cubeToRemove);
            // put the cubes back in the inactive container
            game.MapElements.InactiveInfectionCubesPerType[treatType].Add(cubeToRemove);
        }

        public static void GO_Remove_All_InfectionCubes_OfType_FromCity(
            this PD_Game game,
            PD_City city,
            int diseaseType
            )
        {
            if (
                game.MapElements.InfectionCubesPerCityID[city.ID]
                .Any(
                    x =>
                    x.Type == diseaseType
                    ) == false
                )
            {
                throw new System.Exception("the city does not have any cubes of this type");
            }

            var cubesToRemove = game.MapElements.InfectionCubesPerCityID[city.ID]
                .FindAll(
                    x =>
                    x.Type == diseaseType
                    );

            // remove the cube from the city
            game.MapElements.InfectionCubesPerCityID[city.ID].RemoveAll(
                x =>
                cubesToRemove.Contains(x)
                );
            // put the cubes back in the inactive container
            game.MapElements.InactiveInfectionCubesPerType[diseaseType].AddRange(cubesToRemove);
        }

        public static void GO_Remove_InfectionCubes_FromCity(
            this PD_Game game,
            PD_City city,
            List<PD_ME_InfectionCube> cubesToRemove,
            int treat_Type
            )
        {
            if (
                cubesToRemove.Any(x => x.Type != treat_Type)
                )
            {
                throw new System.Exception("cubes do not match the selected type");
            }
            if (
                game.MapElements.InfectionCubesPerCityID[city.ID]
                .ContainsAll(cubesToRemove)
                == false
                )
            {
                throw new System.Exception("selected cubes are not contained on the selected city!");
            }

            // remove the cubes from the city
            game.MapElements.InfectionCubesPerCityID[city.ID]
                .RemoveAll(
                    x =>
                    cubesToRemove.Contains(x)
                    );
            // put the cubes back in the inactive container
            game.MapElements.InactiveInfectionCubesPerType[treat_Type].AddRange(cubesToRemove);
        }
    }
}
