using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_GiveCard :
        PD_GameAction_Base,
        IEquatable<PD_PA_ShareKnowledge_GiveCard>,
        I_Player_Action
    {
        public PD_Player Player { get; private set; }
        public PD_Player OtherPlayer { get; private set; }
        public PD_CityCard CityCardToGive { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="otherPlayer"></param>
        /// <param name="cityCardToGive"></param>
        [JsonConstructor]
        public PD_PA_ShareKnowledge_GiveCard(
            PD_Player player,
            PD_Player otherPlayer,
            PD_CityCard cityCardToGive
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
        private PD_PA_ShareKnowledge_GiveCard(
            PD_PA_ShareKnowledge_GiveCard actionToCopy
            )
        {
            Player = actionToCopy.Player.GetCustomDeepCopy();
            OtherPlayer = actionToCopy.OtherPlayer.GetCustomDeepCopy();
            CityCardToGive = actionToCopy.CityCardToGive.GetCustomDeepCopy();
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
            else if (game.GQ_Find_Player_Role(Player) == PD_Player_Roles.Researcher)
            {
                throw new System.Exception("wrong player role!");
            }
            else if (game.GQ_PlayerLocation(Player) != game.GQ_PlayerLocation(OtherPlayer))
            {
                throw new System.Exception("Players do not share the same location");
            }
            else if (game.GQ_PlayerLocation(Player) != CityCardToGive.City)
            {
                throw new System.Exception("Player is not on the correct city");
            }
#endif
            game.Cards.PlayerCardsPerPlayerID[Player.ID].Remove(CityCardToGive);
            game.Cards.PlayerCardsPerPlayerID[OtherPlayer.ID].Add(CityCardToGive);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_ShareKnowledge_GiveCard(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: SHARE_KNOWLEDGE | GIVE {1} to {2}",
                Player.Name,
                CityCardToGive.City.Name,
                OtherPlayer.Name
                );
        }

        #region equality overrides
        public bool Equals(PD_PA_ShareKnowledge_GiveCard other)
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
            if (other is PD_PA_ShareKnowledge_GiveCard other_action)
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
            if (other is PD_PA_ShareKnowledge_GiveCard other_action)
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
            hash = hash * 31 + OtherPlayer.GetHashCode();
            hash = hash * 31 + CityCardToGive.GetHashCode();

            return hash;
        }



        #endregion
    }
}