using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_DrawingNewPlayerCards : PD_GameStateBase, ICustomDeepCopyable<PD_GS_DrawingNewPlayerCards>
    {

        public PD_GS_DrawingNewPlayerCards()
        {

        }

        public PD_GS_DrawingNewPlayerCards GetCustomDeepCopy()
        {
            return new PD_GS_DrawingNewPlayerCards();
        }

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_GameAction_Base command)
        {
            if (command.GetType() == typeof(PD_DrawNewPlayerCards))
            {
                command.Execute(randomness_provider,game);

                // conditional checking...
                bool playerHandIncludesEpidemicCard = game.GQ_SS_CurrentPlayerHandIncludesEpidemicCard();
                bool playerHandIsBiggerThanPermitted = game.GQ_SS_CurrentPlayerHandIsBiggerThanPermitted();

                if (playerHandIncludesEpidemicCard == true)
                {
                    return new PD_GS_ApplyingEpidemicCard();
                }
                else if (playerHandIncludesEpidemicCard == false)
                {
                    if (playerHandIsBiggerThanPermitted == true)
                    {
                        return new PD_GS_Discarding_AfterDrawing();
                    }
                    else if (playerHandIsBiggerThanPermitted == false)
                    {
                        return new PD_GS_DrawingNewInfectionCards();
                    }
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
        public bool Equals(PD_GS_DrawingNewPlayerCards other)
        {
            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_DrawingNewPlayerCards other_state)
            {
                return Equals(other_state);
            }
            else {
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