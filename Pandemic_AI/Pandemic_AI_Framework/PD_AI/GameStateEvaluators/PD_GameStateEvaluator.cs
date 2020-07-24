using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandemic_AI_Framework;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_GameStateEvaluator
    {
        public abstract double EvaluateGameState(PD_Game game);
    }
}
