using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class RH_Gene : IDescribable, ICustomDeepCopyable<RH_Gene>
    {
        public int Gene_TurnIndex { get; private set; }
        public List<PD_MacroAction> MacroActions { get; private set; }

        #region constructors
        public RH_Gene(
            int gene_GameTurnIndex,
            List<PD_MacroAction> macroActions
            )
        {
            Gene_TurnIndex = gene_GameTurnIndex;
            MacroActions = macroActions;

            //CheckGeneCorrectness();
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes!
        /// </summary>
        private RH_Gene(
            RH_Gene geneToCopy
            )
        {
            this.Gene_TurnIndex = geneToCopy.Gene_TurnIndex;
            this.MacroActions = geneToCopy.MacroActions.CustomDeepCopy();
        }
        #endregion

        public void MutateSelf(
            Random randomness_provider,
            PD_Game initial_gameState,
            PD_AI_PathFinder pathFinder,
            PD_AI_Macro_Agent_Base defaultPolicyAgent,
            PD_AI_Macro_Agent_Base mutatorAgent
            )
        {
            PD_Game mutation_GameState = initial_gameState.Request_Randomized_Copy(randomness_provider);

            int numberOfMacros = MacroActions.Count;
            int mutationIndex = randomness_provider.Next(numberOfMacros);

            List<PD_MacroAction> mutated_MacroActions = new List<PD_MacroAction>();

            // first apply the non - mutated actions
            int counter = 0;
            while (Is_GameState_CorrectTurn_And_Ongoing(mutation_GameState))
            {
                if (counter < mutationIndex)
                {
                    // counter < mutation index => simply copy the existing macros into the mutated macros list.

                    PD_MacroAction currentMacro = MacroActions[counter];

                    RH_State_Type stateType = Calculate_State_Type(mutation_GameState);
                    RH_Macro_Type macroType = Calculate_Macro_Type(currentMacro);

                    RH_Macro_Apply_Type response = Calculate_Macro_Application_Type(
                        stateType,
                        macroType
                        );

                    switch (response)
                    {
                        case RH_Macro_Apply_Type.Normal:

                            // store the macro and then apply it
                            mutated_MacroActions.Add(
                                MacroActions[counter]
                                );

                            ApplyMacroOnGameState(
                                randomness_provider,
                                mutation_GameState,
                                MacroActions[counter]
                                );

                            counter++;

                            break;
                        case RH_Macro_Apply_Type.Insert:

                            PD_MacroAction actionToInsert = defaultPolicyAgent.GetNextMacroAction(
                                randomness_provider,
                                mutation_GameState,
                                pathFinder
                                );

                            // first correct the macro actions, themselves...
                            MacroActions.Insert(
                                counter,
                                actionToInsert
                                );

                            mutated_MacroActions.Add(
                                actionToInsert
                                );

                            ApplyMacroOnGameState(
                                randomness_provider,
                                mutation_GameState,
                                actionToInsert
                                );

                            counter++;

                            mutationIndex++;

                            break;
                        case RH_Macro_Apply_Type.Skip:

                            // keep the skipped action, but do not apply it!
                            mutated_MacroActions.Add(
                                MacroActions[counter]
                                );

                            counter++;

                            break;
                        case RH_Macro_Apply_Type.Error:

                            throw new System.Exception("error here!");
                    }
                }
                else if (counter == mutationIndex)
                {
                    // counter == mutation index => ask the mutator to provide a new action

                    var replacement_MacroAction = mutatorAgent.GetNextMacroAction(
                        randomness_provider,
                        mutation_GameState,
                        pathFinder
                        );

                    mutated_MacroActions.Add(replacement_MacroAction);

                    ApplyMacroOnGameState(
                        randomness_provider,
                        mutation_GameState,
                        replacement_MacroAction
                        );

                    counter++;
                }
                else
                {
                    // counter > mutation index => ask the default policy agent to provide a new action

                    var nextMacro = defaultPolicyAgent.GetNextMacroAction(
                        randomness_provider,
                        mutation_GameState,
                        pathFinder
                        );
                    if (nextMacro == null)
                    {
                        throw new System.Exception("problem here");
                    }
                    ApplyMacroOnGameState(
                        randomness_provider,
                        mutation_GameState,
                        nextMacro
                        );
                    mutated_MacroActions.Add(nextMacro);

                    counter++;
                }
            }

            MacroActions = mutated_MacroActions;
        }

        public void ApplySelfOnGameState(
            Random randomness_provider,
            PD_Game gameStateToApplySelfOn,
            PD_AI_PathFinder pathFinder,
            PD_AI_Macro_Agent_Base defaultPolicyAgent
            )
        {
            int currentMacroIndex = 0;
            while (Is_GameState_CorrectTurn_And_Ongoing(gameStateToApplySelfOn))
            {
                PD_MacroAction currentMacro;
                if (currentMacroIndex < MacroActions.Count)
                {
                    currentMacro = MacroActions[currentMacroIndex];
                }
                else
                {
                    currentMacro = null;
                }

                RH_State_Type state_Type = Calculate_State_Type(gameStateToApplySelfOn);
                RH_Macro_Type macro_Type = Calculate_Macro_Type(currentMacro);

                int currentGameState_TurnIndex = gameStateToApplySelfOn.game_state_counter.turn_index;

                RH_Macro_Apply_Type macroApplicationType = Calculate_Macro_Application_Type(
                    state_Type,
                    macro_Type
                    );

                // ACT ACCORDINGLY!
                switch (macroApplicationType)
                {
                    case RH_Macro_Apply_Type.Normal:
                        ApplyMacroOnGameState(
                            randomness_provider,
                            gameStateToApplySelfOn,
                            currentMacro
                            );
                        currentMacroIndex++;
                        break;
                    case RH_Macro_Apply_Type.Skip:
                        currentMacroIndex++;
                        break;
                    case RH_Macro_Apply_Type.Insert:
                        var missingMacro = defaultPolicyAgent.GetNextMacroAction(
                            randomness_provider,
                            gameStateToApplySelfOn,
                            pathFinder
                            );
                        RH_Macro_Type missingMacroType = Calculate_Macro_Type(missingMacro);
                        MacroActions.Insert(
                            currentMacroIndex,
                            missingMacro
                            );
                        ApplyMacroOnGameState(
                            randomness_provider,
                            gameStateToApplySelfOn,
                            missingMacro
                            );
                        currentMacroIndex++;
                        break;
                    case RH_Macro_Apply_Type.Error:
                        throw new System.Exception("something wrong here!");
                    default:
                        throw new System.Exception("something wrong here!");
                }
            }
        }

        public void ApplyMacroOnGameState(
            Random randomness_provider,
            PD_Game gameState,
            PD_MacroAction macro
            )
        {
            RH_State_Type stateType = Calculate_State_Type(gameState);
            RH_Macro_Type macroType = Calculate_Macro_Type(macro);

            bool gameStateOngoing = gameState.GQ_Is_Ongoing();

            if (gameStateOngoing == false)
            {
                return;
            }

            if (macroType == RH_Macro_Type.MainPlayerActions)
            {
                int numAvailableActions = gameState.GQ_RemainingPlayerActions_ThisRound();
                List<PD_Action> executablePart = new List<PD_Action>();
                for (int i = 0; i < numAvailableActions; i++)
                {
                    if (i < macro.Actions_All.Count)
                        executablePart.Add(macro.Actions_All[i]);
                }
                // soft apply!
                foreach (var action in executablePart)
                {
                    var availableCommands = gameState.CurrentAvailablePlayerActions;
                    if (availableCommands.Contains(action))
                    {
                        gameState.Apply_Action(
                            randomness_provider,
                            action
                            );
                    }
                    else
                    {
                        var stayAction = availableCommands.Find(
                            x =>
                            x.GetType() == typeof(PA_Stay)
                            );
                        if (stayAction != null)
                        {
                            gameState.Apply_Action(
                                randomness_provider,
                                stayAction
                                );
                        }
                        else
                        {
                            throw new System.Exception("something wrong here!");
                        }
                    }
                }
            }
            else if (macroType == RH_Macro_Type.Discarding_During)
            {
                foreach (var action in macro.Actions_All)
                {
                    var availableCommands = gameState.CurrentAvailablePlayerActions;
                    if (availableCommands.Contains(action))
                    {
                        gameState.Apply_Action(
                            randomness_provider,
                            action
                            );
                    }
                    else
                    {
                        // pick one at random!
                        gameState.Apply_Action(
                            randomness_provider,
                            availableCommands.GetOneRandom(randomness_provider)
                            );
                    }
                }
            }
            else if (macroType == RH_Macro_Type.Discarding_After)
            {
                foreach (var action in macro.Actions_All)
                {
                    var availableCommands = gameState.CurrentAvailablePlayerActions;
                    if (availableCommands.Contains(action))
                    {
                        gameState.Apply_Action(
                            randomness_provider,
                            action
                            );
                    }
                    else
                    {
                        // pick one at random!
                        gameState.Apply_Action(
                            randomness_provider,
                            availableCommands.GetOneRandom(randomness_provider)
                            );
                    }
                }
            }
            else
            {
                throw new System.Exception("something wrong here!");
            }
        }

        public RH_Macro_Apply_Type Calculate_Macro_Application_Type(
            RH_State_Type state_Type,
            RH_Macro_Type macro_Type
            )
        {
            switch (state_Type)
            {
                case RH_State_Type.NextTurn:
                    switch (macro_Type)
                    {
                        case RH_Macro_Type.MainPlayerActions:
                            return RH_Macro_Apply_Type.Error;
                        case RH_Macro_Type.Discarding_During:
                            return RH_Macro_Apply_Type.Skip;
                        case RH_Macro_Type.Discarding_After:
                            return RH_Macro_Apply_Type.Skip;
                        case RH_Macro_Type.None:
                            return RH_Macro_Apply_Type.Skip;
                        default:
                            return RH_Macro_Apply_Type.Error;
                    }
                case RH_State_Type.MainPlayerActions:
                    switch (macro_Type)
                    {
                        case RH_Macro_Type.MainPlayerActions:
                            return RH_Macro_Apply_Type.Normal;
                        case RH_Macro_Type.Discarding_During:
                            return RH_Macro_Apply_Type.Skip;
                        case RH_Macro_Type.Discarding_After:
                            return RH_Macro_Apply_Type.Insert;
                        case RH_Macro_Type.None:
                            return RH_Macro_Apply_Type.Insert;
                        default:
                            return RH_Macro_Apply_Type.Error;
                    }
                case RH_State_Type.Discarding_During:
                    switch (macro_Type)
                    {
                        case RH_Macro_Type.MainPlayerActions:
                            return RH_Macro_Apply_Type.Insert;
                        case RH_Macro_Type.Discarding_During:
                            return RH_Macro_Apply_Type.Normal;
                        case RH_Macro_Type.Discarding_After:
                            return RH_Macro_Apply_Type.Insert;
                        case RH_Macro_Type.None:
                            return RH_Macro_Apply_Type.Insert;
                        default:
                            return RH_Macro_Apply_Type.Error;
                    }
                case RH_State_Type.Discarding_After:
                    switch (macro_Type)
                    {
                        case RH_Macro_Type.MainPlayerActions:
                            return RH_Macro_Apply_Type.Error;
                        case RH_Macro_Type.Discarding_During:
                            return RH_Macro_Apply_Type.Skip;
                        case RH_Macro_Type.Discarding_After:
                            return RH_Macro_Apply_Type.Normal;
                        case RH_Macro_Type.None:
                            return RH_Macro_Apply_Type.Insert;
                        default:
                            return RH_Macro_Apply_Type.Error;
                    }
                case RH_State_Type.None:
                    switch (macro_Type)
                    {
                        case RH_Macro_Type.MainPlayerActions:
                            return RH_Macro_Apply_Type.Error;
                        case RH_Macro_Type.Discarding_During:
                            return RH_Macro_Apply_Type.Error;
                        case RH_Macro_Type.Discarding_After:
                            return RH_Macro_Apply_Type.Error;
                        case RH_Macro_Type.None:
                            return RH_Macro_Apply_Type.Error;
                        default:
                            return RH_Macro_Apply_Type.Error;
                    }
                default:
                    return RH_Macro_Apply_Type.Error;
            }
        }

        private RH_State_Type Calculate_State_Type(
            PD_Game game
            )
        {
            if (
                game.game_state_counter.turn_index > Gene_TurnIndex
                || game.GQ_Is_Lost()
                || game.GQ_Is_Won()
                )
            {
                return RH_State_Type.NextTurn;
            }
            else if (
                game.game_FSM.CurrentState.GetType() == typeof(PD_GS_ApplyingMainPlayerActions)
                )
            {
                return RH_State_Type.MainPlayerActions;
            }
            else if (
                game.game_FSM.CurrentState.GetType() == typeof(PD_GS_Discarding_DuringMainPlayerActions)
                )
            {
                return RH_State_Type.Discarding_During;
            }
            else if (
                game.game_FSM.CurrentState.GetType() == typeof(PD_GS_Discarding_AfterDrawing)
                )
            {
                return RH_State_Type.Discarding_After;
            }
            else
            {
                return RH_State_Type.None;
            }
        }

        private RH_Macro_Type Calculate_Macro_Type(
            PD_MacroAction macroAction
            )
        {
            if (macroAction == null)
            {
                return RH_Macro_Type.None;
            }
            else if (
                macroAction.MacroAction_Type == PD_MacroAction_Type.Stay_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.Walk_Macro

                || macroAction.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro

                || macroAction.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro

                || macroAction.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro

                || macroAction.MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro

                || macroAction.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Macro
                || macroAction.MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Scientist_Macro
                )
            {
                return RH_Macro_Type.MainPlayerActions;
            }
            else if (
                macroAction.MacroAction_Type == PD_MacroAction_Type.Discard_DuringMainPlayerActions_Macro
                )
            {
                return RH_Macro_Type.Discarding_During;
            }
            else if (
                macroAction.MacroAction_Type == PD_MacroAction_Type.Discard_AfterDrawing_Macro
                )
            {
                return RH_Macro_Type.Discarding_After;
            }
            else
            {
                throw new System.Exception("something wrong here!");
            }
        }

        private bool Is_GameState_CorrectTurn_And_Ongoing(
            PD_Game gameState
            )
        {
            bool correctTurn = gameState.game_state_counter.turn_index == Gene_TurnIndex;
            bool ongoing = gameState.GQ_Is_Ongoing();
            return correctTurn && ongoing;
        }

        #region various
        public RH_Gene GetCustomDeepCopy()
        {
            return new RH_Gene(this);
        }

        public string GetDescription()
        {
            string description = "RH_Individual_Gene: \n";
            foreach (var macro in MacroActions)
            {
                description += " " + macro.GetDescription() + "\n";
            }
            return description;
        }
        #endregion


        #region equalityOverride
        public bool Equals(RH_Gene other)
        {
            if (this.Gene_TurnIndex != other.Gene_TurnIndex)
            {
                return false;
            }

            if (this.MacroActions.Count != other.MacroActions.Count)
            {
                return false;
            }

            for (int i = 0; i < MacroActions.Count; i++)
            {
                if (
                    this.MacroActions[i] != other.MacroActions[i]
                    )
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is RH_Gene other_gene)
            {
                return Equals(other_gene);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + Gene_TurnIndex.GetHashCode();
            foreach (var macro in MacroActions)
            {
                hash = (hash * 13) + macro.GetHashCode();
            }

            return hash;
        }



        public static bool operator ==(RH_Gene c1, RH_Gene c2)
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

        public static bool operator !=(RH_Gene c1, RH_Gene c2)
        {
            return !(c1 == c2);
        }
        #endregion
    }

    public enum RH_State_Type
    {
        None,
        NextTurn,
        MainPlayerActions,
        Discarding_During,
        Discarding_After
    }

    public enum RH_Macro_Type
    {
        None,
        MainPlayerActions,
        Discarding_During,
        Discarding_After
    }

    public enum RH_Macro_Apply_Type
    {
        Normal,
        Skip,
        Insert,
        Error
    }
}
