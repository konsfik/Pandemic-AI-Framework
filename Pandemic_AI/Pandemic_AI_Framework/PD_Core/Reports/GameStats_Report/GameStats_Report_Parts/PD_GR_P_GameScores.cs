using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_GR_P_GameScores : PD_Report_Part
    {
        public int Num_CuredDiseases { get; private set; }
        public int Num_EradicatedDiseases { get; private set; }
        public int Num_RemainingPlayerCards { get; private set; }
        public int Num_Outbreaks { get; private set; }
        public int Num_Epidemics { get; private set; }

        public PD_GR_P_GameScores(
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
            Num_RemainingPlayerCards =
                game
                .Cards
                .DividedDeckOfPlayerCards
                .GetNumberOfElementsOfAllSubLists();

            Num_Outbreaks = game.GameStateCounter.OutbreaksCounter;
            Num_Epidemics = game.GameStateCounter.EpidemicsCounter;

            Num_CuredDiseases = 0;
            Num_EradicatedDiseases = 0;
            for (int i = 0; i < 4; i++)
            {
                if (game.GameStateCounter.CureMarkersStates[i] == 1)
                {
                    Num_CuredDiseases++;
                }
                if (game.GameStateCounter.CureMarkersStates[i] == 2)
                {
                    Num_CuredDiseases++;
                    Num_EradicatedDiseases++;
                }
            }
        }
    }
}
