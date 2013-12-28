using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointerFactory;
using Base;
namespace idx
{
    public class eptr<T>
    {
       public int index;
        private Ptr<srg<T>> Base;

        public eptr(Ptr<srg<T>> b, int i) { Base = b; index = i; }

       

        public eptr() { }

        public eptr(Ptr<srg<T>> b) { Base = b; index = 0; }

        public static eptr<T> operator ++(eptr<T> a)
        {
            a.index += 1;
            return a;
        }

        public static eptr<T> operator --(eptr<T> a)
        {
            a.index -= 1;
            return a;
        }

        // Indexer to get and set words of the containing document:
        public T this[int i]
        {
            get
            {
                return Base.I.Get(i);
               // return ((srg<T>)Base.FetchItem()).Get(index);
            }
            set
            {             
                Base.I.Set(i,value);
                //((srg<T>)Base.FetchItem()).Set(index,value);
            }
        }

        public T item
        {
            get { return this[index]; }
            set { this[index] = value; }
        }
    }
}
