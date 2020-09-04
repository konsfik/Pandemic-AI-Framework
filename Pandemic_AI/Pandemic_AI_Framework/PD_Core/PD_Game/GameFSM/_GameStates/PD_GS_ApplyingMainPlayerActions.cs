using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_ApplyingMainPlayerActions : PD_GameStateBase, ICustomDeepCopyable<PD_GS_ApplyingMainPlayerActions>
    {
        #region constructors
        public PD_GS_ApplyingMainPlayerActions()
        {

        }

        public PD_GS_ApplyingMainPlayerActions GetCustomDeepCopy()
        {
            return new PD_GS_ApplyingMainPlayerActions();
        }
        #endregion

        public override PD_GameStateBase OnCommand(
            Random randommness_provider,
            PD_Game game,
            PD_GameAction_Base command)
        {
            command.Execute(
                randommness_provider,
                game
                );

            game.GameStateCounter.IncreasePlayerActionIndex();

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
                game.GameStateCounter.NotEnoughPlayerCardsToDraw = true;
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
                game.PlayerActionsHistory.GetLast().GetType() !=
                typeof(PD_PA_Discard_DuringMainPlayerActions)
                )
            {
                game.GameStateCounter.ResetPlayerActionIndex();
            }
        }

        public override void OnExit(PD_Game game)
        {

        }

        #region equality override
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
        #endregion
    }
}