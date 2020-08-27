using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_MapElement_Base
    {
        public int ID { get; protected set; }

        public PD_MapElement_Base(int id)
        {
            ID = id;
        }

        #region equality overrides
        public abstract override bool Equals(object otherObject);
        public abstract override int GetHashCode();

        public static bool operator ==(PD_MapElement_Base c1, PD_MapElement_Base c2)
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

        public static bool operator !=(PD_MapElement_Base c1, PD_MapElement_Base c2)
        {
            return !(c1 == c2);
        }

        #endregion
    }
}
