using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_CardDiscard_Random : PD_AI_MacroAgent_DiscardPolicy_Base
    {
        public override List<PD_MacroAction> FilterMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
#if DEBUG
            if (game.GQ_IsInState_Discard_Any() == false)
            {
                throw new System.Exception("wrong state");
            }
            if (
                allMacros.Any(
                    x =>
                    x.Is_TypeOf_Discard_Any() == false
                    )
                )
            {
                throw new System.Exception("non discard actions are included here...");
            }
#endif

            return allMacros;
        }
    }
}
