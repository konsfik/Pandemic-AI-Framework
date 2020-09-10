using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_Action_Queries
    {
        public static List<PD_GameAction_Base> FindAvailable_PlayerActions(
            this PD_Game game
            )
        {
            List<PD_GameAction_Base> action_set = new List<PD_GameAction_Base>();

            int currentPlayer = game.GQ_CurrentPlayer();

            if (game.game_FSM.CurrentState.GetType() != null && game.game_FSM.CurrentState.GetType() == typeof(PD_GS_Idle))
            {
                PD_GA_SetupGame_Random sg = new PD_GA_SetupGame_Random();
                action_set.Add(sg);
            }
            else if (game.GQ_IsInState_ApplyingMainPlayerActions())
            {
                action_set.AddRange(FindAvailable_Stay_Actions(game));

                action_set.AddRange(FindAvailable_DriveFerry_Actions(game));
                action_set.AddRange(FindAvailable_DirectFlight_Actions(game));
                action_set.AddRange(FindAvailable_CharterFlight_Actions(game));
                action_set.AddRange(FindAvailable_ShuttleFlight_Actions(game));
                action_set.AddRange(FindAvailable_OperationsExpertFlight_Actions(game));

                action_set.AddRange(FindAvailable_BuildResearchStation_Actions(game));
                action_set.AddRange(FindAvailable_BuildResearchStation_OperationsExpert_Actions(game));
                action_set.AddRange(FindAvailable_MoveResearchStation_Actions(game));

                action_set.AddRange(FindAvailable_TreatDisease_Actions(game));
                action_set.AddRange(FindAvailable_TreatDisease_Medic_Actions(game));

                action_set.AddRange(FindAvailable_ShareKnowledge_Give_Actions(game));
                action_set.AddRange(FindAvailable_ShareKnowledge_Take_Actions(game));
                action_set.AddRange(FindAvailable_ShareKnowledge_Give_ResearcherGives_Actions(game));
                action_set.AddRange(FindAvailable_ShareKnowledge_Take_FromResearcher_Actions(game));

                action_set.AddRange(FindAvailable_DiscoverCure_Actions(game));
                action_set.AddRange(FindAvailable_DiscoverCure_Scientist_Actions(game));
            }
            else if (game.GQ_IsInState_DiscardDuringMainPlayerActions())
            {
                action_set.AddRange(
                    GAQ_FindAvailable_DiscardDuringMainPlayerActions_Actions(game)
                    );
            }
            else if (game.GQ_IsInState_DrawingNewPlayerCards())
            {
                action_set.Add(
                    new PA_DrawNewPlayerCards(currentPlayer)
                    );
            }
            else if (game.GQ_IsInState_DiscardAfterDrawing())
            {
                action_set.AddRange(
                    GAQ_FindAvailable_DiscardAfterDrawing_Actions(game)
                    );
            }
            else if (game.game_FSM.CurrentState.GetType() == typeof(PD_GS_ApplyingEpidemicCard))
            {
                action_set.Add(
                    new PA_ApplyEpidemicCard(currentPlayer)
                    );
            }
            else if (game.game_FSM.CurrentState.GetType() == typeof(PD_GS_DrawingNewInfectionCards))
            {
                action_set.Add(
                    new PA_DrawNewInfectionCards(currentPlayer)
                    );
            }
            else if (game.game_FSM.CurrentState.GetType() == typeof(PD_GS_ApplyingInfectionCards))
            {
                action_set.Add(
                    new PA_ApplyInfectionCard(currentPlayer, game.cards.active_infection_cards[0])
                    );
            }

            return action_set;
        }

        private static List<PA_Stay> FindAvailable_Stay_Actions(
            PD_Game game
            )
        {
            return new List<PA_Stay>() {
                new PA_Stay(
                    game.GQ_CurrentPlayer(),
                    game.GQ_CurrentPlayer_Location()
                    )
            };
        }

        private static List<PA_DriveFerry> FindAvailable_DriveFerry_Actions(
            PD_Game game
            )
        {
            int current_player = game.GQ_CurrentPlayer();
            int current_player_location = game.GQ_CurrentPlayer_Location();
            List<int> neighbor_locations = game.map.neighbors__per__city[current_player_location];

            List<PA_DriveFerry> drive_ferry_actions = new List<PA_DriveFerry>();
            foreach (int neighbor_location in neighbor_locations)
            {
                drive_ferry_actions.Add(
                    new PA_DriveFerry(
                        current_player,
                        current_player_location,
                        neighbor_location
                        )
                    );
            }
            return drive_ferry_actions;
        }

        private static List<PA_DirectFlight> FindAvailable_DirectFlight_Actions(
            PD_Game game
            )
        {
            int current_player = game.GQ_CurrentPlayer();
            int current_player_location = game.GQ_CurrentPlayer_Location();

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            var directFlightActions = new List<PA_DirectFlight>();

            foreach (int cityCard in cityCardsInCurrentPlayerHand)
            {
                if (cityCard != current_player_location)
                {
                    var action = new PA_DirectFlight(
                        current_player,
                        current_player_location,
                        cityCard,
                        cityCard);
                    directFlightActions.Add(action);
                }
            }

            return directFlightActions;
        }

        private static List<PA_CharterFlight> FindAvailable_CharterFlight_Actions(
            PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();
            int current_player_location = game.GQ_CurrentPlayer_Location();

            var cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            var charterFlightActions = new List<PA_CharterFlight>();

            // if the player is standing on a location that exists in the city cards in their hand
            var citiesOfCityCardsInPlayerHand = new List<int>();
            foreach (var card in cityCardsInCurrentPlayerHand)
            {
                citiesOfCityCardsInPlayerHand.Add(card);
            }

            if (citiesOfCityCardsInPlayerHand.Any(x => x == current_player_location))
            {
                var allOtherCities = game.map.cities.FindAll(
                    x =>
                    x != current_player_location
                    );

                var cityCardtoDiscard = cityCardsInCurrentPlayerHand.Find(
                    x =>
                    x == current_player_location
                    );

                foreach (var otherCity in allOtherCities)
                {
                    var action = new PA_CharterFlight(
                        currentPlayer,
                        current_player_location,
                        otherCity,
                        cityCardtoDiscard);
                    charterFlightActions.Add(action);
                }
            }

            return charterFlightActions;
        }

        private static List<PA_ShuttleFlight> FindAvailable_ShuttleFlight_Actions(
            PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();
            var current_player_location = game.GQ_CurrentPlayer_Location();

            if (game.GQ_Is_City_ResearchStation(current_player_location) == false)
            {
                return new List<PA_ShuttleFlight>();
            }

            List<int> other_research_station_cities = game.map.cities.FindAll(
                x =>
                game.GQ_Is_City_ResearchStation(x)
                &&
                x != current_player_location
                );

            if (other_research_station_cities.Count == 0)
            {
                return new List<PA_ShuttleFlight>();
            }

            var shuttleFlightActions = new List<PA_ShuttleFlight>();

            foreach (var city in other_research_station_cities)
            {
                var action = new PA_ShuttleFlight(
                    currentPlayer,
                    current_player_location,
                    city
                    );
                shuttleFlightActions.Add(action);
            }

            return shuttleFlightActions;
        }

        private static List<PA_OperationsExpert_Flight> FindAvailable_OperationsExpertFlight_Actions(
            PD_Game game
            )
        {
            if (game.GQ_CurrentPlayer_Role() != PD_Player_Roles.Operations_Expert)
            {
                return new List<PA_OperationsExpert_Flight>();
            }

            int currentPlayer = game.GQ_CurrentPlayer();
            int currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            if (game.GQ_Is_City_ResearchStation(currentPlayerLocation) == false)
            {
                return new List<PA_OperationsExpert_Flight>();
            }

            if (game.GQ_OperationsExpertFlight_HasBeenUsedInThisTurn())
            {
                return new List<PA_OperationsExpert_Flight>();
            }

            List<int> city_cards_in_player_hand = game.GQ_CityCardsInCurrentPlayerHand();
            if (city_cards_in_player_hand.Count == 0)
            {
                return new List<PA_OperationsExpert_Flight>();
            }

            List<int> target_locations = game.map.cities.FindAll(
                x =>
                x != currentPlayerLocation
                );

            List<PA_OperationsExpert_Flight> available_actions = new List<PA_OperationsExpert_Flight>();

            foreach (var used_card in city_cards_in_player_hand)
            {
                foreach (var targetLocation in target_locations)
                {
                    PA_OperationsExpert_Flight action = new PA_OperationsExpert_Flight(
                        currentPlayer,
                        currentPlayerLocation,
                        targetLocation,
                        used_card
                        );

                    available_actions.Add(action);
                }
            }

            return available_actions;
        }

        private static List<PA_BuildResearchStation> FindAvailable_BuildResearchStation_Actions(
            PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            bool currentLocation_Is_RS =
                game.GQ_Is_City_ResearchStation(currentPlayerLocation);

            bool inactive_RS_Available =
                game.map_elements.inactive_research_stations > 0;

            if (
                currentLocation_Is_RS
                ||
                inactive_RS_Available == false
                )
            {
                return new List<PA_BuildResearchStation>();
            }

            bool currentPlayer_Is_Operations_Expert =
                game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Operations_Expert;

            if (currentPlayer_Is_Operations_Expert)
            {
                return new List<PA_BuildResearchStation>();
            }
            else
            {
                List<PA_BuildResearchStation> buildResearchStation_Actions = new List<PA_BuildResearchStation>();

                var cityCardsInPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

                bool player_Has_CurrentLocation_CityCard =
                    cityCardsInPlayerHand.Any(
                        x =>
                        x == currentPlayerLocation
                        );

                if (player_Has_CurrentLocation_CityCard == true)
                {
                    PA_BuildResearchStation action = new PA_BuildResearchStation(
                        currentPlayer,
                        cityCardsInPlayerHand.Find(x => x == currentPlayerLocation),
                        currentPlayerLocation
                        );
                    buildResearchStation_Actions.Add(action);
                }

                return buildResearchStation_Actions;
            }
        }

        private static List<PA_BuildResearchStation_OperationsExpert> FindAvailable_BuildResearchStation_OperationsExpert_Actions(
            PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            bool currentLocation_Is_RS = game.GQ_Is_City_ResearchStation(currentPlayerLocation);

            bool inactive_RS_Available =
                game.map_elements.inactive_research_stations > 0;

            if (
                currentLocation_Is_RS
                ||
                inactive_RS_Available == false
                )
            {
                return new List<PA_BuildResearchStation_OperationsExpert>();
            }

            bool currentPlayer_Is_Operations_Expert =
                game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Operations_Expert;

            if (currentPlayer_Is_Operations_Expert)
            {
                List<PA_BuildResearchStation_OperationsExpert> buildResearchStation_Actions =
                    new List<PA_BuildResearchStation_OperationsExpert>();

                PA_BuildResearchStation_OperationsExpert action =
                    new PA_BuildResearchStation_OperationsExpert(
                        currentPlayer,
                        currentPlayerLocation
                        );
                buildResearchStation_Actions.Add(action);

                return buildResearchStation_Actions;
            }
            return new List<PA_BuildResearchStation_OperationsExpert>();
        }

        private static List<PA_MoveResearchStation> FindAvailable_MoveResearchStation_Actions(
            PD_Game game
            )
        {
            // if there are available research stations, this action does not apply...
            if (game.GQ_NumInactiveResearchStations() > 0)
            {
                return new List<PA_MoveResearchStation>();
            }

            // if the current player's location is a research station, this action does not apply...
            if (game.GQ_Is_City_ResearchStation(game.GQ_CurrentPlayer_Location()))
            {
                return new List<PA_MoveResearchStation>();
            }

            int current_player = game.GQ_CurrentPlayer();
            var current_player_location = game.GQ_PlayerLocation(current_player);
            var city_cards_in_current_player_hand = game.GQ_CityCardsInCurrentPlayerHand();

            // if the player does not have the city card of their current location, the action cannot be executed
            if (city_cards_in_current_player_hand.Any(x => x == current_player_location) == false)
            {
                return new List<PA_MoveResearchStation>();
            }

            var research_station_cities = game.GQ_ResearchStationCities();

            List<PA_MoveResearchStation> moveResearchStationActions = new List<PA_MoveResearchStation>();
            foreach (var rs_city in research_station_cities)
            {
                PA_MoveResearchStation action = new PA_MoveResearchStation(
                    current_player,
                    city_cards_in_current_player_hand.Find(x => x == current_player_location),
                    rs_city,
                    current_player_location
                    );
                moveResearchStationActions.Add(action);
            }

            return moveResearchStationActions;
        }

        private static List<PA_TreatDisease> FindAvailable_TreatDisease_Actions(
            PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();

            bool currentPlayerIsMedic = game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Medic;
            if (currentPlayerIsMedic)
            {
                return new List<PA_TreatDisease>();
            }

            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            List<int> infectionCubesTypesOnCity =
                game.GQ_InfectionCubeTypes_OnCity(currentPlayerLocation);

            List<PA_TreatDisease> availableTreatDiseaseActions = new List<PA_TreatDisease>();

            foreach (var type in infectionCubesTypesOnCity)
            {
                var action = new PA_TreatDisease(currentPlayer, currentPlayerLocation, type);
                availableTreatDiseaseActions.Add(action);
            }

            return availableTreatDiseaseActions;
        }

        private static List<PA_TreatDisease_Medic> FindAvailable_TreatDisease_Medic_Actions(
            PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();

            bool currentPlayerIsMedic = game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Medic;
            if (currentPlayerIsMedic == false)
            {
                return new List<PA_TreatDisease_Medic>();
            }

            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            List<int> infectionCubesTypesOnCity =
                game.GQ_InfectionCubeTypes_OnCity(currentPlayerLocation);

            List<PA_TreatDisease_Medic> availableTreatDiseaseMedicActions = new List<PA_TreatDisease_Medic>();

            foreach (var type in infectionCubesTypesOnCity)
            {
                var action = new PA_TreatDisease_Medic(currentPlayer, currentPlayerLocation, type);
                availableTreatDiseaseMedicActions.Add(action);
            }

            return availableTreatDiseaseMedicActions;
        }

        private static List<PA_ShareKnowledge_GiveCard> FindAvailable_ShareKnowledge_Give_Actions(
            PD_Game game
            )
        {
            if (game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Researcher)
            {
                return new List<PA_ShareKnowledge_GiveCard>();
            }

            int current_player = game.GQ_CurrentPlayer();
            var current_player_location = game.GQ_PlayerLocation(current_player);
            var city_cards_in_player_hand = game.GQ_CityCardsInPlayerHand(current_player);
            int city_card_to_give = current_player_location;

            if (city_cards_in_player_hand.Contains(city_card_to_give) == false)
            {
                return new List<PA_ShareKnowledge_GiveCard>();
            }

            List<PA_ShareKnowledge_GiveCard> available_ShareKnowledge_GiveCard_Actions =
                new List<PA_ShareKnowledge_GiveCard>();

            var other_players_in_same_location = game.players.FindAll(
                x =>
                x != current_player
                && game.GQ_PlayerLocation(x) == current_player_location
                );

            foreach (var other_player in other_players_in_same_location)
            {
                available_ShareKnowledge_GiveCard_Actions.Add(
                    new PA_ShareKnowledge_GiveCard(
                        current_player,
                        other_player,
                        city_card_to_give
                        )
                    );
            }

            return available_ShareKnowledge_GiveCard_Actions;
        }

        private static List<PA_ShareKnowledge_TakeCard> FindAvailable_ShareKnowledge_Take_Actions(
            PD_Game game
            )
        {
            int current_player = game.GQ_CurrentPlayer();
            int current_player_location = game.GQ_CurrentPlayer_Location();
            int city_card_to_take = current_player_location;

            List<int> other_players_same_location_not_researcher = game.players.FindAll(
                x =>
                    x != current_player
                    &&
                    current_player_location == game.GQ_PlayerLocation(x)
                    &&
                    game.GQ_Player_Role(x) != PD_Player_Roles.Researcher
                );

            List<PA_ShareKnowledge_TakeCard> availableShareKnowledge_TakeCard_Actions =
                new List<PA_ShareKnowledge_TakeCard>();

            foreach (var other_player in other_players_same_location_not_researcher)
            {
                List<int> city_cards_in_other_player_hand = game.GQ_CityCardsInPlayerHand(other_player);
                if (city_cards_in_other_player_hand.Contains(city_card_to_take))
                {
                    availableShareKnowledge_TakeCard_Actions.Add(
                        new PA_ShareKnowledge_TakeCard(
                            current_player,
                            other_player,
                            city_card_to_take
                            )
                        );
                }

            }

            return availableShareKnowledge_TakeCard_Actions;
        }

        private static List<PA_ShareKnowledge_GiveCard_ResearcherGives>
            FindAvailable_ShareKnowledge_Give_ResearcherGives_Actions(
            PD_Game game
            )
        {
            int currentPlayerRole = game.GQ_CurrentPlayer_Role();
            if (currentPlayerRole != PD_Player_Roles.Researcher)
            {
                return new List<PA_ShareKnowledge_GiveCard_ResearcherGives>();
            }

            int currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);

            var otherPlayersInSameLocation = game.players.FindAll(
                x =>
                x != currentPlayer
                &&
                game.GQ_PlayerLocation(x) == currentPlayerLocation
                );

            List<PA_ShareKnowledge_GiveCard_ResearcherGives> available_actions =
                new List<PA_ShareKnowledge_GiveCard_ResearcherGives>();

            var cityCardsInCurrentPlayerHand = game.GQ_CityCardsInPlayerHand(currentPlayer);

            foreach (var otherPlayer in otherPlayersInSameLocation)
            {
                int otherPlayerLocation = game.GQ_PlayerLocation(otherPlayer);
                if (otherPlayerLocation == currentPlayerLocation)
                {
                    foreach (var cityCardInCurrentPlayerHand in cityCardsInCurrentPlayerHand)
                    {
                        PA_ShareKnowledge_GiveCard_ResearcherGives giveCommand = new PA_ShareKnowledge_GiveCard_ResearcherGives(
                            currentPlayer,
                            otherPlayer,
                            cityCardInCurrentPlayerHand
                            );
                        available_actions.Add(giveCommand);
                    }
                }
            }

            return available_actions;
        }

        private static List<PA_ShareKnowledge_TakeCard_FromResearcher>
            FindAvailable_ShareKnowledge_Take_FromResearcher_Actions(
            PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();

            int currentPlayerRole = game.GQ_CurrentPlayer_Role();
            if (currentPlayerRole == PD_Player_Roles.Researcher)
            {
                return new List<PA_ShareKnowledge_TakeCard_FromResearcher>();
            }

            if (game.players.Any(
                x => game.GQ_Player_Role(x) == PD_Player_Roles.Researcher
                ) == false)
            {
                return new List<PA_ShareKnowledge_TakeCard_FromResearcher>();
            }

            var researcher = game.players.Find(
                x =>
                game.GQ_Player_Role(x) == PD_Player_Roles.Researcher
                );

            var currentPlayerLocation = game.GQ_PlayerLocation(currentPlayer);
            int researcherLocation = game.GQ_PlayerLocation(researcher);

            if (researcherLocation != currentPlayerLocation)
            {
                return new List<PA_ShareKnowledge_TakeCard_FromResearcher>();
            }

            List<PA_ShareKnowledge_TakeCard_FromResearcher>
                available_ShareKnowledge_TakeCard_FromResearcher_Actions =
                new List<PA_ShareKnowledge_TakeCard_FromResearcher>();

            List<int> cityCardsInResearchersHand = game.GQ_CityCardsInPlayerHand(researcher);
            foreach (var cityCardToTake in cityCardsInResearchersHand)
            {
                PA_ShareKnowledge_TakeCard_FromResearcher takeAction = new PA_ShareKnowledge_TakeCard_FromResearcher(
                    currentPlayer,
                    researcher,
                    cityCardToTake
                    );
                available_ShareKnowledge_TakeCard_FromResearcher_Actions.Add(takeAction);
            }

            return available_ShareKnowledge_TakeCard_FromResearcher_Actions;
        }

        private static List<PA_DiscoverCure>
            FindAvailable_DiscoverCure_Actions(
            PD_Game game
            )
        {
            if (game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Scientist)
            {
                return new List<PA_DiscoverCure>();
            }

            int currentPlayer = game.GQ_CurrentPlayer();
            int currentPlayerLocation = game.GQ_CurrentPlayer_Location();

            if (game.GQ_Is_City_ResearchStation(currentPlayerLocation) == false)
            {
                return new List<PA_DiscoverCure>();
            }

            List<List<int>> usableDiscoverCureCardGroups =
                game.GQ_Find_UsableDiscoverCureCardGroups(currentPlayer);
            if (usableDiscoverCureCardGroups.Count == 0)
            {
                return new List<PA_DiscoverCure>();
            }

            List<PA_DiscoverCure> availableDiscoverCureActions = new List<PA_DiscoverCure>();
            foreach (var usableDiscoverCureCardGroup in usableDiscoverCureCardGroups)
            {
                int group_type = game.map.infection_type__per__city[usableDiscoverCureCardGroup[0]];
                var action = new PA_DiscoverCure(
                    currentPlayer,
                    currentPlayerLocation,
                    usableDiscoverCureCardGroup,
                    group_type
                    );
                availableDiscoverCureActions.Add(action);
            }

            return availableDiscoverCureActions;
        }

        private static List<PA_DiscoverCure_Scientist>
            FindAvailable_DiscoverCure_Scientist_Actions(
            PD_Game game
            )
        {
            if (game.GQ_CurrentPlayer_Role() != PD_Player_Roles.Scientist)
            {
                return new List<PA_DiscoverCure_Scientist>();
            }

            int currentPlayer = game.GQ_CurrentPlayer();
            var currentPlayerLocation = game.GQ_CurrentPlayer_Location();

            if (game.GQ_Is_City_ResearchStation(currentPlayerLocation) == false)
            {
                return new List<PA_DiscoverCure_Scientist>();
            }

            var usableDiscoverCureCardGroups =
                game.GQ_Find_UsableDiscoverCureCardGroups(currentPlayer);
            if (usableDiscoverCureCardGroups.Count == 0)
            {
                return new List<PA_DiscoverCure_Scientist>();
            }

            List<PA_DiscoverCure_Scientist> availableDiscoverCureActions = new List<PA_DiscoverCure_Scientist>();
            foreach (var usableDiscoverCureCardGroup in usableDiscoverCureCardGroups)
            {
                int group_type = game.map.infection_type__per__city[usableDiscoverCureCardGroup[0]];
                var action = new PA_DiscoverCure_Scientist(
                    currentPlayer,
                    currentPlayerLocation,
                    usableDiscoverCureCardGroup,
                    group_type
                    );
                availableDiscoverCureActions.Add(action);
            }

            return availableDiscoverCureActions;
        }

        private static List<PA_Discard_DuringMainPlayerActions>
            GAQ_FindAvailable_DiscardDuringMainPlayerActions_Actions(
            PD_Game game
            )
        {
            List<PA_Discard_DuringMainPlayerActions> discardDuringMainPlayerActions_Actions =
                new List<PA_Discard_DuringMainPlayerActions>();

            if (game.GQ_IsInState_DiscardDuringMainPlayerActions())
            {
                foreach (var player in game.players)
                {
                    var cityCardsInPlayerHand = game.GQ_CityCardsInPlayerHand(player);
                    if (cityCardsInPlayerHand.Count > 7)
                    {
                        foreach (var cardToDiscard in cityCardsInPlayerHand)
                        {
                            PA_Discard_DuringMainPlayerActions discardAction = new PA_Discard_DuringMainPlayerActions(
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

        private static List<PA_Discard_AfterDrawing>
            GAQ_FindAvailable_DiscardAfterDrawing_Actions(
            PD_Game game
            )
        {
            List<PA_Discard_AfterDrawing> discardAfterDrawing_Actions = new List<PA_Discard_AfterDrawing>();

            if (game.GQ_IsInState_DiscardAfterDrawing())
            {
                int currentPlayer = game.GQ_CurrentPlayer();

                List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();
                if (cityCardsInCurrentPlayerHand.Count > 7)
                {
                    foreach (var cardToDiscard in cityCardsInCurrentPlayerHand)
                    {
                        PA_Discard_AfterDrawing discardAction = new PA_Discard_AfterDrawing(
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
