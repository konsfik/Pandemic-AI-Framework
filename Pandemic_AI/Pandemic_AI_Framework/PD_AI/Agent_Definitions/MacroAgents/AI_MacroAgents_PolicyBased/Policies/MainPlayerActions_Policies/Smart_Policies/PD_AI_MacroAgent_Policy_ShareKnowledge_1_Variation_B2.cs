using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// Variation A:
    /// remove the check that the "other" can get to the exchange in 4 steps (in the take position part)
    /// </summary>
    [Serializable]
    public class PD_AI_MacroAgent_Policy_ShareKnowledge_1_Variation_B2 : PD_AI_MacroAgent_MainPolicy_Base
    {

        public override List<PD_MacroAction> FilterMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
            if (PD_Game_Queries.GQ_IsInState_ApplyingMainPlayerActions(game) == false)
            {
                throw new System.Exception("wrong state.");
            }

            // share positive now
            List<PD_MacroAction> shareKnowledge_Positive_Now =
                PD_AI_MacroAgent_SubPolicies.ShareKnowledge_Positive_Now(
                    game,
                    pathFinder,
                    allMacros
                    );
            if (shareKnowledge_Positive_Now.Count > 0)
            {
                return shareKnowledge_Positive_Now;
            }

            // take position positive now - other 4
            List<PD_MacroAction> takePosition_Positive_Now_OtherPlayerWithinFourSteps =
                PD_AI_MacroAgent_SubPolicies.TakePosition_Positive_Now_OtherPlayerWithinFourSteps(
                    game,
                    pathFinder,
                    allMacros
                    );
            if (takePosition_Positive_Now_OtherPlayerWithinFourSteps.Count > 0)
            {
                return takePosition_Positive_Now_OtherPlayerWithinFourSteps;
            }

            // share positive next round
            List<PD_MacroAction> shareKnowledge_Positive_NextRound_Macros =
                PD_AI_MacroAgent_SubPolicies.ShareKnowledge_Positive_NextRound(
                    game,
                    pathFinder,
                    allMacros
                    );
            if (shareKnowledge_Positive_NextRound_Macros.Count > 0)
            {
                return shareKnowledge_Positive_NextRound_Macros;
            }


            return new List<PD_MacroAction>();
        }
    }
}
