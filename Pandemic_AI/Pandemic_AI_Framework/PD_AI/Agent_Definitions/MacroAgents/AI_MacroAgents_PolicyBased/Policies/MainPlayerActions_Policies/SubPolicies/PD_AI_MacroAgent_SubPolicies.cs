using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public static class PD_AI_MacroAgent_SubPolicies
    {
        public static List<PD_MacroAction> ShareKnowledge_Positive_Now(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
            if (PD_Game_Queries.GQ_IsInState_ApplyingMainPlayerActions(game) == false)
            {
                throw new System.Exception("wrong state.");
            }

            var shareKnowledgeMacros_ExecutableNow = allMacros.FindAll(
                x =>
                x.Is_TypeOf_ShareKnowledge_Any()
                &&
                x.Is_ExecutableNow() == true
                );
            if (shareKnowledgeMacros_ExecutableNow.Count > 0)
            {
                List<PD_MacroAction> positiveShareKnowledgeMacros = new List<PD_MacroAction>();
                foreach (var macro in shareKnowledgeMacros_ExecutableNow)
                {
                    List<PD_GameAction_Base> walkSequence = macro.Actions_All.CustomDeepCopy();
                    walkSequence.RemoveAt(walkSequence.Count - 1);
                    double walkSequenceValue = PD_AI_CardEvaluation_Utilities
                        .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            walkSequence,
                            true
                            );
                    if (walkSequenceValue < 0)
                    {
                        continue;
                    }

                    PD_GameAction_Base lastCommand = macro.Find_LastCommand();
                    double exchangeValue =
                        PD_AI_CardEvaluation_Utilities
                        .Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            lastCommand,
                            true
                            );

                    if (exchangeValue > 0.0)
                    {
                        positiveShareKnowledgeMacros.Add(macro);
                    }
                }
                if (positiveShareKnowledgeMacros.Count > 0)
                {
                    return positiveShareKnowledgeMacros;
                }
            }

            return new List<PD_MacroAction>();
        }

        public static List<PD_MacroAction> ShareKnowledge_Positive_NextRound(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
            if (PD_Game_Queries.GQ_IsInState_ApplyingMainPlayerActions(game) == false)
            {
                throw new System.Exception("wrong state.");
            }

            int numberOfStepsUntilEndOfNextRound = 4 + PD_Game_Queries.GQ_Count_RemainingPlayerActions_ThisRound(game);
            var shareKnowledgeMacros_ExecutableNextRound = allMacros.FindAll(
                x =>
                x.Is_TypeOf_ShareKnowledge_Any()
                &&
                x.Is_ExecutableNow() == false
                &&
                x.Count_Total_Length() <= numberOfStepsUntilEndOfNextRound
                );
            if (shareKnowledgeMacros_ExecutableNextRound.Count > 0)
            {
                List<PD_MacroAction> positiveShareKnowledgeMacros = new List<PD_MacroAction>();
                foreach (var macro in shareKnowledgeMacros_ExecutableNextRound)
                {
                    List<PD_GameAction_Base> walkSequence = macro.Actions_All.CustomDeepCopy();
                    walkSequence.RemoveAt(walkSequence.Count - 1);
                    double walkSequenceValue = PD_AI_CardEvaluation_Utilities
                        .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            walkSequence,
                            true
                            );
                    if (walkSequenceValue < 0)
                    {
                        continue;
                    }

                    PD_GameAction_Base lastCommand = macro.Find_LastCommand();
                    double exchangeValue =
                        PD_AI_CardEvaluation_Utilities
                        .Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            lastCommand,
                            true
                            );

                    if (exchangeValue > 0.0)
                    {
                        positiveShareKnowledgeMacros.Add(macro);
                    }
                }
                if (positiveShareKnowledgeMacros.Count > 0)
                {
                    return positiveShareKnowledgeMacros;
                }
            }

            return new List<PD_MacroAction>();
        }

        public static List<PD_MacroAction> TakePosition_Positive_Now_OtherPlayerWithinFourSteps(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
            var takePositionFor_shareKnowledge_Macros_ExecutableNow = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TakePositionFor_ShareKnowledge_Any()
                &&
                x.Is_ExecutableNow()
                );

            if (takePositionFor_shareKnowledge_Macros_ExecutableNow.Count > 0)
            {
                List<PD_MacroAction> positiveTakePosition_Macros = new List<PD_MacroAction>();
                // find all take position macros that would have a positive outcome (ability to cure)
                foreach (var macro in takePositionFor_shareKnowledge_Macros_ExecutableNow)
                {
                    List<PD_GameAction_Base> walkSequence = macro.Actions_All;
                    double walkSequenceValue = PD_AI_CardEvaluation_Utilities
                        .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            walkSequence,
                            true
                            );
                    if (walkSequenceValue < 0)
                    {
                        continue;
                    }

                    var lastCommand = macro.NonExecutable_ShareKnowledge_Action;

                    double exchangeValue =
                        PD_AI_CardEvaluation_Utilities
                        .Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            lastCommand,
                            true
                            );

                    if (exchangeValue > 0.0)
                    {
                        positiveTakePosition_Macros.Add(macro);
                    }
                }

                if (positiveTakePosition_Macros.Count > 0)
                {
                    // find all of them that could be executed by the other player in one turn (4 steps)
                    List<PD_MacroAction> executableByOtherPlayerInOneTurn = new List<PD_MacroAction>();
                    foreach (var macro in positiveTakePosition_Macros)
                    {
                        if (
                            macro.NonExecutable_ShareKnowledge_Action.GetType()
                            ==
                            typeof(PD_PA_ShareKnowledge_GiveCard)
                            )
                        {
                            PD_PA_ShareKnowledge_GiveCard action =
                                (PD_PA_ShareKnowledge_GiveCard)macro.NonExecutable_ShareKnowledge_Action;
                            PD_City destination = action.CityCardToGive.City;
                            PD_Player otherPlayer = action.OtherPlayer;
                            PD_City otherPlayerLocation =
                                PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);
                            int distanceFromOtherPlayer = pathFinder.GetPrecalculatedShortestDistance(
                                game,
                                PD_Game_Queries.GQ_Find_ResearchStationCities(game),
                                destination,
                                otherPlayerLocation
                                );
                            if (distanceFromOtherPlayer < 4)
                            {
                                executableByOtherPlayerInOneTurn.Add(macro);
                            }
                        }
                        else if (
                            macro.NonExecutable_ShareKnowledge_Action.GetType()
                            ==
                            typeof(PD_PA_ShareKnowledge_TakeCard)
                            )
                        {
                            PD_PA_ShareKnowledge_TakeCard action =
                                (PD_PA_ShareKnowledge_TakeCard)macro.NonExecutable_ShareKnowledge_Action;
                            PD_City destination = action.CityCardToTake.City;
                            PD_Player otherPlayer = action.OtherPlayer;
                            PD_City otherPlayerLocation =
                                PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);
                            int distanceFromOtherPlayer = pathFinder.GetPrecalculatedShortestDistance(
                                game,
                                PD_Game_Queries.GQ_Find_ResearchStationCities(game),
                                destination,
                                otherPlayerLocation
                                );
                            if (distanceFromOtherPlayer < 4)
                            {
                                executableByOtherPlayerInOneTurn.Add(macro);
                            }
                        }
                    }

                    if (executableByOtherPlayerInOneTurn.Count > 0)
                    {
                        return executableByOtherPlayerInOneTurn;
                    }
                }
            }
            return new List<PD_MacroAction>();
        }

        public static List<PD_MacroAction> TakePosition_Positive_Now(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
            var takePosition_Now = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TakePositionFor_ShareKnowledge_Any()
                &&
                x.Is_ExecutableNow()
                );

            if (takePosition_Now.Count > 0)
            {
                List<PD_MacroAction> takePosition_Positive_Now = new List<PD_MacroAction>();
                // find all take position macros that would have a positive outcome (ability to cure)
                foreach (var macro in takePosition_Now)
                {
                    List<PD_GameAction_Base> walkSequence = macro.Actions_All;
                    double walkSequenceValue = PD_AI_CardEvaluation_Utilities
                        .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            walkSequence,
                            true
                            );
                    if (walkSequenceValue < 0)
                    {
                        continue;
                    }

                    var lastCommand = macro.NonExecutable_ShareKnowledge_Action;

                    double exchangeValue =
                        PD_AI_CardEvaluation_Utilities
                        .Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            lastCommand,
                            true
                            );

                    if (exchangeValue > 0.0)
                    {
                        takePosition_Positive_Now.Add(macro);
                    }
                }

                if (takePosition_Positive_Now.Count > 0)
                {
                    return takePosition_Positive_Now;
                }
            }
            return new List<PD_MacroAction>();
        }

        public static List<PD_MacroAction> TakePosition_Positive_NextRound_OtherPlayerWithinFourSteps(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {
            int numberOfStepsUntilEndOfNextRound = 4 + PD_Game_Queries.GQ_Count_RemainingPlayerActions_ThisRound(game);
            var takePosition_NextRound = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TakePositionFor_ShareKnowledge_Any()
                &&
                x.Is_ExecutableNow() == false
                &&
                x.Count_Total_Length() <= numberOfStepsUntilEndOfNextRound
                );

            if (takePosition_NextRound.Count > 0)
            {
                List<PD_MacroAction> takePosition_Positive_NextRound = new List<PD_MacroAction>();
                // find all take position macros that would have a positive outcome (ability to cure)
                foreach (var macro in takePosition_NextRound)
                {
                    List<PD_GameAction_Base> walkSequence = macro.Actions_All;
                    double walkSequenceValue = PD_AI_CardEvaluation_Utilities
                        .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            walkSequence,
                            true
                            );
                    if (walkSequenceValue < 0)
                    {
                        continue;
                    }

                    var lastCommand = macro.NonExecutable_ShareKnowledge_Action;

                    double exchangeValue =
                        PD_AI_CardEvaluation_Utilities
                        .Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            lastCommand,
                            true
                            );

                    if (exchangeValue > 0.0)
                    {
                        takePosition_Positive_NextRound.Add(macro);
                    }
                }

                if (takePosition_Positive_NextRound.Count > 0)
                {
                    // find all of them that could be executed by the other player in one turn (4 steps)
                    List<PD_MacroAction> executableByOtherPlayerInOneTurn = new List<PD_MacroAction>();
                    foreach (var macro in takePosition_Positive_NextRound)
                    {
                        if (
                            macro.NonExecutable_ShareKnowledge_Action.GetType()
                            ==
                            typeof(PD_PA_ShareKnowledge_GiveCard)
                            )
                        {
                            PD_PA_ShareKnowledge_GiveCard action =
                                (PD_PA_ShareKnowledge_GiveCard)macro.NonExecutable_ShareKnowledge_Action;
                            PD_City destination = action.CityCardToGive.City;
                            PD_Player otherPlayer = action.OtherPlayer;
                            PD_City otherPlayerLocation =
                                PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);
                            int distanceFromOtherPlayer = pathFinder.GetPrecalculatedShortestDistance(
                                game,
                                PD_Game_Queries.GQ_Find_ResearchStationCities(game),
                                destination,
                                otherPlayerLocation
                                );
                            if (distanceFromOtherPlayer < 4)
                            {
                                executableByOtherPlayerInOneTurn.Add(macro);
                            }
                        }
                        else if (
                            macro.NonExecutable_ShareKnowledge_Action.GetType()
                            ==
                            typeof(PD_PA_ShareKnowledge_TakeCard)
                            )
                        {
                            PD_PA_ShareKnowledge_TakeCard action =
                                (PD_PA_ShareKnowledge_TakeCard)macro.NonExecutable_ShareKnowledge_Action;
                            PD_City destination = action.CityCardToTake.City;
                            PD_Player otherPlayer = action.OtherPlayer;
                            PD_City otherPlayerLocation =
                                PD_Game_Queries.GQ_Find_PlayerLocation(game, otherPlayer);
                            int distanceFromOtherPlayer = pathFinder.GetPrecalculatedShortestDistance(
                                game,
                                PD_Game_Queries.GQ_Find_ResearchStationCities(game),
                                destination,
                                otherPlayerLocation
                                );
                            if (distanceFromOtherPlayer < 4)
                            {
                                executableByOtherPlayerInOneTurn.Add(macro);
                            }
                        }
                    }

                    if (executableByOtherPlayerInOneTurn.Count > 0)
                    {
                        return executableByOtherPlayerInOneTurn;
                    }
                }
            }
            return new List<PD_MacroAction>();
        }

        public static List<PD_MacroAction> TakePosition_Positive_NextRound(
            PD_Game game,
            PD_AI_PathFinder pathFinder,
            List<PD_MacroAction> allMacros
            )
        {

            int numberOfStepsUntilEndOfNextRound = 4 + PD_Game_Queries.GQ_Count_RemainingPlayerActions_ThisRound(game);
            var takePosition_NextRound = allMacros.FindAll(
                x =>
                x.Is_TypeOf_TakePositionFor_ShareKnowledge_Any()
                &&
                x.Is_ExecutableNow() == false
                &&
                x.Count_Total_Length() <= numberOfStepsUntilEndOfNextRound
                );

            if (takePosition_NextRound.Count > 0)
            {
                List<PD_MacroAction> takePosition_Positive_NextRound = new List<PD_MacroAction>();
                // find all take position macros that would have a positive outcome (ability to cure)
                foreach (var macro in takePosition_NextRound)
                {
                    List<PD_GameAction_Base> walkSequence = macro.Actions_All;
                    double walkSequenceValue = PD_AI_CardEvaluation_Utilities
                        .Calculate_ListOfPlayerActions_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            walkSequence,
                            true
                            );
                    if (walkSequenceValue < 0)
                    {
                        continue;
                    }

                    var lastCommand = macro.NonExecutable_ShareKnowledge_Action;

                    double exchangeValue =
                        PD_AI_CardEvaluation_Utilities
                        .Calculate_PlayerAction_Effect_On_Percent_AbilityToCureDiseases(
                            game,
                            lastCommand,
                            true
                            );

                    if (exchangeValue > 0.0)
                    {
                        takePosition_Positive_NextRound.Add(macro);
                    }
                }

                if (takePosition_Positive_NextRound.Count > 0)
                {
                    return takePosition_Positive_NextRound;
                }
            }
            return new List<PD_MacroAction>();
        }


        
    }
}
