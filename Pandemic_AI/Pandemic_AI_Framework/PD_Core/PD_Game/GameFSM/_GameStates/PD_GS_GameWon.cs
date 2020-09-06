﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_GameWon : PD_GameStateBase, ICustomDeepCopyable<PD_GS_GameWon>
    {

        public PD_GS_GameWon()
        {

        }

        public PD_GS_GameWon GetCustomDeepCopy()
        {
            return new PD_GS_GameWon();
        }

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_GameAction_Base command
            )
        {
            return null;
        }

        public override void OnEnter(PD_Game game)
        {
            game.OverrideEndTime();
        }

        public override void OnExit(PD_Game game)
        {

        }

        #region equality override
        public bool Equals(PD_GS_GameWon other)
        {
            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_GameWon other_state)
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