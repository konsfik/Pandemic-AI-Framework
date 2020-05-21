using System.Collections;
using System.Collections.Generic;

namespace Pandemic_AI_Framework
{
    public interface IGameAction: IDescribable
    {
        void Execute(PD_Game game);
    }
}