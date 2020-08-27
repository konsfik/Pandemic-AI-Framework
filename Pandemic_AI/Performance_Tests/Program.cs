using System;
using System.Collections.Generic;
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
            int number_of_games = 2000;
            Random randomness_provider = new Random(1000);
            string game_creation_data = DataUtilities.Read_GameCreationData();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            string final_report = "";
            final_report += "Date / time: " + DateTime.UtcNow.ToString();
            Console.WriteLine(final_report);

            watch.Restart();
            List<PD_Game> games = new List<PD_Game>();
            for (int i = 0; i < number_of_games; i++)
            {
                games.Add(
                    PD_GameCreator.CreateNewGame(
                        randomness_provider,
                        4,
                        0,
                        game_creation_data,
                        true
                        )
                    );
            }
            watch.Stop();
            long time_to_generate_games = watch.ElapsedMilliseconds;
            Console.WriteLine("time to generate games: " + time_to_generate_games.ToString());

            games.Clear();
            for (int i = 0; i < number_of_games; i++)
            {
                games.Add(
                    PD_GameCreator.CreateNewGame(
                        randomness_provider,
                        4,
                        0,
                        game_creation_data,
                        true
                        )
                    );
            }
            watch.Stop();
            time_to_generate_games = watch.ElapsedMilliseconds;
            Console.WriteLine("time to generate games: " + time_to_generate_games.ToString());

            Console.ReadKey();

            watch.Restart();
            List<PD_Game> game_copies_fair = new List<PD_Game>();
            foreach (var game in games)
            {
                PD_Game game_copy = game.Request_Fair_ForwardModel(randomness_provider);
                game_copies_fair.Add(game_copy);
            }
            watch.Stop();
            Console.WriteLine("done");
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());

            var a = 0;
            watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var game in games)
            {
                while (PD_Game_Queries.GQ_Is_GameOngoing(game))
                {
                    var selectedAction = game.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider);
                    game.ApplySpecificPlayerAction(randomness_provider, selectedAction);
                }
            }
            watch.Stop();
            Console.WriteLine("done");
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());

            Console.ReadKey();
        }
    }
}
