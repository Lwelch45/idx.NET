using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
namespace idx
{
    using intg = Int32;
    // idxlooper ///////////////////////////////////////////////////////////////////

    //! idxlooper: a kind of iterator used by bloop
    //! and eloop macros. idxlooper is a subclass of idx,
    //! It is used as follows:
    //! for (idxlooper z(&idx,0); z.notdone(); z.next()) { .... }
    public class idxlooper<T> : idx<T> where T : struct
    {
        intg i;  // loop index
        intg dimd;  // number of elements to iterated upon
        intg modd; // stride in dimension being iterated upon
        eptr<T> item;
        

        public idxlooper(idx<T> m, int ld)
        {
            if (m.order() == 0) { Global.eblerror("cannot loop on idx with order 0. idx is: " + m); }
            i = 0;
            dimd = m.spec.dim[ld];
            modd = m.spec.mod[ld];
            m.spec.select_into(ref spec, ld, i);
            this.srgptr = m.srgptr.Addreference();
            item = new eptr<T>(srgptr, spec.getoffset());
        }

        public eptr<T> next()
        {
            i++;
            spec.add_offset(modd);
            return new eptr<T>(srgptr, spec.getoffset());
        }

        public bool notdone() {return (i < dimd);}
    }
}
