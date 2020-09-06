using System.Collections;
using System.Collections.Generic;
using System;

namespace Pandemic_AI_Framework
{
    [Serializable]
    public class PD_Color : ICustomDeepCopyable<PD_Color>
    {
        private float _r;
        private float _g;
        private float _b;
        private float _a;

        public float R
        {
            get { return _r; }
            set { _r = value; }
        }
        public float G
        {
            get { return _g; }
            set { _g = value; }
        }
        public float B
        {
            get { return _b; }
            set { _b = value; }
        }
        public float A
        {
            get { return _a; }
            set { _a = value; }
        }

        public PD_Color(float r, float g, float b, float a)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;

            // limit values between zero and one... 
            if (_r < 0.0f) _r = 0.0f;
            if (_g < 0.0f) _g = 0.0f;
            if (_b < 0.0f) _b = 0.0f;
            if (_a < 0.0f) _a = 0.0f;
            if (_r > 1.0f) _r = 1.0f;
            if (_g > 1.0f) _g = 1.0f;
            if (_b > 1.0f) _b = 1.0f;
            if (_a > 1.0f) _a = 1.0f;
        }

        public PD_Color(float r, float g, float b) : this(r, g, b, 1.0f) { }

        public PD_Color(float grey) : this(grey, grey, grey, 1.0f) { }

        public PD_Color(float grey, float a) : this(grey, grey, grey, a) { }

        public PD_Color GetCustomDeepCopy()
        {
            return new PD_Color(
                this.R,
                this.G,
                this.B,
                this.A
                );
        }

        #region equalityOverride
        public bool Equals(PD_Color other)
        {
            if (other.R != this.R)
            {
                return false;
            }
            if (other.G != this.G)
            {
                return false;
            }
            if (other.B != this.B)
            {
                return false;
            }
            if (other.A != this.A)
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
            if (otherObject is PD_Color other_game_fsm)
            {
                return Equals(other_game_fsm);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 13) + this.R.GetHashCode();
            hash = (hash * 13) + this.G.GetHashCode();
            hash = (hash * 13) + this.B.GetHashCode();
            hash = (hash * 13) + this.A.GetHashCode();

            return hash;
        }



        public static bool operator ==(PD_Color s1, PD_Color s2)
        {
            if (Object.ReferenceEquals(s1, null))
            {
                if (Object.ReferenceEquals(s2, null))
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
                if (Object.ReferenceEquals(s2, null)) // c2 is null
                {
                    return false;
                }
            }
            // c1 is not null && c2 is not null
            // -> actually check equality
            return s1.Equals(s2);
        }

        public static bool operator !=(PD_Color s1, PD_Color s2)
        {
            return !(s1 == s2);
        }
        #endregion
    }
}
