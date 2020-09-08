using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class RH_Individual : IDescribable, ICustomDeepCopyable<RH_Individual>
    {
        public int MaxGenomeLength { get; private set; }
        public List<RH_Gene> Genome { get; private set; }
        public int NumSimulationsForEvaluation { get; private set; }
        public List<double> EvaluationScores { get; private set; }

        #region constructors
        /// <summary>
        /// normal constructor
        /// </summary>
        /// <param name="genome"></param>
        /// <param name="numSimulationsForEvaluation"></param>
        public RH_Individual(
            int maxGenomeLength,
            List<RH_Gene> genome,
            int numSimulationsForEvaluation
            )
        {
            MaxGenomeLength = maxGenomeLength;
            Genome = genome;
            NumSimulationsForEvaluation = numSimulationsForEvaluation;
            EvaluationScores = new List<double>();
        }

        public RH_Individual(
            int maxGenomeLength,
            int numSimulationsForEvaluation
            )
        {
            MaxGenomeLength = maxGenomeLength;
            Genome = new List<RH_Gene>();
            NumSimulationsForEvaluation = numSimulationsForEvaluation;
            EvaluationScores = new List<double>();
        }

        /// <summary>
        /// special constructor, for deep copy purposes only
        /// </summary>
        /// <param name="individualToCopy"></param>
        private RH_Individual(
            RH_Individual individualToCopy
            )
        {
            this.MaxGenomeLength = individualToCopy.MaxGenomeLength;
            this.Genome = individualToCopy.Genome.CustomDeepCopy();
            this.NumSimulationsForEvaluation = individualToCopy.NumSimulationsForEvaluation;
            this.EvaluationScores = new List<double>(individualToCopy.EvaluationScores);
        }
        #endregion

        public void MutateSelf(
            Random randomness_provider,
            PD_Game gameState,
            PD_AI_PathFinder pathFinder,
            PD_AI_Macro_Agent_Base defaultPolicyAgent,
            PD_AI_Macro_Agent_Base mutatorAgent,
            double mutationRate,
            Random randomnessProvider
            )
        {
            PD_Game mutationGameState = gameState.Request_Fair_ForwardModel(randomness_provider);

            bool atLeastOneMutation = false;

            for (int i = 0; i < Genome.Count; i++)
            {
                if (mutationGameState.GQ_Is_Ongoing())
                {
                    double mutationChance = randomnessProvider.NextDouble();
                    if (mutationChance < mutationRate)
                    {
                        atLeastOneMutation = true;

                        Genome[i].MutateSelf(
                            randomness_provider,
                            mutationGameState,
                            pathFinder,
                            defaultPolicyAgent,
                            mutatorAgent
                            );
                        Genome[i].ApplySelfOnGameState(
                            randomness_provider,
                            mutationGameState,
                            pathFinder,
                            defaultPolicyAgent
                            );
                    }
                    else
                    {
                        Genome[i].ApplySelfOnGameState(
                            randomness_provider,
                            mutationGameState,
                            pathFinder,
                            defaultPolicyAgent
                            );
                    }
                }
            }

            if (atLeastOneMutation == false)
            {
                PD_Game newMutationGameState = gameState.Request_Fair_ForwardModel(randomness_provider);

                int randomMutationIndex = randomnessProvider.Next(Genome.Count);
                for (int i = 0; i < Genome.Count; i++)
                {
                    if (newMutationGameState.GQ_Is_Ongoing())
                    {
                        if (i != randomMutationIndex)
                        {
                            Genome[i].ApplySelfOnGameState(
                                randomness_provider,
                                newMutationGameState,
                                pathFinder,
                                defaultPolicyAgent
                                );
                        }
                        else if (i == randomMutationIndex)
                        {
                            Genome[i].MutateSelf(
                                randomness_provider,
                                newMutationGameState,
                                pathFinder,
                                defaultPolicyAgent,
                                mutatorAgent
                                );
                            Genome[i].ApplySelfOnGameState(
                                randomness_provider,
                                newMutationGameState,
                                pathFinder,
                                defaultPolicyAgent
                                );
                        }
                    }
                }
            }

            if (
                mutationGameState.GQ_Is_Ongoing()
                && IsComplete() == false
                )
            {
                Expand(
                    randomness_provider,
                    mutationGameState,
                    pathFinder,
                    defaultPolicyAgent
                    );
            }
        }

        public void EvaluateSelf(
            Random randomness_provider,
            PD_Game initial_GameState,
            PD_AI_PathFinder pathFinder,
            PD_AI_Macro_Agent_Base defaultPolicyAgent,
            List<PD_GameStateEvaluator> gameStateEvaluators
            )
        {
            EvaluationScores = new List<double>();
            List<double> evaluationScoreSums = new List<double>();

            foreach (var evaluator in gameStateEvaluators)
            {
                EvaluationScores.Add(0.0);
                evaluationScoreSums.Add(0.0f);
            }

            for (int i = 0; i < NumSimulationsForEvaluation; i++)
            {
                PD_Game evaluation_GameState = initial_GameState.Request_Fair_ForwardModel(randomness_provider);
                foreach (var gene in Genome)
                {
                    if (evaluation_GameState.GQ_Is_Ongoing())
                    {
                        gene.ApplySelfOnGameState(
                            randomness_provider,
                            evaluation_GameState,
                            pathFinder,
                            defaultPolicyAgent
                            );
                    }
                }

                if (evaluation_GameState.GQ_Is_Ongoing() && IsComplete() == false)
                {
                    Expand(
                        randomness_provider,
                        evaluation_GameState,
                        pathFinder,
                        defaultPolicyAgent
                        );
                }

                for (int j = 0; j < gameStateEvaluators.Count; j++)
                {
                    evaluationScoreSums[j] += gameStateEvaluators[j].EvaluateGameState(evaluation_GameState);
                }
            }

            for (int j = 0; j < gameStateEvaluators.Count; j++)
            {
                EvaluationScores[j] = evaluationScoreSums[j] / (double)NumSimulationsForEvaluation;
            }
        }

        public void Expand(
            Random randomness_provider,
            PD_Game gameState,
            PD_AI_PathFinder pathFinder,
            PD_AI_Macro_Agent_Base defaultPolicyAgent
            )
        {
            PD_Game generator_GameState = gameState.Request_Fair_ForwardModel(randomness_provider);

            int initialTurn = generator_GameState.GameStateCounter.CurrentTurnIndex;
            int finalTurn = initialTurn + MaxGenomeLength - Genome.Count;
            int currentTurn = initialTurn;

            // prepare the dictionary of macros!
            Dictionary<int, List<PD_MacroAction>> macroActionsPerTurn = new Dictionary<int, List<PD_MacroAction>>();
            for (int i = initialTurn; i < finalTurn; i++)
            {
                macroActionsPerTurn.Add(
                    i,
                    new List<PD_MacroAction>()
                    );
            }

            // create the new macros and put them in the dictionary
            while (
                generator_GameState.GQ_Is_Ongoing()
                &&
                currentTurn < finalTurn
                )
            {
                var nextMacro = defaultPolicyAgent.GetNextMacroAction(
                    randomness_provider,
                    generator_GameState,
                    pathFinder
                    );

                macroActionsPerTurn[currentTurn].Add(nextMacro);

                generator_GameState.Apply_Macro_Action(
                    randomness_provider,
                    nextMacro
                    );

                currentTurn = generator_GameState.GameStateCounter.CurrentTurnIndex;
            }

            // expand the genome
            for (int i = initialTurn; i < finalTurn; i++)
            {
                if (macroActionsPerTurn[i].Count > 0)
                {
                    RH_Gene gene = new RH_Gene(
                        i,
                        macroActionsPerTurn[i]
                        );
                    Genome.Add(gene);
                }
            }
        }

        public void ResetEvaluationcores()
        {
            EvaluationScores = new List<double>();
        }

        public RH_Individual GenerateChild()
        {
            RH_Individual child = GetCustomDeepCopy();
            child.ResetEvaluationcores();
            return child;
        }

        public bool IsComplete()
        {
            return MaxGenomeLength == Genome.Count;
        }

        public bool IsBetterThan(
            RH_Individual other,
            P_RHE_Replacement_Rule replacementRule
            )
        {
            if (this.EvaluationScores.Count != other.EvaluationScores.Count)
            {
                throw new System.Exception("not equal number of evaluations");
            }

            switch (replacementRule)
            {
                case P_RHE_Replacement_Rule.Average_Better:

                    double thisIndividual_average = 0;
                    foreach (var score in EvaluationScores)
                    {
                        thisIndividual_average += score;
                    }
                    thisIndividual_average /= EvaluationScores.Count;

                    double otherIndividual_average = 0;
                    foreach (var score in other.EvaluationScores)
                    {
                        otherIndividual_average += score;
                    }
                    otherIndividual_average /= other.EvaluationScores.Count;

                    if (thisIndividual_average > otherIndividual_average)
                    {
                        return true;
                    }
                    return false;

                case P_RHE_Replacement_Rule.All_Better:
                    // all scores of this individual need to be better 
                    // than all scores of the other individual.

                    // =>

                    // if any score of the other individual is equal to or better 
                    // than any score of this individual, return false
                    for (int i = 0; i < EvaluationScores.Count; i++)
                    {
                        if (other.EvaluationScores[i] >= EvaluationScores[i])
                        {
                            return false;
                        }
                    }
                    // otherwise, return true
                    return true;

                case P_RHE_Replacement_Rule.All_Equal_Or_Better:
                    // all scores of this individual need to be equal to or better 
                    // than all scores of the other individual.

                    // =>

                    // if any score of the other individual is better 
                    // than any score of this individual, return false
                    for (int i = 0; i < EvaluationScores.Count; i++)
                    {
                        if (other.EvaluationScores[i] > EvaluationScores[i])
                        {
                            return false;
                        }
                    }
                    // otherwise, return true
                    return true;

                case P_RHE_Replacement_Rule.One_Better__Rest_Equal_Or_Better:
                    // one score of this individual needs to be better than a score of the other individual.
                    // The rest need to be equal to or better than the other's 

                    // =>

                    // check if there is one better
                    bool oneBetter = false;
                    for (int i = 0; i < EvaluationScores.Count; i++)
                    {
                        if (EvaluationScores[i] > other.EvaluationScores[i])
                        {
                            oneBetter = true;
                        }
                    }

                    // if none better, then false
                    if (oneBetter == false)
                    {
                        return false;
                    }

                    // check if all scores of this individual
                    // are equal to or better than
                    // all scores of the other individual
                    else
                    {
                        // if any score of the other individual
                        // is better than any score of this individual
                        // then return false
                        for (int i = 0; i < EvaluationScores.Count; i++)
                        {
                            if (other.EvaluationScores[i] > EvaluationScores[i])
                            {
                                return false;
                            }
                        }
                    }

                    return true;

                case P_RHE_Replacement_Rule.One_Better__Rest_Whatever:
                    // one score of this individual needs to be better than a score of the other individual.
                    // The rest can be whatever.

                    // =>

                    // check if there is one better
                    for (int i = 0; i < EvaluationScores.Count; i++)
                    {
                        if (EvaluationScores[i] > other.EvaluationScores[i])
                        {
                            return true;
                        }
                    }

                    return false;
            }

            for (int i = 0; i < EvaluationScores.Count; i++)
            {
                // if other is better than me, then false.
                // if other is equal to me, then false.
                if (other.EvaluationScores[i] >= EvaluationScores[i])
                {
                    return false;
                }
            }
            return true;
        }

        public RH_Individual GetCustomDeepCopy()
        {
            return new RH_Individual(this);
        }

        public string GetDescription()
        {
            string description = "RH_Individual:\n";
            foreach (var gene in Genome)
            {
                description += gene.GetDescription();
            }
            return description;
        }

        #region equalityOverride
        public bool Equals(RH_Individual other)
        {
            if (this.NumSimulationsForEvaluation != other.NumSimulationsForEvaluation)
            {
                return false;
            }
            if (this.EvaluationScores.SequenceEqual(other.EvaluationScores) == false)
            {
                return false;
            }

            if (this.Genome.Count != other.Genome.Count)
            {
                return false;
            }

            for (int i = 0; i < Genome.Count; i++)
            {
                if (
                    this.Genome[i] != other.Genome[i]
                    )
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is RH_Individual other_individual)
            {
                return Equals(other_individual);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + NumSimulationsForEvaluation.GetHashCode();
            hash = (hash * 13) + EvaluationScores.GetHashCode();
            foreach (var gene in Genome)
            {
                hash = (hash * 13) + gene.GetHashCode();
            }

            return hash;
        }



        public static bool operator ==(RH_Individual c1, RH_Individual c2)
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
            // c1 is not null && c2 is not null
            // -> actually check equality
            return c1.Equals(c2);
        }

        public static bool operator !=(RH_Individual c1, RH_Individual c2)
        {
            return !(c1 == c2);
        }
        #endregion
    }
}
