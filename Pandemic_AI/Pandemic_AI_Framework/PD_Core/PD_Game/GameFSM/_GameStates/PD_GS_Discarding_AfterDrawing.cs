using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_Discarding_AfterDrawing :
        PD_GameStateBase, ICustomDeepCopyable<PD_GS_Discarding_AfterDrawing>
    {
        #region constructors
        public PD_GS_Discarding_AfterDrawing()
        {

        }

        public PD_GS_Discarding_AfterDrawing GetCustomDeepCopy()
        {
            return new PD_GS_Discarding_AfterDrawing();
        }
        #endregion

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_GameAction_Base command)
        {
            if (command.GetType() == typeof(PD_PA_Discard_AfterDrawing))
            {
                command.Execute(
                    randomness_provider,
                    game
                    );

                // condition checks:
                bool playerHandBiggerThanPermitted =
                    game.GQ_SS_CurrentPlayerHandIsBiggerThanPermitted();

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

        #region equality override
        public bool Equals(PD_GS_Discarding_AfterDrawing other)
        {
            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_Discarding_AfterDrawing other_state)
            {
                return Equals(other_state);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }


        #endregion
    }
}