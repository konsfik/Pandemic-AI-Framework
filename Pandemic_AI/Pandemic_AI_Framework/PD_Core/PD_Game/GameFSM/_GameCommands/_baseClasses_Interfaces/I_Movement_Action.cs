using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public interface I_Movement_Action
    {
        PD_City InitialLocation { get; }
        PD_City TargetLocation { get; }
    }
}
