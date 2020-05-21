using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_EpidemicCard : PD_PlayerCardBase, ICustomDeepCopyable<PD_EpidemicCard>
    {

        public PD_EpidemicCard(int id) : base(id)
        {

        }

        public PD_EpidemicCard GetCustomDeepCopy()
        {
            return new PD_EpidemicCard(this.ID);
        }

        public override string GetDescription()
        {
            return "EPIDEMIC";
        }
    }
}