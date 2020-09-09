using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_Discard_AfterDrawing : 
        PD_GameAction_Base, 
        IEquatable<PD_PA_Discard_AfterDrawing>,
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
        public PD_PA_Discard_AfterDrawing(
            int player,
            int playerCardToDiscard
            )
        {
            this.Player = player;
            this.PlayerCardToDiscard = playerCardToDiscard;
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            if (PlayerCardToDiscard < 48)
            {
                return new PD_PA_Discard_AfterDrawing(
                    Player,
                    PlayerCardToDiscard
                    );
            }
            else if (PlayerCardToDiscard >= 128)
            {
                return new PD_PA_Discard_AfterDrawing(
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
            if (game.GQ_IsInState_DiscardAfterDrawing() == false)
            {
                throw new System.Exception("wrong state!");
            }
            else if (Player != game.GQ_CurrentPlayer())
            {
                throw new System.Exception("wrong player...");
            }
            else if (
                game.Cards.PlayerCardsPerPlayerID[Player].Count
                <= game.GameSettings.MaximumNumberOfPlayerCardsPerPlayer)
            {
                throw new System.Exception("Player does not need to discard cards!");
            }
#endif
            game.GO_PlayerDiscardsPlayerCard(Player, PlayerCardToDiscard);
        }

        #region equality overrides
        public bool Equals(PD_PA_Discard_AfterDrawing other)
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

        public override bool Equals(PD_GameAction_Base other)
        {
            if (other is PD_PA_Discard_AfterDrawing other_action)
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
            if (other is PD_PA_Discard_AfterDrawing other_action)
            {
                return Equals(other_action);
            }
            else {
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
