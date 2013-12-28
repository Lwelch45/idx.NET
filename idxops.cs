using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using i = idx.loops;
using g = Base.Global;
using db = System.Double;
using intg = System.Int32;
using Base;
using System.Linq.Expressions;
namespace idx
{
    public class idxops
    {
        public static void idx_random<T>(idx<T> m, double v1)
        {
            i.idx_aloop1(m, (m1) => m1.item = (T)(object)numerics.drand(v1));
        }

        public static void idx_random<T>(idx<T> m, double v1, double v2)
        {
            i.idx_aloop1(m, (m1) => m1.item = (T)(object)numerics.drand(v1, v2));
        }

        public static void idx_abs<T>(idx<T> m, idx<T> dst)
        {
            i.idx_aloop2(m, dst, (m1, dst1) => dst1.item = (T)(object)Math.Abs((double)(object)m1.item));
        }

        public static int idx_max(idx<int> m)
        {

            Int32 V = Int32.MinValue;
            bool loop = false;
            i.idx_aloop1(m, (m1) =>
            {
                loop = true;
                V = m1.item >= V ? m1.item : V;
                //V = Global.iif<double>(GreaterThan<double>(Convert.ToDouble(m1.item), V), Convert.ToDouble(m1.item), V);
            });
            if (!loop) { return 0; }
            return V;
        }
        
        public static T idx_max<T>(idx<T> m)
        {

            double V = Int32.MinValue;
            bool loop = false;
            i.idx_aloop1(m, (m1) =>
            {
                loop = true;
                V = Convert.ToDouble(m1.item) >= V ? Convert.ToDouble(m1.item) : V;
                //V = Global.iif<double>(GreaterThan<double>(Convert.ToDouble(m1.item), V), Convert.ToDouble(m1.item), V);
            });
            if (!loop) { return (T)Global.B(0); }
            return (T)Global.B(V);
        }

        public static T idx_min<T>(idx<T> m)
        {

            T V = (T)Global.B(int.MaxValue);
            bool loop = false;
            i.idx_aloop1(m, (m1) =>
            {
                loop = true;
                V = Global.iif<T>(LessThan<T>(m1.item, V), m1.item, V);
            });
            if (!loop) { return (T)Global.B(0); }
            return V;
        }

        public static T idx_sum<T>(idx<T> m)
        {
            T V = (T)Global.B(0.0);
            i.idx_aloop1(m, (m1) => V = Add<T>(V, m1.item));
            return V;
        }

        public static T idx_sumacc<T>(idx<T> m, idx<T> acc)
        {
            var sum = Add<T>(acc.get(), idx_sum(m));
            acc.set((T)Global.B(sum));
            return sum;
        }

        public static void idx_clear<T>(idx<T> m)
        {
            i.idx_aloop1(m, (pinp) => pinp.item = (T)g.B(0.0));
        }
        public static void idx_clear(idx<int> m)
        {
            i.idx_aloop1(m, (pinp) => pinp.item = 0);
        }

        public static void idx_fill<T>(idx<T> m, T V)
        {
            i.idx_aloop1(m, (pinp) => pinp.item = V);
        }

        public static void idx_copy<T>(idx<T> m, idx<T> dst)
        {
            if (m.order() == 0 && dst.order() == 0) { dst.set(m.get()); return; }
            i.idx_aloop2(m, dst, (m1, d1) => d1 = m1);
        }

        public static idx<T> idx_copy<T>(idx<T> m)
        {
            idx<T> dst = new idx<T>(m.get_idxdim());
            idx_copy<T>(m, dst);
            return dst;

        }

        public static void idx_copy<T, T2>(idx<T> m, idx<T2> dst)
        {
            if (m.order() == 0 && dst.order() == 0) { dst.set((T2)Global.B(m.get())); return; }
            i.idx_aloop2(m, dst, (m1, d1) => d1.item = (T2)Global.B(m1.index));
        }

