using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_Idle : PD_GameStateBase, ICustomDeepCopyable<PD_GS_Idle>
    {
        public PD_GS_Idle()
        {

        }

        public PD_GS_Idle GetCustomDeepCopy()
        {
            return new PD_GS_Idle();
        }

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_GameAction_Base command
            )
        {
            if (command.GetType() == typeof(PD_GA_SetupGame_Random))
            {
                command.Execute(
                    randomness_provider,
                    game
                    );
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