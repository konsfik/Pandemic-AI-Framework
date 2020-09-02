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
            CurrentState = new PD_GS_Idle();
            CurrentState.OnEnter(game);
        }

        // custom constructor for deep copy
        // and for json serializer
        [JsonConstructor]
        public PD_GameFSM(
            PD_GameStateBase currentState
            )
        {
            if (currentState.GetType() == typeof(PD_GS_Idle))
                CurrentState = ((PD_GS_Idle)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_ApplyingMainPlayerActions))
                CurrentState = ((PD_GS_ApplyingMainPlayerActions)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_Discarding_DuringMainPlayerActions))
                CurrentState = ((PD_GS_Discarding_DuringMainPlayerActions)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_DrawingNewPlayerCards))
                CurrentState = ((PD_GS_DrawingNewPlayerCards)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_ApplyingEpidemicCard))
                CurrentState = ((PD_GS_ApplyingEpidemicCard)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_Discarding_AfterDrawing))
                CurrentState = ((PD_GS_Discarding_AfterDrawing)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_DrawingNewInfectionCards))
                CurrentState = ((PD_GS_DrawingNewInfectionCards)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_ApplyingInfectionCards))
                CurrentState = ((PD_GS_ApplyingInfectionCards)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_GameLost))
                CurrentState = ((PD_GS_GameLost)currentState)
                    .GetCustomDeepCopy();
            else if (currentState.GetType() == typeof(PD_GS_GameWon))
                CurrentState = ((PD_GS_GameWon)currentState)
                    .GetCustomDeepCopy();
        }
        #endregion

        public void OnCommand(
            Random randomness_provider,
            PD_Game game,
            PD_GameAction_Base command
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

        public PD_GameFSM GetCustomDeepCopy()
        {
            return new PD_GameFSM(
                CurrentState
                );
        }

        #region equalityOverride
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            PD_GameFSM other = (PD_GameFSM)otherObject;

            if (this.CurrentState != other.CurrentState) {
                return false;
            }

            return true;
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