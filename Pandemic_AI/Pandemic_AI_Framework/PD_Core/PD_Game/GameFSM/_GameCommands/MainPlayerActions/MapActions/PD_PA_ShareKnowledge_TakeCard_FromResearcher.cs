using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_TakeCard_FromResearcher : PD_GameAction_Base, I_Player_Action
    {
        public PD_Player Player { get; private set; }
        public PD_Player OtherPlayer { get; private set; }
        public PD_CityCard CityCardToTake { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="otherPlayer"></param>
        /// <param name="cityCardToTake"></param>
        [JsonConstructor]
        public PD_PA_ShareKnowledge_TakeCard_FromResearcher(
            PD_Player player,
            PD_Player otherPlayer,
            PD_CityCard cityCardToTake
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
            Player = actionToCopy.Player.GetCustomDeepCopy();
            OtherPlayer = actionToCopy.OtherPlayer.GetCustomDeepCopy();
            CityCardToTake = actionToCopy.CityCardToTake.GetCustomDeepCopy();
        }

        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Cards.PlayerCardsPerPlayerID[OtherPlayer.ID].Remove(CityCardToTake);
            game.Cards.PlayerCardsPerPlayerID[Player.ID].Add(CityCardToTake);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_ShareKnowledge_TakeCard_FromResearcher(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: SHARE_KNOWLEDGE_TAKE {1} FROM RESEARCHER: {2}",
                Player.Name,
                CityCardToTake.City.Name,
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

            var other = (PD_PA_ShareKnowledge_TakeCard_FromResearcher)otherObject;

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

        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * 31 + Player.GetHashCode();
            hash = hash * 31 + OtherPlayer.GetHashCode();
            hash = hash * 31 + CityCardToTake.GetHashCode();

            return hash;
        }

        #endregion
    }
}
