using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ME_ResearchStation :
        PD_MapElement_Base,
        IEquatable<PD_ME_ResearchStation>,
        ICustomDeepCopyable<PD_ME_ResearchStation>
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
        public bool Equals(PD_ME_ResearchStation other)
        {
            if (this.ID != other.ID)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_ME_ResearchStation other_research_station)
            {
                return Equals(other_research_station);
            }
            else
            {
                return false;
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