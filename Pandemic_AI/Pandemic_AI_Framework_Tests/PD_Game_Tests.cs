using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void Deserialize_ExistingGame_Test()
        {
            Random randomness_provider = new Random();

            string saved_games_path = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                "ParameterTuning_TestBed"
                );
            var game_file_paths = PD_IO_Utilities.GetFilePathsInFolder(saved_games_path);

            // play the games until the end, using single actions
            foreach (var game_file_path in game_file_paths)
            {
                PD_Game deserialized_game = PD_IO_Utilities.DeserializeFromJsonFile<PD_Game>(
                    game_file_path
                    );


                // play the game until the end...
                while (deserialized_game.GQ_Is_Ongoing())
                {
                    var available_actions = deserialized_game.CurrentAvailablePlayerActions;
                    var random_action = available_actions.GetOneRandom(randomness_provider);
                    deserialized_game.ApplySpecificPlayerAction(
                        randomness_provider,
                        random_action
                        );
                }

                Assert.IsTrue(deserialized_game.GQ_Is_Ongoing() == false);
            }

            // play the games until the end, using macro actions
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();
            foreach (var game_file_path in game_file_paths)
            {
                PD_Game deserialized_game = PD_IO_Utilities.DeserializeFromJsonFile<PD_Game>(
                    game_file_path
                    );

                // play the game until the end...
                while (deserialized_game.GQ_Is_Ongoing())
                {
                    var available_macro_actions = deserialized_game.GetAvailableMacros(pathFinder);
                    var random_macro_action = available_macro_actions.GetOneRandom(randomness_provider);
                    deserialized_game.ApplySpecificMacro(
                        randomness_provider,
                        random_macro_action
                        );
                }

                Assert.IsTrue(deserialized_game.GQ_Is_Ongoing() == false);
            }
        }

        [TestMethod()]
        public void Generate_Game_Without_Data() {
            Random randomness_provider = new Random();

            // generate 100 games for every possible settings selection
            for (int num_players = 2; num_players <= 4; num_players++)
            {
                for (int game_difficulty = 0; game_difficulty <= 2; game_difficulty++)
                {
                    // repeat the process 100 times!
                    for (int i = 0; i < 100; i++)
                    {
                        PD_Game game = PD_Game.Create_Default(
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

        /// <summary>
        /// Converts the game from its default implementation
        /// to the minified game state, 
        /// then to a serialized state (json_
        /// then back to the minified state
        /// and finally back to the default implementation...
        /// Then, the final game state must be playable until the end, without any errors...
        /// 
        /// The whole process is repeated a number of times, with randomly set up games...
        /// </summary>
        [TestMethod()]
        public void Game_Conversions() {
            Random randomness_provider = new Random();

            PD_Game original_game = PD_Game.Create_Default(
                randomness_provider,
                4,
                0,
                true
                );

            //Pandemic_Mini_State mini_game = Pandemic_Mini_State.From_Normal_State(original_game);

        }

        [TestMethod()]
        public void RandomSeed_Tests() {
            Random randomness_provider = new Random(1000);

            PD_Game game_1 = PD_Game.Create_Default(
                randomness_provider,
                4,
                0,
                true
                );

            randomness_provider = new Random(1000);

            PD_Game game_2 = PD_Game.Create_Default(
                randomness_provider,
                4,
                0,
                true
                );

            for (int i = 0; i < game_1.Cards.DividedDeckOfPlayerCards.Count; i++)
            {
                for (int j = 0; j < game_1.Cards.DividedDeckOfPlayerCards[i].Count; j++)
                {
                    Assert.IsTrue(
                        game_1.Cards.DividedDeckOfPlayerCards[i][j] 
                        == game_2.Cards.DividedDeckOfPlayerCards[i][j]
                        );
                }
            }
        }

        [TestMethod()]
        public void GetCustomDeepCopy_Test()
        {
            Random randomness_provider = new Random();

            PD_Game game = PD_Game.Create_Default(
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