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
    }
}
