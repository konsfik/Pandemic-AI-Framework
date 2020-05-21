using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_RollingHorizon_Agent_Report : PD_Report_Part
    {
        public int Times_AgentUsed { get; private set; }
        public int Total_Successful_Mutations { get; private set; }
        public int Total_Successful_Mutations_FirstActionDifferent { get; private set; }
        public int Times_AgentUsed_With_Successful_Mutations { get; private set; }
        public int Times_AgentUsed_With_Successful_Mutations_FirstActionDifferent { get; private set; }

        public PD_RollingHorizon_Agent_Report()
        {
            Reset();
        }

        public void Reset() {
            Times_AgentUsed = 0;
            Total_Successful_Mutations = 0;
            Total_Successful_Mutations_FirstActionDifferent = 0;
            Times_AgentUsed_With_Successful_Mutations = 0;
            Times_AgentUsed_With_Successful_Mutations_FirstActionDifferent = 0;
        }

        public void Update(PD_Game game, PD_AI_PathFinder pathFinder, PolicyBased_RHE_Agent rollingHorizonAgent)
        {
            Times_AgentUsed++;
            Total_Successful_Mutations += rollingHorizonAgent.Num_Successful_Mutations_ThisTime;
            Total_Successful_Mutations_FirstActionDifferent += rollingHorizonAgent.Num_Successful_Mutations_Different_FirstAction_ThisTime;
            if (rollingHorizonAgent.Num_Successful_Mutations_ThisTime > 0)
            {
                Times_AgentUsed_With_Successful_Mutations++;
            }
            if (rollingHorizonAgent.Num_Successful_Mutations_Different_FirstAction_ThisTime > 0)
            {
                Times_AgentUsed_With_Successful_Mutations_FirstActionDifferent++;
            }
        }

        public override void Update(PD_Game game, PD_AI_PathFinder pathFinder)
        {
            throw new NotImplementedException();
        }
    }
}
