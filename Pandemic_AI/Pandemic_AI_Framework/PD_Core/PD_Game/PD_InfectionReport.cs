using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_InfectionReport :
        IDescribable,
        IEquatable<PD_InfectionReport>,
        ICustomDeepCopyable<PD_InfectionReport>
    {

        #region Properties
        public bool HappenedDuringGameSetup { get; private set; }

        public PD_Player PlayerWhoAppliesInfection { get; private set; }
        public PD_City InitialCity { get; private set; }

        public int InfectionType { get; private set; }
        public int InitialNumberOfCubes { get; private set; }

        public List<PD_City> InfectedCities { get; private set; }
        public List<PD_City> InfectionsPrevented_ByMedic { get; private set; }
        public List<PD_City> CitiesThatHaveCausedOutbreaks { get; private set; }

        public int NumberOfInfectionCubesUsed { get; private set; }
        public InfectionFailureReasons FailureReason { get; private set; }
        #endregion

        #region constructors
        /// <summary>
        /// normal constructor
        /// </summary>
        /// <param name="playerWhoAppliesInfection"></param>
        /// <param name="initialCity"></param>
        /// <param name="infectionType"></param>
        /// <param name="initialNumberOfCubes"></param>
        public PD_InfectionReport(
            bool happenedDuringGameSetup,

            PD_Player playerWhoAppliesInfection,
            PD_City initialCity,

            int infectionType,
            int initialNumberOfCubes
            )
        {
            HappenedDuringGameSetup = happenedDuringGameSetup;

            PlayerWhoAppliesInfection = playerWhoAppliesInfection;
            InitialCity = initialCity;

            InfectionType = infectionType;
            InitialNumberOfCubes = initialNumberOfCubes;

            InfectedCities = new List<PD_City>();
            InfectionsPrevented_ByMedic = new List<PD_City>();
            CitiesThatHaveCausedOutbreaks = new List<PD_City>();

            NumberOfInfectionCubesUsed = 0;
            FailureReason = InfectionFailureReasons.none;
        }

        /// <summary>
        /// special constructor, specially designed for the JSON serializer.
        /// </summary>
        /// <param name="playerWhoAppliesInfection"></param>
        /// <param name="initialCity"></param>
        /// <param name="infectionType"></param>
        /// <param name="initialNumberOfCubes"></param>
        /// <param name="infectedCities"></param>
        /// <param name="citiesThatHaveCausedOutbreaks"></param>
        /// <param name="numberOfInfectionCubesUsed"></param>
        /// <param name="failureReason"></param>
        [JsonConstructor]
        public PD_InfectionReport(
            bool happenedDuringGameSetup,

            PD_Player playerWhoAppliesInfection,
            PD_City initialCity,

            int infectionType,
            int initialNumberOfCubes,

            List<PD_City> infectedCities,
            List<PD_City> infectionsPrevented_ByMedic,
            List<PD_City> citiesThatHaveCausedOutbreaks,

            int numberOfInfectionCubesUsed,
            InfectionFailureReasons failureReason
            )
        {
            this.HappenedDuringGameSetup = happenedDuringGameSetup;

            this.PlayerWhoAppliesInfection = playerWhoAppliesInfection.GetCustomDeepCopy();
            this.InitialCity = initialCity.GetCustomDeepCopy();

            this.InfectionType = infectionType;
            this.InitialNumberOfCubes = initialNumberOfCubes;

            this.InfectedCities = infectedCities.CustomDeepCopy();
            this.InfectionsPrevented_ByMedic = infectionsPrevented_ByMedic.CustomDeepCopy();
            this.CitiesThatHaveCausedOutbreaks = citiesThatHaveCausedOutbreaks.CustomDeepCopy();

            this.NumberOfInfectionCubesUsed = numberOfInfectionCubesUsed;
            this.FailureReason = failureReason;
        }

        /// <summary>
        /// private constructor, for deep copy purposes only
        /// </summary>
        /// <param name="reportToCopy"></param>
        private PD_InfectionReport(
            PD_InfectionReport reportToCopy
            )
        {
            this.HappenedDuringGameSetup =
                reportToCopy.HappenedDuringGameSetup;

            this.PlayerWhoAppliesInfection =
                reportToCopy.PlayerWhoAppliesInfection.GetCustomDeepCopy();
            this.InitialCity =
                reportToCopy.InitialCity.GetCustomDeepCopy();

            this.InfectionType =
                reportToCopy.InfectionType;
            this.InitialNumberOfCubes =
                reportToCopy.InitialNumberOfCubes;

            this.InfectedCities =
                reportToCopy.InfectedCities.CustomDeepCopy();
            this.InfectionsPrevented_ByMedic =
                reportToCopy.InfectionsPrevented_ByMedic.CustomDeepCopy();
            this.CitiesThatHaveCausedOutbreaks =
                reportToCopy.CitiesThatHaveCausedOutbreaks.CustomDeepCopy();

            this.NumberOfInfectionCubesUsed =
                reportToCopy.NumberOfInfectionCubesUsed;
            this.FailureReason =
                reportToCopy.FailureReason;
        }

        public PD_InfectionReport GetCustomDeepCopy()
        {
            return new PD_InfectionReport(this);
        }
        #endregion

        public void AddUsedCubes(int usedCubes)
        {
            NumberOfInfectionCubesUsed += usedCubes;
        }

        public void AddInfectedCity(PD_City infectedCity)
        {
            if (InfectedCities.Contains(infectedCity) == false)
            {
                InfectedCities.Add(infectedCity);
            }
        }

        public void AddInfectionPreventedByMedic(PD_City infectionPrevented)
        {
            InfectionsPrevented_ByMedic.Add(infectionPrevented);
        }

        public void AddCityThatCausedOutbreak(PD_City cityThatCausedOutbreak)
        {
            if (CitiesThatHaveCausedOutbreaks.Contains(cityThatCausedOutbreak) == false)
            {
                CitiesThatHaveCausedOutbreaks.Add(cityThatCausedOutbreak);
            }
        }

        public void SetFailureReason(InfectionFailureReasons failureReason)
        {
            FailureReason = failureReason;
        }

        public string GetDescription()
        {
            string description = "";

            description += "Infection Report: \n";
            description += "- Player who applied infection:";
            if (PlayerWhoAppliesInfection != null)
            {
                description += PlayerWhoAppliesInfection.Name + "\n";
            }
            else
            {
                description += "\n";
            }
            description += "- Initial City: " + InitialCity.Name + "\n";
            description += "- Infection Type: " + InfectionType + "\n";
            description += "- Initial Number of Cubes: " + InitialNumberOfCubes + "\n";
            description += "- Number of Infected Cities: " + InfectedCities.Count + "\n";
            description += "- Infected Cities: \n";
            foreach (var city in InfectedCities)
            {
                description += "- - " + city.Name + "\n";
            }
            description += "- Number of Outbreaks: " + CitiesThatHaveCausedOutbreaks.Count + "\n";
            description += "- Cities that caused outbreaks:\n";
            foreach (var city in CitiesThatHaveCausedOutbreaks)
            {
                description += "- - " + city.Name + "\n";
            }
            description += "- Infection Failure Reason: ";
            description += FailureReason.ToString() + "\n";
            description += "\n";

            return description;
        }

        #region equality override
        public bool Equals(PD_InfectionReport other)
        {
            if (this.HappenedDuringGameSetup != other.HappenedDuringGameSetup)
            {
                return false;
            }
            else if (this.PlayerWhoAppliesInfection != other.PlayerWhoAppliesInfection)
            {
                return false;
            }
            else if (this.InitialCity != other.InitialCity)
            {
                return false;
            }
            else if (this.InfectionType != other.InfectionType)
            {
                return false;
            }
            else if (this.InitialNumberOfCubes != other.InitialNumberOfCubes)
            {
                return false;
            }
            else if (this.InfectedCities != other.InfectedCities)
            {
                return false;
            }
            else if (this.InfectionsPrevented_ByMedic != other.InfectionsPrevented_ByMedic)
            {
                return false;
            }
            else if (this.CitiesThatHaveCausedOutbreaks != other.CitiesThatHaveCausedOutbreaks)
            {
                return false;
            }
            else if (this.NumberOfInfectionCubesUsed != other.NumberOfInfectionCubesUsed)
            {
                return false;
            }
            else if (this.FailureReason != other.FailureReason)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is PD_InfectionReport other_infection_report)
            {
                return Equals(other_infection_report);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 31) + (HappenedDuringGameSetup == true ? 1 : 0);
            hash = (hash * 31) + PlayerWhoAppliesInfection.GetHashCode();
            hash = (hash * 31) + InitialCity.GetHashCode();
            hash = (hash * 31) + InfectionType;
            hash = (hash * 31) + InitialNumberOfCubes;
            hash = (hash * 31) + InfectedCities.Custom_HashCode();
            hash = (hash * 31) + InfectionsPrevented_ByMedic.Custom_HashCode();
            hash = (hash * 31) + CitiesThatHaveCausedOutbreaks.Custom_HashCode();
            hash = (hash * 31) + NumberOfInfectionCubesUsed;
            hash = (hash * 31) + FailureReason.GetHashCode();

            return hash;
        }

        public static bool operator ==(PD_InfectionReport c1, PD_InfectionReport c2)
        {
            if (Object.ReferenceEquals(c1, null))
            {
                if (Object.ReferenceEquals(c2, null))
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
                if (Object.ReferenceEquals(c2, null)) // c2 is null
                {
                    return false;
                }
            }
            return c1.Equals(c2);
        }

        public static bool operator !=(PD_InfectionReport c1, PD_InfectionReport c2)
        {
            return !(c1 == c2);
        }
        #endregion


    }
}

public enum InfectionFailureReasons
{
    none,
    notEnoughDiseaseCubes,
    maximumOutbreaksSurpassed
}
