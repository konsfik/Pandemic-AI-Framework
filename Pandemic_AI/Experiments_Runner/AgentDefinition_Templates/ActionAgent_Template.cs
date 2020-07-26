using System;
using System.Collections.Generic;
using System.Text;
using Pandemic_AI_Framework;

namespace Experiments_Runner
{
    /// <summary>
    /// A template for developing an agent that operates within the 
    /// single action - based representation of the game state.
    /// </summary>
    public class ActionAgent_Template : PD_AI_Action_Agent_Base
    {
        /// <summary>
        /// Returns a description of the agent
        /// This is optional and has nothing to do with the agent's functionality.
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return this.GetType().Name.ToString();
        }

        /// <summary>
        /// This is the method that defines the agent's operation.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public override PD_GameAction_Base GetNextAction(
            Random randomness_provider,
            PD_Game game
            )
        {
            var availableActions = game.CurrentAvailablePlayerActions;

            return availableActions.GetOneRandom(randomness_provider);
        }
    }
}
