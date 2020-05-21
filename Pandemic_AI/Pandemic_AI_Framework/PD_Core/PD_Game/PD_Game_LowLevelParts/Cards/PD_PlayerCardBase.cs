using System.Collections;
using System.Collections.Generic;
using System;
 

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_PlayerCardBase : PD_Card_Base
    {
        public PD_PlayerCardBase(int id) : base(id)
        {

        }
    }
}