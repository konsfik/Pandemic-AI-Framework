using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PolicyBased_RHE_Agent : PD_AI_Macro_Agent_Base
    {
        private Random _randomnessProvider;

        public PD_AI_Macro_Agent_Base Creator_Agent { get; private set; }
        public PD_AI_Macro_Agent_Base Mutator_Agent { get; private set; }
        public PD_AI_Macro_Agent_Base Corrector_Agent { get; private set; }

        public List<PD_GameStateEvaluator> GameStateEvaluators { get; private set; }
        public RollingHorizonAgent_IndividualReplacementRule ReplacementRule { get; private set; }

        public int MaxGenomeLength { get; private set; }
        public int NumMutations { get; private set; }
        public double Initial_MutationRate { get; private set; }
        public double Final_MutationRate { get; private set; }
        public double MutationRate { get; private set; }
        public int NumSimulationsPerEvaluation { get; private set; }

        // rolling horizon agent - report - related
        public int Num_Successful_Mutations_ThisTime { get; private set; }
        public int Num_Successful_Mutations_Different_FirstAction_ThisTime { get; private set; }

        public PolicyBased_RHE_Agent(
            PD_AI_Macro_Agent_Base defaultPolicy_Agent,
            PD_AI_Macro_Agent_Base mutator_Agent,

            List<PD_GameStateEvaluator> gameStateEvaluators,
            RollingHorizonAgent_IndividualReplacementRule replacementRule,

            int maxGenomeLength,
            int numMutations,
            double initial_MutationRate,
            double final_MutationRate,
            int numSimulationsPerEvaluation
            )
        {
            _randomnessProvider = new Random();

            Creator_Agent = defaultPolicy_Agent;
            Mutator_Agent = mutator_Agent;

            GameStateEvaluators = gameStateEvaluators;
            ReplacementRule = replacementRule;

            MaxGenomeLength = maxGenomeLength;
            NumMutations = numMutations;
            Initial_MutationRate = initial_MutationRate;
            Final_MutationRate = final_MutationRate;
            MutationRate = initial_MutationRate;
            NumSimulationsPerEvaluation = numSimulationsPerEvaluation;

            Num_Successful_Mutations_ThisTime = 0;
            Num_Successful_Mutations_Different_FirstAction_ThisTime = 0;
        }

        public PolicyBased_RHE_Agent(
            PD_AI_Macro_Agent_Base defaultPolicy_Agent,
            PD_AI_Macro_Agent_Base mutator_Agent,

            List<PD_GameStateEvaluator> gameStateEvaluators,
            RollingHorizonAgent_IndividualReplacementRule replacementRule,

            int maxGenomeLength,
            int numMutations,
            double initial_final_mutationRate,
            int numSimulationsPerEvaluation
            ):this(
                defaultPolicy_Agent,
                mutator_Agent,

                gameStateEvaluators,
                replacementRule,

                maxGenomeLength,
                numMutations,
                initial_final_mutationRate,
                initial_final_mutationRate,
                numSimulationsPerEvaluation
                )
        {
            
        }

        protected override PD_MacroAction MainPlayerActions_Behaviour(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            return GeneralAgentBehaviour(game, pathFinder);
        }

        protected override PD_MacroAction Discarding_DuringMainPlayerActions_Behaviour(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            return GeneralAgentBehaviour(game, pathFinder);
        }

        protected override PD_MacroAction Discarding_AfterDrawing_Behaviour(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            return GeneralAgentBehaviour(game, pathFinder);
        }

        private PD_MacroAction GeneralAgentBehaviour(
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            Num_Successful_Mutations_ThisTime = 0;
            Num_Successful_Mutations_Different_FirstAction_ThisTime = 0;

            PD_Game rh_gameState = game.Request_Fair_ForwardModel();

            // generate parent individual
            RH_Individual parentIndividual = new RH_Individual(
                MaxGenomeLength,
                NumSimulationsPerEvaluation
                );

            // expand the parent individual (create the genes)
            parentIndividual.Expand(
                rh_gameState,
                pathFinder,
                Creator_Agent
                );

            // evaluate parent individual
            parentIndividual.EvaluateSelf(
                rh_gameState,
                pathFinder,
                Creator_Agent,
                GameStateEvaluators
                );

            string description = "  first individual evaluated: ";
            foreach (var evaluationScore in parentIndividual.EvaluationScores)
            {
                description += evaluationScore.ToString() + ", ";
            }
            Console.WriteLine(description);

            // mutate individual, until computational resources run out!
            for (int i = 0; i < NumMutations; i++)
            {
                double percentMutations = (double)(i+1) / (double)NumMutations;

                MutationRate = MyUtilities.MapValueDouble(
                    percentMutations,
                    0.0,
                    1.0,
                    Initial_MutationRate,
                    Final_MutationRate
                    );

                // create child by copying the parent
                RH_Individual childIndividual = parentIndividual.GenerateChild();

                // mutate the child
                childIndividual.MutateSelf(
                    rh_gameState,
                    pathFinder,
                    Creator_Agent,
                    Mutator_Agent,
                    MutationRate,
                    _randomnessProvider
                    );

                // evaluate the child
                childIndividual.EvaluateSelf(
                        rh_gameState,
                        pathFinder,
                        Creator_Agent,
                        GameStateEvaluators
                        );
                

                // compare / replace
                if (childIndividual.IsBetterThan(parentIndividual, ReplacementRule))
                {
                    Num_Successful_Mutations_ThisTime++;
                    bool firstActionSame =
                        childIndividual.Genome[0].MacroActions[0] == parentIndividual.Genome[0].MacroActions[0];
                    if (!firstActionSame) {
                        Num_Successful_Mutations_Different_FirstAction_ThisTime++;
                    }
                    string childDescription = "  found better child: ";
                    foreach (var eval in childIndividual.EvaluationScores)
                    {
                        childDescription += eval.ToString() + ", ";
                    }
                    childDescription += (firstActionSame ? " first action - SAME" : " first action - DIFFERENT");
                    Console.WriteLine(childDescription);

                    parentIndividual = childIndividual.GetCustomDeepCopy();
                }
            }

            Console.WriteLine("num successful mutations: " 
                + Num_Successful_Mutations_ThisTime.ToString());
            Console.WriteLine("num successful mutations - different first action: " 
                + Num_Successful_Mutations_Different_FirstAction_ThisTime.ToString());


            // return the first macro of the first gene of the parent
            return parentIndividual.Genome[0].MacroActions[0];
        }

        public override string GetDescription()
        {
            return "";
        }
    }

    public enum RollingHorizonAgent_IndividualReplacementRule {
        Average_Better, // the average of all scores needs to be better
        All_Better, // all evaluations need to be better
        One_Better__Rest_Equal_Or_Better, // one evaluation needs to be better, the rest need to be equal or better
        All_Equal_Or_Better, // all evaluations need to be equal or better
        One_Better__Rest_Whatever // one needs to be better, the rest can even be worse
    }
}
