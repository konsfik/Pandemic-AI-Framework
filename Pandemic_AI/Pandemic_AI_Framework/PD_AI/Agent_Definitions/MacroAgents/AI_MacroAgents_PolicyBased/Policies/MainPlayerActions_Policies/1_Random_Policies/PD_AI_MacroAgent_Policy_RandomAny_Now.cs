using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_AI_MacroAgent_Policy_RandomAny_Now : PD_AI_MacroAgent_MainPolicy_Base
    {
        public override List<PD_MacroAction> FilterMacros(PD_Game game, PD_AI_PathFinder pathFinder, List<PD_MacroAction> allMacros)
        {
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state.");
            }

            List<PD_MacroAction> executableNowMacros = allMacros.FindAll(
                x =>
                x.Is_ExecutableNow() == true
                );

            if (executableNowMacros.Count > 0)
            {
                return executableNowMacros;
            }
            else
            {
                return new List<PD_MacroAction>();
            }
        }
    }
}
