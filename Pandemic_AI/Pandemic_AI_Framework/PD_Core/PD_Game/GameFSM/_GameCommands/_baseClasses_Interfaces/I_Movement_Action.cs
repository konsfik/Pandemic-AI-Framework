﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public interface I_Movement_Action
    {
        int FromCity { get; }
        int ToCity { get; }
    }
}
