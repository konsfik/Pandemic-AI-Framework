﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_ApplyingEpidemicCard : PD_GameStateBase, ICustomDeepCopyable<PD_GS_ApplyingEpidemicCard>
    {
        #region constructors

        public PD_GS_ApplyingEpidemicCard GetCustomDeepCopy()
        {
            return new PD_GS_ApplyingEpidemicCard();
        }
        #endregion

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_Action command)
        {
            if (command.GetType() == typeof(PA_ApplyEpidemicCard))
            {
                command.Execute(
                    randomness_provider,
                    game
                    );

                // condition checks:
                bool playerHandIncludesEpidemicCard = game.GQ_SS_CurrentPlayerHandIncludesEpidemicCard();
                bool playerHandBiggerThanPermitted = game.GQ_SS_CurrentPlayerHandIsBiggerThanPermitted();

                if (game.GQ_SS_DeadlyOutbreaks() == true)
                {
                    return new PD_GS_GameLost();
                }
                else if (game.GQ_SS_NotEnoughDiseaseCubestoCompleteAnInfection() == true)
                {
                    return new PD_GS_GameLost();
                }
                else if (playerHandIncludesEpidemicCard == true)
                {
                    return new PD_GS_ApplyingEpidemicCard();
                }
                else if (playerHandBiggerThanPermitted == true)
                {
                    return new PD_GS_Discarding_AfterDrawing();
                }
                else
                {
                    return new PD_GS_DrawingNewInfectionCards();
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

        public bool Equals(PD_GS_ApplyingEpidemicCard other)
        {
            return true;
        }
        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_ApplyingEpidemicCard other_action)
            {
                return Equals(other_action);
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