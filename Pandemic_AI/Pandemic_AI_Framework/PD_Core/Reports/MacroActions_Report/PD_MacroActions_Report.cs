using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_MacroActions_Report : PD_Report_Part
    {
        public int MAR__Num__All_Macros { get; private set; }
        public int MAR__Num__Now { get; private set; }
        public int MAR__Num__Now__NoWalk { get; private set; }
        public int MAR__Num__Now__Walk { get; private set; }
        public int MAR__Num__Later { get; private set; }

        public int MAR__Num__Treat { get; private set; }
        public int MAR__Num__Treat__Now { get; private set; }
        public int MAR__Num__Treat__Later { get; private set; }

        public int MAR__Num__Build_RS { get; private set; }
        public int MAR__Num__Build_RS__Now { get; private set; }
        public int MAR__Num__Build_RS__Later { get; private set; }

        public int MAR__Num__Move_RS { get; private set; }
        public int MAR__Num__Move_RS__Now { get; private set; }
        public int MAR__Num__Move_RS__Later { get; private set; }

        public int MAR__Num__ShareKnowledge { get; private set; }
        public int MAR__Num__ShareKnowledge__Now { get; private set; }
        public int MAR__Num__ShareKnowledge__Later { get; private set; }

        public int MAR__Num__TakePosition { get; private set; }
        public int MAR__Num__TakePosition__Now { get; private set; }
        public int MAR__Num__TakePosition__Later { get; private set; }

        public int MAR__Num__Cure { get; private set; }
        public int MAR__Num__Cure__Now { get; private set; }
        public int MAR__Num__Cure__Later { get; private set; }

        public PD_MacroActions_Report(PD_Game game, PD_AI_PathFinder pathFinder)
        {
            Update(game, pathFinder);
        }

        public override void Update(PD_Game game, PD_AI_PathFinder pathFinder)
        {
            List<PD_MacroAction> allMacros = game.GetAvailableMacros(pathFinder);

            MAR__Num__All_Macros = allMacros.Count;
            MAR__Num__Now = allMacros.FindAll(
                x =>
                x.Is_ExecutableNow() == true
                ).Count;
            MAR__Num__Now__NoWalk = allMacros.FindAll(
                x =>
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_Type != PD_MacroAction_Type.Walk_Macro
                ).Count;
            MAR__Num__Now__Walk = allMacros.FindAll(
                x =>
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_Type == PD_MacroAction_Type.Walk_Macro
                ).Count;
            MAR__Num__Later = allMacros.FindAll(
                x =>
                x.Is_ExecutableNow() == false
                ).Count;

            // treat
            MAR__Num__Treat = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TreatDisease_Any() == true
                ).Count;
            MAR__Num__Treat__Now = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TreatDisease_Any() == true
                &&
                x.Is_ExecutableNow() == true
                ).Count;
            MAR__Num__Treat__Later = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TreatDisease_Any() == true
                &&
                x.Is_ExecutableNow() == false
                ).Count;

            // build research station
            MAR__Num__Build_RS = allMacros.FindAll(
                x =>
                x.Is_TypeOf_BuildResearchStation_Any() == true
                ).Count;
            MAR__Num__Build_RS__Now = allMacros.FindAll(
                x =>
                x.Is_TypeOf_BuildResearchStation_Any() == true
                &&
                x.Is_ExecutableNow() == true
                ).Count;
            MAR__Num__Build_RS__Later = allMacros.FindAll(
                x =>
                x.Is_TypeOf_BuildResearchStation_Any() == true
                &&
                x.Is_ExecutableNow() == false
                ).Count;

            // move research station
            MAR__Num__Move_RS = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro
                ).Count;
            MAR__Num__Move_RS__Now = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro
                &&
                x.Is_ExecutableNow() == true
                ).Count;
            MAR__Num__Move_RS__Later = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro
                &&
                x.Is_ExecutableNow() == false
                ).Count;

            // share knowledge - any
            MAR__Num__ShareKnowledge = allMacros.FindAll(
                x =>
                x.Is_TypeOf_ShareKnowledge_Any() == true
                ).Count;
            MAR__Num__ShareKnowledge__Now = allMacros.FindAll(
                x =>
                x.Is_TypeOf_ShareKnowledge_Any() == true
                &&
                x.Is_ExecutableNow() == true
                ).Count;
            MAR__Num__ShareKnowledge__Later = allMacros.FindAll(
                x =>
                x.Is_TypeOf_ShareKnowledge_Any() == true
                &&
                x.Is_ExecutableNow() == false
                ).Count;

            // take position for share knowledge _ any
            MAR__Num__TakePosition = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TakePositionFor_ShareKnowledge_Any() == true
                ).Count;
            MAR__Num__TakePosition__Now = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TakePositionFor_ShareKnowledge_Any() == true
                &&
                x.Is_ExecutableNow() == true
                ).Count;
            MAR__Num__TakePosition__Later = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TakePositionFor_ShareKnowledge_Any() == true
                &&
                x.Is_ExecutableNow() == false
                ).Count;

            // cure - any
            MAR__Num__Cure = allMacros.FindAll(
                x =>
                x.Is_TypeOf_Cure_Any() == true
                ).Count;
            MAR__Num__Cure__Now = allMacros.FindAll(
                x =>
                x.Is_TypeOf_Cure_Any() == true
                &&
                x.Is_ExecutableNow() == true
                ).Count;
            MAR__Num__Cure__Later = allMacros.FindAll(
                x =>
                x.Is_TypeOf_Cure_Any() == true
                &&
                x.Is_ExecutableNow() == false
                ).Count;

        }

    }
}
