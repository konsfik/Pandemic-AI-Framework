using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_Player :
        IEquatable<PD_Player>,
        ICustomDeepCopyable<PD_Player>
    {
        #region properties
        public int ID { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region constructors
        /// <summary>
        /// public constructor, also used for json deserialization...
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        [JsonConstructor]
        public PD_Player(
            int id,
            string name
            )
        {
            ID = id;
            Name = name;
        }

        /// <summary>
        /// private constructor, used for deep copy purposes, only
        /// </summary>
        /// <param name="player_to_copy"></param>
        private PD_Player(PD_Player player_to_copy)
        {
            this.ID = player_to_copy.ID;
            this.Name = player_to_copy.Name;
        }

        public PD_Player GetCustomDeepCopy()
        {
            return new PD_Player(this);
        }
        #endregion

        #region equality overrides
        public bool Equals(PD_Player other)
        {
            if (
                this.ID == other.ID &&
                this.Name == other.Name
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object otherObject)
        {
            if (otherObject is PD_Player other_player)
            {
                return Equals(other_player);
            }
            else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + ID.GetHashCode();
            hash = hash * 31 + Name.GetHashCode();
            return hash;
        }



        public static bool operator ==(PD_Player p1, PD_Player p2)
        {
            if (object.ReferenceEquals(p1, p2))
            {
                return true;
            }
            if (object.ReferenceEquals(p1, null))
            {
                if (object.ReferenceEquals(p2, null))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (object.ReferenceEquals(p2, null))
                {
                    return false;
                }
            }
            return p1.Equals(p2);
        }

        public static bool operator !=(PD_Player p1, PD_Player p2)
        {
            return !(p1 == p2);
        }
        #endregion

        public override string ToString()
        {
            return ID.ToString() + "." + Name;
        }
    }
}