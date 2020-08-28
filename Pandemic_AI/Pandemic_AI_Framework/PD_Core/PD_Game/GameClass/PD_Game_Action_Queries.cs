using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_Action_Queries
    {
        public static List<PD_GameAction_Base>
            GAQ_Find_AvailablePlayerActions(PD_Game game)
        {
            List<PD_GameAction_Base> currentAvailablePlayerActions = new List<PD_GameAction_Base>();

            PD_Player currentPlayer = PD_Game_Queries.GQ_CurrentPlayer(game);

            if (game.GameFSM.CurrentState.GetType() != null && game.GameFSM.CurrentState.GetType() == typeof(PD_GS_Idle))
            {
                PD_GA_SetupGame_Random sg = new PD_GA_SetupGame_Random();
                currentAvailablePlayerActions.Add(sg);
            }
            else if (game.GQ_IsInState_ApplyingMainPlayerActions())
            {
                currentAvailablePlayerActions.AddRange(FindAvailable_Stay_Actions(game));

                currentAvailablePlayerActions.AddRange(FindAvailable_DriveFerry_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_DirectFlight_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_CharterFlight_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_ShuttleFlight_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_OperationsExpertFlight_Actions(game));

                currentAvailablePlayerActions.AddRange(FindAvailable_BuildResearchStation_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_BuildResearchStation_OperationsExpert_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_MoveResearchStation_Actions(game));

                currentAvailablePlayerActions.AddRange(Find_AvailablePlayerActions_PA_TreatDisease(game));
                currentAvailablePlayerActions.AddRange(Find_AvailablePlayerActions_PA_TreatDisease_Medic(game));

                currentAvailablePlayerActions.AddRange(Find_AvailablePlayerActions_PA_ShareKnowledge_GiveCard(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_ShareKnowledge_TakeCard_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_ShareKnowledge_GiveCard_ResearcherGives_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_ShareKnowledge_TakeCard_FromResearcher_Actions(game));

                currentAvailablePlayerActions.AddRange(FindAvailable_DiscoverCure_Actions(game));
                currentAvailablePlayerActions.AddRange(FindAvailable_DiscoverCure_Scientist_Actions(game));
            }
            else if (game.GQ_IsInState_DiscardDuringMainPlayerActions())
            {
                currentAvailablePlayerActions.AddRange(
                    GAQ_FindAvailable_DiscardDuringMainPlayerActions_Actions(game)
                    );
            }
            else if (game.GQ_IsInState_DrawingNewPlayerCards())
            {
                currentAvailablePlayerActions.Add(
                    new PD_DrawNewPlayerCards(currentPlayer)
                    );
            }
            else if (game.GQ_IsInState_DiscardAfterDrawing())
            {
                currentAvailablePlayerActions.AddRange(
                    GAQ_FindAvailable_DiscardAfterDrawing_Actions(game)
                    );
            }
            else if (game.GameFSM.CurrentState.GetType() == typeof(PD_GS_ApplyingEpidemicCard))
            {
                currentAvailablePlayerActions.Add(
                    new PD_ApplyEpidemicCard(currentPlayer)
                    );
            }
            else if (game.GameFSM.CurrentState.GetType() == typeof(PD_GS_DrawingNewInfectionCards))
            {
                currentAvailablePlayerActions.Add(
                    new PD_DrawNewInfectionCards(currentPlayer)
                    );
            }
            else if (game.GameFSM.CurrentState.GetType() == typeof(PD_GS_ApplyingInfectionCards))
            {
                currentAvailablePlayerActions.Add(
                    new PD_ApplyInfectionCard(currentPlayer, game.Cards.ActiveInfectionCards[0])
                    );
            }

            return currentAvailablePlayerActions;
        }

        private static List<PD_PA_Stay> FindAvailable_Stay_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            List<PD_PA_Stay> availableStayCommands = new List<PD_PA_Stay>();
            PD_PA_Stay stayCommand = new PD_PA_Stay(
                currentPlayer,
                game.GQ_PlayerLocation(currentPlayer)
                );
            availableStayCommands.Add(stayCommand);
            return availableStayCommands;
        }

        private static List<PD_PMA_DriveFerry> FindAvailable_DriveFerry_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerPawn = game.PlayerPawnsPerPlayerID[currentPlayer.ID];
            var currentPlayerPawnLocation = game.GQ_Find_PlayerPawnLocation(currentPlayerPawn);

            var currentPawnAdjacentLocations = game.Map.CityNeighbors_PerCityID[currentPlayerPawnLocation.ID];

            List<PD_PMA_DriveFerry> availablePlayerActions_Ferry = new List<PD_PMA_DriveFerry>();
            foreach (var adjacentLocation in currentPawnAdjacentLocations)
            {
                PD_PMA_DriveFerry pa1 = new PD_PMA_DriveFerry(
                    currentPlayer,
                    currentPlayerPawnLocation,
                    adjacentLocation
                    );
                availablePlayerActions_Ferry.Add(pa1);
            }
            return availablePlayerActions_Ferry;
        }

        private static List<PD_PMA_DirectFlight> FindAvailable_DirectFlight_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerPawn = game.PlayerPawnsPerPlayerID[currentPlayer.ID];
            var currentPawnLocation = game.GQ_Find_PlayerPawnLocation(currentPlayerPawn);

            var cityCardsInCurrentPlayerHand = game.GQ_Find_CityCardsInCurrentPlayerHand();

            var directFlightActions = new List<PD_PMA_DirectFlight>();

            foreach (var cityCard in cityCardsInCurrentPlayerHand)
            {
                if (cityCard.City != currentPawnLocation)
                {
                    var action = new PD_PMA_DirectFlight(
                        currentPlayer,
                        currentPawnLocation,
                        cityCard.City,
                        cityCard);
                    directFlightActions.Add(action);
                }
            }

            return directFlightActions;
        }

        private static List<PD_PMA_CharterFlight> FindAvailable_CharterFlight_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerPawn = game.PlayerPawnsPerPlayerID[currentPlayer.ID];
            var currentPawnLocation = game.GQ_Find_PlayerPawnLocation(currentPlayerPawn);

            var cityCardsInCurrentPlayerHand = game.GQ_Find_CityCardsInCurrentPlayerHand();

            var charterFlightActions = new List<PD_PMA_CharterFlight>();

            // if the player is standing on a location that exists in the city cards in their hand
            var citiesOfCityCardsInPlayerHand = new List<PD_City>();
            foreach (var card in cityCardsInCurrentPlayerHand)
            {
                citiesOfCityCardsInPlayerHand.Add(card.City);
            }

            if (citiesOfCityCardsInPlayerHand.Any(x => x == currentPawnLocation))
            {
                var allOtherCities = game.Map.Cities.FindAll(
                    x =>
                    x != currentPawnLocation
                    );

                var cityCardtoDiscard = cityCardsInCurrentPlayerHand.Find(
                    x =>
                    x.City == currentPawnLocation
                    );

                foreach (var otherCity in allOtherCities)
                {
                    var action = new PD_PMA_CharterFlight(
                        currentPlayer,
                        currentPawnLocation,
                        otherCity,
                        cityCardtoDiscard);
                    charterFlightActions.Add(action);
                }
            }

            return charterFlightActions;
        }

        private static List<PD_PMA_ShuttleFlight> FindAvailable_ShuttleFlight_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerPawn = game.PlayerPawnsPerPlayerID[currentPlayer.ID];
            var currentPawnLocation = game.GQ_Find_PlayerPawnLocation(currentPlayerPawn);

            var cityCardsInCurrentPlayerHand = game.GQ_Find_CityCardsInCurrentPlayerHand();

            var shuttleFlightActions = new List<PD_PMA_ShuttleFlight>();

            var allLocationsWithResearchStations = new List<PD_City>();
            foreach (var city in game.Map.Cities)
            {
                if (game.MapElements.ResearchStationsPerCityID[city.ID].Count > 0)
                {
                    allLocationsWithResearchStations.Add(city);
                }
            }

            if (allLocationsWithResearchStations.Any(x => x == currentPawnLocation))
            {
                var allOtherLocationsWithResearchStations = allLocationsWithResearchStations.FindAll(
                    x =>
                    x != currentPawnLocation
                    );

                foreach (var city in allOtherLocationsWithResearchStations)
                {
                    var action = new PD_PMA_ShuttleFlight(
                        currentPlayer,
                        currentPawnLocation,
                        city
                        );
                    shuttleFlightActions.Add(action);
                }
            }

            return shuttleFlightActions;
        }

        private static List<PD_PMA_OperationsExpert_Flight> FindAvailable_OperationsExpertFlight_Actions(
            PD_Game game
            )
        {
            PD_Player_Roles currentPlayerRole = game.GQ_CurrentPlayer_Role();
            bool currentPlayerIsOperationsExpert = currentPlayerRole == PD_Player_Roles.Operations_Expert;
            if (currentPlayerIsOperationsExpert == false)
            {
                return new List<PD_PMA_OperationsExpert_Flight>();
            }
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            PD_City currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            List<PD_City> allResearchStationCities = game.GQ_ResearchStationCities();
            bool currentPlayerLocationIsResearchStation = allResearchStationCities.Contains(currentPlayerLocation);
            if (currentPlayerLocationIsResearchStation == false)
            {
                return new List<PD_PMA_OperationsExpert_Flight>();
            }

            bool operationsExpertFlightHasBeenUsedInThisTurn = game.Find_If_OperationsExpertFlight_HasBeenUsedInThisTurn();

            if (operationsExpertFlightHasBeenUsedInThisTurn)
            {
                return new List<PD_PMA_OperationsExpert_Flight>();
            }

            List<PD_CityCard> cityCardsInCurerrentPlayerHand = game.GQ_Find_CityCardsInCurrentPlayerHand();
            if (cityCardsInCurerrentPlayerHand.Count == 0)
            {
                return new List<PD_PMA_OperationsExpert_Flight>();
            }

            List<PD_City> targetLocations = game.Map.Cities.FindAll(
                x =>
                x != currentPlayerLocation
                );

            List<PD_PMA_OperationsExpert_Flight> availableActions = new List<PD_PMA_OperationsExpert_Flight>();

            foreach (var cityCardToDiscard in cityCardsInCurerrentPlayerHand)
            {
                foreach (var targetLocation in targetLocations)
                {
                    PD_PMA_OperationsExpert_Flight action = new PD_PMA_OperationsExpert_Flight(
                        currentPlayer,
                        currentPlayerLocation,
                        targetLocation,
                        cityCardToDiscard
                        );

                    availableActions.Add(action);
                }
            }

            return availableActions;
        }

        private static List<PD_PA_BuildResearchStation> FindAvailable_BuildResearchStation_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            bool currentLocation_Is_RS =
                game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count > 0;

            bool inactive_RS_Available =
                game.MapElements.InactiveResearchStations.Count > 0;

            if (
                currentLocation_Is_RS
                ||
                inactive_RS_Available == false
                )
            {
                return new List<PD_PA_BuildResearchStation>();
            }

            bool currentPlayer_Is_Operations_Expert =
                game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Operations_Expert;

            if (currentPlayer_Is_Operations_Expert)
            {
                return new List<PD_PA_BuildResearchStation>();
            }
            else
            {
                List<PD_PA_BuildResearchStation> buildResearchStation_Actions = new List<PD_PA_BuildResearchStation>();

                var cityCardsInPlayerHand = game.GQ_Find_CityCardsInCurrentPlayerHand();

                bool player_Has_CurrentLocation_CityCard =
                    cityCardsInPlayerHand.Any(
                        x =>
                        x.City == currentPlayerLocation
                        );

                if (player_Has_CurrentLocation_CityCard == true)
                {
                    PD_PA_BuildResearchStation action = new PD_PA_BuildResearchStation(
                        currentPlayer,
                        cityCardsInPlayerHand.Find(x => x.City == currentPlayerLocation),
                        currentPlayerLocation
                        );
                    buildResearchStation_Actions.Add(action);
                }

                return buildResearchStation_Actions;
            }
        }

        private static List<PD_PA_BuildResearchStation_OperationsExpert> FindAvailable_BuildResearchStation_OperationsExpert_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            bool currentLocation_Is_RS =
                game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count > 0;

            bool inactive_RS_Available =
                game.MapElements.InactiveResearchStations.Count > 0;

            if (
                currentLocation_Is_RS
                ||
                inactive_RS_Available == false
                )
            {
                return new List<PD_PA_BuildResearchStation_OperationsExpert>();
            }

            bool currentPlayer_Is_Operations_Expert =
                game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Operations_Expert;

            if (currentPlayer_Is_Operations_Expert)
            {
                List<PD_PA_BuildResearchStation_OperationsExpert> buildResearchStation_Actions =
                    new List<PD_PA_BuildResearchStation_OperationsExpert>();

                PD_PA_BuildResearchStation_OperationsExpert action =
                    new PD_PA_BuildResearchStation_OperationsExpert(
                        currentPlayer,
                        currentPlayerLocation
                        );
                buildResearchStation_Actions.Add(action);

                return buildResearchStation_Actions;
            }
            return new List<PD_PA_BuildResearchStation_OperationsExpert>();
        }

        private static List<PD_PA_MoveResearchStation> FindAvailable_MoveResearchStation_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            var cityCardsInPlayerHand = game.GQ_Find_CityCardsInCurrentPlayerHand();

            var citiesOfCityCardsInPlayerHand = new List<PD_City>();
            foreach (var card in cityCardsInPlayerHand)
            {
                citiesOfCityCardsInPlayerHand.Add(card.City);
            }

            var allOtherCities = game.Map.Cities.FindAll(
                x =>
                x != currentPlayerLocation
                );

            var allOtherCitiesThatContainResearchStations = new List<PD_City>();
            foreach (var city in allOtherCities)
            {
                if (game.MapElements.ResearchStationsPerCityID[city.ID].Count > 0)
                {
                    allOtherCitiesThatContainResearchStations.Add(city);
                }
            }

            bool currentLocationDoesNotHaveAResearchStation = game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count == 0;
            bool playerOwnsCardOfCurrentCity = citiesOfCityCardsInPlayerHand.Any(x => x == currentPlayerLocation);
            bool thereAreNotInactiveResearchStations = game.MapElements.InactiveResearchStations.Count == 0;

            List<PD_PA_MoveResearchStation> moveResearchStationActions = new List<PD_PA_MoveResearchStation>();
            if (currentLocationDoesNotHaveAResearchStation && playerOwnsCardOfCurrentCity && thereAreNotInactiveResearchStations)
            {
                foreach (var rs_city in allOtherCitiesThatContainResearchStations)
                {
                    PD_PA_MoveResearchStation action = new PD_PA_MoveResearchStation(
                        currentPlayer,
                        cityCardsInPlayerHand.Find(x => x.City == currentPlayerLocation),
                        rs_city,
                        currentPlayerLocation
                        );
                    moveResearchStationActions.Add(action);
                }
            }

            return moveResearchStationActions;
        }

        private static List<PD_PA_TreatDisease> Find_AvailablePlayerActions_PA_TreatDisease(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();

            bool currentPlayerIsMedic = game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Medic;
            if (currentPlayerIsMedic)
            {
                return new List<PD_PA_TreatDisease>();
            }

            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            List<int> infectionCubesTypesOnCity =
                game.GQ_Find_InfectionCubeTypes_OnCity(currentPlayerLocation);

            List<PD_PA_TreatDisease> availableTreatDiseaseActions = new List<PD_PA_TreatDisease>();

            foreach (var type in infectionCubesTypesOnCity)
            {
                var action = new PD_PA_TreatDisease(currentPlayer, currentPlayerLocation, type);
                availableTreatDiseaseActions.Add(action);
            }

            return availableTreatDiseaseActions;
        }

        private static List<PD_PA_TreatDisease_Medic> Find_AvailablePlayerActions_PA_TreatDisease_Medic(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();

            bool currentPlayerIsMedic = game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Medic;
            if (currentPlayerIsMedic == false)
            {
                return new List<PD_PA_TreatDisease_Medic>();
            }

            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            List<int> infectionCubesTypesOnCity =
                game.GQ_Find_InfectionCubeTypes_OnCity(currentPlayerLocation);

            List<PD_PA_TreatDisease_Medic> availableTreatDiseaseMedicActions = new List<PD_PA_TreatDisease_Medic>();

            foreach (var type in infectionCubesTypesOnCity)
            {
                var action = new PD_PA_TreatDisease_Medic(currentPlayer, currentPlayerLocation, type);
                availableTreatDiseaseMedicActions.Add(action);
            }

            return availableTreatDiseaseMedicActions;
        }

        private static List<PD_PA_ShareKnowledge_GiveCard> Find_AvailablePlayerActions_PA_ShareKnowledge_GiveCard(
            PD_Game game
            )
        {
            PD_Player_Roles currentPlayerRole = game.GQ_CurrentPlayer_Role();
            if (currentPlayerRole == PD_Player_Roles.Researcher)
            {
                return new List<PD_PA_ShareKnowledge_GiveCard>();
            }

            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            var cityCardsInCurrentPlayerHand = game.GQ_CityCardsInPlayerHand(currentPlayer);

            var cityCardToGive = game.GameElementReferences.CityCards.Find(
                x =>
                x.City == currentPlayerLocation
                );

            List<PD_PA_ShareKnowledge_GiveCard> available_ShareKnowledge_GiveCard_Actions =
                new List<PD_PA_ShareKnowledge_GiveCard>();

            if (cityCardsInCurrentPlayerHand.Contains(cityCardToGive))
            {
                var otherPlayersInSameLocation = game.Players.FindAll(
                    x =>
                    x != currentPlayer
                    && game.GQ_PlayerLocation(x) == currentPlayerLocation
                    );

                foreach (var otherPlayerInSameLocation in otherPlayersInSameLocation)
                {
                    var shareKnowledgeAction =
                        new PD_PA_ShareKnowledge_GiveCard(
                            currentPlayer,
                            otherPlayerInSameLocation,
                            cityCardToGive
                            );

                    available_ShareKnowledge_GiveCard_Actions.Add(shareKnowledgeAction);
                }
            }

            return available_ShareKnowledge_GiveCard_Actions;
        }

        private static List<PD_PA_ShareKnowledge_TakeCard>FindAvailable_ShareKnowledge_TakeCard_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            List<PD_PA_ShareKnowledge_TakeCard> availableShareKnowledge_TakeCard_Actions =
                new List<PD_PA_ShareKnowledge_TakeCard>();

            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            var cityCardToTake = game.GameElementReferences.CityCards.Find(x => x.City == currentPlayerLocation);

            var otherPlayersInSameLocation = game.Players.FindAll(
                x =>
                x != currentPlayer
                && currentPlayerLocation == game.GQ_PlayerLocation(x)
                );

            foreach (var otherPlayerInSameLocation in otherPlayersInSameLocation)
            {
                PD_Player_Roles otherPlayerRole = game.GQ_Find_Player_Role(otherPlayerInSameLocation);
                if (otherPlayerRole == PD_Player_Roles.Researcher)
                {
                    continue;
                }
                var otherPlayerCityCards = game.GQ_CityCardsInPlayerHand(otherPlayerInSameLocation);
                if (otherPlayerCityCards.Contains(cityCardToTake))
                {
                    var action = new PD_PA_ShareKnowledge_TakeCard(
                        currentPlayer,
                        otherPlayerInSameLocation,
                        cityCardToTake
                        );

                    availableShareKnowledge_TakeCard_Actions.Add(action);
                }

            }

            return availableShareKnowledge_TakeCard_Actions;
        }

        private static List<PD_PA_ShareKnowledge_GiveCard_ResearcherGives>
            FindAvailable_ShareKnowledge_GiveCard_ResearcherGives_Actions(
            PD_Game game
            )
        {
            PD_Player_Roles currentPlayerRole = game.GQ_CurrentPlayer_Role();
            if (currentPlayerRole != PD_Player_Roles.Researcher)
            {
                return new List<PD_PA_ShareKnowledge_GiveCard_ResearcherGives>();
            }

            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            var otherPlayers = game.Players.FindAll(
                x =>
                x != currentPlayer
                );
            var otherPlayersInSameLocation = otherPlayers.FindAll(
                x =>
                game.GQ_PlayerLocation(x) == currentPlayerLocation
                );

            List<PD_PA_ShareKnowledge_GiveCard_ResearcherGives> available_ShareKnowledge_GiveCard_ResearcherGives_Actions =
                new List<PD_PA_ShareKnowledge_GiveCard_ResearcherGives>();

            var cityCardsInCurrentPlayerHand = game.GQ_CityCardsInPlayerHand(currentPlayer);

            foreach (var otherPlayer in otherPlayersInSameLocation)
            {
                PD_City otherPlayerLocation = game.GQ_PlayerLocation(otherPlayer);
                if (otherPlayerLocation == currentPlayerLocation)
                {
                    foreach (var cityCardInCurrentPlayerHand in cityCardsInCurrentPlayerHand)
                    {
                        PD_PA_ShareKnowledge_GiveCard_ResearcherGives giveCommand = new PD_PA_ShareKnowledge_GiveCard_ResearcherGives(
                            currentPlayer,
                            otherPlayer,
                            cityCardInCurrentPlayerHand
                            );
                        available_ShareKnowledge_GiveCard_ResearcherGives_Actions.Add(giveCommand);
                    }
                }
            }

            return available_ShareKnowledge_GiveCard_ResearcherGives_Actions;
        }

        private static List<PD_PA_ShareKnowledge_TakeCard_FromResearcher>
            FindAvailable_ShareKnowledge_TakeCard_FromResearcher_Actions(
            PD_Game game
            )
        {
            PD_Player currentPlayer = game.GQ_CurrentPlayer();

            PD_Player_Roles currentPlayerRole = game.GQ_CurrentPlayer_Role();
            if (currentPlayerRole == PD_Player_Roles.Researcher)
            {
                return new List<PD_PA_ShareKnowledge_TakeCard_FromResearcher>();
            }

            var researcher = game.Players.Find(
                x =>
                game.GQ_Find_Player_Role(x) == PD_Player_Roles.Researcher
                );

            if (researcher == null)
            {
                return new List<PD_PA_ShareKnowledge_TakeCard_FromResearcher>();
            }

            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            PD_City researcherLocation = game.GQ_PlayerLocation(researcher);

            if (researcherLocation != currentPlayerLocation)
            {
                return new List<PD_PA_ShareKnowledge_TakeCard_FromResearcher>();
            }

            List<PD_PA_ShareKnowledge_TakeCard_FromResearcher>
                available_ShareKnowledge_TakeCard_FromResearcher_Actions =
                new List<PD_PA_ShareKnowledge_TakeCard_FromResearcher>();

            List<PD_CityCard> cityCardsInResearchersHand = game.GQ_CityCardsInPlayerHand(researcher);
            foreach (var cityCardToTake in cityCardsInResearchersHand)
            {
                PD_PA_ShareKnowledge_TakeCard_FromResearcher takeAction = new PD_PA_ShareKnowledge_TakeCard_FromResearcher(
                    currentPlayer,
                    researcher,
                    cityCardToTake
                    );
                available_ShareKnowledge_TakeCard_FromResearcher_Actions.Add(takeAction);
            }

            return available_ShareKnowledge_TakeCard_FromResearcher_Actions;
        }

        private static List<PD_PA_DiscoverCure>
            FindAvailable_DiscoverCure_Actions(
            PD_Game game
            )
        {
            PD_Player_Roles currentPlayerRole = game.GQ_CurrentPlayer_Role();
            bool currentPlayerRoleIsScientist = currentPlayerRole == PD_Player_Roles.Scientist;
            if (currentPlayerRoleIsScientist)
            {
                return new List<PD_PA_DiscoverCure>();
            }

            var currentPlayerLocation = game.GQ_Find_CurrentPlayer_Location();
            bool currentPlayerLocationIsResearchStation =
                game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count > 0;
            if (currentPlayerLocationIsResearchStation == false)
            {
                return new List<PD_PA_DiscoverCure>();
            }

            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var usableDiscoverCureCardGroups =
                game.GQ_Find_UsableDiscoverCureCardGroups(currentPlayer);
            if (usableDiscoverCureCardGroups.Count == 0)
            {
                return new List<PD_PA_DiscoverCure>();
            }

            List<PD_PA_DiscoverCure> availableDiscoverCureActions = new List<PD_PA_DiscoverCure>();
            foreach (var usableDiscoverCureCardGroup in usableDiscoverCureCardGroups)
            {
                var action = new PD_PA_DiscoverCure(
                    currentPlayer,
                    currentPlayerLocation,
                    usableDiscoverCureCardGroup,
                    usableDiscoverCureCardGroup[0].Type
                    );
                availableDiscoverCureActions.Add(action);
            }

            return availableDiscoverCureActions;
        }

        private static List<PD_PA_DiscoverCure_Scientist>
            FindAvailable_DiscoverCure_Scientist_Actions(
            PD_Game game
            )
        {
            PD_Player_Roles currentPlayerRole = game.GQ_CurrentPlayer_Role();
            bool currentPlayerRoleIsScientist = currentPlayerRole == PD_Player_Roles.Scientist;
            if (currentPlayerRoleIsScientist == false)
            {
                return new List<PD_PA_DiscoverCure_Scientist>();
            }

            var currentPlayerLocation = game.GQ_Find_CurrentPlayer_Location();
            bool currentPlayerLocationIsResearchStation =
                game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count > 0;
            if (currentPlayerLocationIsResearchStation == false)
            {
                return new List<PD_PA_DiscoverCure_Scientist>();
            }

            PD_Player currentPlayer = game.GQ_CurrentPlayer();
            var usableDiscoverCureCardGroups =
                game.GQ_Find_UsableDiscoverCureCardGroups(currentPlayer);
            if (usableDiscoverCureCardGroups.Count == 0)
            {
                return new List<PD_PA_DiscoverCure_Scientist>();
            }

            List<PD_PA_DiscoverCure_Scientist> availableDiscoverCureActions = new List<PD_PA_DiscoverCure_Scientist>();
            foreach (var usableDiscoverCureCardGroup in usableDiscoverCureCardGroups)
            {
                var action = new PD_PA_DiscoverCure_Scientist(
                    currentPlayer,
                    currentPlayerLocation,
                    usableDiscoverCureCardGroup,
                    usableDiscoverCureCardGroup[0].Type
                    );
                availableDiscoverCureActions.Add(action);
            }

            return availableDiscoverCureActions;
        }

        private static List<PD_PA_Discard_DuringMainPlayerActions>
            GAQ_FindAvailable_DiscardDuringMainPlayerActions_Actions(
            PD_Game game
            )
        {
            List<PD_PA_Discard_DuringMainPlayerActions> discardDuringMainPlayerActions_Actions =
                new List<PD_PA_Discard_DuringMainPlayerActions>();

            if (game.GQ_IsInState_DiscardDuringMainPlayerActions())
            {
                foreach (var player in game.Players)
                {
                    var cityCardsInPlayerHand = game.GQ_CityCardsInPlayerHand(player);
                    if (cityCardsInPlayerHand.Count > 7)
                    {
                        foreach (var cardToDiscard in cityCardsInPlayerHand)
                        {
                            PD_PA_Discard_DuringMainPlayerActions discardAction = new PD_PA_Discard_DuringMainPlayerActions(
                                player,
                                cardToDiscard
                                );
                            discardDuringMainPlayerActions_Actions.Add(discardAction);
                        }
                    }
                }
            }
            return discardDuringMainPlayerActions_Actions;
        }

        private static List<PD_PA_Discard_AfterDrawing>
            GAQ_FindAvailable_DiscardAfterDrawing_Actions(
            PD_Game game
            )
        {
            List<PD_PA_Discard_AfterDrawing> discardAfterDrawing_Actions = new List<PD_PA_Discard_AfterDrawing>();

            if (game.GQ_IsInState_DiscardAfterDrawing())
            {
                PD_Player currentPlayer = game.GQ_CurrentPlayer();

                List<PD_CityCard> cityCardsInCurrentPlayerHand =
                    game.GQ_Find_CityCardsInCurrentPlayerHand();
                if (cityCardsInCurrentPlayerHand.Count > 7)
                {
                    foreach (var cardToDiscard in cityCardsInCurrentPlayerHand)
                    {
                        PD_PA_Discard_AfterDrawing discardAction = new PD_PA_Discard_AfterDrawing(
                            currentPlayer,
                            cardToDiscard
                            );
                        discardAfterDrawing_Actions.Add(discardAction);
                    }

                }
            }
            return discardAfterDrawing_Actions;
        }

    }
}
