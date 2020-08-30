using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public class PD_Mini_CityCard : PD_Mini_Card, ICustomDeepCopyable<PD_Mini_CityCard>
    {
        int city_id;
        public PD_Mini_CityCard(int city_id)
        {
            this.city_id = city_id;
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (otherObject.GetType() != this.GetType())
            {
                return false;
            }

            var other = (PD_Mini_CityCard)otherObject;

            if (this.city_id != other.city_id)
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
            return city_id;
        }
        #endregion

        public PD_Mini_CityCard GetCustomDeepCopy()
        {
            return new PD_Mini_CityCard(this.city_id);
        }

    }
}
