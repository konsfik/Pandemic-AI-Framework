using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_MoveResearchStation : PD_MapAction_Base
    {
        public PD_CityCard Used_CityCard { get; private set; }
        public PD_City Move_RS_From { get; private set; }
        public PD_City Move_RS_To { get; private set; }
        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="used_CityCard"></param>
        /// <param name="move_RS_From"></param>
        /// <param name="move_RS_To"></param>
        [JsonConstructor]
        public PD_PA_MoveResearchStation(
            PD_Player player,
            PD_CityCard used_CityCard,
            PD_City move_RS_From,
            PD_City move_RS_To
            ) : base(player)
        {
            if (used_CityCard.City != move_RS_To) {
                throw new System.Exception("used card does not match research station city");
            }
            Used_CityCard = used_CityCard;
            Move_RS_From = move_RS_From;
            Move_RS_To = move_RS_To;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_MoveResearchStation(
            PD_PA_MoveResearchStation actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            Used_CityCard = actionToCopy.Used_CityCard.GetCustomDeepCopy();
            Move_RS_From = actionToCopy.Move_RS_From.GetCustomDeepCopy();
            Move_RS_To = actionToCopy.Move_RS_To.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PA_MoveResearchStation(Player, Move_RS_From, Move_RS_To);
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_MoveResearchStation(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: MOVE_RESEARCH_STATION from {1} to {2}",
                Player.Name,
                Move_RS_From.Name,
                Move_RS_To.Name
                );
        }
    }
}