using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GA_SetupGame_Random :
        PD_Action,
        IEquatable<PD_GA_SetupGame_Random>
    {
        public PD_GA_SetupGame_Random()
        {

        }

        public override void Execute(
            Random randomness_provider,
            PD_Game game
            )
        {
            game.Com_SetupGame_Random(randomness_provider);
        }

        public override PD_Action GetCustomDeepCopy()
        {
            return new PD_GA_SetupGame_Random();
        }

        public override string GetDescription()
        {
            string description = "Game Setup Begins";
            return description;
        }

        #region equality overrides
        public bool Equals(PD_GA_SetupGame_Random other)
        {
            return true;
        }

        public override bool Equals(PD_Action other)
        {
            if (other is PD_GA_SetupGame_Random other_action)
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
            if (other is PD_GA_SetupGame_Random other_action)
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
            return 0;
        }



        #endregion
    }
}