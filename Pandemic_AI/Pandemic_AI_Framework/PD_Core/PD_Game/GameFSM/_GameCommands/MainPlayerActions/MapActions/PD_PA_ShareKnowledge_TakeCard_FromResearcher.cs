using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_TakeCard_FromResearcher : PD_MapAction_Base
    {
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
            ) : base(
                player
                )
        {
            OtherPlayer = otherPlayer;
            CityCardToTake = cityCardToTake;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_ShareKnowledge_TakeCard_FromResearcher(
            PD_PA_ShareKnowledge_TakeCard_FromResearcher actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            OtherPlayer = actionToCopy.OtherPlayer.GetCustomDeepCopy();
            CityCardToTake = actionToCopy.CityCardToTake.GetCustomDeepCopy();
        }

        #endregion

        public override void Execute(PD_Game game)
        {
            game.Com_PA_ShareKnowledge_TakeCard_FromResearcher(
                Player,
                OtherPlayer,
                CityCardToTake
                );
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
    }
}
