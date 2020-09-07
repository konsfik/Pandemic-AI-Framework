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
        public Dictionary<int, List<PD_ME_InfectionCube>> InactiveInfectionCubesPerType { get; private set; }
        public Dictionary<int, List<PD_ME_InfectionCube>> InfectionCubesPerCityID { get; private set; }

        public int inactive_research_stations;
        public Dictionary<int, bool> research_stations__per__city;

        #endregion

        #region constructors
        // normal constructor
        public PD_MapElements(
            List<PD_Player> players,
            List<int> cities
            )
        {
            location__per__player = new Dictionary<int, int>();
            foreach (var player in players)
            {
                location__per__player.Add(player.ID, -1);
            }

            InactiveInfectionCubesPerType = new Dictionary<int, List<PD_ME_InfectionCube>>();
            InfectionCubesPerCityID = new Dictionary<int, List<PD_ME_InfectionCube>>();
            foreach (var city in cities)
                InfectionCubesPerCityID.Add(city, new List<PD_ME_InfectionCube>());

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
            Dictionary<int, List<PD_ME_InfectionCube>> inactiveInfectionCubesPerType,
            Dictionary<int, List<PD_ME_InfectionCube>> infectionCubesPerCityID,
            int inactive_research_stations,
            Dictionary<int, bool> research_stations__per__city
            )
        {
            this.location__per__player = location__per__player.CustomDeepCopy();
            this.InactiveInfectionCubesPerType = inactiveInfectionCubesPerType.CustomDeepCopy();
            this.InfectionCubesPerCityID = infectionCubesPerCityID.CustomDeepCopy();
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
            this.location__per__player = mapElementsToCopy.location__per__player.CustomDeepCopy();
            this.InactiveInfectionCubesPerType = mapElementsToCopy.InactiveInfectionCubesPerType.CustomDeepCopy();
            this.InfectionCubesPerCityID = mapElementsToCopy.InfectionCubesPerCityID.CustomDeepCopy();
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
            else if (this.InactiveInfectionCubesPerType.Dictionary_Equals(
                other.InactiveInfectionCubesPerType) == false)
            {
                return false;
            }
            else if (this.InfectionCubesPerCityID.Dictionary_Equals(
                other.InfectionCubesPerCityID) == false)
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
            hash = (hash * 13) + InactiveInfectionCubesPerType.Custom_HashCode();
            hash = (hash * 13) + InfectionCubesPerCityID.Custom_HashCode();
            hash = (hash * 13) + inactive_research_stations;
            hash = (hash * 13) + research_stations__per__city.Custom_HashCode();

            return hash;
        }
        #endregion
    }
}