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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (otherObject.GetType() != this.GetType())
            {
                return false;
            }

            var other = (PD_EpidemicCard)otherObject;

            if (this.ID != other.ID)
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
            int hash = 7;

            hash = (hash * 13) + ID;

            return hash;
        }

        #endregion
    }
}