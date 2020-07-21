using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic_AI_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDTests.GameGenerationAndSerialization
{
    [TestClass()]
    class Game_Tests
    {
        [TestMethod()]
        public void GenerateGame_Test() {

            string data = DataUtilities.Read_GameCreationData();

            PD_GameCreator.CreateNewGame(
                4,
                0,
                data,
                true
                );
        }
    }
}
