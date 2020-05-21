using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_NonMapAction_Base : PD_MainAction_Base
    {
        public PD_NonMapAction_Base(
            PD_Player player
            ) : base(
                player
                )
        {

        }
    }
}
