using System;
using System.Collections.Generic;
using System.Text;
using Pandemic_AI_Framework;

namespace Experiments_Runner
{
    public static class AgentsFactory
    {
        #region simple agents
        /// <summary>
        /// An agent that does nothing at all. 
        /// It "operates" within the action - based representation of the game.
        /// </summary>
        /// <returns></returns>
        public static PD_AI_Agent_Base ActionAgent_Idle()
        {
            return new PD_AI_ActionAgent_Idle();
        }

        /// <summary>
        /// A random agent that operates within the action - based representation of the game.
        /// It simply returns one of the available actions, completely at random.
        /// </summary>
        /// <returns></returns>
        public static PD_AI_Agent_Base ActionAgent_Random()
        {
            return new PD_AI_ActionAgent_Random();
        }

        /// <summary>
        /// A Hierarchical Policy Agent with an optimized set of included policies.
        /// This is to be used either on its own, or as the creator agent of the P_RHE_Agent
        /// </summary>
        /// <returns></returns>
        public static PD_AI_Agent_Base HierarchicalPolicyAgent()
        {
            return new PD_AI_MacroAgent_HierarchicalPolicy(
                new List<PD_AI_MacroAgent_MainPolicy_Base>() {
                    new PD_AI_MacroAgent_Policy_Cure_ASAP(),
                    new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(3),
                    new PD_AI_MacroAgent_Policy_ShareKnowledge_1_Variation_A(),
                    new PD_AI_MacroAgent_Policy_BuildRS_MaxTotal_MinDistance(6,3),
                    new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(2),
                    new PD_AI_MacroAgent_Policy_Treat_MinSameCubes_Now_Smart(1),
                    new PD_AI_MacroAgent_Policy_WalkAway()
                },
                new List<PD_AI_MacroAgent_DiscardPolicy_Base>() {
                    new PD_AI_MacroAgent_Policy_CardDiscard_Smart()
                }
                );
        }

        /// <summary>
        /// A Random Policy Agent that uses the same policies as the HPA, 
        /// but with randomized priority.
        /// This is to be used as the P_RHE_Agent's mutator agent.
        /// </summary>
        /// <returns></returns>
        public static PD_AI_Agent_Base RandomPolicyAgent()
        {
            return new PD_AI_MacroAgent_RandomPolicy(
                new List<PD_AI_MacroAgent_MainPolicy_Base>() {
                    new PD_AI_MacroAgent_Policy_Cure_ASAP(),
                    new PD_AI_MacroAgent_Policy_Treat_Hierarchical_Smart(),
                    new PD_AI_MacroAgent_Policy_ShareKnowledge_1_Variation_A(),
                    new PD_AI_MacroAgent_Policy_BuildRS_MaxTotal_MinDistance(6,3)
                },

                new PD_AI_MacroAgent_Policy_RandomAny_Now(),

                new List<PD_AI_MacroAgent_DiscardPolicy_Base>() {
                    new PD_AI_MacroAgent_Policy_CardDiscard_Random()
                }
                );
        }
        #endregion

        /// <summary>
        /// The Policy Based Rolling Horizon Evolution agent, 
        /// with optimized parameters.
        /// </summary>
        /// <param name="pathFinder"></param>
        /// <returns></returns>
        public static PD_AI_Agent_Base P_RHE_Agent(PD_AI_PathFinder pathFinder)
        {
            PolicyBased_RHE_Agent agent =
                new PolicyBased_RHE_Agent(
                    (PD_AI_Macro_Agent_Base)HierarchicalPolicyAgent(),
                    (PD_AI_Macro_Agent_Base)RandomPolicyAgent(),

                    // list of game state evaluators
                    new List<PD_GameStateEvaluator>() {
                        // only one aggregated game state evaluator is used
                        new GSE_Aggregated_WinOrDefeat_Adjusted(
                            // list of scores to be aggregated
                            new List<GameStateEvaluationScore>()
                            { 
                                // gradient cured diseases score
                                new GSES_GradientCuredDiseases(),
                                // multiplied remaining disease cubes score
                                new GSES_MultipliedRemainingDiseaseCubes()
                            }
                            )
                    },

                    // since only one evaluator is used,
                    // the replacement takes place when the child's evaluation is better than the parent's
                    P_RHE_Replacement_Rule.All_Better,

                    5,      // max genome length
                    100,     // num mutations
                    1.0,   // initial mutation rate
                    0.5,   // final mutation rate
                    5     // num simulations per evaluation
                 );

            return agent;
        }
    }
}
