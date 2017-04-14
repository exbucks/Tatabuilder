using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    // http://msdn.microsoft.com/en-us/library/ee308751(v=vs.110).aspx

    public class TEasingFunction
    {
        #region constants
        // Const values come from sdk\inc\crt\float.h
        internal const double DBL_EPSILON = 2.2204460492503131e-016; /* smallest such that 1.0+DBL_EPSILON != 1.0 */
        internal const float FLT_MIN = 1.175494351e-38F; /* Number close to zero, where float.MinValue is -float.MaxValue */
        #endregion

        public enum EasingType { None, Exponential, Sine, Elastic, Bounce, Back };
        public enum EasingMode { In, Out, InOut };

        // EaseExponential Properties
        public double exponent { get; set; }

        // EaseBounce Properties
        public int bounces { get; set; }
        public double bounciness { get; set; }

        // EaseElastic Properties
        public int oscillations { get; set; }
        public double springiness { get; set; }

        // EaseBack Properties
        public double amplitude { get; set; }

        public TEasingFunction()
        {
            // EaseExponential
            exponent = 10;

            // EaseBounce
            bounces = 4;
            bounciness = 3;

            // EaseElastic
            oscillations = 3;
            springiness = 3.0;

            // EaseBack
            amplitude = 0.5;
        }

        public float ease(EasingType type, EasingMode mode, float duration, float time, float startVal, float endVal)
        {
            float normalizedTime = duration > 0 ? time / duration : 1;
            float deltaVal = endVal - startVal;
            return (float)(startVal + deltaVal * ease(type, mode, normalizedTime));
        }

        // Transforms normalized time to control the pace of an animation.
        private double ease(EasingType type, EasingMode mode, double normalizedTime)
        {
            switch (mode) {
                case EasingMode.In:
                    return ease(type, normalizedTime);
                case EasingMode.Out:
                    // EaseOut is the same as EaseIn, except time is reversed & the result is flipped.
                    return 1f - ease(type, 1f - normalizedTime);
                case EasingMode.InOut:
                default:
                    // EaseInOut is a combination of EaseIn & EaseOut fit to the 0-1, 0-1 range.
                    if (normalizedTime < 0.5f)
                        return ease(type, normalizedTime * 2f) * 0.5f;
                    else
                        return (1f - ease(type, (1f - normalizedTime) * 2f)) * 0.5f + 0.5f;
            }
        }

        // Calculate easing function case when EasingMode.In
        private double ease(EasingType type, double normalizedTime)
        {
            switch (type) {
                case EasingType.Exponential:
                    return easeExponential(normalizedTime);
                case EasingType.Sine:
                    return easeSine(normalizedTime);
                case EasingType.Elastic:
                    return easeElastic(normalizedTime);
                case EasingType.Bounce:
                    return easeBounce(normalizedTime);
                case EasingType.Back:
                    return easeBack(normalizedTime);
                case EasingType.None:
                default:
                    return easeLinear(normalizedTime);
            }
        }

        public double easeLinear(double t)
        {
            return t;
        }

        public double easeExponential(double t)
        {
            return (Math.Exp(exponent * t) - 1) / (Math.Exp(exponent) - 1);
        }

        public double easeSine(double t)
        {
            return 1 - Math.Sin((1 - t) * Math.PI / 2);
        }

        public double easeElastic(double t)
        {
            double expo;
            if (Math.Abs(springiness) < 10.0 * DBL_EPSILON) {
                expo = t;
            } else {
                expo = (Math.Exp(springiness * t) - 1.0) / (Math.Exp(springiness) - 1.0);
            }

            return expo * (Math.Sin((Math.PI * 2.0 * oscillations + Math.PI * 0.5) * t));
        }

        public double easeBounce(double t)
        {
            // Clamp the bounciness so we dont hit a divide by zero
            if (bounciness < 1.0 || Math.Abs(bounciness - 1.0) < 10.0 * DBL_EPSILON) {
                // Make it just over one.  In practice, this will look like 1.0 but avoid divide by zeros.
                bounciness = 1.001;
            }

            double pow = Math.Pow(bounciness, bounces);
            double oneMinusBounciness = 1.0 - bounciness;

            // 'unit' space calculations.
            // Our bounces grow in the x axis exponentially.  we define the first bounce as having a 'unit' width of 1.0 and compute
            // the total number of 'units' using a geometric series.
            // We then compute which 'unit' the current time is in.
            double sumOfUnits = (1.0 - pow) / oneMinusBounciness + pow * 0.5; // geometric series with only half the last sum
            double unitAtT = t * sumOfUnits;

            // 'bounce' space calculations.
            // Now that we know which 'unit' the current time is in, we can determine which bounce we're in by solving the geometric equation:
            // unitAtT = (1 - bounciness^bounce) / (1 - bounciness), for bounce.
            double bounceAtT = Math.Log(-unitAtT * (1.0 - bounciness) + 1.0, bounciness);
            double start = Math.Floor(bounceAtT);
            double end = start + 1.0;

            // 'time' space calculations.
            // We then project the start and end of the bounce into 'time' space
            double startTime = (1.0 - Math.Pow(bounciness, start)) / (oneMinusBounciness * sumOfUnits);
            double endTime = (1.0 - Math.Pow(bounciness, end)) / (oneMinusBounciness * sumOfUnits);

            // Curve fitting for bounce.
            double midTime = (startTime + endTime) * 0.5;
            double timeRelativeToPeak = t - midTime;
            double radius = midTime - startTime;
            double amplitude = Math.Pow(1.0 / bounciness, (bounces - start));

            // Evaluate a quadratic that hits (startTime,0), (endTime, 0), and peaks at amplitude.
            return (-amplitude / (radius * radius)) * (timeRelativeToPeak - radius) * (timeRelativeToPeak + radius);
        }

        public double easeBack(double t)
        {
            return Math.Pow(t, 3) - t * amplitude * Math.Sin(t * Math.PI);
        }

        public double easeCircle(double t)
        {
            return 1 - Math.Sqrt(1 - t * t);
        }

        public double easeQuadratic(double t)
        {
            return Math.Pow(t, 2);
        }

        public double easeCubic(double t)
        {
            return Math.Pow(t, 3);
        }

        public double easeQuartic(double t)
        {
            return Math.Pow(t, 4);
        }

        public double easeQuintic(double t)
        {
            return Math.Pow(t, 5);
        }
    }
}
