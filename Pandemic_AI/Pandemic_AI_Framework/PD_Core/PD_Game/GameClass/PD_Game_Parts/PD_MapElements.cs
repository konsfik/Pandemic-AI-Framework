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

        public List<PD_ME_PlayerPawn> InactivePlayerPawns { get; private set; }
        public Dictionary<int, List<PD_ME_PlayerPawn>> PlayerPawnsPerCityID { get; private set; }
        public Dictionary<int, List<PD_ME_InfectionCube>> InactiveInfectionCubesPerType { get; private set; }
        public Dictionary<int, List<PD_ME_InfectionCube>> InfectionCubesPerCityID { get; private set; }
        public List<PD_ME_ResearchStation> InactiveResearchStations { get; private set; }
        public Dictionary<int, List<PD_ME_ResearchStation>> ResearchStationsPerCityID { get; private set; }

        #endregion

        #region constructors
        // normal constructor
        public PD_MapElements(
            List<int> cities
            )
        {
            InactivePlayerPawns = new List<PD_ME_PlayerPawn>();
            PlayerPawnsPerCityID = new Dictionary<int, List<PD_ME_PlayerPawn>>();
            foreach (var city in cities)
                PlayerPawnsPerCityID.Add(city, new List<PD_ME_PlayerPawn>());

            InactiveInfectionCubesPerType = new Dictionary<int, List<PD_ME_InfectionCube>>();
            InfectionCubesPerCityID = new Dictionary<int, List<PD_ME_InfectionCube>>();
            foreach (var city in cities)
                InfectionCubesPerCityID.Add(city, new List<PD_ME_InfectionCube>());

            InactiveResearchStations = new List<PD_ME_ResearchStation>();
            ResearchStationsPerCityID = new Dictionary<int, List<PD_ME_ResearchStation>>();
            foreach (var city in cities)
                ResearchStationsPerCityID.Add(city, new List<PD_ME_ResearchStation>());
        }

        // special constructor, for use with the json serializer
        [JsonConstructor]
        public PD_MapElements(
            List<PD_ME_PlayerPawn> inactivePlayerPawns,
            Dictionary<int, List<PD_ME_PlayerPawn>> playerPawnsPerCityID,
            Dictionary<int, List<PD_ME_InfectionCube>> inactiveInfectionCubesPerType,
            Dictionary<int, List<PD_ME_InfectionCube>> infectionCubesPerCityID,
            List<PD_ME_ResearchStation> inactiveResearchStations,
            Dictionary<int, List<PD_ME_ResearchStation>> researchStationsPerCityID
            )
        {
            this.InactivePlayerPawns = inactivePlayerPawns.CustomDeepCopy();
            this.PlayerPawnsPerCityID = playerPawnsPerCityID.CustomDeepCopy();
            this.InactiveInfectionCubesPerType = inactiveInfectionCubesPerType.CustomDeepCopy();
            this.InfectionCubesPerCityID = infectionCubesPerCityID.CustomDeepCopy();
            this.InactiveResearchStations = inactiveResearchStations.CustomDeepCopy();
            this.ResearchStationsPerCityID = researchStationsPerCityID.CustomDeepCopy();
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
            this.InactivePlayerPawns = mapElementsToCopy.InactivePlayerPawns.CustomDeepCopy();
            this.PlayerPawnsPerCityID = mapElementsToCopy.PlayerPawnsPerCityID.CustomDeepCopy();
            this.InactiveInfectionCubesPerType = mapElementsToCopy.InactiveInfectionCubesPerType.CustomDeepCopy();
            this.InfectionCubesPerCityID = mapElementsToCopy.InfectionCubesPerCityID.CustomDeepCopy();
            this.InactiveResearchStations = mapElementsToCopy.InactiveResearchStations.CustomDeepCopy();
            this.ResearchStationsPerCityID = mapElementsToCopy.ResearchStationsPerCityID.CustomDeepCopy();
        }

        #region equalityOverride
        public bool Equals(PD_MapElements other)
        {
            if (this.InactivePlayerPawns.List_Equals(
                other.InactivePlayerPawns) == false)
            {
                return false;
            }
            else if (this.PlayerPawnsPerCityID.Dictionary_Equals(
                other.PlayerPawnsPerCityID) == false)
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
            else if (this.InactiveResearchStations.List_Equals(
                other.InactiveResearchStations) == false)
            {
                return false;
            }
            else if (this.ResearchStationsPerCityID.Dictionary_Equals(
                other.ResearchStationsPerCityID) == false)
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

            hash = (hash * 13) + InactivePlayerPawns.Custom_HashCode();
            hash = (hash * 13) + PlayerPawnsPerCityID.Custom_HashCode();
            hash = (hash * 13) + InactiveInfectionCubesPerType.Custom_HashCode();
            hash = (hash * 13) + InfectionCubesPerCityID.Custom_HashCode();
            hash = (hash * 13) + InactiveResearchStations.Custom_HashCode();
            hash = (hash * 13) + ResearchStationsPerCityID.Custom_HashCode();

            return hash;
        }
        #endregion
    }
}