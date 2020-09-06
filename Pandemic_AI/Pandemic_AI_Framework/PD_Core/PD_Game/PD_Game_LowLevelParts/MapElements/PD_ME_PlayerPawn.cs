using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ME_PlayerPawn : 
        PD_MapElement_Base, 
        IEquatable<PD_ME_PlayerPawn>,
        ICustomDeepCopyable<PD_ME_PlayerPawn>
    {
        public PD_Player_Roles Role;

        /// <summary>
        /// normal constructor, also json constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        [JsonConstructor]
        public PD_ME_PlayerPawn(
            int id,
            PD_Player_Roles role
            ) : base(id)
        {
            Role = role;
        }

        /// <summary>
        /// private constructor, for use with custom deep copy, only!
        /// </summary>
        /// <param name="playerPawnToCopy"></param>
        private PD_ME_PlayerPawn(
            PD_ME_PlayerPawn playerPawnToCopy
            ) : base(
                playerPawnToCopy.ID
                )
        {
            this.Role = playerPawnToCopy.Role;
        }

        public PD_ME_PlayerPawn GetCustomDeepCopy()
        {
            return new PD_ME_PlayerPawn(this);
        }

        #region equality overrides
        public bool Equals(PD_ME_PlayerPawn other) {
            if (this.ID != other.ID)
            {
                return false;
            }
            else if (this.Role != other.Role)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_ME_PlayerPawn other_pawn)
            {
                return Equals(other_pawn);
            }
            else 
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + ID;
            hash = hash * 31 + Role.GetHashCode();

            return hash;
        }

        #endregion

        public override string ToString()
        {
            return String.Format(
                "{0}, {1}",
                ID,
                Role.ToString()
                );
        }
    }
}
