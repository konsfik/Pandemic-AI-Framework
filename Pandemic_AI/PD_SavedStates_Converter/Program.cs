using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


            // 
            string old_folder_path_1
                = "D:\\Sync\\___PGA_Output\\__Dataset_Conversions\\Old_State\\Set_1_10000_RandomGames";
            string old_folder_path_2
                = "D:\\Sync\\___PGA_Output\\__Dataset_Conversions\\Old_State\\Set_2_10_SelectedGames";

            string new_folder_path_1
                = "D:\\Sync\\___PGA_Output\\__Dataset_Conversions\\New_State\\Set_1_10000_RandomGames";
            string new_folder_path_2
                = "D:\\Sync\\___PGA_Output\\__Dataset_Conversions\\New_State\\Set_2_10_SelectedGames";


            // convert 
            var file_paths = PD_IO_Utilities.GetFilePathsInFolder(old_folder_path_1);

            foreach (string file_path in file_paths)
            {
                PD_Game game = PD_IO_Utilities.DeserializeFromJsonFile<PD_Game>(file_path);

                PD_MiniGame mini_game = game.Convert_To_MiniGame();

                string serialized_mini_game = mini_game.To_Json_String(
                    Newtonsoft.Json.Formatting.Indented,
                    Newtonsoft.Json.TypeNameHandling.None,
                    Newtonsoft.Json.PreserveReferencesHandling.None
                    );
                string output_path = Path.Combine(
                    new_folder_path_1,
                    game.unique_id.ToString() + ".csv"
                    );
                PD_IO_Utilities.CreateFile(output_path, true, false);
                PD_IO_Utilities.AppendToFile(
                    output_path,
                    serialized_mini_game
                    );
            }


            //// read the saved states
            //string test_bed_folder = System.IO.Path.Combine(
            //    Directory.GetCurrentDirectory(),
            //    "ParameterTuning_TestBed"
            //    );

            //string output_folder = System.IO.Path.Combine(
            //    Directory.GetCurrentDirectory(),
            //    "Output"
            //    );
            //if (PD_IO_Utilities.FolderExists(output_folder) == false)
            //{
            //    PD_IO_Utilities.CreateFolder(output_folder, true);
            //}

            //var games_file_paths = PD_IO_Utilities.GetFilePathsInFolder(test_bed_folder);
            //List<PD_Game> games = new List<PD_Game>();
            //List<PD_MiniGame> mini_games = new List<PD_MiniGame>();
            //List<string> serialized_games = new List<string>();

            //foreach (var game_file_path in games_file_paths)
            //{
            //    PD_Game game = PD_IO_Utilities.DeserializeFromJsonFile<PD_Game>(game_file_path);
            //    games.Add(game);
            //    PD_MiniGame mini_game = game.Convert_To_MiniGame();
            //    mini_games.Add(mini_game);
            //    string serialized_mini_game = mini_game.To_Json_String(
            //        Newtonsoft.Json.Formatting.None,
            //        Newtonsoft.Json.TypeNameHandling.None,
            //        Newtonsoft.Json.PreserveReferencesHandling.None
            //        );
            //    serialized_games.Add(serialized_mini_game);
            //    string game_id = game.UniqueID.ToString();
            //    string file_path = System.IO.Path.Combine(
            //        output_folder,
            //        game_id + ".json"
            //        );
            //    PD_IO_Utilities.CreateFile(
            //        file_path,
            //        true,
            //        true
            //        );
            //    PD_IO_Utilities.AppendToFile(
            //        file_path,
            //        serialized_mini_game
            //        );
            //}

            //Console.ReadKey();
        }
    }
}
