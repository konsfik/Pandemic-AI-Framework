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
        IEquatable<PD_GameSettings>,
        ICustomDeepCopyable<PD_GameSettings>
    {
        // fixed, private settings...
        //private const int _maximumViableOutbreaks = 7;
        //private const int _maximumNumberOfPlayerCardsPerPlayer = 7;

        #region properties
        public int GameDifficultyLevel { get; private set; }
        public Dictionary<int, int> NumberOfInitialCardsPerNumberOfPlayers { get; private set; }
        public Dictionary<int, int> InfectionRatesPerEpidemicsCounter { get; private set; }
        public Dictionary<int, int> NumberOfEpidemicCardsPerDifficultyLevel { get; private set; }
        public int MaximumViableOutbreaks { get; private set; }
        public int MaximumNumberOfPlayerCardsPerPlayer { get; private set; }
        #endregion

        #region constructors
        // normal constructor
        public PD_GameSettings(int gameDifficultyLevel)
        {
            GameDifficultyLevel = gameDifficultyLevel;

            InfectionRatesPerEpidemicsCounter = new Dictionary<int, int>() {
                {0,2}, {1,2}, {2,2}, {3,3}, {4,3}, {5,4}, {6,4} };

            NumberOfInitialCardsPerNumberOfPlayers = new Dictionary<int, int>() {
                {2,4}, {3,3}, {4,2} };

            NumberOfEpidemicCardsPerDifficultyLevel = new Dictionary<int, int>() {
                {0,4}, {1,5}, {2,6} };

            MaximumViableOutbreaks = 7;
            MaximumNumberOfPlayerCardsPerPlayer = 7;
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
            this.GameDifficultyLevel 
                = gameDifficultyLevel;
            this.NumberOfInitialCardsPerNumberOfPlayers 
                = numberOfInitialCardsPerNumberOfPlayers.CustomDeepCopy();
            this.InfectionRatesPerEpidemicsCounter 
                = infectionRatesPerEpidemicsCounter.CustomDeepCopy();
            this.NumberOfEpidemicCardsPerDifficultyLevel 
                = numberOfEpidemicCardsPerDifficultyLevel.CustomDeepCopy();
            this.MaximumViableOutbreaks 
                = maximumViableOutbreaks;
            this.MaximumNumberOfPlayerCardsPerPlayer 
                = maximumNumberOfPlayerCardsPerPlayer;
        }

        // custom private constructor for deep copy!
        private PD_GameSettings(
            PD_GameSettings gameSettingsToCopy
            )
        {
            this.GameDifficultyLevel 
                = gameSettingsToCopy.GameDifficultyLevel;
            this.NumberOfInitialCardsPerNumberOfPlayers 
                = gameSettingsToCopy.NumberOfInitialCardsPerNumberOfPlayers.CustomDeepCopy();
            this.InfectionRatesPerEpidemicsCounter 
                = gameSettingsToCopy.InfectionRatesPerEpidemicsCounter.CustomDeepCopy();
            this.NumberOfEpidemicCardsPerDifficultyLevel
                = gameSettingsToCopy.NumberOfEpidemicCardsPerDifficultyLevel.CustomDeepCopy();
            this.MaximumViableOutbreaks
                = gameSettingsToCopy.MaximumViableOutbreaks;
            this.MaximumNumberOfPlayerCardsPerPlayer
                = gameSettingsToCopy.MaximumNumberOfPlayerCardsPerPlayer;
        }

        // custom deep copy
        public PD_GameSettings GetCustomDeepCopy()
        {
            return new PD_GameSettings(this);
        }
        #endregion

        public int GetNumberOfInitialCardsToDealPlayers(int numberOfPlayers)
        {
            return NumberOfInitialCardsPerNumberOfPlayers[numberOfPlayers];
        }

        public int GetNumberOfEpidemicCardsToUseInGame()
        {
            return NumberOfEpidemicCardsPerDifficultyLevel[GameDifficultyLevel];
        }

        #region equalityOverride
        public bool Equals(PD_GameSettings other)
        {
            if (this.GameDifficultyLevel != other.GameDifficultyLevel)
            {
                return false;
            }
            else if (
                this.NumberOfInitialCardsPerNumberOfPlayers.Dictionary_Equals(
                    other.NumberOfInitialCardsPerNumberOfPlayers
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.InfectionRatesPerEpidemicsCounter.Dictionary_Equals(
                    other.InfectionRatesPerEpidemicsCounter
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.NumberOfEpidemicCardsPerDifficultyLevel.Dictionary_Equals(
                    other.NumberOfEpidemicCardsPerDifficultyLevel
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.MaximumViableOutbreaks
                != other.MaximumViableOutbreaks
                )
            {
                return false;
            }
            else if (
                this.MaximumNumberOfPlayerCardsPerPlayer
                != other.MaximumNumberOfPlayerCardsPerPlayer
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

            hash = (hash * 13) + GameDifficultyLevel;
            hash = (hash * 13) + NumberOfInitialCardsPerNumberOfPlayers.Custom_HashCode();
            hash = (hash * 13) + InfectionRatesPerEpidemicsCounter.Custom_HashCode();
            hash = (hash * 13) + NumberOfEpidemicCardsPerDifficultyLevel.Custom_HashCode();
            hash = (hash * 13) + MaximumViableOutbreaks;
            hash = (hash * 13) + MaximumNumberOfPlayerCardsPerPlayer;

            return hash;
        }

        
        #endregion
    }
}