using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_CityCard : PD_PlayerCardBase, ICustomDeepCopyable<PD_CityCard>
    {
        public PD_City City { get; private set; }

        public string Name { get { return City.Name; } }
        public int Type { get { return City.Type; } }

        [JsonConstructor]
        public PD_CityCard(int id, PD_City city) : base(id)
        {
            City = city;
        }

        public PD_CityCard GetCustomDeepCopy()
        {
            return new PD_CityCard(
                this.ID,
                this.City.GetCustomDeepCopy()
                );
        }

        public override string GetDescription()
        {
            return "." + City.Name;
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (otherObject.GetType() != this.GetType())
            {
                return false;
            }

            var other = (PD_CityCard)otherObject;

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

        public override int GetHashCode()
        {
            int hash = 7;

            hash = (hash * 13) + ID;
            hash = (hash * 13) + City.GetHashCode();

            return hash;
        }
        #endregion

        public override string ToString()
        {
            return String.Format("City Card {0}:{1}", ID, City.ToString());
        }

    }
}
