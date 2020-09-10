using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_DrawingNewInfectionCards : PD_GameStateBase, ICustomDeepCopyable<PD_GS_DrawingNewInfectionCards>
    {

        public PD_GS_DrawingNewInfectionCards GetCustomDeepCopy()
        {
            return new PD_GS_DrawingNewInfectionCards();
        }

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_Action command)
        {
            if (command.GetType() == typeof(PA_DrawNewInfectionCards))
            {
                command.Execute(
                    randomness_provider,
                    game
                    );
                return new PD_GS_ApplyingInfectionCards();
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
        public bool Equals(PD_GS_DrawingNewInfectionCards other)
        {
            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_DrawingNewInfectionCards other_action)
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