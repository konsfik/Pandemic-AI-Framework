using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;
using Newtonsoft.Json;

namespace Pandemic_Mini_State_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("debug mode");
#endif

            Random randomness_provider = new Random();

            PD_Game randomly_generated_game = PD_Game.Create(
                    randomness_provider,
                    4,
                    0,
                    true
                );

            PD_MiniGame mini_state = randomly_generated_game.To_MiniGame();

            string serialized_mini_state = mini_state.To_Json_String(
                Formatting.None,
                TypeNameHandling.None,
                PreserveReferencesHandling.None
                );
            Console.WriteLine(serialized_mini_state);

            PD_MiniGame deserialized_mini_state = JsonConvert.DeserializeObject<PD_MiniGame>(
                serialized_mini_state
                );


            string objectSerializedToString =
                JsonConvert.SerializeObject(
                    mini_state,
                    Formatting.None,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None,
                        PreserveReferencesHandling = PreserveReferencesHandling.None
                    }
                );

            PD_IO_Utilities.CreateFile(
                "D:\\saved_state.json",
                true,
                false
                );

            PD_IO_Utilities.AppendToFile(
                "D:\\saved_state.json",
                objectSerializedToString
                );

            //PD_IO_Utilities.SerializeToJsonAndSave(
            //    mini_state,
            //    "D:\\saved_state.json",
            //    true,
            //    false
            //    );


            Console.WriteLine(mini_state.state_counter___current_player);

            ///

        }
    }
}
