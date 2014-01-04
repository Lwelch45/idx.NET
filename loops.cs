using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
namespace idx
{
    public static class loops
    {
        public static void CHECK_CONTIGUOUS1<T>(idx<T> m1) where T : struct
        {
            if (!(m1).contiguousp()) Global.eblerror("expected contiguous tensor " + m1);
        }

        public static void CHECK_CONTIGUOUS2<T, T2>(idx<T> m1, idx<T2> m2)
            where T : struct
            where T2 : struct
        {
            if (!(m1).contiguousp()) Global.eblerror("expected contiguous tensor " + m1);
            if (!(m2).contiguousp()) Global.eblerror("expected contiguous tensor " + m2);
        }

        public static void CHECK_CONTIGUOUS3<T, T2, T3>(idx<T> m1, idx<T2> m2, idx<T3> m3)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            if (!(m1).contiguousp()) Global.eblerror("expected contiguous tensor " + m1);
            if (!(m2).contiguousp()) Global.eblerror("expected contiguous tensor " + m2);
            if (!(m3).contiguousp()) Global.eblerror("expected contiguous tensor " + m3);
        }

        public static void idx_checkorder1<T>(idx<T> src0, int o0) where T : struct
        {
            if ((src0).order() != o0)
            {
                Global.eblerror(src0 + " does not have order " + o0);
            }
        }

        public static void idx_checkorder2<T, T2>(idx<T> src0, int o0, idx<T2> src1, int o1)
            where T : struct
            where T2 : struct
        {
            if ((src0).order() != o0)
            {
                Global.eblerror(src0 + " does not have order " + o0);
            }
            if ((src1).order() != o1)
            {
                Global.eblerror(src1 + " does not have order " + o1);
            }
        }

