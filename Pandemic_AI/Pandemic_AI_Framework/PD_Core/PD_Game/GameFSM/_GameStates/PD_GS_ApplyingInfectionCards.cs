﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_ApplyingInfectionCards : PD_GameStateBase, ICustomDeepCopyable<PD_GS_ApplyingInfectionCards>
    {
        #region constructors
        public PD_GS_ApplyingInfectionCards()
        {

        }

        public PD_GS_ApplyingInfectionCards GetCustomDeepCopy()
        {
            return new PD_GS_ApplyingInfectionCards();
        }
        #endregion

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_GameAction_Base command
            )
        {
            if (command.GetType() == typeof(PD_ApplyInfectionCard))
            {
                command.Execute(
                    randomness_provider,
                    game
                    );

                bool thereAreMoreActiveInfectionCards = game.GQ_SS_ThereAreActiveInfectionCards();

                // check if game is lost, etc..
                if (game.GQ_SS_NotEnoughDiseaseCubestoCompleteAnInfection())
                    return new PD_GS_GameLost();
                if (game.GQ_SS_DeadlyOutbreaks())
                    return new PD_GS_GameLost();
                if (game.GQ_SS_ThereAreActiveInfectionCards())
                    return null;

                game.game_state_counter.IncreaseCurrentPlayerIndex();
                return new PD_GS_ApplyingMainPlayerActions();
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
        public bool Equals(PD_GS_ApplyingInfectionCards other)
        {
            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_ApplyingInfectionCards other_action)
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