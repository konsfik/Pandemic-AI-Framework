using System.Collections;

using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_City : IDescribable, ICustomDeepCopyable<PD_City>
    {

        #region properties
        public int ID { get; private set; }
        public int Type { get; private set; }
        public string Name { get; private set; }
        public PD_Point Position { get; private set; }
        #endregion

        #region constructor
        public PD_City(int id, int type, string name, PD_Point position)
        {
            ID = id;
            Type = type;
            Name = name;
            Position = position;
            //_neighbors = new List<PD_City>();
        }
        #endregion

        #region basic methods

        public string GetDescription()
        {
            return String.Format
                (
                "City - ID: {0}, Type: {1}, Name: {2}",
                ID, Type, Name
                );
        }

        public PD_City GetCustomDeepCopy()
        {
            return new PD_City(
                this.ID,
                this.Type,
                this.Name,
                this.Position.GetCustomDeepCopy()
                );
        }
        #endregion

        #region equality overrides

        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            PD_City other = (PD_City)otherObject;

            if (
                this.ID == other.ID &&
                this.Type == other.Type &&
                this.Name == other.Name &&
                this.Position == other.Position
                )
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + ID.GetHashCode();
            hash = hash * 31 + Type.GetHashCode();
            hash = hash * 31 + Name.GetHashCode();
            hash = hash * 31 + Position.GetHashCode();
            return hash;
        }

        public static bool operator ==(PD_City c1, PD_City c2)
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

        public static bool operator !=(PD_City c1, PD_City c2)
        {
            return !(c1 == c2);
        }
        #endregion

    }
}