        public static void idx_add<T>(idx<T> m, idx<T> n, idx<T> dst)
        {
            i.idx_aloop3(m, n, dst, (m1, n1, dst1) => dst1.item = Add<T>(n1.item, m1.item));
        }
        public static void idx_addacc<T>(idx<T> m, idx<T> n, idx<T> dst)
        {
            i.idx_aloop3(m, n, dst, (m1, n1, dst1) => dst1.item = Add<T>(dst1.item, Add<T>(n1.item, m1.item)));
        }

        public static void idx_addc<T>(idx<T> m, T V)
        {
            i.idx_aloop1(m, (L) => L.item = Add<T>(L.item, V));
        }

        public static void idx_addc<T>(idx<T> m, T V, idx<T> dst)
        {
            i.idx_aloop2(m, dst, (L, dst1) => dst1.item = Add<T>(L.item, V));
        }

        public static void idx_dotcacc<T, T2>(idx<T> m, T2 V, idx<T> dst)
        {
            i.idx_aloop2(m, dst, (m1, dst1) => dst1.item = Add(Dot(m1.item, V), V));
        }

        public static void idx_dotc<T, T2>(idx<T> m, T2 V, idx<T> dst)
        {
            i.idx_aloop2(m, dst, (m1, dst1) => dst1.item = Dot(m1.item, V));
        }

        public static void idx_signdotcacc(idx<double> m, double V, idx<double> dst)
        {
            i.idx_aloop2(m, dst, (m1, dst1) => dst1.item = Add<double>(Dot<double>(g.iif(m1.item < 0, -m1.item, m1.item), V), V));
        }

        public static void idx_lincomb<T>(idx<T> m, T k1, idx<T> n, T k2, idx<T> dst)
        {
            i.idx_aloop3(m, n, dst, (m1, n1, dst1) => dst1.item = Add<T>(Add<T>(m1.item, k1), Add<T>(n1.item, k2)));
        }

        public static void idx_mul<T>(idx<T> m, idx<T> V, idx<T> dst)
        {
            i.idx_aloop3(m, V, dst, (m1, v1, dst1) => dst1.item = Dot<T>(m1.item, v1.item));
        }

        public static void idx_inv<T>(idx<T> m, idx<T> dst)
        {
            i.idx_aloop2(m, dst, (m1, dst1) => dst1.item = Inv<T>(m1.item));
        }

        public static void idx_sub<T>(idx<T> m, idx<T> V, idx<T> dst)
        {
            i.idx_aloop3(m, V, dst, (m1, v1, dst1) => dst1.item = Subtract(m1.item, v1.item));
        }

        public static void idx_sub<T>(idx<T> m, idx<T> dst)
        {
            i.idx_aloop2(m, dst, (m1, dst1) => dst1.item = Subtract(m1.item, dst1.item));
        }

