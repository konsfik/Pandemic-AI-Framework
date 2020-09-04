using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_Point :
        IEquatable<PD_Point>,
        ICustomDeepCopyable<PD_Point>
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public PD_Point()
        {
            X = 0;
            Y = 0;
        }

        [JsonConstructor]
        public PD_Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public PD_Point GetCustomDeepCopy()
        {
            return new PD_Point(
                this.X,
                this.Y
                );
        }


        #region equality overrides
        public bool Equals(PD_Point other)
        {
            if (
                this.X == other.X &&
                this.Y == other.Y
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_Point other_point)
            {
                return Equals(other_point);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + X.GetHashCode();
            hash = hash * 31 + Y.GetHashCode();

            return hash;
        }


        public static bool operator ==(PD_Point p1, PD_Point p2)
        {
            if (ReferenceEquals(p1, null))
            {
                if (ReferenceEquals(p2, null))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (ReferenceEquals(p2, null))
                {
                    return false;
                }
            }
            return p1.Equals(p2);
        }

        public static bool operator !=(PD_Point p1, PD_Point p2)
        {
            return !(p1 == p2);
        }
        #endregion

        public override string ToString()
        {
            return String.Format(
                "point:({0},{1})",
                X, Y
                );
        }
    }
}