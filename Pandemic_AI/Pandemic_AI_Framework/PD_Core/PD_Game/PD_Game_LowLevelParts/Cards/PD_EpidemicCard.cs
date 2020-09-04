using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_EpidemicCard :
        PD_PlayerCardBase,
        IEquatable<PD_EpidemicCard>,
        ICustomDeepCopyable<PD_EpidemicCard>
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
        public bool Equals(PD_EpidemicCard other)
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

        public override bool Equals(PD_PlayerCardBase other)
        {
            if (other is PD_EpidemicCard other_epidemic_card)
            {
                return Equals(other_epidemic_card);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object other)
        {
            if (other is PD_EpidemicCard other_epidemic_card)
            {
                return Equals(other_epidemic_card);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 7;

            hash = (hash * 13) + ID;

            return hash;
        }


        #endregion
        public override string ToString()
        {
            return String.Format("Epidemic Card {0}", ID);
        }

        
    }
}