        public static void idx_m4dotm2acc<T>(idx<T> i1, idx<T> i2, idx<T> o1)
        {
            //loops.idx_checkorder3(i1, 4, i2, 2, o1, 2); // check for compatible orders
            //if ((i1.dim(0) != o1.dim(0)) || (i1.dim(1) != o1.dim(1))
            //  || (i1.dim(2) != i2.dim(0)) || (i1.dim(3) != i2.dim(1)))
            //    loops.idx_compatibility_error3(i1, i2, o1, "incompatible dimensions");
            eptr<T> c1, c1_2;
            eptr<T> c2, c2_0;
            eptr<T> c1_0, c1_1;
            eptr<T> ker;
            intg c1_m2 = (i1).mod(2), c2_m0 = (i2).mod(0);
            intg c1_m3 = (i1).mod(3), c2_m1 = (i2).mod(1);
            intg k, l, kmax = (i2).dim(0), lmax = (i2).dim(1);
            eptr<T> d1_0, d1;
            T f;
            intg c1_m0 = (i1).mod(0), d1_m0 = (o1).mod(0);
            intg c1_m1 = (i1).mod(1), d1_m1 = (o1).mod(1);
            intg i, j, imax = (o1).dim(0), jmax = (o1).dim(1);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1_0 = o1.idx_ptr();
            c1_1 = c1_0;
            d1 = d1_0;
            c1_2 = c1_1;
            c2_0 = ker;
            for (i = 0; i < imax; i++)
            {
                c1_1.index = c1_0.index;
                d1.index = d1_0.index;
                for (j = 0; j < jmax; j++)
                {
                    f = d1.item;
                    c1_2.index = c1_1.index;
                    c2_0.index = ker.index;
                    for (k = 0; k < kmax; k++)
                    {
                        c1 = c1_2;
                        c2 = c2_0;
                        for (l = 0; l < lmax; l++)
                        {
                            f = Add<T>(Dot<T>(c1.item, c2.item), f);
                            c1.index += c1_m3;
                            c2.index += c2_m1;
                        }
                        c1_2.index += c1_m2;
                        c2_0.index += c2_m0;
                    }
                    d1.item = f;
                    d1.index += d1_m1;
                    c1_1.index += c1_m1;
                }
                d1_0.index += d1_m0;
                c1_0.index += c1_m0;
            }
        }

        public static void idx_m4squdotm2acc<T>(idx<T> i1, idx<T> i2, idx<T> o1)
        {
            loops.idx_checkorder3(i1, 4, i2, 2, o1, 2); // check for compatible orders
            if ((i1.dim(0) != o1.dim(0)) || (i1.dim(1) != o1.dim(1))
              || (i1.dim(2) != i2.dim(0)) || (i1.dim(3) != i2.dim(1)))
                loops.idx_compatibility_error3(i1, i2, o1, "incompatible dimensions");
            eptr<T> c1, c1_2;
            eptr<T> c2, c2_0;
            eptr<T> c1_0, c1_1;
            eptr<T> ker;
            intg c1_m2 = (i1).mod(2), c2_m0 = (i2).mod(0);
            intg c1_m3 = (i1).mod(3), c2_m1 = (i2).mod(1);
            intg k, l, kmax = (i2).dim(0), lmax = (i2).dim(1);
            eptr<T> d1_0, d1;
            T f;
            intg c1_m0 = (i1).mod(0), d1_m0 = (o1).mod(0);
            intg c1_m1 = (i1).mod(1), d1_m1 = (o1).mod(1);
            intg i, j, imax = (o1).dim(0), jmax = (o1).dim(1);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1_0 = o1.idx_ptr();
            c1_1 = c1_0;
            d1 = d1_0;
            c1_2 = c1_1;
            c2_0 = ker;
            for (i = 0; i < imax; i++)
            {
                c1_1.index = c1_0.index;
                d1.index = d1_0.index;
                for (j = 0; j < jmax; j++)
                {
                    f = d1.item;
                    c1_2.index = c1_1.index;
                    c2_0.index = ker.index;
                    for (k = 0; k < kmax; k++)
                    {
                        c1 = c1_2;
                        c2 = c2_0;
                        for (l = 0; l < lmax; l++)
                        {
                            f = Add<T>(Dot<T>(Dot<T>(c1.item, c1.item), c2.item), f);
                            c1.index += c1_m3;
                            c2.index += c2_m1;
                        }
                        c1_2.index += c1_m2;
                        c2_0.index += c2_m0;
                    }
                    d1.item = f;
                    d1.index += d1_m1;
                    c1_1.index += c1_m1;
                }
                d1_0.index += d1_m0;
                c1_0.index += c1_m0;
            }
        }

