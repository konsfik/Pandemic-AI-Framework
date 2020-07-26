using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    public interface IGameState
    {

        PD_GameStateBase OnCommand(
            Random randomness_provider, 
            PD_Game game, 
            PD_GameAction_Base command
            );
        void OnEnter(PD_Game game);
        void OnExit(PD_Game game);
    }
}