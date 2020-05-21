using System.Collections;
using System.Collections.Generic;
using System;


namespace Pandemic_AI_Framework
{
    [Serializable]
    public abstract class PD_MapMovementAction_Base : PD_MainAction_Base
    {
        public PD_City InitialLocation { get; private set; }
        public PD_City TargetLocation { get; private set; }

        public PD_MapMovementAction_Base(
            PD_Player player,
            PD_City initialLocation,
            PD_City targetLocation
            ) : base(
                player
                )
        {
            if (initialLocation == targetLocation)
            {
                throw new System.Exception("initial location cannot be same as target location in a movement action");
            }
            InitialLocation = initialLocation;
            TargetLocation = targetLocation;
        }
    }
}
