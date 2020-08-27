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
    public class PD_Game_MacroSynthesis_Tests
    {
        [TestMethod()]
        public void FindAll_Macros_Test()
        {
            Random randomness_provider = new Random();

            string data = DataUtilities.Read_GameCreationData();
            PD_Game game = PD_GameCreator.CreateNewGame(
                randomness_provider,
                4, 
                0, 
                data, 
                true
                );
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();

            List<PD_Role_Card> allRoleCards = game.GameElementReferences.RoleCards;

            // fix player roles
            game.RoleCardsPerPlayerID[0] = allRoleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Operations_Expert
                );
            game.RoleCardsPerPlayerID[1] = allRoleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Medic
                );
            game.RoleCardsPerPlayerID[2] = allRoleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Researcher
                );
            game.RoleCardsPerPlayerID[3] = allRoleCards.Find(
                x =>
                x.Role == PD_Player_Roles.Scientist
                );

            game.Cards.PlayerCardsPerPlayerID[0].Clear();
            game.Cards.PlayerCardsPerPlayerID[1].Clear();
            game.Cards.PlayerCardsPerPlayerID[2].Clear();
            game.Cards.PlayerCardsPerPlayerID[3].Clear();


            var rsCities = PD_Game_Queries.GQ_Find_ResearchStationCities(game);
            List<PD_MacroAction> allMacros = PD_MacroActionsSynthesisSystem.FindAll_Macros(
                game,
                pathFinder,
                rsCities
                );

            var buildRSMacros = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro
                );

            Assert.IsTrue(buildRSMacros.Count == 47);

            var simpleWalks = PD_MacroActionsSynthesisSystem.FindAll_SimpleWalk_Sequences(
                game,
                pathFinder,
                rsCities,
                PD_Game_Queries.GQ_CurrentPlayer(game),
                PD_Game_Queries.GQ_Find_CurrentPlayer_Location(game)
                );

            Assert.IsTrue(simpleWalks.Count == 47);

            PD_City Milan = game.Map.Cities.Find(
                x =>
                x.Name == "Milan"
                );
            PD_City Atlanta = game.Map.Cities.Find(
                x =>
                x.Name == "Atlanta"
                );

            PD_Game_Operators.GO_MovePawnFromCityToCity(
                game,
                PD_Game_Queries.GQ_Find_PlayerPawn(game, game.Players[1]),
                Atlanta,
                Milan
                );
            PD_Game_Operators.GO_MovePawnFromCityToCity(
                game,
                PD_Game_Queries.GQ_Find_PlayerPawn(game, game.Players[2]),
                Atlanta,
                Milan
                );
            PD_Game_Operators.GO_MovePawnFromCityToCity(
                game,
                PD_Game_Queries.GQ_Find_PlayerPawn(game, game.Players[3]),
                Atlanta,
                Milan
                );

            game.Cards.PlayerCardsPerPlayerID[0].Add(
                game.GameElementReferences.CityCards.Find(
                    x =>
                    x.Name == "Milan"
                    )
                );
            game.Cards.PlayerCardsPerPlayerID[0].Add(
                game.GameElementReferences.CityCards.Find(
                    x =>
                    x.Name == "Chicago"
                    )
                );

            allMacros = PD_MacroActionsSynthesisSystem.FindAll_Macros(
                game,
                pathFinder,
                rsCities
                );

            var shareKnowledge_Give_Macros = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro
                && x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                );

            Assert.IsTrue(shareKnowledge_Give_Macros.Count == 3);

            var shareKnowledge_Give_Macros_2 = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro
                && x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                );

            Assert.IsTrue(shareKnowledge_Give_Macros_2.Count == 3);

            //game.Cards.PlayerCardsPerPlayerID[0].RemoveAll(
            //    x =>
            //    x

            //    );

        }
    }
}