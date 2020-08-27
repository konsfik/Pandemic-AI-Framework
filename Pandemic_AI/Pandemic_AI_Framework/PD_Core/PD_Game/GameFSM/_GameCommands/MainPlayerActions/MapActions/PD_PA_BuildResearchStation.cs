using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_BuildResearchStation : PD_MapAction_Base
    {
        public PD_CityCard Used_CityCard { get; private set; }
        public PD_City Build_RS_On { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="used_CityCard"></param>
        /// <param name="build_RS_On"></param>
        [JsonConstructor]
        public PD_PA_BuildResearchStation(
            PD_Player player,
            PD_CityCard used_CityCard,
            PD_City build_RS_On
            ) : base(
                player
                )
        {
            if (used_CityCard.City != build_RS_On)
            {
                throw new System.Exception("City card to use does not match the city to build on.");
            }
            Used_CityCard = used_CityCard;
            Build_RS_On = build_RS_On;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_BuildResearchStation(
            PD_PA_BuildResearchStation actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            Used_CityCard = actionToCopy.Used_CityCard.GetCustomDeepCopy();
            Build_RS_On = actionToCopy.Build_RS_On.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider, 
            PD_Game game
            )
        {
            game.Com_PA_BuildResearchStation(
                Player,
                Build_RS_On,
                Used_CityCard
                );
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_BuildResearchStation(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: BUILD_RS on {1}",
                Player.Name,
                Build_RS_On.Name
                );
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PA_BuildResearchStation)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            if (this.Used_CityCard != other.Used_CityCard)
            {
                return false;
            }
            if (this.Build_RS_On != other.Build_RS_On)
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
            hash = hash * 31 + Used_CityCard.GetHashCode();
            hash = hash * 31 + Build_RS_On.GetHashCode();

            return hash;
        }

        #endregion
    }
}