        public static void idx_m4dotm2<T>(idx<T> i1, idx<T> i2, idx<T> o1)
        {
            loops.idx_checkorder3(i1, 4, i2, 2, o1, 2); // check for compatible orders
            if ((i1.dim(0) != o1.dim(0)) || (i1.dim(1) != o1.dim(1))
               || (i1.dim(2) != i2.dim(0)) || (i1.dim(3) != i2.dim(1)))
                loops.idx_compatibility_error3(i1, i2, o1, "incompatible dimensions");
            eptr<T> c1, c1_2;
            eptr<T> c2, c2_0;
            eptr<T> c1_0, c1_1;
            eptr<T> ker;
            intg c1_m2 = (i1).mod(2), c2_m0 = (i2).mod(0);
            intg c1_m3 = (i1).mod(3), c2_m1 = (i2).mod(1);
            intg k, l, kmax = (i2).dim(0), lmax = (i2).dim(1);
            eptr<T> d1_0, d1;
            T f;
            intg c1_m0 = (i1).mod(0), d1_m0 = (o1).mod(0);
            intg c1_m1 = (i1).mod(1), d1_m1 = (o1).mod(1);
            intg i, j, imax = (o1).dim(0), jmax = (o1).dim(1);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1_0 = o1.idx_ptr();
            c1_1 = c1_0;
            d1 = d1_0;
            c1_2 = c1_1;
            c2_0 = ker;
            for (i = 0; i < imax; i++)
            {
                c1_1.index = c1_0.index;
                d1.index = d1_0.index;
                for (j = 0; j < jmax; j++)
                {
                    f = (T)Global.B(0.0);
                    c1_2.index = c1_1.index;
                    c2_0.index = ker.index;
                    for (k = 0; k < kmax; k++)
                    {
                        c1 = c1_2;
                        c2 = c2_0;
                        for (l = 0; l < lmax; l++)
                        {
                            f = Add<T>(Dot<T>(c1.item, c2.item), f);
                            c1.index += c1_m3;
                            c2.index += c2_m1;
                        }
                        c1_2.index += c1_m2;
                        c2_0.index += c2_m0;
                    }
                    d1.item = f;
                    d1.index += d1_m1;
                    c1_1.index += c1_m1;
                }
                d1_0.index += d1_m0;
                c1_0.index += c1_m0;
            }
        }

        public static void idx_m2extm2<T>(idx<T> i1, idx<T> i2, idx<T> o1)
        {
            eptr<T> c2_0, c2_1;
            eptr<T> d1_2, d1_3;
            eptr<T> d1_0, d1_1;
            eptr<T> c1_0, c1_1;
            eptr<T> ker;
            intg c2_m0 = (i2).mod(0), c2_m1 = (i2).mod(1);
            intg d1_m2 = (o1).mod(2), d1_m3 = (o1).mod(3);
            intg c1_m0 = (i1).mod(0), c1_m1 = (i1).mod(1);
            intg d1_m0 = (o1).mod(0), d1_m1 = (o1).mod(1);
            intg k, l, lmax = (o1).dim(3), kmax = (o1).dim(2);
            intg i, j, imax = (o1).dim(0), jmax = (o1).dim(1);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1_0 = o1.idx_ptr();
            d1_2 = d1_0;
            c2_0 = ker;
            for (i = 0; i < imax; i++)
            {
                d1_1 = d1_0;
                c1_1 = c1_0;
                for (j = 0; j < jmax; j++)
                {
                    d1_2.index = d1_1.index;
                    c2_0.index = ker.index;
                    for (k = 0; k < kmax; k++)
                    {
                        d1_3 = d1_2;
                        c2_1 = c2_0;
                        for (l = 0; l < lmax; l++)
                        {
                            d1_3.item = Dot<T>(c1_1.item, c2_1.item);
                            d1_3.index += d1_m3;
                            c2_1.index += c2_m1;
                        }
                        d1_2.index += d1_m2;
                        c2_0.index += c2_m0;
                    }
                    d1_1.index += d1_m1;
                    c1_1.index += c1_m1;
                }
                d1_0.index += d1_m0;
                c1_0.index += c1_m0;
            }
        }

