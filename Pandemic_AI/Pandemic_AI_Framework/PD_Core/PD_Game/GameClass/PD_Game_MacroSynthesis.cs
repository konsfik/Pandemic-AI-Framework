using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_Game_MacroSynthesis
    {
        public static List<PD_MacroAction> FindAll_Macros(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities
            )
        {
            int numAvailableActions = PD_Game_Queries.GQ_Count_RemainingPlayerActions_ThisRound(game);

            List<PD_MacroAction> allMacros = new List<PD_MacroAction>();

            if (PD_Game_Queries.GQ_IsInState_ApplyingMainPlayerActions(game))
            {
                ////////////////////////////////////////////////////////////////
                /// OTHER REUSABLE DATA
                ////////////////////////////////////////////////////////////////
                PD_Player currentPlayer = PD_Game_Queries.GQ_Find_CurrentPlayer(game);
                PD_City currentPlayerLocation = PD_Game_Queries.GQ_Find_CurrentPlayer_Location(game);
                PD_Player_Roles currentPlayerRole = PD_Game_Queries.GQ_Find_CurrentPlayer_Role(game);
                List<PD_City> allCitiesExceprCurrentLocation = game.Map.Cities.FindAll(
                    x =>
                    x != currentPlayerLocation
                    );
                ////////////////////////////////////////////////////////////////
                /// WALK SEQUENCES
                ////////////////////////////////////////////////////////////////

                // all walk sequences
                List<List<PD_GameAction_Base>> all_SimpleWalk_Sequences =
                    FindAll_SimpleWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities,

                        currentPlayer,
                        currentPlayerLocation
                        );
                List<List<PD_GameAction_Base>> all_DirectFlightWalk_Sequences =
                    FindAll_DirectFlightWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities,

                        currentPlayer,
                        currentPlayerLocation,

                        all_SimpleWalk_Sequences
                        );
                List<List<PD_GameAction_Base>> all_CharterFlightWalk_Sequences =
                    FindAll_CharterFlightWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities,

                        currentPlayer,
                        currentPlayerLocation,

                        all_SimpleWalk_Sequences
                        );
                List<List<PD_GameAction_Base>> all_operationsExpertFlightWalk_Sequences =
                    FindAll_OperationsExpertFlightWalk_Sequences(
                        game,
                        pathFinder,
                        researchStationCities,

                        currentPlayer,
                        currentPlayerLocation,

                        all_SimpleWalk_Sequences
                        );

                // walk sequences of maximum executable length
                List<List<PD_GameAction_Base>> simpleWalk_Sequences_MaxExecutableLength =
                    all_SimpleWalk_Sequences.FindAll(
                        x =>
                        x.Count == numAvailableActions
                        );
                List<List<PD_GameAction_Base>> directFlightWalk_Sequences_MaxExecutableLength =
                    all_DirectFlightWalk_Sequences.FindAll(
                        x =>
                        x.Count == numAvailableActions
                        );
                List<List<PD_GameAction_Base>> charterFlightWalk_Sequences_MaxExecutableLength =
                    all_CharterFlightWalk_Sequences.FindAll(
                        x =>
                        x.Count == numAvailableActions
                        );
                List<List<PD_GameAction_Base>> operationsExpertFlightWalk_Sequences_MaxExecutableLength =
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

                // STAY MACROS
                //List<PD_MacroAction> stay_Macros = 
                //    FindAll_StayMacros_ExecutableNow(
                //        game,
                //        pathFinder,

                //        currentPlayer,
                //        currentPlayerLocation,
                //        currentPlayerRole,
                //        numAvailableActions,

                //        simpleWalk_Sequences_LessThan_MaxExecutableLength,
                //        directFlightWalk_Sequences_LessThan_MaxExecutableLength,
                //        charterFlightWalk_Sequences_LessThan_MaxExecutableLength,
                //        operationsExpertFlightWalk_Sequences_LessThan_MaxExecutableLength
                //        );

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
            else if (PD_Game_Queries.GQ_IsInState_Discard_Any(game))
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
            if (PD_Game_Queries.GQ_IsInState_DiscardAfterDrawing(game))
            {
                var availableCommands = game.CurrentAvailablePlayerActions;
                if (
                    availableCommands.Any(
                        x =>
                        x.GetType() != typeof(PD_PA_Discard_AfterDrawing)
                        )
                    )
                {
                    throw new System.Exception("problem here!");
                }
                foreach (var command in availableCommands)
                {
                    if (command.GetType() == typeof(PD_PA_Discard_AfterDrawing))
                    {
                        var commandSequence = new List<PD_GameAction_Base>();
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

                if (macros.Any(x => x == null))
                {
                    throw new System.Exception("problem here!");
                }

            }
            else if (PD_Game_Queries.GQ_IsInState_DiscardDuringMainPlayerActions(game))
            {
                var availableCommands = game.CurrentAvailablePlayerActions;
                if (
                    availableCommands.Any(
                        x =>
                        x.GetType() != typeof(PD_PA_Discard_DuringMainPlayerActions)
                        )
                    )
                {
                    throw new System.Exception("problem here!");
                }
                foreach (var command in availableCommands)
                {
                    var macroType = command.GetType();
                    if (macroType == typeof(PD_PA_Discard_DuringMainPlayerActions))
                    {
                        var commandSequence = new List<PD_GameAction_Base>();
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

                if (macros.Any(x => x == null))
                {
                    throw new System.Exception("problem here!");
                }

            }

            return macros;
        }

        #region walkMacros_executableNow
        public static List<PD_MacroAction> FindAll_WalkMacros_ExecutableNow(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,
            List<List<PD_GameAction_Base>> simpleWalk_ExecutableNow_ActionSequences_MaximumLength,
            List<List<PD_GameAction_Base>> directFlightWalk_ExecutableNow_ActionSequences_MaximumLength,
            List<List<PD_GameAction_Base>> charterFlightWalk_ExecutableNow_ActionSequences_MaximumLength,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_ExecutableNow_ActionSequences_MaximumLength
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences_MaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            int numRemainingActions = 4 - game.GameStateCounter.CurrentPlayerActionIndex;

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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength
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
                PD_City destination = macro.Find_Destination();
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
                PD_City destination = macro.Find_Destination();
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
                PD_City destination = macro.Find_Destination();
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions
            )
        {
            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();
            for (int i = 0; i < numAvailableActions; i++)
            {
                actionsList.Add(
                    new PD_PA_Stay(
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            List<PD_MacroAction> stayMacros_ExecutableNow = new List<PD_MacroAction>();

            foreach (var seq in walkSequences_NonMaximumLength)
            {
                int numRemainingActions = numAvailableActions - seq.Count;

                PD_Player player = ((PD_MapMovementAction_Base)seq.GetLast()).Player;
                PD_City destination = ((PD_MapMovementAction_Base)seq.GetLast()).TargetLocation;

                List<PD_GameAction_Base> completeSequence = new List<PD_GameAction_Base>();
                completeSequence.AddRange(seq);

                for (int i = 0; i < numRemainingActions; i++)
                {
                    PD_PA_Stay stayAction = new PD_PA_Stay(
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_ActionSequences,
            List<List<PD_GameAction_Base>> directFlightWalk_ActionSequences,
            List<List<PD_GameAction_Base>> charterFlightWalk_ActionSequences,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_ActionSequences
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
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
                bool currentPlayerLocationIsInfectedWithThisType =
                    game.MapElements.InfectionCubesPerCityID[currentPlayerLocation.ID].FindAll(
                        x =>
                        x.Type == infectionType
                        ).Count > 0;
                bool thisDiseaseTypeHasNotBeenCured = PD_Game_Queries.GQ_Is_DiseaseCured_OR_Eradicated(game, infectionType);

                if (
                    currentPlayerLocationIsInfectedWithThisType
                    &&
                    currentPlayerIsMedic
                    &&
                    thisDiseaseTypeHasNotBeenCured
                    )
                {
                    PD_PA_TreatDisease_Medic treatDisease_Medic_Action = new PD_PA_TreatDisease_Medic(
                            currentPlayer,
                            currentPlayerLocation,
                            infectionType
                            );

                    List<PD_GameAction_Base> allCommands = new List<PD_GameAction_Base>();
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
                    PD_PA_TreatDisease treatDiseaseAction = new PD_PA_TreatDisease(
                            currentPlayer,
                            currentPlayerLocation,
                            infectionType
                            );

                    List<PD_GameAction_Base> allCommands = new List<PD_GameAction_Base>();
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences,
            PD_MacroAction_WalkType walkType
            )
        {
            bool currentPlayerIsMedic = currentPlayerRole == PD_Player_Roles.Medic;

            List<PD_MacroAction> treatDiseaseMacros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences)
            {
                var destination = ((PD_MapMovementAction_Base)walkSequence.GetLast()).TargetLocation;

                for (int infectionType = 0; infectionType < 4; infectionType++)
                {
                    bool destinationIsInfectedWithThisType =
                        game.MapElements.InfectionCubesPerCityID[destination.ID].FindAll(
                            x =>
                            x.Type == infectionType
                            ).Count > 0;
                    bool thisDiseaseTypeHasNotBeenCured = PD_Game_Queries.GQ_Is_DiseaseCured_OR_Eradicated(game, infectionType) == false;

                    if (
                        destinationIsInfectedWithThisType == true
                        &&
                        currentPlayerIsMedic == true
                        &&
                        thisDiseaseTypeHasNotBeenCured == true
                        )
                    {
                        PD_PA_TreatDisease_Medic treatDisease_Medic_Action = new PD_PA_TreatDisease_Medic(
                            currentPlayer,
                            destination,
                            infectionType
                            );

                        List<PD_GameAction_Base> allCommands = new List<PD_GameAction_Base>();

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
                        PD_PA_TreatDisease treatDiseaseAction = new PD_PA_TreatDisease(
                                currentPlayer,
                                destination,
                                infectionType
                                );

                        List<PD_GameAction_Base> allCommands = new List<PD_GameAction_Base>();

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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_Sequences,
            List<List<PD_GameAction_Base>> directFlightWalk_Sequences,
            List<List<PD_GameAction_Base>> charterFlightWalk_Sequences,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_Sequences
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences,
            PD_MacroAction_WalkType walkType
            )
        {
            bool currentPlayerIsMedic = currentPlayerRole == PD_Player_Roles.Medic;

            if (currentPlayerIsMedic == false)
            {
                return new List<PD_MacroAction>();
            }

            List<int> curedDiseaseTypes = PD_Game_Queries.GQ_Find_Cured_or_Eradicated_DiseaseTypes(game);

            if (curedDiseaseTypes.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_MacroAction> autoTreatDisease_Medic_Macros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences)
            {
                PD_City destination = ((PD_MapMovementAction_Base)walkSequence.GetLast()).TargetLocation;
                foreach (var curedDiseaseType in curedDiseaseTypes)
                {
                    int num_InfectionCubes_Of_CuredDiseaseType_On_Destination =
                        PD_Game_Queries.GQ_Count_Num_InfectionCubes_OfType_OnCity(game, destination, curedDiseaseType);

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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_Sequences,
            List<List<PD_GameAction_Base>> directFlightWalk_Sequences,
            List<List<PD_GameAction_Base>> charterFlightWalk_Sequences,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_Sequences
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions
            )
        {
            int numInactiveResearchStations = game.MapElements.InactiveResearchStations.Count;

            if (numInactiveResearchStations == 0)
            {
                return new List<PD_MacroAction>();
            }

            bool currentLocationIsResearchStation = game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count > 0;

            if (currentLocationIsResearchStation)
            {
                return new List<PD_MacroAction>();
            }

            bool currentPlayerIsOperationsExpert = currentPlayerRole == PD_Player_Roles.Operations_Expert;

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            List<PD_MacroAction> buildResearchStationMacros = new List<PD_MacroAction>();

            if (currentPlayerIsOperationsExpert)
            {
                PD_PA_BuildResearchStation_OperationsExpert buildResearchStationAction = new PD_PA_BuildResearchStation_OperationsExpert(
                    currentPlayer,
                    currentPlayerLocation
                    );

                List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();

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
                    if (currentPlayerLocation == cityCardToBuildResearchStation.City)
                    {
                        PD_PA_BuildResearchStation buildResearchStationAction = new PD_PA_BuildResearchStation(
                            currentPlayer,
                            cityCardToBuildResearchStation,
                            currentPlayerLocation
                            );
                        List<PD_GameAction_Base> totalListOfActions = new List<PD_GameAction_Base>();

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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            int numInactiveResearchStations = game.MapElements.InactiveResearchStations.Count;

            if (numInactiveResearchStations == 0)
            {
                return new List<PD_MacroAction>();
            }

            bool currentPlayerIsOperationsExpert = currentPlayerRole == PD_Player_Roles.Operations_Expert;

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            List<PD_MacroAction> buildResearchStationMacros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences_NonMaximumLength)
            {
                var destination = ((PD_MapMovementAction_Base)walkSequence.GetLast()).TargetLocation;

                bool destinationIsResearchStation = game.MapElements.ResearchStationsPerCityID[destination.ID].Count > 0;

                if (destinationIsResearchStation)
                {
                    continue;
                }

                if (currentPlayerIsOperationsExpert)
                {
                    PD_PA_BuildResearchStation_OperationsExpert buildResearchStationAction = new PD_PA_BuildResearchStation_OperationsExpert(
                        currentPlayer,
                        destination
                        );

                    List<PD_GameAction_Base> totalListOfActions = new List<PD_GameAction_Base>();
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
                        if (destination == cityCardToBuildResearchStation.City)
                        {
                            PD_PA_BuildResearchStation buildResearchStationAction = new PD_PA_BuildResearchStation(
                                currentPlayer,
                                cityCardToBuildResearchStation,
                                destination
                                );
                            List<PD_GameAction_Base> totalListOfActions = new List<PD_GameAction_Base>();
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions
            )
        {
            int numInactiveResearchStations = game.MapElements.InactiveResearchStations.Count;

            if (numInactiveResearchStations > 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            List<PD_MacroAction> moveResearchStationMacros = new List<PD_MacroAction>();

            bool destinationIsResearchStation = game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count > 0;

            if (destinationIsResearchStation)
            {
                return new List<PD_MacroAction>();
            }

            foreach (var cityCardToMoveResearchStation in cityCardsInCurrentPlayerHand)
            {
                if (currentPlayerLocation == cityCardToMoveResearchStation.City)
                {
                    List<PD_City> existingResearchStationCities = PD_Game_Queries.GQ_Find_ResearchStationCities(game);

                    foreach (var existing_rs_city in existingResearchStationCities)
                    {
                        PD_PA_MoveResearchStation moveResearchStationAction = new PD_PA_MoveResearchStation(
                            currentPlayer,
                            cityCardToMoveResearchStation,
                            existing_rs_city,
                            currentPlayerLocation
                            );

                        List<PD_GameAction_Base> totalListOfActions = new List<PD_GameAction_Base>();

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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            int numInactiveResearchStations = game.MapElements.InactiveResearchStations.Count;

            if (numInactiveResearchStations > 0)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            List<PD_MacroAction> moveResearchStationMacros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences_NonMaximumLength)
            {
                var destination = ((PD_MapMovementAction_Base)walkSequence.GetLast()).TargetLocation;

                bool destinationIsResearchStation = game.MapElements.ResearchStationsPerCityID[destination.ID].Count > 0;

                if (destinationIsResearchStation)
                {
                    continue;
                }

                List<PD_CityCard> cityCardsUsedForWalking = new List<PD_CityCard>();
                foreach (var action in walkSequence)
                {
                    if (action.GetType() == typeof(PD_PMA_DirectFlight))
                    {
                        cityCardsUsedForWalking.Add(
                            ((PD_PMA_DirectFlight)action).CityCardToDiscard
                            );
                    }
                    else if (action.GetType() == typeof(PD_PMA_CharterFlight))
                    {
                        cityCardsUsedForWalking.Add(
                            ((PD_PMA_CharterFlight)action).CityCardToDiscard
                            );
                    }
                    else if (action.GetType() == typeof(PD_PMA_OperationsExpert_Flight))
                    {
                        cityCardsUsedForWalking.Add(
                            ((PD_PMA_OperationsExpert_Flight)action).CityCardToDiscard
                            );
                    }
                }

                List<PD_CityCard> cityCardsRemainingForMovingResearchStation = cityCardsInCurrentPlayerHand.FindAll(
                    x =>
                    cityCardsUsedForWalking.Contains(x) == false
                    );

                foreach (var cityCardToMoveResearchStation in cityCardsRemainingForMovingResearchStation)
                {
                    if (destination == cityCardToMoveResearchStation.City)
                    {
                        List<PD_City> existingResearchStationCities = PD_Game_Queries.GQ_Find_ResearchStationCities(game);

                        foreach (var existing_rs_city in existingResearchStationCities)
                        {
                            PD_PA_MoveResearchStation moveResearchStationAction = new PD_PA_MoveResearchStation(
                                currentPlayer,
                                cityCardToMoveResearchStation,
                                existing_rs_city,
                                destination
                                );

                            List<PD_GameAction_Base> totalListOfActions = new List<PD_GameAction_Base>();
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_Sequences,
            List<List<PD_GameAction_Base>> directFlightWalk_Sequences,
            List<List<PD_GameAction_Base>> charterFlightWalk_Sequences,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_Sequences
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions
            )
        {
            bool currentPlayerIsResearcher = currentPlayerRole == PD_Player_Roles.Researcher;

            List<PD_MacroAction> shareKnowledge_Give_Macros = new List<PD_MacroAction>();

            List<PD_Player> otherPlayers = game.Players.FindAll(
                x =>
                x != currentPlayer
                );

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            if (currentPlayerIsResearcher)
            {
                foreach (var otherPlayer in otherPlayers)
                {
                    var otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);

                    if (otherPlayerLocation == currentPlayerLocation)
                    {
                        foreach (var cityCardToGive in cityCardsInCurrentPlayerHand)
                        {
                            PD_PA_ShareKnowledge_GiveCard_ResearcherGives shareKnowledgeAction = new PD_PA_ShareKnowledge_GiveCard_ResearcherGives(
                                currentPlayer,
                                otherPlayer,
                                cityCardToGive
                                );

                            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();

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
                    if (cityCardToGive.City == currentPlayerLocation)
                    {
                        foreach (var otherPlayer in otherPlayers)
                        {
                            var otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);

                            if (otherPlayerLocation == currentPlayerLocation)
                            {
                                PD_PA_ShareKnowledge_GiveCard shareKnowledgeAction = new PD_PA_ShareKnowledge_GiveCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToGive
                                );

                                List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();

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
                                PD_PA_ShareKnowledge_GiveCard supposedShareKnowledgeAction = new PD_PA_ShareKnowledge_GiveCard(
                                    currentPlayer,
                                    otherPlayer,
                                    cityCardToGive
                                    );

                                List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();

                                for (int i = 0; i < numAvailableActions; i++)
                                {
                                    PD_PA_Stay stayCommand = new PD_PA_Stay(
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences,
            PD_MacroAction_WalkType walkType
            )
        {

            List<PD_MacroAction> shareKnowledge_Give_Macros = new List<PD_MacroAction>();

            List<PD_Player> otherPlayers = game.Players.FindAll(
                x =>
                x != currentPlayer
                );

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            foreach (var walkSequence in walkSequences)
            {
                var destination = ((PD_MapMovementAction_Base)walkSequence.GetLast()).TargetLocation;

                var cityCardsUsedForWalking = PD_Macro_Utilities.Find_CityCardsUsedForWalking_In_ActionList(walkSequence);

                var remainingCityCardsInPlayerHand = cityCardsInCurrentPlayerHand.FindAll(
                    x =>
                    cityCardsUsedForWalking.Contains(x) == false
                    );

                foreach (var cityCardToGive in remainingCityCardsInPlayerHand)
                {
                    foreach (var otherPlayer in otherPlayers)
                    {
                        var otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);

                        bool currentPlayerIsResearcher = currentPlayerRole == PD_Player_Roles.Researcher;
                        bool card_Matches_Destination = cityCardToGive.City == destination;
                        bool otherPlayer_IsAt_Destination = otherPlayerLocation == destination;

                        if (
                            currentPlayerIsResearcher == true
                            &&
                            otherPlayer_IsAt_Destination == true
                            )
                        {
                            PD_PA_ShareKnowledge_GiveCard_ResearcherGives shareKnowledgeAction = new PD_PA_ShareKnowledge_GiveCard_ResearcherGives(
                                currentPlayer,
                                otherPlayer,
                                cityCardToGive
                                );

                            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();
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
                            PD_PA_ShareKnowledge_GiveCard shareKnowledgeAction = new PD_PA_ShareKnowledge_GiveCard(
                                    currentPlayer,
                                    otherPlayer,
                                    cityCardToGive
                                    );

                            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();
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
                            PD_PA_ShareKnowledge_GiveCard shareKnowledgeAction = new PD_PA_ShareKnowledge_GiveCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToGive
                                );

                            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();
                            actionsList.AddRange(walkSequence);

                            int walkLength = walkSequence.Count;
                            int numRemainingActions = numAvailableActions - walkLength;
                            if (numRemainingActions > 0)
                            {
                                for (int i = 0; i < numRemainingActions; i++)
                                {
                                    PD_PA_Stay stayAction = new PD_PA_Stay(
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> directFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> charterFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_ExecutableNow_Optimal_ActionSequences_NonMaximumLength
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions
            )
        {
            List<PD_Player> otherPlayers = game.Players.FindAll(
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

                var otherPlayerRole = PD_Game_Queries.GQ_Find_Player_Role(game, otherPlayer);
                PD_City otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);
                List<PD_CityCard> cityCardsInOtherPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInPlayerHand(game, otherPlayer);

                foreach (var cityCardToTake in cityCardsInOtherPlayerHand)
                {

                    bool otherPlayerIsResearcher = otherPlayerRole == PD_Player_Roles.Researcher;
                    bool cityCardToTake_Matches_CurrentLocation = cityCardToTake.City == currentPlayerLocation;
                    bool otherPlayer_IsAt_CurrentLocation = otherPlayerLocation == currentPlayerLocation;

                    if (
                        otherPlayerIsResearcher == true
                        &&
                        otherPlayer_IsAt_CurrentLocation == true
                        )
                    {
                        PD_PA_ShareKnowledge_TakeCard_FromResearcher shareKnowledge_Take_FromResearcher_Action =
                            new PD_PA_ShareKnowledge_TakeCard_FromResearcher(
                                currentPlayer,
                                otherPlayer,
                                cityCardToTake
                                );

                        List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();

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
                        PD_PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PD_PA_ShareKnowledge_TakeCard(
                            currentPlayer,
                            otherPlayer,
                            cityCardToTake
                            );

                        List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();

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
                        PD_PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PD_PA_ShareKnowledge_TakeCard(
                            currentPlayer,
                            otherPlayer,
                            cityCardToTake
                            );

                        List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();

                        for (int i = 0; i < numAvailableActions; i++)
                        {
                            PD_PA_Stay stayAction = new PD_PA_Stay(
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            List<PD_Player> otherPlayers = game.Players.FindAll(
                x =>
                x != currentPlayer
                );

            List<PD_MacroAction> shareKnowledge_Take_Macros = new List<PD_MacroAction>();

            foreach (var walkSequence in walkSequences_NonMaximumLength)
            {
                var destination = ((PD_MapMovementAction_Base)walkSequence.GetLast()).TargetLocation;

                List<PD_Player> otherPlayersAtDestination = otherPlayers.FindAll(
                    x =>
                    PD_Game_Queries.GQ_Find_PlayerLocation(game, x) == destination
                    );

                int walkLength = walkSequence.Count;
                int numRemainingActions_AfterWalking = numAvailableActions - walkLength;

                foreach (var otherPlayer in otherPlayersAtDestination)
                {
                    var otherPlayerRole = PD_Game_Queries.GQ_Find_Player_Role(game, otherPlayer);
                    PD_City otherPlayerLocation = PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);
                    List<PD_CityCard> cityCardsInOtherPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInPlayerHand(game, otherPlayer);

                    foreach (var cityCardToTake in cityCardsInOtherPlayerHand)
                    {

                        bool otherPlayer_Is_Researcher = otherPlayerRole == PD_Player_Roles.Researcher;
                        bool cityCard_Matches_Destination = cityCardToTake.City == destination;
                        bool otherPlayer_IsAt_Destination = otherPlayerLocation == destination;

                        if (
                            otherPlayer_Is_Researcher == true
                            &&
                            otherPlayer_IsAt_Destination == true
                            )
                        {
                            PD_PA_ShareKnowledge_TakeCard_FromResearcher shareKnowledge_Take_FromResearcher_Action =
                                new PD_PA_ShareKnowledge_TakeCard_FromResearcher(
                                    currentPlayer,
                                    otherPlayer,
                                    cityCardToTake
                                    );

                            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();
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
                            PD_PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PD_PA_ShareKnowledge_TakeCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToTake
                                );

                            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();
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
                            PD_PA_ShareKnowledge_TakeCard shareKnowledge_Take_Action = new PD_PA_ShareKnowledge_TakeCard(
                                currentPlayer,
                                otherPlayer,
                                cityCardToTake
                                );

                            List<PD_GameAction_Base> actionsList = new List<PD_GameAction_Base>();
                            actionsList.AddRange(walkSequence);

                            if (numRemainingActions_AfterWalking > 0)
                            {
                                for (int i = 0; i < numRemainingActions_AfterWalking; i++)
                                {
                                    PD_PA_Stay stayAction = new PD_PA_Stay(
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> simpleWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> directFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> charterFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength,
            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_ExecutableNow_ActionSequences_NonMaximumLength
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
            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions
            )
        {
            bool currentPlayerIsScientist = currentPlayerRole == PD_Player_Roles.Scientist;

            List<List<PD_CityCard>> discoverCureCardGroups = PD_Game_Queries.GQ_Find_UsableDiscoverCureCardGroups(game, currentPlayer);

            if (discoverCureCardGroups.Count == 0)
            {
                return new List<PD_MacroAction>();
            }

            bool currentPlayerLocationIsResearchStation = game.MapElements.ResearchStationsPerCityID[currentPlayerLocation.ID].Count > 0;

            if (currentPlayerLocationIsResearchStation == false)
            {
                return new List<PD_MacroAction>();
            }

            List<PD_City> researchStationCities = PD_Game_Queries.GQ_Find_ResearchStationCities(game);

            List<PD_MacroAction> discoverCureMacros = new List<PD_MacroAction>();

            foreach (var cardGroup in discoverCureCardGroups)
            {
                if (currentPlayerIsScientist)
                {
                    PD_PA_DiscoverCure_Scientist discoverCure_Scientist_Action = new PD_PA_DiscoverCure_Scientist(
                        currentPlayer,
                        currentPlayerLocation,
                        cardGroup,
                        cardGroup[0].Type
                        );

                    PD_MacroAction discoverCure_Scientist_Macro = new PD_MacroAction(
                        new List<PD_GameAction_Base>() { discoverCure_Scientist_Action },
                        PD_MacroAction_Type.DiscoverCure_Scientist_Macro,
                        PD_MacroAction_WalkType.None,
                        numAvailableActions
                        );

                    discoverCureMacros.Add(discoverCure_Scientist_Macro);
                }
                else
                {
                    PD_PA_DiscoverCure discoverCure_Action = new PD_PA_DiscoverCure(
                        currentPlayer,
                        currentPlayerLocation,
                        cardGroup,
                        cardGroup[0].Type
                        );

                    PD_MacroAction discoverCure_Macro = new PD_MacroAction(
                        new List<PD_GameAction_Base>() { discoverCure_Action },
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

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            PD_Player_Roles currentPlayerRole,
            int numAvailableActions,

            List<List<PD_GameAction_Base>> walkSequences_NonMaximumLength,
            PD_MacroAction_WalkType walkType
            )
        {
            bool currentPlayerIsScientist = currentPlayerRole == PD_Player_Roles.Scientist;

            List<List<PD_CityCard>> discoverCureCardGroups = PD_Game_Queries.GQ_Find_UsableDiscoverCureCardGroups(game, currentPlayer);

            List<PD_City> researchStationCities = PD_Game_Queries.GQ_Find_ResearchStationCities(game);

            List<PD_MacroAction> discoverCureMacros = new List<PD_MacroAction>();

            if (discoverCureCardGroups.Count == 0)
            {
                return new List<PD_MacroAction>();
            }
            else
            {
                foreach (var walkSequence in walkSequences_NonMaximumLength)
                {
                    var destination = ((PD_MapMovementAction_Base)walkSequence.GetLast()).TargetLocation;

                    var cityCardsUsedForWalking = PD_Macro_Utilities.Find_CityCardsUsedForWalking_In_ActionList(walkSequence);

                    List<List<PD_CityCard>> remainingDiscoverCureCardGroups = new List<List<PD_CityCard>>();
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

                    bool destinationIsResearchStation = game.MapElements.ResearchStationsPerCityID[destination.ID].Count > 0;
                    if (destinationIsResearchStation == false)
                    {
                        continue;
                    }
                    foreach (var cardGroup in remainingDiscoverCureCardGroups)
                    {
                        if (currentPlayerIsScientist)
                        {
                            List<PD_GameAction_Base> allCommands = new List<PD_GameAction_Base>();

                            allCommands.AddRange(walkSequence);

                            PD_PA_DiscoverCure_Scientist discoverCure_Scientist_Action = new PD_PA_DiscoverCure_Scientist(
                                currentPlayer,
                                destination,
                                cardGroup,
                                cardGroup[0].Type
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
                            List<PD_GameAction_Base> allCommands = new List<PD_GameAction_Base>();

                            allCommands.AddRange(walkSequence);

                            PD_PA_DiscoverCure discoverCure_Action = new PD_PA_DiscoverCure(
                                currentPlayer,
                                destination,
                                cardGroup,
                                cardGroup[0].Type
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

        public static List<List<PD_GameAction_Base>> FindAll_SimpleWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,
            PD_Player currentPlayer,
            PD_City currentPlayerLocation
            )
        {
            List<List<PD_GameAction_Base>> simpleWalk_Sequences =
                new List<List<PD_GameAction_Base>>();

            foreach (var city in game.Map.Cities)
            {
                if (city != currentPlayerLocation)
                {
                    List<PD_GameAction_Base> simpleWalkCommandSequence = Compose_SimpleWalk_CommandSequence(
                        game,
                        pathFinder,
                        researchStationCities,
                        currentPlayerLocation,
                        city
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
        public static List<List<PD_GameAction_Base>> FindAll_DirectFlightWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,
            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            List<List<PD_GameAction_Base>> simpleWalk_ActionSequences
            )
        {

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);
            if (cityCardsInCurrentPlayerHand.Count == 0)
            {
                return new List<List<PD_GameAction_Base>>();
            }

            List<PD_CityCard> directFlight_CityCards = cityCardsInCurrentPlayerHand.FindAll(
                x =>
                x.City != currentPlayerLocation
                );
            if (directFlight_CityCards.Count == 0)
            {
                return new List<List<PD_GameAction_Base>>();
            }

            List<List<PD_GameAction_Base>> directFlightWalk_Sequences =
                new List<List<PD_GameAction_Base>>();

            foreach (var cityCard in directFlight_CityCards)
            {
                PD_City directFlightCity = cityCard.City;

                PD_PMA_DirectFlight directFlightAction = new PD_PMA_DirectFlight(
                    currentPlayer,
                    currentPlayerLocation,
                    directFlightCity,
                    cityCard
                    );

                List<PD_GameAction_Base> singleActionList = new List<PD_GameAction_Base>();
                singleActionList.Add(directFlightAction);

                directFlightWalk_Sequences.Add(singleActionList);

                List<PD_City> citiesToWalkTo = game.Map.Cities.FindAll(
                    x =>
                    x != currentPlayerLocation
                    && x != directFlightCity
                    );

                foreach (var finalDestination in citiesToWalkTo)
                {
                    List<PD_GameAction_Base> actionSequence = new List<PD_GameAction_Base>();
                    actionSequence.Add(directFlightAction);
                    List<PD_GameAction_Base> simpleWalkCommandSequence = Compose_SimpleWalk_CommandSequence(
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

            List<List<PD_GameAction_Base>> directFlightWalk_Optimal_Sequences = new List<List<PD_GameAction_Base>>();
            foreach (var seq in directFlightWalk_Sequences)
            {
                int directFlightSequenceLength = seq.Count;
                PD_City targetLocation = ((PD_MapMovementAction_Base)seq.GetLast()).TargetLocation;
                List<PD_GameAction_Base> simpleWalkEquivalent = simpleWalk_ActionSequences.Find(
                    x =>
                    ((PD_MapMovementAction_Base)x.GetLast()).TargetLocation == targetLocation
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

        public static List<List<PD_GameAction_Base>> FindAll_CharterFlightWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,
            PD_Player currentPlayer,
            PD_City currentPlayerLocation,
            List<List<PD_GameAction_Base>> simpleWalk_ActionSequences
            )
        {

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            List<List<PD_GameAction_Base>> charterFlightWalk_Sequences =
                new List<List<PD_GameAction_Base>>();

            foreach (var cityCard in cityCardsInCurrentPlayerHand)
            {
                PD_City charterCity = cityCard.City;

                List<PD_GameAction_Base> simpleWalkToCharterCity = Compose_SimpleWalk_CommandSequence(
                    game,
                    pathFinder,
                    researchStationCities,
                    currentPlayerLocation,
                    charterCity
                    );

                List<PD_City> possible_Destinations = game.Map.Cities.FindAll(
                    x =>
                    x != currentPlayerLocation
                    && x != charterCity
                    );

                foreach (var finalDestination in possible_Destinations)
                {
                    PD_PMA_CharterFlight charterFlightAction = new PD_PMA_CharterFlight(
                        currentPlayer,
                        charterCity,
                        finalDestination,
                        cityCard
                        );

                    List<PD_GameAction_Base> charterWalkCommandSequence = new List<PD_GameAction_Base>();
                    charterWalkCommandSequence.AddRange(simpleWalkToCharterCity);
                    charterWalkCommandSequence.Add(charterFlightAction);

                    charterFlightWalk_Sequences.Add(charterWalkCommandSequence);
                }
            }

            List<List<PD_GameAction_Base>> charterFlightWalk_Optimal_Sequences = new List<List<PD_GameAction_Base>>();
            foreach (var seq in charterFlightWalk_Sequences)
            {
                int directFlightSequenceLength = seq.Count;
                PD_City targetLocation = ((PD_MapMovementAction_Base)seq.GetLast()).TargetLocation;
                List<PD_GameAction_Base> simpleWalkEquivalent = simpleWalk_ActionSequences.Find(
                    x =>
                    ((PD_MapMovementAction_Base)x.GetLast()).TargetLocation == targetLocation
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

        public static List<List<PD_GameAction_Base>> FindAll_OperationsExpertFlightWalk_Sequences(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,

            PD_Player currentPlayer,
            PD_City currentPlayerLocation,

            List<List<PD_GameAction_Base>> simpleWalk_ActionSequences
            )
        {

            PD_Player_Roles currentPlayerRole = PD_Game_Queries.GQ_Find_CurrentPlayer_Role(game);
            if (currentPlayerRole != PD_Player_Roles.Operations_Expert)
            {
                return new List<List<PD_GameAction_Base>>();
            }

            bool operationsExpertFlightHasBeenUsedInThisPlayerTurn = PD_Game_Queries.Find_If_OperationsExpertFlight_HasBeenUsedInThisTurn(game);
            if (operationsExpertFlightHasBeenUsedInThisPlayerTurn)
            {
                return new List<List<PD_GameAction_Base>>();
            }

            List<PD_CityCard> cityCardsInCurrentPlayerHand = PD_Game_Queries.GQ_Find_CityCardsInCurrentPlayerHand(game);

            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_Sequences =
                new List<List<PD_GameAction_Base>>();

            if (PD_Game_Queries.GQ_Is_City_ResearchStation(game, currentPlayerLocation))
            {
                foreach (var cityCardToUse in cityCardsInCurrentPlayerHand)
                {
                    foreach (var finalDestination in game.Map.Cities)
                    {
                        if (finalDestination != currentPlayerLocation)
                        {
                            PD_PMA_OperationsExpert_Flight action = new PD_PMA_OperationsExpert_Flight(
                                currentPlayer,
                                currentPlayerLocation,
                                finalDestination,
                                cityCardToUse
                                );

                            List<PD_GameAction_Base> singleActionList = new List<PD_GameAction_Base>() {
                                action
                            };

                            operationsExpertFlightWalk_Sequences.Add(singleActionList);
                        }
                    }
                }
            }
            else
            {
                PD_City nearest_RS_City = researchStationCities[0];
                pathFinder.Find_RS_ClosestToCity(
                    game,
                    researchStationCities,
                    currentPlayerLocation
                    );

                List<PD_GameAction_Base> simpleWalkSequence = Compose_SimpleWalk_CommandSequence(
                    game,
                    pathFinder,
                    researchStationCities,
                    currentPlayerLocation,
                    nearest_RS_City
                    );

                foreach (var cityCardToUse in cityCardsInCurrentPlayerHand)
                {
                    foreach (var finalDestination in game.Map.Cities)
                    {
                        if (finalDestination != currentPlayerLocation)
                        {
                            PD_PMA_OperationsExpert_Flight finalAction = new PD_PMA_OperationsExpert_Flight(
                                currentPlayer,
                                currentPlayerLocation,
                                nearest_RS_City,
                                cityCardToUse
                                );

                            List<PD_GameAction_Base> totalActionSequence = new List<PD_GameAction_Base>();
                            totalActionSequence.AddRange(simpleWalkSequence);
                            totalActionSequence.Add(finalAction);

                            operationsExpertFlightWalk_Sequences.Add(totalActionSequence);
                        }
                    }
                }
            }


            List<List<PD_GameAction_Base>> operationsExpertFlightWalk_ExecutableNow_Optimal_CommandSequences = new List<List<PD_GameAction_Base>>();
            foreach (var seq in operationsExpertFlightWalk_Sequences)
            {
                int directFlightSequenceLength = seq.Count;
                PD_City targetLocation = ((PD_MapMovementAction_Base)seq.GetLast()).TargetLocation;
                List<PD_GameAction_Base> simpleWalkEquivalent = simpleWalk_ActionSequences.Find(
                    x =>
                    ((PD_MapMovementAction_Base)x.GetLast()).TargetLocation == targetLocation
                    );
                if (simpleWalkEquivalent != null)
                {
                    int simpleWalkLength = simpleWalkEquivalent.Count;
                    if (directFlightSequenceLength < simpleWalkLength)
                    {
                        operationsExpertFlightWalk_ExecutableNow_Optimal_CommandSequences.Add(seq);
                    }
                }
                else
                {
                    operationsExpertFlightWalk_ExecutableNow_Optimal_CommandSequences.Add(seq);
                }
            }

            return operationsExpertFlightWalk_ExecutableNow_Optimal_CommandSequences;
        }
        #region walk-types-command-sequences
        public static List<PD_GameAction_Base> Compose_SimpleWalk_CommandSequence(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,
            PD_City root,
            PD_City destination
            )
        {
            if (game.Map.Cities.Contains(root) == false)
            {
                throw new System.Exception("root not in map");
            }
            if (game.Map.Cities.Contains(destination) == false)
            {
                throw new System.Exception("destination not in map");
            }
            if (root == destination)
            {
                return new List<PD_GameAction_Base>();
            }

            List<PD_City> walkPath = pathFinder.GetPrecalculatedShortestPath(
                game,
                researchStationCities,
                root,
                destination
                );

            if (walkPath.Count < 2) return new List<PD_GameAction_Base>();

            PD_Player currentPlayer = PD_Game_Queries.GQ_Find_CurrentPlayer(game);

            List<PD_GameAction_Base> commands = new List<PD_GameAction_Base>();

            for (int i = 0; i < walkPath.Count - 1; i++)
            {
                var city1 = walkPath[i];
                var city2 = walkPath[i + 1];
                if (game.Map.CityNeighbors_PerCityID[city1.ID].Contains(city2) == true)
                {
                    PD_PMA_DriveFerry driveFerryCommand = new PD_PMA_DriveFerry(
                        currentPlayer,
                        city1,
                        city2
                        );
                    commands.Add(driveFerryCommand);
                }
                else if (
                    researchStationCities.Contains(city1)
                    && researchStationCities.Contains(city2)
                    )
                {
                    PD_PMA_ShuttleFlight shuttleFlightCommand = new PD_PMA_ShuttleFlight(
                        currentPlayer,
                        city1,
                        city2
                        );
                    commands.Add(shuttleFlightCommand);
                }
                else
                {
                    throw new System.Exception("could not create the commands for this walk");
                }
            }

            return commands;
        }

        public static List<PD_GameAction_Base> Compose_CharterWalk_CommandSequence(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,
            PD_City root,
            PD_City destination,
            PD_CityCard charterCard
            )
        {
            PD_Player currentPlayer = PD_Game_Queries.GQ_Find_CurrentPlayer(game);
            var charterCity = charterCard.City;
            // step 1: walk to charter city
            var walkCommands = Compose_SimpleWalk_CommandSequence(
                game,
                pathFinder,
                researchStationCities,
                root,
                charterCity
                );
            // step 2: apply charter command to go from charter city to destination
            PD_PMA_CharterFlight charterCommand = new PD_PMA_CharterFlight(
                currentPlayer,
                charterCity,
                destination,
                charterCard
                );
            // combine commands
            var commandSequence = walkCommands;
            commandSequence.Add(charterCommand);
            return commandSequence;
        }

        public static List<PD_GameAction_Base> Compose_DirectFlightWalk_CommandSequence(
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,
            PD_Game game,
            PD_City root,
            PD_City destination,
            PD_CityCard directFlightCard
            )
        {
            PD_Player currentPlayer = PD_Game_Queries.GQ_Find_CurrentPlayer(game);
            var directFlightCity = directFlightCard.City;

            // step 1: use the direct flight card to go to the direct flight city
            PD_PMA_DirectFlight directFlightCommand = new PD_PMA_DirectFlight(
                currentPlayer,
                root,
                directFlightCity,
                directFlightCard
                );

            // step 2: walk from the direct flight city to the destination
            var walkCommands = Compose_SimpleWalk_CommandSequence(
                game,
                pathFinder,
                researchStationCities,
                directFlightCity,
                destination
                );

            // combine commands
            var commandSequence = new List<PD_GameAction_Base>();
            commandSequence.Add(directFlightCommand);
            commandSequence.AddRange(walkCommands);
            return commandSequence;
        }

        public static List<PD_GameAction_Base> Compose_CombinedWalk_CommandSequence(
            PD_AI_PathFinder pathFinder,
            List<PD_City> researchStationCities,
            PD_Game game,
            PD_City root,
            PD_City destination,
            PD_CityCard directFlightCard,
            PD_CityCard charterFlightCard
            )
        {

            PD_Player currentPlayer = PD_Game_Queries.GQ_Find_CurrentPlayer(game);
            PD_City directFlightCity = directFlightCard.City;
            PD_City charterFlightCity = charterFlightCard.City;

            // step 1: use the direct flight card to go to the direct flight city
            PD_PMA_DirectFlight directFlightCommand = new PD_PMA_DirectFlight(
                currentPlayer,
                root,
                directFlightCity,
                directFlightCard
                );

            // step 2: walk from the direct flight city to the city of the charter flight
            var walkCommands = Compose_SimpleWalk_CommandSequence(
                game,
                pathFinder,
                researchStationCities,
                directFlightCity,
                charterFlightCity
                );

            // step 3: perform a charter flight to go to the destination
            PD_PMA_CharterFlight charterFlightCommand = new PD_PMA_CharterFlight(
                currentPlayer,
                charterFlightCity,
                destination,
                charterFlightCard
                );

            // combine commands
            var commandSequence = new List<PD_GameAction_Base>();
            commandSequence.Add(directFlightCommand);
            commandSequence.AddRange(walkCommands);
            commandSequence.Add(charterFlightCommand);
            return commandSequence;
        }
        #endregion
    }
}
