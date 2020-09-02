﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameSettings : PD_GameParts_Base, ICustomDeepCopyable<PD_GameSettings>
    {
        // fixed, private settings...
        private const int _initialInfectionMarkerValue = 0;
        private const int _minimumNumberOfPlayers = 2;
        private const int _maximumNumberOfPlayers = 4;
        private const int _maximumViableOutbreaks = 7;
        private const int _maximumNumberOfPlayerCardsPerPlayer = 7;

        #region properties
        public int GameDifficultyLevel { get; private set; }
        public Dictionary<int, int> NumberOfInitialCardsPerNumberOfPlayers { get; private set; }
        public Dictionary<int, int> InfectionRatesPerEpidemicsCounter { get; private set; }
        public Dictionary<int, int> NumberOfEpidemicCardsPerDifficultyLevel { get; private set; }
        public int MaximumViableOutbreaks { get { return _maximumViableOutbreaks; } }
        public int MaximumNumberOfPlayerCardsPerPlayer { get { return _maximumNumberOfPlayerCardsPerPlayer; } }
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
        }

        // custom private constructor for deep copy!
        [JsonConstructor]
        public PD_GameSettings(
            int gameDifficultyLevel,
            Dictionary<int, int> numberOfInitialCardsPerNumberOfPlayers,
            Dictionary<int, int> infectionRatesPerEpidemicsCounter,
            Dictionary<int, int> numberOfEpidemicCardsPerDifficultyLevel
            )
        {
            GameDifficultyLevel = gameDifficultyLevel;
            NumberOfInitialCardsPerNumberOfPlayers = numberOfInitialCardsPerNumberOfPlayers;
            InfectionRatesPerEpidemicsCounter = infectionRatesPerEpidemicsCounter;
            NumberOfEpidemicCardsPerDifficultyLevel = numberOfEpidemicCardsPerDifficultyLevel;
        }

        // custom private constructor for deep copy!
        private PD_GameSettings(
            PD_GameSettings gameSettingsToCopy
            )
        {
            this.GameDifficultyLevel = gameSettingsToCopy.GameDifficultyLevel;
            this.NumberOfInitialCardsPerNumberOfPlayers = gameSettingsToCopy.NumberOfInitialCardsPerNumberOfPlayers;
            this.InfectionRatesPerEpidemicsCounter = gameSettingsToCopy.InfectionRatesPerEpidemicsCounter;
            this.NumberOfEpidemicCardsPerDifficultyLevel = gameSettingsToCopy.NumberOfEpidemicCardsPerDifficultyLevel;
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

        // custom deep copy
        public PD_GameSettings GetCustomDeepCopy()
        {
            return new PD_GameSettings(this);
        }

        #region equalityOverride
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_GameSettings)otherObject;

            if (this.GameDifficultyLevel != other.GameDifficultyLevel)
            {
                return false;
            }
            else if (
                this.NumberOfInitialCardsPerNumberOfPlayers.Dictionary_Equal(
                    other.NumberOfInitialCardsPerNumberOfPlayers
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.InfectionRatesPerEpidemicsCounter.Dictionary_Equal(
                    other.InfectionRatesPerEpidemicsCounter
                    ) == false
                )
            {
                return false;
            }
            else if (
                this.NumberOfEpidemicCardsPerDifficultyLevel.Dictionary_Equal(
                    other.NumberOfEpidemicCardsPerDifficultyLevel
                    ) == false
                )
            {
                return false;
            }
            else if (this.MaximumViableOutbreaks != other.MaximumViableOutbreaks)
            {
                return false;
            }
            else if (this.MaximumNumberOfPlayerCardsPerPlayer != other.MaximumNumberOfPlayerCardsPerPlayer)
            {
                return false;
            }
            else
            {
                return true;
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