using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_MAR_P_AutoTreat_Medic : PD_MAR_P_Base
    {
        public int Num__AutoTreat_Medic { get; private set; }

        public int Num__AutoTreat_Medic__Now { get; private set; }
        public int Num__AutoTreat_Medic__Later { get; private set; }

        public int Num__AutoTreat_Medic__Now__SimpleWalk { get; private set; }
        public int Num__AutoTreat_Medic__Now__DirectFlightWalk { get; private set; }
        public int Num__AutoTreat_Medic__Now__CharterFlightWalk { get; private set; }
        public int Num__AutoTreat_Medic__Now__OperationsExpertFlightWalk { get; private set; }

        public int Num__AutoTreat_Medic__Later__SimpleWalk { get; private set; }
        public int Num__AutoTreat_Medic__Later__DirectFlightWalk { get; private set; }
        public int Num__AutoTreat_Medic__Later__CharterFlightWalk { get; private set; }
        public int Num__AutoTreat_Medic__Later__OperationsExpertFlightWalk { get; private set; }

        public PD_MAR_P_AutoTreat_Medic(PD_Game game, List<PD_MacroAction> allMacros)
        {
            Update(game, allMacros);
        }

        public override void Update(PD_Game game, List<PD_MacroAction> allMacros)
        {
            Num__AutoTreat_Medic = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                ).Count;

            Num__AutoTreat_Medic__Now = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == true
                ).Count;

            Num__AutoTreat_Medic__Later = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == false
                ).Count;

            Num__AutoTreat_Medic__Now__SimpleWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                ).Count;

            Num__AutoTreat_Medic__Now__DirectFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                ).Count;

            Num__AutoTreat_Medic__Now__CharterFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                ).Count;

            Num__AutoTreat_Medic__Now__OperationsExpertFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == true
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                ).Count;

            Num__AutoTreat_Medic__Later__SimpleWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                ).Count;

            Num__AutoTreat_Medic__Later__DirectFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                ).Count;

            Num__AutoTreat_Medic__Later__CharterFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                ).Count;

            Num__AutoTreat_Medic__Later__OperationsExpertFlightWalk = allMacros.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                &&
                x.Is_ExecutableNow() == false
                &&
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                ).Count;
        }
    }
}
