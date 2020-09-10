using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GR_P_ActionStats : PD_Report_Part
    {
        public int NumActions_Stay { get; private set; }

        public int NumActions_DriveFerry { get; private set; }
        public int NumActions_DirectFlight { get; private set; }
        public int NumActions_CharterFlight { get; private set; }
        public int NumActions_ShuttleFlight { get; private set; }
        public int NumActions_OperationsExpertFlight { get; private set; }

        public int NumActions_BuildResearchStation { get; private set; }
        public int NumActions_BuildResearchStation_OperationsExpert { get; private set; }
        public int NumActions_MoveResearchStation { get; private set; }

        public int NumActions_TreatDisease { get; private set; }
        public int NumActions_TreatDisease_Medic { get; private set; }
        public int NumActions_TreatDisease_Medic_Auto { get; private set; }

        public int NumActions_ShareKnowledge_Give { get; private set; }
        public int NumActions_ShareKnowledge_Give_ResearcherGives { get; private set; }
        public int NumActions_ShareKnowledge_Take { get; private set; }
        public int NumActions_ShareKnowledge_Take_FromResearcher { get; private set; }

        public int NumActions_DiscoverCure { get; private set; }
        public int NumActions_DiscoverCure_Scientist { get; private set; }

        public int NumActions_Discard_DuringMainPlayerActions { get; private set; }
        public int NumActions_Discard_AfterDrawing { get; private set; }

        public PD_GR_P_ActionStats(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            Update(game, pathFinder);
        }

        public override void Update(PD_Game game, PD_AI_PathFinder pathFinder)
        {
            NumActions_Stay =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_Stay)
                    ).Count;

            NumActions_DriveFerry =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_DriveFerry)
                    ).Count;
            NumActions_DirectFlight =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_DirectFlight)
                    ).Count;
            NumActions_CharterFlight =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_CharterFlight)
                    ).Count;
            NumActions_ShuttleFlight =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_ShuttleFlight)
                    ).Count;
            NumActions_OperationsExpertFlight =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_OperationsExpert_Flight)
                    ).Count;

            NumActions_BuildResearchStation =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_BuildResearchStation)
                    ).Count;
            NumActions_BuildResearchStation_OperationsExpert =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_BuildResearchStation_OperationsExpert)
                    ).Count;
            NumActions_MoveResearchStation =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_MoveResearchStation)
                    ).Count;

            NumActions_TreatDisease =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_TreatDisease)
                    ).Count;
            NumActions_TreatDisease_Medic =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_TreatDisease_Medic)
                    ).Count;
            NumActions_TreatDisease_Medic_Auto =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_TreatDisease_Medic_Auto)
                    ).Count;

            NumActions_ShareKnowledge_Give =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_ShareKnowledge_GiveCard)
                    ).Count;
            NumActions_ShareKnowledge_Give_ResearcherGives =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_ShareKnowledge_GiveCard_ResearcherGives)
                    ).Count;
            NumActions_ShareKnowledge_Take =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_ShareKnowledge_TakeCard)
                    ).Count;
            NumActions_ShareKnowledge_Take_FromResearcher =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_ShareKnowledge_TakeCard_FromResearcher)
                    ).Count;

            NumActions_DiscoverCure =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_DiscoverCure)
                    ).Count;
            NumActions_DiscoverCure_Scientist =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_DiscoverCure_Scientist)
                    ).Count;

            NumActions_Discard_DuringMainPlayerActions =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_Discard_DuringMainPlayerActions)
                    ).Count;
            NumActions_Discard_AfterDrawing =
                game.PlayerActionsHistory.FindAll(
                    x =>
                    x.GetType() == typeof(PA_Discard_AfterDrawing)
                    ).Count;
        }

    }
}
