using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using System.Diagnostics;
namespace idx
{
    static public class numerics
    {
        public const double TWOPI = 6.283185308;
        public const double PI = 3.141592654;
        public const double PI_OVER2 = 1.570796327;
        public const double PR = ((double)0.66666666);

        public const double PO = ((double)1.71593428);
        public const double A0 = ((double)(1.0));
        public const double A1 = ((double)(0.125 * PR));
        public const double A2 = ((double)(0.0078125 * PR * PR));
        public const double A3 = ((double)(0.000325520833333 * PR * PR * PR));

        private static MersenneTwister gen = new MersenneTwister();

        static public double dtanh(double x)
        {
            double e = Math.Exp(-2 * (double)(x));
            double e1 = 1 + e;
            e1 = e1 * e1;
            if (Double.IsInfinity(e1)) return 0.0;
            return ((4 * e) / e1);
        }

        static public double arccot(double x)
        {
            if (x == 0)
                return PI_OVER2;
            if (x > 0)
                return Math.Atan(1 / x);
            return PI + Math.Atan(1 / x);
        }

        static public double stdsigmoid(double x)
        {
            double y;

            if (x >= 0.0)
                if (x < (double)13)
                    y = A0 + x * (A1 + x * (A2 + x * (A3)));
                else
                    return PO;
            else
                if (x > -(double)13)
                    y = A0 - x * (A1 - x * (A2 - x * (A3)));
                else
                    return -PO;
            y *= y;
            y *= y;
            y *= y;
            y *= y;
            return (x > 0.0) ? PO * (y - 1.0) / (y + 1.0) : PO * (1.0 - y) / (y + 1.0);
        }

        public static double dstdsigmoid(double x)
        {
            if (x < 0.0)
                x = -x;
            if (x < (double)13)
            {
                double y;
                y = A0 + x * (A1 + x * (A2 + x * (A3)));
                y *= y;
                y *= y;
                y *= y;
                y *= y;
                y = (y - 1.0) / (y + 1.0);
                return PR * PO - PR * PO * y * y;
            }
            else
                return 0.0;
        }

        public static double gaussian(double x, double m, double sigma)
        {
            sigma *= sigma * 2;
            x -= m;
            return Math.Exp(-x * x / sigma) / Math.Sqrt(PI * sigma);
        }

        public static double angle_distance(double a1, double a2)
        {
            double d = a1 - a2;
            double fd = Math.Abs(d);
            if (fd < TWOPI - fd) return d;
            else
            {
                if (d < 0) return (TWOPI - fd);
                else return (fd - TWOPI);
            }
        }

        public static void init_drand(int x)
        {
            Global.DEBUG_LOW("init_drand with seed " + x);
            gen = new MersenneTwister(x);

        }

        static public int fixed_init_drand()
        {
            int seed = 0;
            init_drand(seed);
            return seed;
        }

       public static int dynamic_init_drand()
        {
            uint seed = (uint)DateTime.Now.Millisecond;
            init_drand((int)seed);
            seed += (uint)drand(long.MinValue, long.MaxValue);
            init_drand((int)seed);
            seed *= (uint)Process.GetCurrentProcess().Id;
            seed *= (uint)DateTime.Now.Ticks;
            init_drand((int)seed);
            return (int)seed;
        }

        public static double drand()
        {
            return gen.NextDoublePositive();
        }

        /// <summary>
        /// Random distribution: [-v,v]
        /// </summary>
        /// <param name="V"></param>
        /// <returns>random double within range</returns>
        public static double drand(double V)
        {
            return V * 2 * drand() - V;
        }

        /// <summary>
        /// Random number generator. Return a random number drawn from a uniform distribution over [v0,v1].
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <returns>random double between [v0,v1]</returns>
        public static double drand(double v0, double v1)
        {
            return gen.Next(v0, v1);
        }



    }
}
