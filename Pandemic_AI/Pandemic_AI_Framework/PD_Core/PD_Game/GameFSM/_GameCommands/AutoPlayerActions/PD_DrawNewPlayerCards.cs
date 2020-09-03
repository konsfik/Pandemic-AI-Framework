﻿using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_DrawNewPlayerCards : PD_GameAction_Base, I_Auto_Action, I_Player_Action
    {
        public PD_Player Player { get; protected set; }

        [JsonConstructor]
        public PD_DrawNewPlayerCards(
            PD_Player player
            )
        {
            this.Player = player.GetCustomDeepCopy();
        }

        // private constructor, for custom deep copy purposes only
        private PD_DrawNewPlayerCards(
            PD_DrawNewPlayerCards actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_DrawNewPlayerCards(this);
        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            // draw new player cards...
            var newPlayerCards = new List<PD_PlayerCardBase>();

            for (int i = 0; i < 2; i++)
            {
                newPlayerCards.Add(game.Cards.DividedDeckOfPlayerCards.DrawLastElementOfLastSubList());
            }

            game.Cards.PlayerCardsPerPlayerID[Player.ID].AddRange(newPlayerCards);
        }

        public override string GetDescription()
        {
            return String.Format("{0}: DRAW Player Cards.", Player.Name);
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_DrawNewPlayerCards)otherObject;

            if (this.Player != other.Player)
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

            return hash;
        }

        #endregion
    }
}