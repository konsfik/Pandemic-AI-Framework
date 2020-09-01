using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public interface I_Discard_Action
    {
        PD_PlayerCardBase PlayerCardToDiscard { get; }
    }
}
