using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_AI_MacroAgent_Policy_BuildRS_MaxTotal_MinDistance : PD_AI_MacroAgent_MainPolicy_Base
    {
        public int Max_TotalNum_RS { get; private set; }
        public int Min_Dist_Between_RS { get; private set; }

        public PD_AI_MacroAgent_Policy_BuildRS_MaxTotal_MinDistance(
            int max_TotalNum_RS,
            int min_Dist_Between_RS
            )
        {
            Max_TotalNum_RS = max_TotalNum_RS;
            Min_Dist_Between_RS = min_Dist_Between_RS;
        }

        public override List<PD_MacroAction> FilterMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
#if DEBUG
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state.");
            }

            if (Max_TotalNum_RS < 1 || Max_TotalNum_RS > 6)
            {
                throw new System.Exception("this is not possible");
            }

            if (Min_Dist_Between_RS < 1 || Min_Dist_Between_RS > 10)
            {
                throw new System.Exception("this is not possible");
            }
#endif

            // check if maximum number of RS is already reached
            var rs_cities = game.GQ_ResearchStationCities();
            if (rs_cities.Count >= Max_TotalNum_RS)
            {
                return new List<PD_MacroAction>();
            }

            // find legitimate cities for RS
            var cities_Legitimate_ForRS =
                pathFinder.FindCitiesThatAreAwayFromResearchStations(
                    game,
                    Min_Dist_Between_RS
                    );
            if (cities_Legitimate_ForRS.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            var buildResearchStationMacros_ExecutableNow = allMacros.FindAll(
                x =>
                x.Is_TypeOf_BuildResearchStation_Any()
                &&
                x.Is_ExecutableNow()
                );

            if (buildResearchStationMacros_ExecutableNow.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_MacroAction> nonHarmful_BuildRS_Macros = new List<PD_MacroAction>();
            foreach (var macro in buildResearchStationMacros_ExecutableNow)
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
                    nonHarmful_BuildRS_Macros.Add(macro);
                }
            }

            if (nonHarmful_BuildRS_Macros.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_MacroAction> legit_BuildRS_Macros = new List<PD_MacroAction>();
            foreach (var macro in nonHarmful_BuildRS_Macros)
            {
                PD_City destination = macro.Find_Destination();
                if (cities_Legitimate_ForRS.Contains(destination))
                {
                    legit_BuildRS_Macros.Add(macro);
                }
            }
            if (legit_BuildRS_Macros.Count > 0)
            {
                return legit_BuildRS_Macros;
            }
            else
            {
                return new List<PD_MacroAction>();
            }
        }
    }
}
