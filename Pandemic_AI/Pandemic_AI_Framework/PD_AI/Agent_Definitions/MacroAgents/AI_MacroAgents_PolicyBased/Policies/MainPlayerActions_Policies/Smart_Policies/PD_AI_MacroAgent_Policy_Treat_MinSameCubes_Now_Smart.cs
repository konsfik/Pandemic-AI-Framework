using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart : PD_AI_MacroAgent_MainPolicy_Base
    {
        public int MinSameCubes { get; private set; }

        public PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(
            int minSameCubes
            )
        {
            MinSameCubes = minSameCubes;
        }

        public override List<PD_MacroAction> FilterMacros(PD_Game game, PD_AI_PathFinder pathFinder, List<PD_MacroAction> allMacros)
        {
            if (PD_Game_Queries.GQ_IsInState_ApplyingMainPlayerActions(game) == false)
            {
                throw new System.Exception("wrong state.");
            }

            var allTreatMacros_ExecutableNow = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TreatDisease_Any()
                &&
                x.Is_ExecutableNow()
                );

            if (allTreatMacros_ExecutableNow.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_MacroAction> noNegativeEffect_TreatMacros = new List<PD_MacroAction>();
            foreach (var macro in allTreatMacros_ExecutableNow)
            {
                double value =
                    PD_AI_CardEvaluation_Utilities
                    .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                        game,
                        macro.Actions_Executable_Now,
                        true
                        );
                if (value == 0)
                {
                    noNegativeEffect_TreatMacros.Add(macro);
                }
            }

            if (noNegativeEffect_TreatMacros.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_MacroAction> treatMacros_ofCities_InfectedWith_RequestedNumCubes = new List<PD_MacroAction>();
            foreach (var macro in noNegativeEffect_TreatMacros)
            {
                if (
                    macro.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                    )
                {
                    PD_PA_TreatDisease treatAction = (PD_PA_TreatDisease)macro.Find_LastCommand();
                    PD_City destination = treatAction.CityToTreatDiseaseAt;
                    int treatType = treatAction.TypeOfDiseaseToTreat;
                    int numCubesThisTypeOnThatCity =
                        PD_Game_Queries.GQ_Count_Num_InfectionCubes_OfType_OnCity(
                            game,
                            destination,
                            treatType
                            );
                    if (numCubesThisTypeOnThatCity >= MinSameCubes)
                    {
                        treatMacros_ofCities_InfectedWith_RequestedNumCubes.Add(macro);
                    }
                }
                else if (
                    macro.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro
                    )
                {
                    PD_PA_TreatDisease_Medic treatAction = (PD_PA_TreatDisease_Medic)macro.Find_LastCommand();
                    PD_City destination = treatAction.CityToTreatDiseaseAt;
                    int treatType = treatAction.TypeOfDiseaseToTreat;
                    int numCubesThisTypeOnThatCity =
                        PD_Game_Queries.GQ_Count_Num_InfectionCubes_OfType_OnCity(
                            game,
                            destination,
                            treatType
                            );
                    if (numCubesThisTypeOnThatCity >= MinSameCubes)
                    {
                        treatMacros_ofCities_InfectedWith_RequestedNumCubes.Add(macro);
                    }
                }
                else if (
                    macro.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                    )
                {
                    PD_City destination = macro.Find_Destination();
                    int treatType = destination.Type;
                    int numCubesThisTypeOnThatCity =
                        PD_Game_Queries.GQ_Count_Num_InfectionCubes_OfType_OnCity(
                            game,
                            destination,
                            treatType
                            );
                    if (numCubesThisTypeOnThatCity >= MinSameCubes)
                    {
                        treatMacros_ofCities_InfectedWith_RequestedNumCubes.Add(macro);
                    }
                }

            }

            if (treatMacros_ofCities_InfectedWith_RequestedNumCubes.Count > 0)
            {
                return treatMacros_ofCities_InfectedWith_RequestedNumCubes;
            }
            else
            {
                return new List<PD_MacroAction>();
            }
        }
    }
}
