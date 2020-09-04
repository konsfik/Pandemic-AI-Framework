﻿using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_ShareKnowledge_TakeCard :
        PD_GameAction_Base,
        IEquatable<PD_PA_ShareKnowledge_TakeCard>,
        I_Player_Action
    {
        public PD_Player Player { get; private set; }
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
        private PD_PA_ShareKnowledge_TakeCard(
            PD_PA_ShareKnowledge_TakeCard actionToCopy
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
#if DEBUG
            if (game.GQ_IsInState_ApplyingMainPlayerActions() == false)
            {
                throw new System.Exception("wrong state!");
            }
            else if (Player != game.GQ_CurrentPlayer())
            {
                throw new System.Exception("wrong player!");
            }
            else if (game.GQ_Find_Player_Role(OtherPlayer) == PD_Player_Roles.Researcher)
            {
                throw new System.Exception("the other player must not be researcher!");
            }
            else if (game.GQ_PlayerLocation(Player) != game.GQ_PlayerLocation(OtherPlayer))
            {
                throw new System.Exception("Players do not share the same location");
            }
            else if (game.GQ_PlayerLocation(Player) != CityCardToTake.City)
            {
                throw new System.Exception("Player is not on the correct city");
            }
#endif
            game.Cards.PlayerCardsPerPlayerID[OtherPlayer.ID].Remove(CityCardToTake);
            game.Cards.PlayerCardsPerPlayerID[game.GQ_CurrentPlayer().ID].Add(CityCardToTake);
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

        #region equality overrides
        public bool Equals(PD_PA_ShareKnowledge_TakeCard other)
        {
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

        public override bool Equals(PD_GameAction_Base other)
        {
            if (other is PD_PA_ShareKnowledge_TakeCard other_action)
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
            if (other is PD_PA_ShareKnowledge_TakeCard other_action)
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
            hash = hash * 31 + OtherPlayer.GetHashCode();
            hash = hash * 31 + CityCardToTake.GetHashCode();

            return hash;
        }



        #endregion
    }
}