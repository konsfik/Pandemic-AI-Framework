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

        public PD_AI_Macro_Agent_Base Generator_Agent { get; private set; }
        public PD_AI_Macro_Agent_Base Mutator_Agent { get; private set; }
        public PD_AI_Macro_Agent_Base Corrector_Agent { get; private set; }

        public List<PD_GameStateEvaluator> Game_State_Evaluators { get; private set; }
        public P_RHE_Replacement_Rule Replacement_Rule { get; private set; }

        public int Genome_Length { get; private set; }
        public int Num_Generations { get; private set; }
        public double Initial_MutationRate { get; private set; }
        public double Final_MutationRate { get; private set; }
        public double Mutation_Rate { get; private set; }
        public int Num_Evaluation_Repetitions { get; private set; }

        // rolling horizon agent - report - related
        public int Num_Successful_Mutations_ThisTime { get; private set; }
        public int Num_Successful_Mutations_Different_FirstAction_ThisTime { get; private set; }

        public PolicyBased_RHE_Agent(
            PD_AI_Macro_Agent_Base generator_agent,
            PD_AI_Macro_Agent_Base mutator_agent,

            List<PD_GameStateEvaluator> game_state_evaluators,
            P_RHE_Replacement_Rule replacement_rule,

            int genome_length,
            int num_generations,
            double initial_MutationRate,
            double final_MutationRate,
            int num_evaluation_repetitions
            )
        {
            _randomnessProvider = new Random();

            Generator_Agent = generator_agent;
            Mutator_Agent = mutator_agent;

            Game_State_Evaluators = game_state_evaluators;
            Replacement_Rule = replacement_rule;

            Genome_Length = genome_length;
            Num_Generations = num_generations;
            Initial_MutationRate = initial_MutationRate;
            Final_MutationRate = final_MutationRate;
            Mutation_Rate = initial_MutationRate;
            Num_Evaluation_Repetitions = num_evaluation_repetitions;

            Num_Successful_Mutations_ThisTime = 0;
            Num_Successful_Mutations_Different_FirstAction_ThisTime = 0;
        }

        public PolicyBased_RHE_Agent(
            PD_AI_Macro_Agent_Base generator_agent,
            PD_AI_Macro_Agent_Base mutator_agent,

            List<PD_GameStateEvaluator> gameStateEvaluators,
            P_RHE_Replacement_Rule replacement_rule,

            int genome_length,
            int num_generations,
            double mutation_rate,
            int num_evaluation_repetitions
            ):this(
                generator_agent,
                mutator_agent,

                gameStateEvaluators,
                replacement_rule,

                genome_length,
                num_generations,
                mutation_rate,
                mutation_rate,
                num_evaluation_repetitions
                )
        {
            
        }

        protected override PD_MacroAction MainPlayerActions_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            return GeneralAgentBehaviour(randomness_provider, game, pathFinder);
        }

        protected override PD_MacroAction Discarding_DuringMainPlayerActions_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            return GeneralAgentBehaviour(randomness_provider, game, pathFinder);
        }

        protected override PD_MacroAction Discarding_AfterDrawing_Behaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            return GeneralAgentBehaviour(randomness_provider, game, pathFinder);
        }

        private PD_MacroAction GeneralAgentBehaviour(
            Random randomness_provider,
            PD_Game game,
            PD_AI_PathFinder pathFinder
            )
        {
            Num_Successful_Mutations_ThisTime = 0;
            Num_Successful_Mutations_Different_FirstAction_ThisTime = 0;

            PD_Game rh_gameState = game.Request_Fair_ForwardModel(randomness_provider);

            // generate parent individual
            RH_Individual parentIndividual = new RH_Individual(
                Genome_Length,
                Num_Evaluation_Repetitions
                );

            // expand the parent individual (create the genes)
            parentIndividual.Expand(
                randomness_provider,
                rh_gameState,
                pathFinder,
                Generator_Agent
                );

            // evaluate parent individual
            parentIndividual.EvaluateSelf(
                randomness_provider,
                rh_gameState,
                pathFinder,
                Generator_Agent,
                Game_State_Evaluators
                );

            string description = "  first individual evaluated: ";
            foreach (var evaluationScore in parentIndividual.EvaluationScores)
            {
                description += evaluationScore.ToString() + ", ";
            }
            Console.WriteLine(description);

            // mutate individual, until computational resources run out!
            for (int i = 0; i < Num_Generations; i++)
            {
                double percentMutations = (double)(i+1) / (double)Num_Generations;

                Mutation_Rate = MyUtilities.MapValueDouble(
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
                    randomness_provider,
                    rh_gameState,
                    pathFinder,
                    Generator_Agent,
                    Mutator_Agent,
                    Mutation_Rate,
                    _randomnessProvider
                    );

                // evaluate the child
                childIndividual.EvaluateSelf(
                    randomness_provider,
                    rh_gameState,
                    pathFinder,
                    Generator_Agent,
                    Game_State_Evaluators
                    );
                

                // compare / replace
                if (childIndividual.IsBetterThan(parentIndividual, Replacement_Rule))
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

    public enum P_RHE_Replacement_Rule {
        Average_Better, // the average of all scores needs to be better
        All_Better, // all evaluations need to be better
        One_Better__Rest_Equal_Or_Better, // one evaluation needs to be better, the rest need to be equal or better
        All_Equal_Or_Better, // all evaluations need to be equal or better
        One_Better__Rest_Whatever // one needs to be better, the rest can even be worse
    }
}
