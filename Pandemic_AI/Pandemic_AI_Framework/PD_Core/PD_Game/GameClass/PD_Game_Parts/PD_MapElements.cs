using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// All the relations of type: This is IN this Position
    /// </summary>
    [Serializable]
    public class PD_MapElements :
        PD_GameParts_Base,
        IEquatable<PD_MapElements>,
        ICustomDeepCopyable<PD_MapElements>
    {
        #region properties

        public Dictionary<int, int> location__per__player;

        public Dictionary<int, int> inactive_infection_cubes__per__type;

        public Dictionary<int, Dictionary<int, int>> infections__per__type__per__city;

        public int inactive_research_stations;
        public Dictionary<int, bool> research_stations__per__city;

        #endregion

        #region constructors
        // normal constructor
        public PD_MapElements(
            List<int> players,
            List<int> cities
            )
        {
            location__per__player = new Dictionary<int, int>();
            foreach (var player in players)
            {
                location__per__player.Add(player, -1);
            }

            inactive_infection_cubes__per__type = new Dictionary<int, int>();
            infections__per__type__per__city = new Dictionary<int, Dictionary<int, int>>();
            foreach (int city in cities)
            {
                infections__per__type__per__city.Add(city, new Dictionary<int, int>());
                for (int t = 0; t < 4; t++)
                {
                    infections__per__type__per__city[city].Add(t, 0);
                }
            }

            inactive_research_stations = 6;
            research_stations__per__city = new Dictionary<int, bool>();
            foreach (int city in cities)
            {
                research_stations__per__city.Add(city, false);
            }
        }

        // special constructor, for use with the json serializer
        [JsonConstructor]
        public PD_MapElements(
            Dictionary<int, int> location__per__player,
            Dictionary<int, int> inactive_infection_cubes__per__type,
            Dictionary<int, Dictionary<int, int>> infections__per__type__per__city,
            int inactive_research_stations,
            Dictionary<int, bool> research_stations__per__city
            )
        {
            this.location__per__player = location__per__player.CustomDeepCopy();
            this.inactive_infection_cubes__per__type = inactive_infection_cubes__per__type.CustomDeepCopy();
            this.infections__per__type__per__city = infections__per__type__per__city.CustomDeepCopy();
            this.inactive_research_stations = inactive_research_stations;
            this.research_stations__per__city = research_stations__per__city;
        }

        public PD_MapElements GetCustomDeepCopy()
        {
            return new PD_MapElements(this);
        }
        #endregion

        /// <summary>
        /// private constructor, for use with custom deep copy, only!
        /// </summary>
        /// <param name="mapElementsToCopy"></param>
        private PD_MapElements(
            PD_MapElements mapElementsToCopy
            )
        {
            this.location__per__player 
                = mapElementsToCopy.location__per__player.CustomDeepCopy();
            this.inactive_infection_cubes__per__type
                = mapElementsToCopy.inactive_infection_cubes__per__type.CustomDeepCopy();
            this.infections__per__type__per__city
                = mapElementsToCopy.infections__per__type__per__city.CustomDeepCopy();
            this.inactive_research_stations = mapElementsToCopy.inactive_research_stations;
            this.research_stations__per__city = mapElementsToCopy.research_stations__per__city.CustomDeepCopy();
        }

        #region equalityOverride
        public bool Equals(PD_MapElements other)
        {
            if (this.location__per__player.Dictionary_Equals(
                other.location__per__player) == false)
            {
                return false;
            }
            else if (this.inactive_infection_cubes__per__type.Dictionary_Equals(
                other.inactive_infection_cubes__per__type) == false)
            {
                return false;
            }
            else if (this.infections__per__type__per__city.Dictionary_Equals(
                other.infections__per__type__per__city) == false)
            {
                return false;
            }
            else if (this.inactive_research_stations
                != other.inactive_research_stations)
            {
                return false;
            }
            else if (this.research_stations__per__city.Dictionary_Equals(
                other.research_stations__per__city) == false)
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
            if (otherObject is PD_MapElements other_map_elments)
            {
                return Equals(other_map_elments);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + location__per__player.Custom_HashCode();
            hash = (hash * 13) + inactive_infection_cubes__per__type.Custom_HashCode();
            hash = (hash * 13) + infections__per__type__per__city.Custom_HashCode();
            hash = (hash * 13) + inactive_research_stations;
            hash = (hash * 13) + research_stations__per__city.Custom_HashCode();

            return hash;
        }
        #endregion
    }
}