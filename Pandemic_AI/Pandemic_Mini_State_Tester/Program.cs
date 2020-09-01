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
            Random randomness_provider = new Random();

            PD_Game randomly_generated_game = PD_Game.Create_Default(
                    randomness_provider,
                    4,
                    0,
                    true
                );

            PD_MiniGame mini_state = PD_State_Converter.MiniGame__From__Game(
                randomly_generated_game
                );

            Console.WriteLine(mini_state.state_counter___current_player);

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
