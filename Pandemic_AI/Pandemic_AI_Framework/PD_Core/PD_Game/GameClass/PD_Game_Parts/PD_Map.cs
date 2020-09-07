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
        public int number_of_cities;
        public List<int> cities;
        public Dictionary<int, string> name__per__city;
        public Dictionary<int, PD_Point> position__per__city;
        public Dictionary<int, int> infection_type__per__city;
        public Dictionary<int, List<int>> neighbors__per__city;

        #region constructors

        // normal / json constructor
        [JsonConstructor]
        public PD_Map(
            int number_of_cities,
            List<int> cities,
            Dictionary<int, string> name__per__city,
            Dictionary<int, PD_Point> position__per__city,
            Dictionary<int, int> infection_type__per__city,
            Dictionary<int, List<int>> neighbors__per__city
            )
        {
            this.number_of_cities = number_of_cities;
            this.cities = cities.CustomDeepCopy();
            this.name__per__city = name__per__city.CustomDeepCopy(); ;
            this.position__per__city = position__per__city.CustomDeepCopy(); ;
            this.infection_type__per__city = infection_type__per__city.CustomDeepCopy(); ;
            this.neighbors__per__city = neighbors__per__city.CustomDeepCopy(); ;
        }

        // private constructor, for deepcopy purposes only
        private PD_Map(
            PD_Map mapToCopy
            )
        {
            this.number_of_cities = mapToCopy.number_of_cities;
            this.cities = mapToCopy.cities.CustomDeepCopy();
            this.name__per__city = mapToCopy.name__per__city.CustomDeepCopy();
            this.position__per__city = mapToCopy.position__per__city.CustomDeepCopy();
            this.infection_type__per__city = mapToCopy.infection_type__per__city.CustomDeepCopy();
            this.neighbors__per__city = mapToCopy.neighbors__per__city.CustomDeepCopy();
        }
        #endregion

        public PD_Map GetCustomDeepCopy()
        {
            return new PD_Map(this);
        }

        #region equalityOverride
        public bool Equals(PD_Map other)
        {
            if (this.number_of_cities != other.number_of_cities)
            {
                return false;
            }
            else if (this.cities.List_Equals(other.cities) == false)
            {
                return false;
            }
            else if (this.name__per__city.Dictionary_Equal(other.name__per__city) == false)
            {
                return false;
            }
            else if (this.position__per__city.Dictionary_Equals(other.position__per__city) == false)
            {
                return false;
            }
            else if (this.infection_type__per__city.Dictionary_Equals(other.infection_type__per__city) == false)
            {
                return false;
            }
            else if (this.neighbors__per__city.Dictionary_Equal(other.neighbors__per__city) == false)
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

            hash = (hash * 13) + number_of_cities;
            hash = (hash * 13) + cities.Custom_HashCode();
            hash = (hash * 13) + name__per__city.Custom_HashCode();
            hash = (hash * 13) + position__per__city.Custom_HashCode();
            hash = (hash * 13) + infection_type__per__city.Custom_HashCode();
            hash = (hash * 13) + neighbors__per__city.Custom_HashCode();

            return hash;
        }


        #endregion
    }
}
