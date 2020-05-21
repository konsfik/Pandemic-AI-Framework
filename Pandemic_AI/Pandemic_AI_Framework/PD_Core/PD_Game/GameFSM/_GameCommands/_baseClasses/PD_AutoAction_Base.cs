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
    public abstract class PD_AutoAction_Base : PD_PlayerAction_Base
    {
        public PD_AutoAction_Base(
            PD_Player player
            ) : base(player)
        {

        }
    }
}
