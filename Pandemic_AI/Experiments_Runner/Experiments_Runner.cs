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

            experiments.Add(2, Exp_3__RandomGames__p_RHE_Agent);
            experimentNames.Add(2, "Random games | Policy Based - Rolling Horizon Evolution agent (p-RHE)");


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

        public static void Exp_1__RandomGames__RandomActionAgent()
        {
            Random randomness_provider = new Random();

            // testbed - settings...
            int number_of_games_to_generate = 1000;
            int number_of_players = 4;
            int game_difficulty = 0; // easy

            // generate the random games (test-bed)
            List<PD_Game> games = Generate_Random_Games(
                randomness_provider,
                number_of_games_to_generate,
                number_of_players,
                game_difficulty
                );

            // experiment - settings
            int number_of_repetitions_per_game = 10;
            bool save_initial_game_states = true;
            bool keep_game_stats_report = true;
            bool keep_trace = false;

            // debugging settings
            bool display_actions = false;
            bool display_end_state = true;

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

            // initialize the experiment
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();
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
                randomness_provider,
                pathFinder,
                display_actions,
                display_end_state
                );
        }

        public static void Exp_2__RandomGames__HierarchicalPolicyAgent()
        {
            Random randomness_provider = new Random();

            // testbed - settings...
            int number_of_games_to_generate = 10;
            int number_of_players = 4;
            int game_difficulty = 0; // easy

            // generate the random games (test-bed)
            List<PD_Game> games = Generate_Random_Games(
                randomness_provider,
                number_of_games_to_generate,
                number_of_players,
                game_difficulty
                );

            // experiment - settings
            int number_of_repetitions_per_game = 10;
            bool save_initial_game_states = false;
            bool keep_game_stats_report = true;
            bool keep_trace = false;

            // debugging settings
            bool display_actions = true;
            bool display_end_state = true;

            // initialize the pathFinder
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();

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
                save_initial_game_states,
                agentsDictionary,
                pathFinder,
                number_of_repetitions_per_game,
                experimentResults_Directory,
                keep_game_stats_report,
                keep_trace
                );

            experiment.RunExperiment(
                randomness_provider,
                pathFinder,
                display_actions,
                display_end_state
                );
        }

        public static void Exp_3__RandomGames__p_RHE_Agent()
        {
            Random randomness_provider = new Random();

            // testbed - settings...
            int number_of_games_to_generate = 10;
            int number_of_players = 4;
            int game_difficulty = 0; // easy

            // generate the random games (test-bed)
            List<PD_Game> games = Generate_Random_Games(
                randomness_provider,
                number_of_games_to_generate,
                number_of_players,
                game_difficulty
                );

            // experiment - settings
            int number_of_repetitions_per_game = 10;
            bool save_initial_game_states = true;
            bool keep_game_stats_report = true;
            bool keep_trace = false;

            // debugging settings
            bool display_actions = false;
            bool display_end_state = true;

            // initialize the pathFinder
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();

            // define the agent and the dictionary for the experiment runner
            var p_RHE_agent = AgentsFactory.P_RHE_Agent(pathFinder);
            Dictionary<PD_AI_Agent_Base, string> agentsDictionary = new Dictionary<PD_AI_Agent_Base, string>() {
                { p_RHE_agent, "p_RHE" }
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
                randomness_provider,
                pathFinder,
                display_actions,
                display_end_state
                );
        }

        public static PD_Game Generate_Random_Game(
            Random randomness_provider,
            int numberOfPlayers,
            int gameDifficulty
            )
        {
            return PD_Game.Create(
                randomness_provider,
                numberOfPlayers,
                gameDifficulty,
                true
                );
        }

        public static List<PD_Game> Generate_Random_Games(
            Random randomness_provider,
            int numberOfGames,
            int numberOfPlayers,
            int gameDifficulty
            )
        {
            List<PD_Game> games = new List<PD_Game>();
            for (int i = 0; i < numberOfGames; i++)
            {
                games.Add(
                    PD_Game.Create(
                        randomness_provider,
                        numberOfPlayers, 
                        gameDifficulty, 
                        true
                        )
                    );
            }
            return games;
        }
    }

}
