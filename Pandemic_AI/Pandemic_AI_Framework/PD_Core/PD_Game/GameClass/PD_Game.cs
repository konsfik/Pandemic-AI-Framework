using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public partial class PD_Game : ICustomDeepCopyable<PD_Game>
    {
        #region properties
        public long UniqueID { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public PD_GameSettings GameSettings { get; private set; }
        public PD_GameFSM GameFSM { get; private set; }

        // STATE - RELATED
        public PD_GameStateCounter GameStateCounter { get; private set; }

        // REFERENCES
        public List<PD_Player> Players { get; private set; }
        public PD_Map Map { get; private set; }
        public PD_GameElementReferences GameElementReferences { get; private set; }

        // CONTAINERS
        public PD_GameCards Cards { get; private set; }
        public PD_MapElements MapElements { get; private set; }

        // OWNERSHIPS
        public Dictionary<int, PD_ME_PlayerPawn> PlayerPawnsPerPlayerID { get; private set; }
        public Dictionary<int, PD_Role_Card> RoleCardsPerPlayerID { get; private set; }

        // GAME HISTORY
        public List<PD_GameAction_Base> PlayerActionsHistory { get; private set; }
        public List<PD_InfectionReport> InfectionReports { get; private set; }

        public List<PD_GameAction_Base> CurrentAvailablePlayerActions { get; private set; }
        public List<PD_MacroAction> CurrentAvailableMacros { get; private set; }

        #endregion

        #region constructors
        /// <summary>
        /// GAME FROM GAME SETUP
        /// </summary>
        //public PD_Game(
        //    PD_Specific_GameSetup_Definition gameSetup
        //    )
        //{

        //}

        /// <summary>
        /// NORMAL CONSTRUCTOR!
        /// </summary>
        /// <param name="gameDifficultyLevel"></param>
        /// <param name="players"></param>
        /// <param name="cities"></param>
        /// <param name="edges"></param>
        /// <param name="allCityCards"></param>
        /// <param name="allInfectionCards"></param>
        /// <param name="allEpidemicCards"></param>
        /// <param name="allPlayerPawns"></param>
        /// <param name="allResearchStations"></param>
        /// <param name="allInfectionCubes"></param>
        public PD_Game(
            int gameDifficultyLevel,
            List<PD_Player> players,
            List<PD_City> cities,
            List<PD_CityEdge> edges,

            List<PD_CityCard> allCityCards,
            List<PD_InfectionCard> allInfectionCards,
            List<PD_EpidemicCard> allEpidemicCards,

            List<PD_ME_PlayerPawn> allPlayerPawns,
            List<PD_Role_Card> allRoleCards,
            List<PD_ME_ResearchStation> allResearchStations,
            List<PD_ME_InfectionCube> allInfectionCubes
            )
        {
            CurrentAvailableMacros = new List<PD_MacroAction>();

            StartTime = DateTime.UtcNow;
            UniqueID = DateTime.UtcNow.Ticks;

            GameSettings = new PD_GameSettings(gameDifficultyLevel);

            GameStateCounter = new PD_GameStateCounter(
                players.Count,
                0,
                0
                );

            GameFSM = new PD_GameFSM(this);

            Players = players;

            UpdateAvailablePlayerActions();

            Map = new PD_Map(cities, edges);

            // game element references
            GameElementReferences = new PD_GameElementReferences(
                allCityCards,
                allInfectionCards,
                allEpidemicCards,

                allPlayerPawns,
                allRoleCards,

                allResearchStations,
                allInfectionCubes
                );

            MapElements = new PD_MapElements(Map.Cities);
            Cards = new PD_GameCards(Players, allRoleCards);

            PlayerPawnsPerPlayerID = new Dictionary<int, PD_ME_PlayerPawn>();
            foreach (var player in Players)
            {
                PlayerPawnsPerPlayerID.Add(player.ID, null);
            }

            RoleCardsPerPlayerID = new Dictionary<int, PD_Role_Card>();
            foreach (var player in players)
            {
                RoleCardsPerPlayerID.Add(player.ID, null);
            }

            PlayerActionsHistory = new List<PD_GameAction_Base>();
            InfectionReports = new List<PD_InfectionReport>();

        }

        [JsonConstructor]
        public PD_Game(
            long uniqueID,
            DateTime startTime,
            DateTime endTime,

            PD_GameSettings gameSettings,
            PD_GameFSM gameFSM,
            PD_GameStateCounter gameStateCounter,

            List<PD_Player> players,
            PD_Map map,

            PD_GameElementReferences gameElementReferences,
            PD_MapElements mapElements,
            PD_GameCards cards,

            Dictionary<int, PD_ME_PlayerPawn> playerPawnsPerPlayerID,
            Dictionary<int, PD_Role_Card> roleCardsPerPlayerID,

            List<PD_GameAction_Base> playerActionsHistory,
            List<PD_InfectionReport> infectionReports,

            List<PD_GameAction_Base> currentAvailablePlayerActions,
            List<PD_MacroAction> currentAvailableMacros
            )
        {
            UniqueID = uniqueID;
            StartTime = startTime;
            EndTime = endTime;

            GameSettings = gameSettings.GetCustomDeepCopy();
            GameFSM = gameFSM.GetCustomDeepCopy();
            GameStateCounter = gameStateCounter.GetCustomDeepCopy();

            Players = players.CustomDeepCopy();
            Map = map.GetCustomDeepCopy();

            GameElementReferences = gameElementReferences.GetCustomDeepCopy();
            MapElements = mapElements.GetCustomDeepCopy();
            Cards = cards.GetCustomDeepCopy();

            PlayerPawnsPerPlayerID = playerPawnsPerPlayerID.CustomDeepCopy();
            RoleCardsPerPlayerID = roleCardsPerPlayerID.CustomDeepCopy();

            PlayerActionsHistory = playerActionsHistory.CustomDeepCopy();
            InfectionReports = infectionReports.CustomDeepCopy();

            CurrentAvailablePlayerActions = currentAvailablePlayerActions.CustomDeepCopy();
            CurrentAvailableMacros = currentAvailableMacros.CustomDeepCopy();
        }

        /// <summary>
        /// private constructor, for deep copy purposes, only!
        /// </summary>
        /// <param name="gameToCopy"></param>
        private PD_Game(
            PD_Game gameToCopy
            )
        {
            UniqueID = gameToCopy.UniqueID;
            StartTime = gameToCopy.StartTime;
            EndTime = gameToCopy.EndTime;

            GameSettings = gameToCopy.GameSettings.GetCustomDeepCopy();
            GameFSM = gameToCopy.GameFSM.GetCustomDeepCopy();
            GameStateCounter = gameToCopy.GameStateCounter.GetCustomDeepCopy();

            Players = gameToCopy.Players.CustomDeepCopy();
            Map = gameToCopy.Map.GetCustomDeepCopy();

            GameElementReferences = gameToCopy.GameElementReferences.GetCustomDeepCopy();
            MapElements = gameToCopy.MapElements.GetCustomDeepCopy();
            Cards = gameToCopy.Cards.GetCustomDeepCopy();

            PlayerPawnsPerPlayerID = gameToCopy.PlayerPawnsPerPlayerID.CustomDeepCopy();
            RoleCardsPerPlayerID = gameToCopy.RoleCardsPerPlayerID.CustomDeepCopy();

            PlayerActionsHistory = gameToCopy.PlayerActionsHistory.CustomDeepCopy();
            InfectionReports = gameToCopy.InfectionReports.CustomDeepCopy();

            CurrentAvailablePlayerActions = gameToCopy.CurrentAvailablePlayerActions.CustomDeepCopy();
            CurrentAvailableMacros = gameToCopy.CurrentAvailableMacros.CustomDeepCopy();
        }

        #endregion

        #region command methods
        public void Com_SetupGame_Random(Random randomness_provider)
        {
            // 1. Set out board and pieces
            // 1.1. put research stations in research stations container
            MapElements.InactiveResearchStations.AddRange(
                new List<PD_ME_ResearchStation>(GameElementReferences.ResearchStations));

            // 1.2. separate the infection cubes by color (type) in their containers
            for (int i = 0; i < 4; i++)
            {
                List<PD_ME_InfectionCube> infectionCubesByType =
                    GameElementReferences.InfectionCubes.FindAll(
                        x =>
                        x.Type == i
                    );
                MapElements.InactiveInfectionCubesPerType.Add(i, infectionCubesByType);
            }

            // 1.3. place research station on atlanta
            PD_City atlanta = PD_Game_Queries.GQ_Find_CityByName(this, "Atlanta");
            PD_Game_Operators.GO_PlaceResearchStationOnCity(
                this, atlanta);

            // 2. Place outbreaks and cure markers
            // put outbreaks counter to position zero
            GameStateCounter.ResetOutbreaksCounter();

            // place cure markers vial side up
            GameStateCounter.InitializeCureMarkerStates();

            // 3. place infection marker and infect 9 cities
            // 3.1. place the infection marker (epidemics counter) on the lowest position
            GameStateCounter.ResetEpidemicsCounter();

            // 3.2. Infect the first cities - process
            // 3.2.1. put all infection cards in the divided deck of infection cards...
            Cards.DividedDeckOfInfectionCards.Add(new List<PD_InfectionCard>(GameElementReferences.InfectionCards));

            // 3.2.2 shuffle the infection cards deck...
            Cards.DividedDeckOfInfectionCards.ShuffleAllSubListsElements(randomness_provider);

            // 3.2.3 actually infect the cities.. 
            var firstPlayer = Players[0];
            for (int num_InfectionCubes_ToPlace = 3; num_InfectionCubes_ToPlace > 0; num_InfectionCubes_ToPlace--)
            {
                for (int city_Counter = 0; city_Counter < 3; city_Counter++)
                {
                    var infectionCard = Cards.DividedDeckOfInfectionCards.DrawLastElementOfLastSubList();

                    PD_City city = infectionCard.City;

                    PD_InfectionReport report = new PD_InfectionReport(
                        true,
                        firstPlayer,
                        city,
                        city.Type,
                        num_InfectionCubes_ToPlace
                        );

                    PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                        this,
                        city,
                        num_InfectionCubes_ToPlace,
                        report,
                        true
                        );

                    InfectionReports.Add(finalReport);

                    Cards.DeckOfDiscardedInfectionCards.Add(infectionCard);
                }
            }

            // 4. Give each player cards and a pawn
            // 4.1. Assign random roles and pawns to players
            MapElements.InactivePlayerPawns.AddRange(GameElementReferences.PlayerPawns);

            foreach (var player in Players)
            {
                var pawn = MapElements.InactivePlayerPawns.DrawOneRandom(randomness_provider);
                var roleCard = GameElementReferences.RoleCards.Find(
                    x =>
                    x.Role == pawn.Role
                    );
                Cards.InactiveRoleCards.Remove(roleCard);
                PlayerPawnsPerPlayerID[player.ID] = pawn;
                RoleCardsPerPlayerID[player.ID] = roleCard;
            }

            // 4.2. Deal cards to players: initial hands
            var playerCardsTempList = new List<PD_PlayerCardBase>();
            playerCardsTempList.AddRange(GameElementReferences.CityCards.Cast<PD_PlayerCardBase>().ToList());
            Cards.DividedDeckOfPlayerCards.Add(playerCardsTempList);
            Cards.DividedDeckOfPlayerCards.ShuffleAllSubListsElements(randomness_provider);

            int numPlayers = Players.Count;
            int numCardsToDealPerPlayer = GameSettings.GetNumberOfInitialCardsToDealPlayers(numPlayers);
            foreach (var player in Players)
            {
                for (int i = 0; i < numCardsToDealPerPlayer; i++)
                {
                    Cards.PlayerCardsPerPlayerID[player.ID].Add(Cards.DividedDeckOfPlayerCards.DrawLastElementOfLastSubList());
                }
            }

            // 5. Prepare the player deck
            // 5.1. get the necessary number of epidemic cards
            int numEpidemicCards = GameSettings.GetNumberOfEpidemicCardsToUseInGame();

            var epidemicCardsTempList = new List<PD_EpidemicCard>(GameElementReferences.EpidemicCards);
            while (epidemicCardsTempList.Count > numEpidemicCards)
            {
                epidemicCardsTempList.DrawOneRandom(randomness_provider);
            }

            // divide the player cards deck in as many sub decks as necessary
            var allPlayerCardsList = Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            //int numberOfPlayerCardsPerSubDeck = allPlayerCardsList.Count / numEpidemicCards;

            int numCards = allPlayerCardsList.Count;
            int numSubDecks = numEpidemicCards;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            // create the sub decks
            List<List<PD_PlayerCardBase>> temporaryDividedList = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numEpidemicCards; i++)
            {
                var subDeck = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    subDeck.Add(allPlayerCardsList.DrawOneRandom(randomness_provider));
                }
                temporaryDividedList.Add(subDeck);
            }
            // add the remaining cards
            for (int i = 0; i < remainingCardsNumber; i++)
            {
                int deckIndex = (temporaryDividedList.Count - 1) - i;
                temporaryDividedList[deckIndex].Add(allPlayerCardsList.DrawOneRandom(randomness_provider));
            }
            // insert the epidemic cards
            foreach (var subList in temporaryDividedList)
            {
                subList.Add(epidemicCardsTempList.DrawOneRandom(randomness_provider));
            }

            // shuffle all the sublists!
            temporaryDividedList.ShuffleAllSubListsElements(randomness_provider);

            // set the player cards deck as necessary
            Cards.DividedDeckOfPlayerCards.Clear();
            foreach (var sublist in temporaryDividedList)
            {
                Cards.DividedDeckOfPlayerCards.Add(sublist);
            }

            //// place all pawns on atlanta
            PD_Game_Operators.GO_PlaceAllPawnsOnAtlanta(this);

            UpdateAvailablePlayerActions();
        }

        public void Com_PA_Stay()
        {
            // do nothing! :)
        }

        public void Com_PMA_DriveFerry(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation
            )
        {
            PD_Game_Operators.GO_MovePawnFromCityToCity(
                this,
                PlayerPawnsPerPlayerID[player.ID],
                initialLocation,
                targetLocation
                );

            Medic_MoveTreat(targetLocation);
        }

        public void Com_PMA_DirectFlight(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation,
            PD_CityCard cityCardToDiscard
            )
        {
            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(
                this,
                player,
                cityCardToDiscard
                );

            PD_Game_Operators.GO_MovePawnFromCityToCity(
                this,
                PlayerPawnsPerPlayerID[player.ID],
                initialLocation,
                targetLocation
                );

            Medic_MoveTreat(targetLocation);
        }

        public void Com_PMA_CharterFlight(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation,
            PD_CityCard cityCardToDiscard
            )
        {
            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(
                this,
                player,
                cityCardToDiscard
                );

            PD_Game_Operators.GO_MovePawnFromCityToCity(
                this,
                PlayerPawnsPerPlayerID[player.ID],
                initialLocation,
                targetLocation
                );

            Medic_MoveTreat(targetLocation);
        }

        public void Com_PMA_ShuttleFlight(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation
            )
        {
            PD_Game_Operators.GO_MovePawnFromCityToCity(
                this,
                PlayerPawnsPerPlayerID[player.ID],
                initialLocation,
                targetLocation
                );

            Medic_MoveTreat(targetLocation);
        }

        public void Com_PMA_OperationsExpertFlight(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation,
            PD_CityCard cityCardToDiscard
            )
        {
            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(
                this,
                player,
                cityCardToDiscard
                );

            PD_Game_Operators.GO_MovePawnFromCityToCity(
                this,
                PlayerPawnsPerPlayerID[player.ID],
                initialLocation,
                targetLocation
                );

            Medic_MoveTreat(targetLocation);
        }

        private void Medic_MoveTreat(
            PD_City city
            )
        {
            // if player is a medic:
            if (PD_Game_Queries.GQ_Find_CurrentPlayer_Role(this) == PD_Player_Roles.Medic)
            {
                List<int> typesOfInfectionCubesOnTargetLocation = PD_Game_Queries.GQ_Find_InfectionCubeTypes_OnCity(
                    this,
                    city
                    );
                foreach (var type in typesOfInfectionCubesOnTargetLocation)
                {
                    // if this type has been cured:
                    if (PD_Game_Queries.GQ_Is_DiseaseCured_OR_Eradicated(this, type))
                    {
                        PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                            this,
                            city,
                            type
                            );
                        // create the supposed (auto) action
                        PD_PA_TreatDisease_Medic_Auto actionToStore = new PD_PA_TreatDisease_Medic_Auto(
                            PD_Game_Queries.GQ_Find_CurrentPlayer(this),
                            city,
                            type
                            );

                        // store the action in the game actions history
                        PlayerActionsHistory.Add(actionToStore);
                    }

                }
            }
        }

        public void Com_PA_BuildResearchStation(
            PD_Player player,
            PD_City cityToBuildResearchStationOn,
            PD_CityCard cityCardToDiscard
            )
        {
            if (MapElements.ResearchStationsPerCityID[cityToBuildResearchStationOn.ID].Count > 0)
            {
                throw new System.Exception("The chosen city already contains a research station");
            }
            if (MapElements.InactiveResearchStations.Count <= 0)
            {
                throw new System.Exception("There are not any inactive research stations... try move instead?");
            }

            // discard the corresponding card from players hand
            var cityCardsInPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInPlayerHand(this, player);

            if (cityCardToDiscard != null)
            {
                PD_Game_Operators.GO_PlayerDiscardsPlayerCard(
                    this,
                    player,
                    cityCardToDiscard
                    );
            }


            PD_Game_Operators.GO_PlaceResearchStationOnCity(
                this,
                cityToBuildResearchStationOn
                );

        }

        public void Com_PA_BuildResearchStation_OperationsExpert(
            PD_Player player,
            PD_City cityToBuildResearchStationOn
            )
        {
            if (MapElements.ResearchStationsPerCityID[cityToBuildResearchStationOn.ID].Count > 0)
            {
                throw new System.Exception("The chosen city already contains a research station");
            }
            if (MapElements.InactiveResearchStations.Count <= 0)
            {
                throw new System.Exception("There are not any inactive research stations... try move instead?");
            }

            PD_Game_Operators.GO_PlaceResearchStationOnCity(
                this,
                cityToBuildResearchStationOn
                );
        }

        public void Com_PA_MoveResearchStation(
            PD_Player player,
            PD_City cityToMoveResearchStation_From,
            PD_City cityToMoveResearchStation_To
            )
        {
            if (MapElements.ResearchStationsPerCityID[cityToMoveResearchStation_From.ID].Count <= 0)
            {
                throw new System.Exception("there is no research station in the FROM city");
            }
            if (MapElements.ResearchStationsPerCityID[cityToMoveResearchStation_To.ID].Count > 0)
            {
                throw new System.Exception("The TO city already contains a research station");
            }

            var cityCardsInPlayerHand = Cards.PlayerCardsPerPlayerID[player.ID].FindAll(
                x =>
                x.GetType() == typeof(PD_CityCard)
                ).Cast<PD_CityCard>().ToList();

            var cityCardToDiscard = cityCardsInPlayerHand.Find(
                x =>
                x.Name == cityToMoveResearchStation_To.Name
                );

            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(this, player, cityCardToDiscard);

            var researchStationToMove = MapElements.ResearchStationsPerCityID[cityToMoveResearchStation_From.ID].DrawLast();
            MapElements.ResearchStationsPerCityID[cityToMoveResearchStation_To.ID].Add(researchStationToMove);
        }

        public void Com_PA_TreatDisease(
            PD_Player player,
            PD_City city,
            int treat_Type
            )
        {
            if (
                PD_Game_Queries.GQ_Count_Num_InfectionCubes_OfType_OnCity(
                    this,
                    city,
                    treat_Type
                    ) == 0
                )
            {
                throw new System.Exception("City does not have any infection cubes of the requested type");
            }

            if (PD_Game_Queries.GQ_Find_CurrentPlayer_Role(this) == PD_Player_Roles.Medic)
            {
                throw new System.Exception(
                    "Medic cannot perform this type of action. Use special treat disease action, instead");
            }

            bool diseaseCured = PD_Game_Queries.GQ_Is_DiseaseCured_OR_Eradicated(this, treat_Type);

            if (diseaseCured)
            {
                // remove all cubes of this type
                PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                    this,
                    city,
                    treat_Type
                    );

                // check if disease is eradicated...
                var remainingCubesOfThisType = new List<PD_ME_InfectionCube>();
                foreach (var someCity in Map.Cities)
                {
                    var cubesOfThisTypeOnSomeCity = MapElements.InfectionCubesPerCityID[someCity.ID].FindAll(
                        x =>
                        x.Type == treat_Type
                        );
                    remainingCubesOfThisType.AddRange(cubesOfThisTypeOnSomeCity);
                }

                // if disease eradicated -> set marker to 2
                if (remainingCubesOfThisType.Count == 0)
                {
                    GameStateCounter.CureMarkersStates[treat_Type] = 2;
                }
            }
            else
            {
                // remove only one cube of this type
                PD_Game_Operators.GO_Remove_One_InfectionCube_OfType_FromCity(
                    this,
                    city,
                    treat_Type
                    );
            }

        }

        public void Com_PA_TreatDisease_Medic(
            PD_Player player,
            PD_City city,
            int treat_Type
            )
        {
            if (
                PD_Game_Queries.GQ_Count_Num_InfectionCubes_OfType_OnCity(
                    this,
                    city,
                    treat_Type
                    ) == 0
                )
            {
                throw new System.Exception("City does not have any infection cubes of the requested type");
            }

            if (PD_Game_Queries.GQ_Find_CurrentPlayer_Role(this) != PD_Player_Roles.Medic)
            {
                throw new System.Exception(
                    "Only Medic can perform this type of action.");
            }

            bool diseaseCured = PD_Game_Queries.GQ_Is_DiseaseCured_OR_Eradicated(this, treat_Type);

            if (diseaseCured)
            {
                // remove all cubes of this type
                PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                    this,
                    city,
                    treat_Type
                    );

                // check if disease is eradicated...
                var remainingCubesOfThisType = new List<PD_ME_InfectionCube>();
                foreach (var someCity in Map.Cities)
                {
                    var cubesOfThisTypeOnSomeCity = MapElements.InfectionCubesPerCityID[someCity.ID].FindAll(
                        x =>
                        x.Type == treat_Type
                        );
                    remainingCubesOfThisType.AddRange(cubesOfThisTypeOnSomeCity);
                }

                // if disease eradicated -> set marker to 2
                if (remainingCubesOfThisType.Count == 0)
                {
                    GameStateCounter.CureMarkersStates[treat_Type] = 2;
                }
            }
            else
            {
                // remove all cubes of this type
                PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                    this,
                    city,
                    treat_Type
                    );
            }

        }

        public void Com_PA_ShareKnowledge_GiveCard(
            PD_Player player_Gives_Card,
            PD_Player otherPlayer_TakesCard,
            PD_CityCard cityCardToGive
            )
        {
            var playerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, player_Gives_Card);
            var otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, otherPlayer_TakesCard);
            if (playerLocation != otherPlayerLocation)
            {
                throw new System.Exception("Players do not share location!");
            }

            Cards.PlayerCardsPerPlayerID[player_Gives_Card.ID].Remove(cityCardToGive);
            Cards.PlayerCardsPerPlayerID[otherPlayer_TakesCard.ID].Add(cityCardToGive);
        }

        public void Com_PA_ShareKnowledge_TakeCard(
            PD_Player player_TakesCard,
            PD_Player otherPlayer_GivesCard,
            PD_CityCard cityCardToTake
            )
        {
            var playerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, player_TakesCard);
            var otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, otherPlayer_GivesCard);
            if (playerLocation != otherPlayerLocation)
            {
                Console.WriteLine("player location: " + playerLocation.GetDescription());
                Console.WriteLine("other player location: " + otherPlayerLocation.GetDescription());
                throw new System.Exception("Players do not share location!");
            }

            Cards.PlayerCardsPerPlayerID[otherPlayer_GivesCard.ID].Remove(cityCardToTake);
            Cards.PlayerCardsPerPlayerID[player_TakesCard.ID].Add(cityCardToTake);
        }

        public void Com_PA_ShareKnowledge_GiveCard_ResearcherGives(
            PD_Player player_Researcher_GivesCard,
            PD_Player otherPlayer_TakesCard,
            PD_CityCard cityCardToGive
            )
        {
            var playerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, player_Researcher_GivesCard);
            var otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, otherPlayer_TakesCard);
            if (playerLocation != otherPlayerLocation)
            {
                Console.WriteLine("player location: " + playerLocation.GetDescription());
                Console.WriteLine("other player location: " + otherPlayerLocation.GetDescription());
                throw new System.Exception("Players do not share location!");
            }

            Cards.PlayerCardsPerPlayerID[player_Researcher_GivesCard.ID].Remove(cityCardToGive);
            Cards.PlayerCardsPerPlayerID[otherPlayer_TakesCard.ID].Add(cityCardToGive);
        }

        public void Com_PA_ShareKnowledge_TakeCard_FromResearcher(
            PD_Player player_TakesCard,
            PD_Player otherPlayer_Researcher_GivesCard,
            PD_CityCard cityCardToTake
            )
        {
            var playerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, player_TakesCard);
            var otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(this, otherPlayer_Researcher_GivesCard);
            if (playerLocation != otherPlayerLocation)
            {
                Console.WriteLine("player location: " + playerLocation.GetDescription());
                Console.WriteLine("other player location: " + otherPlayerLocation.GetDescription());
                throw new System.Exception("Players do not share location!");
            }

            Cards.PlayerCardsPerPlayerID[otherPlayer_Researcher_GivesCard.ID].Remove(cityCardToTake);
            Cards.PlayerCardsPerPlayerID[player_TakesCard.ID].Add(cityCardToTake);
        }

        public void Com_PA_DiscoverCure(
            PD_Player player,
            List<PD_CityCard> cityCardsToDiscard,
            int typeOfDiseaseToCure
            )
        {
            bool player_Is_Scientist = PD_Game_Queries.GQ_Find_CurrentPlayer_Role(this) == PD_Player_Roles.Scientist;

            if (player_Is_Scientist)
            {
                throw new System.Exception("Scientist cannot apply this kind of action");
            }

            if (cityCardsToDiscard.Count != 5)
            {
                throw new System.Exception("number of city cards to discard is not 5!");
            }

            foreach (var cityCard in cityCardsToDiscard)
            {
                if (cityCard.Type != typeOfDiseaseToCure)
                {
                    throw new System.Exception("The city card type does not match the cure type");
                }
            }

            // discard the cards
            foreach (var cityCard in cityCardsToDiscard)
            {
                PD_Game_Operators.GO_PlayerDiscardsPlayerCard(
                    this,
                    player,
                    cityCard
                    );
            }

            // discover the cure for the disease here...
            GameStateCounter.CureDisease(typeOfDiseaseToCure);

            Medic_AutoTreat_AfterDiscoverCure(typeOfDiseaseToCure);
        }

        public void Com_PA_DiscoverCure_Scientist(
            PD_Player player,
            List<PD_CityCard> cityCardsToDiscard,
            int typeOfDiseaseToCure
            )
        {
            bool player_Is_Scientist = PD_Game_Queries.GQ_Find_CurrentPlayer_Role(this) == PD_Player_Roles.Scientist;

            if (player_Is_Scientist == false)
            {
                throw new System.Exception("Only Scientist can apply this kind of action");
            }

            if (cityCardsToDiscard.Count != 4)
            {
                throw new System.Exception("number of city cards to discard needs to be 4");
            }
            foreach (var cityCard in cityCardsToDiscard)
            {
                if (cityCard.Type != typeOfDiseaseToCure)
                {
                    throw new System.Exception("The city card type does not match the cure type");
                }
            }

            // discard the cards
            foreach (var cityCard in cityCardsToDiscard)
            {
                PD_Game_Operators.GO_PlayerDiscardsPlayerCard(
                    this,
                    player,
                    cityCard
                    );
            }

            // discover the cure for the disease here...
            GameStateCounter.CureDisease(typeOfDiseaseToCure);

            Medic_AutoTreat_AfterDiscoverCure(typeOfDiseaseToCure);
        }

        public void Medic_AutoTreat_AfterDiscoverCure(int curedDiseaaseType)
        {
            PD_City medicLocation = PD_Game_Queries.GQ_Find_Medic_Location(this);
            if (medicLocation != null)
            {
                List<int> infectionCubeTypes_OnMedicLocation =
                    PD_Game_Queries.GQ_Find_InfectionCubeTypes_OnCity(this, medicLocation);

                if (infectionCubeTypes_OnMedicLocation.Contains(curedDiseaaseType))
                {
                    // remove all cubes of this type from medic's location
                    PD_Game_Operators.GO_Remove_All_InfectionCubes_OfType_FromCity(
                        this,
                        medicLocation,
                        curedDiseaaseType
                        );
                }
            }
        }

        public void Com_DrawNewPlayerCards(
            PD_Player player
            )
        {
            // draw new player cards...
            var newPlayerCards = new List<PD_PlayerCardBase>();

            for (int i = 0; i < 2; i++)
            {
                newPlayerCards.Add(Cards.DividedDeckOfPlayerCards.DrawLastElementOfLastSubList());
            }

            Cards.PlayerCardsPerPlayerID[player.ID].AddRange(newPlayerCards);
        }

        public void Com_ApplyEpidemicCard(
            Random randomness_provider,
            PD_Player player
            )
        {
            // 0. discard the epidemic card...
            var allCards_InPlayerHand = Cards.PlayerCardsPerPlayerID[player.ID];
            var epidemicCard = allCards_InPlayerHand.Find(
                x =>
                x.GetType() == typeof(PD_EpidemicCard)
                );
            Cards.PlayerCardsPerPlayerID[player.ID].Remove(epidemicCard);
            Cards.DeckOfDiscardedPlayerCards.Add(epidemicCard);

            // 1. move the infection rate marker by one position
            GameStateCounter.IncreaseEpidemicsCounter();

            // 2. Infect: Draw the bottom card from the Infection Deck. 
            // Unless its disease color has been eradicated, put 3 disease cubes of that color on the named city.
            // If the city already has cubes of this color, do not add 3 cubes to it.
            // Instead, add just enough cubes so that it has 3 cubes of this color and then an outbreak of this disease occurs in the city(see Outbreaks below). 
            // Discard this card to the Infection Discard Pile.
            var cardFromBottom = Cards.DividedDeckOfInfectionCards.DrawFirstElementOfFirstSubList();

            int epidemicInfectionType = cardFromBottom.Type;
            bool diseaseTypeEradicated = PD_Game_Queries.GQ_Is_Disease_Eradicated(this, epidemicInfectionType);

            if (diseaseTypeEradicated == false)
            {
                // actually apply the epidemic infection here...

                PD_InfectionReport initialReport = new PD_InfectionReport(
                    false, // not game setup...
                    player,
                    cardFromBottom.City,
                    cardFromBottom.Type,
                    3
                    );

                // apply infection of this city here
                PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                    this,
                    cardFromBottom.City,
                    3,
                    initialReport,
                    false
                    );

                InfectionReports.Add(finalReport);

                if (finalReport.FailureReason == InfectionFailureReasons.notEnoughDiseaseCubes)
                {
                    GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection = true;
                }
            }

            // put card in discarded infection cards pile
            Cards.DeckOfDiscardedInfectionCards.Add(cardFromBottom);

            // 3. intensify: Reshuffle just the cards in the Infection Discard Pile 
            // and place them on top of the Infection Deck.
            Cards.DeckOfDiscardedInfectionCards.Shuffle(randomness_provider);
            Cards.DividedDeckOfInfectionCards.Add(
                Cards.DeckOfDiscardedInfectionCards.DrawAll()
                );
        }

        public void Com_Discard_AfterDrwing(
            PD_Player player,
            PD_PlayerCardBase playerCardToDiscard
            )
        {
            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(this, player, playerCardToDiscard);
        }

        public void Com_Discard_DuringMainPlayerActions(
            PD_Player player,
            PD_PlayerCardBase playerCardToDiscard
            )
        {
            PD_Game_Operators.GO_PlayerDiscardsPlayerCard(this, player, playerCardToDiscard);
        }

        public void Com_DrawNewInfectionCards(PD_Player player)
        {
            int numberOfInfectionCardsToDraw =
                GameSettings.InfectionRatesPerEpidemicsCounter[GameStateCounter.EpidemicsCounter];

            var infectionCards = new List<PD_InfectionCard>();

            for (int i = 0; i < numberOfInfectionCardsToDraw; i++)
            {
                infectionCards.Add(Cards.DividedDeckOfInfectionCards.DrawLastElementOfLastSubList());
            }

            Cards.ActiveInfectionCards.AddRange(infectionCards);

        }

        public void Com_ApplyInfectionCard(PD_Player player, PD_InfectionCard infectionCard)
        {
            int infectionType = infectionCard.Type;
            bool diseaseEradicated = PD_Game_Queries.GQ_Is_Disease_Eradicated(this, infectionType);

            if (diseaseEradicated == false)
            {
                PD_InfectionReport initialReport = new PD_InfectionReport(
                    false, // not game setup
                    player,
                    infectionCard.City,
                    infectionCard.Type,
                    1
                    );

                PD_InfectionReport finalReport = PD_Game_Operators.GO_InfectCity(
                    this,
                    infectionCard.City,
                    1,
                    initialReport,
                    false
                    );

                InfectionReports.Add(finalReport);

                if (finalReport.FailureReason == InfectionFailureReasons.notEnoughDiseaseCubes)
                {
                    GameStateCounter.NotEnoughDiseaseCubesToCompleteAnInfection = true;
                }
            }

            // remove the infection card from the active infection cards pile
            Cards.ActiveInfectionCards.Remove(infectionCard);
            Cards.DeckOfDiscardedInfectionCards.Add(infectionCard);

        }
        #endregion

        public void UpdateAvailablePlayerActions()
        {
            CurrentAvailablePlayerActions = PD_Game_Action_Queries.GAQ_Find_AvailablePlayerActions(this);
        }

        public void ApplySpecificPlayerAction(
            Random randomness_provider,
            PD_GameAction_Base playerAction
            )
        {
            CurrentAvailableMacros = new List<PD_MacroAction>();

            if (CurrentAvailablePlayerActions.Contains(playerAction) == false)
            {
                throw new System.Exception("non applicable action");
            }

            PlayerActionsHistory.Add(playerAction);

            GameFSM.OnCommand(randomness_provider,this, playerAction);

            UpdateAvailablePlayerActions();

            // after an action is applied, see if the next action is auto action
            // and if so, apply it automatically.
            if (
                CurrentAvailablePlayerActions != null
                && CurrentAvailablePlayerActions.Count > 0
                )
            {
                if (
                    CurrentAvailablePlayerActions[0]
                    .GetType()
                    .IsSubclassOf(typeof(PD_AutoAction_Base))
                    )
                {
                    ApplySpecificPlayerAction(
                        randomness_provider,
                        CurrentAvailablePlayerActions[0]
                        );
                }
            }
        }

        public List<PD_MacroAction> GetAvailableMacros(PD_AI_PathFinder pathFinder)
        {
            if (
                CurrentAvailableMacros == null
                ||
                CurrentAvailableMacros.Count == 0
                )
            {
                var researchStationCities = PD_Game_Queries.GQ_Find_ResearchStationCities(this);
                CurrentAvailableMacros = PD_MacroActionsSynthesisSystem.FindAll_Macros(
                    this,
                    pathFinder,
                    researchStationCities
                    );
                return new List<PD_MacroAction>(CurrentAvailableMacros);
            }

            return new List<PD_MacroAction>(CurrentAvailableMacros);
        }

        public void ApplySpecificMacro(
            Random randomness_provider,
            PD_MacroAction macro
            )
        {
            if (GameFSM.CurrentState.GetType() == typeof(PD_GS_ApplyingMainPlayerActions))
            {
                if (
                    macro.MacroAction_Type == PD_MacroAction_Type.Walk_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.Stay_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro

                    || macro.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Macro
                    || macro.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Scientist_Macro
                    )
                {
                    var executablePart = new List<PD_GameAction_Base>();
                    int numRemainingActionsThisRound = 4 - GameStateCounter.CurrentPlayerActionIndex;
                    for (int i = 0; i < numRemainingActionsThisRound; i++)
                    {
                        if (i < macro.Count_Total_Length())
                        {
                            executablePart.Add(macro.Actions_All[i].GetCustomDeepCopy());
                        }
                    }

                    foreach (var command in executablePart)
                    {
                        if (CurrentAvailablePlayerActions.Contains(command))
                        {
                            ApplySpecificPlayerAction(
                                randomness_provider,
                                command
                                );
                        }
                        else
                        {
                            Console.WriteLine(macro.GetDescription());
                            Console.WriteLine("problem: " + command.GetDescription());
                            throw new System.Exception("command cannot be applied!");
                        }
                    }
                }
                else
                {
                    throw new System.Exception("wrong macro type");
                }
            }
            else if (PD_Game_Queries.GQ_IsInState_DiscardAfterDrawing(this))
            {
                PD_MacroAction_Type mat = macro.MacroAction_Type;
                if (mat == PD_MacroAction_Type.Discard_AfterDrawing_Macro)
                {
                    var action = macro.Actions_All[0];
                    if (CurrentAvailablePlayerActions.Contains(action))
                    {
                        //Console.WriteLine("applying command: " + action.GetDescription());
                        ApplySpecificPlayerAction(
                            randomness_provider,
                            action
                            );
                    }
                    else
                    {
                        throw new System.Exception("command cannot be applied!");
                    }
                }
                else
                {
                    throw new System.Exception("wrong macro type");
                }
            }
            else if (PD_Game_Queries.GQ_IsInState_DiscardDuringMainPlayerActions(this))
            {
                PD_MacroAction_Type mat = macro.MacroAction_Type;
                if (mat == PD_MacroAction_Type.Discard_DuringMainPlayerActions_Macro)
                {
                    var action = macro.Actions_All[0];
                    if (CurrentAvailablePlayerActions.Contains(action))
                    {
                        //Console.WriteLine("applying command: " + action.GetDescription());
                        ApplySpecificPlayerAction(
                            randomness_provider,
                            action
                            );
                    }
                    else
                    {
                        throw new System.Exception("command cannot be applied!");
                    }
                }
                else
                {
                    throw new System.Exception("wrong macro type");
                }
            }
        }

        public void OverrideStartTime()
        {
            StartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// This method is called when entering the GameLost or the GameWon states
        /// </summary>
        public void OverrideEndTime()
        {
            EndTime = DateTime.UtcNow;
        }

        public PD_Game Request_Fair_ForwardModel(Random randomness_provider)
        {
            PD_Game gameCopy = this.GetCustomDeepCopy();
            PD_Game_Operators.GO_RandomizeGame(
                randomness_provider,
                gameCopy
                );
            return gameCopy;
        }

        public PD_Game Request_Unfair_ForwardModel()
        {
            PD_Game gameCopy = this.GetCustomDeepCopy();
            return gameCopy;
        }



        public PD_Game GetCustomDeepCopy()
        {
            return new PD_Game(this);
        }



        #region equality_override

        #endregion
    }
}
