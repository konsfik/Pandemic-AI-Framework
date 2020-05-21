﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_GiveCard_ResearcherGives : PD_MapAction_Base
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
        public PD_PA_ShareKnowledge_GiveCard_ResearcherGives(
            PD_Player player,
            PD_Player otherPlayer,
            PD_CityCard cityCardToGive
            ) : base(
                player
                )
        {
            OtherPlayer = otherPlayer;
            CityCardToGive = cityCardToGive;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_ShareKnowledge_GiveCard_ResearcherGives(
            PD_PA_ShareKnowledge_GiveCard_ResearcherGives actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            OtherPlayer = actionToCopy.OtherPlayer.GetCustomDeepCopy();
            CityCardToGive = actionToCopy.CityCardToGive.GetCustomDeepCopy();
        }

        #endregion

        public override void Execute(PD_Game game)
        {
            game.Com_PA_ShareKnowledge_GiveCard_ResearcherGives(
                Player,
                OtherPlayer,
                CityCardToGive
                );
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_ShareKnowledge_GiveCard_ResearcherGives(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: SHARE_KNOWLEDGE_RESEARCHER | GIVE {1} to {2}",
                Player.Name,
                CityCardToGive.City.Name,
                OtherPlayer.Name
                );
        }
    }
}
