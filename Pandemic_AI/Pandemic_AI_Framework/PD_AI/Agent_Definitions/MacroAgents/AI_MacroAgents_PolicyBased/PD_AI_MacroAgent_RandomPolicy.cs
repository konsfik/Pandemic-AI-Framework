using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_RandomPolicy : PD_AI_Macro_Agent_Base
    {
        public List<PD_AI_MacroAgent_MainPolicy_Base> Available_MainPolicies { get; private set; }
        public PD_AI_MacroAgent_MainPolicy_Base MainPolicy_FallBack { get; private set; }
        public List<PD_AI_MacroAgent_DiscardPolicy_Base> Available_DiscardPolicies { get; private set; }

        public PD_AI_MacroAgent_RandomPolicy(
            List<PD_AI_MacroAgent_MainPolicy_Base> available_MainPolicies,
            PD_AI_MacroAgent_MainPolicy_Base mainPolicy_FallBack,
            List<PD_AI_MacroAgent_DiscardPolicy_Base> available_DiscardPolicies
            )
        {
            Available_MainPolicies = available_MainPolicies;
            MainPolicy_FallBack = mainPolicy_FallBack;
            Available_DiscardPolicies = available_DiscardPolicies;
        }

        protected override PD_MacroAction MainPlayerActions_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            var allMacros = game.GetAvailableMacros(pathFinder);

            if (allMacros == null || allMacros.Count == 0) return null;

            Available_MainPolicies.Shuffle(randomness_provider);

            foreach (var policy in Available_MainPolicies) {
                var macros = policy.FilterMacros(game, pathFinder, allMacros);
                if (macros.Count > 0)
                {
                    return macros.GetOneRandom(randomness_provider);
                }
            }

            var fallBack_Macros = MainPolicy_FallBack.FilterMacros(
                game,
                pathFinder,
                allMacros
                );

            if (fallBack_Macros.Count > 0)
            {
                return fallBack_Macros.GetOneRandom(randomness_provider);
            }

            // if none applicable, then:
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

            // DISCARD POLICY
            Available_DiscardPolicies.Shuffle(randomness_provider);

            foreach (var policy in Available_DiscardPolicies)
            {
                var macros = policy.FilterMacros(game, pathFinder, allMacros);
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

            // DISCARD POLICY
            Available_DiscardPolicies.Shuffle(randomness_provider);

            foreach (var policy in Available_DiscardPolicies)
            {
                var macros = policy.FilterMacros(game, pathFinder, allMacros);
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
