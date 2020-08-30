using System;
using System.Collections.Generic;
using System.Text;

namespace Pandemic_AI_Framework
{
    public abstract class PD_Mini_Card
    {
        #region equality overrides
        public abstract override bool Equals(object otherObject);
        public abstract override int GetHashCode();

        public static bool operator ==(PD_Mini_Card c1, PD_Mini_Card c2)
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

        public static bool operator !=(PD_Mini_Card c1, PD_Mini_Card c2)
        {
            return !(c1 == c2);
        }

        #endregion
    }
}
