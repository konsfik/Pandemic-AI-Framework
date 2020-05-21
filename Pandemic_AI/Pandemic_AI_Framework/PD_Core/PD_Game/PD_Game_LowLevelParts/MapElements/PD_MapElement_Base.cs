using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;


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

        public override bool Equals(object otherObject)
        {
            if (otherObject.GetType() != this.GetType())
            {
                return false;
            }

            var other = (PD_MapElement_Base)otherObject;

            Type type = this.GetType();

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object selfValue = type.GetProperty(pi.Name).GetValue(this, null);
                object otherValue = type.GetProperty(pi.Name).GetValue(other, null);

                if (
                    selfValue != otherValue
                    && (selfValue == null || selfValue.Equals(otherValue) == false)
                    )
                {
                    return false;
                }
            }
            return true;

        }

        public override int GetHashCode()
        {
            int hash = 7;

            Type type = this.GetType();

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object selfValue = type.GetProperty(pi.Name).GetValue(this, null);

                hash += (hash * 13) + selfValue.GetHashCode();
            }

            return hash;
        }



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
