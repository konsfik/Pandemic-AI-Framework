using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_CityCard : PD_PlayerCardBase, ICustomDeepCopyable<PD_CityCard>
    {
        private readonly PD_City _city;

        public PD_City City { get { return _city; } }

        public string Name { get { return _city.Name; } }
        public int Type { get { return _city.Type; } }

        public PD_CityCard(int id, PD_City city) : base(id)
        {
            _city = city;
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
            else if (this._city != other._city)
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
    }
}
