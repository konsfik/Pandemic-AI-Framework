using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_ShareKnowledge_GiveCard_ResearcherGives :
        PD_GameAction_Base,
        IEquatable<PA_ShareKnowledge_GiveCard_ResearcherGives>,
        I_Player_Action
    {
        public int Player { get; private set; }
        public int OtherPlayer { get; private set; }
        public int CityCardToGive { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="otherPlayer"></param>
        /// <param name="cityCardToGive"></param>
        [JsonConstructor]
        public PA_ShareKnowledge_GiveCard_ResearcherGives(
            int player,
            int otherPlayer,
            int cityCardToGive
            )
        {
            Player = player;
            OtherPlayer = otherPlayer;
            CityCardToGive = cityCardToGive;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PA_ShareKnowledge_GiveCard_ResearcherGives(
            PA_ShareKnowledge_GiveCard_ResearcherGives actionToCopy
            )
        {
            Player = actionToCopy.Player;
            OtherPlayer = actionToCopy.OtherPlayer;
            CityCardToGive = actionToCopy.CityCardToGive;
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
            else if (game.GQ_Player_Role(Player) != PD_Player_Roles.Researcher)
            {
                throw new System.Exception("wrong player role!");
            }
            else if (game.GQ_PlayerLocation(Player) != game.GQ_PlayerLocation(OtherPlayer))
            {
                throw new System.Exception("players do not share location!");
            }
#endif
            game.cards.player_hand__per__player[Player].Remove(CityCardToGive);
            game.cards.player_hand__per__player[OtherPlayer].Add(CityCardToGive);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PA_ShareKnowledge_GiveCard_ResearcherGives(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: SHARE_KNOWLEDGE_RESEARCHER | GIVE {1} to {2}",
                Player.ToString(),
                CityCardToGive.ToString(),
                OtherPlayer.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PA_ShareKnowledge_GiveCard_ResearcherGives other)
        {
            if (this.Player != other.Player)
            {
                return false;
            }
            else if (this.OtherPlayer != other.OtherPlayer)
            {
                return false;
            }
            else if (this.CityCardToGive != other.CityCardToGive)
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
            if (other is PA_ShareKnowledge_GiveCard_ResearcherGives other_action)
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
            if (other is PA_ShareKnowledge_GiveCard_ResearcherGives other_action)
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
            hash = hash * 31 + CityCardToGive;

            return hash;
        }



        #endregion
    }
}
