using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_GameStateBase : IGameState
    {

        public PD_GameStateBase()
        {

        }

        public abstract PD_GameStateBase OnCommand(
            Random randomness_provider,
            PD_Game game, 
            PD_GameAction_Base command
            );
        public abstract void OnEnter(PD_Game game);
        public abstract void OnExit(PD_Game game);

        #region equalityOverride
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
            int hash = 17;

            hash += (hash * 13) + this.GetType().Name.ToString().GetHashCode();

            return hash;
        }


        public static bool operator ==(PD_GameStateBase s1, PD_GameStateBase s2)
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

        public static bool operator !=(PD_GameStateBase s1, PD_GameStateBase s2)
        {
            return !(s1 == s2);
        }
        #endregion
    }
}