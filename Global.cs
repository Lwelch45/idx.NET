using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
   public static class Global
    {
        public const int MAXDIMS = 8;
        public const bool DEBUG = false;

        
        public static void DEBUG_LOW(object obj) { if (DEBUG) Console.WriteLine(obj); }
        public static void EDEBUG(object obj) { if (DEBUG)  Console.WriteLine("Debug: " + obj); }
        public static void EDEBUG_MAT(string st,object obj) { if (DEBUG)  Console.WriteLine(st+ "/n" + obj); }
        public static void eblwarn(object obj) { Console.WriteLine("Warning: " + obj); }
        public static void eblerror(string o) { throw new Exception(o); }
        public static void eblprint(object obj) { Console.WriteLine(obj); }
        public static object B(object o) { return ((object)o); }
        public static void Print(object o) { Console.WriteLine(o); }     
        public static T iif<T>(bool expression, T truePart, T falsePart) { return expression ? truePart : falsePart; }
   }
}