        public static void idx_checkorder3<T, T2, T3>(idx<T> src0, int o0, idx<T2> src1, int o1, idx<T3> src2, int o2)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            if ((src0).order() != o0)
            {
                Global.eblerror(src0 + " does not have order " + o0);
            }
            if ((src1).order() != o1)
            {
                Global.eblerror(src1 + " does not have order " + o1);
            }
            if ((src2).order() != o2)
            {
                Global.eblerror(src2 + " does not have order " + o2);
            }
        }

        public static void idx_compatibility_error2<T, T2>(idx<T> idx1, idx<T2> idx2, string errmsg)
            where T : struct
            where T2 : struct
        {
            Global.eblerror(idx1 + " and " + idx2 + " are incompatible: " + errmsg);
        }

        public static void idx_compatibility_error3<T, T2, T3>(idx<T> idx1, idx<T2> idx2, idx<T3> idx3, string errmsg)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            Global.eblerror(idx1 + " , " + idx2 + " and " + idx3 + " are incompatible: " + errmsg);
        }

        public static void idx_checknelems2_all<T, T2>(idx<T> src0, idx<T2> src1)
            where T : struct
            where T2 : struct
        {
            if ((src0).nelements() != (src1).nelements())
            {
                Global.eblerror(src0 + " and " + src1 +
                     " should have the same number of elements");
            }
        }

        public static void idx_checknelems3_all<T, T2, T3>(idx<T> src0, idx<T2> src1, idx<T3> src2)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            if (((src0).nelements() != (src1).nelements()) ||
          ((src0).nelements() != (src2).nelements()))
            {
                Global.eblerror(src0 + ", " + src1 + " and " + src2
                         + " should have the same number of elements");
            }
        }



        #region bloop
        public static void idx_bloop1<T>(idx<T> src0, Action<idxlooper<T>, idx<T>> act)
            where T : struct
   
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, 0);
            for (; dst0.notdone(); dst0.next()) act.Invoke(dst0, src0);
        }

        public static void idx_bloop2<T, T2>(idx<T> src0, idx<T2> src1, Action<idxlooper<T>, idx<T>, idxlooper<T2>, idx<T2>> act)
            where T : struct
            where T2 : struct
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, 0);
            idxlooper<T2> dst1 = new idxlooper<T2>(src1, 0);
            for (; dst0.notdone(); dst0.next(), dst1.next()) act.Invoke(dst0, src0, dst1, src1);
        }

        public static void idx_bloop3<T, T2, T3>(idx<T> src0, idx<T2> src1, idx<T3> src2, Action<idxlooper<T>, idx<T>, idxlooper<T2>, idx<T2>, idxlooper<T3>, idx<T3>> act)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, 0);
            idxlooper<T2> dst1 = new idxlooper<T2>(src1, 0);
            idxlooper<T3> dst2 = new idxlooper<T3>(src2, 0);
            for (; dst0.notdone(); dst0.next(), dst1.next(), dst2.next()) act.Invoke(dst0, src0, dst1, src1, dst2, src2);
        }
        #endregion

        #region cloop
        //Custom bloop that does not use sorce tensors
        public static void idx_cloop1<T>(idx<T> src0, Action<idxlooper<T>> act)
            where T : struct
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, 0);
            for (; dst0.notdone(); dst0.next()) act.Invoke(dst0);
        }

        public static void idx_cloop2<T, T2>(idx<T> src0, idx<T2> src1, Action<idxlooper<T>, idxlooper<T2>> act)
            where T : struct
            where T2 : struct
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, 0);
            idxlooper<T2> dst1 = new idxlooper<T2>(src1, 0);
            for (; dst0.notdone(); dst0.next(), dst1.next()) act.Invoke(dst0, dst1);
        }

        public static void idx_cloop3<T, T2, T3>(idx<T> src0, idx<T2> src1, idx<T3> src2, Action<idxlooper<T>, idxlooper<T2>, idxlooper<T3>> act)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, 0);
            idxlooper<T2> dst1 = new idxlooper<T2>(src1, 0);
            idxlooper<T3> dst2 = new idxlooper<T3>(src2, 0);
            for (; dst0.notdone(); dst0.next(), dst1.next(), dst2.next()) act.Invoke(dst0, dst1, dst2);
        }

        #endregion

        #region eloop
        public static void idx_eloop1<T>(idx<T> src0, Action<idxlooper<T>, idx<T>> act)
            where T : struct
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, src0.order() - 1);
            for (; dst0.notdone(); dst0.next()) act.Invoke(dst0, src0);
        }

        public static void idx_eloop2<T, T2>(idx<T> src0, idx<T2> src1, Action<idxlooper<T>, idx<T>, idxlooper<T2>, idx<T2>> act)
            where T : struct
            where T2 : struct
        {
            idxlooper<T> dst0 = new idxlooper<T>(src0, src0.order() - 1);
            idxlooper<T2> dst1 = new idxlooper<T2>(src1, src1.order() - 1);
            for (; dst0.notdone(); dst0.next(), dst1.next()) act.Invoke(dst0, src0, dst1, src1);
        }
        #endregion

        #region aloop
        public static void idx_aloop1_on<T>(idxiter<T> itr0, idx<T> src0, Action<eptr<T>> act) //Action<eptr<T>, idx<T>> act
            where T : struct
        {
            for (itr0.init(src0); itr0.notdone(); itr0.next())
                act.Invoke(itr0.data);
        }

        public static void idx_aloop1<T>(idx<T> src0, Action<eptr<T>> act) where T : struct { idx_aloop1_on(new idxiter<T>(), src0, act); }

        public static void idx_aloop2<T, T2>(idx<T> src0, idx<T2> src1, Action<eptr<T>, eptr<T2>> act)
            where T : struct
            where T2 : struct
        {
            var itr0 = new idxiter<T>();
            var itr1 = new idxiter<T2>();

            for (itr0.init(src0), itr1.init(src1); itr0.notdone(); itr0.next(), itr1.next())
                act.Invoke(itr0.data, itr1.data);
        }

        public static void idx_aloop3<T, T2, T3>(idx<T> src0, idx<T2> src1, idx<T3> src2, Action<eptr<T>, eptr<T2>, eptr<T3>> act)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            var itr0 = new idxiter<T>();
            var itr1 = new idxiter<T2>();
            var itr2 = new idxiter<T3>();
            for (itr0.init(src0), itr1.init(src1), itr2.init(src2); itr0.notdone(); itr0.next(), itr1.next(), itr2.next())
                act.Invoke(itr0.data, itr1.data, itr2.data);
        }
        #endregion
    }
}
