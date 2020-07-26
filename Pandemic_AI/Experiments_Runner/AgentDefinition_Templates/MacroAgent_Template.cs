using System;
using System.Collections.Generic;
using System.Text;
using Pandemic_AI_Framework;

namespace Experiments_Runner
{
    /// <summary>
    /// A template for developing an agent that operates within the 
    /// macro action - based representation of the game state.
    /// </summary>
    public class MacroAgent_Template : PD_AI_Macro_Agent_Base
    {
        /// <summary>
        /// Returns a description of the agent
        /// This is optional and has nothing to do with the agent's functionality.
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return this.GetType().Name;
        }

        protected override PD_MacroAction Discarding_AfterDrawing_Behaviour(
            Random randomness_provider,
            PD_Game game, 
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.CurrentAvailableMacros;
            return allMacros.GetOneRandom(randomness_provider);
        }

        protected override PD_MacroAction Discarding_DuringMainPlayerActions_Behaviour(
            Random randommness_provider,
            PD_Game game, 
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.CurrentAvailableMacros;
            return allMacros.GetOneRandom(randommness_provider);
        }

        protected override PD_MacroAction MainPlayerActions_Behaviour(
            Random randommness_provider,
            PD_Game game, 
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.CurrentAvailableMacros;
            return allMacros.GetOneRandom(randommness_provider);
        }
    }
}
