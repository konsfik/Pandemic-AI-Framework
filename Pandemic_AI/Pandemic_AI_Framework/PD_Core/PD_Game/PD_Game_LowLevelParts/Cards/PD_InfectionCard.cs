using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_InfectionCard :
        PD_PlayerCardBase, 
        IEquatable<PD_InfectionCard>,
        ICustomDeepCopyable<PD_InfectionCard>
    {
        public int City { get; private set; }

        [JsonConstructor]
        public PD_InfectionCard(int id, int city) : base(id)
        {
            City = city;
        }

        public override string GetDescription()
        {
            return "_" + City.ToString();
        }

        public PD_InfectionCard GetCustomDeepCopy()
        {
            return new PD_InfectionCard(
                this.ID,
                this.City
                );
        }

        #region equality overrides
        public bool Equals(PD_InfectionCard other) {
            if (this.ID != other.ID)
            {
                return false;
            }
            else if (this.City != other.City)
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
            if (other is PD_InfectionCard other_infection_card)
            {
                return Equals(other_infection_card);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object other)
        {
            if (other is PD_InfectionCard other_infection_card)
            {
                return Equals(other_infection_card);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 7;

            hash = (hash * 13) + ID;
            hash = (hash * 13) + City;

            return hash;
        }

        #endregion
        public override string ToString()
        {
            return String.Format("Infection Card {0}:{1}",ID, City.ToString());
        }

        
    }
}
