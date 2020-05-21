using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_ApplyingEpidemicCard : PD_GameStateBase, ICustomDeepCopyable<PD_GS_ApplyingEpidemicCard>
    {
        public PD_GS_ApplyingEpidemicCard()
        {

        }

        public PD_GS_ApplyingEpidemicCard GetCustomDeepCopy()
        {
            return new PD_GS_ApplyingEpidemicCard();
        }

        public override PD_GameStateBase OnCommand(
            PD_Game game,
            PD_GameAction_Base command)
        {
            if (command.GetType() == typeof(PD_ApplyEpidemicCard))
            {
                command.Execute(game);

                // condition checks:
                bool playerHandIncludesEpidemicCard = PD_Game_Queries.GQ_SS_CurrentPlayerHandIncludesEpidemicCard(game);
                bool playerHandBiggerThanPermitted = PD_Game_Queries.GQ_SS_CurrentPlayerHandIsBiggerThanPermitted(game);

                if (PD_Game_Queries.GQ_SS_DeadlyOutbreaks(game) == true)
                {
                    return new PD_GS_GameLost();
                }

                if (PD_Game_Queries.GQ_SS_NotEnoughDiseaseCubestoCompleteAnInfection(game) == true)
                {
                    return new PD_GS_GameLost();
                }

                if (playerHandIncludesEpidemicCard == true)
                {
                    return new PD_GS_ApplyingEpidemicCard();
                }

                if (playerHandBiggerThanPermitted == true)
                {
                    return new PD_GS_Discarding_AfterDrawing();
                }

                return new PD_GS_DrawingNewInfectionCards();
            }
            return null;
        }

        public override void OnEnter(PD_Game game)
        {

        }

        public override void OnExit(PD_Game game)
        {

        }
    }
}