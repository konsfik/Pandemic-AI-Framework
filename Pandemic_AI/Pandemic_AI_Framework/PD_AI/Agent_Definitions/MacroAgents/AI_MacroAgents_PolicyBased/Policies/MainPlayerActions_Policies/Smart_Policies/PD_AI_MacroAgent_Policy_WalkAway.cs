using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_AI_MacroAgent_Policy_WalkAway : PD_AI_MacroAgent_MainPolicy_Base
    {
        public override List<PD_MacroAction> FilterMacros(PD_Game game, PD_AI_PathFinder pathFinder, List<PD_MacroAction> allMacros)
        {
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state.");
            }

            List<PD_MacroAction> walkMacros = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.Walk_Macro
                );

            if (walkMacros.Count > 0)
            {
                return walkMacros;
            }
            else
            {
                return new List<PD_MacroAction>();
            }
        }
    }
}
