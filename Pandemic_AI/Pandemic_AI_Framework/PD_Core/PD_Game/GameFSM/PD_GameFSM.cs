using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GameFSM : ICustomDeepCopyable<PD_GameFSM>
    {
        public PD_GameStateBase CurrentState { get; protected set; }

        #region constructors
        public PD_GameFSM(PD_Game game)
        {
            CurrentState = new PD_GS_ApplyingMainPlayerActions();
            CurrentState.OnEnter(game);
        }

        // custom constructor for deep copy
        // and for json serializer
        [JsonConstructor]
        public PD_GameFSM(
            PD_GameStateBase currentState
            )
        {
            if (currentState is PD_GS_ApplyingMainPlayerActions main_actions_state)
                CurrentState = main_actions_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_Discarding_DuringMainPlayerActions discarding_main_state)
                CurrentState = discarding_main_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_DrawingNewPlayerCards drawing_player_cards_state)
                CurrentState = drawing_player_cards_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_ApplyingEpidemicCard epidemic_state)
                CurrentState = epidemic_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_Discarding_AfterDrawing discarding_after_state)
                CurrentState = discarding_after_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_DrawingNewInfectionCards drawing_infection_cards_state)
                CurrentState = drawing_infection_cards_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_ApplyingInfectionCards infection_state)
                CurrentState = infection_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_GameLost lost_state)
                CurrentState = lost_state.GetCustomDeepCopy();
            else if (currentState is PD_GS_GameWon won_state)
                CurrentState = won_state.GetCustomDeepCopy();
        }

        public PD_GameFSM GetCustomDeepCopy()
        {
            return new PD_GameFSM(
                CurrentState
                );
        }
        #endregion

        public void OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_Action command
            )
        {
            PD_GameStateBase nextState = CurrentState.OnCommand(
                randomness_provider,
                game,
                command
                );
            if (nextState == null)
            {
                // just stay on the same state
            }
            else if (nextState.GetType() == CurrentState.GetType())
            {
                CurrentState.OnExit(game);
                CurrentState = nextState;
                CurrentState.OnEnter(game);
            }
            else
            {
                CurrentState.OnExit(game);
                CurrentState = nextState;
                CurrentState.OnEnter(game);
            }
        }



        #region equalityOverride
        public bool Equals(PD_GameFSM other)
        {
            if (other.CurrentState == this.CurrentState)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_GameFSM other_game_fsm)
            {
                return Equals(other_game_fsm);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + this.CurrentState.GetHashCode();

            return hash;
        }



        public static bool operator ==(PD_GameFSM s1, PD_GameFSM s2)
        {
            if (Object.ReferenceEquals(s1, null))
            {
                if (Object.ReferenceEquals(s2, null))
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
                if (Object.ReferenceEquals(s2, null)) // c2 is null
                {
                    return false;
                }
            }
            // c1 is not null && c2 is not null
            // -> actually check equality
            return s1.Equals(s2);
        }

        public static bool operator !=(PD_GameFSM s1, PD_GameFSM s2)
        {
            return !(s1 == s2);
        }
        #endregion
    }
}