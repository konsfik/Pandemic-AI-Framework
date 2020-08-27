using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_ME_InfectionCube : PD_MapElement_Base, IDescribable, ICustomDeepCopyable<PD_ME_InfectionCube>
    {
         private readonly int _type;

        public int Type { get { return _type; } }

        public PD_ME_InfectionCube(int id, int type) : base(id)
        {
            _type = type;
        }

        public string GetDescription()
        {
            return String.Format("Infection Cube - ID: {0}, Type: {1}", ID, Type);
        }

        public PD_ME_InfectionCube GetCustomDeepCopy()
        {
            return new PD_ME_InfectionCube(
                this.ID, 
                this.Type
                );
        }

        #region equality overrides
        public override bool Equals(object otherObject)
        {
            if (this.GetType() != otherObject.GetType())
            {
                return false;
            }

            var other = (PD_ME_InfectionCube)otherObject;

            if (this.ID != other.ID)
            {
                return false;
            }
            else if (this.Type != other.Type)
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

            hash = hash * 31 + ID.GetHashCode();
            hash = hash * 31 + Type.GetHashCode();

            return hash;
        }

        #endregion
    }
}
