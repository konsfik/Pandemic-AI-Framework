using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_Cure_ASAP : PD_AI_MacroAgent_MainPolicy_Base
    {
        public override List<PD_MacroAction> FilterMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state.");
            }

            var cureMacros = allMacros.FindAll(
                x =>
                x.Is_TypeOf_Cure_Any()
                );

            if (cureMacros.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            int smallestWalkLength = cureMacros.Min(
                x =>
                x.Count_Walk_Length()
                );

            var smallestWalkLengthCureMacros = cureMacros.FindAll(
                x =>
                x.Count_Walk_Length() == smallestWalkLength
                );

            if (smallestWalkLengthCureMacros.Count > 0)
            {
                return smallestWalkLengthCureMacros;
            }
            else
            {
                return new List<PD_MacroAction>();
            }
        }
    }
}
