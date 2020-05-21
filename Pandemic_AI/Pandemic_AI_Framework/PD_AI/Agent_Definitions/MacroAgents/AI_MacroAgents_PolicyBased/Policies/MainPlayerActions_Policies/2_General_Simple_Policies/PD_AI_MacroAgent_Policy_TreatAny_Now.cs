using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_AI_MacroAgent_Policy_TreatAny_Now : PD_AI_MacroAgent_MainPolicy_Base
    {
        public override List<PD_MacroAction> FilterMacros(
            PD_Game game, 
            PD_AI_PathFinder pathFinder, 
            List<PD_MacroAction> allMacros)
        {
            return allMacros.FindAll(
                x => 
                x.Is_TypeOf_TreatDisease_Any()
                &&
                x.Is_ExecutableNow()
                );
        }
    }
}
