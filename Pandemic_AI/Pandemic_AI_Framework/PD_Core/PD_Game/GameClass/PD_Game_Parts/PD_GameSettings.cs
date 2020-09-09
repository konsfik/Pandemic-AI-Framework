using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameSettings : 
        PD_GameParts_Base, 
        ICustomDeepCopyable<PD_GameSettings>
    {
        // fixed, private settings...
        //private const int _maximumViableOutbreaks = 7;
        //private const int _maximumNumberOfPlayerCardsPerPlayer = 7;

        #region properties
        public int game_difficulty_level;
        public Dictionary<int, int> initial_hand_size__per__number_of_players;
        public Dictionary<int, int> infection_rate__per__number_of_epidemics;
        public Dictionary<int, int> number_of_epidemic_cards__per__difficulty_level;
        public int maximum_viable_outbreaks;
        public int maximum_player_hand_size;
        #endregion

        #region constructors
        // normal constructor
        public PD_GameSettings(int gameDifficultyLevel)
        {
            game_difficulty_level = gameDifficultyLevel;

            infection_rate__per__number_of_epidemics = new Dictionary<int, int>() {
                {0,2}, {1,2}, {2,2}, {3,3}, {4,3}, {5,4}, {6,4} };

            initial_hand_size__per__number_of_players = new Dictionary<int, int>() {
                {2,4}, {3,3}, {4,2} };

            number_of_epidemic_cards__per__difficulty_level = new Dictionary<int, int>() {
                {0,4}, {1,5}, {2,6} };

            maximum_viable_outbreaks = 7;
            maximum_player_hand_size = 7;
        }

        // custom constructor for json deserialization...
        [JsonConstructor]
        public PD_GameSettings(
            int gameDifficultyLevel,
            Dictionary<int, int> numberOfInitialCardsPerNumberOfPlayers,
            Dictionary<int, int> infectionRatesPerEpidemicsCounter,
            Dictionary<int, int> numberOfEpidemicCardsPerDifficultyLevel,
            int maximumViableOutbreaks,
            int maximumNumberOfPlayerCardsPerPlayer
            )
        {
            this.game_difficulty_level 
                = gameDifficultyLevel;
            this.initial_hand_size__per__number_of_players 
                = numberOfInitialCardsPerNumberOfPlayers.CustomDeepCopy();
            this.infection_rate__per__number_of_epidemics 
                = infectionRatesPerEpidemicsCounter.CustomDeepCopy();
            this.number_of_epidemic_cards__per__difficulty_level 
                = numberOfEpidemicCardsPerDifficultyLevel.CustomDeepCopy();
            this.maximum_viable_outbreaks 
                = maximumViableOutbreaks;
            this.maximum_player_hand_size 
                = maximumNumberOfPlayerCardsPerPlayer;
        }

        // custom private constructor for deep copy!
        private PD_GameSettings(
            PD_GameSettings gameSettingsToCopy
            )
        {
            this.game_difficulty_level 
                = gameSettingsToCopy.game_difficulty_level;
            this.initial_hand_size__per__number_of_players 
                = gameSettingsToCopy.initial_hand_size__per__number_of_players.CustomDeepCopy();
            this.infection_rate__per__number_of_epidemics 
                = gameSettingsToCopy.infection_rate__per__number_of_epidemics.CustomDeepCopy();
            this.number_of_epidemic_cards__per__difficulty_level
                = gameSettingsToCopy.number_of_epidemic_cards__per__difficulty_level.CustomDeepCopy();
            this.maximum_viable_outbreaks
                = gameSettingsToCopy.maximum_viable_outbreaks;
            this.maximum_player_hand_size
                = gameSettingsToCopy.maximum_player_hand_size;
        }

        // custom deep copy
        public PD_GameSettings GetCustomDeepCopy()
        {
            return new PD_GameSettings(this);
        }
        #endregion

        public int GetNumberOfInitialCardsToDealPlayers(int numberOfPlayers)
        {
            return initial_hand_size__per__number_of_players[numberOfPlayers];
        }

        public int GetNumberOfEpidemicCardsToUseInGame()
        {
            return number_of_epidemic_cards__per__difficulty_level[game_difficulty_level];
        }

        #region equalityOverride
        public bool Equals(PD_GameSettings other)
        {
            if (this.game_difficulty_level != other.game_difficulty_level)
            {
                return false;
            }
            else if (
                this.initial_hand_size__per__number_of_players.Dictionary_Equals(
                    other.initial_hand_size__per__number_of_players
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.infection_rate__per__number_of_epidemics.Dictionary_Equals(
                    other.infection_rate__per__number_of_epidemics
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.number_of_epidemic_cards__per__difficulty_level.Dictionary_Equals(
                    other.number_of_epidemic_cards__per__difficulty_level
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.maximum_viable_outbreaks
                != other.maximum_viable_outbreaks
                )
            {
                return false;
            }
            else if (
                this.maximum_player_hand_size
                != other.maximum_player_hand_size
                )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GameSettings other_game_settings)
            {
                return Equals(other_game_settings);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + game_difficulty_level;
            hash = (hash * 13) + initial_hand_size__per__number_of_players.Custom_HashCode();
            hash = (hash * 13) + infection_rate__per__number_of_epidemics.Custom_HashCode();
            hash = (hash * 13) + number_of_epidemic_cards__per__difficulty_level.Custom_HashCode();
            hash = (hash * 13) + maximum_viable_outbreaks;
            hash = (hash * 13) + maximum_player_hand_size;

            return hash;
        }

        
        #endregion
    }
}