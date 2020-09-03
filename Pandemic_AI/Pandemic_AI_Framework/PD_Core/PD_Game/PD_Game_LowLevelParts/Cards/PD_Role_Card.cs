using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_Role_Card : PD_Card_Base, ICustomDeepCopyable<PD_Role_Card>
    {
        public PD_Player_Roles Role { get; private set; }

        public PD_Role_Card(
            int id,
            PD_Player_Roles role
            ) : base(
                id
                )
        {
            Role = role;
        }

        private PD_Role_Card(
            PD_Role_Card roleCardToCopy
            ):base(
            roleCardToCopy.ID
            )
        {
            this.Role = roleCardToCopy.Role;
        }

        public override string GetDescription()
        {
            return Role.ToString();
        }

        public PD_Role_Card GetCustomDeepCopy()
        {
            return new PD_Role_Card(this);
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (otherObject.GetType() != this.GetType())
            {
                return false;
            }

            var other = (PD_Role_Card)otherObject;

            if (this.ID != other.ID)
            {
                return false;
            }
            else if (this.Role != other.Role)
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
            int hash = 7;

            hash = (hash * 13) + ID;
            hash = (hash * 13) + Role.GetHashCode();

            return hash;
        }

        #endregion
        public override string ToString()
        {
            return String.Format("Role Card {0}:{1}",ID, Role.ToString());
        }
    }

    // certain player roles are omitted...
    public enum PD_Player_Roles {
        None,
        //Contingency_Planner, // this one is removed, because it depends on event cards which are omitted
        Operations_Expert,
        //Dispatcher,
        //Quarantine_Specialist,
        Researcher,
        Medic,
        Scientist
    }
}
