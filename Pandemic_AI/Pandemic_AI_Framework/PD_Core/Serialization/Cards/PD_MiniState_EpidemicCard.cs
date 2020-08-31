using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class PD_MiniState_EpidemicCard : PD_MiniState_Card, ICustomDeepCopyable<PD_MiniState_EpidemicCard>
    {
        public int epidemic_id;

        #region constructors
        public PD_MiniState_EpidemicCard(int epidemic_id)
        {
            this.epidemic_id = epidemic_id;
        }

        public PD_MiniState_EpidemicCard GetCustomDeepCopy()
        {
            return new PD_MiniState_EpidemicCard(this.epidemic_id);
        }
        #endregion

        #region equality override
        public override bool Equals(object otherObject)
        {
            if (otherObject.GetType() != this.GetType())
            {
                return false;
            }

            var other = (PD_MiniState_EpidemicCard)otherObject;

            if (this.epidemic_id != other.epidemic_id)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            return epidemic_id;
        }
        #endregion
    }
}
