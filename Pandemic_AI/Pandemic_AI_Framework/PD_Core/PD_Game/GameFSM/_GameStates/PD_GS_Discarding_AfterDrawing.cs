using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_Discarding_AfterDrawing :
        PD_GameStateBase, ICustomDeepCopyable<PD_GS_Discarding_AfterDrawing>
    {

        public PD_GS_Discarding_AfterDrawing()
        {

        }

        public PD_GS_Discarding_AfterDrawing GetCustomDeepCopy()
        {
            return new PD_GS_Discarding_AfterDrawing();
        }

        public override PD_GameStateBase OnCommand(
            PD_Game game,
            PD_GameAction_Base command)
        {
            if (command.GetType() == typeof(PD_PA_Discard_AfterDrawing))
            {
                command.Execute(game);

                // condition checks:
                bool playerHandBiggerThanPermitted =
                    PD_Game_Queries.GQ_SS_CurrentPlayerHandIsBiggerThanPermitted(game);

                if (playerHandBiggerThanPermitted == false)
                {
                    return new PD_GS_DrawingNewInfectionCards();
                }
                else
                {
                    return null; // stay in same state
                }
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