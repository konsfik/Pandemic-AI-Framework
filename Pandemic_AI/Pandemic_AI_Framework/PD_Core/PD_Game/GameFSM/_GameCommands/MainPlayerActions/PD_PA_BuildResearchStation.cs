using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_BuildResearchStation :
        PD_GameAction_Base,
        IEquatable<PD_PA_BuildResearchStation>,
        I_Player_Action
    {
        public PD_Player Player { get; private set; }
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
            )
        {
            this.Player = player;
            this.Used_CityCard = used_CityCard;
            this.Build_RS_On = build_RS_On;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_BuildResearchStation(
            PD_PA_BuildResearchStation actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
            this.Used_CityCard = actionToCopy.Used_CityCard.GetCustomDeepCopy();
            this.Build_RS_On = actionToCopy.Build_RS_On.GetCustomDeepCopy();
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
                throw new System.Exception("wrong player...");
            }
            else if (Used_CityCard.City != game.GQ_CurrentPlayer_Location())
            {
                throw new System.Exception("city card does not match current player position");
            }
            else if (Build_RS_On != game.GQ_CurrentPlayer_Location())
            {
                throw new System.Exception("selected city does not match current player position");
            }
            else if (game.GQ_CurrentPlayer_Role() == PD_Player_Roles.Operations_Expert)
            {
                throw new System.Exception("wrong player role!");
            }
#endif
            game.GO_PlayerDiscardsPlayerCard(
                Player,
                Used_CityCard
                );

            game.GO_PlaceResearchStationOnCity(
                Build_RS_On
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
        public bool Equals(PD_PA_BuildResearchStation other)
        {
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

        public override bool Equals(PD_GameAction_Base other)
        {
            if (other is PD_PA_BuildResearchStation other_action)
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
            if (other is PD_PA_BuildResearchStation other_action)
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

            hash = hash * 31 + Player.GetHashCode();
            hash = hash * 31 + Used_CityCard.GetHashCode();
            hash = hash * 31 + Build_RS_On.GetHashCode();

            return hash;
        }


        #endregion
    }
}