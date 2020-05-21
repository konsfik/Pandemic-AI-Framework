using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_SinglePolicy : PD_AI_Macro_Agent_Base
    {
        public PD_AI_MacroAgent_MainPolicy_Base MainPolicy { get; private set; }
        public PD_AI_MacroAgent_DiscardPolicy_Base DiscardPolicy { get; private set; }

        public PD_AI_MacroAgent_SinglePolicy(
            PD_AI_MacroAgent_MainPolicy_Base mainPolicy,
            PD_AI_MacroAgent_DiscardPolicy_Base discardPolicy
            )
        {
            MainPolicy = mainPolicy;
            DiscardPolicy = discardPolicy;
        }

        protected override PD_MacroAction MainPlayerActions_Behaviour(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.GetAvailableMacros(pathFinder);

            if (allMacros == null || allMacros.Count == 0) return null;

            var macros = MainPolicy.FilterMacros(game, pathFinder, allMacros);

            if (macros.Count > 0) {
                return macros.GetOneRandom();
            }

            return allMacros.GetOneRandom();
        }

        protected override PD_MacroAction Discarding_DuringMainPlayerActions_Behaviour(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.GetAvailableMacros(pathFinder);

            if (allMacros == null || allMacros.Count == 0) return null;

            var macros = DiscardPolicy.FilterMacros(game, pathFinder, allMacros);

            if (macros.Count > 0) {
                return macros.GetOneRandom();
            }

            return allMacros.GetOneRandom();
        }

        protected override PD_MacroAction Discarding_AfterDrawing_Behaviour(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.GetAvailableMacros(pathFinder);

            if (allMacros == null || allMacros.Count == 0) return null;

            var macros = DiscardPolicy.FilterMacros(game, pathFinder, allMacros);

            if (macros.Count > 0)
            {
                return macros.GetOneRandom();
            }

            return allMacros.GetOneRandom();
        }

        public override string GetDescription()
        {
            return this.GetType().Name.ToString();
        }
    }
}
