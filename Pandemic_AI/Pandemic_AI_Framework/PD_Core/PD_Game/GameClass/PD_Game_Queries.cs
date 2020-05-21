using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_Queries
    {
        public static bool Find_If_OperationsExpertFlight_HasBeenUsedInThisTurn(
            PD_Game game
            )
        {
            bool operationsExpertFlightHasBeenUsedInThisTurn = false;
            int totalNumActions = game.PlayerActionsHistory.Count;
            int numActionsToSearch = 4;
            if (totalNumActions < numActionsToSearch)
            {
                numActionsToSearch = totalNumActions;
            }
            for (int i = 0; i < numActionsToSearch; i++)
            {
                int index = game.PlayerActionsHistory.Count - 1 - i;
                var action = game.PlayerActionsHistory[index];
                if (action.GetType() == typeof(PD_PMA_OperationsExpert_Flight))
                {
                    operationsExpertFlightHasBeenUsedInThisTurn = true;
                }
            }
            return operationsExpertFlightHasBeenUsedInThisTurn;
        }

        public static PD_City GQ_Find_Medic_Location(
            PD_Game game
            )
        {
            PD_Player medic = game.Players.Find(
                x =>
                GQ_Find_Player_Role(game, x) == PD_Player_Roles.Medic
                );
            PD_City medic_location = null;
            if (medic != null)
            {
                medic_location = PD_Game_Queries.GQ_Find_PlayerLocation(game, medic);
            }
            return medic_location;
        }

        public static int Count_Num_InactiveInfectionCubes_OfType(
            PD_Game game,
            int type
            )
        {
            return game.MapElements.InactiveInfectionCubesPerType[type].Count;
        }

        public static PD_Player Find_OwnerOfPawn(
            PD_Game game,
            PD_ME_PlayerPawn pawn
            )
        {
            foreach (var player in game.Players)
            {
                if (game.PlayerPawnsPerPlayerID[player.ID] == pawn)
                {
                    return player;
                }
            }
            return null;
        }

        public static List<PD_CityCard> GQ_Find_UselessCityCards(PD_Game game)
        {
            List<PD_CityCard> cityCardsInAllPlayerHands = GQ_Find_CityCards_InAllPlayersHands(game);
            List<int> curedDiseaseTypes = GQ_Find_Cured_or_Eradicated_DiseaseTypes(game);
            List<PD_CityCard> uselessCards = new List<PD_CityCard>();
            foreach (var card in cityCardsInAllPlayerHands)
            {
                if (
                    curedDiseaseTypes.Contains(card.Type)
                    )
                {
                    uselessCards.Add(card);
                }
            }
            return uselessCards;
        }

        public static int GQ_Count_RemainingPlayerActions_ThisRound(PD_Game game)
        {
            return 4 - game.GameStateCounter.CurrentPlayerActionIndex;
        }

        public static List<PD_CityCard> GQ_Find_CityCards_InAllPlayersHands(PD_Game game)
        {
            List<PD_CityCard> allCityCardsInAllPlayersHands = new List<PD_CityCard>();
            foreach (var player in game.Players)
            {
                allCityCardsInAllPlayersHands.AddRange(
                    GQ_Find_CityCardsInPlayerHand(game, player)
                    );
            }
            return allCityCardsInAllPlayersHands;
        }

        public static bool GQ_SS_NotEnoughDiseaseCubestoCompleteAnInfection(
            PD_Game game
            )
        {
            return game.GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection;
        }

        public static bool GQ_SS_DeadlyOutbreaks(
            PD_Game game
            )
        {
            return
                game.GameStateCounter.OutbreaksCounter
                > game.GameSettings.MaximumViableOutbreaks;
        }

        public static bool GQ_SS_AllDiseasesCured(
            PD_Game game
            )
        {
            for (int i = 0; i < 4; i++)
            {
                bool diseaseCuredOrEradicated =
                    GQ_Is_DiseaseCured_OR_Eradicated(game, i);
                if (diseaseCuredOrEradicated == false)
                    return false;
            }
            return true;
        }

        public static bool GQ_SS_EnoughPlayerCardsToDraw(
            PD_Game game
            )
        {
            int numPlayerCards =
                game.Cards.DividedDeckOfPlayerCards.GetNumberOfElementsOfAllSubLists();
            return numPlayerCards >= 2;
        }

        public static bool GQ_SS_ThereAreActiveInfectionCards(
            PD_Game game
            )
        {
            return game.Cards.ActiveInfectionCards.Count > 0;
        }

        public static bool SS_PlayerActionsFinished(
            PD_Game game
            )
        {
            return game.GameStateCounter.CurrentPlayerActionIndex > 3;
        }

        public static bool GQ_SS_CurrentPlayerHandIsBiggerThanPermitted(
            PD_Game game
            )
        {
            return
                GQ_Find_CurrentPlayerHand(game).Count
                >
                game.GameSettings.MaximumNumberOfPlayerCardsPerPlayer;
        }

        public static bool GQ_SS_CurrentPlayerHandIncludesEpidemicCard(
            PD_Game game
            )
        {
            foreach (var card in GQ_Find_CurrentPlayerHand(game))
            {
                if (card.GetType() == typeof(PD_EpidemicCard))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GQ_SS_AnyPlayerHandIsBiggerThanPermitted(
            PD_Game game
            )
        {
            foreach (var player in game.Players)
            {
                var playerHand = game.Cards.PlayerCardsPerPlayerID[player.ID];
                if (playerHand.Count > game.GameSettings.MaximumNumberOfPlayerCardsPerPlayer)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GQ_Is_GameWon(
            PD_Game game
            )
        {
            return game.GameFSM.CurrentState.GetType() == typeof(PD_GS_GameWon);
        }

        public static bool GQ_Is_GameLost(
            PD_Game game
            )
        {
            return game.GameFSM.CurrentState.GetType() == typeof(PD_GS_GameLost);
        }

        public static bool GQ_Is_GameOngoing(
            PD_Game game
            )
        {
            return !GQ_Is_GameFinished(game);
        }

        public static bool GQ_Is_GameFinished(
            PD_Game game
            )
        {
            return GQ_Is_GameWon(game) || GQ_Is_GameLost(game);
        }

        public static List<PD_PlayerCardBase> GQ_Find_CurrentPlayerHand(
            PD_Game game
            )
        {
            var currentPlayer = GQ_Find_CurrentPlayer(game);
            return game.Cards.PlayerCardsPerPlayerID[currentPlayer.ID];
        }

        public static bool GQ_Is_DiseaseCured_OR_Eradicated(
            PD_Game game,
            int diseaseType
            )
        {
            return
                game.GameStateCounter.CureMarkersStates[diseaseType] == 1
                ||
                game.GameStateCounter.CureMarkersStates[diseaseType] == 2;
        }

        public static bool GQ_Is_DiseaseCured_AND_NOT_Eradicated(
            PD_Game game,
            int diseaseType
            )
        {
            return game.GameStateCounter.CureMarkersStates[diseaseType] == 1;
        }

        public static bool GQ_Is_Disease_Eradicated(
            PD_Game game,
            int diseaseType
            )
        {
            return game.GameStateCounter.CureMarkersStates[diseaseType] == 2;
        }

        public static PD_Player GQ_Find_CurrentPlayer(
            PD_Game game
            )
        {
            return game.Players[game.GameStateCounter.CurrentPlayerIndex];
        }

        public static PD_Player_Roles GQ_Find_CurrentPlayer_Role(
            PD_Game game
            )
        {
            var currentPlayer = GQ_Find_CurrentPlayer(game);
            return GQ_Find_Player_Role(game, currentPlayer);
        }

        public static PD_Player_Roles GQ_Find_Player_Role(
            PD_Game game,
            PD_Player player
            )
        {
            return game.RoleCardsPerPlayerID[player.ID].Role;
        }

        public static List<PD_City> GQ_Find_CitiesWith_X_SameTypeCubes(
            PD_Game game,
            int requested_numSameTypeCubes
            )
        {

            if (requested_numSameTypeCubes < 0 || requested_numSameTypeCubes > 3)
            {
                throw new System.Exception("number of cubes must be between 0 and 3");
            }
            List<PD_City> allCities = game.Map.Cities;
            List<PD_City> citiesWith_X_SameTypeCubes = new List<PD_City>();
            foreach (var city in allCities)
            {
                int max_NumSameTypeCubesOnCity = 0;
                List<PD_ME_InfectionCube> allCubesOnCity = game.MapElements.InfectionCubesPerCityID[city.ID];
                for (int i = 0; i < 4; i++)
                {
                    List<PD_ME_InfectionCube> cubesOfThisTypeOnCity = allCubesOnCity.FindAll(
                        x =>
                        x.Type == i
                        );
                    int numCubesOfThisTypeOnCity = cubesOfThisTypeOnCity.Count;
                    if (numCubesOfThisTypeOnCity > max_NumSameTypeCubesOnCity)
                    {
                        max_NumSameTypeCubesOnCity = numCubesOfThisTypeOnCity;
                    }
                }
                if (max_NumSameTypeCubesOnCity == requested_numSameTypeCubes)
                {
                    citiesWith_X_SameTypeCubes.Add(city);
                }
            }

            return citiesWith_X_SameTypeCubes;
        }

        public static List<int> GQ_Find_InfectionCubeTypes_OnCity(
            PD_Game game,
            PD_City city
            )
        {
            var infectionCubesOnThisCity = game.MapElements.InfectionCubesPerCityID[city.ID];
            List<int> infectionCubeTypesOnSpecificCity = new List<int>();
            foreach (var cube in infectionCubesOnThisCity)
            {
                if (infectionCubeTypesOnSpecificCity.Contains(cube.Type) == false)
                {
                    infectionCubeTypesOnSpecificCity.Add(cube.Type);
                }
            }
            return infectionCubeTypesOnSpecificCity;
        }

        public static Dictionary<PD_City, List<int>> GQ_Find_InfectionCubeTypesPerInfectedCity(
            PD_Game game
            )
        {
            Dictionary<PD_City, List<int>> infectionCubeTypesPerInfectedCity = new Dictionary<PD_City, List<int>>();

            var allInfectedCities = GQ_Find_InfectedCities(game);
            foreach (var city in allInfectedCities)
            {
                List<int> infectionCubeTypesOnThisCity = GQ_Find_InfectionCubeTypes_OnCity(game, city);
                infectionCubeTypesPerInfectedCity.Add(city, infectionCubeTypesOnThisCity);
            }
            return infectionCubeTypesPerInfectedCity;
        }

        public static List<List<PD_CityCard>> GQ_Find_UsableDiscoverCureCardGroups(
            PD_Game game,
            PD_Player player
            )
        {
            var discoverCureCardGroups = GQ_Find_DiscoverCureCardGroups(game, player);
            var uncuredDiseaseTypes = GQ_Find_UncuredDiseaseTypes(game);

            var usableDiscoverCureCardGroups = new List<List<PD_CityCard>>();
            foreach (var cardGroup in discoverCureCardGroups)
            {
                if (
                    uncuredDiseaseTypes.Contains(cardGroup[0].Type)
                    )
                {
                    usableDiscoverCureCardGroups.Add(cardGroup);
                }
            }

            return usableDiscoverCureCardGroups;
        }

        public static List<List<PD_CityCard>> GQ_Find_DiscoverCureCardGroups(
            PD_Game game,
            PD_Player player
            )
        {
            bool player_Is_Scientist = game.RoleCardsPerPlayerID[player.ID].Role == PD_Player_Roles.Scientist;
            var cityCardsInPlayerHand = GQ_Find_CityCardsInPlayerHand(game, player);

            if (player_Is_Scientist == false && cityCardsInPlayerHand.Count < 5)
            {
                return new List<List<PD_CityCard>>();
            }
            else if (player_Is_Scientist && cityCardsInPlayerHand.Count < 4)
            {
                return new List<List<PD_CityCard>>();
            }

            List<int> availableTypes = new List<int>();
            foreach (var cityCard in cityCardsInPlayerHand)
            {
                if (availableTypes.Contains(cityCard.Type) == false)
                {
                    availableTypes.Add(cityCard.Type);
                }
            }

            List<List<PD_CityCard>> cityCardsByType = new List<List<PD_CityCard>>();
            foreach (var type in availableTypes)
            {
                List<PD_CityCard> cityCardsOfThisType = cityCardsInPlayerHand.FindAll(
                    x =>
                    x.Type == type
                    ).ToList();
                cityCardsByType.Add(cityCardsOfThisType);
            }

            if (player_Is_Scientist == false)
            {
                List<List<PD_CityCard>> discoverCureCardGroups = new List<List<PD_CityCard>>();

                var groups_OfFive_OrMore = cityCardsByType.FindAll(
                    x =>
                    x.Count >= 5
                    );

                foreach (var group in groups_OfFive_OrMore)
                {
                    var subGroups = group.GetAllSubSetsOfSpecificSize(5);
                    discoverCureCardGroups.AddRange(subGroups);
                }

                return discoverCureCardGroups;
            }
            else
            {
                List<List<PD_CityCard>> discoverCureCardGroups = new List<List<PD_CityCard>>();

                var groups_OfFour_OrMore = cityCardsByType.FindAll(
                    x =>
                    x.Count >= 4
                    );

                foreach (var group in groups_OfFour_OrMore)
                {
                    var subGroups = group.GetAllSubSetsOfSpecificSize(4);
                    discoverCureCardGroups.AddRange(subGroups);
                }

                return discoverCureCardGroups;
            }
        }

        public static PD_City GQ_Find_CurrentPlayer_Location(
            PD_Game game
            )
        {
            var currentPlayer = GQ_Find_CurrentPlayer(game);
            var currentPlayerPawn = game.PlayerPawnsPerPlayerID[currentPlayer.ID];
            return GQ_Find_PlayerPawnLocation(game, currentPlayerPawn);
        }

        public static PD_City GQ_Find_PlayerLocation(
            PD_Game game,
            PD_Player player
            )
        {
            var playerPawn = game.PlayerPawnsPerPlayerID[player.ID];
            return GQ_Find_PlayerPawnLocation(game, playerPawn);
        }

        public static PD_City GQ_Find_PlayerPawnLocation(
            PD_Game game,
            PD_ME_PlayerPawn playerPawn
            )
        {
            if (playerPawn == null)
            {
                throw new System.Exception("the player pawn is null");
            }

            foreach (var city in game.Map.Cities)
            {
                if (game.MapElements.PlayerPawnsPerCityID[city.ID].Contains(playerPawn))
                {
                    return city;
                }
            }

            return null;
        }

        public static PD_City GQ_Find_CityByName(
            PD_Game game,
            string cityName
            )
        {
            return game.Map.Cities.Find(
                x =>
                x.Name == cityName
                );
        }

        public static PD_ME_PlayerPawn GQ_Find_PlayerPawn(
            PD_Game game,
            PD_Player player
            )
        {
            return game.PlayerPawnsPerPlayerID[player.ID];
        }

        public static List<PD_City> GQ_Find_ResearchStationCities(
            PD_Game game
            )
        {
            List<PD_City> researchStationCities = new List<PD_City>();
            foreach (var city in game.Map.Cities)
            {
                if (GQ_Is_City_ResearchStation(game, city))
                {
                    researchStationCities.Add(city);
                }
            }
            return researchStationCities;
        }

        public static bool GQ_Is_City_ResearchStation(
            PD_Game game,
            PD_City city
            )
        {
            return game.MapElements.ResearchStationsPerCityID[city.ID].Count > 0;
        }

        public static List<PD_City> GQ_Find_InfectedCities(
            PD_Game game
            )
        {
            List<PD_City> allInfectedCities = new List<PD_City>();
            foreach (var city in game.Map.Cities)
            {
                if (game.MapElements.InfectionCubesPerCityID[city.ID].Count > 0)
                {
                    allInfectedCities.Add(city);
                }
            }
            return allInfectedCities;
        }

        public static List<PD_City> GQ_Find_InfectionTypePerCity_MinCubes(
            PD_Game game,
            int minimumCubes_AnyType
            )
        {
            if (minimumCubes_AnyType < 1)
            {
                throw new System.Exception("minimum number infection cubes must be at least 1");
            }
            List<PD_City> allInfectedCities = new List<PD_City>();
            foreach (var city in game.Map.Cities)
            {
                if (game.MapElements.InfectionCubesPerCityID[city.ID].Count >= minimumCubes_AnyType)
                {
                    allInfectedCities.Add(city);
                }
            }
            return allInfectedCities;
        }

        public static List<PD_CityCard> GQ_Find_CityCardsInCurrentPlayerHand(
            PD_Game game
            )
        {
            PD_Player currentPlayer = PD_Game_Queries.GQ_Find_CurrentPlayer(game);
            return GQ_Find_CityCardsInPlayerHand(
                game,
                currentPlayer
                );
        }

        public static List<PD_CityCard> GQ_Find_CityCardsInPlayerHand(
            PD_Game game,
            PD_Player player
            )
        {
            var playerHand = game.Cards.PlayerCardsPerPlayerID[player.ID];
            var cityCardsInPlayerHand = playerHand.FindAll(
                x =>
                x.GetType() == typeof(PD_CityCard)
                ).Cast<PD_CityCard>().ToList();
            return cityCardsInPlayerHand;
        }

        public static List<int> GQ_Find_UncuredDiseaseTypes(
            PD_Game game
            )
        {
            List<int> uncuredDiseaseTypes = new List<int>();
            var allDiseaseTypes = game.GameStateCounter.CureMarkersStates.Keys.ToList();
            foreach (var dt in allDiseaseTypes)
            {
                if (game.GameStateCounter.CureMarkersStates[dt] == 0)
                {
                    uncuredDiseaseTypes.Add(dt);
                }
            }
            return uncuredDiseaseTypes;
        }

        public static double GQ_PercentageAvailableCubes(
            PD_Game game
            )
        {
            int numAllCubes = game.GameElementReferences.InfectionCubes.Count;
            int numAvailableCubes = 0;
            for (int i = 0; i < 4; i++)
            {
                numAvailableCubes += game.MapElements.InactiveInfectionCubesPerType[i].Count;
            }
            return (double)numAvailableCubes / (double)numAllCubes;
        }

        public static double GQ_PercentageCubesOnTheBoard(
            PD_Game game
            )
        {
            int numAllCubes = game.GameElementReferences.InfectionCubes.Count;
            int numCubesOnTheBoard = PD_Game_Queries.GQ_Count_Num_DiseaseCubesOnTheBoard(game);
            return (double)numCubesOnTheBoard / (double)numAllCubes;
        }

        public static int GQ_Count_Num_InfectionCubes_OfType_OnCity(
            PD_Game game,
            PD_City city,
            int infectionCubeType
            )
        {
            return GQ_Find_InfectionCubes_OfType_OnCity(
                game,
                city,
                infectionCubeType
                ).Count;
        }

        public static List<PD_ME_InfectionCube> GQ_Find_InfectionCubes_OfType_OnCity(
            PD_Game game,
            PD_City city,
            int type
            )
        {
            return game.MapElements.InfectionCubesPerCityID[city.ID].FindAll(
                x =>
                x.Type == type
                );
        }

        public static int GQ_Count_Num_DiseaseCubesOnTheBoard(
            PD_Game game
            )
        {
            int numCubesOnTheBoard = 0;
            foreach (var city in game.Map.Cities)
            {
                numCubesOnTheBoard += game.MapElements.InfectionCubesPerCityID[city.ID].Count;
            }
            return numCubesOnTheBoard;
        }

        public static List<int> GQ_Find_Cured_or_Eradicated_DiseaseTypes(PD_Game game)
        {
            List<int> curedOrEradicatedDiseaseTypes = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (game.GameStateCounter.CureMarkersStates[i] > 0)
                    curedOrEradicatedDiseaseTypes.Add(i);
            }
            return curedOrEradicatedDiseaseTypes;
        }

        public static int GQ_Count_Num_DiseasesCured(
            PD_Game game
            )
        {
            int numDiseasesCured = 0;
            for (int i = 0; i < 4; i++)
            {
                if (game.GameStateCounter.CureMarkersStates[i] > 0)
                    numDiseasesCured++;
            }
            return numDiseasesCured;
        }

        #region gameState queries
        public static bool GQ_IsInState_ApplyingMainPlayerActions(
            PD_Game game
            )
        {
            return game.GameFSM.CurrentState.GetType() == typeof(PD_GS_ApplyingMainPlayerActions);
        }

        public static bool GQ_IsInState_DiscardDuringMainPlayerActions(
            PD_Game game
            )
        {
            if (
                game.GameFSM.CurrentState.GetType() == typeof(PD_GS_Discarding_DuringMainPlayerActions)
                )
            {
                return true;
            }
            return false;
        }

        public static bool GQ_IsInState_DrawingNewPlayerCards(
            PD_Game game
            )
        {
            if (
                game.GameFSM.CurrentState.GetType() == typeof(PD_GS_DrawingNewPlayerCards)
                )
            {
                return true;
            }
            return false;
        }

        public static bool GQ_IsInState_DiscardAfterDrawing(
            PD_Game game
            )
        {
            if (
                game.GameFSM.CurrentState.GetType() == typeof(PD_GS_Discarding_AfterDrawing)
                )
            {
                return true;
            }
            return false;
        }

        public static bool GQ_IsInState_Discard_Any(
            PD_Game game
            )
        {
            return (
                GQ_IsInState_DiscardDuringMainPlayerActions(game)
                ||
                GQ_IsInState_DiscardAfterDrawing(game)
                );
        }


        #endregion

        public static int GQ_NumInactiveResearchStations(
            PD_Game game
            )
        {
            return game.MapElements.InactiveResearchStations.Count;
        }

        public static int GQ_NumActiveResearchStations(
            PD_Game game
            )
        {
            int totalNumResearchStations = 6;
            int numInactiveResearchStations = GQ_NumInactiveResearchStations(game);
            return totalNumResearchStations - numInactiveResearchStations;
        }
    }
}
