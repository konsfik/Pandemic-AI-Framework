﻿using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_GS_ApplyingInfectionCards : PD_GameStateBase, ICustomDeepCopyable<PD_GS_ApplyingInfectionCards>
    {

        public PD_GS_ApplyingInfectionCards()
        {

        }

        public PD_GS_ApplyingInfectionCards GetCustomDeepCopy()
        {
            return new PD_GS_ApplyingInfectionCards();
        }

        public override PD_GameStateBase OnCommand(
            PD_Game game,
            PD_GameAction_Base command
            )
        {
            if (command.GetType() == typeof(PD_ApplyInfectionCard))
            {
                command.Execute(game);

                bool thereAreMoreActiveInfectionCards = PD_Game_Queries.GQ_SS_ThereAreActiveInfectionCards(game);

                // check if game is lost, etc..
                if (PD_Game_Queries.GQ_SS_NotEnoughDiseaseCubestoCompleteAnInfection(game))
                    return new PD_GS_GameLost();
                if (PD_Game_Queries.GQ_SS_DeadlyOutbreaks(game))
                    return new PD_GS_GameLost();
                if (PD_Game_Queries.GQ_SS_ThereAreActiveInfectionCards(game))
                    return null;

                game.GameStateCounter.IncreaseCurrentPlayerIndex();
                return new PD_GS_ApplyingMainPlayerActions();
            }
            return null;
        }

        public override void OnEnter(PD_Game game)
        {

        }

        public override void OnExit(PD_Game game)
        {

        }
    }
}