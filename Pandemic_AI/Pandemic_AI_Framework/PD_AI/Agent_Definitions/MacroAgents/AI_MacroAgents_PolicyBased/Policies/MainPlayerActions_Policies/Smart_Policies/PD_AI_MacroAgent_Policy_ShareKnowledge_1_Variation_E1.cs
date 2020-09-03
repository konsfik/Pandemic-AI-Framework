using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_ShareKnowledge_1_Variation_E1 : PD_AI_MacroAgent_MainPolicy_Base
    {

        public override List<PD_MacroAction> FilterMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
#if DEBUG
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state.");
            }
#endif

            // share positive now
            List<PD_MacroAction> shareKnowledge_Positive_Now_Macros =
                PD_AI_MacroAgent_SubPolicies.ShareKnowledge_Positive_Now(
                    game,
                    pathFinder,
                    allMacros
                    );
            if (shareKnowledge_Positive_Now_Macros.Count > 0)
            {
                return shareKnowledge_Positive_Now_Macros;
            }

            // share positive nextRound
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

            // take position positive now
            List<PD_MacroAction> takePosition_Positive_Now =
                PD_AI_MacroAgent_SubPolicies.TakePosition_Positive_Now(
                    game,
                    pathFinder,
                    allMacros
                    );
            if (takePosition_Positive_Now.Count > 0)
            {
                return takePosition_Positive_Now;
            }


            return new List<PD_MacroAction>();
        }
    }
}
