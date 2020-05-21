using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_DrawingNewInfectionCards : PD_GameStateBase, ICustomDeepCopyable<PD_GS_DrawingNewInfectionCards>
    {

        public PD_GS_DrawingNewInfectionCards()
        {

        }

        public PD_GS_DrawingNewInfectionCards GetCustomDeepCopy()
        {
            return new PD_GS_DrawingNewInfectionCards();
        }

        public override PD_GameStateBase OnCommand(
            PD_Game game,
            PD_GameAction_Base command)
        {
            if (command.GetType() == typeof(PD_DrawNewInfectionCards))
            {
                command.Execute(game);
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
    }
}