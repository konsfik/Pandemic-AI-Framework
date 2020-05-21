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
    public class PD_MapElements : ICustomDeepCopyable<PD_MapElements>
    {
        #region properties

        public List<PD_ME_PlayerPawn> InactivePlayerPawns { get; private set; }
        public Dictionary<int, List<PD_ME_PlayerPawn>> PlayerPawnsPerCityID { get; private set; }
        public Dictionary<int, List<PD_ME_InfectionCube>> InactiveInfectionCubesPerType { get; private set; }
        public Dictionary<int, List<PD_ME_InfectionCube>> InfectionCubesPerCityID { get; private set; }
        public List<PD_ME_ResearchStation> InactiveResearchStations { get; private set; }
        public Dictionary<int, List<PD_ME_ResearchStation>> ResearchStationsPerCityID { get; private set; }

        #endregion

        /// <summary>
        /// normal constructor!
        /// </summary>
        /// <param name="players"></param>
        /// <param name="cities"></param>
        public PD_MapElements(
            List<PD_City> cities
            )
        {
            InactivePlayerPawns = new List<PD_ME_PlayerPawn>();
            PlayerPawnsPerCityID = new Dictionary<int, List<PD_ME_PlayerPawn>>();
            foreach (var city in cities)
                PlayerPawnsPerCityID.Add(city.ID, new List<PD_ME_PlayerPawn>());

            InactiveInfectionCubesPerType = new Dictionary<int, List<PD_ME_InfectionCube>>();
            InfectionCubesPerCityID = new Dictionary<int, List<PD_ME_InfectionCube>>();
            foreach (var city in cities)
                InfectionCubesPerCityID.Add(city.ID, new List<PD_ME_InfectionCube>());

            InactiveResearchStations = new List<PD_ME_ResearchStation>();
            ResearchStationsPerCityID = new Dictionary<int, List<PD_ME_ResearchStation>>();
            foreach (var city in cities)
                ResearchStationsPerCityID.Add(city.ID, new List<PD_ME_ResearchStation>());
        }

        /// <summary>
        /// special constructor, for use with the json serializer
        /// </summary>
        /// <param name="inactivePlayerPawns"></param>
        /// <param name="playerPawnsPerCityID"></param>
        /// <param name="inactiveInfectionCubesPerType"></param>
        /// <param name="infectionCubesPerCityID"></param>
        /// <param name="inactiveResearchStations"></param>
        /// <param name="researchStationsPerCityID"></param>
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
            this.InactivePlayerPawns = inactivePlayerPawns;
            this.PlayerPawnsPerCityID = playerPawnsPerCityID;
            this.InactiveInfectionCubesPerType = inactiveInfectionCubesPerType;
            this.InfectionCubesPerCityID = infectionCubesPerCityID;
            this.InactiveResearchStations = inactiveResearchStations;
            this.ResearchStationsPerCityID = researchStationsPerCityID;
        }

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

        public PD_MapElements GetCustomDeepCopy()
        {
            return new PD_MapElements(this);
        }
    }
}