        public static void idx_m2squextm2acc<T>(idx<T> i1, idx<T> i2, idx<T> o1)
        {
            eptr<T> c2_0, c2_1;
            eptr<T> d1_2, d1_3;
            eptr<T> d1_0, d1_1;
            eptr<T> c1_0, c1_1;
            eptr<T> ker;
            intg c2_m0 = (i2).mod(0), c2_m1 = (i2).mod(1);
            intg d1_m2 = (o1).mod(2), d1_m3 = (o1).mod(3);
            intg c1_m0 = (i1).mod(0), c1_m1 = (i1).mod(1);
            intg d1_m0 = (o1).mod(0), d1_m1 = (o1).mod(1);
            intg k, l, lmax = (o1).dim(3), kmax = (o1).dim(2);
            intg i, j, imax = (o1).dim(0), jmax = (o1).dim(1);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1_0 = o1.idx_ptr();
            d1_2 = d1_0;
            c2_0 = ker;
            for (i = 0; i < imax; i++)
            {
                d1_1 = d1_0;
                c1_1 = c1_0;
                for (j = 0; j < jmax; j++)
                {
                    d1_2.index = d1_1.index;
                    c2_0.index = ker.index;
                    for (k = 0; k < kmax; k++)
                    {
                        d1_3 = d1_2;
                        c2_1 = c2_0;
                        for (l = 0; l < lmax; l++)
                        {
                            d1_3.item = Add<T>(Dot<T>(Dot<T>(c1_1.item, c2_1.item), c2_1.item), d1_3.item);
                            d1_3.index += d1_m3;
                            c2_1.index += c2_m1;
                        }
                        d1_2.index += d1_m2;
                        c2_0.index += c2_m0;
                    }
                    d1_1.index += d1_m1;
                    c1_1.index += c1_m1;
                }
                d1_0.index += d1_m0;
                c1_0.index += c1_m0;
            }
        }

        public static void idx_m2extm2acc<T>(idx<T> i1, idx<T> i2, idx<T> o1)
        {
            eptr<T> c2_0, c2_1;
            eptr<T> d1_2, d1_3;
            eptr<T> d1_0, d1_1;
            eptr<T> c1_0, c1_1;
            eptr<T> ker;
            intg c2_m0 = (i2).mod(0), c2_m1 = (i2).mod(1);
            intg d1_m2 = (o1).mod(2), d1_m3 = (o1).mod(3);
            intg c1_m0 = (i1).mod(0), c1_m1 = (i1).mod(1);
            intg d1_m0 = (o1).mod(0), d1_m1 = (o1).mod(1);
            intg k, l, lmax = (o1).dim(3), kmax = (o1).dim(2);
            intg i, j, imax = (o1).dim(0), jmax = (o1).dim(1);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1_0 = o1.idx_ptr();
            d1_2 = d1_0;
            c2_0 = ker;
            for (i = 0; i < imax; i++)
            {
                d1_1 = d1_0;
                c1_1 = c1_0;
                for (j = 0; j < jmax; j++)
                {
                    d1_2.index = d1_1.index;
                    c2_0.index = ker.index;
                    for (k = 0; k < kmax; k++)
                    {
                        d1_3 = d1_2;
                        c2_1 = c2_0;
                        for (l = 0; l < lmax; l++)
                        {
                            d1_3.item = Add<T>(Dot<T>(c1_1.item, c2_1.item), d1_3.item);
                            d1_3.index += d1_m3;
                            c2_1.index += c2_m1;
                        }
                        d1_2.index += d1_m2;
                        c2_0.index += c2_m0;
                    }
                    d1_1.index += d1_m1;
                    c1_1.index += c1_m1;
                }
                d1_0.index += d1_m0;
                c1_0.index += c1_m0;
            }
        }

