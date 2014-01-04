using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
namespace Base
{

    // idxd ////////////////////////////////////////////////////////////////////////

    //! This class allows to extract dimensions information from existing idx
    //! objects in order to create other idx objects with the same order without
    //! knowning their order in advance. One can modify the order, each dimensions
    //! and their offsets. Offsets are 0 by default, but can be used to define
    //! bounding boxes for a n-dimensional tensor.
   public  class idxd<T> : IDisposable
    {
        public   T[] offsets;
        public   int ndim;
        public   T[] dims; // why not get the maximum precision?
        public   Guid id = Guid.NewGuid();

        public static bool operator ==(idxd<T> a, idxd<T> b)
        {
            if (b.Equals(null)) { return false; }
            if (!(a.ndim == b.ndim)) { return false; }
            if (!(a.dims == b.dims)) { return false; }
            return true;
        }

        public static bool operator !=(idxd<T> a, idxd<T> b)
        {
            if (b.Equals(null)) { return true; }
            if ((a.ndim == b.ndim) && (a.dims == b.dims)) { return false; }
            return true;
        }
        public int order() { return ndim; }

        public dynamic dim(int dimn)
        {
            if (dimn >= ndim)
                throw new Exception("trying to access size of dimension " + dimn
                         + " but idxdim's maximum dimensions is " + ndim);
            return dims[dimn];
        }

        public void Dispose()
        {
            if (!(offsets == null)) { offsets = null; }
        } 
    }
}
