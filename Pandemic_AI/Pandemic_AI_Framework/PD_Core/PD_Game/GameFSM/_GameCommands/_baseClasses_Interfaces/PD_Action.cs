using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_Action: 
        IEquatable<PD_Action>,
        ICustomDeepCopyable<PD_Action>
    {

        public abstract PD_Action GetCustomDeepCopy();
        public abstract void Execute(
            Random randomness_provider, 
            PD_Game game
            );
        public abstract string GetDescription();

        #region equalityOverride
        public abstract bool Equals(PD_Action other);
        public abstract override bool Equals(object otherObject);
        public abstract override int GetHashCode();

        public static bool operator ==(PD_Action c1, PD_Action c2)
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

        public static bool operator !=(PD_Action c1, PD_Action c2)
        {
            return !(c1 == c2);
        }
        #endregion
    }
}