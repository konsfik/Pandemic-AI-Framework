using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_InfectionReport : IDescribable, ICustomDeepCopyable<PD_InfectionReport>
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

        public PD_InfectionReport GetCustomDeepCopy()
        {
            return new PD_InfectionReport(this);
        }
    }
}

public enum InfectionFailureReasons
{
    none,
    notEnoughDiseaseCubes,
    maximumOutbreaksSurpassed
}
