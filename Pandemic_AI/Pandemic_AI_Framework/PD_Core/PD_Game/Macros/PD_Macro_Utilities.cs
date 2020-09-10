using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_Macro_Utilities
    {

        public static List<int> Find_CityCardsUsedForWalking_In_ActionList(
            List<PD_GameAction_Base> actionList
            )
        {
            List<int> cityCardsUsedForWalking = new List<int>();
            foreach (var action in actionList)
            {
                if (action is PA_DirectFlight direct_flight_action)
                {
                    int cityCardUsedForWalking = direct_flight_action.UsedCard;
                    cityCardsUsedForWalking.Add(cityCardUsedForWalking);
                }
                else if (action is PA_CharterFlight charter_flight_action)
                {
                    int cityCardUsedForWalking = charter_flight_action.UsedCard;
                    cityCardsUsedForWalking.Add(cityCardUsedForWalking);
                }
                else if (action is PA_OperationsExpert_Flight operations_expert_flight_action)
                {
                    int cityCardUsedForWalking = operations_expert_flight_action.UsedCard;
                    cityCardsUsedForWalking.Add(cityCardUsedForWalking);
                }
            }
            return cityCardsUsedForWalking;
        }
    }
}
