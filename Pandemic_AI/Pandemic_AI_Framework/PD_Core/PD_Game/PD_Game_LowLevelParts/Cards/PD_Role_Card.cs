using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework
{
    public class PD_Role_Card :
        PD_Card_Base,
        IEquatable<PD_Role_Card>,
        ICustomDeepCopyable<PD_Role_Card>
    {
        public int Role { get; private set; }

        public PD_Role_Card(
            int id,
            int role
            ) : base(
                id
                )
        {
            Role = role;
        }

        private PD_Role_Card(
            PD_Role_Card roleCardToCopy
            ) : base(
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
        public bool Equals(PD_Role_Card other)
        {
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
        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_Role_Card other_role_card)
            {
                return Equals(other_role_card);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 7;

            hash = (hash * 13) + ID;
            hash = (hash * 13) + Role;

            return hash;
        }

        #endregion
        public override string ToString()
        {
            return String.Format("Role Card {0}:{1}", ID, Role.ToString());
        }
    }

}
