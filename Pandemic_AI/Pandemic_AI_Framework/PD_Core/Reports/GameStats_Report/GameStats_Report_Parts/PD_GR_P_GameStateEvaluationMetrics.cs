using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_GR_P_GameStateEvaluationMetrics : PD_Report_Part
    {
        public double PercentCuredDiseases { get; private set; }
        public double PercentAbilityToCureDiseases { get; private set; }
        public double PercentAbilityToCureDiseases_Squared { get; private set; }
        public double PercentCuredDiseases_Gradient { get; private set; }
        public double PercentCuredDiseases_Gradient_Squared { get; private set; }

        public double MinimumPercentAvailableDiseaseCubes { get; private set; }
        public double MultipliedPercentAvailableDiseaseCubes { get; private set; }
        public double AveragePercentAvailableDiseaseCubes { get; private set; }

        public double PercentRemainingOutbreaks { get; private set; }

        public double PercentRemainingPlayerCards { get; private set; }

        public PD_GR_P_GameStateEvaluationMetrics(
            PD_Game game, 
            PD_AI_PathFinder pathFinder
            )
        {
            Update(game, pathFinder);
        }

        public override void Update(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            PercentCuredDiseases =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Percent_CuredDiseases(game);
            PercentAbilityToCureDiseases =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Percent_AbilityToCureDiseases(game, false);
            PercentAbilityToCureDiseases_Squared =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Percent_AbilityToCureDiseases(game, true);
            PercentCuredDiseases_Gradient =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Percent_CuredDiseases_Gradient(game, false);
            PercentCuredDiseases_Gradient_Squared =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Percent_CuredDiseases_Gradient(game, true);

            MinimumPercentAvailableDiseaseCubes =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Minimum_PercentAvailableDiseaseCubes_Min_1(game);
            MultipliedPercentAvailableDiseaseCubes =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Multiplied_Percent_AvailableDiseaseCubes_Min_1(game);
            AveragePercentAvailableDiseaseCubes =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_Average_Percent_AvailableDiseaseCubes_Min_1(game);

            PercentRemainingOutbreaks =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_PercentRemainingOutbreaks_Min_1(game);

            PercentRemainingPlayerCards =
                PD_AI_GameStateEvaluation_HelpMethods.Calculate_PercentRemainingPlayerCards(game);
        }

    }
}
