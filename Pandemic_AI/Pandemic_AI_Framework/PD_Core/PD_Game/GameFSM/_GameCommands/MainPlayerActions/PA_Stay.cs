using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PA_Stay :
        PD_GameAction_Base,
        IEquatable<PA_Stay>,
        I_Player_Action
    {
        public int Player { get; private set; }
        public int CityToStayOn { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json Constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cityToStayOn"></param>
        [JsonConstructor]
        public PA_Stay(
            int player,
            int cityToStayOn
            )
        {
            Player = player; ;
            CityToStayOn = cityToStayOn;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PA_Stay(
            PA_Stay actionToCopy
            )
        {
            Player = actionToCopy.Player;
            CityToStayOn = actionToCopy.CityToStayOn;
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
#endif
            // do nothing! :)
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PA_Stay(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: STAY on {1}",
                Player.ToString(),
                CityToStayOn.ToString()
                );
        }

        #region equality overrides
        public bool Equals(PA_Stay other)
        {
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

        public override bool Equals(PD_GameAction_Base other)
        {
            if (other is PA_Stay other_action)
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
            if (other is PA_Stay other_action)
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

            hash = hash * 31 + Player;
            hash = hash * 31 + CityToStayOn;

            return hash;
        }



        #endregion
    }
}
