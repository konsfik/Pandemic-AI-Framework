using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace Pandemic_Mini_State_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Random randomness_provider = new Random();
            string game_creation_data = DataUtilities.Read_GameCreationData();

            PD_Game randomly_generated_game = PD_GameCreator.CreateNewGame(
                    randomness_provider,
                    4,
                    0,
                    game_creation_data,
                    true
                );

            Pandemic_Mini_State mini_state = Pandemic_Mini_State.From_Normal_State(
                randomly_generated_game
                );

            Console.WriteLine(mini_state.current_player);

            var a = 0;

            Console.WriteLine(mini_state.current_player);

            ///

        }
    }
}
