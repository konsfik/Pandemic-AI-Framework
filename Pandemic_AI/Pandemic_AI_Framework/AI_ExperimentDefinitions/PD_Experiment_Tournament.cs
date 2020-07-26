using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_Experiment_Tournament
    {
        public PD_AI_PathFinder PathFinder { get; private set; }
        public List<PD_Game> AllGames { get; private set; }
        public Dictionary<PD_AI_Agent_Base, string> NamePerAgent { get; private set; }
        public int NumRepetitionsPerAgent { get; private set; }

        public string ExperimentResults_ParentFolder_Path { get; private set; }
        public string ThisTournament_Folder_Path { get; private set; }
        public Dictionary<PD_AI_Agent_Base, string> FolderPath_Per_Agent { get; private set; }
        public Dictionary<PD_AI_Agent_Base, string> Report_FilePath_PerAgent { get; private set; }

        public PD_GameState_Report GameState_Report { get; private set; }
        public PD_GameStats_Report GameStats_Report { get; private set; }
        public PD_MacroActions_Report MacroActions_Report { get; private set; }
        public PD_RollingHorizon_Agent_Report RollingHorizonAgent_Report { get; private set; }

        public bool Keep_GameStats_Report { get; private set; }
        public bool Keep_MacroActions_Report { get; private set; }
        public bool Keep_Trace { get; private set; }

        public PD_Experiment_Tournament(
            List<PD_Game> allGames,
            Dictionary<PD_AI_Agent_Base, string> namePerAgent,
            PD_AI_PathFinder pathFinder,
            int numRepetitionsPerAgent,
            string experimentResults_ParentFolder_Path,
            bool keep_GameStats_Report,
            bool keep_MacroActions_Report,
            bool keep_Trace
            )
        {
            PathFinder = new PD_AI_PathFinder();

            AllGames = allGames;
            NamePerAgent = namePerAgent;
            NumRepetitionsPerAgent = numRepetitionsPerAgent;

            GameState_Report = new PD_GameState_Report(AllGames[0], pathFinder);
            GameStats_Report = new PD_GameStats_Report(AllGames[0], pathFinder);
            MacroActions_Report = new PD_MacroActions_Report(AllGames[0], pathFinder);
            RollingHorizonAgent_Report = new PD_RollingHorizon_Agent_Report();

            ExperimentResults_ParentFolder_Path = experimentResults_ParentFolder_Path;
            if (PD_IO_Utilities.FolderExists(ExperimentResults_ParentFolder_Path) == false)
            {
                PD_IO_Utilities.CreateFolder(ExperimentResults_ParentFolder_Path, true);
            }

            Keep_GameStats_Report = keep_GameStats_Report;
            Keep_MacroActions_Report = keep_MacroActions_Report;
            Keep_Trace = keep_Trace;

            string thisTournament_Folder_Name = DateTime.UtcNow.Ticks.ToString();
            ThisTournament_Folder_Path = Path.Combine(ExperimentResults_ParentFolder_Path, thisTournament_Folder_Name);
            if (PD_IO_Utilities.FolderExists(ThisTournament_Folder_Path) == false)
            {
                PD_IO_Utilities.CreateFolder(ThisTournament_Folder_Path, true);
            }

            FolderPath_Per_Agent = new Dictionary<PD_AI_Agent_Base, string>();
            Report_FilePath_PerAgent = new Dictionary<PD_AI_Agent_Base, string>();

            List<PD_AI_Agent_Base> allAgents = NamePerAgent.Keys.ToList();
            foreach (var agent in allAgents)
            {
                string agentName = NamePerAgent[agent];

                string agent_FolderName = agentName;
                string agent_FolderPath = Path.Combine(ThisTournament_Folder_Path, agent_FolderName);
                FolderPath_Per_Agent.Add(agent, agent_FolderPath);
                if (PD_IO_Utilities.FolderExists(agent_FolderPath) == false)
                {
                    PD_IO_Utilities.CreateFolder(agent_FolderPath, true);
                }

                string agent_Report_FileName = agentName + ".csv";
                string agent_Report_FilePath = Path.Combine(agent_FolderPath, agent_Report_FileName);
                Report_FilePath_PerAgent.Add(agent, agent_Report_FilePath);

                string agent_Description_FileName = agentName + ".json";
                string agent_Description_FilePath = Path.Combine(agent_FolderPath, agent_Description_FileName);
                PD_IO_Utilities.SerializeToJsonAndSave(agent, agent_Description_FilePath, true, false);
            }

            int counter = 0;
            foreach (var agent in allAgents)
            {
                PD_IO_Utilities.CreateFile(Report_FilePath_PerAgent[agent], false, false);
                //PD_GameStats_Report tempGameReport = new PD_GameStats_Report(AllGames[0], pathFinder);

                PD_IO_Utilities.AppendToFile(
                    Report_FilePath_PerAgent[agent],
                    Get_Record_Header(AllGames[0], pathFinder, agent)
                );

                counter++;
            }
        }

        public void RunExperiment(
            Random randomness_provider
            )
        {
            List<PD_AI_Agent_Base> allAgents = NamePerAgent.Keys.ToList();

            for (int gameIndex = 0; gameIndex < AllGames.Count; gameIndex++)
            {
                for (int agentIndex = 0; agentIndex < allAgents.Count; agentIndex++)
                {
                    for (int repetitionIndex = 0; repetitionIndex < NumRepetitionsPerAgent; repetitionIndex++)
                    {
                        var gameCopy = AllGames[gameIndex].GetCustomDeepCopy();
                        var thisAgent = allAgents[agentIndex];
                        bool agentIsRollingHorizon = thisAgent.GetType() == typeof(PolicyBased_RHE_Agent);
                        if (agentIsRollingHorizon)
                        {
                            RollingHorizonAgent_Report.Reset();
                        }

                        Console.WriteLine(
                            "game: " + gameIndex
                            + " | agent: " + allAgents[agentIndex].GetType().ToString()
                            + " | repetition: " + repetitionIndex
                            );

                        if (thisAgent.GetType().IsSubclassOf(typeof(PD_AI_Macro_Agent_Base)))
                        {
                            gameCopy.OverrideStartTime();
                            while (PD_Game_Queries.GQ_Is_GameOngoing(gameCopy))
                            {
                                if (Keep_Trace)
                                {
                                    string filePath_ToAppend = Report_FilePath_PerAgent[thisAgent];

                                    PD_IO_Utilities.AppendToFile(
                                        filePath_ToAppend,
                                        Get_Record_Row(
                                            gameCopy,
                                            PathFinder,
                                            thisAgent,
                                            repetitionIndex
                                            )
                                        );
                                }

                                var nextMacro = ((PD_AI_Macro_Agent_Base)thisAgent).GetNextMacroAction(
                                    randomness_provider,
                                    gameCopy,
                                    PathFinder
                                    );

                                if (agentIsRollingHorizon)
                                {
                                    RollingHorizonAgent_Report.Update(
                                        gameCopy,
                                        PathFinder,
                                        (PolicyBased_RHE_Agent)thisAgent
                                        );
                                }

                                foreach (var playerAction in nextMacro.Actions_Executable_Now)
                                {
                                    if (gameCopy.CurrentAvailablePlayerActions.Contains(playerAction))
                                    {

                                        gameCopy.ApplySpecificPlayerAction(
                                            randomness_provider,
                                            playerAction
                                            );
                                    }
                                }
                            }

                            Console.WriteLine(PD_Game_Queries.GQ_Is_GameWon(gameCopy) ? "won" : "lost");

                            string filePath = Report_FilePath_PerAgent[thisAgent];

                            PD_IO_Utilities.AppendToFile(
                                filePath,
                                Get_Record_Row(
                                    gameCopy,
                                    PathFinder,
                                    thisAgent,
                                    repetitionIndex
                                    )
                                );
                        }
                        else if (thisAgent.GetType().IsSubclassOf(typeof(PD_AI_Action_Agent_Base)))
                        {
                            gameCopy.OverrideStartTime();
                            while (PD_Game_Queries.GQ_Is_GameOngoing(gameCopy))
                            {

                                var nextAction =
                                    ((PD_AI_Action_Agent_Base)thisAgent).GetNextAction(
                                        randomness_provider,
                                        gameCopy
                                        );
                                gameCopy.ApplySpecificPlayerAction(
                                    randomness_provider,
                                    nextAction
                                    );
                            }

                            Console.WriteLine(PD_Game_Queries.GQ_Is_GameWon(gameCopy) ? "won" : "lost");

                            string filePath = Report_FilePath_PerAgent[thisAgent];
                            PD_IO_Utilities.AppendToFile(filePath,
                                Get_Record_Row(
                                    gameCopy,
                                    PathFinder,
                                    thisAgent,
                                    repetitionIndex
                                    )
                                );
                        }
                    }
                }
            }
        }

        public string Get_Record_Header(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            PD_AI_Agent_Base agent
            )
        {

            string record_Header = "";

            List<PD_Report_Part> report_Parts = new List<PD_Report_Part>();

            // game state report is included in all cases...
            GameState_Report.Update(game, pathFinder, 0);
            report_Parts.Add(GameState_Report);

            // include game stats report?
            if (Keep_GameStats_Report == true)
            {
                GameStats_Report.Update(game, pathFinder);
                report_Parts.Add(GameStats_Report);
            }
            // include macro actions report?
            if (Keep_MacroActions_Report == true)
            {
                MacroActions_Report.Update(game, pathFinder);
                report_Parts.Add(MacroActions_Report);
            }
            // include rolling horizon agent report?
            bool agentIsRollingHorizon = agent.GetType() == typeof(PolicyBased_RHE_Agent);
            if (agentIsRollingHorizon)
            {
                report_Parts.Add(RollingHorizonAgent_Report);
            }

            record_Header = PD_Report_Part.Get_Joined_CSV_Header(report_Parts);

            return record_Header;
        }

        public string Get_Record_Row(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            PD_AI_Agent_Base agent,
            int repetitionIndex
            )
        {
            string record_Row = "";

            List<PD_Report_Part> report_Parts = new List<PD_Report_Part>();

            // game state report is included in all cases...
            GameState_Report.Update(game, pathFinder, repetitionIndex);
            report_Parts.Add(GameState_Report);

            // include game stats report?
            if (Keep_GameStats_Report == true)
            {
                GameStats_Report.Update(game, pathFinder);
                report_Parts.Add(GameStats_Report);
            }
            // include macro actions report?
            if (Keep_MacroActions_Report == true)
            {
                MacroActions_Report.Update(game, pathFinder);
                report_Parts.Add(MacroActions_Report);
            }
            // include rolling horizon agent report?
            bool agentIsRollingHorizon = agent.GetType() == typeof(PolicyBased_RHE_Agent);
            if (agentIsRollingHorizon)
            {
                report_Parts.Add(RollingHorizonAgent_Report);
            }

            record_Row = PD_Report_Part.Get_Joined_CSV_Row(report_Parts);

            return record_Row;
        }

    }

}
