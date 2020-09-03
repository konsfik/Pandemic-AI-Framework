using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_CardDiscard_Smart : PD_AI_MacroAgent_DiscardPolicy_Base
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
                throw new System.Exception("game is not in discard state");
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

            PD_Player playerWhoDiscards = ((I_Player_Action)allMacros[0].Actions_All[0]).Player;

            Dictionary<PD_MacroAction, double> effectPerMacroAction = new Dictionary<PD_MacroAction, double>();
            foreach (var macro in allMacros)
            {
                double effect =
                    PD_AI_CardEvaluation_Utilities
                    .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                        game,
                        macro.Actions_Executable_Now,
                        true
                        );
                effectPerMacroAction.Add(macro, effect);
            }

            double maximumValue = effectPerMacroAction.Max(x => x.Value);

            List<PD_MacroAction> maximumValueMacros = new List<PD_MacroAction>();
            foreach (var macro in allMacros)
            {
                if (effectPerMacroAction[macro] == maximumValue)
                {
                    maximumValueMacros.Add(macro);
                }
            }

            return maximumValueMacros;
        }
    }
}
