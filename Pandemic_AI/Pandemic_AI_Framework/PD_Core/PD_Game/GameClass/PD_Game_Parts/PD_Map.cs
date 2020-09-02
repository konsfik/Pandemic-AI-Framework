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
    public class PD_Map : ICustomDeepCopyable<PD_Map>
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
    }
}
