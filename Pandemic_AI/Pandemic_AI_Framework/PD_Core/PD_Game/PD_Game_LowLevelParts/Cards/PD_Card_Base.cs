using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_Card_Base : IDescribable
    {
        private readonly int _id;

        public int ID { get { return _id; } }

        public PD_Card_Base(int id)
        {
            _id = id;
        }

        public abstract string GetDescription();

        #region equality overrides
        public abstract override bool Equals(object otherObject);
        public abstract override int GetHashCode();

        public static bool operator ==(PD_Card_Base c1, PD_Card_Base c2)
        {
            if (Object.ReferenceEquals(c1, null))
            {
                if (Object.ReferenceEquals(c2, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // c1 is not null
            {
                if (Object.ReferenceEquals(c2, null)) // c2 is null
                {
                    return false;
                }
            }
            return c1.Equals(c2);
        }

        public static bool operator !=(PD_Card_Base c1, PD_Card_Base c2)
        {
            return !(c1 == c2);
        }

        #endregion
    }
}