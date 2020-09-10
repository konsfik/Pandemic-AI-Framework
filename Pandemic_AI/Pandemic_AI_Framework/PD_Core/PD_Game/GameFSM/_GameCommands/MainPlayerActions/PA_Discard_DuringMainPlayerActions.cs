using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_Discard_DuringMainPlayerActions :
        PD_Action,
        IEquatable<PA_Discard_DuringMainPlayerActions>,
        I_Player_Action,
        I_Discard_Action
    {
        public int Player { get; private set; }
        public int PlayerCardToDiscard { get; protected set; }

        /// <summary>
        /// normal && json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="playerCardToDiscard"></param>
        [JsonConstructor]
        public PA_Discard_DuringMainPlayerActions(
            int player,
            int playerCardToDiscard
            )
        {
            this.Player = player;
            this.PlayerCardToDiscard = playerCardToDiscard;
        }

        public override PD_Action GetCustomDeepCopy()
        {
            if (PlayerCardToDiscard < 48 || PlayerCardToDiscard >= 128) {
                return new PA_Discard_DuringMainPlayerActions(
                    Player,
                    PlayerCardToDiscard
                    );
            }
            return null;
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
#if DEBUG
            if (game.GQ_IsInState_DiscardDuringMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state!");
            }
            else if (
                game.cards.player_hand__per__player[Player].Count
                <= game.game_settings.maximum_player_hand_size)
            {
                throw new System.Exception("Player does not need to discard cards!");
            }
#endif
            game.GO_PlayerDiscardsPlayerCard(Player, PlayerCardToDiscard);
        }

        #region equality overrides
        public bool Equals(PA_Discard_DuringMainPlayerActions other)
        {
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

        public override bool Equals(PD_Action other)
        {
            if (other is PA_Discard_DuringMainPlayerActions other_action)
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
            if (other is PA_Discard_DuringMainPlayerActions other_action)
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

            hash = hash * 31 + Player;
            hash = hash * 31 + PlayerCardToDiscard;

            return hash;
        }

        #endregion

        public override string GetDescription()
        {
            return String.Format(
                "{0} discards the card: {1}",
                this.Player.ToString(),
                this.PlayerCardToDiscard.ToString()
                );
        }


    }

}
