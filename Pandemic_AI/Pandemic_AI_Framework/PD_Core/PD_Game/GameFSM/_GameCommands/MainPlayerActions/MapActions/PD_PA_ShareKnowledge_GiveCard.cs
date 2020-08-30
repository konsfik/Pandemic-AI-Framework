using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_GiveCard : PD_MainAction_Base
    {
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
            ) : base(player)
        {
            OtherPlayer = otherPlayer;
            CityCardToGive = cityCardToGive;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_ShareKnowledge_GiveCard(
            PD_PA_ShareKnowledge_GiveCard actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            OtherPlayer = actionToCopy.OtherPlayer.GetCustomDeepCopy();
            CityCardToGive = actionToCopy.CityCardToGive.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PA_ShareKnowledge_GiveCard(Player, OtherPlayer, CityCardToGive);
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
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PA_ShareKnowledge_GiveCard)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            if (this.OtherPlayer != other.OtherPlayer)
            {
                return false;
            }
            if (this.CityCardToGive != other.CityCardToGive)
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
            hash = hash * 31 + OtherPlayer.GetHashCode();
            hash = hash * 31 + CityCardToGive.GetHashCode();

            return hash;
        }

        #endregion
    }
}