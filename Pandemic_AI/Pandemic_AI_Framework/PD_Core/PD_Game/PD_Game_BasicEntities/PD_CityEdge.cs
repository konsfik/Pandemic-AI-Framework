using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_CityEdge : ICustomDeepCopyable<PD_CityEdge>
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

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            PD_CityEdge edge = (PD_CityEdge)obj;

            if (edge.City1 == City1 && edge.City2 == City2)
            {
                return true;
            }

            if (edge.City1 == City2 && edge.City2 == City1)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + City1.GetHashCode() + City2.GetHashCode();

            return hash;
        }

        public PD_CityEdge GetCustomDeepCopy()
        {
            return new PD_CityEdge(
                City1.GetCustomDeepCopy(),
                City2.GetCustomDeepCopy()
                );
        }

        public static bool operator ==(PD_CityEdge edge1, PD_CityEdge edge2)
        {
            return edge1.Equals(edge2);
        }

        public static bool operator !=(PD_CityEdge edge1, PD_CityEdge edge2)
        {
            return !edge1.Equals(edge2);
        }
    }
}
