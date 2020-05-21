using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_Player : ICustomDeepCopyable<PD_Player>
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public PD_Player(int id, string name)
        {
            ID = id;
            Name = name;
        }

        #region equality overrides

        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            PD_Player other = (PD_Player)otherObject;

            if (
                this.ID == other.ID &&
                this.Name == other.Name
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
            hash = hash * 31 + Name.GetHashCode();
            return hash;
        }

        public PD_Player GetCustomDeepCopy()
        {
            return new PD_Player(
                this.ID,
                this.Name
                );
        }

        public static bool operator ==(PD_Player p1, PD_Player p2)
        {
            if (object.ReferenceEquals(p1, p2))
            {
                return true;
            }
            if (object.ReferenceEquals(p1, null))
            {
                if (object.ReferenceEquals(p2, null))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (object.ReferenceEquals(p2, null))
                {
                    return false;
                }
            }
            return p1.Equals(p2);
        }

        public static bool operator !=(PD_Player p1, PD_Player p2)
        {
            return !(p1 == p2);
        }
        #endregion
    }
}