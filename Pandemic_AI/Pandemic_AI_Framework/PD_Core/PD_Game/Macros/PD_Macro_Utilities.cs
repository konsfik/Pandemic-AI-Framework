using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_Macro_Utilities
    {
        public static string GetMacroActionsDescription(
            List<PD_MacroAction> allMacroActions
            )
        {
            string description = "";

            // walk macros
            List<PD_MacroAction> walk_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.Walk_Macro
                );
            List<PD_MacroAction> walk_Macros__SimpleWalk = walk_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                );
            List<PD_MacroAction> walk_Macros__DirectFlightWalk = walk_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                );
            List<PD_MacroAction> walk_Macros__CharterFlightWalk = walk_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                );
            List<PD_MacroAction> walk_Macros__OperationsExpertFlightWalk = walk_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                );

            description +=
                "walkMacros: " + walk_Macros.Count
                + " | simpleWalk: " + walk_Macros__SimpleWalk.Count
                + " | directFlightWalk: " + walk_Macros__DirectFlightWalk.Count
                + " | charterFlightWalk: " + walk_Macros__CharterFlightWalk.Count
                + " | operationsExpertFlightWalk: " + walk_Macros__OperationsExpertFlightWalk.Count
                + "\n";

            // stay macros
            List<PD_MacroAction> stay_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.Stay_Macro
                );
            List<PD_MacroAction> stay_Macros__NoWalk = stay_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.None
                );
            List<PD_MacroAction> stay_Macros__SimpleWalk = stay_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                );
            List<PD_MacroAction> stay_Macros__DirectFlightWalk = stay_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                );
            List<PD_MacroAction> stay_Macros__CharterFlightWalk = stay_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                );
            List<PD_MacroAction> stay_Macros__OperationsExpertFlightWalk = stay_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                );

            description +=
                "stayMacros: " + stay_Macros.Count
                + " | noWalk: " + stay_Macros__SimpleWalk.Count
                + " | simpleWalk: " + stay_Macros__SimpleWalk.Count
                + " | directFlightWalk: " + stay_Macros__DirectFlightWalk.Count
                + " | charterFlightWalk: " + stay_Macros__CharterFlightWalk.Count
                + " | operationsExpertFlightWalk: " + stay_Macros__OperationsExpertFlightWalk.Count
                + "\n";

            // treat disease macros
            List<PD_MacroAction> treatDisease_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                );
            List<PD_MacroAction> treatDisease_Macros__NoWalk = treatDisease_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.None
                );
            List<PD_MacroAction> treatDisease_Macros__SimpleWalk = treatDisease_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                );
            List<PD_MacroAction> treatDisease_Macros__DirectFlightWalk = treatDisease_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                );
            List<PD_MacroAction> treatDisease_Macros__CharterFlightWalk = treatDisease_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                );
            List<PD_MacroAction> treatDisease_Macros__OperationsExpertFlightWalk = treatDisease_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                );

            description +=
                "stayMacros: " + stay_Macros.Count
                + " | noWalk: " + stay_Macros__SimpleWalk.Count
                + " | simpleWalk: " + stay_Macros__SimpleWalk.Count
                + " | directFlightWalk: " + stay_Macros__DirectFlightWalk.Count
                + " | charterFlightWalk: " + stay_Macros__CharterFlightWalk.Count
                + " | operationsExpertFlightWalk: " + stay_Macros__OperationsExpertFlightWalk.Count
                + "\n";

            List<PD_MacroAction> treatDisease_Medic_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro
                );
            List<PD_MacroAction> treatDisease_Medic_Macros__NoWalk = treatDisease_Medic_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.None
                );
            List<PD_MacroAction> treatDisease_Medic_Macros__SimpleWalk = treatDisease_Medic_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk
                );
            List<PD_MacroAction> treatDisease_Medic_Macros__DirectFlightWalk = treatDisease_Medic_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                );
            List<PD_MacroAction> treatDisease_Medic_Macros__CharterFlightWalk = treatDisease_Medic_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                );
            List<PD_MacroAction> treatDisease_Medic_Macros__OperationsExpertFlightWalk = treatDisease_Medic_Macros.FindAll(
                x =>
                x.MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk
                );

            List<PD_MacroAction> autoTreatDisease_Medic_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro
                );
            List<PD_MacroAction> buildResearchStation_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro
                );
            List<PD_MacroAction> buildResearchStation_OperationsExpert_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro
                );
            List<PD_MacroAction> moveResearchStation_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro
                );
            List<PD_MacroAction> shareKnowledge_Give_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro
                );
            List<PD_MacroAction> shareKnowledge_Give_ResearcherGives_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro
                );
            List<PD_MacroAction> shareKnowledge_Take_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_Macro
                );
            List<PD_MacroAction> shareKnowledge_Take_FromResearcher_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro
                );
            List<PD_MacroAction> discoverCure_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Macro
                );
            List<PD_MacroAction> discoverCure_Scientist_Macros = allMacroActions.FindAll(
                x =>
                x.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Scientist_Macro
                );

            return description;
        }

        public static List<PD_CityCard> Find_CityCardsUsedForWalking_In_MacroAction(
            PD_MacroAction macroAction
            )
        {
            return Find_CityCardsUsedForWalking_In_ActionList(macroAction.Actions_All);
        }

        public static List<PD_CityCard> Find_CityCardsUsedForWalking_In_ActionList(
            List<PD_GameAction_Base> actionList
            )
        {
            List<PD_CityCard> cityCardsUsedForWalking = new List<PD_CityCard>();
            foreach (var action in actionList)
            {
                if (action.GetType() == typeof(PD_PMA_DirectFlight))
                {
                    PD_CityCard cityCardUsedForWalking = ((PD_PMA_DirectFlight)action).CityCardToDiscard;
                    cityCardsUsedForWalking.Add(cityCardUsedForWalking);
                }
                else if (action.GetType() == typeof(PD_PMA_CharterFlight))
                {
                    PD_CityCard cityCardUsedForWalking = ((PD_PMA_CharterFlight)action).CityCardToDiscard;
                    cityCardsUsedForWalking.Add(cityCardUsedForWalking);
                }
                else if (action.GetType() == typeof(PD_PMA_OperationsExpert_Flight))
                {
                    PD_CityCard cityCardUsedForWalking = ((PD_PMA_OperationsExpert_Flight)action).CityCardToDiscard;
                    cityCardsUsedForWalking.Add(cityCardUsedForWalking);
                }
            }
            return cityCardsUsedForWalking;
        }
    }
}
