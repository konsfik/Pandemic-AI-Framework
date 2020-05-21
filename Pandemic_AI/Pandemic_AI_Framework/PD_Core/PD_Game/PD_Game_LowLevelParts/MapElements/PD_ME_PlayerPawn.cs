using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ME_PlayerPawn : PD_MapElement_Base, ICustomDeepCopyable<PD_ME_PlayerPawn>
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

    }
}
