using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace Performance_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateTimeOfExperiment = DateTime.UtcNow;
            int number_of_games = 1000;
            List<int> roles_list = new List<int>() {
                PD_Player_Roles.Operations_Expert,
                PD_Player_Roles.Researcher,
                PD_Player_Roles.Medic,
                PD_Player_Roles.Scientist
            };

            Random randomness_provider = new Random(1001);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            string report_file_name = "report_" + dateTimeOfExperiment.Ticks.ToString() + ".txt";
            string report_file_path = Path.Combine(
                Directory.GetCurrentDirectory(),
                report_file_name
                );
            PD_IO_Utilities.CreateFile(report_file_path, false, true);

            string report_part = "Date / time: " + dateTimeOfExperiment.ToString();
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

            //////////////////////////////////////////////////////////////////////////////
            /// 1.1 Generate the games, from data
            //////////////////////////////////////////////////////////////////////////////
            watch.Restart();
            List<PD_Game> games = new List<PD_Game>();
            for (int i = 0; i < number_of_games; i++)
            {
                games.Add(
                    PD_Game.Create_Game__AvailableRolesList(
                        randomness_provider,
                        4,
                        0,
                        roles_list
                        )
                    );
            }
            watch.Stop();
            long time_to_generate_games = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Generate {0} games from data and perform game setup: {1}",
                number_of_games,
                time_to_generate_games
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

            //////////////////////////////////////////////////////////////////////////////
            /// 1.3 Run all games until the end, by selecting random actions
            //////////////////////////////////////////////////////////////////////////////
            watch.Restart();
            foreach (var game in games)
            {
                while (PD_Game_Queries.GQ_Is_Ongoing(game))
                {
                    game.Apply_Action(
                        randomness_provider,
                        game.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider)
                    );
                }
            }
            watch.Stop();
            long time_to_play_until_the_end_using_random_actions = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Play {0} games until the end, using random actions: {1}",
                number_of_games,
                time_to_play_until_the_end_using_random_actions
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

            //////////////////////////////////////////////////////////////////////////////
            /// 1.4 Make temporary copies of all games...
            //////////////////////////////////////////////////////////////////////////////
            watch.Restart();
            List<PD_Game> game_copies = new List<PD_Game>();
            foreach (var game in games)
            {
                PD_Game game_copy = game.GetCustomDeepCopy();
                game_copies.Add(game_copy);
            }
            watch.Stop();
            long time_to_copy_all_finished_games = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Copy {0} finished games: {1}",
                number_of_games,
                time_to_copy_all_finished_games
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

            // clear the games' list..
            games.Clear();
            game_copies.Clear();

            //////////////////////////////////////////////////////////////////////////////
            /// 2.1 Generate games and automatically perform setup...
            //////////////////////////////////////////////////////////////////////////////
            watch.Restart();
            for (int i = 0; i < number_of_games; i++)
            {
                games.Add(
                    PD_Game.Create_Game__AvailableRolesList(
                        randomness_provider,
                        4,
                        0,
                        roles_list
                        )
                    );
            }
            watch.Stop();
            long time_to_generate_and_setup_games = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Generate {0} games from data and perform set up: {1}",
                number_of_games,
                time_to_generate_and_setup_games
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

            //////////////////////////////////////////////////////////////////////////////
            /// 2.2. Run all games until the end, by selecting random macro actions
            //////////////////////////////////////////////////////////////////////////////
            PD_AI_PathFinder pf = new PD_AI_PathFinder();
            watch.Restart();
            foreach (var game in games)
            {
                while (PD_Game_Queries.GQ_Is_Ongoing(game))
                {
                    var available_macros = game.GetAvailableMacros(pf);
                    game.Apply_Macro_Action(
                        randomness_provider,
                        available_macros.GetOneRandom(randomness_provider)
                        );
                }
            }
            watch.Stop();
            long time_to_play_until_the_end_using_random_macro_actions = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Play {0} games until the end, using random macro actions: {1}",
                number_of_games,
                time_to_play_until_the_end_using_random_macro_actions
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

            //////////////////////////////////////////////////////////////////////////////
            /// 1.4 Make temporary copies of all games...
            //////////////////////////////////////////////////////////////////////////////
            watch.Restart();
            foreach (var game in games)
            {
                PD_Game game_copy = game.GetCustomDeepCopy();
                game_copies.Add(game_copy);
            }
            watch.Stop();
            long time_to_copy_all_macro_finished_games = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Copy {0} finished games: {1}",
                number_of_games,
                time_to_copy_all_macro_finished_games
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");





            // clear the games' list..
            games.Clear();
            game_copies.Clear();

            //////////////////////////////////////////////////////////////////////////////
            /// 3.1 Generate games and automatically perform setup...
            //////////////////////////////////////////////////////////////////////////////
            watch.Restart();
            for (int i = 0; i < number_of_games; i++)
            {
                games.Add(
                    PD_Game.Create_Game__AvailableRolesList(
                        randomness_provider,
                        4,
                        0,
                        roles_list
                        )
                    );
            }
            watch.Stop();
            time_to_generate_and_setup_games = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Generate {0} games from data and perform set up: {1}",
                number_of_games,
                time_to_generate_and_setup_games
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

            //////////////////////////////////////////////////////////////////////////////
            /// 3.2. Run all games until the end, by selecting random macro actions
            //////////////////////////////////////////////////////////////////////////////
            //PD_AI_PathFinder pf = new PD_AI_PathFinder();
            PD_AI_MacroAgent_HierarchicalPolicy agent = new PD_AI_MacroAgent_HierarchicalPolicy(
                new List<PD_AI_MacroAgent_MainPolicy_Base>() {
                    new PD_AI_MacroAgent_Policy_Cure_ASAP(),
                    new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(3),
                    new PD_AI_MacroAgent_Policy_ShareKnowledge_1_Variation_A(),
                    new PD_AI_MacroAgent_Policy_BuildRS_MaxTotal_MinDistance(6,3),
                    new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(2),
                    new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(1),
                    new PD_AI_MacroAgent_Policy_WalkAway()
                },
                new List<PD_AI_MacroAgent_DiscardPolicy_Base>() {
                    new PD_AI_MacroAgent_Policy_CardDiscard_Smart()
                }
                );
            watch.Restart();
            foreach (var game in games)
            {
                while (PD_Game_Queries.GQ_Is_Ongoing(game))
                {
                    var macro = agent.GetNextMacroAction(randomness_provider, game, pf);
                    game.Apply_Macro_Action(
                        randomness_provider,
                        macro
                        );
                }
            }
            watch.Stop();
            long time_played = watch.ElapsedMilliseconds;
            report_part = String.Format(
                "Play {0} games until the end, using HPA: {1}",
                number_of_games,
                time_played
                );
            Console.WriteLine(report_part);
            PD_IO_Utilities.AppendToFile(report_file_path, report_part + "\n");

        }
    }
}
