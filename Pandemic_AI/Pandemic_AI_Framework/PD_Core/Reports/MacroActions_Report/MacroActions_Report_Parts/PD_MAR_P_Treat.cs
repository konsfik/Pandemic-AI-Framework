using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_MAR_P_Treat : PD_MAR_P_Base
    {
        public int Num__Treat { get; private set; }

        public int Num__Treat__Now { get; private set; }
        public int Num__Treat__Later { get; private set; }

        public int Num__Treat__Now__SimpleWalk { get; private set; }
        public int Num__Treat__Now__DirectFlightWalk { get; private set; }
        public int Num__Treat__Now__CharterFlightWalk { get; private set; }
        public int Num__Treat__Now__OperationsExpertFlightWalk { get; private set; }

        public int Num__Treat__Later__SimpleWalk { get; private set; }
        public int Num__Treat__Later__DirectFlightWalk { get; private set; }
        public int Num__Treat__Later__CharterFlightWalk { get; private set; }
        public int Num__Treat__Later__OperationsExpertFlightWalk { get; private set; }

        public PD_MAR_P_Treat(PD_Game game, List<PD_MacroAction> allMacros)
        {
            Update(game, allMacros);
        }

        public override void Update(PD_Game game, List<PD_MacroAction> allMacros)
        {
            Num__Treat = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                ).Count;

            Num__Treat__Now = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == true
                ).Count;

            Num__Treat__Later = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == false
                ).Count;

            Num__Treat__Now__SimpleWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                ).Count;

            Num__Treat__Now__DirectFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                ).Count;

            Num__Treat__Now__CharterFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                ).Count;

            Num__Treat__Now__OperationsExpertFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                ).Count;

            Num__Treat__Later__SimpleWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                ).Count;

            Num__Treat__Later__DirectFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                ).Count;

            Num__Treat__Later__CharterFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                ).Count;

            Num__Treat__Later__OperationsExpertFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                ).Count;
        }
    }
}
