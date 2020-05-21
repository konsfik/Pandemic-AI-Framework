using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_Specific_GameSetup_Definition
    {



        public int GameDifficultyLevel { get; private set; }
        public List<PD_Player> Players { get; private set; }
        public List<PD_City> Cities { get; private set; }
        public List<PD_CityEdge> Edges { get; private set; }

        public List<PD_CityCard> AllCityCards { get; private set; }
        public List<PD_InfectionCard> AllInfectionCards { get; private set; }
        public List<PD_EpidemicCard> AllEpidemicCards { get; private set; }

        public List<PD_ME_PlayerPawn> AllPlayerPawns { get; private set; }
        public List<PD_Role_Card> AllRoleCards { get; private set; }
        public List<PD_ME_ResearchStation> AllResearchStations { get; private set; }
        public List<PD_ME_InfectionCube> AllInfectionCubes { get; private set; }

        public PD_Specific_GameSetup_Definition(
            int gameDifficultyLevel,
            List<PD_Player> players,
            List<PD_City> cities,
            List<PD_CityEdge> edges,

            List<PD_CityCard> allCityCards,
            List<PD_InfectionCard> allInfectionCards,
            List<PD_EpidemicCard> allEpidemicCards,

            List<PD_ME_PlayerPawn> allPlayerPawns,
            List<PD_Role_Card> allRoleCards,
            List<PD_ME_ResearchStation> allResearchStations,
            List<PD_ME_InfectionCube> allInfectionCubes
            )
        {
            GameDifficultyLevel = GameDifficultyLevel;
            Players = players;
            Cities = cities;
            Edges = edges;


            Dictionary<int, string> playerName_Per_PlayerID = new Dictionary<int, string>();
            playerName_Per_PlayerID.Add(0, "name1");
        }
    }
}
