using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_Queries
    {

        public static int GQ_City_InfectionType(
            this PD_Game game,
            int city
            )
        {
            return game.Map.infection_type__per__city[city];
        }

        public static bool GQ_OperationsExpertFlight_HasBeenUsedInThisTurn(
            this PD_Game game
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

        public static int GQ_Find_Medic_Location(
            this PD_Game game
            )
        {
            int medic = game.Players.Find(
                x =>
                GQ_Find_Player_Role(game, x) == PD_Player_Roles.Medic
                );
            int medic_location = -1;
            if (medic != null)
            {
                medic_location = game.GQ_PlayerLocation(medic);
            }
            return medic_location;
        }

        public static int Num_InactiveInfectionCubes_OfType(
            this PD_Game game,
            int type
            )
        {
            return game.MapElements.inactive_infection_cubes__per__type[type];
        }

        public static int GQ_RemainingPlayerActions_ThisRound(
            this PD_Game game
            )
        {
            return 4 - game.GameStateCounter.CurrentPlayerActionIndex;
        }

        public static List<PD_CityCard> GQ_Find_CityCards_InAllPlayersHands(
            this PD_Game game
            )
        {
            List<PD_CityCard> allCityCardsInAllPlayersHands = new List<PD_CityCard>();
            foreach (var player in game.Players)
            {
                allCityCardsInAllPlayersHands.AddRange(
                    GQ_CityCardsInPlayerHand(game, player)
                    );
            }
            return allCityCardsInAllPlayersHands;
        }

        public static bool GQ_SS_NotEnoughDiseaseCubestoCompleteAnInfection(
            this PD_Game game
            )
        {
            return game.GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection;
        }

        public static bool GQ_SS_DeadlyOutbreaks(
            this PD_Game game
            )
        {
            return
                game.GameStateCounter.OutbreaksCounter
                > game.GameSettings.MaximumViableOutbreaks;
        }

        public static bool GQ_SS_AllDiseasesCured(
            this PD_Game game
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
            this PD_Game game
            )
        {
            int numPlayerCards =
                game.Cards.DividedDeckOfPlayerCards.GetNumberOfElementsOfAllSubLists();
            return numPlayerCards >= 2;
        }

        public static bool GQ_SS_ThereAreActiveInfectionCards(
            this PD_Game game
            )
        {
            return game.Cards.ActiveInfectionCards.Count > 0;
        }

        public static bool SS_PlayerActionsFinished(
            this PD_Game game
            )
        {
            return game.GameStateCounter.CurrentPlayerActionIndex > 3;
        }

        public static bool GQ_SS_CurrentPlayerHandIsBiggerThanPermitted(
            this PD_Game game
            )
        {
            return
                GQ_CurrentPlayerHand(game).Count
                >
                game.GameSettings.MaximumNumberOfPlayerCardsPerPlayer;
        }

        public static bool GQ_SS_CurrentPlayerHandIncludesEpidemicCard(
            this PD_Game game
            )
        {
            foreach (var card in GQ_CurrentPlayerHand(game))
            {
                if (card.GetType() == typeof(PD_EpidemicCard))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GQ_SS_AnyPlayerHandIsBiggerThanPermitted(
            this PD_Game game
            )
        {
            foreach (var player in game.Players)
            {
                var playerHand = game.Cards.PlayerCardsPerPlayerID[player];
                if (playerHand.Count > game.GameSettings.MaximumNumberOfPlayerCardsPerPlayer)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GQ_Is_Won(
            this PD_Game game
            )
        {
            return game.GameFSM.CurrentState.GetType() == typeof(PD_GS_GameWon);
        }

        public static bool GQ_Is_Lost(
            this PD_Game game
            )
        {
            return game.GameFSM.CurrentState.GetType() == typeof(PD_GS_GameLost);
        }

        public static bool GQ_Is_Ongoing(
            this PD_Game game
            )
        {
            return !GQ_Is_Finished(game);
        }

        public static bool GQ_Is_Finished(
            this PD_Game game
            )
        {
            return GQ_Is_Won(game) || GQ_Is_Lost(game);
        }

        public static List<PD_PlayerCardBase> GQ_CurrentPlayerHand(
            this PD_Game game
            )
        {
            var currentPlayer = GQ_CurrentPlayer(game);
            return game.Cards.PlayerCardsPerPlayerID[currentPlayer];
        }

        public static List<PD_PlayerCardBase> GQ_PlayerHand(
            this PD_Game game,
            int player
            )
        {
            return game.Cards.PlayerCardsPerPlayerID[player];
        }

        public static bool GQ_Is_DiseaseCured_OR_Eradicated(
            this PD_Game game,
            int diseaseType
            )
        {
            return
                game.GameStateCounter.CureMarkersStates[diseaseType] == 1
                ||
                game.GameStateCounter.CureMarkersStates[diseaseType] == 2;
        }

        public static bool GQ_Is_DiseaseCured_AND_NOT_Eradicated(
            this PD_Game game,
            int diseaseType
            )
        {
            return game.GameStateCounter.CureMarkersStates[diseaseType] == 1;
        }

        public static bool GQ_Is_Disease_Eradicated(
            this PD_Game game,
            int diseaseType
            )
        {
            return game.GameStateCounter.CureMarkersStates[diseaseType] == 2;
        }

        public static int GQ_CurrentPlayer(
            this PD_Game game
            )
        {
            return game.Players[game.GameStateCounter.CurrentPlayerIndex];
        }

        public static int GQ_CurrentPlayer_Role(
            this PD_Game game
            )
        {
            var currentPlayer = GQ_CurrentPlayer(game);
            return GQ_Find_Player_Role(game, currentPlayer);
        }

        public static int GQ_Find_Player_Role(
            this PD_Game game,
            int player
            )
        {
            return game.RoleCardsPerPlayerID[player];
        }


        public static List<int> GQ_InfectionCubeTypes_OnCity(
            this PD_Game game,
            int city
            )
        {
            List<int> infectionCubeTypesOnSpecificCity = new List<int>();

            for (int t = 0; t < 4; t++)
            {
                if (game.MapElements.infections__per__type__per__city[city][t] > 0)
                {
                    infectionCubeTypesOnSpecificCity.Add(t);
                }
            }

            return infectionCubeTypesOnSpecificCity;
        }

        public static Dictionary<int, List<int>> GQ_InfectionCubeTypesPerInfectedCity(
            this PD_Game game
            )
        {
            Dictionary<int, List<int>> infectionCubeTypesPerInfectedCity = new Dictionary<int, List<int>>();

            var allInfectedCities = GQ_InfectedCities(game);
            foreach (var city in allInfectedCities)
            {
                List<int> infectionCubeTypesOnThisCity = GQ_InfectionCubeTypes_OnCity(game, city);
                infectionCubeTypesPerInfectedCity.Add(city, infectionCubeTypesOnThisCity);
            }
            return infectionCubeTypesPerInfectedCity;
        }

        public static List<List<PD_CityCard>> GQ_Find_UsableDiscoverCureCardGroups(
            this PD_Game game,
            int player
            )
        {
            var discoverCureCardGroups = GQ_Find_DiscoverCureCardGroups(game, player);
            var uncuredDiseaseTypes = GQ_UncuredDiseaseTypes(game);

            var usableDiscoverCureCardGroups = new List<List<PD_CityCard>>();
            foreach (var cardGroup in discoverCureCardGroups)
            {
                int city = cardGroup[0].City;
                int disease_type = game.GQ_City_InfectionType(city);
                if (uncuredDiseaseTypes.Contains(disease_type))
                {
                    usableDiscoverCureCardGroups.Add(cardGroup);
                }
            }

            return usableDiscoverCureCardGroups;
        }

        public static List<List<PD_CityCard>> GQ_Find_DiscoverCureCardGroups(
            this PD_Game game,
            int player
            )
        {
            bool player_Is_Scientist = game.RoleCardsPerPlayerID[player] == PD_Player_Roles.Scientist;
            var cityCardsInPlayerHand = GQ_CityCardsInPlayerHand(game, player);

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
                int city_card_infection_type = game.Map.infection_type__per__city[cityCard.City];
                if (availableTypes.Contains(city_card_infection_type) == false)
                {
                    availableTypes.Add(city_card_infection_type);
                }
            }

            List<List<PD_CityCard>> cityCardsByType = new List<List<PD_CityCard>>();
            foreach (var type in availableTypes)
            {
                List<PD_CityCard> cityCardsOfThisType = cityCardsInPlayerHand.FindAll(
                    x =>
                    game.GQ_City_InfectionType(x.City) == type
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

        public static int GQ_CurrentPlayer_Location(
            this PD_Game game
            )
        {
            int currentPlayer = GQ_CurrentPlayer(game);
            return game.MapElements.location__per__player[currentPlayer];
        }

        public static int GQ_PlayerLocation(
            this PD_Game game,
            int player
            )
        {
            return game.MapElements.location__per__player[player];
        }

        public static int GQ_Find_CityByName(
            this PD_Game game,
            string cityName
            )
        {
            return game.Map.cities.Find(
                x =>
                game.Map.name__per__city[x] == cityName
                );
        }

        public static List<int> GQ_ResearchStationCities(
            this PD_Game game
            )
        {
            List<int> researchStationCities = new List<int>();
            foreach (var city in game.Map.cities)
            {
                if (GQ_Is_City_ResearchStation(game, city))
                {
                    researchStationCities.Add(city);
                }
            }
            return researchStationCities;
        }

        public static bool GQ_Is_City_ResearchStation(
            this PD_Game game,
            int city
            )
        {
            return game.MapElements.research_stations__per__city[city] == true;
        }

        public static List<int> GQ_InfectedCities(
            this PD_Game game
            )
        {
            List<int> allInfectedCities = new List<int>();
            foreach (var city in game.Map.cities)
            {
                for (int t = 0; t < 4; t++)
                {
                    if (game.MapElements.infections__per__type__per__city[city][t] > 0)
                    {
                        allInfectedCities.Add(city);
                        break;
                    }
                }
            }
            return allInfectedCities;
        }

        public static List<int> GQ_Find_InfectionTypePerCity_MinCubes(
            this PD_Game game,
            int minimumCubes_AnyType
            )
        {
            List<int> allInfectedCities = new List<int>();
            foreach (var city in game.Map.cities)
            {
                if (game.Num_InfectionCubes_OnCity(city) >= minimumCubes_AnyType)
                {
                    allInfectedCities.Add(city);
                }
            }
            return allInfectedCities;
        }

        public static int Num_InfectionCubes_OnCity(
            this PD_Game game,
            int city
            )
        {
            int num_cubes = 0;
            for (int t = 0; t < 4; t++)
            {
                num_cubes += game.MapElements.infections__per__type__per__city[city][t];
            }
            return num_cubes;
        }

        public static List<PD_CityCard> GQ_CityCardsInCurrentPlayerHand(
            this PD_Game game
            )
        {
            int currentPlayer = game.GQ_CurrentPlayer();
            return GQ_CityCardsInPlayerHand(
                game,
                currentPlayer
                );
        }

        public static List<PD_CityCard> GQ_CityCardsInPlayerHand(
            this PD_Game game,
            int player
            )
        {
            List<PD_CityCard> city_cards_in_player_hand = new List<PD_CityCard>();
            foreach (var card in game.Cards.PlayerCardsPerPlayerID[player]) {
                if (card is PD_CityCard city_card) {
                    city_cards_in_player_hand.Add(city_card);
                }
            }
            return city_cards_in_player_hand;

            //var playerHand = game.Cards.PlayerCardsPerPlayerID[player];
            //var cityCardsInPlayerHand = playerHand.FindAll(
            //    x =>
            //    x.GetType() == typeof(PD_CityCard)
            //    ).Cast<PD_CityCard>().ToList();
            //return cityCardsInPlayerHand;
        }

        public static List<int> GQ_UncuredDiseaseTypes(
            this PD_Game game
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

        public static int GQ_InfectionCubes_OfType_OnCity(
            this PD_Game game,
            int city,
            int type
            )
        {
            return game.MapElements.infections__per__type__per__city[city][type];
        }

        public static int GQ_Find_InfectionCubes_OfType_OnCity(
            this PD_Game game,
            int city,
            int type
            )
        {

            return game.MapElements.infections__per__type__per__city[city][type];
        }

        public static List<int> GQ_Find_Cured_or_Eradicated_DiseaseTypes(
            this PD_Game game
            )
        {
            List<int> curedOrEradicatedDiseaseTypes = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (game.GameStateCounter.CureMarkersStates[i] > 0)
                    curedOrEradicatedDiseaseTypes.Add(i);
            }
            return curedOrEradicatedDiseaseTypes;
        }

        public static int GQ_Num_DiseasesCured(
            this PD_Game game
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
            this PD_Game game
            )
        {
            if (game.GameFSM.CurrentState is PD_GS_ApplyingMainPlayerActions)
            {
                return true;
            }
            return false;
        }

        public static bool GQ_IsInState_DiscardDuringMainPlayerActions(
            this PD_Game game
            )
        {
            if (game.GameFSM.CurrentState is PD_GS_Discarding_DuringMainPlayerActions)
            {
                return true;
            }
            return false;
        }

        public static bool GQ_IsInState_DrawingNewPlayerCards(
            this PD_Game game
            )
        {
            if (game.GameFSM.CurrentState is PD_GS_DrawingNewPlayerCards)
            {
                return true;
            }
            return false;
        }

        public static bool GQ_IsInState_DiscardAfterDrawing(
            this PD_Game game
            )
        {
            if (game.GameFSM.CurrentState is PD_GS_Discarding_AfterDrawing)
            {
                return true;
            }
            return false;
        }

        public static bool GQ_IsInState_Discard_Any(
            this PD_Game game
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
            this PD_Game game
            )
        {
            return game.MapElements.inactive_research_stations;
        }

        public static int GQ_NumActiveResearchStations(
            this PD_Game game
            )
        {
            int totalNumResearchStations = 6;
            int numInactiveResearchStations = GQ_NumInactiveResearchStations(game);
            return totalNumResearchStations - numInactiveResearchStations;
        }
    }
}
