using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_TakeCard : PD_MapAction_Base
    {
        public PD_Player OtherPlayer { get; private set; }
        public PD_CityCard CityCardToTake { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json Constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="otherPlayer"></param>
        /// <param name="cityCardToTake"></param>
        [JsonConstructor]
        public PD_PA_ShareKnowledge_TakeCard(
            PD_Player player,
            PD_Player otherPlayer,
            PD_CityCard cityCardToTake
            ) : base(player)
        {
            OtherPlayer = otherPlayer;
            CityCardToTake = cityCardToTake;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_ShareKnowledge_TakeCard(
            PD_PA_ShareKnowledge_TakeCard actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            OtherPlayer = actionToCopy.OtherPlayer.GetCustomDeepCopy();
            CityCardToTake = actionToCopy.CityCardToTake.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PA_ShareKnowledge_TakeCard(Player, OtherPlayer, CityCardToTake);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_ShareKnowledge_TakeCard(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: SHARE_KNOWLEDGE | TAKE {1} from {2}",
                Player.Name,
                CityCardToTake.City.Name,
                OtherPlayer.Name
                );
        }
    }
}