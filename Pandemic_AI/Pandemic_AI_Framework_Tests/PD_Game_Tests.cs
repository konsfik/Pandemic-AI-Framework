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
                while (PD_Game_Queries.GQ_Is_GameOngoing(deserialized_game))
                {
                    var available_actions = deserialized_game.CurrentAvailablePlayerActions;
                    deserialized_game.ApplySpecificPlayerAction(available_actions.GetOneRandom());
                }

                Assert.IsTrue(PD_Game_Queries.GQ_Is_GameOngoing(deserialized_game) == false);
            }

            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();
            // play the games until the end, using macro actions
            foreach (var game_file_path in game_file_paths)
            {
                PD_Game deserialized_game = PD_IO_Utilities.DeserializeFromJsonFile<PD_Game>(
                    game_file_path
                    );


                // play the game until the end...
                while (PD_Game_Queries.GQ_Is_GameOngoing(deserialized_game))
                {
                    var available_macro_actions = deserialized_game.GetAvailableMacros(pathFinder);
                    deserialized_game.ApplySpecificMacro(available_macro_actions.GetOneRandom());
                }

                Assert.IsTrue(PD_Game_Queries.GQ_Is_GameOngoing(deserialized_game) == false);
            }
        }

        [TestMethod()]
        public void Generate_Game()
        {
            string data = DataUtilities.Read_GameCreationData();

            for (int num_players = 2; num_players <= 4; num_players++)
            {
                for (int game_difficulty = 0; game_difficulty <= 2; game_difficulty++)
                {
                    // repeat the process 100 times!
                    for (int i = 0; i < 100; i++)
                    {
                        PD_Game game = PD_GameCreator.CreateNewGame(num_players, game_difficulty, data, true);

                        while (PD_Game_Queries.GQ_Is_GameOngoing(game))
                        {
                            var available_actions = game.CurrentAvailablePlayerActions;
                            game.ApplySpecificPlayerAction(available_actions.GetOneRandom());
                        }

                        Assert.IsTrue(PD_Game_Queries.GQ_Is_GameOngoing(game) == false);
                    }
                }
            }

        }


        [TestMethod()]
        public void GetCustomDeepCopy_Test()
        {

            string data = DataUtilities.Read_GameCreationData();
            PD_Game game = PD_GameCreator.CreateNewGame_SpecificRoles(data);
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