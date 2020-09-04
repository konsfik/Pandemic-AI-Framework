using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_DrawNewInfectionCards : PD_GameAction_Base, I_Auto_Action, I_Player_Action
    {
        public PD_Player Player { get; protected set; }

        [JsonConstructor]
        public PD_DrawNewInfectionCards(
            PD_Player player
            )
        {
            this.Player = player;
        }

        // private constructor, for custom deep copy purposes only
        private PD_DrawNewInfectionCards(
            PD_DrawNewInfectionCards actionToCopy
            )
        {
            this.Player = actionToCopy.Player.GetCustomDeepCopy();
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_DrawNewInfectionCards(this);
        }

        public override void Execute(
            Random randomness_provider, 
            PD_Game game
            )
        {
#if DEBUG
            if (Player != game.GQ_CurrentPlayer())
            {
                throw new System.Exception("wrong player!");
            }
            else if ((game.GameFSM.CurrentState is PD_GS_DrawingNewInfectionCards) == false)
            {
                throw new System.Exception("wrong state!");
            }
#endif
            int numberOfInfectionCardsToDraw =
                game.GameSettings.InfectionRatesPerEpidemicsCounter[
                    game.GameStateCounter.EpidemicsCounter];

            var infectionCards = new List<PD_InfectionCard>();

            for (int i = 0; i < numberOfInfectionCardsToDraw; i++)
            {
                infectionCards.Add(
                    game.Cards.DividedDeckOfInfectionCards.DrawLastElementOfLastSubList());
            }

            game.Cards.ActiveInfectionCards.AddRange(infectionCards);
        }

        public override string GetDescription()
        {
            return String.Format("{0}: DRAW Infection cards", Player.Name);
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_DrawNewInfectionCards)otherObject;

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