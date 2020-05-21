using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ME_ResearchStation : PD_MapElement_Base, ICustomDeepCopyable<PD_ME_ResearchStation>
    {

        public PD_ME_ResearchStation(int id) : base(id)
        {

        }

        public PD_ME_ResearchStation GetCustomDeepCopy()
        {
            return new PD_ME_ResearchStation(
                this.ID
                );
        }
    }
}