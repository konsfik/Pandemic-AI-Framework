using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_Discard_DuringMainPlayerActions : PD_Discard_Base
    {
        /// <summary>
        /// normal && json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cardToDiscard"></param>
        [JsonConstructor]
        public PD_PA_Discard_DuringMainPlayerActions(
            PD_Player player,
            PD_PlayerCardBase cardToDiscard
            ) : base(
                player,
                cardToDiscard
                )
        {

        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            if (PlayerCardToDiscard.GetType() == typeof(PD_CityCard))
            {
                return new PD_PA_Discard_DuringMainPlayerActions(
                    Player.GetCustomDeepCopy(),
                    ((PD_CityCard)PlayerCardToDiscard).GetCustomDeepCopy()
                    );
            }
            else if (PlayerCardToDiscard.GetType() == typeof(PD_EpidemicCard))
            {
                return new PD_PA_Discard_DuringMainPlayerActions(
                    Player.GetCustomDeepCopy(),
                    ((PD_EpidemicCard)PlayerCardToDiscard).GetCustomDeepCopy()
                    );
            }
            else if (PlayerCardToDiscard.GetType() == typeof(PD_InfectionCard))
            {
                return new PD_PA_Discard_DuringMainPlayerActions(
                    Player.GetCustomDeepCopy(),
                    ((PD_InfectionCard)PlayerCardToDiscard).GetCustomDeepCopy()
                    );
            }
            return null;
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_Discard_DuringMainPlayerActions(Player, PlayerCardToDiscard);
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PA_Discard_DuringMainPlayerActions)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            if (this.PlayerCardToDiscard != other.PlayerCardToDiscard)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player.GetHashCode();
            hash = hash * 31 + PlayerCardToDiscard.GetHashCode();

            return hash;
        }

        #endregion
    }

}
