using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_ShareKnowledge_TakeCard :
        PD_GameAction_Base,
        IEquatable<PA_ShareKnowledge_TakeCard>,
        I_Player_Action
    {
        public int Player { get; private set; }
        public int OtherPlayer { get; private set; }
        public int CityCardToTake { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json Constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="otherPlayer"></param>
        /// <param name="cityCardToTake"></param>
        [JsonConstructor]
        public PA_ShareKnowledge_TakeCard(
            int player,
            int otherPlayer,
            int cityCardToTake
            )
        {
            Player = player;
            OtherPlayer = otherPlayer;
            CityCardToTake = cityCardToTake;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PA_ShareKnowledge_TakeCard(
            PA_ShareKnowledge_TakeCard actionToCopy
            )
        {
            Player = actionToCopy.Player;
            OtherPlayer = actionToCopy.OtherPlayer;
            CityCardToTake = actionToCopy.CityCardToTake;
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
#if DEBUG
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state!");
            }
            else if (Player != game.GQ_CurrentPlayer())
            {
                throw new System.Exception("wrong player!");
            }
            else if (game.GQ_Player_Role(OtherPlayer) == PD_Player_Roles.Researcher)
            {
                throw new System.Exception("the other player must not be researcher!");
            }
            else if (game.GQ_PlayerLocation(Player) != game.GQ_PlayerLocation(OtherPlayer))
            {
                throw new System.Exception("Players do not share the same location");
            }
            else if (game.GQ_PlayerLocation(Player) != CityCardToTake)
            {
                throw new System.Exception("Player is not on the correct city");
            }
#endif
            game.cards.player_hand__per__player[OtherPlayer].Remove(CityCardToTake);
            game.cards.player_hand__per__player[game.GQ_CurrentPlayer()].Add(CityCardToTake);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PA_ShareKnowledge_TakeCard(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: SHARE_KNOWLEDGE | TAKE {1} from {2}",
                Player.ToString(),
                CityCardToTake.ToString(),
                OtherPlayer.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PA_ShareKnowledge_TakeCard other)
        {
            if (this.Player != other.Player)
            {
                return false;
            }
            if (this.OtherPlayer != other.OtherPlayer)
            {
                return false;
            }
            if (this.CityCardToTake != other.CityCardToTake)
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
            if (other is PA_ShareKnowledge_TakeCard other_action)
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
            if (other is PA_ShareKnowledge_TakeCard other_action)
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
            hash = hash * 31 + OtherPlayer;
            hash = hash * 31 + CityCardToTake;

            return hash;
        }



        #endregion
    }
}