using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_Stay : PD_MapAction_Base
    {
        public PD_City CityToStayOn { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json Constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cityToStayOn"></param>
        [JsonConstructor]
        public PD_PA_Stay(
            PD_Player player,
            PD_City cityToStayOn
            ) : base(
                player
                )
        {
            CityToStayOn = cityToStayOn;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_Stay(
            PD_PA_Stay actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            CityToStayOn = actionToCopy.CityToStayOn.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_PA_Stay();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_Stay(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: STAY on {1}",
                Player.Name,
                CityToStayOn.Name
                );
        }
    }
}
