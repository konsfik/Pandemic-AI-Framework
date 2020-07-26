using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_HierarchicalPolicy : PD_AI_Macro_Agent_Base
    {
        public List<PD_AI_MacroAgent_MainPolicy_Base> Main_Policies { get; private set; }
        public List<PD_AI_MacroAgent_DiscardPolicy_Base> Discard_Policies { get; private set; }

        public PD_AI_MacroAgent_HierarchicalPolicy(
            List<PD_AI_MacroAgent_MainPolicy_Base> main_Policies,
            List<PD_AI_MacroAgent_DiscardPolicy_Base> discard_Policies
            )
        {
            Main_Policies = main_Policies;
            Discard_Policies = discard_Policies;
        }

        protected override PD_MacroAction MainPlayerActions_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.GetAvailableMacros(pathFinder);

            if (allMacros == null || allMacros.Count == 0) return null;

            foreach (var policy in Main_Policies)
            {
                var policyMacros = policy.FilterMacros(game, pathFinder, allMacros);
                if (policyMacros.Count > 0)
                {
                    return policyMacros.GetOneRandom(randomness_provider);
                }
            }

            return allMacros.GetOneRandom(randomness_provider);
        }

        protected override PD_MacroAction Discarding_DuringMainPlayerActions_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.GetAvailableMacros(pathFinder);

            if (allMacros == null || allMacros.Count == 0) return null;

            foreach (var discardPolicy in Discard_Policies)
            {
                var macros = discardPolicy.FilterMacros(
                    game,
                    pathFinder,
                    allMacros
                    );
                if (macros.Count > 0)
                {
                    return macros.GetOneRandom(randomness_provider);
                }
            }
            return allMacros.GetOneRandom(randomness_provider);
        }

        protected override PD_MacroAction Discarding_AfterDrawing_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.GetAvailableMacros(pathFinder);

            if (allMacros == null || allMacros.Count == 0) return null;

            foreach (var discardPolicy in Discard_Policies)
            {
                var macros = discardPolicy.FilterMacros(
                    game,
                    pathFinder,
                    allMacros
                    );
                if (macros.Count > 0)
                {
                    return macros.GetOneRandom(randomness_provider);
                }
            }
            return allMacros.GetOneRandom(randomness_provider);
        }

        public override string GetDescription()
        {
            return this.GetType().Name.ToString();
        }
    }
}
