using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_MiniState_InfectionCard: PD_MiniState_Card, ICustomDeepCopyable<PD_MiniState_InfectionCard>
    {
        public int city_id;

        public override string ToString()
        {
            return city_id.ToString();
        }

        public PD_MiniState_InfectionCard(int city_id)
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

            var other = (PD_MiniState_InfectionCard)otherObject;

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

        public PD_MiniState_InfectionCard GetCustomDeepCopy()
        {
            return new PD_MiniState_InfectionCard(this.city_id);
        }
    }
}
