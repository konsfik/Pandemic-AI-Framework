using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_GameLost : PD_GameStateBase, ICustomDeepCopyable<PD_GS_GameLost>
    {
        public PD_GS_GameLost()
        {

        }

        public PD_GS_GameLost GetCustomDeepCopy()
        {
            return new PD_GS_GameLost();
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
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
        #endregion
    }
}
