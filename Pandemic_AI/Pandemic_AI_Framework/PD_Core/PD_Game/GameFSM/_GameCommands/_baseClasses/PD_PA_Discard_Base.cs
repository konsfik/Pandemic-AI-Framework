using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_Discard_Base : PD_MainAction_Base
    {
        public PD_PlayerCardBase PlayerCardToDiscard { get; private set; }

        public PD_Discard_Base(
            PD_Player player,
            PD_PlayerCardBase playerCardToDiscard
            ) : base(player)
        {
            PlayerCardToDiscard = playerCardToDiscard;
        }

        

        

        public override string GetDescription()
        {
            if (PlayerCardToDiscard.GetType() == typeof(PD_CityCard))
            {
                var cityCard = (PD_CityCard)PlayerCardToDiscard;
                return String.Format("{0} discards the city card of {1} from their hand", Player.Name, cityCard.City.Name);
            }
            else return "";
        }
    }
}
