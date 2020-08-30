using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// A player action, for which the player has no actual choice.
    /// For example, draw new player cards...
    /// </summary>
    [Serializable]
    public abstract class PD_AutoAction_Base : PD_GameAction_Base, I_Player_Action
    {
        public PD_Player Player { get; protected set; }

        public PD_AutoAction_Base(
            PD_Player player
            )
        {
            Player = player;
        }


    }
}
