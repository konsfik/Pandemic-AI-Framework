using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_RandomAny_NoWalk_AnyTime : PD_AI_MacroAgent_MainPolicy_Base
    {
        public override List<PD_MacroAction> FilterMacros(PD_Game game, PD_AI_PathFinder pathFinder, List<PD_MacroAction> allMacros)
        {
            if (PD_Game_Queries.GQ_IsInState_ApplyingMainPlayerActions(game) == false)
            {
                throw new System.Exception("wrong state.");
            }

            var macros = allMacros.FindAll(
                x =>
                x.MacroAction_Type != PD_MacroAction_Type.Walk_Macro
                );

            if (macros.Count > 0)
            {
                return macros;
            }
            else
            {
                return new List<PD_MacroAction>();
            }
        }
    }
}
