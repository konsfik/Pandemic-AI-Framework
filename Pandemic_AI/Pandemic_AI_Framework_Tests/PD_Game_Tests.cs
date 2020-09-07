using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Pandemic_AI_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework.Tests
{
    [TestClass()]
    public class PD_Game_Tests
    {
        [TestMethod()]
        public void Serialize_Deserialize_MiniGame()
        {
            Random randomness_provider = new Random();

            for (int i = 0; i < 2000; i++)
            {
                // create a normal game, for later use
                PD_Game initial_game = PD_Game.Create(randomness_provider, 4, 0, true);

                // convert to mini game
                PD_MiniGame converted_mini_game = initial_game.Convert_To_MiniGame();

                // serialize the mini game
                string serialized_mini_game = converted_mini_game.To_Json_String(
                    Formatting.None,
                    TypeNameHandling.None,
                    PreserveReferencesHandling.None
                    );

                // deserialize the mini game
                PD_MiniGame deserialized_mini_game
                    = JsonConvert.DeserializeObject<PD_MiniGame>(serialized_mini_game);

                // test that the deserialized game is exactly the same as the mini game
                Assert.IsTrue(converted_mini_game.GetHashCode() == deserialized_mini_game.GetHashCode());
                Assert.IsTrue(converted_mini_game == deserialized_mini_game);
                Assert.IsTrue(converted_mini_game.Equals(deserialized_mini_game));

                // create a normal game, from the deserialized game
                PD_Game final_game = deserialized_mini_game.Convert_To_Game();

                Assert.IsTrue(
                    Games_Practically_Equal(
                        initial_game,
                        final_game
                        )
                    );

                final_game.UpdateAvailablePlayerActions();

                randomness_provider = new Random(i);
                while (final_game.GQ_Is_Ongoing())
                {
                    PD_GameAction_Base action =
                        final_game
                        .CurrentAvailablePlayerActions
                        .GetOneRandom(randomness_provider);
                    final_game.ApplySpecificPlayerAction(
                        randomness_provider,
                        action
                        );
                }
                randomness_provider = new Random(i);
                while (initial_game.GQ_Is_Ongoing())
                {
                    var action = initial_game.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider);
                    initial_game.ApplySpecificPlayerAction(
                        randomness_provider,
                        action);
                }

                Assert.IsTrue(Games_Practically_Equal(initial_game, final_game));
            }
        }

        #region help methods
        private bool Games_Practically_Equal(PD_Game game_1, PD_Game game_2)
        {
            if (game_1.UniqueID != game_2.UniqueID)
            {
                return false;
            }
            else if (game_1.GameSettings.Equals(game_2.GameSettings) == false)
            {
                return false;
            }
            else if (game_1.GameFSM.Equals(game_2.GameFSM) == false)
            {
                return false;
            }
            else if (game_1.GameStateCounter.Equals(game_2.GameStateCounter) == false)
            {
                return false;
            }
            else if (game_1.Players.List_Equals(game_2.Players) == false)
            {
                return false;
            }
            else if (game_1.Map.Equals(game_2.Map) == false)
            {
                return false;
            }
            else if (PracticalGameComparison_GameElementReferences(game_1, game_2) == false)
            {
                return false;
            }
            else if (PracticalGameComparison_Cards(game_1, game_2) == false)
            {
                return false;
            }
            else if (PracticalGameComparison_MapElements(game_1, game_2) == false)
            {
                return false;
            }
            else if (
                game_1.CurrentAvailablePlayerActions.List_Equals(
                    game_2.CurrentAvailablePlayerActions
                    ) == false
                )
            {
                return false;
            }

            return true;
        }

        public bool PracticalGameComparison_GameElementReferences(PD_Game game_1, PD_Game game_2)
        {
            if (game_1.GameElementReferences.InfectionCards.List_Equals(
                game_2.GameElementReferences.InfectionCards) == false)
                return false;
            else if (game_1.GameElementReferences.CityCards.List_Equals(
                game_2.GameElementReferences.CityCards) == false)
                return false;
            else if (game_1.GameElementReferences.EpidemicCards.List_Equals(
                game_2.GameElementReferences.EpidemicCards) == false)
                return false;

            // make sure the pawns correspond to the same roles.
            // skip checking the pawn - ids, this is not important...
            if (game_1.GameElementReferences.PlayerPawns.Count
                != game_2.GameElementReferences.PlayerPawns.Count)
            {
                return false;
            }
            for (int pp = 0; pp < game_1.GameElementReferences.PlayerPawns.Count; pp++)
            {
                if (game_1.GameElementReferences.PlayerPawns[pp].Role
                    != game_2.GameElementReferences.PlayerPawns[pp].Role)
                {
                    return false;
                }
            }

            // same as in the pawns, only check the roles, not the card ids.
            if (game_1.GameElementReferences.RoleCards.Count
                != game_2.GameElementReferences.RoleCards.Count)
            {
                return false;
            }
            for (int pp = 0; pp < game_1.GameElementReferences.RoleCards.Count; pp++)
            {
                if (game_1.GameElementReferences.RoleCards[pp].Role
                    != game_2.GameElementReferences.RoleCards[pp].Role)
                {
                    return false;
                }
            }

            if (game_1.GameElementReferences.ResearchStations.List_Equals(
                game_2.GameElementReferences.ResearchStations) == false)
                return false;
            else if (game_1.GameElementReferences.InfectionCubes.List_Equals(
                game_2.GameElementReferences.InfectionCubes) == false)
                return false;
            else
                return true;
        }

        public bool PracticalGameComparison_Cards(PD_Game game_1, PD_Game game_2)
        {
            if (game_1.Cards.DividedDeckOfInfectionCards.List_Equals(
                game_2.Cards.DividedDeckOfInfectionCards) == false)
            {
                return false;
            }
            else if (game_1.Cards.ActiveInfectionCards.List_Equals(
                game_2.Cards.ActiveInfectionCards) == false)
            {
                return false;
            }
            else if (game_1.Cards.DeckOfDiscardedInfectionCards.List_Equals(
                game_2.Cards.DeckOfDiscardedInfectionCards) == false)
            {
                return false;
            }
            else if (game_1.Cards.DividedDeckOfPlayerCards.List_Equals(
                game_2.Cards.DividedDeckOfPlayerCards) == false)
            {
                return false;
            }
            else if (game_1.Cards.DeckOfDiscardedPlayerCards.List_Equals(
                game_2.Cards.DeckOfDiscardedPlayerCards) == false)
            {
                return false;
            }
            else if (game_1.Cards.DeckOfDiscardedPlayerCards.List_Equals(
                game_2.Cards.DeckOfDiscardedPlayerCards) == false)
            {
                return false;
            }
            else if (game_1.Cards.PlayerCardsPerPlayerID.Dictionary_Equal(
                game_2.Cards.PlayerCardsPerPlayerID) == false)
            {
                return false;
            }
            else if (game_1.Cards.InactiveRoleCards.List_Equals(
                game_2.Cards.InactiveRoleCards) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool PracticalGameComparison_MapElements(PD_Game game_1, PD_Game game_2)
        {
            // inactive player pawns
            if (game_1.MapElements.InactivePlayerPawns.Count
                != game_2.MapElements.InactivePlayerPawns.Count)
            {
                return false;
            }
            foreach (var pp in game_1.MapElements.InactivePlayerPawns)
            {
                if (game_2.MapElements.InactivePlayerPawns.Any(x => x.Role == pp.Role) == false)
                {
                    return false;
                }
            }

            foreach (var city in game_1.Map.Cities)
            {
                List<PD_ME_PlayerPawn> this_city_pawns = game_1.MapElements.PlayerPawnsPerCityID[city.ID];
                List<PD_ME_PlayerPawn> other_city_pawns = game_2.MapElements.PlayerPawnsPerCityID[city.ID];
                foreach (var pawn in this_city_pawns)
                {
                    if (other_city_pawns.Any(x => x.Role == pawn.Role) == false)
                    {
                        return false;
                    }
                }
            }

            for (int t = 0; t < 4; t++)
            {
                if (
                    game_1.MapElements.InactiveInfectionCubesPerType[t].Count
                    !=
                    game_2.MapElements.InactiveInfectionCubesPerType[t].Count
                    )
                {
                    return false;
                }
            }

            foreach (var city in game_1.Map.Cities)
            {
                for (int t = 0; t < 4; t++)
                {
                    if (
                        game_1.GQ_Find_InfectionCubes_OfType_OnCity(city, t).Count
                        !=
                        game_2.GQ_Find_InfectionCubes_OfType_OnCity(city, t).Count
                        )
                    {
                        return false;
                    }
                }
            }

            if (game_1.MapElements.InactiveResearchStations.List_Equals(
                game_2.MapElements.InactiveResearchStations) == false)
            {
                return false;
            }
            else if (game_1.MapElements.ResearchStationsPerCityID.Dictionary_Equals(
                game_2.MapElements.ResearchStationsPerCityID) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        [TestMethod()]
        public void Deserialize_ExistingGame_NewRepresentation_Test()
        {
            Random randomness_provider = new Random(1001);

            string saved_games_path = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                "ParameterTuning_TestBed_NewRepresentation"
                );
            var game_file_paths = PD_IO_Utilities.GetFilePathsInFolder(saved_games_path);

            // play the games until the end, using single actions
            foreach (var game_file_path in game_file_paths)
            {
                string serialized_mini_game = PD_IO_Utilities.ReadFile(game_file_path);

                PD_MiniGame deserialized_mini_game = 
                    JsonConvert.DeserializeObject<PD_MiniGame>(serialized_mini_game);

                PD_Game game = deserialized_mini_game.Convert_To_Game();

                // play the game until the end...
                while (game.GQ_Is_Ongoing())
                {
                    var available_actions = game.CurrentAvailablePlayerActions;
                    var random_action = available_actions.GetOneRandom(randomness_provider);
                    game.ApplySpecificPlayerAction(
                        randomness_provider,
                        random_action
                        );
                }

                Assert.IsTrue(game.GQ_Is_Ongoing() == false);
            }

            // play the games until the end, using macro actions
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();
            foreach (var game_file_path in game_file_paths)
            {
                string serialized_mini_game = PD_IO_Utilities.ReadFile(game_file_path);

                PD_MiniGame deserialized_mini_game =
                    JsonConvert.DeserializeObject<PD_MiniGame>(serialized_mini_game);

                PD_Game game = deserialized_mini_game.Convert_To_Game();

                // play the game until the end...
                while (game.GQ_Is_Ongoing())
                {
                    var available_macro_actions = game.GetAvailableMacros(pathFinder);
                    var random_macro_action = available_macro_actions.GetOneRandom(randomness_provider);
                    game.ApplySpecificMacro(
                        randomness_provider,
                        random_macro_action
                        );
                }

                Assert.IsTrue(game.GQ_Is_Ongoing() == false);
            }
        }

        [TestMethod()]
        public void Generate_Game_Without_Data()
        {
            Random randomness_provider = new Random();

            // generate 100 games for every possible settings selection
            for (int num_players = 2; num_players <= 4; num_players++)
            {
                for (int game_difficulty = 0; game_difficulty <= 2; game_difficulty++)
                {
                    // repeat the process 100 times!
                    for (int i = 0; i < 100; i++)
                    {
                        PD_Game game = PD_Game.Create(
                            randomness_provider,
                            num_players,
                            game_difficulty,
                            true
                            );

                        while (game.GQ_Is_Ongoing())
                        {
                            var random_action = game
                                .CurrentAvailablePlayerActions
                                .GetOneRandom(randomness_provider);

                            game.ApplySpecificPlayerAction(
                                randomness_provider,
                                random_action
                                );
                        }

                        Assert.IsTrue(game.GQ_Is_Ongoing() == false);
                    }
                }
            }

        }


        [TestMethod()]
        public void RandomSeed_Tests()
        {
            Random randomness_provider = new Random(1000);

            PD_Game game_1 = PD_Game.Create(
                randomness_provider,
                4,
                0,
                true
                );

            randomness_provider = new Random(1000);

            PD_Game game_2 = PD_Game.Create(
                randomness_provider,
                4,
                0,
                true
                );

            randomness_provider = new Random(1000);
            while (game_1.GQ_Is_Ongoing())
            {
                game_1.ApplySpecificPlayerAction(
                    randomness_provider,
                    game_1.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider)
                    );
            }

            randomness_provider = new Random(1000);
            while (game_2.GQ_Is_Ongoing())
            {
                game_2.ApplySpecificPlayerAction(
                    randomness_provider,
                    game_2.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider)
                    );
            }

            game_2.OverrideUniqueID(game_1.UniqueID);
            game_2.OverrideStartTime(game_1.StartTime);
            game_2.OverrideEndTime(game_1.EndTime);

            Assert.IsTrue(game_1.Equals(game_2));
            Assert.IsTrue(game_1 == game_2);
            Assert.IsTrue(game_1.GetHashCode() == game_2.GetHashCode());
        }

        [TestMethod()]
        public void GetCustomDeepCopy_Test()
        {
            Random randomness_provider = new Random();

            PD_Game game = PD_Game.Create(
                randomness_provider,
                4,
                0,
                true
                );
            PD_Game gameCopy = game.GetCustomDeepCopy();

            Assert.IsTrue(
                game.UniqueID
                ==
                gameCopy.UniqueID
                );

            Assert.IsTrue(
                game.StartTime
                ==
                gameCopy.StartTime
                );

            Assert.IsTrue(
                game.EndTime
                ==
                gameCopy.EndTime
                );

            Assert.IsTrue(
                game.GameSettings
                ==
                gameCopy.GameSettings
                );

            Assert.IsTrue(
                game.GameFSM
                ==
                gameCopy.GameFSM
                );

            Assert.IsTrue(
                game.GameStateCounter
                ==
                gameCopy.GameStateCounter
                );

            Assert.IsTrue(
                game.Players.SequenceEqual(gameCopy.Players)
                );

        }
    }
}