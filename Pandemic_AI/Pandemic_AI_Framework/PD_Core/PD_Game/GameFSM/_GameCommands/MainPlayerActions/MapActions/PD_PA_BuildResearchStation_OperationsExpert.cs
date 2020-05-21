using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_PA_BuildResearchStation_OperationsExpert : PD_MapAction_Base
    {
        public PD_City Build_RS_On { get; private set; }

        #region constructors
        /// <summary>
        /// Normal & Json constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="used_CityCard"></param>
        /// <param name="build_RS_On"></param>
        [JsonConstructor]
        public PD_PA_BuildResearchStation_OperationsExpert(
            PD_Player player,
            PD_City build_RS_On
            ) : base(
                player
                )
        {
            Build_RS_On = build_RS_On;
        }

        /// <summary>
        /// private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="actionToCopy"></param>
        private PD_PA_BuildResearchStation_OperationsExpert(
            PD_PA_BuildResearchStation_OperationsExpert actionToCopy
            ) : base(
                actionToCopy.Player.GetCustomDeepCopy()
                )
        {
            Build_RS_On = actionToCopy.Build_RS_On.GetCustomDeepCopy();
        }
        #endregion

        public override void Execute(PD_Game game)
        {
            game.Com_PA_BuildResearchStation_OperationsExpert(
                Player,
                Build_RS_On
                );
        }

        public override PD_GameAction_Base GetCustomDeepCopy()
        {
            return new PD_PA_BuildResearchStation_OperationsExpert(this);
        }

        public override string GetDescription()
        {
            return String.Format(
                "{0}: BUILD_RS_OPERATIONS_EXPERT on {1}",
                Player.Name,
                Build_RS_On.Name
                );
        }
    }
}