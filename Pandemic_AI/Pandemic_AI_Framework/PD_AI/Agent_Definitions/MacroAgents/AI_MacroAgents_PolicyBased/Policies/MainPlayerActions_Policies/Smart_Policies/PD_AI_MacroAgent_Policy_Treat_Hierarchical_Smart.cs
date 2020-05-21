using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_Treat_Hierarchical_Smart : PD_AI_MacroAgent_MainPolicy_Base
    {
        public PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart Treat_3 { get; private set; }
        public PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart Treat_2 { get; private set; }
        public PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart Treat_1 { get; private set; }

        public PD_AI_MacroAgent_Policy_Treat_Hierarchical_Smart()
        {
            Treat_3 = new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(3);
            Treat_2 = new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(2);
            Treat_1 = new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(1);
        }

        public override List<PD_MacroAction> FilterMacros(PD_Game game, PD_AI_PathFinder pathFinder, List<PD_MacroAction> allMacros)
        {
            if (PD_Game_Queries.GQ_IsInState_ApplyingMainPlayerActions(game) == false)
            {
                throw new System.Exception("wrong state.");
            }

            var treatDiseaseMacros = allMacros.FindAll(
                x => x.Is_TypeOf_TreatDisease_Any()
                );

            if (treatDiseaseMacros.Count == 0) return new List<PD_MacroAction>();

            // TREAT 3
            var treat_3 = Treat_3.FilterMacros(
                game,
                pathFinder,
                allMacros
                );
            if (treat_3.Count > 0) return treat_3;

            // TREAT 2
            var treat_2 = Treat_2.FilterMacros(
                game,
                pathFinder,
                allMacros
                );
            if (treat_2.Count > 0) return treat_2;

            // TREAT 1
            var treat_1 = Treat_1.FilterMacros(
                game,
                pathFinder,
                allMacros
                );
            if (treat_1.Count > 0) return treat_1;

            return new List<PD_MacroAction>();
        }
    }
}
