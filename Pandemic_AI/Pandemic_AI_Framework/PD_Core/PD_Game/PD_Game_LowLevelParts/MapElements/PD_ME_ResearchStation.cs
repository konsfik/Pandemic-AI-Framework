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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_ME_ResearchStation)otherObject;

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
            int hash = 17;

            hash = hash * 31 + ID.GetHashCode();

            return hash;
        }

        #endregion
    }
}