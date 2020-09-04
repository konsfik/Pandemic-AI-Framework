using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameStateCounter : 
        PD_GameParts_Base, 
        IEquatable<PD_GameStateCounter>,
        IDescribable, 
        ICustomDeepCopyable<PD_GameStateCounter>
    {
        public int NumberOfPlayers { get; private set; }
        public int CurrentPlayerActionIndex { get; private set; }
        public int CurrentPlayerIndex { get; private set; }
        public int CurrentTurnIndex { get; private set; }
        public Dictionary<int, int> CureMarkersStates { get; private set; }
        public int OutbreaksCounter { get; private set; }
        public int EpidemicsCounter { get; private set; }

        public bool NotEnoughDiseaseCubesToCompleteAnInfection { get; set; }
        public bool NotEnoughPlayerCardsToDraw { get; set; }

        #region constructors
        // normal constructor
        public PD_GameStateCounter(
            int numberOfPlayers,
            int currentPlayerIndex,
            int currentTurnIndex
            )
        {
            NumberOfPlayers = numberOfPlayers;
            CurrentPlayerIndex = currentPlayerIndex;
            CurrentTurnIndex = currentTurnIndex;
            OutbreaksCounter = 0;
            EpidemicsCounter = 0;
            NotEnoughDiseaseCubesToCompleteAnInfection = false;
            NotEnoughPlayerCardsToDraw = false;
        }

        // custom private constructor for deep copy purposes!
        [JsonConstructor]
        public PD_GameStateCounter(
            int numberOfPlayers,
            int currentPlayerActionIndex,
            int currentPlayerIndex,
            int currentTurnIndex,
            Dictionary<int, int> cureMarkersStates,
            int outbreaksCounter,
            int epidemicsCounter,
            bool notEnoughDiseaseCubesToCompleteAnInfection,
            bool notEnoughPlayerCardsToDraw
            )
        {
            NumberOfPlayers = numberOfPlayers;
            CurrentPlayerActionIndex = currentPlayerActionIndex;
            CurrentPlayerIndex = currentPlayerIndex;
            CurrentTurnIndex = currentTurnIndex;
            CureMarkersStates = cureMarkersStates.CustomDeepCopy();
            OutbreaksCounter = outbreaksCounter;
            EpidemicsCounter = epidemicsCounter;
            NotEnoughDiseaseCubesToCompleteAnInfection 
                = notEnoughDiseaseCubesToCompleteAnInfection;
            NotEnoughPlayerCardsToDraw = notEnoughPlayerCardsToDraw;
        }

        // private constructor for deepcopy purposes only
        private PD_GameStateCounter(
            PD_GameStateCounter gameStateCounterToCopy
            )
        {
            this.NumberOfPlayers = gameStateCounterToCopy.NumberOfPlayers;
            this.CurrentPlayerActionIndex = gameStateCounterToCopy.CurrentPlayerActionIndex;
            this.CurrentPlayerIndex = gameStateCounterToCopy.CurrentPlayerIndex;
            this.CurrentTurnIndex = gameStateCounterToCopy.CurrentTurnIndex;
            this.CureMarkersStates =
                gameStateCounterToCopy.CureMarkersStates.CustomDeepCopy();
            this.OutbreaksCounter = gameStateCounterToCopy.OutbreaksCounter;
            this.EpidemicsCounter = gameStateCounterToCopy.EpidemicsCounter;
            this.NotEnoughDiseaseCubesToCompleteAnInfection = gameStateCounterToCopy.NotEnoughDiseaseCubesToCompleteAnInfection;
            this.NotEnoughPlayerCardsToDraw = gameStateCounterToCopy.NotEnoughPlayerCardsToDraw;
        }

        public PD_GameStateCounter GetCustomDeepCopy()
        {
            return new PD_GameStateCounter(this);
        }
        #endregion

        #region various methods
        public void Override_NumberOfPlayers(int numberOfPlayers)
        {
            NumberOfPlayers = numberOfPlayers;
        }

        public void ResetPlayerActionIndex()
        {
            CurrentPlayerActionIndex = 0;
        }

        public void IncreasePlayerActionIndex()
        {
            CurrentPlayerActionIndex++;
        }

        public void IncreaseCurrentPlayerIndex()
        {
            CurrentPlayerIndex++;
            CurrentTurnIndex++;
            if (CurrentPlayerIndex >= NumberOfPlayers)
            {
                CurrentPlayerIndex = 0;
            }
        }

        public void InitializeCureMarkerStates()
        {
            CureMarkersStates = new Dictionary<int, int>() {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0}
            };
        }

        public void CureDisease(int diseaseTypeToCure)
        {
            if (CureMarkersStates[diseaseTypeToCure] != 0)
            {
                throw new System.Exception("trying to cure a cured disease");
            }
            CureMarkersStates[diseaseTypeToCure] = 1;
        }

        public void EradicateDisease(int diseaseTypeToEradicate)
        {
            CureMarkersStates[diseaseTypeToEradicate] = 2;
        }

        public void ResetOutbreaksCounter()
        {
            OutbreaksCounter = 0;
        }

        public void IncreaseOutbreaksCounter()
        {
            OutbreaksCounter++;
        }

        public void ResetEpidemicsCounter()
        {
            EpidemicsCounter = 0;
        }

        public void IncreaseEpidemicsCounter()
        {
            EpidemicsCounter++;
        }

        public string GetDescription()
        {
            string report = "";
            report += "GAME STATE COUNTER:\n";

            var myProperties = this.GetType().GetProperties();

            for (int i = 0; i < myProperties.Length; i++)
            {
                var property = myProperties[i];
                if (property.GetValue(this) != null)
                {
                    report += String.Format(
                        "{0} : {1}\n",
                        property.Name,
                        property.GetValue(this).ToString()
                        );
                }
            }

            return report;
        }
        #endregion

        #region equalityOverride
        public bool Equals(PD_GameStateCounter other)
        {
            if (this.NumberOfPlayers != other.NumberOfPlayers)
            {
                return false;
            }
            if (this.CurrentPlayerActionIndex != other.CurrentPlayerActionIndex)
            {
                return false;
            }
            if (this.CurrentPlayerIndex != other.CurrentPlayerIndex)
            {
                return false;
            }
            if (this.CurrentTurnIndex != other.CurrentTurnIndex)
            {
                return false;
            }
            if (this.CureMarkersStates.Dictionary_Equals(other.CureMarkersStates) == false)
            {
                return false;
            }
            if (this.OutbreaksCounter != other.OutbreaksCounter)
            {
                return false;
            }
            if (this.EpidemicsCounter != other.EpidemicsCounter)
            {
                return false;
            }

            if (this.NotEnoughDiseaseCubesToCompleteAnInfection != other.NotEnoughDiseaseCubesToCompleteAnInfection)
            {
                return false;
            }
            if (this.NotEnoughPlayerCardsToDraw != other.NotEnoughPlayerCardsToDraw)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GameStateCounter other_game_state_counter)
            {
                return Equals(other_game_state_counter);
            }
            else 
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + NumberOfPlayers;
            hash = (hash * 13) + CurrentPlayerActionIndex;
            hash = (hash * 13) + CurrentPlayerIndex;
            hash = (hash * 13) + CurrentTurnIndex;
            hash = (hash * 13) + CureMarkersStates.Custom_HashCode();
            hash = (hash * 13) + OutbreaksCounter;
            hash = (hash * 13) + EpidemicsCounter;

            hash = (hash * 13) + NotEnoughDiseaseCubesToCompleteAnInfection.GetHashCode();
            hash = (hash * 13) + NotEnoughPlayerCardsToDraw.GetHashCode();

            return hash;
        }

        
        #endregion
    }
}