using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    /// <summary>
    /// A macro command is a series of commands (steps) that lead to the 
    /// fulfilment of a goal.
    /// </summary>
    public class PD_MacroAction : IDescribable, ICustomDeepCopyable<PD_MacroAction>
    {
        public List<PD_GameAction_Base> Actions_All { get; private set; }
        public List<PD_GameAction_Base> Actions_Executable_Now { get; private set; }
        public List<PD_GameAction_Base> Actions_Executable_Later { get; private set; }
        public PD_GameAction_Base NonExecutable_ShareKnowledge_Action { get; private set; }

        public PD_MacroAction_Type MacroAction_Type { get; private set; }
        public PD_MacroAction_WalkType MacroAction_WalkType { get; private set; }

        #region constructors
        /// <summary>
        /// normal constructor
        /// also used as a Json constructor.
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="macroAction_Type"></param>
        /// <param name="macroAction_WalkType"></param>
        [JsonConstructor]
        public PD_MacroAction(
            List<PD_GameAction_Base> actions_All,
            PD_MacroAction_Type macroAction_Type,
            PD_MacroAction_WalkType macroAction_WalkType,
            int current_NumAvailableActions
            )
        {
            // set the types, first
            MacroAction_Type = macroAction_Type;
            MacroAction_WalkType = macroAction_WalkType;

            Actions_All = new List<PD_GameAction_Base>();
            Actions_Executable_Now = new List<PD_GameAction_Base>();
            Actions_Executable_Later = new List<PD_GameAction_Base>();

            if (Is_TypeOf_TakePositionFor_ShareKnowledge_Any() == true)
            {
                Actions_All = actions_All.CustomDeepCopy();
                NonExecutable_ShareKnowledge_Action = Actions_All[Actions_All.Count - 1];
                Actions_All.Remove(NonExecutable_ShareKnowledge_Action);
            }
            else
            {
                Actions_All = actions_All.CustomDeepCopy();
            }

            if (Is_TypeOf_DuringMainPlayerActions())
            {
                for (int actionIndex = 0; actionIndex < Actions_All.Count; actionIndex++)
                {
                    if (actionIndex < current_NumAvailableActions)
                    {
                        Actions_Executable_Now.Add(Actions_All[actionIndex]);
                    }
                    else if (actionIndex >= current_NumAvailableActions)
                    {
                        Actions_Executable_Later.Add(Actions_All[actionIndex]);
                    }
                }
            }
            else if (Is_TypeOf_Discard_Any())
            {
                Actions_Executable_Now.AddRange(Actions_All);
            }

        }

        /// <summary>
        /// custom, private constructor, for custom deep copy purposes only
        /// </summary>
        /// <param name="macroActionToCopy"></param>
        private PD_MacroAction(
            PD_MacroAction macroActionToCopy
            )
        {
            this.Actions_All = macroActionToCopy.Actions_All.CustomDeepCopy();
            this.Actions_Executable_Now = macroActionToCopy.Actions_Executable_Now.CustomDeepCopy();
            this.Actions_Executable_Later = macroActionToCopy.Actions_Executable_Later.CustomDeepCopy();
            if (macroActionToCopy.NonExecutable_ShareKnowledge_Action != null)
            {
                this.NonExecutable_ShareKnowledge_Action = macroActionToCopy.NonExecutable_ShareKnowledge_Action.GetCustomDeepCopy();
            }

            this.MacroAction_Type = macroActionToCopy.MacroAction_Type;
            this.MacroAction_WalkType = macroActionToCopy.MacroAction_WalkType;
        }
        #endregion
        public bool Is_ExecutableNow()
        {
            return Actions_All.Count == Actions_Executable_Now.Count;
        }

        public bool Is_WalkTypeOf_SimpleWalk_OR_NoWalk()
        {
            return
                MacroAction_WalkType == PD_MacroAction_WalkType.None
                || MacroAction_WalkType == PD_MacroAction_WalkType.SimpleWalk;
        }

        public bool Is_WalkTypeOf_CardSpending_Any()
        {
            return
                MacroAction_WalkType == PD_MacroAction_WalkType.DirectFlightWalk
                || MacroAction_WalkType == PD_MacroAction_WalkType.CharterFlightWalk
                || MacroAction_WalkType == PD_MacroAction_WalkType.OperationsExpertFlightWalk;
        }

        public bool Is_TypeOf_TreatDisease_Any()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                || MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro
                || MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro;
        }

        public bool Is_TypeOf_BuildResearchStation_Any()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro
                || MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro;
        }

        public bool Is_TypeOf_BuildResearchStation_Any_Or_MoveResearchStation()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro
                || MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro
                || MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro;
        }

        public bool Is_TypeOf_Cure_Any()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Macro
                || MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Scientist_Macro;
        }



        public bool Is_TypeOf_DuringMainPlayerActions()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.Walk_Macro
                || MacroAction_Type == PD_MacroAction_Type.Stay_Macro

                || MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro
                || MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro
                || MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro

                || MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro
                || MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro
                || MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro

                || MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro
                || MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro
                || MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_Macro
                || MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro

                || MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro
                || MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro

                || MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Macro
                || MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Scientist_Macro;
        }

        public bool Is_TypeOf_Discard_Any()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.Discard_DuringMainPlayerActions_Macro
                || MacroAction_Type == PD_MacroAction_Type.Discard_AfterDrawing_Macro;
        }

        public bool Is_TypeOf_ShareKnowledge_Any()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro
                || MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro
                || MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_Macro
                || MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro;
        }

        public bool Is_TypeOf_TakePositionFor_ShareKnowledge_Any()
        {
            return
                MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro
                || MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro;
        }

        public PD_GameAction_Base Find_LastCommand()
        {
            return Actions_All.GetLast();
        }

        public PD_City Find_Destination()
        {
            if (Actions_All == null) return null;

            var lastCommand = Find_LastCommand();

            if (MacroAction_Type == PD_MacroAction_Type.Walk_Macro)
            {
                return ((I_Movement_Action)lastCommand).TargetLocation;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.Stay_Macro)
            {
                return ((PD_PA_Stay)lastCommand).CityToStayOn;
            }

            else if (MacroAction_Type == PD_MacroAction_Type.TreatDisease_Macro)
            {
                return ((PD_PA_TreatDisease)lastCommand).CityToTreatDiseaseAt;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.TreatDisease_Medic_Macro)
            {
                return ((PD_PA_TreatDisease_Medic)lastCommand).CityToTreatDiseaseAt;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.AutoTreatDisease_Medic_Macro)
            {
                return ((I_Movement_Action)lastCommand).TargetLocation;
            }

            else if (MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_Macro)
            {
                return ((PD_PA_BuildResearchStation)lastCommand).Build_RS_On;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.BuildResearchStation_OperationsExpert_Macro)
            {
                return ((PD_PA_BuildResearchStation_OperationsExpert)lastCommand).Build_RS_On;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.MoveResearchStation_Macro)
            {
                return ((PD_PA_MoveResearchStation)lastCommand).Move_RS_To;
            }

            else if (MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_Macro)
            {
                return ((PD_PA_ShareKnowledge_GiveCard)lastCommand).CityCardToGive.City;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Give_ResearcherGives_Macro)
            {
                return ((PD_PA_ShareKnowledge_GiveCard_ResearcherGives)lastCommand).CityCardToGive.City;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_Macro)
            {
                return ((PD_PA_ShareKnowledge_TakeCard)lastCommand).CityCardToTake.City;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.ShareKnowledge_Take_FromResearcher_Macro)
            {
                return ((PD_PA_ShareKnowledge_TakeCard_FromResearcher)lastCommand).CityCardToTake.City;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Give_Macro)
            {
                if (lastCommand.GetType() == typeof(PD_PA_Stay))
                {
                    return ((PD_PA_Stay)lastCommand).CityToStayOn;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_DriveFerry))
                {
                    return ((PD_PMA_DriveFerry)lastCommand).TargetLocation;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_ShuttleFlight))
                {
                    return ((PD_PMA_ShuttleFlight)lastCommand).TargetLocation;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_DirectFlight))
                {
                    return ((PD_PMA_DirectFlight)lastCommand).TargetLocation;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_OperationsExpert_Flight))
                {
                    return ((PD_PMA_OperationsExpert_Flight)lastCommand).TargetLocation;
                }
                else
                {
                    return null;
                }
            }
            else if (MacroAction_Type == PD_MacroAction_Type.TakePositionFor_ShareKnowledge_Take_Macro)
            {
                if (lastCommand.GetType() == typeof(PD_PA_Stay))
                {
                    return ((PD_PA_Stay)lastCommand).CityToStayOn;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_DriveFerry))
                {
                    return ((PD_PMA_DriveFerry)lastCommand).TargetLocation;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_ShuttleFlight))
                {
                    return ((PD_PMA_ShuttleFlight)lastCommand).TargetLocation;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_DirectFlight))
                {
                    return ((PD_PMA_DirectFlight)lastCommand).TargetLocation;
                }
                else if (lastCommand.GetType() == typeof(PD_PMA_OperationsExpert_Flight))
                {
                    return ((PD_PMA_OperationsExpert_Flight)lastCommand).TargetLocation;
                }
                else
                {
                    return null;
                }
            }

            else if (MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Macro)
            {
                return ((PD_PA_DiscoverCure)lastCommand).CityOfResearchStation;
            }
            else if (MacroAction_Type == PD_MacroAction_Type.DiscoverCure_Scientist_Macro)
            {
                return ((PD_PA_DiscoverCure_Scientist)lastCommand).CityOfResearchStation;
            }
            else
            {
                return null;
            }
        }

        public List<PD_CityCard> Find_CityCardsUsedForWalking()
        {
            return PD_Macro_Utilities.Find_CityCardsUsedForWalking_In_ActionList(Actions_All);
        }

        public int Count_Walk_Length()
        {
            return Actions_All.FindAll(
                x =>
                x is I_Movement_Action
                &&
                x.GetType() != typeof(PD_PA_Stay)
                ).Count;
        }

        public int Count_Total_Length()
        {
            if (Actions_All == null) return 0;
            if (Actions_All.Count == 0) return 0;
            return Actions_All.Count;
        }

        public int Count_ExecutableNow_Length()
        {
            if (Actions_Executable_Now == null) return 0;
            if (Actions_Executable_Now.Count == 0) return 0;
            return Actions_Executable_Now.Count;
        }

        public PD_MacroAction GetCustomDeepCopy()
        {
            return new PD_MacroAction(this);
        }

        public string GetDescription()
        {
            string description = String.Format(
                "{0}: {1} - TML:{2} | {3} - WL:{4} | {5}",
                ((I_Player_Action)Actions_All[0]).Player.Name,
                MacroAction_Type.ToString(),
                Count_Total_Length(),
                MacroAction_WalkType.ToString(),
                Count_Walk_Length(),
                Find_Destination() == null ? "none" : Find_Destination().Name
                );

            return description;
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            PD_MacroAction other = (PD_MacroAction)otherObject;

            if (this.Count_Total_Length() != other.Count_Total_Length())
            {
                return false;
            }
            for (int i = 0; i < Actions_All.Count; i++)
            {
                if (this.Actions_All[i] != other.Actions_All[i])
                {
                    return false;
                }
            }

            if (this.Actions_Executable_Now.Count != other.Actions_Executable_Now.Count)
            {
                return false;
            }
            for (int i = 0; i < Actions_Executable_Now.Count; i++)
            {
                if (this.Actions_Executable_Now[i] != other.Actions_Executable_Now[i])
                {
                    return false;
                }
            }

            if (this.Actions_Executable_Later.Count != other.Actions_Executable_Later.Count)
            {
                return false;
            }
            if (this.Actions_Executable_Later.Count > 0)
            {
                for (int i = 0; i < Actions_Executable_Later.Count; i++)
                {
                    if (this.Actions_Executable_Later[i] != other.Actions_Executable_Later[i])
                    {
                        return false;
                    }
                }

            }
            if (this.NonExecutable_ShareKnowledge_Action != other.NonExecutable_ShareKnowledge_Action)
            {
                return false;
            }

            // compare the rest of the properties
            if (
                this.MacroAction_Type == other.MacroAction_Type
                && this.MacroAction_WalkType == other.MacroAction_WalkType
                )
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var action in Actions_All)
            {
                hash = hash * 31 + action.GetHashCode();
            }
            foreach (var action in Actions_Executable_Now)
            {
                hash = hash * 31 + action.GetHashCode();
            }
            foreach (var action in Actions_Executable_Later)
            {
                hash = hash * 31 + action.GetHashCode();
            }
            hash = hash * 31 + MacroAction_Type.GetHashCode();
            hash = hash * 31 + MacroAction_WalkType.GetHashCode();

            return hash;
        }

        public static bool operator ==(PD_MacroAction c1, PD_MacroAction c2)
        {
            if (Object.ReferenceEquals(c1, null))
            {
                if (Object.ReferenceEquals(c2, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // c1 is not null
            {
                if (Object.ReferenceEquals(c2, null)) // c2 is null
                {
                    return false;
                }
            }
            return c1.Equals(c2);
        }

        public static bool operator !=(PD_MacroAction c1, PD_MacroAction c2)
        {
            return !(c1 == c2);
        }
        #endregion
    }

    public enum PD_MacroAction_WalkType
    {
        None,
        SimpleWalk,
        CharterFlightWalk,
        DirectFlightWalk,
        OperationsExpertFlightWalk
    }

    public enum PD_MacroAction_Type
    {
        None,

        Walk_Macro,
        Stay_Macro,

        TreatDisease_Macro,
        TreatDisease_Medic_Macro,
        AutoTreatDisease_Medic_Macro,

        BuildResearchStation_Macro,
        BuildResearchStation_OperationsExpert_Macro,
        MoveResearchStation_Macro,

        ShareKnowledge_Give_Macro,
        ShareKnowledge_Take_Macro,
        ShareKnowledge_Give_ResearcherGives_Macro,
        ShareKnowledge_Take_FromResearcher_Macro,

        TakePositionFor_ShareKnowledge_Give_Macro,
        TakePositionFor_ShareKnowledge_Take_Macro,

        DiscoverCure_Macro,
        DiscoverCure_Scientist_Macro,

        Discard_DuringMainPlayerActions_Macro,
        Discard_AfterDrawing_Macro
    }
}
