using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_AI_Macro_Agent_Base : PD_AI_Agent_Base
    {
        public PD_MacroAction GetNextMacroAction(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            if (game.GQ_IsInState_ApplyingMainPlayerActions())
            {
                return MainPlayerActions_Behaviour(randomness_provider, game, pathFinder);
            }
            else if (game.GQ_IsInState_DiscardDuringMainPlayerActions())
            {
                return Discarding_DuringMainPlayerActions_Behaviour(randomness_provider, game, pathFinder);
            }
            else if (game.GQ_IsInState_DiscardAfterDrawing())
            {
                return Discarding_AfterDrawing_Behaviour(randomness_provider, game, pathFinder);
            }
            else
            {
                throw new System.Exception(
                    "A macro action was requested in an invalid game state"
                    );
            }
        }

        protected abstract PD_MacroAction MainPlayerActions_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            );

        protected abstract PD_MacroAction Discarding_DuringMainPlayerActions_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            );

        protected abstract PD_MacroAction Discarding_AfterDrawing_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            );
    }
}
