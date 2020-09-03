using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_Map : PD_GameParts_Base, ICustomDeepCopyable<PD_Map>
    {
        public List<PD_City> Cities { get; private set; }
        public List<PD_CityEdge> CityEdges { get; private set; }
        public Dictionary<int, List<PD_City>> CityNeighbors_PerCityID { get; private set; }

        #region constructors
        // normal constructor
        public PD_Map(
            List<PD_City> cities,
            List<PD_CityEdge> cityEdges
            )
        {
            Cities = cities.CustomDeepCopy();
            CityEdges = cityEdges.CustomDeepCopy();
            ComputeNeighbors();
        }

        // private constructor, for deep copy purposes only... 
        [JsonConstructor]
        public PD_Map(
            List<PD_City> cities,
            List<PD_CityEdge> cityEdges,
            Dictionary<int, List<PD_City>> cityNeighbors_PerCityID
            )
        {
            Cities = cities.CustomDeepCopy();
            CityEdges = cityEdges.CustomDeepCopy();
            CityNeighbors_PerCityID = cityNeighbors_PerCityID.CustomDeepCopy();
        }

        // private constructor, for deepcopy purposes only
        private PD_Map(
            PD_Map mapToCopy
            )
        {
            this.Cities = mapToCopy.Cities.CustomDeepCopy();
            this.CityEdges = mapToCopy.CityEdges.CustomDeepCopy();
            this.CityNeighbors_PerCityID = mapToCopy.CityNeighbors_PerCityID.CustomDeepCopy();
        }
        #endregion

        private void ComputeNeighbors()
        {
            CityNeighbors_PerCityID = new Dictionary<int, List<PD_City>>();
            foreach (var thisCity in Cities)
            {
                var thisCityNeighbors = new List<PD_City>();
                var thisCityEdges = CityEdges.FindAll(
                    x =>
                    x.ContainsCity(thisCity)
                    );
                foreach (var edge in thisCityEdges)
                {
                    var neighbor = edge.GetOtherCity(thisCity);
                    thisCityNeighbors.Add(neighbor);
                }
                CityNeighbors_PerCityID.Add(thisCity.ID, thisCityNeighbors);
            }
        }

        public PD_Map GetCustomDeepCopy()
        {
            return new PD_Map(this);
        }

        #region equalityOverride
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            PD_Map other = (PD_Map)otherObject;

            if (this.Cities.List_Equals(other.Cities) == false)
            {
                return false;
            }
            //else if (this.CityEdges.List_Equals(other.CityEdges) == false)
            //{
            //    return false;
            //}
            else if (this.CityNeighbors_PerCityID.Dictionary_Equal(other.CityNeighbors_PerCityID) == false)
            {
                return false;
            }
            else {
                return true;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + Cities.Custom_HashCode();
            hash = (hash * 13) + CityEdges.Custom_HashCode();
            hash = (hash * 13) + CityNeighbors_PerCityID.Custom_HashCode();

            return hash;
        }
        #endregion
    }
}
