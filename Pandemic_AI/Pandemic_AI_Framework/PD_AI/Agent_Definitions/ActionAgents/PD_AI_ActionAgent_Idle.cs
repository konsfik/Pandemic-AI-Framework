using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_ActionAgent_Idle : PD_AI_Action_Agent_Base
    {
        public override PD_GameAction_Base GetNextAction(
            Random randomness_provider,
            PD_Game game
            )
        {
            var availableCommands = game.CurrentAvailablePlayerActions;

            if (availableCommands.Count == 0)
            {
                return null;
            }

            if (
                availableCommands.Any(
                    x =>
                    x.GetType() == typeof(PA_Stay)
                    )
                )
            {
                return availableCommands.Find(
                    x =>
                    x.GetType() == typeof(PA_Stay)
                    );
            }

            return availableCommands.GetOneRandom(randomness_provider);
        }

        public override string GetDescription()
        {
            string description =
                this.GetType().ToString() +
                " : " +
                "An AI agent that chooses one of the available actions" +
                "completely at random.";

            return description;
        }
    }
}