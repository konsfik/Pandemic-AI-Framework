using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_DrawNewPlayerCards :
        PD_Action,
        IEquatable<PA_DrawNewPlayerCards>,
        I_Auto_Action,
        I_Player_Action
    {
        public int Player { get; protected set; }

        [JsonConstructor]
        public PA_DrawNewPlayerCards(
            int player
            )
        {
            this.Player = player;
        }

        // private constructor, for custom deep copy purposes only
        private PA_DrawNewPlayerCards(
            PA_DrawNewPlayerCards actionToCopy
            )
        {
            this.Player = actionToCopy.Player;
        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PA_DrawNewPlayerCards(this);
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            // draw new player cards...
            var newPlayerCards = new List<int>();

            for (int i = 0; i < 2; i++)
            {
                newPlayerCards.Add(game.cards.divided_deck_of_player_cards.DrawLastElementOfLastSubList());
            }

            game.cards.player_hand__per__player[Player].AddRange(newPlayerCards);
        }

        public override string GetDescription()
        {
            return String.Format("{0}: DRAW Player Cards.", Player.ToString());
        }

        #region equality overrides
        public bool Equals(PA_DrawNewPlayerCards other)
        {
            if (this.Player != other.Player)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(PD_Action other)
        {
            if (other is PA_DrawNewPlayerCards other_action)
            {
                return Equals(other_action);
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object other)
        {
            if (other is PA_DrawNewPlayerCards other_action)
            {
                return Equals(other_action);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player.GetHashCode();

            return hash;
        }



        #endregion
    }
}