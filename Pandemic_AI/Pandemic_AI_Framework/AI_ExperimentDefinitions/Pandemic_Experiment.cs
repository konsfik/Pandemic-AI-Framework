using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class Pandemic_Experiment
    {
        public List<PD_Game> AllGames { get; private set; }
        public bool SaveInitialGameStates { get; private set; }
        public Dictionary<PD_AI_Agent_Base, string> NamePerAgent { get; private set; }
        public int NumRepetitionsPerAgent { get; private set; }

        public string ExperimentResults_FolderPath { get; private set; }
        public string ThisExperiment_FolderPath { get; private set; }
        public Dictionary<PD_AI_Agent_Base, string> Report_FilePath_PerAgent { get; private set; }

        public PD_GameState_Report GameState_Report { get; private set; }
        public PD_GameStats_Report GameStats_Report { get; private set; }

        public bool Keep_GameStats_Report { get; private set; }
        public bool Keep_Trace { get; private set; }

        public Pandemic_Experiment(
            List<PD_Game> allGames,
            bool saveInitialGameStates,
            Dictionary<PD_AI_Agent_Base, string> namePerAgent,
            PD_AI_PathFinder pathFinder,
            int numRepetitionsPerAgent,
            string experimentResults_FolderPath,
            bool keep_GameStats_Report,
            bool keep_Trace
            )
        {
            AllGames = allGames;
            SaveInitialGameStates = saveInitialGameStates;

            NamePerAgent = namePerAgent;
            NumRepetitionsPerAgent = numRepetitionsPerAgent;

            GameState_Report = new PD_GameState_Report(AllGames[0], pathFinder);
            GameStats_Report = new PD_GameStats_Report(AllGames[0], pathFinder);

            ExperimentResults_FolderPath = experimentResults_FolderPath;
            if (PD_IO_Utilities.FolderExists(ExperimentResults_FolderPath) == false)
            {
                PD_IO_Utilities.CreateFolder(ExperimentResults_FolderPath, true);
            }

            Keep_GameStats_Report = keep_GameStats_Report;
            Keep_Trace = keep_Trace;

            string thisExperiment_FolderName = DateTime.UtcNow.Ticks.ToString();
            ThisExperiment_FolderPath = Path.Combine(ExperimentResults_FolderPath, thisExperiment_FolderName);
            if (PD_IO_Utilities.FolderExists(ThisExperiment_FolderPath) == false)
            {
                PD_IO_Utilities.CreateFolder(ThisExperiment_FolderPath, true);
            }

            if (SaveInitialGameStates)
            {
                foreach (var game in AllGames)
                {
                    string gameFileName = "game_" + game.UniqueID.ToString() + ".json";
                    string gameFilePath = Path.Combine(ThisExperiment_FolderPath, gameFileName);

                    PD_IO_Utilities.SerializeGameToJsonAndSave(
                        game,
                        false,
                        gameFilePath,
                        false,
                        false
                        );
                }
            }

            Report_FilePath_PerAgent = new Dictionary<PD_AI_Agent_Base, string>();

            List<PD_AI_Agent_Base> allAgents = NamePerAgent.Keys.ToList();
            foreach (var agent in allAgents)
            {
                string agentName = NamePerAgent[agent];

                string agent_Report_FileName = agentName + ".csv";
                string agent_Report_FilePath = Path.Combine(ThisExperiment_FolderPath, agent_Report_FileName);
                Report_FilePath_PerAgent.Add(agent, agent_Report_FilePath);

                string agent_Description_FileName = agentName + ".json";
                string agent_Description_FilePath = Path.Combine(ThisExperiment_FolderPath, agent_Description_FileName);
                PD_IO_Utilities.SerializeToJsonAndSave(agent, agent_Description_FilePath, true, false);
            }

            int counter = 0;
            foreach (var agent in allAgents)
            {
                PD_IO_Utilities.CreateFile(Report_FilePath_PerAgent[agent], false, false);

                PD_IO_Utilities.AppendToFile(
                    Report_FilePath_PerAgent[agent],
                    Get_Record_Header(AllGames[0], pathFinder, agent)
                );

                counter++;
            }
        }

        public void RunExperiment(
            Random randomness_provider,
            PD_AI_PathFinder pathFinder,
            bool displayActions,
            bool displayEndState
            )
        {
            List<PD_AI_Agent_Base> allAgents = NamePerAgent.Keys.ToList();

            for (int agentIndex = 0; agentIndex < allAgents.Count; agentIndex++)
            {
                for (int gameIndex = 0; gameIndex < AllGames.Count; gameIndex++)
                {
                    for (int repetitionIndex = 0; repetitionIndex < NumRepetitionsPerAgent; repetitionIndex++)
                    {
                        var gameCopy = AllGames[gameIndex].GetCustomDeepCopy();
                        var thisAgent = allAgents[agentIndex];

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
                                            pathFinder,
                                            thisAgent,
                                            repetitionIndex
                                            )
                                        );
                                }

                                var nextMacro = ((PD_AI_Macro_Agent_Base)thisAgent).GetNextMacroAction(
                                    randomness_provider,
                                    gameCopy.Request_Fair_ForwardModel(randomness_provider),
                                    pathFinder
                                    );

                                if (displayActions)
                                {
                                    Console.WriteLine(nextMacro.GetDescription());
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

                            if (displayEndState)
                            {
                                Console.WriteLine(PD_Game_Queries.GQ_Is_GameWon(gameCopy) ? "won" : "lost");
                            }

                            string filePath = Report_FilePath_PerAgent[thisAgent];

                            PD_IO_Utilities.AppendToFile(
                                filePath,
                                Get_Record_Row(
                                    gameCopy,
                                    pathFinder,
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
                                        gameCopy.Request_Fair_ForwardModel(randomness_provider)
                                        );

                                if (displayActions)
                                {
                                    Console.WriteLine(nextAction.GetDescription());
                                }

                                gameCopy.ApplySpecificPlayerAction(
                                    randomness_provider,
                                    nextAction
                                    );
                            }

                            if (displayEndState)
                            {
                                Console.WriteLine(PD_Game_Queries.GQ_Is_GameWon(gameCopy) ? "won" : "lost");
                            }

                            string filePath = Report_FilePath_PerAgent[thisAgent];
                            PD_IO_Utilities.AppendToFile(filePath,
                                Get_Record_Row(
                                    gameCopy,
                                    pathFinder,
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

            record_Row = PD_Report_Part.Get_Joined_CSV_Row(report_Parts);

            return record_Row;
        }

    }

}
