﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_Discarding_DuringMainPlayerActions :
        PD_GameStateBase, ICustomDeepCopyable<PD_GS_Discarding_DuringMainPlayerActions>
    {
        #region constructors
        public PD_GS_Discarding_DuringMainPlayerActions GetCustomDeepCopy()
        {
            return new PD_GS_Discarding_DuringMainPlayerActions();
        }
        #endregion

        public override PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_GameAction_Base command
            )
        {
            if (command.GetType() == typeof(PA_Discard_DuringMainPlayerActions))
            {
                command.Execute(
                    randomness_provider,
                    game);

                bool playerActionsFinished = game.SS_PlayerActionsFinished();
                bool allDiseasesCured = game.GQ_SS_AllDiseasesCured();
                bool anyPlayerNeedsToDiscard = game.GQ_SS_AnyPlayerHandIsBiggerThanPermitted();
                bool enoughPlayerCardsToDraw = game.GQ_SS_EnoughPlayerCardsToDraw();

                // stay in same state
                bool stayAt_Discarding_During_MainPlayerActions =
                    anyPlayerNeedsToDiscard == true;

                // go back to applying main player actions
                bool goTo_Applying_MainPlayerActions =
                    playerActionsFinished == false
                    &&
                    anyPlayerNeedsToDiscard == false;

                // proceed to drawing new player cards
                bool goTo_Drawing_New_PlayerCards =
                    playerActionsFinished == true
                    &&
                    anyPlayerNeedsToDiscard == false
                    &&
                    enoughPlayerCardsToDraw == true
                    &&
                    allDiseasesCured == false;

                // go to game lost
                bool goTo_GameLost =
                    playerActionsFinished == true
                    &&
                    allDiseasesCured == false
                    &&
                    enoughPlayerCardsToDraw == false;

                // go to game won
                bool goTo_GameWon =
                    playerActionsFinished == true
                    &&
                    allDiseasesCured == true
                    &&
                    anyPlayerNeedsToDiscard == false;


                if (stayAt_Discarding_During_MainPlayerActions)
                {
                    return null; // stay in same state
                }
                else if (goTo_Applying_MainPlayerActions)
                {
                    return new PD_GS_ApplyingMainPlayerActions();
                }
                else if (goTo_Drawing_New_PlayerCards)
                {
                    return new PD_GS_DrawingNewPlayerCards();
                }
                else if (goTo_GameLost)
                {
                    return new PD_GS_GameLost();
                }
                else if (goTo_GameWon)
                {
                    return new PD_GS_GameWon();
                }
                else
                {
                    throw new System.Exception("something wrong here...");
                }
            }
            else
            {
                throw new System.Exception("something wrong here");
            }
        }

        public override void OnEnter(PD_Game game)
        {

        }

        public override void OnExit(PD_Game game)
        {

        }

        #region equality override
        public bool Equals(PD_GS_Discarding_DuringMainPlayerActions other)
        {
            return true;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GS_Discarding_DuringMainPlayerActions other_state)
            {
                return Equals(other_state);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }

        
        #endregion
    }
}