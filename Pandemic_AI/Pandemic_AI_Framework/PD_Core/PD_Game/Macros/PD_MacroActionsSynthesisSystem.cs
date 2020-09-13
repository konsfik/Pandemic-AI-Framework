using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_MacroActionsSynthesisSystem
    {
        public static List<PD_MacroAction> FindAll_Macros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<int> researchStationCities
            )
        {
            int numAvailableActions = game.GQ_RemainingPlayerActions_ThisRound();

            List<PD_MacroAction> allMacros = new List<PD_MacroAction>();

            if (game.GQ_IsInState_ApplyingMainPlayerActions())
            {
                ////////////////////////////////////////////////////////////////
                /// OTHER REUSABLE DATA
                ////////////////////////////////////////////////////////////////
                int currentPlayer = game.GQ_CurrentPlayer();
                int currentPlayerLocation = game.GQ_CurrentPlayer_Location();
                int currentPlayerRole = game.GQ_CurrentPlayer_Role();
                List<int> allCitiesExceprCurrentLocation = game.map.cities.FindAll(
                    x =>
                    x != currentPlayerLocation
                    );
                ////////////////////////////////////////////////////////////////
                /// WALK SEQUENCES
                ////////////////////////////////////////////////////////////////

                // all walk sequences
                List<List<PD_Action>> all_SimpleWalk_Sequences =
                    FindAll_SimpleWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities
                        );
                List<List<PD_Action>> all_DirectFlightWalk_Sequences =
                    FindAll_DirectFlightWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities,

                        currentPlayer,
                        currentPlayerLocation,

                        all_SimpleWalk_Sequences
                        );
                List<List<PD_Action>> all_CharterFlightWalk_Sequences =
                    FindAll_CharterFlightWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities,

                        currentPlayer,
                        currentPlayerLocation,

                        all_SimpleWalk_Sequences
                        );
                List<List<PD_Action>> all_operationsExpertFlightWalk_Sequences =
                    FindAll_OperationsExpertFlightWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities,

                        currentPlayer,
                        currentPlayerLocation,

                        all_SimpleWalk_Sequences
                        );

                // walk sequences of maximum executable length
                List<List<PD_Action>> simpleWalk_Sequences_MaxExecutableLength =
                    all_SimpleWalk_Sequences.FindAll(
                        x =>
                        x.Count == numAvailableActions
                        );
                List<List<PD_Action>> directFlightWalk_Sequences_MaxExecutableLength =
                    all_DirectFlightWalk_Sequences.FindAll(
                        x =>
                        x.Count == numAvailableActions
                        );
                List<List<PD_Action>> charterFlightWalk_Sequences_MaxExecutableLength =
                    all_CharterFlightWalk_Sequences.FindAll(
                        x =>
                        x.Count == numAvailableActions
                        );
                List<List<PD_Action>> operationsExpertFlightWalk_Sequences_MaxExecutableLength =
                    all_operationsExpertFlightWalk_Sequences.FindAll(
                        x =>
                        x.Count == numAvailableActions
                        );

                // walk sequences of less than maximum executable length
                //List<List<PD_GameAction_Base>> simpleWalk_Sequences_LessThan_MaxExecutableLength =
                //    all_SimpleWalk_Sequences.FindAll(
                //        x =>
                //        x.Count < numAvailableActions
                //        );
                //List<List<PD_GameAction_Base>> directFlightWalk_Sequences_LessThan_MaxExecutableLength =
                //    all_DirectFlightWalk_Sequences.FindAll(
                //        x =>
                //        x.Count < numAvailableActions
                //        );
                //List<List<PD_GameAction_Base>> charterFlightWalk_Sequences_LessThan_MaxExecutableLength =
                //    all_CharterFlightWalk_Sequences.FindAll(
                //        x =>
                //        x.Count < numAvailableActions
                //        );
                //List<List<PD_GameAction_Base>> operationsExpertFlightWalk_Sequences_LessThan_MaxExecutableLength =
                //    all_operationsExpertFlightWalk_Sequences.FindAll(
                //        x =>
                //        x.Count < numAvailableActions
                //        );

                // WALK MACROS
                List<PD_MacroAction> walk_Macros =
                    FindAll_WalkMacros_ExecutableNow(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        simpleWalk_Sequences_MaxExecutableLength,
                        directFlightWalk_Sequences_MaxExecutableLength,
                        charterFlightWalk_Sequences_MaxExecutableLength,
                        operationsExpertFlightWalk_Sequences_MaxExecutableLength
                        );

                // TREAT DISEASE MACROS (simple & medic)
                List<PD_MacroAction> treatDisease_Macros =
                    FindAll_TreatDiseaseMacros(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        all_SimpleWalk_Sequences,
                        all_DirectFlightWalk_Sequences,
                        all_CharterFlightWalk_Sequences,
                        all_operationsExpertFlightWalk_Sequences
                        );

                // AUTO TREAT DISEASE MACROS (medic)
                List<PD_MacroAction> autoTreatDisease_Medic_Macros =
                    FindAll_AutoTreatDisease_Medic_Macros(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        all_SimpleWalk_Sequences,
                        all_DirectFlightWalk_Sequences,
                        all_CharterFlightWalk_Sequences,
                        all_operationsExpertFlightWalk_Sequences
                        );

                // BUILD RESEARCH STATION MACROS (normal && operations expert)
                List<PD_MacroAction> buildResearchStation_Macros =
                    FindAll_BuildResearchStationMacros(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        all_SimpleWalk_Sequences,
                        all_DirectFlightWalk_Sequences,
                        all_CharterFlightWalk_Sequences,
                        all_operationsExpertFlightWalk_Sequences
                        );

                // MOVE RESEARCH STATION MACROS
                List<PD_MacroAction> moveResearchStation_Macros =
                    FindAll_MoveResearchStationMacros(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        all_SimpleWalk_Sequences,
                        all_DirectFlightWalk_Sequences,
                        all_CharterFlightWalk_Sequences,
                        all_operationsExpertFlightWalk_Sequences
                        );

                // SHARE KNOWLEDGE GIVE MACROS (simple && take position - give && researcher gives)
                List<PD_MacroAction> shareKnowledge_Give_Macros =
                    FindAll_ShareKnowledge_Give_Macros(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        all_SimpleWalk_Sequences,
                        all_DirectFlightWalk_Sequences,
                        all_CharterFlightWalk_Sequences,
                        all_operationsExpertFlightWalk_Sequences
                        );

                // SHARE KNOWLEDGE TAKE MACROS (simple && take position - take && take from researcher)
                List<PD_MacroAction> shareKnowledge_Take_Macros =
                    FindAll_ShareKnowledge_Take_Macros(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        all_SimpleWalk_Sequences,
                        all_DirectFlightWalk_Sequences,
                        all_CharterFlightWalk_Sequences,
                        all_operationsExpertFlightWalk_Sequences
                        );

                // DISCOVER CURE MACROS (simple && scientist)
                List<PD_MacroAction> discoverCure_Macros =
                    FindAll_DiscoverCureMacros(
                        game,
                        pathFinder,

                        currentPlayer,
                        currentPlayerLocation,
                        currentPlayerRole,
                        numAvailableActions,

                        all_SimpleWalk_Sequences,
                        all_DirectFlightWalk_Sequences,
                        all_CharterFlightWalk_Sequences,
                        all_operationsExpertFlightWalk_Sequences
                        );

                allMacros.AddRange(walk_Macros);
                //allMacros.AddRange(stay_Macros);

                allMacros.AddRange(treatDisease_Macros);
                allMacros.AddRange(autoTreatDisease_Medic_Macros);

                allMacros.AddRange(buildResearchStation_Macros);
                allMacros.AddRange(moveResearchStation_Macros);

                allMacros.AddRange(shareKnowledge_Give_Macros);
                allMacros.AddRange(shareKnowledge_Take_Macros);

                allMacros.AddRange(discoverCure_Macros);

            }
            else if (game.GQ_IsInState_Discard_Any())
            {
                var discardMacros = Find_CardDiscard_Macros_ExecutableNow(
                    game
                    );
                allMacros.AddRange(discardMacros);
            }

            return allMacros;

        }

        public static List<PD_MacroAction> Find_CardDiscard_Macros_ExecutableNow(
            PD_Game game
            )
        {
            List<PD_MacroAction> macros = new List<PD_MacroAction>();
            if (game.GQ_IsInState_DiscardAfterDrawing())
            {
                var availableCommands = game.CurrentAvailablePlayerActions;

                foreach (var command in availableCommands)
                {
                    if (command.GetType() == typeof(PA_Discard_AfterDrawing))
                    {
                        var commandSequence = new List<PD_Action>();
                        commandSequence.Add(command);
                        PD_MacroAction mc = new PD_MacroAction(
                            commandSequence,
                            PD_MacroAction_Type.Discard_AfterDrawing_Macro,
                            PD_MacroAction_WalkType.None,
                            0
                            );
                        macros.Add(mc);
                    }
                }

            }
            else if (game.GQ_IsInState_DiscardDuringMainPlayerActions())
            {
                var availableCommands = game.CurrentAvailablePlayerActions;

                foreach (var command in availableCommands)
                {
                    var macroType = command.GetType();
                    if (macroType == typeof(PA_Discard_DuringMainPlayerActions))
                    {
                        var commandSequence = new List<PD_Action>();
                        commandSequence.Add(command);
                        PD_MacroAction mc = new PD_MacroAction(
                            commandSequence,
                            PD_MacroAction_Type.Discard_DuringMainPlayerActions_Macro,
                            PD_MacroAction_WalkType.None,
                            0
                            );
                        macros.Add(mc);
                    }
                }
            }

            return macros;
        }

        #region walkMacros_executableNow
        public static List<PD_MacroAction> FindAll_WalkMacros_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,
            List<List<PD_Action>> simpleWalk_ExecutableNow_ActionSequences_MaximumLength,
            List<List<PD_Action>> directFlightWalk_ExecutableNow_ActionSequences_MaximumLength,
            List<List<PD_Action>> charterFlightWalk_ExecutableNow_ActionSequences_MaximumLength,
            List<List<PD_Action>> operationsExpertFlightWalk_ExecutableNow_ActionSequences_MaximumLength
            )
        {
            List<PD_MacroAction> walkMacros = new List<PD_MacroAction>();
            walkMacros.AddRange(
                Find_WalkMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    simpleWalk_ExecutableNow_ActionSequences_MaximumLength,
                    PD_MacroAction_WalkType.SimpleWalk
                    )
                );
            walkMacros.AddRange(
                Find_WalkMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    directFlightWalk_ExecutableNow_ActionSequences_MaximumLength,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    )
                );
            walkMacros.AddRange(
                Find_WalkMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    charterFlightWalk_ExecutableNow_ActionSequences_MaximumLength,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    )
                );
            walkMacros.AddRange(
                Find_WalkMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    operationsExpertFlightWalk_ExecutableNow_ActionSequences_MaximumLength,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    )
                );
            return walkMacros;
        }

        public static List<PD_MacroAction> Find_WalkMacros_SpecificWalkType_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences_MaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            int numRemainingActions = 4 - game.game_state_counter.player_action_index;

            List<PD_MacroAction> walkMacros_SimpleWalk_ExecutableNow = new List<PD_MacroAction>();

            foreach (var seq in walkSequences_MaximumLength)
            {
                PD_MacroAction ma = new PD_MacroAction(
                    seq,
                    PD_MacroAction_Type.Walk_Macro,
                    walkType,
                    numAvailableActions
                    );
                walkMacros_SimpleWalk_ExecutableNow.Add(ma);
            }

            return walkMacros_SimpleWalk_ExecutableNow;
        }
        #endregion

        #region stayMacros_ExecutableNow
        public static List<PD_MacroAction> FindAll_StayMacros_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength
            )
        {
            List<PD_MacroAction> stayMacros = new List<PD_MacroAction>();
            List<PD_MacroAction> stayMacros_Immediate_ExecutableNow = Find_StayMacros_Immediate_ExecutableNow(
                game,
                pathFinder,

                currentPlayer,
                currentPlayerLocation,
                currentPlayerRole,
                numAvailableActions
                );
            List<PD_MacroAction> stayMacros_SimpleWalk_ExecutableNow = Find_StayMacros_SpecificWalkType_ExecutableNow(
                game,
                pathFinder,

                currentPlayer,
                currentPlayerLocation,
                currentPlayerRole,
                numAvailableActions,

                simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
                PD_MacroAction_WalkType.SimpleWalk
                );
            List<PD_MacroAction> stayMacros_DirectFlightWalk_ExecutableNow = Find_StayMacros_SpecificWalkType_ExecutableNow(
                game,
                pathFinder,

                currentPlayer,
                currentPlayerLocation,
                currentPlayerRole,
                numAvailableActions,

                directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                PD_MacroAction_WalkType.DirectFlightWalk
                );
            List<PD_MacroAction> stayMacros_CharterFlightWalk_ExecutableNow = Find_StayMacros_SpecificWalkType_ExecutableNow(
                game,
                pathFinder,

                currentPlayer,
                currentPlayerLocation,
                currentPlayerRole,
                numAvailableActions,

                charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                PD_MacroAction_WalkType.CharterFlightWalk
                );
            List<PD_MacroAction> stayMacros_OperationsExpertFlightWalk_ExecutableNow = Find_StayMacros_SpecificWalkType_ExecutableNow(
                game,
                pathFinder,

                currentPlayer,
                currentPlayerLocation,
                currentPlayerRole,
                numAvailableActions,

                operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                PD_MacroAction_WalkType.OperationsExpertFlightWalk
                );

            // remove the stay macros that can be done with simple walk from all other walk types... 
            List<PD_MacroAction> stayMacros_DirectFlightWalk_ExecutableNow_Filtered = new List<PD_MacroAction>();
            foreach (var macro in stayMacros_DirectFlightWalk_ExecutableNow)
            {
                int destination = macro.Find_Destination();
                PD_MacroAction equivalentSimpleWalkMacro = stayMacros_SimpleWalk_ExecutableNow.Find(
                    x =>
                    x.Find_Destination() == destination
                    );
                if (equivalentSimpleWalkMacro == null)
                {
                    stayMacros_DirectFlightWalk_ExecutableNow_Filtered.Add(macro);
                }
            }

            List<PD_MacroAction> stayMacros_CharterFlightWalk_ExecutableNow_Filtered = new List<PD_MacroAction>();
            foreach (var macro in stayMacros_CharterFlightWalk_ExecutableNow)
            {
                int destination = macro.Find_Destination();
                PD_MacroAction equivalentSimpleWalkMacro = stayMacros_SimpleWalk_ExecutableNow.Find(
                    x =>
                    x.Find_Destination() == destination
                    );
                if (equivalentSimpleWalkMacro == null)
                {
                    stayMacros_CharterFlightWalk_ExecutableNow_Filtered.Add(macro);
                }
            }

            List<PD_MacroAction> stayMacros_OperationsExpertFlightWalk_ExecutableNow_Filtered = new List<PD_MacroAction>();
            foreach (var macro in stayMacros_OperationsExpertFlightWalk_ExecutableNow)
            {
                int destination = macro.Find_Destination();
                PD_MacroAction equivalentSimpleWalkMacro = stayMacros_SimpleWalk_ExecutableNow.Find(
                    x =>
                    x.Find_Destination() == destination
                    );
                if (equivalentSimpleWalkMacro == null)
                {
                    stayMacros_OperationsExpertFlightWalk_ExecutableNow_Filtered.Add(macro);
                }
            }

            stayMacros.AddRange(stayMacros_Immediate_ExecutableNow);
            stayMacros.AddRange(stayMacros_SimpleWalk_ExecutableNow);
            stayMacros.AddRange(stayMacros_DirectFlightWalk_ExecutableNow_Filtered);
            stayMacros.AddRange(stayMacros_CharterFlightWalk_ExecutableNow_Filtered);
            stayMacros.AddRange(stayMacros_OperationsExpertFlightWalk_ExecutableNow_Filtered);
            return stayMacros;
        }

        public static List<PD_MacroAction> Find_StayMacros_Immediate_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions
            )
        {
            List<PD_Action> actionsList = new List<PD_Action>();
            for (int i = 0; i < numAvailableActions; i++)
            {
                actionsList.Add(
                    new PA_Stay(
                        currentPlayer,
                        currentPlayerLocation
                        )
                    );
            }

            PD_MacroAction stayMacro = new PD_MacroAction(
                actionsList,
                PD_MacroAction_Type.Stay_Macro,
                PD_MacroAction_WalkType.None,
                numAvailableActions
                );

            return new List<PD_MacroAction>() {
                stayMacro
            };
        }

        public static List<PD_MacroAction> Find_StayMacros_SpecificWalkType_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            List<PD_MacroAction> stayMacros_ExecutableNow = new List<PD_MacroAction>();

            foreach (var seq in walkSequences_NonMaximumLength)
            {
                int numRemainingActions = numAvailableActions - seq.Count;

                int player = ((I_Player_Action)seq.GetLast()).Player;
                int destination = ((I_Movement_Action)(seq.GetLast())).ToCity;

                List<PD_Action> completeSequence = new List<PD_Action>();
                completeSequence.AddRange(seq);

                for (int i = 0; i < numRemainingActions; i++)
                {
                    PA_Stay stayAction = new PA_Stay(
                        player,
                        destination
                        );
                    completeSequence.Add(stayAction);
                }

                PD_MacroAction ma = new PD_MacroAction(
                    completeSequence,
                    PD_MacroAction_Type.Stay_Macro,
                    walkType,
                    numAvailableActions
                    );
                stayMacros_ExecutableNow.Add(ma);
            }

            return stayMacros_ExecutableNow;
        }
        #endregion

        #region treatDiseaseMacros_ExecutableNow
        public static List<PD_MacroAction> FindAll_TreatDiseaseMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_ActionSequences,
            List<List<PD_Action>> directFlightWalk_ActionSequences,
            List<List<PD_Action>> charterFlightWalk_ActionSequences,
            List<List<PD_Action>> operationsExpertFlightWalk_ActionSequences
            )
        {
            var treatDiseaseMacros = new List<PD_MacroAction>();

            List<PD_MacroAction> treatDiseaseMacros_Immediate =
                Find_TreatDiseaseMacros_Immediate(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions
                    );
            List<PD_MacroAction> treatDiseaseMacros_SimpleWalk_ExecutableNow =
                Find_TreatDiseaseMacros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    simpleWalk_ActionSequences,
                    PD_MacroAction_WalkType.SimpleWalk
                    );
            List<PD_MacroAction> treatDiseaseMacros_DirectFlightWalk_ExecutableNow =
                Find_TreatDiseaseMacros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    directFlightWalk_ActionSequences,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    );
            List<PD_MacroAction> treatDiseaseMacros_CharterFlightWalk_ExecutableNow =
                Find_TreatDiseaseMacros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    charterFlightWalk_ActionSequences,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    );
            List<PD_MacroAction> treatDiseaseMacros_OperationsExpertFlightWalk_ExecutableNow =
                Find_TreatDiseaseMacros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    operationsExpertFlightWalk_ActionSequences,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    );

            treatDiseaseMacros.AddRange(treatDiseaseMacros_Immediate);
            treatDiseaseMacros.AddRange(treatDiseaseMacros_SimpleWalk_ExecutableNow);
            treatDiseaseMacros.AddRange(treatDiseaseMacros_DirectFlightWalk_ExecutableNow);
            treatDiseaseMacros.AddRange(treatDiseaseMacros_CharterFlightWalk_ExecutableNow);
            treatDiseaseMacros.AddRange(treatDiseaseMacros_OperationsExpertFlightWalk_ExecutableNow);

            return treatDiseaseMacros;
        }

        public static List<PD_MacroAction> Find_TreatDiseaseMacros_Immediate(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions
            )
        {
            if (numAvailableActions < 1)
            {
                return new List<PD_MacroAction>();
            }

            bool currentPlayerIsMedic = currentPlayerRole == PD_Player_Roles.Medic;

            List<PD_MacroAction> treatDiseaseMacros = new List<PD_MacroAction>();

            for (int infectionType = 0; infectionType < 4; infectionType++)
            {
                bool currentPlayerLocationIsInfectedWithThisType
                    = game.map_elements.infections__per__type__per__city
                    [currentPlayerLocation]
                    [infectionType] > 0;
                bool thisDiseaseTypeHasNotBeenCured
                    = game.GQ_Is_DiseaseCured_OR_Eradicated(infectionType);

                if (
                    currentPlayerLocationIsInfectedWithThisType
                    &&
                    currentPlayerIsMedic
                    &&
                    thisDiseaseTypeHasNotBeenCured
                    )
                {
                    PA_TreatDisease_Medic treatDisease_Medic_Action = new PA_TreatDisease_Medic(
                            currentPlayer,
                            currentPlayerLocation,
                            infectionType
                            );

                    List<PD_Action> allCommands = new List<PD_Action>();
                    allCommands.Add(treatDisease_Medic_Action);

                    PD_MacroAction treatMacro = new PD_MacroAction(
                        allCommands,
                        PD_MacroAction_Type.TreatDisease_Medic_Macro,
                        PD_MacroAction_WalkType.None,
                        numAvailableActions
                        );

                    treatDiseaseMacros.Add(treatMacro);
                }
                else if (
                    currentPlayerLocationIsInfectedWithThisType
                    &&
                    currentPlayerIsMedic == false
                    )
                {
                    PA_TreatDisease treatDiseaseAction = new PA_TreatDisease(
                            currentPlayer,
                            currentPlayerLocation,
                            infectionType
                            );

                    List<PD_Action> allCommands = new List<PD_Action>();
                    allCommands.Add(treatDiseaseAction);

                    PD_MacroAction treatMacro = new PD_MacroAction(
                        allCommands,
                        PD_MacroAction_Type.TreatDisease_Macro,
                        PD_MacroAction_WalkType.None,
                        numAvailableActions
                        );

                    treatDiseaseMacros.Add(treatMacro);
                }
            }

            return treatDiseaseMacros;
        }

        public static List<PD_MacroAction> Find_TreatDiseaseMacros_SpecificWalkType(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences,
            PD_MacroAction_WalkType walkType
            )
        {
            bool currentPlayerIsMedic = currentPlayerRole == PD_Player_Roles.Medic;

            List<PD_MacroAction> treatDiseaseMacros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences)
            {
                var destination = ((I_Movement_Action)walkSequence.GetLast()).ToCity;

                for (int t = 0; t < 4; t++)
                {
                    bool destinationIsInfectedWithThisType
                        = game.map_elements.infections__per__type__per__city[destination][t] > 0;
                    bool thisDiseaseTypeHasNotBeenCured
                        = game.GQ_Is_DiseaseCured_OR_Eradicated(t) == false;

                    if (
                        destinationIsInfectedWithThisType == true
                        &&
                        currentPlayerIsMedic == true
                        &&
                        thisDiseaseTypeHasNotBeenCured == true
                        )
                    {
                        PA_TreatDisease_Medic treatDisease_Medic_Action = new PA_TreatDisease_Medic(
                            currentPlayer,
                            destination,
                            t
                            );

                        List<PD_Action> allCommands = new List<PD_Action>();

                        allCommands.AddRange(walkSequence);

                        allCommands.Add(treatDisease_Medic_Action);

                        PD_MacroAction treatMacro = new PD_MacroAction(
                            allCommands,
                            PD_MacroAction_Type.TreatDisease_Medic_Macro,
                            walkType,
                            numAvailableActions
                            );

                        treatDiseaseMacros.Add(treatMacro);
                    }
                    else if (
                        destinationIsInfectedWithThisType == true
                        &&
                        currentPlayerIsMedic == false
                        )
                    {
                        PA_TreatDisease treatDiseaseAction = new PA_TreatDisease(
                                currentPlayer,
                                destination,
                                t
                                );

                        List<PD_Action> allCommands = new List<PD_Action>();

                        allCommands.AddRange(walkSequence);

                        allCommands.Add(treatDiseaseAction);

                        PD_MacroAction treatMacro = new PD_MacroAction(
                            allCommands,
                            PD_MacroAction_Type.TreatDisease_Macro,
                            walkType,
                            numAvailableActions
                            );

                        treatDiseaseMacros.Add(treatMacro);
                    }
                }
            }

            return treatDiseaseMacros;
        }
        #endregion

        #region treatDiseaseMacros_ExecutableNow
        public static List<PD_MacroAction> FindAll_AutoTreatDisease_Medic_Macros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_Sequences,
            List<List<PD_Action>> directFlightWalk_Sequences,
            List<List<PD_Action>> charterFlightWalk_Sequences,
            List<List<PD_Action>> operationsExpertFlightWalk_Sequences
            )
        {
            var autoTreatDisease_Medic_Macros = new List<PD_MacroAction>();

            List<PD_MacroAction> autoTreatDisease_Medic_Macros_SimpleWalk =
                Find_AutoTreatDisease_Medic_Macros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    simpleWalk_Sequences,
                    PD_MacroAction_WalkType.SimpleWalk
                    );
            List<PD_MacroAction> autoTreatDisease_Medic_Macros_DirectFlightWalk =
                Find_AutoTreatDisease_Medic_Macros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    directFlightWalk_Sequences,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    );
            List<PD_MacroAction> autoTreatDisease_Medic_Macros_CharterFlightWalk =
                Find_AutoTreatDisease_Medic_Macros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    charterFlightWalk_Sequences,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    );
            List<PD_MacroAction> autoTreatDisease_Medic_Macros_OperationsExpertFlightWalk =
                Find_AutoTreatDisease_Medic_Macros_SpecificWalkType(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    operationsExpertFlightWalk_Sequences,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    );

            autoTreatDisease_Medic_Macros.AddRange(autoTreatDisease_Medic_Macros_SimpleWalk);
            autoTreatDisease_Medic_Macros.AddRange(autoTreatDisease_Medic_Macros_DirectFlightWalk);
            autoTreatDisease_Medic_Macros.AddRange(autoTreatDisease_Medic_Macros_CharterFlightWalk);
            autoTreatDisease_Medic_Macros.AddRange(autoTreatDisease_Medic_Macros_OperationsExpertFlightWalk);

            return autoTreatDisease_Medic_Macros;
        }

        public static List<PD_MacroAction> Find_AutoTreatDisease_Medic_Macros_SpecificWalkType(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences,
            PD_MacroAction_WalkType walkType
            )
        {
            bool currentPlayerIsMedic = currentPlayerRole == PD_Player_Roles.Medic;

            if (currentPlayerIsMedic == false)
            {
                return new List<PD_MacroAction>();
            }

            List<int> curedDiseaseTypes = game.GQ_Cured_or_Eradicated_DiseaseTypes();

            if (curedDiseaseTypes.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_MacroAction> autoTreatDisease_Medic_Macros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences)
            {
                int destination = ((I_Movement_Action)walkSequence.GetLast()).ToCity;
                foreach (var curedDiseaseType in curedDiseaseTypes)
                {
                    int num_InfectionCubes_Of_CuredDiseaseType_On_Destination =
                        game.GQ_Num_InfectionCubes_OfType_OnCity(destination, curedDiseaseType);

                    if (num_InfectionCubes_Of_CuredDiseaseType_On_Destination > 0)
                    {
                        PD_MacroAction autoTreatMacro = new PD_MacroAction(
                            walkSequence,
                            PD_MacroAction_Type.AutoTreatDisease_Medic_Macro,
                            walkType,
                            numAvailableActions
                            );
                        autoTreatDisease_Medic_Macros.Add(autoTreatMacro);
                    }
                }
            }

            return autoTreatDisease_Medic_Macros;
        }
        #endregion

        #region buildResearchStationMacros_ExecutableNow
        public static List<PD_MacroAction> FindAll_BuildResearchStationMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_Sequences,
            List<List<PD_Action>> directFlightWalk_Sequences,
            List<List<PD_Action>> charterFlightWalk_Sequences,
            List<List<PD_Action>> operationsExpertFlightWalk_Sequences
            )
        {
            var buildResearchStationMacros = new List<PD_MacroAction>();

            List<PD_MacroAction> buildResearchStationMacros_Immediate =
                Find_BuildResearchStationMacros_Immediate(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions
                    );
            List<PD_MacroAction> buildResearchStationMacros_SimpleWalk =
                Find_BuildResearchStationMacros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    simpleWalk_Sequences,
                    PD_MacroAction_WalkType.SimpleWalk
                    );
            List<PD_MacroAction> buildResearchStationMacros_DirectFlightWalk =
                Find_BuildResearchStationMacros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    directFlightWalk_Sequences,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    );
            List<PD_MacroAction> buildResearchStationMacros_CharterFlightWalk =
                Find_BuildResearchStationMacros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    charterFlightWalk_Sequences,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    );
            List<PD_MacroAction> buildResearchStationMacros_OperationsExpertFlightWalk =
                Find_BuildResearchStationMacros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    operationsExpertFlightWalk_Sequences,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    );

            buildResearchStationMacros.AddRange(buildResearchStationMacros_Immediate);
            buildResearchStationMacros.AddRange(buildResearchStationMacros_SimpleWalk);
            buildResearchStationMacros.AddRange(buildResearchStationMacros_DirectFlightWalk);
            buildResearchStationMacros.AddRange(buildResearchStationMacros_CharterFlightWalk);
            buildResearchStationMacros.AddRange(buildResearchStationMacros_OperationsExpertFlightWalk);

            return buildResearchStationMacros;
        }

        public static List<PD_MacroAction> Find_BuildResearchStationMacros_Immediate(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions
            )
        {
            int numInactiveResearchStations = game.map_elements.available_research_stations;

            if (numInactiveResearchStations == 0)
            {
                return new List<PD_MacroAction>();
            }

            bool currentLocationIsResearchStation
                = game.map_elements.research_stations__per__city[currentPlayerLocation] == true;

            if (currentLocationIsResearchStation)
            {
                return new List<PD_MacroAction>();
            }

            bool currentPlayerIsOperationsExpert = currentPlayerRole == PD_Player_Roles.Operations_Expert;

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            List<PD_MacroAction> buildResearchStationMacros = new List<PD_MacroAction>();

            if (currentPlayerIsOperationsExpert)
            {
                PA_BuildResearchStation_OperationsExpert buildResearchStationAction = new PA_BuildResearchStation_OperationsExpert(
                    currentPlayer,
                    currentPlayerLocation
                    );

                List<PD_Action> actionsList = new List<PD_Action>();

                actionsList.Add(buildResearchStationAction);

                PD_MacroAction macroAction = new PD_MacroAction(
                    actionsList,
                    PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro,
                    PD_MacroAction_WalkType.None,
                    numAvailableActions
                    );

                buildResearchStationMacros.Add(macroAction);
            }
            else
            {
                foreach (var cityCardToBuildResearchStation in cityCardsInCurrentPlayerHand)
                {
                    if (currentPlayerLocation == cityCardToBuildResearchStation)
                    {
                        PA_BuildResearchStation buildResearchStationAction = new PA_BuildResearchStation(
                            currentPlayer,
                            cityCardToBuildResearchStation,
                            currentPlayerLocation
                            );
                        List<PD_Action> totalListOfActions = new List<PD_Action>();

                        totalListOfActions.Add(buildResearchStationAction);

                        PD_MacroAction macroAction = new PD_MacroAction(
                            totalListOfActions,
                            PD_MacroAction_Type.BuildResearchStation_Macro,
                            PD_MacroAction_WalkType.None,
                            numAvailableActions
                            );

                        buildResearchStationMacros.Add(macroAction);
                    }
                }
            }

            return buildResearchStationMacros;
        }

        public static List<PD_MacroAction> Find_BuildResearchStationMacros_SpecificWalkType(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            int numInactiveResearchStations = game.map_elements.available_research_stations;

            if (numInactiveResearchStations == 0)
            {
                return new List<PD_MacroAction>();
            }

            bool currentPlayerIsOperationsExpert = currentPlayerRole == PD_Player_Roles.Operations_Expert;

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            List<PD_MacroAction> buildResearchStationMacros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences_NonMaximumLength)
            {
                var destination = ((I_Movement_Action)walkSequence.GetLast()).ToCity;

                bool destinationIsResearchStation
                    = game.map_elements.research_stations__per__city[destination] == true;

                if (destinationIsResearchStation)
                {
                    continue;
                }

                if (currentPlayerIsOperationsExpert)
                {
                    PA_BuildResearchStation_OperationsExpert buildResearchStationAction
                        = new PA_BuildResearchStation_OperationsExpert(
                            currentPlayer,
                            destination
                            );

                    List<PD_Action> totalListOfActions = new List<PD_Action>();
                    totalListOfActions.AddRange(walkSequence);
                    totalListOfActions.Add(buildResearchStationAction);

                    PD_MacroAction macroAction = new PD_MacroAction(
                        totalListOfActions,
                        PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro,
                        walkType,
                        numAvailableActions
                        );

                    buildResearchStationMacros.Add(macroAction);
                }
                else
                {
                    var cityCardsUsedForWalking = PD_Macro_Utilities.Find_CityCardsUsedForWalking_In_ActionList(walkSequence);
                    var remainingCityCardsInPlayerHand = cityCardsInCurrentPlayerHand.FindAll(
                        x =>
                        cityCardsUsedForWalking.Contains(x) == false
                        );

                    foreach (var cityCardToBuildResearchStation in remainingCityCardsInPlayerHand)
                    {
                        if (destination == cityCardToBuildResearchStation)
                        {
                            PA_BuildResearchStation buildResearchStationAction = new PA_BuildResearchStation(
                                currentPlayer,
                                cityCardToBuildResearchStation,
                                destination
                                );
                            List<PD_Action> totalListOfActions = new List<PD_Action>();
                            totalListOfActions.AddRange(walkSequence);
                            totalListOfActions.Add(buildResearchStationAction);

                            PD_MacroAction macroAction = new PD_MacroAction(
                                totalListOfActions,
                                PD_MacroAction_Type.BuildResearchStation_Macro,
                                walkType,
                                numAvailableActions
                                );

                            buildResearchStationMacros.Add(macroAction);
                        }
                    }
                }
            }

            return buildResearchStationMacros;
        }
        #endregion

        #region moveResearchStationMacros_ExecutableNow
        public static List<PD_MacroAction> FindAll_MoveResearchStationMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength
            )
        {
            var moveResearchStationMacros = new List<PD_MacroAction>();

            List<PD_MacroAction> moveResearchStationMacros_Immediate_ExecutableNow =
                Find_MoveResearchStationMacros_Immediate_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions
                    );
            List<PD_MacroAction> buildResearchStationMacros_SimpleWalk_ExecutableNow =
                Find_MoveResearchStationMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.SimpleWalk
                    );
            List<PD_MacroAction> buildResearchStationMacros_DirectFlightWalk_ExecutableNow =
                Find_MoveResearchStationMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    );
            List<PD_MacroAction> buildResearchStationMacros_CharterFlightWalk_ExecutableNow =
                Find_MoveResearchStationMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    );
            List<PD_MacroAction> buildResearchStationMacros_OperationsExpertFlightWalk_ExecutableNow =
                Find_MoveResearchStationMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    );

            moveResearchStationMacros.AddRange(moveResearchStationMacros_Immediate_ExecutableNow);
            moveResearchStationMacros.AddRange(buildResearchStationMacros_SimpleWalk_ExecutableNow);
            moveResearchStationMacros.AddRange(buildResearchStationMacros_DirectFlightWalk_ExecutableNow);
            moveResearchStationMacros.AddRange(buildResearchStationMacros_CharterFlightWalk_ExecutableNow);
            moveResearchStationMacros.AddRange(buildResearchStationMacros_OperationsExpertFlightWalk_ExecutableNow);

            return moveResearchStationMacros;
        }

        public static List<PD_MacroAction> Find_MoveResearchStationMacros_Immediate_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions
            )
        {
            int numInactiveResearchStations = game.map_elements.available_research_stations;

            if (numInactiveResearchStations > 0)
            {
                return new List<PD_MacroAction>();
            }

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            List<PD_MacroAction> moveResearchStationMacros = new List<PD_MacroAction>();

            bool destinationIsResearchStation
                = game.map_elements.research_stations__per__city[currentPlayerLocation] == true;

            if (destinationIsResearchStation)
            {
                return new List<PD_MacroAction>();
            }

            foreach (var cityCardToMoveResearchStation in cityCardsInCurrentPlayerHand)
            {
                if (currentPlayerLocation == cityCardToMoveResearchStation)
                {
                    List<int> existingResearchStationCities = game.GQ_ResearchStationCities();

                    foreach (var existing_rs_city in existingResearchStationCities)
                    {
                        PA_MoveResearchStation moveResearchStationAction = new PA_MoveResearchStation(
                            currentPlayer,
                            cityCardToMoveResearchStation,
                            existing_rs_city,
                            currentPlayerLocation
                            );

                        List<PD_Action> totalListOfActions = new List<PD_Action>();

                        totalListOfActions.Add(moveResearchStationAction);

                        PD_MacroAction macroAction = new PD_MacroAction(
                            totalListOfActions,
                            PD_MacroAction_Type.BuildResearchStation_Macro,
                            PD_MacroAction_WalkType.None,
                            numAvailableActions
                            );

                        moveResearchStationMacros.Add(macroAction);
                    }
                }
            }

            return moveResearchStationMacros;
        }

        public static List<PD_MacroAction> Find_MoveResearchStationMacros_SpecificWalkType_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            int numInactiveResearchStations = game.map_elements.available_research_stations;

            if (numInactiveResearchStations > 0)
            {
                return new List<PD_MacroAction>();
            }

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            List<PD_MacroAction> moveResearchStationMacros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences_NonMaximumLength)
            {
                var destination = ((I_Movement_Action)walkSequence.GetLast()).ToCity;

                bool destinationIsResearchStation = game.GQ_Is_City_ResearchStation(destination);

                if (destinationIsResearchStation)
                {
                    continue;
                }

                List<int> cityCardsUsedForWalking = new List<int>();
                foreach (var action in walkSequence)
                {
                    if (action.GetType() == typeof(PA_DirectFlight))
                    {
                        cityCardsUsedForWalking.Add(
                            ((PA_DirectFlight)action).UsedCard
                            );
                    }
                    else if (action.GetType() == typeof(PA_CharterFlight))
                    {
                        cityCardsUsedForWalking.Add(
                            ((PA_CharterFlight)action).UsedCard
                            );
                    }
                    else if (action.GetType() == typeof(PA_OperationsExpert_Flight))
                    {
                        cityCardsUsedForWalking.Add(
                            ((PA_OperationsExpert_Flight)action).UsedCard
                            );
                    }
                }

                List<int> cityCardsRemainingForMovingResearchStation = cityCardsInCurrentPlayerHand.FindAll(
                    x =>
                    cityCardsUsedForWalking.Contains(x) == false
                    );

                foreach (var cityCardToMoveResearchStation in cityCardsRemainingForMovingResearchStation)
                {
                    if (destination == cityCardToMoveResearchStation)
                    {
                        List<int> existingResearchStationCities = game.GQ_ResearchStationCities();

                        foreach (var existing_rs_city in existingResearchStationCities)
                        {
                            PA_MoveResearchStation moveResearchStationAction = new PA_MoveResearchStation(
                                currentPlayer,
                                cityCardToMoveResearchStation,
                                existing_rs_city,
                                destination
                                );

                            List<PD_Action> totalListOfActions = new List<PD_Action>();
                            totalListOfActions.AddRange(walkSequence);
                            totalListOfActions.Add(moveResearchStationAction);

                            PD_MacroAction macroAction = new PD_MacroAction(
                                totalListOfActions,
                                PD_MacroAction_Type.BuildResearchStation_Macro,
                                walkType,
                                numAvailableActions
                                );

                            moveResearchStationMacros.Add(macroAction);
                        }
                    }
                }
            }

            return moveResearchStationMacros;
        }
        #endregion

        #region shareKnowledge_Give_Macros
        public static List<PD_MacroAction> FindAll_ShareKnowledge_Give_Macros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_Sequences,
            List<List<PD_Action>> directFlightWalk_Sequences,
            List<List<PD_Action>> charterFlightWalk_Sequences,
            List<List<PD_Action>> operationsExpertFlightWalk_Sequences
            )
        {
            var shareKnowledge_Give_Macros = new List<PD_MacroAction>();

            List<PD_MacroAction> shareKnowledge_Give_Macros_Immediate =
                Find_ShareKnowledge_Give_Macros_Immediate(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions
                    );
            List<PD_MacroAction> shareKnowledge_Give_Macros_SimpleWalk =
                Find_ShareKnowledge_Give_Macros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    simpleWalk_Sequences,
                    PD_MacroAction_WalkType.SimpleWalk
                    );
            List<PD_MacroAction> shareKnowledge_Give_Macros_DirectFlightWalk =
                Find_ShareKnowledge_Give_Macros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    directFlightWalk_Sequences,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    );
            List<PD_MacroAction> shareKnowledge_Give_Macros_CharterFlightWalk =
                Find_ShareKnowledge_Give_Macros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    charterFlightWalk_Sequences,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    );
            List<PD_MacroAction> shareKnowledge_Give_Macros_OperationsExpertFlightWalk =
                Find_ShareKnowledge_Give_Macros_SpecificWalkType(
                    game,
                    pathFinder,

                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,

                    operationsExpertFlightWalk_Sequences,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    );

            shareKnowledge_Give_Macros.AddRange(shareKnowledge_Give_Macros_Immediate);
            shareKnowledge_Give_Macros.AddRange(shareKnowledge_Give_Macros_SimpleWalk);
            shareKnowledge_Give_Macros.AddRange(shareKnowledge_Give_Macros_DirectFlightWalk);
            shareKnowledge_Give_Macros.AddRange(shareKnowledge_Give_Macros_CharterFlightWalk);
            shareKnowledge_Give_Macros.AddRange(shareKnowledge_Give_Macros_OperationsExpertFlightWalk);

            return shareKnowledge_Give_Macros;
        }

        public static List<PD_MacroAction> Find_ShareKnowledge_Give_Macros_Immediate(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions
            )
        {
            bool currentPlayerIsResearcher = currentPlayerRole == PD_Player_Roles.Researcher;

            List<PD_MacroAction> shareKnowledge_Give_Macros = new List<PD_MacroAction>();

            List<int> otherPlayers = game.players.FindAll(
                x =>
                x != currentPlayer
                );

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            if (currentPlayerIsResearcher)
            {
                foreach (var otherPlayer in otherPlayers)
                {
                    var otherPlayerLocation = game.GQ_PlayerLocation(otherPlayer);

                    if (otherPlayerLocation == currentPlayerLocation)
                    {
                        foreach (var cityCardToGive in cityCardsInCurrentPlayerHand)
                        {
                            PA_ShareKnowledge_GiveCard_ResearcherGives shareKnowledgeAction = new PA_ShareKnowledge_GiveCard_ResearcherGives(
                                currentPlayer,
                                otherPlayer,
                                cityCardToGive
                                );

                            List<PD_Action> actionsList = new List<PD_Action>();

                            actionsList.Add(shareKnowledgeAction);

                            PD_MacroAction shareKnowledgeMacro = new PD_MacroAction(
                                actionsList,
                                PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro,
                                PD_MacroAction_WalkType.None,
                                numAvailableActions
                                );

                            shareKnowledge_Give_Macros.Add(shareKnowledgeMacro);
                        }
                    }
                }
            }
            else
            {
                foreach (var cityCardToGive in cityCardsInCurrentPlayerHand)
                {
                    if (cityCardToGive == currentPlayerLocation)
                    {
                        foreach (var otherPlayer in otherPlayers)
                        {
                            var otherPlayerLocation = game.GQ_PlayerLocation(otherPlayer);

                            if (otherPlayerLocation == currentPlayerLocation)
                            {
                                PA_ShareKnowledge_GiveCard shareKnowledgeAction = new PA_ShareKnowledge_GiveCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToGive
                                );

                                List<PD_Action> actionsList = new List<PD_Action>();

                                actionsList.Add(shareKnowledgeAction);

                                PD_MacroAction shareKnowledgeMacro = new PD_MacroAction(
                                    actionsList,
                                    PD_MacroAction_Type.ShareKnowledge_Give_Macro,
                                    PD_MacroAction_WalkType.None,
                                    numAvailableActions
                                    );

                                shareKnowledge_Give_Macros.Add(shareKnowledgeMacro);
                            }

                            else
                            {
                                PA_ShareKnowledge_GiveCard supposedShareKnowledgeAction = new PA_ShareKnowledge_GiveCard(
                                    currentPlayer,
                                    otherPlayer,
                                    cityCardToGive
                                    );

                                List<PD_Action> actionsList = new List<PD_Action>();

                                for (int i = 0; i < numAvailableActions; i++)
                                {
                                    PA_Stay stayCommand = new PA_Stay(
                                        currentPlayer,
                                        currentPlayerLocation
                                        );
                                    actionsList.Add(stayCommand);
                                }

                                actionsList.Add(supposedShareKnowledgeAction);

                                PD_MacroAction shareKnowledgeMacro = new PD_MacroAction(
                                    actionsList,
                                    PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro,
                                    PD_MacroAction_WalkType.None,
                                    numAvailableActions
                                    );

                                shareKnowledge_Give_Macros.Add(shareKnowledgeMacro);
                            }
                        }
                    }
                }
            }

            return shareKnowledge_Give_Macros;
        }

        public static List<PD_MacroAction> Find_ShareKnowledge_Give_Macros_SpecificWalkType(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences,
            PD_MacroAction_WalkType walkType
            )
        {

            List<PD_MacroAction> shareKnowledge_Give_Macros = new List<PD_MacroAction>();

            List<int> otherPlayers = game.players.FindAll(
                x =>
                x != currentPlayer
                );

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            foreach (var walkSequence in walkSequences)
            {
                var destination = ((I_Movement_Action)walkSequence.GetLast()).ToCity;

                var cityCardsUsedForWalking = PD_Macro_Utilities.Find_CityCardsUsedForWalking_In_ActionList(walkSequence);

                var remainingCityCardsInPlayerHand = cityCardsInCurrentPlayerHand.FindAll(
                    x =>
                    cityCardsUsedForWalking.Contains(x) == false
                    );

                foreach (var cityCardToGive in remainingCityCardsInPlayerHand)
                {
                    foreach (var otherPlayer in otherPlayers)
                    {
                        var otherPlayerLocation = game.GQ_PlayerLocation(otherPlayer);

                        bool currentPlayerIsResearcher = currentPlayerRole == PD_Player_Roles.Researcher;
                        bool card_Matches_Destination = cityCardToGive == destination;
                        bool otherPlayer_IsAt_Destination = otherPlayerLocation == destination;

                        if (
                            currentPlayerIsResearcher == true
                            &&
                            otherPlayer_IsAt_Destination == true
                            )
                        {
                            PA_ShareKnowledge_GiveCard_ResearcherGives shareKnowledgeAction =
                                new PA_ShareKnowledge_GiveCard_ResearcherGives(
                                    currentPlayer,
                                    otherPlayer,
                                    cityCardToGive
                                    );

                            List<PD_Action> actionsList = new List<PD_Action>();
                            actionsList.AddRange(walkSequence);
                            actionsList.Add(shareKnowledgeAction);

                            PD_MacroAction shareKnowledgeMacro = new PD_MacroAction(
                                actionsList,
                                PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro,
                                walkType,
                                numAvailableActions
                                );

                            shareKnowledge_Give_Macros.Add(shareKnowledgeMacro);
                        }
                        else if (
                            currentPlayerIsResearcher == false
                            &&
                            card_Matches_Destination == true
                            &&
                            otherPlayer_IsAt_Destination == true
                            )
                        {
                            PA_ShareKnowledge_GiveCard shareKnowledgeAction = new PA_ShareKnowledge_GiveCard(
                                    currentPlayer,
                                    otherPlayer,
                                    cityCardToGive
                                    );

                            List<PD_Action> actionsList = new List<PD_Action>();
                            actionsList.AddRange(walkSequence);
                            actionsList.Add(shareKnowledgeAction);

                            PD_MacroAction shareKnowledgeMacro = new PD_MacroAction(
                                actionsList,
                                PD_MacroAction_Type.ShareKnowledge_Give_Macro,
                                walkType,
                                numAvailableActions
                                );

                            shareKnowledge_Give_Macros.Add(shareKnowledgeMacro);
                        }
                        else if (
                            currentPlayerIsResearcher == false
                            &&
                            card_Matches_Destination == true
                            &&
                            otherPlayer_IsAt_Destination == false
                            )
                        {
                            PA_ShareKnowledge_GiveCard shareKnowledgeAction = new PA_ShareKnowledge_GiveCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToGive
                                );

                            List<PD_Action> actionsList = new List<PD_Action>();
                            actionsList.AddRange(walkSequence);

                            int walkLength = walkSequence.Count;
                            int numRemainingActions = numAvailableActions - walkLength;
                            if (numRemainingActions > 0)
                            {
                                for (int i = 0; i < numRemainingActions; i++)
                                {
                                    PA_Stay stayAction = new PA_Stay(
                                        currentPlayer,
                                        destination
                                        );
                                    actionsList.Add(stayAction);
                                }
                                actionsList.Add(shareKnowledgeAction);
                                PD_MacroAction shareKnowledgeMacro = new PD_MacroAction(
                                    actionsList,
                                    PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro,
                                    walkType,
                                    numAvailableActions
                                    );

                                shareKnowledge_Give_Macros.Add(shareKnowledgeMacro);
                            }
                            else
                            {
                                actionsList.Add(shareKnowledgeAction);
                                PD_MacroAction shareKnowledgeMacro = new PD_MacroAction(
                                    actionsList,
                                    PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro,
                                    walkType,
                                    numAvailableActions
                                    );

                                shareKnowledge_Give_Macros.Add(shareKnowledgeMacro);
                            }
                        }
                    }
                }
            }

            return shareKnowledge_Give_Macros;
        }
        #endregion

        #region shareKnowledge_Take_Macros
        public static List<PD_MacroAction> FindAll_ShareKnowledge_Take_Macros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength
            )
        {
            var shareKnowledge_Take_Macros = new List<PD_MacroAction>();

            List<PD_MacroAction> shareKnowledge_Take_Macros_Immediate_ExecutableNow =
                Find_ShareKnowledge_Take_Macros_Immediate_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions
                    );
            List<PD_MacroAction> shareKnowledge_Take_Macros_SimpleWalk_ExecutableNow =
                Find_ShareKnowledge_Take_Macros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.SimpleWalk
                    );
            List<PD_MacroAction> shareKnowledge_Take_Macros_DirectFlightWalk_ExecutableNow =
                Find_ShareKnowledge_Take_Macros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    );
            List<PD_MacroAction> shareKnowledge_Take_Macros_CharterFlightWalk_ExecutableNow =
                Find_ShareKnowledge_Take_Macros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    );
            List<PD_MacroAction> shareKnowledge_Take_Macros_OperationsExpertFlightWalk_ExecutableNow =
                Find_ShareKnowledge_Take_Macros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    );

            shareKnowledge_Take_Macros.AddRange(shareKnowledge_Take_Macros_Immediate_ExecutableNow);
            shareKnowledge_Take_Macros.AddRange(shareKnowledge_Take_Macros_SimpleWalk_ExecutableNow);
            shareKnowledge_Take_Macros.AddRange(shareKnowledge_Take_Macros_DirectFlightWalk_ExecutableNow);
            shareKnowledge_Take_Macros.AddRange(shareKnowledge_Take_Macros_CharterFlightWalk_ExecutableNow);
            shareKnowledge_Take_Macros.AddRange(shareKnowledge_Take_Macros_OperationsExpertFlightWalk_ExecutableNow);

            return shareKnowledge_Take_Macros;
        }

        public static List<PD_MacroAction> Find_ShareKnowledge_Take_Macros_Immediate_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions
            )
        {
            List<int> otherPlayers = game.players.FindAll(
                x =>
                x != currentPlayer
                );

            //List<PD_Player> otherPlayersAtCurrentLocation = otherPlayers.FindAll(
            //    x =>
            //    PD_Game_Queries.GQ_Find_PlayerLocation(game, x) == currentPlayerLocation
            //    );

            List<PD_MacroAction> shareKnowledge_Take_Macros = new List<PD_MacroAction>();

            foreach (var otherPlayer in otherPlayers)
            {

                var otherPlayerRole = game.GQ_Player_Role(otherPlayer);
                int otherPlayerLocation = game.GQ_PlayerLocation(otherPlayer);
                List<int> cityCardsInOtherPlayerHand = game.GQ_CityCardsInPlayerHand(otherPlayer);

                foreach (var cityCardToTake in cityCardsInOtherPlayerHand)
                {

                    bool otherPlayerIsResearcher = otherPlayerRole == PD_Player_Roles.Researcher;
                    bool cityCardToTake_Matches_CurrentLocation = cityCardToTake == currentPlayerLocation;
                    bool otherPlayer_IsAt_CurrentLocation = otherPlayerLocation == currentPlayerLocation;

                    if (
                        otherPlayerIsResearcher == true
                        &&
                        otherPlayer_IsAt_CurrentLocation == true
                        )
                    {
                        PA_ShareKnowledge_TakeCard_FromResearcher shareKnowledge_Take_FromResearcher_Action =
                            new PA_ShareKnowledge_TakeCard_FromResearcher(
                                currentPlayer,
                                otherPlayer,
                                cityCardToTake
                                );

                        List<PD_Action> actionsList = new List<PD_Action>();

                        actionsList.Add(shareKnowledge_Take_FromResearcher_Action);

                        PD_MacroAction shareKnowledge_Take_FromResearcher_Macro = new PD_MacroAction(
                            actionsList,
                            PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro,
                            PD_MacroAction_WalkType.None,
                            numAvailableActions
                            );

                        shareKnowledge_Take_Macros.Add(shareKnowledge_Take_FromResearcher_Macro);
                    }
                    else if (
                        otherPlayerIsResearcher == false
                        &&
                        cityCardToTake_Matches_CurrentLocation == true
                        &&
                        otherPlayer_IsAt_CurrentLocation == true
                        )
                    {
                        PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PA_ShareKnowledge_TakeCard(
                            currentPlayer,
                            otherPlayer,
                            cityCardToTake
                            );

                        List<PD_Action> actionsList = new List<PD_Action>();

                        actionsList.Add(shareKnowledge_Take_Action);

                        PD_MacroAction shareKnowledge_Take_Macro = new PD_MacroAction(
                            actionsList,
                            PD_MacroAction_Type.ShareKnowledge_Take_Macro,
                            PD_MacroAction_WalkType.None,
                            numAvailableActions
                            );

                        shareKnowledge_Take_Macros.Add(shareKnowledge_Take_Macro);
                    }
                    else if (
                        otherPlayerIsResearcher == false
                        &&
                        cityCardToTake_Matches_CurrentLocation == true
                        &&
                        otherPlayer_IsAt_CurrentLocation == false
                        )
                    {
                        PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PA_ShareKnowledge_TakeCard(
                            currentPlayer,
                            otherPlayer,
                            cityCardToTake
                            );

                        List<PD_Action> actionsList = new List<PD_Action>();

                        for (int i = 0; i < numAvailableActions; i++)
                        {
                            PA_Stay stayAction = new PA_Stay(
                                currentPlayer,
                                currentPlayerLocation
                                );
                            actionsList.Add(stayAction);
                        }

                        actionsList.Add(shareKnowledge_Take_Action);

                        PD_MacroAction shareKnowledge_Take_Macro = new PD_MacroAction(
                            actionsList,
                            PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro,
                            PD_MacroAction_WalkType.None,
                            numAvailableActions
                            );

                        shareKnowledge_Take_Macros.Add(shareKnowledge_Take_Macro);
                    }
                }

            }

            return shareKnowledge_Take_Macros;
        }

        public static List<PD_MacroAction> Find_ShareKnowledge_Take_Macros_SpecificWalkType_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            List<int> otherPlayers = game.players.FindAll(
                x =>
                x != currentPlayer
                );

            List<PD_MacroAction> shareKnowledge_Take_Macros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences_NonMaximumLength)
            {
                var destination = ((I_Movement_Action)walkSequence.GetLast()).ToCity;

                List<int> otherPlayersAtDestination = otherPlayers.FindAll(
                    x =>
                    game.GQ_PlayerLocation(x) == destination
                    );

                int walkLength = walkSequence.Count;
                int numRemainingActions_AfterWalking = numAvailableActions - walkLength;

                foreach (var otherPlayer in otherPlayersAtDestination)
                {
                    var otherPlayerRole = game.GQ_Player_Role(otherPlayer);
                    int otherPlayerLocation = game.GQ_PlayerLocation(otherPlayer);
                    List<int> cityCardsInOtherPlayerHand = game.GQ_CityCardsInPlayerHand(otherPlayer);

                    foreach (var cityCardToTake in cityCardsInOtherPlayerHand)
                    {

                        bool otherPlayer_Is_Researcher = otherPlayerRole == PD_Player_Roles.Researcher;
                        bool cityCard_Matches_Destination = cityCardToTake == destination;
                        bool otherPlayer_IsAt_Destination = otherPlayerLocation == destination;

                        if (
                            otherPlayer_Is_Researcher == true
                            &&
                            otherPlayer_IsAt_Destination == true
                            )
                        {
                            PA_ShareKnowledge_TakeCard_FromResearcher shareKnowledge_Take_FromResearcher_Action =
                                new PA_ShareKnowledge_TakeCard_FromResearcher(
                                    currentPlayer,
                                    otherPlayer,
                                    cityCardToTake
                                    );

                            List<PD_Action> actionsList = new List<PD_Action>();
                            actionsList.AddRange(walkSequence);
                            actionsList.Add(shareKnowledge_Take_FromResearcher_Action);

                            PD_MacroAction shareKnowledge_Take_FromResearcher_Macro = new PD_MacroAction(
                                actionsList,
                                PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro,
                                walkType,
                                numAvailableActions
                                );

                            shareKnowledge_Take_Macros.Add(shareKnowledge_Take_FromResearcher_Macro);
                        }
                        else if (
                            otherPlayer_Is_Researcher == false
                            &&
                            cityCard_Matches_Destination == true
                            &&
                            otherPlayer_IsAt_Destination == true
                            )
                        {
                            PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PA_ShareKnowledge_TakeCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToTake
                                );

                            List<PD_Action> actionsList = new List<PD_Action>();
                            actionsList.AddRange(walkSequence);
                            actionsList.Add(shareKnowledge_Take_Action);

                            PD_MacroAction shareKnowledge_Take_Macro = new PD_MacroAction(
                                actionsList,
                                PD_MacroAction_Type.ShareKnowledge_Take_Macro,
                                walkType,
                                numAvailableActions
                                );

                            shareKnowledge_Take_Macros.Add(shareKnowledge_Take_Macro);
                        }
                        else if (
                            otherPlayer_Is_Researcher == false
                            &&
                            cityCard_Matches_Destination == true
                            &&
                            otherPlayer_IsAt_Destination == false
                            )
                        {
                            PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PA_ShareKnowledge_TakeCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToTake
                                );

                            List<PD_Action> actionsList = new List<PD_Action>();
                            actionsList.AddRange(walkSequence);

                            if (numRemainingActions_AfterWalking > 0)
                            {
                                for (int i = 0; i < numRemainingActions_AfterWalking; i++)
                                {
                                    PA_Stay stayAction = new PA_Stay(
                                        currentPlayer,
                                        currentPlayerLocation
                                        );
                                    actionsList.Add(stayAction);
                                }

                                actionsList.Add(shareKnowledge_Take_Action);

                                PD_MacroAction shareKnowledge_Take_Macro = new PD_MacroAction(
                                    actionsList,
                                    PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro,
                                    walkType,
                                    numAvailableActions
                                    );

                                shareKnowledge_Take_Macros.Add(shareKnowledge_Take_Macro);
                            }
                            else
                            {
                                actionsList.Add(shareKnowledge_Take_Action);

                                PD_MacroAction shareKnowledge_Take_Macro = new PD_MacroAction(
                                    actionsList,
                                    PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro,
                                    walkType,
                                    numAvailableActions
                                    );

                                shareKnowledge_Take_Macros.Add(shareKnowledge_Take_Macro);
                            }
                        }
                    }
                }
            }

            return shareKnowledge_Take_Macros;
        }
        #endregion

        #region discoverCureMacros_ExecutableNow
        public static List<PD_MacroAction> FindAll_DiscoverCureMacros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> directFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> charterFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_Action>> operationsExpertFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength
            )
        {
            var discoverCureMacros = new List<PD_MacroAction>();

            List<PD_MacroAction> discoverCureMacros_Immediate_ExecutableNow =
                Find_DiscoverCureMacros_Immediate_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions
                    );
            List<PD_MacroAction> discoverCureMacros_SimpleWalk_ExecutableNow =
                Find_DiscoverCureMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.SimpleWalk
                    );
            List<PD_MacroAction> discoverCureMacros_DirectFlightWalk_ExecutableNow =
                Find_DiscoverCureMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    directFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.DirectFlightWalk
                    );
            List<PD_MacroAction> discoverCureMacros_CharterFlightWalk_ExecutableNow =
                Find_DiscoverCureMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    charterFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.CharterFlightWalk
                    );
            List<PD_MacroAction> discoverCureMacros_OperationsExpertFlightWalk_ExecutableNow =
                Find_DiscoverCureMacros_SpecificWalkType_ExecutableNow(
                    game,
                    pathFinder,
                    currentPlayer,
                    currentPlayerLocation,
                    currentPlayerRole,
                    numAvailableActions,
                    operationsExpertFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength,
                    PD_MacroAction_WalkType.OperationsExpertFlightWalk
                    );

            discoverCureMacros.AddRange(discoverCureMacros_Immediate_ExecutableNow);
            discoverCureMacros.AddRange(discoverCureMacros_SimpleWalk_ExecutableNow);
            discoverCureMacros.AddRange(discoverCureMacros_DirectFlightWalk_ExecutableNow);
            discoverCureMacros.AddRange(discoverCureMacros_CharterFlightWalk_ExecutableNow);
            discoverCureMacros.AddRange(discoverCureMacros_OperationsExpertFlightWalk_ExecutableNow);

            return discoverCureMacros;
        }

        public static List<PD_MacroAction> Find_DiscoverCureMacros_Immediate_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions
            )
        {
            bool currentPlayerIsScientist = currentPlayerRole == PD_Player_Roles.Scientist;

            List<List<int>> discoverCureCardGroups = game.GQ_Find_UsableDiscoverCureCardGroups(currentPlayer);

            if (discoverCureCardGroups.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            bool currentPlayerLocationIsResearchStation
                = game.GQ_Is_City_ResearchStation(currentPlayerLocation);

            if (currentPlayerLocationIsResearchStation == false)
            {
                return new List<PD_MacroAction>();
            }

            List<int> researchStationCities = game.GQ_ResearchStationCities();

            List<PD_MacroAction> discoverCureMacros = new List<PD_MacroAction>();

            foreach (var cardGroup in discoverCureCardGroups)
            {
                int group_type = game.map.infection_type__per__city[cardGroup[0]];
                if (currentPlayerIsScientist)
                {
                    PA_DiscoverCure_Scientist discoverCure_Scientist_Action = new PA_DiscoverCure_Scientist(
                        currentPlayer,
                        currentPlayerLocation,
                        cardGroup,
                        group_type
                        );

                    PD_MacroAction discoverCure_Scientist_Macro = new PD_MacroAction(
                        new List<PD_Action>() { discoverCure_Scientist_Action },
                        PD_MacroAction_Type.DiscoverCure_Scientist_Macro,
                        PD_MacroAction_WalkType.None,
                        numAvailableActions
                        );

                    discoverCureMacros.Add(discoverCure_Scientist_Macro);
                }
                else
                {
                    PA_DiscoverCure discoverCure_Action = new PA_DiscoverCure(
                        currentPlayer,
                        currentPlayerLocation,
                        cardGroup,
                        group_type
                        );

                    PD_MacroAction discoverCure_Macro = new PD_MacroAction(
                        new List<PD_Action>() { discoverCure_Action },
                        PD_MacroAction_Type.DiscoverCure_Macro,
                        PD_MacroAction_WalkType.None,
                        numAvailableActions
                        );

                    discoverCureMacros.Add(discoverCure_Macro);
                }
            }
            return discoverCureMacros;
        }

        public static List<PD_MacroAction> Find_DiscoverCureMacros_SpecificWalkType_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,

            int currentPlayer,
            int currentPlayerLocation,
            int currentPlayerRole,
            int numAvailableActions,

            List<List<PD_Action>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            bool currentPlayerIsScientist = currentPlayerRole == PD_Player_Roles.Scientist;

            List<List<int>> discoverCureCardGroups = game.GQ_Find_UsableDiscoverCureCardGroups(currentPlayer);

            List<int> researchStationCities = game.GQ_ResearchStationCities();

            List<PD_MacroAction> discoverCureMacros = new List<PD_MacroAction>();

            if (discoverCureCardGroups.Count == 0)
            {
                return new List<PD_MacroAction>();
            }
            else
            {
                foreach (var walkSequence in walkSequences_NonMaximumLength)
                {
                    var destination = ((I_Movement_Action)walkSequence.GetLast()).ToCity;

                    var cityCardsUsedForWalking = PD_Macro_Utilities.Find_CityCardsUsedForWalking_In_ActionList(walkSequence);

                    List<List<int>> remainingDiscoverCureCardGroups = new List<List<int>>();
                    foreach (var dcg in discoverCureCardGroups)
                    {
                        if (
                            dcg.Any(
                                x =>
                                cityCardsUsedForWalking.Contains(x)
                                ) == false
                            )
                        {
                            remainingDiscoverCureCardGroups.Add(dcg);
                        }
                    }

                    if (remainingDiscoverCureCardGroups.Count == 0)
                    {
                        continue;
                    }

                    bool destinationIsResearchStation
                        = game.GQ_Is_City_ResearchStation(destination);
                    if (destinationIsResearchStation == false)
                    {
                        continue;
                    }
                    foreach (var cardGroup in remainingDiscoverCureCardGroups)
                    {
                        int card_group_type = game.map.infection_type__per__city[cardGroup[0]];
                        if (currentPlayerIsScientist)
                        {
                            List<PD_Action> allCommands = new List<PD_Action>();

                            allCommands.AddRange(walkSequence);

                            PA_DiscoverCure_Scientist discoverCure_Scientist_Action = new PA_DiscoverCure_Scientist(
                                currentPlayer,
                                destination,
                                cardGroup,
                                card_group_type
                                );

                            allCommands.Add(discoverCure_Scientist_Action);

                            PD_MacroAction discoverCure_Scientist_Macro = new PD_MacroAction(
                                allCommands,
                                PD_MacroAction_Type.DiscoverCure_Scientist_Macro,
                                walkType,
                                numAvailableActions
                                );

                            discoverCureMacros.Add(discoverCure_Scientist_Macro);
                        }
                        else
                        {
                            List<PD_Action> allCommands = new List<PD_Action>();

                            allCommands.AddRange(walkSequence);

                            PA_DiscoverCure discoverCure_Action = new PA_DiscoverCure(
                                currentPlayer,
                                destination,
                                cardGroup,
                                card_group_type
                                );

                            allCommands.Add(discoverCure_Action);

                            PD_MacroAction discoverCure_Macro = new PD_MacroAction(
                                allCommands,
                                PD_MacroAction_Type.DiscoverCure_Macro,
                                walkType,
                                numAvailableActions
                                );

                            discoverCureMacros.Add(discoverCure_Macro);
                        }
                    }
                }
                return discoverCureMacros;
            }
        }
        #endregion

        public static List<List<PD_Action>> FindAll_SimpleWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<int> research_station_cities
            )
        {
            int current_player_location = game.GQ_CurrentPlayer_Location();

            List<List<PD_Action>> simpleWalk_Sequences =
                new List<List<PD_Action>>();

            foreach (var target_location in game.map.cities)
            {
                if (target_location != current_player_location)
                {
                    List<PD_Action> simpleWalkCommandSequence = Compose_SimpleWalk_CommandSequence(
                        game,
                        pathFinder,
                        research_station_cities,
                        current_player_location,
                        target_location
                        );
                    simpleWalk_Sequences.Add(simpleWalkCommandSequence);
                }
            }

            return simpleWalk_Sequences;
        }



        /// <summary>
        /// All walk sequences of this type begin with a direct flight action and are
        /// followed by a simple walk sequence, up to the target location.
        /// After the initial synthesis, they are compared to the list of simple walks sequences.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="pathFinder"></param>
        /// <param name="researchStationCities"></param>
        /// <param name="currentPlayer"></param>
        /// <param name="currentPlayerLocation"></param>
        /// <param name="simpleWalk_ActionSequences"></param>
        /// <param name="numAvailableActions"></param>
        /// <returns></returns>
        public static List<List<PD_Action>> FindAll_DirectFlightWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<int> researchStationCities,
            int currentPlayer,
            int currentPlayerLocation,
            List<List<PD_Action>> simpleWalk_ActionSequences
            )
        {

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();
            if (cityCardsInCurrentPlayerHand.Count == 0)
            {
                return new List<List<PD_Action>>();
            }

            List<int> directFlight_CityCards = cityCardsInCurrentPlayerHand.FindAll(
                x =>
                x != currentPlayerLocation
                );

            if (directFlight_CityCards.Count == 0)
            {
                return new List<List<PD_Action>>();
            }

            List<List<PD_Action>> directFlightWalk_Sequences =
                new List<List<PD_Action>>();

            foreach (var cityCard in directFlight_CityCards)
            {
                int directFlightCity = cityCard;

                PA_DirectFlight directFlightAction = new PA_DirectFlight(
                    currentPlayer,
                    directFlightCity,
                    cityCard
                    );

                List<PD_Action> singleActionList = new List<PD_Action>();
                singleActionList.Add(directFlightAction);

                directFlightWalk_Sequences.Add(singleActionList);

                //List<int> citiesToWalkTo = game.Map.cities.FindAll(
                //    x =>
                //    x != currentPlayerLocation
                //    && x != directFlightCity
                //    );

                foreach (var finalDestination in game.map.cities)
                {
                    if (
                        finalDestination == currentPlayerLocation
                        ||
                        finalDestination == directFlightCity
                        )
                    {
                        continue;
                    }

                    List<PD_Action> actionSequence = new List<PD_Action>();
                    actionSequence.Add(directFlightAction);

                    List<PD_Action> simpleWalkCommandSequence = Compose_SimpleWalk_CommandSequence(
                        game,
                        pathFinder,
                        researchStationCities,
                        directFlightCity,
                        finalDestination
                        );

                    actionSequence.AddRange(
                        simpleWalkCommandSequence
                        );

                    directFlightWalk_Sequences.Add(actionSequence);
                }
            }

            List<List<PD_Action>> directFlightWalk_Optimal_Sequences = new List<List<PD_Action>>();
            foreach (var seq in directFlightWalk_Sequences)
            {
                int directFlightSequenceLength = seq.Count;
                int targetLocation = ((I_Movement_Action)seq.GetLast()).ToCity;
                List<PD_Action> simpleWalkEquivalent = simpleWalk_ActionSequences.Find(
                    x =>
                    ((I_Movement_Action)x.GetLast()).ToCity == targetLocation
                    );
                if (simpleWalkEquivalent != null)
                {
                    int simpleWalkLength = simpleWalkEquivalent.Count;
                    if (directFlightSequenceLength < simpleWalkLength)
                    {
                        directFlightWalk_Optimal_Sequences.Add(seq);
                    }
                }
                else
                {
                    directFlightWalk_Optimal_Sequences.Add(seq);
                }
            }

            return directFlightWalk_Optimal_Sequences;
        }

        public static List<List<PD_Action>> FindAll_CharterFlightWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<int> researchStationCities,
            int currentPlayer,
            int currentPlayerLocation,
            List<List<PD_Action>> simpleWalk_ActionSequences
            )
        {

            List<int> city_cards_in_player_hand = game.GQ_CityCardsInCurrentPlayerHand();

            List<List<PD_Action>> charterFlightWalk_Sequences =
                new List<List<PD_Action>>();

            foreach (var city_card in city_cards_in_player_hand)
            {
                int charterCity = city_card;

                List<PD_Action> simpleWalkToCharterCity = Compose_SimpleWalk_CommandSequence(
                    game,
                    pathFinder,
                    researchStationCities,
                    currentPlayerLocation,
                    charterCity
                    );

                List<int> possible_Destinations = game.map.cities.FindAll(
                    x =>
                    x != currentPlayerLocation
                    && x != charterCity
                    );

                foreach (var finalDestination in possible_Destinations)
                {
                    PA_CharterFlight charterFlightAction = new PA_CharterFlight(
                        currentPlayer,
                        finalDestination,
                        city_card
                        );

                    List<PD_Action> charterWalkCommandSequence = new List<PD_Action>();
                    charterWalkCommandSequence.AddRange(simpleWalkToCharterCity);
                    charterWalkCommandSequence.Add(charterFlightAction);

                    charterFlightWalk_Sequences.Add(charterWalkCommandSequence);
                }
            }

            List<List<PD_Action>> charterFlightWalk_Optimal_Sequences = new List<List<PD_Action>>();
            foreach (var seq in charterFlightWalk_Sequences)
            {
                int directFlightSequenceLength = seq.Count;
                int targetLocation = ((I_Movement_Action)seq.GetLast()).ToCity;
                List<PD_Action> simpleWalkEquivalent = simpleWalk_ActionSequences.Find(
                    x =>
                    ((I_Movement_Action)x.GetLast()).ToCity == targetLocation
                    );
                if (simpleWalkEquivalent != null)
                {
                    int simpleWalkLength = simpleWalkEquivalent.Count;
                    if (directFlightSequenceLength < simpleWalkLength)
                    {
                        charterFlightWalk_Optimal_Sequences.Add(seq);
                    }
                }
                else
                {
                    charterFlightWalk_Optimal_Sequences.Add(seq);
                }
            }

            return charterFlightWalk_Optimal_Sequences;
        }

        public static List<List<PD_Action>> FindAll_OperationsExpertFlightWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<int> researchStationCities,
            int currentPlayer,
            int currentPlayerLocation,

            List<List<PD_Action>> simpleWalk_ActionSequences
            )
        {
            // if role is not operations expert -> skip
            if (game.GQ_CurrentPlayer_Role() != PD_Player_Roles.Operations_Expert)
            {
                return new List<List<PD_Action>>();
            }

            // if operations expert flight has been used this turn -> skip
            if (game.GQ_OperationsExpertFlight_HasBeenUsedInThisTurn())
            {
                return new List<List<PD_Action>>();
            }

            List<int> cityCardsInCurrentPlayerHand = game.GQ_CityCardsInCurrentPlayerHand();

            List<List<PD_Action>> operations_expert_walk_sequences =
                new List<List<PD_Action>>();

            if (game.GQ_Is_City_ResearchStation(currentPlayerLocation))
            {
                foreach (var cityCardToUse in cityCardsInCurrentPlayerHand)
                {
                    foreach (var finalDestination in game.map.cities)
                    {
                        if (finalDestination != currentPlayerLocation)
                        {
                            PA_OperationsExpert_Flight action = new PA_OperationsExpert_Flight(
                                currentPlayer,
                                finalDestination,
                                cityCardToUse
                                );

                            List<PD_Action> singleActionList = new List<PD_Action>() {
                                action
                            };

                            operations_expert_walk_sequences.Add(singleActionList);
                        }
                    }
                }
            }
            else
            {
                int nearest_RS_City = researchStationCities[0];
                pathFinder.Find_RS_ClosestToCity(
                    game,
                    researchStationCities,
                    currentPlayerLocation
                    );

                List<PD_Action> simpleWalkSequence = Compose_SimpleWalk_CommandSequence(
                    game,
                    pathFinder,
                    researchStationCities,
                    currentPlayerLocation,
                    nearest_RS_City
                    );

                foreach (var cityCardToUse in cityCardsInCurrentPlayerHand)
                {
                    foreach (var finalDestination in game.map.cities)
                    {
                        if (finalDestination == currentPlayerLocation)
                        {
                            continue;
                        }
                        // create the list
                        List<PD_Action> operations_expert_sequence = new List<PD_Action>();
                        // add the walk to the research station
                        operations_expert_sequence.AddRange(simpleWalkSequence);

                        // add the final action...
                        PA_OperationsExpert_Flight finalAction = new PA_OperationsExpert_Flight(
                            currentPlayer,
                            finalDestination,
                            cityCardToUse
                            );
                        operations_expert_sequence.Add(finalAction);

                        // add the complete sequence to the list...
                        operations_expert_walk_sequences.Add(operations_expert_sequence);
                    }
                }
            }

            List<List<PD_Action>> operations_expert_optimal_sequences = new List<List<PD_Action>>();
            foreach (var operations_expert_sequence in operations_expert_walk_sequences)
            {
                I_Movement_Action operations_expert_last_action = (I_Movement_Action)operations_expert_sequence.GetLast();
                int target_location = operations_expert_last_action.ToCity;
                List<PD_Action> simple_walk_equivalent_sequence = simpleWalk_ActionSequences.Find(
                    x =>
                    ((I_Movement_Action)x.GetLast()).ToCity == target_location
                    );
                if (simple_walk_equivalent_sequence != null)
                {
                    if (operations_expert_sequence.Count < simple_walk_equivalent_sequence.Count)
                    {
                        operations_expert_optimal_sequences.Add(operations_expert_sequence);
                    }
                }
                else
                {
                    operations_expert_optimal_sequences.Add(operations_expert_sequence);
                }
            }

            return operations_expert_optimal_sequences;
        }

        public static List<PD_Action> Compose_SimpleWalk_CommandSequence(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<int> researchStationCities,
            int root,
            int destination
            )
        {
            int current_player = game.GQ_CurrentPlayer();

            List<int> walkPath = pathFinder.GetPrecalculatedShortestPath(
                game,
                researchStationCities,
                root,
                destination
                );

            if (walkPath.Count < 2) return new List<PD_Action>();

            List<PD_Action> commands = new List<PD_Action>();

            for (int i = 0; i < walkPath.Count - 1; i++)
            {
                var from_city = walkPath[i];
                var to_city = walkPath[i + 1];
                bool cities_are_neighbors = game.GQ_Cities_Are_Neighbors(from_city, to_city);
                bool cities_are_research_stations =
                    game.GQ_Is_City_ResearchStation(from_city)
                    &&
                    game.GQ_Is_City_ResearchStation(to_city);

                if (cities_are_neighbors)
                {
                    PA_DriveFerry drive_ferry_action = new PA_DriveFerry(
                        current_player,
                        to_city
                        );
                    commands.Add(drive_ferry_action);
                }
                else if (cities_are_research_stations)
                {
                    PA_ShuttleFlight shuttle_flight_action = new PA_ShuttleFlight(
                        current_player,
                        to_city
                        );
                    commands.Add(shuttle_flight_action);
                }
                else
                {
                    throw new System.Exception("could not create the commands for this walk");
                }
            }

            return commands;
        }
    }
}
