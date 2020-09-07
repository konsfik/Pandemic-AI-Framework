using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace PD_SavedStates_Converter
{
    /// <summary>
    /// Temporary program 
    /// for converting all the saved states into the new format...
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Random randomness_provider = new Random(1000);

            // read the saved states
            string test_bed_folder = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "ParameterTuning_TestBed"
                );

            string output_folder = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "Output"
                );
            if (PD_IO_Utilities.FolderExists(output_folder) == false)
            {
                PD_IO_Utilities.CreateFolder(output_folder, true);
            }

            var games_file_paths = PD_IO_Utilities.GetFilePathsInFolder(test_bed_folder);
            List<PD_Game> games = new List<PD_Game>();
            List<PD_MiniGame> mini_games = new List<PD_MiniGame>();
            List<string> serialized_games = new List<string>();

            foreach (var game_file_path in games_file_paths)
            {
                PD_Game game = PD_IO_Utilities.DeserializeFromJsonFile<PD_Game>(game_file_path);
                games.Add(game);
                PD_MiniGame mini_game = game.To_MiniGame();
                mini_games.Add(mini_game);
                string serialized_mini_game = mini_game.To_Json_String(
                    Newtonsoft.Json.Formatting.Indented,
                    Newtonsoft.Json.TypeNameHandling.None,
                    Newtonsoft.Json.PreserveReferencesHandling.None
                    );
                serialized_games.Add(serialized_mini_game);
                string file_path = System.IO.Path.Combine(
                    output_folder,
                    DateTime.Now.Ticks.ToString() + ".json"
                    );
                PD_IO_Utilities.CreateFile(
                    file_path,
                    true,
                    true
                    );
                PD_IO_Utilities.AppendToFile(
                    file_path,
                    serialized_mini_game
                    );
            }

            foreach (var mini_game in mini_games) {
                PD_Game g = mini_game.To_Game();
                g.UpdateAvailablePlayerActions();
                while (g.GQ_Is_Ongoing()) {
                    var available_actions = g.CurrentAvailablePlayerActions;
                    var selected_action = available_actions.GetOneRandom(randomness_provider);
                    Console.WriteLine(selected_action.GetDescription());
                    g.ApplySpecificPlayerAction(
                        randomness_provider,
                        selected_action
                        );
                }
                Console.WriteLine("---------------");
            }
            Console.ReadKey();
        }
    }
}