        public static void idx_m2squdotm2<T>(idx<T> i1, idx<T> i2, idx<T> o)
        {
            loops.idx_checkorder3(i1, 2, i2, 2, o, 0);
#warning   idx_checkdim2(i1, 0, i2.dim(0), i1, 1, i2.dim(1));

            intg imax = i1.dim(0), jmax = i2.dim(1);
            intg c1_m0 = i1.mod(0), c2_m0 = i2.mod(0);
            intg c1_m1 = i1.mod(1), c2_m1 = i2.mod(1);
            eptr<T> c1_0, c2_0, d1, c1, c2;

            c1_0 = i1.idx_ptr();
            c2_0 = i2.idx_ptr();
            d1 = o.idx_ptr();

            c1 = c1_0;
            c2 = c2_0;
            T f = (T)Global.B(0.0);
            for (int i = 0; i < imax; ++i)
            {
                c1.index = c1_0.index;
                c2.index = c2_0.index;
                for (int j = 0; j < jmax; ++j)
                {
                    T ff = Dot<T>(Dot<T>(c1.item, c1.item), c2.item);
                    f = Add<T>(ff, f);
                    c1.index += c1_m1;
                    c2.index += c2_m1;
                }
                c1_0.index += c1_m0;
                c2_0.index += c2_m0;
            }
            d1.item = f;
        }

        public static void idx_m2squdotm2acc<T>(idx<T> i1, idx<T> i2, idx<T> o)
        {
            loops.idx_checkorder3(i1, 2, i2, 2, o, 0);
#warning   idx_checkdim2(i1, 0, i2.dim(0), i1, 1, i2.dim(1));

            intg imax = i1.dim(0), jmax = i2.dim(1);
            intg c1_m0 = i1.mod(0), c2_m0 = i2.mod(0);
            intg c1_m1 = i1.mod(1), c2_m1 = i2.mod(1);
            eptr<T> c1_0, c2_0, d1, c1, c2;

            c1_0 = i1.idx_ptr();
            c2_0 = i2.idx_ptr();
            d1 = o.idx_ptr();

            c1 = c1_0;
            c2 = c2_0;
            T f = d1.item;
            for (int i = 0; i < imax; ++i)
            {
                c1.index = c1_0.index;
                c2.index = c2_0.index;
                for (int j = 0; j < jmax; ++j)
                {
                    T ff = Dot<T>(Dot<T>(c1.item, c1.item), c2.item);
                    f = Add<T>(ff, f);
                    c1.index += c1_m1;
                    c2.index += c2_m1;
                }
                c1_0.index += c1_m0;
                c2_0.index += c2_m0;
            }
            d1.item = f;
        }

        /* multiply M2 by M1, result in M1 */
        /* matrix - vector product */
        public static void idx_m2dotm1(idx<db> i1, idx<db> i2, idx<db> o1)
        {
            eptr<double> c1, c2, c1_0, ker;

            intg c1_m1 = i1.mod(1), c2_m0 = i2.mod(0);
            intg jmax = i2.dim(0);
            intg c1_m0 = i1.mod(0), d1_m0 = o1.mod(0);
            eptr<double> d1;
            double f = 0;
            intg imax = o1.dim(0);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1 = o1.idx_ptr();

            c1 = i1.idx_ptr();
            c2 = i2.idx_ptr();
            for (int ii = 0; ii < imax; ii++)
            {
                f = 0;
                c1.index = c1_0.index;
                c2.index = ker.index;
                if (c1_m1 == 1 && c2_m0 == 1)
                    for (int j = 0; j < jmax; j++)
                    {
                        f += (c1.item) * (c2.item);
                        c1++; c2++;
                    }
                else
                    for (int j = 0; j < jmax; j++)
                    {
                        f += (c1.item) * (c2.item);
                        c1.index += c1_m1;
                        c2.index += c2_m0;
                    }
                d1.item = f;
                d1.index += d1_m0;
                c1_0.index += c1_m0; //+ c1_0.index;
            }

        }

