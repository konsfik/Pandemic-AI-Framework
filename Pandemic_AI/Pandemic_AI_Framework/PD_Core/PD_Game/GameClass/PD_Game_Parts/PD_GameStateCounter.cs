using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameStateCounter : IDescribable, ICustomDeepCopyable<PD_GameStateCounter>
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
            CureMarkersStates = new Dictionary<int, int>(cureMarkersStates);
            OutbreaksCounter = outbreaksCounter;
            EpidemicsCounter = epidemicsCounter;
            NotEnoughDiseaseCubesToCompleteAnInfection = notEnoughDiseaseCubesToCompleteAnInfection;
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
            this.CureMarkersStates = new Dictionary<int, int>(gameStateCounterToCopy.CureMarkersStates);
            this.OutbreaksCounter = gameStateCounterToCopy.OutbreaksCounter;
            this.EpidemicsCounter = gameStateCounterToCopy.EpidemicsCounter;
            this.NotEnoughDiseaseCubesToCompleteAnInfection = gameStateCounterToCopy.NotEnoughDiseaseCubesToCompleteAnInfection;
            this.NotEnoughPlayerCardsToDraw = gameStateCounterToCopy.NotEnoughPlayerCardsToDraw;
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

        public PD_GameStateCounter GetCustomDeepCopy()
        {
            return new PD_GameStateCounter(this);
        }

        #region equalityOverride
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            PD_GameStateCounter other = (PD_GameStateCounter)otherObject;

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
            if (this.CureMarkersStates.SequenceEqual(other.CureMarkersStates) == false)
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

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + NumberOfPlayers.GetHashCode();
            hash = (hash * 13) + CurrentPlayerActionIndex.GetHashCode();
            hash = (hash * 13) + CurrentPlayerIndex.GetHashCode();
            hash = (hash * 13) + CurrentTurnIndex.GetHashCode();
            hash = (hash * 13) + CureMarkersStates.GetHashCode();
            hash = (hash * 13) + OutbreaksCounter.GetHashCode();
            hash = (hash * 13) + EpidemicsCounter.GetHashCode();

            hash = (hash * 13) + NotEnoughDiseaseCubesToCompleteAnInfection.GetHashCode();
            hash = (hash * 13) + NotEnoughPlayerCardsToDraw.GetHashCode();

            return hash;
        }


        public static bool operator ==(PD_GameStateCounter s1, PD_GameStateCounter s2)
        {
            if (Object.ReferenceEquals(s1, null))
            {
                if (Object.ReferenceEquals(s2, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // c1 is not null
            {
                if (Object.ReferenceEquals(s2, null)) // c2 is null
                {
                    return false;
                }
            }
            // c1 is not null && c2 is not null
            // -> actually check equality
            return s1.Equals(s2);
        }

        public static bool operator !=(PD_GameStateCounter s1, PD_GameStateCounter s2)
        {
            return !(s1 == s2);
        }
        #endregion
    }
}