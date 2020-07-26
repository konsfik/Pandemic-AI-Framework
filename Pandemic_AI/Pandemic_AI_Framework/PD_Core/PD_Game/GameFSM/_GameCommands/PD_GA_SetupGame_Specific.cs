using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_GA_SetupGame_Specific: PD_GameAction_Base
    {
        public PD_GA_SetupGame_Specific()
        {

        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            throw new NotImplementedException();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}
