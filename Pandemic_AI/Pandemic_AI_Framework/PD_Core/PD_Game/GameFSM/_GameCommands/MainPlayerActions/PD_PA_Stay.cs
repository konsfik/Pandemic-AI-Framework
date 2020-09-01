using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_Stay : PD_GameAction_Base, I_Player_Action
    {
        public PD_Player Player { get; private set; }
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
            )
        {
            Player = player;
            CityToStayOn = cityToStayOn;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_Stay(
            PD_PA_Stay actionToCopy
            )
        {
            Player = actionToCopy.Player.GetCustomDeepCopy();
            CityToStayOn = actionToCopy.CityToStayOn.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            // do nothing! :)
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

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_PA_Stay)otherObject;

            if (this.Player != other.Player)
            {
                return false;
            }
            if (this.CityToStayOn != other.CityToStayOn)
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
            hash = hash * 31 + CityToStayOn.GetHashCode();

            return hash;
        }

        #endregion
    }
}
