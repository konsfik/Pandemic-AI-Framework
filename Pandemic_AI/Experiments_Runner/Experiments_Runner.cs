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

    public delegate void Del();

    public class Experiments_Runner
    {
        static void Main(string[] args)
        {

            // dictionary of available methods:
            Dictionary<int, Del> experiments = new Dictionary<int, Del>();
            Dictionary<int, string> experimentNames = new Dictionary<int, string>();

            experiments.Add(0, Exp_1__RandomGames__RandomActionAgent);
            experimentNames.Add(0, "Random games | Random action agent");

            experiments.Add(1, Exp_2__RandomGames__HierarchicalPolicyAgent);
            experimentNames.Add(1, "Random games | Hierarchical Policy Agent");


            Console.WriteLine("PANDEMIC AI - Framework - DEMO\n");
            Console.WriteLine("choose a program to run:");


            int selection = -1;
            bool parseSuccessful = false;
            while (parseSuccessful == false || experiments.ContainsKey(selection) == false)
            {
                Console.Clear();

                Console.WriteLine("Select:");

                foreach (var key in experimentNames.Keys)
                {
                    Console.WriteLine(key.ToString() + ": " + experimentNames[key]);
                }

                string userInput = Console.ReadLine();
                parseSuccessful = int.TryParse(userInput, out selection);
            }

            experiments[selection].Invoke();

            Console.ReadKey();

        }

        /// <summary>
        /// Experiment 1:
        /// - Random action agent
        /// - 10 random games (four players, easy mode)
        /// - 10 repetitions per game.
        /// The experiment saves a complete report, describing the end - state of each one of the 100 games
        /// in the form of a .csv file, for further analysis.
        /// </summary>
        public static void Exp_1__RandomGames__RandomActionAgent()
        {
            // settings...
            int number_of_games_to_generate = 10;
            int number_of_players = 4;
            int game_difficulty = 0; // easy
            int number_of_repetitions_per_game = 10;

            bool save_initial_game_states = true;
            bool keep_game_stats_report = true;
            bool keep_trace = false;

            // debugging settings
            bool display_actions = false;
            bool display_end_state = true;


            List<PD_Game> games = Generate_Random_Games(
                number_of_games_to_generate,
                number_of_players,
                game_difficulty
                );

            // initialize the pathFinder
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder(games[0]);

            // define the agent and the dictionary for the experiment runner
            PD_AI_ActionAgent_Random randomActionAgent = new PD_AI_ActionAgent_Random();
            Dictionary<PD_AI_Agent_Base, string> agentsDictionary =
                new Dictionary<PD_AI_Agent_Base, string>() {
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
                save_initial_game_states,
                agentsDictionary,
                pathFinder,
                number_of_repetitions_per_game,
                experimentResults_Directory,
                keep_game_stats_report, 
                keep_trace
                );

            experiment.RunExperiment(
                pathFinder,
                display_actions,
                display_end_state
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
        public static void Exp_2__RandomGames__HierarchicalPolicyAgent()
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
