using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_RandomAny_AnyTime : PD_AI_MacroAgent_MainPolicy_Base
    {
        public override List<PD_MacroAction> FilterMacros(PD_Game game, PD_AI_PathFinder pathFinder, List<PD_MacroAction> allMacros)
        {
#if DEBUG
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state.");
            }
#endif

            if (allMacros.Count > 0)
            {
                return allMacros;
            }
            else
            {
                return new List<PD_MacroAction>();
            }
        }
    }
}
