using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using PointerFactory;
using System.Linq.Expressions;
using T = System.Double;
namespace idx
{
    static public class Test
    {
        public static void TestReader()
        {
            //idxdim d = idxIO.get_matrix_dims(@"F:\Datasets\DSCompile\net00000.mat");
            //Global.Print(d.ToString());


            FILE fp = new FILE(@"F:\Datasets\DSCompile\net00000.mat");
            idx<double> dubs = idxIO.load_matrix<double>(ref fp, null);
            Global.Print(dubs.ToString());
        
        }

        public static void Testidxlooper()
        {
            idx<Double> ii = new idx<Double>(30, 3);
            int indx = 5;
            loops.idx_aloop1_on<double>(new idxiter<double>(), ii, (x) => x.item += indx++);

            idxlooper<double> lop = new idxlooper<double>(ii, 0);

            for (; lop.notdone(); lop.next())
            {
                //Global.Print(indx++);
                Global.Print(lop.idx_ptr().item);
            }
        }

        public static void Testm2dotm1()
        {
            idx<T> d2 = new idx<T>(2, 5);
            idx<T> d1 = new idx<T>(5);
            idx<T> o = new idx<T>(2);

            idxops<T>.idx_fill(d2, 2.0);
            idxops<T>.idx_fill(d1, 1.0);
            idxops<T>.idx_m2dotm1(d2, d1, o);
            loops.idx_bloop1(o, (lop, o1) => Global.Print(o.get()));
        }

        public static T CheckGenerics<T>(T a, T b)
        {
            return Add(a,b);
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
            return add(a, b);
        }
    }
}