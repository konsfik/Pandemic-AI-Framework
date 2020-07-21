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
            var sol_dir = PD_IO_Utilities.SolutionDirectory().FullName;
            var saved_games_path = System.IO.Path.Combine(sol_dir, "ParameterTuning_TestBed");
            var file_paths = PD_IO_Utilities.GetFilePathsInFolder(saved_games_path);

            foreach (var file_path in file_paths)
            {
                PD_Game deserialized_game = PD_IO_Utilities.DeserializeFromJsonFile<PD_Game>(
                    file_paths[0]
                    );

                Assert.IsTrue(deserialized_game != null);
                Assert.IsTrue(deserialized_game.GameStateCounter.NumberOfPlayers == 4);
                Assert.IsTrue(deserialized_game.GameSettings.GameDifficultyLevel == 0);
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