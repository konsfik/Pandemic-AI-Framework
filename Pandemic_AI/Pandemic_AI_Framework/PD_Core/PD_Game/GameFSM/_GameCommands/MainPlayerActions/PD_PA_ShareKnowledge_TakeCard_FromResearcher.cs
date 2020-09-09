using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_TakeCard_FromResearcher :
        PD_GameAction_Base,
        IEquatable<PD_PA_ShareKnowledge_TakeCard_FromResearcher>,
        I_Player_Action
    {
        public int Player { get; private set; }
        public int OtherPlayer { get; private set; }
        public int CityCardToTake { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="otherPlayer"></param>
        /// <param name="cityCardToTake"></param>
        [JsonConstructor]
        public PD_PA_ShareKnowledge_TakeCard_FromResearcher(
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
        private PD_PA_ShareKnowledge_TakeCard_FromResearcher(
            PD_PA_ShareKnowledge_TakeCard_FromResearcher actionToCopy
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
            else if (game.GQ_Player_Role(OtherPlayer) != PD_Player_Roles.Researcher)
            {
                throw new System.Exception("the other player must be researcher!");
            }
            else if (game.GQ_PlayerLocation(Player) != game.GQ_PlayerLocation(OtherPlayer))
            {
                throw new System.Exception("Players do not share the same location");
            }
#endif
            game.cards.player_hand__per__player[OtherPlayer].Remove(CityCardToTake);
            game.cards.player_hand__per__player[Player].Add(CityCardToTake);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_ShareKnowledge_TakeCard_FromResearcher(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: SHARE_KNOWLEDGE_TAKE {1} FROM RESEARCHER: {2}",
                Player.ToString(),
                CityCardToTake.ToString(),
                OtherPlayer.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PD_PA_ShareKnowledge_TakeCard_FromResearcher other)
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
            if (other is PD_PA_ShareKnowledge_TakeCard_FromResearcher other_action)
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
            if (other is PD_PA_ShareKnowledge_TakeCard_FromResearcher other_action)
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
