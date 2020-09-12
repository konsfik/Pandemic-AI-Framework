using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_ApplyingMainPlayerActions : PD_GameStateBase, ICustomDeepCopyable<PD_GS_ApplyingMainPlayerActions>
    {
        #region constructors
        public PD_GS_ApplyingMainPlayerActions GetCustomDeepCopy()
        {
            return new PD_GS_ApplyingMainPlayerActions();
        }
        #endregion

        public override PD_GameStateBase OnCommand(
            Random randommness_provider,
            PD_Game game,
            PD_Action command)
        {
            command.Execute(
                randommness_provider,
                game
                );

            game.game_state_counter.IncreasePlayerActionIndex();

            // after executing a main player action:
            bool playerActionsFinished = game.SS_PlayerActionsFinished();
            bool allDiseasesCured = game.GQ_SS_AllDiseasesCured();
            bool anyPlayerNeedsToDiscard = game.GQ_SS_AnyPlayerHandIsBiggerThanPermitted();
            bool enoughPlayerCardsToDraw = game.GQ_SS_EnoughPlayerCardsToDraw();


            bool stayAt_Applying_MainPlayerActions =
                playerActionsFinished == false
                &&
                anyPlayerNeedsToDiscard == false;

            bool goTo_DrawingNewPlayerCards =
                playerActionsFinished == true
                &&
                anyPlayerNeedsToDiscard == false
                &&
                enoughPlayerCardsToDraw == true
                &&
                allDiseasesCured == false;

            bool goTo_Discarding_During_MainPlayerActions =
                anyPlayerNeedsToDiscard == true;

            bool goTo_GameLost =
                playerActionsFinished == true
                &&
                allDiseasesCured == false
                &&
                enoughPlayerCardsToDraw == false;

            bool goTo_GameWon =
                playerActionsFinished == true
                &&
                allDiseasesCured == true
                &&
                anyPlayerNeedsToDiscard == false;


            if (stayAt_Applying_MainPlayerActions)
            {
                return null;
            }
            else if (goTo_DrawingNewPlayerCards)
            {
                return new PD_GS_DrawingNewPlayerCards();
            }
            else if (goTo_Discarding_During_MainPlayerActions)
            {
                return new PD_GS_Discarding_DuringMainPlayerActions();
            }
            else if (goTo_GameLost)
            {
                game.game_state_counter.insufficient_player_cards_to_draw = true;
                return new PD_GS_GameLost();
            }
            else if (goTo_GameWon)
            {
                return new PD_GS_GameWon();
            }
            else
            {
                throw new System.Exception("something wrong here");
            }
        }

        public override void OnEnter(PD_Game game)
        {
            // if the last commmand was not a discard command, then 
            // reset the current player action index...
            if (
                game.PlayerActionsHistory.Count == 0
                ||
                game.PlayerActionsHistory.GetLast().GetType() !=
                typeof(PA_Discard_DuringMainPlayerActions)
                )
            {
                // reset the player action index
                game.game_state_counter.ResetPlayerActionIndex();
                // reset the flag of operations expert flight, so that it can be used again
                game.game_state_counter.operations_expert_flight_used_this_turn = false;
            }
        }

        public override void OnExit(PD_Game game)
        {

        }

        #region equality override
        public bool Equals(PD_GS_ApplyingMainPlayerActions other)
        {
            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_ApplyingMainPlayerActions other_action)
            {
                return Equals(other_action);
            }
            else {
                return true;
            }
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }

        
        #endregion
    }
}