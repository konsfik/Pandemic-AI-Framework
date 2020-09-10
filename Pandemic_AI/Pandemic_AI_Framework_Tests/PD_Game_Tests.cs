using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Pandemic_AI_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework.Tests
{
    [TestClass()]
    public class PD_Game_Tests
    {
        [TestMethod()]
        public void Serialize_Deserialize_MiniGame()
        {
            Random randomness_provider = new Random();

            for (int i = 0; i < 2000; i++)
            {
                // create a normal game, for later use
                PD_Game initial_game = PD_Game.Create(randomness_provider, 4, 0, true);

                // convert to mini game
                PD_MiniGame converted_mini_game = initial_game.Convert_To_MiniGame();

                // serialize the mini game
                string serialized_mini_game = converted_mini_game.To_Json_String(
                    Formatting.None,
                    TypeNameHandling.None,
                    PreserveReferencesHandling.None
                    );

                // deserialize the mini game
                PD_MiniGame deserialized_mini_game
                    = JsonConvert.DeserializeObject<PD_MiniGame>(serialized_mini_game);

                // test that the deserialized game is exactly the same as the mini game
                Assert.IsTrue(converted_mini_game.GetHashCode() == deserialized_mini_game.GetHashCode());
                Assert.IsTrue(converted_mini_game == deserialized_mini_game);
                Assert.IsTrue(converted_mini_game.Equals(deserialized_mini_game));

                // create a normal game, from the deserialized game
                PD_Game final_game = deserialized_mini_game.Convert_To_Game();

                Assert.IsTrue(
                    Games_Practically_Equal(
                        initial_game,
                        final_game
                        )
                    );

                final_game.UpdateAvailablePlayerActions();

                randomness_provider = new Random(i);
                while (final_game.GQ_Is_Ongoing())
                {
                    PD_GameAction_Base action =
                        final_game
                        .CurrentAvailablePlayerActions
                        .GetOneRandom(randomness_provider);
                    final_game.Apply_Action(
                        randomness_provider,
                        action
                        );
                }
                randomness_provider = new Random(i);
                while (initial_game.GQ_Is_Ongoing())
                {
                    var action = initial_game.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider);
                    initial_game.Apply_Action(
                        randomness_provider,
                        action);
                }

                Assert.IsTrue(Games_Practically_Equal(initial_game, final_game));
            }
        }

        #region help methods
        private bool Games_Practically_Equal(PD_Game game_1, PD_Game game_2)
        {
            if (game_1.unique_id != game_2.unique_id)
            {
                return false;
            }
            else if (game_1.game_settings.Equals(game_2.game_settings) == false)
            {
                return false;
            }
            else if (game_1.game_FSM.Equals(game_2.game_FSM) == false)
            {
                return false;
            }
            else if (game_1.game_state_counter.Equals(game_2.game_state_counter) == false)
            {
                return false;
            }
            else if (game_1.players.List_Equals(game_2.players) == false)
            {
                return false;
            }
            else if (game_1.map.Equals(game_2.map) == false)
            {
                return false;
            }
            else if (PracticalGameComparison_Cards(game_1, game_2) == false)
            {
                return false;
            }
            else if (PracticalGameComparison_MapElements(game_1, game_2) == false)
            {
                return false;
            }
            else if (
                game_1.CurrentAvailablePlayerActions.List_Equals(
                    game_2.CurrentAvailablePlayerActions
                    ) == false
                )
            {
                return false;
            }

            return true;
        }

        public bool PracticalGameComparison_Cards(PD_Game game_1, PD_Game game_2)
        {
            if (game_1.cards.divided_deck_of_infection_cards.List_Equals(
                game_2.cards.divided_deck_of_infection_cards) == false)
            {
                return false;
            }
            else if (game_1.cards.active_infection_cards.List_Equals(
                game_2.cards.active_infection_cards) == false)
            {
                return false;
            }
            else if (game_1.cards.deck_of_discarded_infection_cards.List_Equals(
                game_2.cards.deck_of_discarded_infection_cards) == false)
            {
                return false;
            }
            else if (game_1.cards.divided_deck_of_player_cards.List_Equals(
                game_2.cards.divided_deck_of_player_cards) == false)
            {
                return false;
            }
            else if (game_1.cards.deck_of_discarded_player_cards.List_Equals(
                game_2.cards.deck_of_discarded_player_cards) == false)
            {
                return false;
            }
            else if (game_1.cards.deck_of_discarded_player_cards.List_Equals(
                game_2.cards.deck_of_discarded_player_cards) == false)
            {
                return false;
            }
            else if (game_1.cards.player_hand__per__player.Dictionary_Equals(
                game_2.cards.player_hand__per__player) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool PracticalGameComparison_MapElements(PD_Game game_1, PD_Game game_2)
        {
            // inactive player pawns

            for (int t = 0; t < 4; t++)
            {
                if (
                    game_1.map_elements.inactive_infection_cubes__per__type[t]
                    !=
                    game_2.map_elements.inactive_infection_cubes__per__type[t]
                    )
                {
                    return false;
                }
            }

            foreach (int city in game_1.map.cities)
            {
                for (int t = 0; t < 4; t++)
                {
                    if (
                        game_1.GQ_Num_InfectionCubes_OfType_OnCity(city, t)
                        !=
                        game_2.GQ_Num_InfectionCubes_OfType_OnCity(city, t)
                        )
                    {
                        return false;
                    }
                }
            }

            if (game_1.map_elements.inactive_research_stations != game_2.map_elements.inactive_research_stations)
            {
                return false;
            }
            else if (game_1.map_elements.research_stations__per__city.Dictionary_Equals(
                game_2.map_elements.research_stations__per__city) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        [TestMethod()]
        public void Deserialize_ExistingGame_NewRepresentation_Test()
        {
            Random randomness_provider = new Random(1001);

            string saved_games_path = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                "ParameterTuning_TestBed_NewRepresentation"
                );
            var game_file_paths = PD_IO_Utilities.GetFilePathsInFolder(saved_games_path);

            // play the games until the end, using single actions
            foreach (var game_file_path in game_file_paths)
            {
                string serialized_mini_game = PD_IO_Utilities.ReadFile(game_file_path);

                PD_MiniGame deserialized_mini_game =
                    JsonConvert.DeserializeObject<PD_MiniGame>(serialized_mini_game);

                PD_Game game = deserialized_mini_game.Convert_To_Game();

                // play the game until the end...
                while (game.GQ_Is_Ongoing())
                {
                    var available_actions = game.CurrentAvailablePlayerActions;
                    var random_action = available_actions.GetOneRandom(randomness_provider);
                    game.Apply_Action(
                        randomness_provider,
                        random_action
                        );
                }

                Assert.IsTrue(game.GQ_Is_Ongoing() == false);
            }

            // play the games until the end, using macro actions
            PD_AI_PathFinder pathFinder = new PD_AI_PathFinder();
            foreach (var game_file_path in game_file_paths)
            {
                string serialized_mini_game = PD_IO_Utilities.ReadFile(game_file_path);

                PD_MiniGame deserialized_mini_game =
                    JsonConvert.DeserializeObject<PD_MiniGame>(serialized_mini_game);

                PD_Game game = deserialized_mini_game.Convert_To_Game();

                // play the game until the end...
                while (game.GQ_Is_Ongoing())
                {
                    var available_macro_actions = game.GetAvailableMacros(pathFinder);
                    var random_macro_action = available_macro_actions.GetOneRandom(randomness_provider);
                    game.Apply_Macro_Action(
                        randomness_provider,
                        random_macro_action
                        );
                }

                Assert.IsTrue(game.GQ_Is_Ongoing() == false);
            }
        }

        [TestMethod()]
        public void Generate_Game_Without_Data()
        {
            Random randomness_provider = new Random(1000);

            // generate 100 games for every possible settings selection
            for (int num_players = 2; num_players <= 4; num_players++)
            {
                for (int game_difficulty = 0; game_difficulty <= 2; game_difficulty++)
                {
                    // repeat the process 100 times!
                    for (int i = 0; i < 1000; i++)
                    {
                        PD_Game game = PD_Game.Create(
                            randomness_provider,
                            num_players,
                            game_difficulty,
                            true
                            );

                        while (game.GQ_Is_Ongoing())
                        {
                            var random_action = game
                                .CurrentAvailablePlayerActions
                                .GetOneRandom(randomness_provider);

                            game.Apply_Action(
                                randomness_provider,
                                random_action
                                );
                        }

                        Assert.IsTrue(game.GQ_Is_Ongoing() == false);
                    }
                }
            }

        }


        [TestMethod()]
        public void RandomSeed_Tests()
        {
            Random randomness_provider = new Random(1000);

            PD_Game game_1 = PD_Game.Create(
                randomness_provider,
                4,
                0,
                true
                );

            randomness_provider = new Random(1000);

            PD_Game game_2 = PD_Game.Create(
                randomness_provider,
                4,
                0,
                true
                );

            randomness_provider = new Random(1000);
            while (game_1.GQ_Is_Ongoing())
            {
                game_1.Apply_Action(
                    randomness_provider,
                    game_1.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider)
                    );
            }

            randomness_provider = new Random(1000);
            while (game_2.GQ_Is_Ongoing())
            {
                game_2.Apply_Action(
                    randomness_provider,
                    game_2.CurrentAvailablePlayerActions.GetOneRandom(randomness_provider)
                    );
            }

            game_2.OverrideUniqueID(game_1.unique_id);
            game_2.OverrideStartTime(game_1.start_time);
            game_2.OverrideEndTime(game_1.end_time);

            Assert.IsTrue(game_1.Equals(game_2));
            Assert.IsTrue(game_1 == game_2);
            Assert.IsTrue(game_1.GetHashCode() == game_2.GetHashCode());
        }

        [TestMethod()]
        public void GetCustomDeepCopy_Test()
        {
            Random randomness_provider = new Random();

            PD_Game game = PD_Game.Create(
                randomness_provider,
                4,
                0,
                true
                );
            PD_Game gameCopy = game.GetCustomDeepCopy();

            Assert.IsTrue(
                game.unique_id
                ==
                gameCopy.unique_id
                );

            Assert.IsTrue(
                game.start_time
                ==
                gameCopy.start_time
                );

            Assert.IsTrue(
                game.end_time
                ==
                gameCopy.end_time
                );

            Assert.IsTrue(
                game.game_settings
                ==
                gameCopy.game_settings
                );

            Assert.IsTrue(
                game.game_FSM
                ==
                gameCopy.game_FSM
                );

            Assert.IsTrue(
                game.game_state_counter
                ==
                gameCopy.game_state_counter
                );

            Assert.IsTrue(
                game.players.SequenceEqual(gameCopy.players)
                );

        }
    }
}