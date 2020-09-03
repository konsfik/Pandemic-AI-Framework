using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameElementReferences : PD_GameParts_Base, ICustomDeepCopyable<PD_GameElementReferences>
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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_GameElementReferences)otherObject;

            if (InfectionCards.List_Equals(other.InfectionCards) == false)
            {
                return false;
            }
            else if (CityCards.List_Equals(other.CityCards) == false)
            {
                return false;
            }
            else if (EpidemicCards.List_Equals(other.EpidemicCards) == false)
            {
                return false;
            }
            else if (PlayerPawns.List_Equals(other.PlayerPawns) == false)
            {
                return false;
            }
            else if (RoleCards.List_Equals(other.RoleCards) == false)
            {
                return false;
            }
            else if (ResearchStations.List_Equals(other.ResearchStations) == false)
            {
                return false;
            }
            else if (InfectionCubes.List_Equals(other.InfectionCubes) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + InfectionCards.Custom_HashCode();
            hash = hash * 31 + CityCards.Custom_HashCode();
            hash = hash * 31 + EpidemicCards.Custom_HashCode();
            hash = hash * 31 + PlayerPawns.Custom_HashCode();
            hash = hash * 31 + RoleCards.Custom_HashCode();
            hash = hash * 31 + ResearchStations.Custom_HashCode();
            hash = hash * 31 + InfectionCubes.Custom_HashCode();

            return hash;
        }
        #endregion
    }
}