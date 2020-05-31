using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;
using System.IO;
using Experiments_Runner;

namespace Experiment_1
{
    public class Experiments_Runner
    {
        static void Main(string[] args)
        {

            //Experiment_1();
            Experiment_2();

        }

        /// <summary>
        /// Experiment 1:
        /// - Random action agent
        /// - 10 random games (four players, easy mode)
        /// - 10 repetitions per game.
        /// The experiment saves a complete report, describing the end - state of each one of the 100 games
        /// in the form of a .csv file, for further analysis.
        /// </summary>
        public static void Experiment_1()
        {
            string gameCreationData = DataUtilities.Read_GameCreationData();

            // generate a set of random games,
            // of four players, set to easy level
            List<PD_Game> games = Generate_Random_Games(10, 4, 0);

            // initialize the pathFinder
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder(games[0]);

            // define the agent and the dictionary for the experiment runner
            PD_AI_ActionAgent_Random randomActionAgent = new PD_AI_ActionAgent_Random();
            Dictionary<PD_AI_Agent_Base, string> agentsDictionary = new Dictionary<PD_AI_Agent_Base, string>() {
                { randomActionAgent, "random_action_agent" }
            };

            // prepare the experiment results directory
            string experimentResults_Directory = Directory.GetCurrentDirectory() + "\\ExperimentResults";
            if (PD_IO_Utilities.FolderExists(experimentResults_Directory) == false)
            {
                PD_IO_Utilities.CreateFolder(experimentResults_Directory, true);
            }

            Pandemic_Experiment experiment = new Pandemic_Experiment(
                games,
                true,   // save the initial game states (serialized as json files) for later review, or repetition
                agentsDictionary,
                pathFinder,
                100, // number of repetitions per game
                experimentResults_Directory,
                true,   // keep game stats report?
                false   // keep trace?
                );

            experiment.RunExperiment(
                pathFinder,
                true,
                true
                );
        }

        /// <summary>
        /// Experiment 2:
        /// - Hierarchical Policy Agent (HPA)
        /// - 10 random games (four players, easy mode)
        /// - 10 repetitions per game.
        /// The experiment saves a complete report, describing the end - state of each one of the 100 games
        /// in the form of a .csv file, for further analysis.
        /// </summary>
        public static void Experiment_2()
        {
            string gameCreationData = DataUtilities.Read_GameCreationData();

            // generate a set of random games,
            // of four players, set to easy level
            List<PD_Game> games = Generate_Random_Games(10, 4, 0);

            // initialize the pathFinder
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder(games[0]);

            // define the agent and the dictionary for the experiment runner
            var hierarchicalPolicyAgent = AgentsFactory.HierarchicalPolicyAgent();
            Dictionary<PD_AI_Agent_Base, string> agentsDictionary = new Dictionary<PD_AI_Agent_Base, string>() {
                { hierarchicalPolicyAgent, "HPA" }
            };

            // prepare the experiment results directory
            string experimentResults_Directory = Directory.GetCurrentDirectory() + "\\ExperimentResults";
            if (PD_IO_Utilities.FolderExists(experimentResults_Directory) == false)
            {
                PD_IO_Utilities.CreateFolder(experimentResults_Directory, true);
            }

            Pandemic_Experiment experiment = new Pandemic_Experiment(
                games,
                true,   // save the initial game states (serialized as json files) for later review, or repetition
                agentsDictionary,
                pathFinder,
                100, // number of repetitions per game
                experimentResults_Directory,
                true,   // keep game stats report?
                false   // keep trace?
                );

            experiment.RunExperiment(
                pathFinder,
                true,
                true
                );
        }


        public static PD_Game Generate_Random_Game(
            int numberOfPlayers,
            int gameDifficulty
            )
        {
            string data = DataUtilities.Read_GameCreationData();
            return PD_GameCreator.CreateNewGame(numberOfPlayers, gameDifficulty, data, true);
        }

        public static List<PD_Game> Generate_Random_Games(
            int numberOfGames,
            int numberOfPlayers,
            int gameDifficulty
            )
        {
            string data = DataUtilities.Read_GameCreationData();
            List<PD_Game> games = new List<PD_Game>();
            for (int i = 0; i < numberOfGames; i++)
            {
                games.Add(PD_GameCreator.CreateNewGame(numberOfPlayers, gameDifficulty, data, true));
            }
            return games;
        }
    }

}
