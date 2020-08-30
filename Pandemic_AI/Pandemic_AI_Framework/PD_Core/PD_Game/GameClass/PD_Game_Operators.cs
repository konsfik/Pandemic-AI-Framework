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
            GO_Randomize_PlayerCards_Deck(
                randomness_provider,
                game
                );
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
            Random randomness_provider,
            PD_Game game
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
            PD_Game game
            )
        {
            PD_City atlanta = PD_Game_Queries.GQ_Find_CityByName(game, "Atlanta");
            foreach (var player in game.Players)
            {
                game.MapElements.PlayerPawnsPerCityID[atlanta.ID].Add(
                    PD_Game_Queries.GQ_Find_PlayerPawn(game, player)
                    );
            }
        }

        public static void GO_Randomize_PlayerRoles(
            Random randomness_provider,
            PD_Game game
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

        #region ROBUSTNESS operators

        public static void GO_ROBUSTNESS_R1_4P_Easy_RandRoles(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );
        }

        public static void GO_ROBUSTNESS_R2_4P_Easy_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_Randomize_PlayerCards_Deck(
                randomness_provider,
                game
                );

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R3_4P_Easy_RandDecks(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);
            GO_Randomize_PlayerCards_Deck(
                randomness_provider,
                game
                );

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }


        public static void GO_ROBUSTNESS_R4_4P_Easy_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );
            GO_Randomize_PlayerCards_Deck(
                randomness_provider,
                game
                );

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R5_4P_Easy_RandRoles_RandDecks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_Randomize_PlayerRoles(randomness_provider, game);
            GO_Randomize_PlayerCards_Deck(randomness_provider, game);
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R6_3P_Easy_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            // randomize the player order, to begin with...
            GO_Randomize_PlayerRoles(randomness_provider, game);

            // then, convert the game to a three player game!
            // and in the end, randomize the decks.

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));
            var epidemicCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_EpidemicCard));

            cityCards.Shuffle(randomness_provider);

            // remove the last player

            // remove the cards reference of player 4
            game.Cards.PlayerCardsPerPlayerID.Remove(player_4_id);
            // ref player 4 pawn
            var player4_pawn_ref = game.PlayerPawnsPerPlayerID[player_4_id];
            // add the pawn to inactive pawns
            game.MapElements.InactivePlayerPawns.Add(player4_pawn_ref);
            // remove the pawn from dictionary of player pawns
            game.PlayerPawnsPerPlayerID.Remove(player_4_id);
            // remove the pawn from the map (from Atlanta)
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player4_pawn_ref);
            // add player 4 role card to unused role cards
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_4_id]);
            // remove player 4 role- reference
            game.RoleCardsPerPlayerID.Remove(player_4_id);

            // actually remove player 4!
            var player4 = game.Players.Find(x => x.ID == player_4_id);
            game.Players.Remove(player4);



            // set number of players to three
            game.GameStateCounter.Override_NumberOfPlayers(3);

            // deal the cards, once more!

            // give three cards to each player
            foreach (var player in game.Players)
            {
                for (int i = 0; i < 3; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in four parts
            int numCards = cityCards.Count;
            int numSubDecks = 4;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R7_3P_Easy_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_ROBUSTNESS_R6_3P_Easy_RandRoles_Rand_P_Deck(
                randomness_provider,
                game
                );
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R8_2P_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            // randomize the player order, to begin with...
            GO_ROBUSTNESS_R1_4P_Easy_RandRoles(
                randomness_provider,
                game
                );

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));
            var epidemicCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_EpidemicCard));

            cityCards.Shuffle(randomness_provider);

            // remove the last players

            // remove the cards reference of player 4 and player 3
            game.Cards.PlayerCardsPerPlayerID.Remove(player_4_id);
            game.Cards.PlayerCardsPerPlayerID.Remove(player_3_id);
            // ref player 4 and 3 pawns
            var player4_pawn_ref = game.PlayerPawnsPerPlayerID[player_4_id];
            var player3_pawn_ref = game.PlayerPawnsPerPlayerID[player_3_id];
            // add the pawns to inactive pawns
            game.MapElements.InactivePlayerPawns.Add(player4_pawn_ref);
            game.MapElements.InactivePlayerPawns.Add(player3_pawn_ref);
            // remove the pawn from dictionary of player pawns
            game.PlayerPawnsPerPlayerID.Remove(player_4_id);
            game.PlayerPawnsPerPlayerID.Remove(player_3_id);
            // remove the pawn from the map (from Atlanta)
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player4_pawn_ref);
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player3_pawn_ref);
            // add player 4 role card to unused role cards
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_4_id]);
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_3_id]);
            // remove player 4 role- reference
            game.RoleCardsPerPlayerID.Remove(player_4_id);
            game.RoleCardsPerPlayerID.Remove(player_3_id);

            // actually remove player 4!
            var player4 = game.Players.Find(x => x.ID == player_4_id);
            game.Players.Remove(player4);
            var player3 = game.Players.Find(x => x.ID == player_3_id);
            game.Players.Remove(player3);



            // set number of players to two
            game.GameStateCounter.Override_NumberOfPlayers(2);

            // deal the cards, once more!

            // give four cards to each player
            foreach (var player in game.Players)
            {
                for (int i = 0; i < 4; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in four parts
            int numCards = cityCards.Count;
            int numSubDecks = 4;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R9_2P_Easy_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_ROBUSTNESS_R6_3P_Easy_RandRoles_Rand_P_Deck(
                randomness_provider,
                game
                );
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }


        public static void GO_ROBUSTNESS_R10_4P_Medium_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            int numEpidemics = 5; // (medium game)

            // randomize the player order, to begin with...
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));

            //find five epidemic cards
            var temp_epidemicCards = game.GameElementReferences.EpidemicCards.CustomDeepCopy();
            List<PD_EpidemicCard> epidemicCards = new List<PD_EpidemicCard>();
            for (int i = 0; i < numEpidemics; i++)
            {
                epidemicCards.Add(temp_epidemicCards.DrawOneRandom(randomness_provider));
            }

            cityCards.Shuffle(randomness_provider);

            // deal the cards, once more!

            // give two cards to each player
            foreach (var player in game.Players)
            {
                for (int i = 0; i < 2; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in five parts
            int numCards = cityCards.Count;
            int numSubDecks = numEpidemics;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R11_4P_Medium_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_ROBUSTNESS_R10_4P_Medium_RandRoles_Rand_P_Deck(
                randomness_provider,
                game
                );
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R12_3P_Medium_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            // randomize the player order, to begin with...
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );

            // then, convert the game to a three player game!
            // and in the end, randomize the decks.

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));

            //find five epidemic cards
            var temp_epidemicCards = game.GameElementReferences.EpidemicCards.CustomDeepCopy();
            List<PD_EpidemicCard> epidemicCards = new List<PD_EpidemicCard>();
            for (int i = 0; i < 5; i++)
            {
                epidemicCards.Add(temp_epidemicCards.DrawOneRandom(randomness_provider));
            }

            cityCards.Shuffle(randomness_provider);

            // remove the last player

            // remove the cards reference of player 4
            game.Cards.PlayerCardsPerPlayerID.Remove(player_4_id);
            // ref player 4 pawn
            var player4_pawn_ref = game.PlayerPawnsPerPlayerID[player_4_id];
            // add the pawn to inactive pawns
            game.MapElements.InactivePlayerPawns.Add(player4_pawn_ref);
            // remove the pawn from dictionary of player pawns
            game.PlayerPawnsPerPlayerID.Remove(player_4_id);
            // remove the pawn from the map (from Atlanta)
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player4_pawn_ref);
            // add player 4 role card to unused role cards
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_4_id]);
            // remove player 4 role- reference
            game.RoleCardsPerPlayerID.Remove(player_4_id);

            // actually remove player 4!
            var player4 = game.Players.Find(x => x.ID == player_4_id);
            game.Players.Remove(player4);



            // set number of players to three
            game.GameStateCounter.Override_NumberOfPlayers(3);

            // deal the cards, once more!

            // give three cards to each player
            foreach (var player in game.Players)
            {
                for (int i = 0; i < 3; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in four parts
            int numCards = cityCards.Count;
            int numSubDecks = 5;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R13_3P_Medium_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_ROBUSTNESS_R12_3P_Medium_RandRoles_Rand_P_Deck(
                randomness_provider,
                game
                );
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R14_2P_Medium_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            // randomize the player order, to begin with...
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );

            // then, convert the game to a three player game!
            // and in the end, randomize the decks.

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));

            //find five epidemic cards
            var temp_epidemicCards = game.GameElementReferences.EpidemicCards.CustomDeepCopy();
            List<PD_EpidemicCard> epidemicCards = new List<PD_EpidemicCard>();
            for (int i = 0; i < 5; i++)
            {
                epidemicCards.Add(temp_epidemicCards.DrawOneRandom(randomness_provider));
            }

            cityCards.Shuffle(randomness_provider);

            // remove the last player

            // remove the cards reference of player 4 & 3
            game.Cards.PlayerCardsPerPlayerID.Remove(player_4_id);
            game.Cards.PlayerCardsPerPlayerID.Remove(player_3_id);
            // ref player 4 pawn
            var player4_pawn_ref = game.PlayerPawnsPerPlayerID[player_4_id];
            var player3_pawn_ref = game.PlayerPawnsPerPlayerID[player_3_id];
            // add the pawn to inactive pawns
            game.MapElements.InactivePlayerPawns.Add(player4_pawn_ref);
            game.MapElements.InactivePlayerPawns.Add(player3_pawn_ref);
            // remove the pawn from dictionary of player pawns
            game.PlayerPawnsPerPlayerID.Remove(player_4_id);
            game.PlayerPawnsPerPlayerID.Remove(player_3_id);
            // remove the pawn from the map (from Atlanta)
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player4_pawn_ref);
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player3_pawn_ref);
            // add player 4 role card to unused role cards
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_4_id]);
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_3_id]);
            // remove player 4 role- reference
            game.RoleCardsPerPlayerID.Remove(player_4_id);
            game.RoleCardsPerPlayerID.Remove(player_3_id);

            // actually remove player 4!
            var player4 = game.Players.Find(x => x.ID == player_4_id);
            game.Players.Remove(player4);
            var player3 = game.Players.Find(x => x.ID == player_3_id);
            game.Players.Remove(player3);



            // set number of players to two!
            game.GameStateCounter.Override_NumberOfPlayers(2);

            // deal the cards, once more!

            // give four cards to each player
            foreach (var player in game.Players)
            {
                for (int i = 0; i < 4; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in four parts
            int numCards = cityCards.Count;
            int numSubDecks = 5;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R15_2P_Medium_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_ROBUSTNESS_R14_2P_Medium_RandRoles_Rand_P_Deck(
                randomness_provider,
                game
                );
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }


        public static void GO_ROBUSTNESS_R16_4P_Hard_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            int numEpidemics = 6; // (hard game)

            // randomize the player order, to begin with...
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));

            //find five epidemic cards
            var temp_epidemicCards = game.GameElementReferences.EpidemicCards.CustomDeepCopy();
            List<PD_EpidemicCard> epidemicCards = new List<PD_EpidemicCard>();
            for (int i = 0; i < numEpidemics; i++)
            {
                epidemicCards.Add(temp_epidemicCards.DrawOneRandom(randomness_provider));
            }

            cityCards.Shuffle(randomness_provider);

            // deal the cards, once more!

            // give two cards to each player
            foreach (var player in game.Players)
            {
                for (int i = 0; i < 2; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in five parts
            int numCards = cityCards.Count;
            int numSubDecks = numEpidemics;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R17_4P_Hard_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_ROBUSTNESS_R16_4P_Hard_RandRoles_Rand_P_Deck(
                randomness_provider,
                game
                );
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R18_3P_Hard_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            int numEpidemics = 6; // (hard game)
            int numPlayers = 3;
            int numCardsToDeal = 3;

            // randomize the player order, to begin with...
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );

            // then, convert the game to a three player game!
            // and in the end, randomize the decks.

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));

            //find five epidemic cards
            var temp_epidemicCards = game.GameElementReferences.EpidemicCards.CustomDeepCopy();
            List<PD_EpidemicCard> epidemicCards = new List<PD_EpidemicCard>();
            for (int i = 0; i < numEpidemics; i++)
            {
                epidemicCards.Add(temp_epidemicCards.DrawOneRandom(randomness_provider));
            }

            cityCards.Shuffle(randomness_provider);

            // remove the last player

            // remove the cards reference of player 4
            game.Cards.PlayerCardsPerPlayerID.Remove(player_4_id);
            // ref player 4 pawn
            var player4_pawn_ref = game.PlayerPawnsPerPlayerID[player_4_id];
            // add the pawn to inactive pawns
            game.MapElements.InactivePlayerPawns.Add(player4_pawn_ref);
            // remove the pawn from dictionary of player pawns
            game.PlayerPawnsPerPlayerID.Remove(player_4_id);
            // remove the pawn from the map (from Atlanta)
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player4_pawn_ref);
            // add player 4 role card to unused role cards
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_4_id]);
            // remove player 4 role- reference
            game.RoleCardsPerPlayerID.Remove(player_4_id);

            // actually remove player 4!
            var player4 = game.Players.Find(x => x.ID == player_4_id);
            game.Players.Remove(player4);



            // set number of players to three
            game.GameStateCounter.Override_NumberOfPlayers(numPlayers);

            // deal the cards, once more!

            // give three cards to each player (three player game)
            foreach (var player in game.Players)
            {
                for (int i = 0; i < numCardsToDeal; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in four parts
            int numCards = cityCards.Count;
            int numSubDecks = numEpidemics;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R19_3P_Hard_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game)
        {
            GO_ROBUSTNESS_R18_3P_Hard_RandRoles_Rand_P_Deck(
                randomness_provider,
                game
                );
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R20_2P_Hard_RandRoles_Rand_P_Deck(
            Random randomness_provider,
            PD_Game game
            )
        {
            int numEpidemics = 6; // (hard game)
            int numPlayers = 2;
            int numCardsToDeal = 4;

            // randomize the player order, to begin with...
            GO_Randomize_PlayerRoles(
                randomness_provider,
                game
                );

            // then, convert the game to a three player game!
            // and in the end, randomize the decks.

            // player ids
            int player_1_id = game.Players[0].ID;
            int player_2_id = game.Players[1].ID;
            int player_3_id = game.Players[2].ID;
            int player_4_id = game.Players[3].ID;

            var allPlayerCards = game.Cards.DividedDeckOfPlayerCards.DrawAllElementsOfAllSubListsAsOneList();
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_1_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_2_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_3_id].DrawAll()
                );
            allPlayerCards.AddRange(
                game.Cards.PlayerCardsPerPlayerID[player_4_id].DrawAll()
                );

            var cityCards = allPlayerCards.FindAll(x => x.GetType() == typeof(PD_CityCard));

            //find five epidemic cards
            var temp_epidemicCards = game.GameElementReferences.EpidemicCards.CustomDeepCopy();
            List<PD_EpidemicCard> epidemicCards = new List<PD_EpidemicCard>();
            for (int i = 0; i < numEpidemics; i++)
            {
                epidemicCards.Add(temp_epidemicCards.DrawOneRandom(randomness_provider));
            }

            cityCards.Shuffle(randomness_provider);

            // remove the last player

            // remove the cards reference of player 4 & 3
            game.Cards.PlayerCardsPerPlayerID.Remove(player_4_id);
            game.Cards.PlayerCardsPerPlayerID.Remove(player_3_id);
            // ref player 4 pawn
            var player4_pawn_ref = game.PlayerPawnsPerPlayerID[player_4_id];
            var player3_pawn_ref = game.PlayerPawnsPerPlayerID[player_3_id];
            // add the pawn to inactive pawns
            game.MapElements.InactivePlayerPawns.Add(player4_pawn_ref);
            game.MapElements.InactivePlayerPawns.Add(player3_pawn_ref);
            // remove the pawn from dictionary of player pawns
            game.PlayerPawnsPerPlayerID.Remove(player_4_id);
            game.PlayerPawnsPerPlayerID.Remove(player_3_id);
            // remove the pawn from the map (from Atlanta)
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player4_pawn_ref);
            game.MapElements.PlayerPawnsPerCityID[game.Map.Cities.Find(x => x.Name == "Atlanta").ID].Remove(player3_pawn_ref);
            // add player 4 role card to unused role cards
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_4_id]);
            game.Cards.InactiveRoleCards.Add(game.RoleCardsPerPlayerID[player_3_id]);
            // remove player 4 role- reference
            game.RoleCardsPerPlayerID.Remove(player_4_id);
            game.RoleCardsPerPlayerID.Remove(player_3_id);

            // actually remove player 4!
            var player4 = game.Players.Find(x => x.ID == player_4_id);
            game.Players.Remove(player4);
            var player3 = game.Players.Find(x => x.ID == player_3_id);
            game.Players.Remove(player3);



            // set number of players to two!
            game.GameStateCounter.Override_NumberOfPlayers(numPlayers);

            // deal the cards, once more!

            // give four cards to each player
            foreach (var player in game.Players)
            {
                for (int i = 0; i < numCardsToDeal; i++)
                {
                    game.Cards.PlayerCardsPerPlayerID[player.ID].Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
            }

            // split the remaining deck in four parts
            int numCards = cityCards.Count;
            int numSubDecks = numEpidemics;
            int numCardsPerSubDeck = numCards / numSubDecks;
            int remainingCardsNumber = numCards % numSubDecks;

            List<List<PD_PlayerCardBase>> temporaryDeck = new List<List<PD_PlayerCardBase>>();
            for (int i = 0; i < numSubDecks; i++)
            {
                List<PD_PlayerCardBase> tempList = new List<PD_PlayerCardBase>();
                for (int j = 0; j < numCardsPerSubDeck; j++)
                {
                    tempList.Add(
                        cityCards.DrawOneRandom(randomness_provider)
                        );
                }
                temporaryDeck.Add(tempList);
            }

            for (int i = 0; i < remainingCardsNumber; i++)
            {
                temporaryDeck[i].Add(cityCards.DrawOneRandom(randomness_provider));
            }

            // add the epidemic cards in the sub decks
            foreach (var subDeck in temporaryDeck)
            {
                subDeck.Add(epidemicCards.DrawOneRandom(randomness_provider));
                subDeck.Shuffle(randomness_provider);
            }

            // apply the deck to the game!
            foreach (var subDeck in temporaryDeck)
            {
                game.Cards.DividedDeckOfPlayerCards.Add(subDeck);
            }



            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        public static void GO_ROBUSTNESS_R21_2P_Hard_RandRoles_Rand_Decks(
            Random randomness_provider,
            PD_Game game
            )
        {
            GO_ROBUSTNESS_R20_2P_Hard_RandRoles_Rand_P_Deck(
                randomness_provider,
                game);
            game.GO_Randomize_InfectionCards_Deck(randomness_provider);

            // clear macros and actions lists, after the transformation is over...
            if (game.CurrentAvailableMacros != null)
            {
                game.CurrentAvailableMacros.Clear();
            }
            game.UpdateAvailablePlayerActions();
        }

        #endregion
        /// <summary>
        /// The act of infecting a city.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="cityToInfect"></param>
        /// <param name="num_CubesToPlace"></param>
        /// <param name="existingReport"></param>
        /// <returns></returns>
        public static PD_InfectionReport GO_InfectCity(
            PD_Game game,
            PD_City cityToInfect,
            int num_CubesToPlace,
            PD_InfectionReport existingReport,
            bool infectionDuringGameSetup
            )
        {
            int currentInfectionType = existingReport.InfectionType;

            if (infectionDuringGameSetup == false)
            {

                PD_City medic_location = PD_Game_Queries.GQ_Find_Medic_Location(game);

                bool currentDiseaseIsCured = PD_Game_Queries.GQ_Is_DiseaseCured_OR_Eradicated(game, currentInfectionType);

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
                PD_Game_Queries.GQ_Count_Num_InfectionCubes_OfType_OnCity(
                    game,
                    cityToInfect,
                    currentInfectionType
                    );

            int num_CubesOfType_ThatCanBePlacedOnCity =
                3 - num_CubesOfType_AlreadyOnCity;

            int num_InactiveInfectionCubes_OfType =
                PD_Game_Queries.Num_InactiveInfectionCubes_OfType(
                    game,
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
            if (PD_Game_Queries.GQ_SS_DeadlyOutbreaks(game) == true)
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
            PD_Game game,
            PD_City city,
            int infectionCubeType
            )
        {
            PD_ME_InfectionCube infectionCubeToAdd =
                game.MapElements.InactiveInfectionCubesPerType[infectionCubeType].DrawLast();
            game.MapElements.InfectionCubesPerCityID[city.ID].Add(infectionCubeToAdd);
        }

        public static void GO_PlaceResearchStationOnCity(
            PD_Game game,
            PD_City city
            )
        {
            PD_ME_ResearchStation researchStation = game.MapElements.InactiveResearchStations.DrawLast();
            game.MapElements.ResearchStationsPerCityID[city.ID].Add(researchStation);
        }

        public static void GO_MovePawnFromCityToCity(
            PD_Game game,
            PD_ME_PlayerPawn pawn,
            PD_City initialCity,
            PD_City targetCity
            )
        {
            game.MapElements.PlayerPawnsPerCityID[initialCity.ID].Remove(pawn);
            game.MapElements.PlayerPawnsPerCityID[targetCity.ID].Add(pawn);
        }

        public static void GO_PlayerDiscardsPlayerCard(
            PD_Game game,
            PD_Player player,
            PD_PlayerCardBase playerCardToDiscard
            )
        {
            game.Cards.PlayerCardsPerPlayerID[player.ID].Remove(playerCardToDiscard);
            game.Cards.DeckOfDiscardedPlayerCards.Add(playerCardToDiscard);
        }

        public static void GO_Remove_One_InfectionCube_OfType_FromCity(
            PD_Game game,
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
            PD_Game game,
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
            PD_Game game,
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
