using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_CityEdge :
        IEquatable<PD_CityEdge>,
        ICustomDeepCopyable<PD_CityEdge>
    {
        public PD_City City1 { get; private set; }
        public PD_City City2 { get; private set; }

        public PD_CityEdge(PD_City city1, PD_City city2)
        {
            if (city1 == city2)
            {
                throw new System.Exception("node ids must be different");
            }
            City1 = city1;
            City2 = city2;
        }

        public PD_CityEdge GetCustomDeepCopy()
        {
            return new PD_CityEdge(
                City1.GetCustomDeepCopy(),
                City2.GetCustomDeepCopy()
                );
        }

        public bool ContainsCity(PD_City city)
        {
            return City1.ID == city.ID || City2.ID == city.ID;
        }

        public PD_City GetOtherCity(PD_City thisCity)
        {
            if (ContainsCity(thisCity) == false)
            {
                throw new System.Exception("the provided ID is not in the edge");
            }
            if (City1 == thisCity)
                return City2;
            else
                return City1;
        }

        #region equality override
        public bool Equals(PD_CityEdge other)
        {
            if (other.City1 == City1 && other.City2 == City2)
            {
                return true;
            }
            else if (other.City1 == City2 && other.City2 == City1)
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
            if (otherObject is PD_CityEdge other_city)
            {
                return Equals(other_city);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + City1.GetHashCode() + City2.GetHashCode();

            return hash;
        }



        public static bool operator ==(PD_CityEdge edge1, PD_CityEdge edge2)
        {
            return edge1.Equals(edge2);
        }

        public static bool operator !=(PD_CityEdge edge1, PD_CityEdge edge2)
        {
            return !edge1.Equals(edge2);
        }
        #endregion
    }
}
