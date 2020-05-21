using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameElementReferences : ICustomDeepCopyable<PD_GameElementReferences>
    {
        public List<PD_InfectionCard> InfectionCards { get; private set; }
        public List<PD_CityCard> CityCards { get; private set; }
        public List<PD_EpidemicCard> EpidemicCards { get; private set; }

        public List<PD_ME_PlayerPawn> PlayerPawns { get; private set; }
        public List<PD_Role_Card> RoleCards { get; private set; }

        public List<PD_ME_ResearchStation> ResearchStations { get; private set; }
        public List<PD_ME_InfectionCube> InfectionCubes { get; private set; }

        /// <summary>
        /// Normal Constructor
        /// also works as a special constructor, for use with the Json serializer,
        /// </summary>
        /// <param name="cityCards"></param>
        /// <param name="infectionCards"></param>
        /// <param name="epidemicCards"></param>
        /// <param name="playerPawns"></param>
        /// <param name="reserchStations"></param>
        /// <param name="infectionCubes"></param>
        [JsonConstructor]
        public PD_GameElementReferences(
            List<PD_CityCard> cityCards,
            List<PD_InfectionCard> infectionCards,
            List<PD_EpidemicCard> epidemicCards,

            List<PD_ME_PlayerPawn> playerPawns,
            List<PD_Role_Card> roleCards,
            List<PD_ME_ResearchStation> researchStations,
            List<PD_ME_InfectionCube> infectionCubes
            )
        {
            CityCards = cityCards.CustomDeepCopy();
            InfectionCards = infectionCards.CustomDeepCopy();
            EpidemicCards = epidemicCards.CustomDeepCopy();

            PlayerPawns = playerPawns.CustomDeepCopy();
            RoleCards = roleCards.CustomDeepCopy();
            ResearchStations = researchStations.CustomDeepCopy();
            InfectionCubes = infectionCubes.CustomDeepCopy();
        }

        /// <summary>
        /// private constructor, for deep copy purposes, only!
        /// </summary>
        /// <param name="gameElementReferencesToCopy"></param>
        private PD_GameElementReferences(
            PD_GameElementReferences gameElementReferencesToCopy
            )
        {
            CityCards = gameElementReferencesToCopy.CityCards.CustomDeepCopy();
            InfectionCards = gameElementReferencesToCopy.InfectionCards.CustomDeepCopy();
            EpidemicCards = gameElementReferencesToCopy.EpidemicCards.CustomDeepCopy();
            PlayerPawns = gameElementReferencesToCopy.PlayerPawns.CustomDeepCopy();
            RoleCards = gameElementReferencesToCopy.RoleCards.CustomDeepCopy();
            ResearchStations = gameElementReferencesToCopy.ResearchStations.CustomDeepCopy();
            InfectionCubes = gameElementReferencesToCopy.InfectionCubes.CustomDeepCopy();
        }

        public PD_GameElementReferences GetCustomDeepCopy()
        {
            return new PD_GameElementReferences(this);
        }
    }
}