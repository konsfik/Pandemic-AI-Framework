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
    public class PD_Map :
        PD_GameParts_Base,
        IEquatable<PD_Map>,
        ICustomDeepCopyable<PD_Map>
    {
        public List<PD_City> Cities { get; private set; }
        public Dictionary<int, List<PD_City>> CityNeighbors_PerCityID { get; private set; }

        #region constructors

        // normal / json constructor
        [JsonConstructor]
        public PD_Map(
            List<PD_City> cities,
            Dictionary<int, List<PD_City>> cityNeighbors_PerCityID
            )
        {
            Cities = cities.CustomDeepCopy();
            CityNeighbors_PerCityID = cityNeighbors_PerCityID.CustomDeepCopy();
        }

        // private constructor, for deepcopy purposes only
        private PD_Map(
            PD_Map mapToCopy
            )
        {
            this.Cities = mapToCopy.Cities.CustomDeepCopy();
            this.CityNeighbors_PerCityID = mapToCopy.CityNeighbors_PerCityID.CustomDeepCopy();
        }
        #endregion

        public PD_Map GetCustomDeepCopy()
        {
            return new PD_Map(this);
        }

        #region equalityOverride
        public bool Equals(PD_Map other)
        {
            if (this.Cities.List_Equals(other.Cities) == false)
            {
                return false;
            }
            else if (this.CityNeighbors_PerCityID.Dictionary_Equal(other.CityNeighbors_PerCityID) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_Map other_map)
            {
                return Equals(other_map);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + Cities.Custom_HashCode();
            hash = (hash * 13) + CityNeighbors_PerCityID.Custom_HashCode();

            return hash;
        }


        #endregion
    }
}
