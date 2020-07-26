using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    public interface IGameAction: IDescribable
    {
        void Execute(Random randomness_provider, PD_Game game);
    }
}