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

            PD_Game game = PD_Game.Create(
                randomness_provider,
                4,
                0,
                true
                );
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();

            List<int> allRoleCards = game.GameElementReferences.RoleCards;

            // fix player roles
            game.role__per__player[0] = allRoleCards.Find(
                x =>
                x == PD_Player_Roles.Operations_Expert
                );
            game.role__per__player[1] = allRoleCards.Find(
                x =>
                x == PD_Player_Roles.Medic
                );
            game.role__per__player[2] = allRoleCards.Find(
                x =>
                x == PD_Player_Roles.Researcher
                );
            game.role__per__player[3] = allRoleCards.Find(
                x =>
                x == PD_Player_Roles.Scientist
                );

            game.Cards.PlayerCardsPerPlayerID[0].Clear();
            game.Cards.PlayerCardsPerPlayerID[1].Clear();
            game.Cards.PlayerCardsPerPlayerID[2].Clear();
            game.Cards.PlayerCardsPerPlayerID[3].Clear();


            var rsCities = game.GQ_ResearchStationCities();
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
                game.GQ_CurrentPlayer(),
                game.GQ_CurrentPlayer_Location()
                );

            Assert.IsTrue(simpleWalks.Count == 47);

            int Milan = game.Map.cities.Find(
                x =>
                game.Map.name__per__city[x] == "Milan"
                );
            int Atlanta = game.Map.cities.Find(
                x =>
                game.Map.name__per__city[x] == "Atlanta"
                );

            game.GO_MovePawnFromCityToCity(
                game.players[1],
                Atlanta,
                Milan
                );
            game.GO_MovePawnFromCityToCity(
                game.players[2],
                Atlanta,
                Milan
                );
            game.GO_MovePawnFromCityToCity(
                game.players[3],
                Atlanta,
                Milan
                );

            game.Cards.PlayerCardsPerPlayerID[0].Add(
                game.GameElementReferences.CityCards.Find(
                    x =>
                    game.Map.name__per__city[x] == "Milan"
                    )
                );
            game.Cards.PlayerCardsPerPlayerID[0].Add(
                game.GameElementReferences.CityCards.Find(
                    x =>
                    game.Map.name__per__city[x] == "Chicago"
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