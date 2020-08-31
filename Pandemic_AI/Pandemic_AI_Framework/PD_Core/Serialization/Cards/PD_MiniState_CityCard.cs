using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_MiniState_CityCard : PD_MiniState_Card, ICustomDeepCopyable<PD_MiniState_CityCard>
    {
        public readonly int city_id;
        public PD_MiniState_CityCard(int city_id)
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

            var other = (PD_MiniState_CityCard)otherObject;

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

        public PD_MiniState_CityCard GetCustomDeepCopy()
        {
            return new PD_MiniState_CityCard(this.city_id);
        }

    }
}
