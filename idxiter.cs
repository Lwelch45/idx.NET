using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using PointerFactory;
namespace idx
{
    using intg = Int32;
    ////////////////////////////////////////////////////////////////
    // idx Iterators: gives you a pointer to the actual data,
    // unlike idxlooper which gives you another idx.

    //! idxiter allows to iterate over all elements of an idx.
    //! Although it can be used directly, it is easier to use
    //! it with the idx_aloopX macros. Example:
    //!  idxiter<double> idx;
    //!  for ( idx.init(m); idx.notdone(); idx.next() ) {
    //!    printf("%g ",*idx);
    //!  }
    //! Here is an example that uses the aloop macro to fill up
    //! an idx with numbers corresponding to the loop index:
    //! idxiter<double> idx;
    //! idx_aloop_on(idx,m) { *idx = idx.i; }
    //! At any point during the loop, the indices of the element
    //! being worked on is stored in idx.d[k] for k=0
    //! to idx.order()-1.
    public class idxiter<T> //use type variable not yet achieved
    {
        public eptr<T> data; //!< pointer to current item
        intg i; //!< number of elements visited so far (loop index)
        intg n; //!< total number of elements in idx
        int j; //!< dimension being looped over
        intg[] d = new intg[Global.MAXDIMS]; //!< loop index array for non-contiguous idx
        //public Ptr<idx<T9>> iterand; //!< pointer to idx being looped over.
        public Ptr<srg<T>> srg;
        public idxspec spec;

        public idxiter() { }

        public eptr<T> init(idx<T> m)
        {
            spec = m.spec;
            i = 0;
            j = spec.ndim;
            data = new eptr<T>(m.srgptr,spec.getoffset());

            n = spec.nelements();
            if (spec.contiguousp())
            {
                d[0] = -1;
            }
            else
            {
                for ( i = 0; i < spec.ndim; i++) { d[i] = 0; }
            }
            return data;
        }

        public eptr<T> next()
        {
            i++;
            if (d[0] < 0)
            {
                // contiguous idx
                data++;
            }
            else
            {
                // non-contiguous idx
                j--;
                do
                {
                    if (j < 0)
                    {
                        break;
                    }
                    if (++d[j] < spec.dim[j])
                    {
                        data.index += spec.mod[j];
                        j++;
                    }
                    else
                    {
                        data.index -= spec.dim[j] * spec.mod[j];
                        d[j--] = -1;
                    }
                } while (j < spec.ndim);
            }
            return data;
        }

        public bool notdone() { return (i < n); }

       
    }
}