        public static void idx_m2dotm1acc(idx<db> i1, idx<db> i2, idx<db> o1)
        {
            eptr<double> c1, c2, c1_0, ker;

            intg c1_m1 = i1.mod(1), c2_m0 = i2.mod(0);
            intg jmax = i2.dim(0);
            intg c1_m0 = i1.mod(0), d1_m0 = o1.mod(0);
            eptr<double> d1;
            double f = 0;
            intg imax = o1.dim(0);
            c1_0 = i1.idx_ptr();
            ker = i2.idx_ptr();
            d1 = o1.idx_ptr();

            c1 = i1.idx_ptr();
            c2 = i2.idx_ptr();
            for (int ii = 0; ii < imax; ii++)
            {
                f = d1.item;
                c1.index = c1_0.index;
                c2.index = ker.index;
                if (c1_m1 == 1 && c2_m0 == 1)
                    for (int j = 0; j < jmax; j++)
                    {
                        f += (c1.item) * (c2.item);
                        c1++; c2++;
                    }
                else
                    for (int j = 0; j < jmax; j++)
                    {
                        f += (c1.item) * (c2.item);
                        c1.index += c1_m1;
                        c2.index += c2_m0;
                    }
                d1.item += f;
                d1.index += d1_m0;
                c1_0.index += c1_m0; //+ c1_0.index;
            }

        }

        public static T Add<T>(T a, T b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");

            // Add the parameters together
            BinaryExpression body = Expression.Add(paramA, paramB);

            // Compile it
            Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

            // Call it
            var res = add.Invoke(a, b);
            return res;
        }

        public static T Subtract<T>(T a, T b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");

            // Add the parameters together
            BinaryExpression body = Expression.Subtract(paramA, paramB);

            // Compile it
            Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

            // Call it
            return add.Invoke(a, b);
        }

        public static T Add<T, T2>(T a, T2 b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T2), "b");

            // Add the parameters together
            BinaryExpression body = Expression.Add(paramA, paramB);

            // Compile it
            Func<T, T2, T> add = Expression.Lambda<Func<T, T2, T>>(body, paramA, paramB).Compile();

            // Call it
            var res = add.Invoke(a, b);
            return res;
        }

        public static T Dot<T>(T a, T b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");

            // Add the parameters together
            BinaryExpression body = Expression.Multiply(paramA, paramB);

            // Compile it
            Func<T, T, T> multiply = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();

            // Call it
            return multiply.Invoke(a, b);
        }

        public static T Dot<T, T2>(T a, T2 b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T2), "b");

            // Add the parameters together
            BinaryExpression body = Expression.Multiply(paramA, paramB);

            // Compile it
            Func<T, T2, T> multiply = Expression.Lambda<Func<T, T2, T>>(body, paramA, paramB).Compile();

            // Call it
            return multiply.Invoke(a, b);
        }

        public static T Inv<T>(T a)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(int));
            var paramB = Expression.Parameter(typeof(T), "a");

            // Add the parameters together
            BinaryExpression body = Expression.Divide(paramA, paramB);

            // Compile it
            Func<int, T, T> inv = Expression.Lambda<Func<int, T, T>>(body, paramA, paramB).Compile();

            // Call it
            return inv.Invoke(1, a);
        }

        public static bool GreaterThan<T>(T a, T b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");

            BinaryExpression body = Expression.GreaterThanOrEqual(paramA, paramB);

            Func<T, T, bool> greaterth = Expression.Lambda<Func<T, T, bool>>(body, paramA, paramB).Compile();

            // Call it
            return greaterth.Invoke(a, b);

        }

        public static bool LessThan<T>(T a, T b)
        {
            // Declare the parameters
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");

            BinaryExpression body = Expression.LessThanOrEqual(paramA, paramB);

            Func<T, T, bool> less = Expression.Lambda<Func<T, T, bool>>(body, paramA, paramB).Compile();

            // Call it
            return less.Invoke(a, b);

        }
    }
}
