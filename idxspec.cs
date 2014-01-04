using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    using intg = System.Int32;

    // idxspec /////////////////////////////////////////////////////////////////////

    //! idxspec contains all the characteristics of an idx,
    //! except the storage. It includes the order (number of dimensions)
    //! the offset, and the dim and mod arrays.
    //! having an idxspec class separate from idx allows us to write
    //! generic manipulation functions that do not depend on the
    //! type of the storage.
    public class idxspec : IDisposable
    {
        public int ndim; //number of dimensions
        int offset; //offset in the storage
        public int[] dim; //array of sizes in each dimension
        public int[] mod; //array of strides in each dimension
        Guid id = Guid.NewGuid();

        public void Dispose()
        {
            Global.DEBUG_LOW("idxspec::~idxspec: " + id.ToString());
            setndim(0);
        }

        public idxspec(idxspec src)
        {
            ndim = -1; dim = null; mod = null;
            copy(src);
        }

        public idxspec()
        {
            ndim = -1;
            offset = 0;
            dim = null; mod = null;
            // dim = new int[Global.MAXDIMS];
            // mod = new int[Global.MAXDIMS];
            // offset = 0;
            // ndim = -1;
        }

        public idxspec(int o)
        {
            ndim = 0;
            offset = o;
            dim = null; mod = null;
        }

        public idxspec(int o, int size0)
        {
            if (size0 < 0) throw new Exception("trying to construct idx1 with negative dimension: " + size0);
            dim = null; mod = null;
            offset = o;
            ndim = -1;
            setndim(1);
            dim[0] = size0;
            mod[0] = 1;
        }

        public idxspec(int o, int size0, int size1)
        {
            if (size0 < 0 || size1 < 0) throw new Exception("trying to construct idx2 with negative dimensions: " + size0 + "x" + size1 + " offset: " + o);
            dim = null; mod = null;
            offset = o;
            ndim = -1;
            setndim(2);
            dim[0] = size0;
            mod[0] = size1;
            dim[1] = size1;
            mod[1] = 1;
        }

        public idxspec(int o, int size0, int size1, int size2)
        {
            if (size0 < 0 || size1 < 0 || size2 < 0) throw new Exception("trying to construct idx3 with negative dimensions: " + size0 + "x" + size1 + "x" + size2 + " offset: " + o);
            dim = null; mod = null;
            offset = o;
            ndim = -1;
            setndim(3);
            dim[0] = size0;
            mod[0] = size1 * size2;
            dim[1] = size1;
            mod[1] = size2;
            dim[2] = size2;
            mod[2] = 1;
        }

        public idxspec(intg o, intg s0, intg s1, intg s2, intg s3, intg s4 = -1, intg s5 = -1, intg s6 = -1, intg s7 = -1)
        {
            init_spec(o, s0, s1, s2, s3, s4, s5, s6, s7);
        }

        public static void INIT_ERROR(int v) { throw new Exception("incompatible dimensions (error " + v + ")"); }

        private void init_spec(intg o, intg s0, intg s1, intg s2, intg s3, intg s4, intg s5, intg s6, intg s7)
        {
            bool ndimset = false;
            intg md = 1;
            dim = null; mod = null;
            offset = o;
            ndim = -1; // required in constructors to avoid side effects in setndim
            if (s7 >= 0)
            {
                if (!ndimset) { setndim(8); ndimset = true; }
                dim[7] = s7; mod[7] = md; md *= s7;
            }
            else { if (ndimset) { INIT_ERROR(-8); } }
            if (s6 >= 0)
            {
                if (!ndimset) { setndim(7); ndimset = true; }
                dim[6] = s6; mod[6] = md; md *= s6;
            }
            else { if (ndimset) { INIT_ERROR(-7); } }
            if (s5 >= 0)
            {
                if (!ndimset) { setndim(6); ndimset = true; }
                dim[5] = s5; mod[5] = md; md *= s5;
            }
            else { if (ndimset) { INIT_ERROR(-6); } }
            if (s4 >= 0)
            {
                if (!ndimset) { setndim(5); ndimset = true; }
                dim[4] = s4; mod[4] = md; md *= s4;
            }
            else { if (ndimset) { INIT_ERROR(-5); } }
            if (s3 >= 0)
            {
                if (!ndimset) { setndim(4); ndimset = true; }
                dim[3] = s3; mod[3] = md; md *= s3;
            }
            else { if (ndimset) { INIT_ERROR(-4); } }
            if (s2 >= 0)
            {
                if (!ndimset) { setndim(3); ndimset = true; }
                dim[2] = s2; mod[2] = md; md *= s2;
            }
            else { if (ndimset) { INIT_ERROR(-3); } }
            if (s1 >= 0)
            {
                if (!ndimset) { setndim(2); ndimset = true; }
                dim[1] = s1; mod[1] = md; md *= s1;
            }
            else { if (ndimset) { INIT_ERROR(-2); } }
            if (s0 >= 0)
            {
                if (!ndimset) { setndim(1); ndimset = true; }
                dim[0] = s0; mod[0] = md;
            }
            else { if (ndimset) { INIT_ERROR(-1); } }
            if (!ndimset) { setndim(0); }
        }

        private void init_spec(intg o, intg s0, intg s1, intg s2, intg s3, intg s4, intg s5, intg s6, intg s7, uint n)
        {
            //could not fully implement the C++ version of this overload becase C# does not support drop through switch statements.
            intg md = 1;
            dim = null; mod = null;
            offset = o;
            ndim = -1; // required in constructors to avoid side effects in setndim
            setndim(int.Parse("" + n));
            switch (n)
            {
                case 8:
                    dim[7] = s7; mod[7] = md; md *= s7; if (s7 < 0) INIT_ERROR(-8);
                    dim[6] = s6; mod[6] = md; md *= s6; if (s6 < 0) INIT_ERROR(-7);
                    dim[5] = s5; mod[5] = md; md *= s5; if (s5 < 0) INIT_ERROR(-6);
                    dim[4] = s4; mod[4] = md; md *= s4; if (s4 < 0) INIT_ERROR(-5);
                    dim[3] = s3; mod[3] = md; md *= s3; if (s3 < 0) INIT_ERROR(-4);
                    dim[2] = s2; mod[2] = md; md *= s2; if (s2 < 0) INIT_ERROR(-3);
                    dim[1] = s1; mod[1] = md; md *= s1; if (s1 < 0) INIT_ERROR(-2);
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
                case 7:
                    dim[6] = s6; mod[6] = md; md *= s6; if (s6 < 0) INIT_ERROR(-7);
                    dim[5] = s5; mod[5] = md; md *= s5; if (s5 < 0) INIT_ERROR(-6);
                    dim[4] = s4; mod[4] = md; md *= s4; if (s4 < 0) INIT_ERROR(-5);
                    dim[3] = s3; mod[3] = md; md *= s3; if (s3 < 0) INIT_ERROR(-4);
                    dim[2] = s2; mod[2] = md; md *= s2; if (s2 < 0) INIT_ERROR(-3);
                    dim[1] = s1; mod[1] = md; md *= s1; if (s1 < 0) INIT_ERROR(-2);
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
                case 6:
                    dim[5] = s5; mod[5] = md; md *= s5; if (s5 < 0) INIT_ERROR(-6);
                    dim[4] = s4; mod[4] = md; md *= s4; if (s4 < 0) INIT_ERROR(-5);
                    dim[3] = s3; mod[3] = md; md *= s3; if (s3 < 0) INIT_ERROR(-4);
                    dim[2] = s2; mod[2] = md; md *= s2; if (s2 < 0) INIT_ERROR(-3);
                    dim[1] = s1; mod[1] = md; md *= s1; if (s1 < 0) INIT_ERROR(-2);
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
                case 5:
                    dim[4] = s4; mod[4] = md; md *= s4; if (s4 < 0) INIT_ERROR(-5);
                    dim[3] = s3; mod[3] = md; md *= s3; if (s3 < 0) INIT_ERROR(-4);
                    dim[2] = s2; mod[2] = md; md *= s2; if (s2 < 0) INIT_ERROR(-3);
                    dim[1] = s1; mod[1] = md; md *= s1; if (s1 < 0) INIT_ERROR(-2);
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
                case 4:
                    dim[3] = s3; mod[3] = md; md *= s3; if (s3 < 0) INIT_ERROR(-4);
                    dim[2] = s2; mod[2] = md; md *= s2; if (s2 < 0) INIT_ERROR(-3);
                    dim[1] = s1; mod[1] = md; md *= s1; if (s1 < 0) INIT_ERROR(-2);
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
                case 3:
                    dim[2] = s2; mod[2] = md; md *= s2; if (s2 < 0) INIT_ERROR(-3);
                    dim[1] = s1; mod[1] = md; md *= s1; if (s1 < 0) INIT_ERROR(-2);
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
                case 2:
                    dim[1] = s1; mod[1] = md; md *= s1; if (s1 < 0) INIT_ERROR(-2);
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
                case 1:
                    dim[0] = s0; mod[0] = md; if (s0 < 0) INIT_ERROR(-1);
                    break;
            }
        }

        public idxspec(intg o, idxdim d)
        {
            init_spec(o, d.dims[0], d.dims[1], d.dims[2], d.dims[3], d.dims[4],
                d.dims[5], d.dims[6], d.dims[7], (uint)d.order());
        }

        public idxspec(intg o, int n, intg[] ldim, intg[] lmod)
        {
            dim = null;
            mod = null;
            offset = o;
            ndim = -1;
            setndim(n);
            for (int i = 0; i < n; i++)
            {
                if (ldim[i] < 0) throw new Exception("negative dimension");
                dim[i] = ldim[i]; mod[i] = lmod[i];
            }
        }

        public int getoffset() { return offset; }

        public void add_offset(int off) { offset += off; }

        public void setoffset(intg o) { offset = 0; }

        public int getndim() { return ndim; }

        public intg footprint()
        {
            intg r = offset + 1;
            for (int i = 0; i < ndim; i++)
                r += mod[i] * (dim[i] - 1);
            return r;
        }

        public int nelements()
        {
            int r = 1;
            for (int i = 0; i < ndim; i++)
                r *= dim[i];
            return r;
        }

        public bool contiguousp()
        {
            int size = 1; bool r = true;
            for (int i = ndim - 1; i >= 0; i--)
            {
                if (size != mod[i]) r = false;
                size *= dim[i];
            }
            return r;
        }

        public int true_order()
        {
            int order = 0;
            for (int i = 0; i < ndim; i++) if (dim[i] > 1) order = i + 1;
            return order;
        }

        private int setndim(int n)
        {
            if ((n < 0) || (n > Global.MAXDIMS)) throw new Exception("idx: cannot set ndim with n=" + n + " MAXDIMS=" + Global.MAXDIMS);
            if ((n == 0) || (n > ndim))
            //if new ndim is zero or larger than before: deallocate arrays
            {
                if (!(dim == null)) { dim = null; }
                if (!(mod == null)) { mod = null; }
            }
            //now allocate new arrays if necessary
            if (n > 0)
            {
                if (dim == null) { dim = new int[Global.MAXDIMS]; }
                if (mod == null) { mod = new int[Global.MAXDIMS]; }
            }
            // if the arrays allocated and ndim was larger
            // than new ndim, we don't do anything.
            ndim = n;
            return ndim;
        }

        private int setndim(int n, int[] ldim, int[] lmod)
        {
            if ((n < 0) || (n > Global.MAXDIMS)) throw new Exception("idx: cannot set ndim with n=" + n + " MAXDIMS=" + Global.MAXDIMS);
            if (!(dim == null)) { dim = null; }
            if (!(mod == null)) { mod = null; }
            dim = ldim;
            mod = lmod;
            ndim = n;
            return ndim;
        }

        public intg resize(intg s0 = -1, intg s1 = -1, intg s2 = -1, intg s3 = -1, intg s4 = -1, intg s5 = -1, intg s6 = -1, intg s7 = -1)
        {
            intg md = 1;
            // resizeing non-contiguous is forbiden to prevent nasty bugs
            if (!contiguousp())
                throw new Exception("Resizing non-contiguous idx is not allowed");
            if (ndim == 0) { INIT_ERROR(0); }  // can't resize idx0
            if (s7 >= 0)
            {
                if (ndim < 8) INIT_ERROR(8);
                dim[7] = s7; mod[7] = md; md *= s7;
            }
            else { if (ndim > 7) INIT_ERROR(-8); }
            if (s6 >= 0)
            {
                if (ndim < 7) INIT_ERROR(7);
                dim[6] = s6; mod[6] = md; md *= s6;
            }
            else { if (ndim > 6) INIT_ERROR(-7); }
            if (s5 >= 0)
            {
                if (ndim < 6) INIT_ERROR(6);
                dim[5] = s5; mod[5] = md; md *= s5;
            }
            else { if (ndim > 5) INIT_ERROR(-6); }
            if (s4 >= 0)
            {
                if (ndim < 5) INIT_ERROR(5);
                dim[4] = s4; mod[4] = md; md *= s4;
            }
            else { if (ndim > 4) INIT_ERROR(-5); }
            if (s3 >= 0)
            {
                if (ndim < 4) INIT_ERROR(4);
                dim[3] = s3; mod[3] = md; md *= s3;
            }
            else { if (ndim > 3) INIT_ERROR(-4); }
            if (s2 >= 0)
            {
                if (ndim < 3) INIT_ERROR(3);
                dim[2] = s2; mod[2] = md; md *= s2;
            }
            else { if (ndim > 2) INIT_ERROR(-3); }
            if (s1 >= 0)
            {
                if (ndim < 2) INIT_ERROR(2);
                dim[1] = s1; mod[1] = md; md *= s1;
            }
            else { if (ndim > 1) INIT_ERROR(-2); }
            if (s0 >= 0)
            {
                if (ndim < 1) INIT_ERROR(1);
                dim[0] = s0; mod[0] = md; md *= s0;
            }
            else { if (ndim > 0) INIT_ERROR(-1); }
            return md + offset; // return new footprint
        }

        public intg resize(idxdim d)
        {
            return resize(d.dims[0], d.dims[1], d.dims[2], d.dims[3], d.dims[4], d.dims[5], d.dims[6], d.dims[7]);
        }

        public intg resize1(intg dimn, intg size)
        {
            // resizeing non-contiguous is forbiden to prevent nasty bugs
            if (!contiguousp()) throw new Exception("Resizing non-contiguous idx is not allowed");
            if ((dimn >= ndim) || (dimn < 0))
                throw new Exception("idxspec::resize1: cannot resize an unallocated dimension");
            if (size < 0)
                throw new Exception("idxspec::resize1: cannot resize with a negative size");
            // since we know the current spec is valid, no need for error checking,
            // simply assign new dimension and propagate new mods.
            dim[dimn] = size;
            for (int i = dimn - 1; i >= 0; --i)
            {
                mod[i] = dim[i + 1] * mod[i + 1];
            }
            return mod[0] * dim[0] + offset; // return new footprint
        }

        public static bool same_dim(idxspec s1, idxspec s2)
        {
            if (s1.ndim != s2.ndim) return false;
            for (int i = 0; i < s1.ndim; i++) { if (s1.dim[i] != s2.dim[i]) return false; }
            return true;
        }

        private void copy(idxspec src)
        {
            Global.DEBUG_LOW("idxspec::copy: " + id.ToString());
            offset = src.offset;
            // we do not initialize ndim before setndim here because it may already
            // be initialized.
            setndim(src.ndim);
            if (ndim > 0)
            {
                dim = src.dim;
                mod = src.mod;
            }
        }

        public override string ToString()
        {
            int i;
            String o = "";
            o += "  idxspec " + id.ToString() + "\n";
            o += "    ndim= " + ndim + "\n";
            o += "    offset= " + offset + "\n";
            if (ndim > 0)
            {
                o += "    dim=[ ";
                for (i = 0; i < ndim - 1; i++) { o += dim[i] + ", "; }
                o += dim[ndim - 1] + "]\n";
                o += "    mod=[ ";
                for (i = 0; i < ndim - 1; i++) { o += mod[i] + ", "; }
                o += mod[ndim - 1] + "]\n";
            }
            else
            {
                o += "    dim = " + dim[0] + ", mod = " + mod[0] + "\n";
            }
            o += "    footprint= " + footprint() + "\n";
            o += "    contiguous= " + ((contiguousp()) ? "yes" : "no") + "\n";
            return o;
        }

        // select, narrow, unfold, etc
        // Each function has 3 version:
        // 1. XXX_into: which writes the result
        // into an existing idxspec apssed as argument.
        // 2. XXX_inplace: writes into the current idxspec
        // 3. XXX: creates a new idxspec and returns it.

        public intg select_into(ref idxspec dst, int d, intg n)
        {
            if (ndim <= 0)
                throw new Exception("cannot select an empty idx idx that is a scalar ("
                         + this + ")");
            if ((n < 0) || (n >= dim[d]))
                throw new Exception("trying to select layer " + n
                         + " of dimension " + d + " which is of size "
                         + dim[d] + " in idx " + this);
            // this preserves the dim/mod arrays if dst == this
            dst.setndim(ndim - 1);
            dst.offset = offset + (n * mod[d]);
            if (ndim - 1 > 0)
            { // dim and mod don't exist for idx0
                for (int j = 0; j < d; j++)
                {
                    dst.dim[j] = dim[j];
                    dst.mod[j] = mod[j];
                }
                for (int j = d; j < ndim - 1; j++)
                {
                    dst.dim[j] = dim[j + 1];
                    dst.mod[j] = mod[j + 1];
                }
            }
            return n;
        }

        public void select_inplace(int d, intg n) { copy(select(d, n)); }

        public idxspec select(int d, intg n)
        {
            idxspec r = new idxspec();
            select_into(ref r, d, n);
            return r;
        }

        public intg narrow_into(ref idxspec dst, int d, intg s, intg o)
        {
            if (ndim <= 0)
                throw new Exception("cannot narrow a scalar");
            if ((d < 0) || (d >= ndim))
                throw new Exception("narrow: illegal dimension index " + d + " in " + this);
            if ((o < 0) || (s < 1) || (s + o > dim[d]))
                throw new Exception("trying to narrow dimension " + d + " to size " + s
                          + " starting at offset " + o + " in " + this);
            // this preserves the dim/mod arrays if dst == this
            dst.setndim(ndim);
            dst.offset = offset + o * mod[d];
            for (int j = 0; j < ndim; j++)
            {
                dst.dim[j] = dim[j];
                dst.mod[j] = mod[j];
            }
            dst.dim[d] = s;
            return s;
        }

        public void narrow_inplace(int d, intg s, intg o)
        {
            copy(narrow(d, s, o));

        }

        idxspec narrow(int d, intg s, intg o)
        {
            // create new idxspec of order ndim
            idxspec r = new idxspec();
            narrow_into(ref r, d, s, o);
            return r;
        }

        ////////////////////////////////////////////////////////////////
        // transpose

        // tranpose two dimensions into pre-existing idxspec
        public int transpose_into(ref idxspec dst, int d1, int d2)
        {
            if ((d1 < 0) || (d1 >= ndim) || (d2 < 0) || (d2 >= ndim))
                throw new Exception("illegal transpose of dimension " + d1
                         + " to dimension " + d2);
            // this preserves the dim/mod arrays if dst == this
            dst.setndim(ndim);
            dst.offset = offset;
            for (int j = 0; j < ndim; j++)
            {
                dst.dim[j] = dim[j];
                dst.mod[j] = mod[j];
            }
            intg tmp;
            // we do this in case dst = this
            tmp = dim[d1]; dst.dim[d1] = dim[d2]; dst.dim[d2] = tmp;
            tmp = mod[d1]; dst.mod[d1] = mod[d2]; dst.mod[d2] = tmp;
            return ndim;
        }

        // tranpose all dims with a permutation vector
        public int transpose_into(ref idxspec dst, int[] p)
        {
            for (int i = 0; i < ndim; i++)
            {
                if ((p[i] < 0) || (p[i] >= ndim))
                    throw new Exception("illegal transpose of dimensions");
            }
            dst.setndim(ndim);
            dst.offset = offset;
            if (dst == this)
            {
                // we need temp storage if done in place
                intg[] tmpdim = new intg[Global.MAXDIMS];
                intg[] tmpmod = new intg[Global.MAXDIMS];
                for (int j = 0; j < ndim; j++)
                {
                    tmpdim[j] = dim[p[j]];
                    tmpmod[j] = mod[p[j]];
                }
                for (int j = 0; j < ndim; j++)
                {
                    dst.dim[j] = tmpdim[j];
                    dst.mod[j] = tmpmod[j];
                }
            }
            else
            {
                // not in place
                for (int j = 0; j < ndim; j++)
                {
                    dst.dim[j] = dim[p[j]];
                    dst.mod[j] = mod[p[j]];
                }
            }
            return ndim;
        }

        public void transpose_inplace(int d1, int d2)
        {
            copy(transpose(d1, d2));
        }

        public void transpose_inplace(int[] p)
        {
            copy(transpose(p));
        }

        public idxspec transpose(int d1, int d2)
        {
            idxspec r = new idxspec();
            transpose_into(ref r, d1, d2);
            return r;
        }

        public idxspec transpose(int[] p)
        {
            idxspec r = new idxspec();
            transpose_into(ref r, p);
            return r;
        }

        ////////////////////////////////////////////////////////////////
        // unfold

        // d: dimension; k: kernel size; s: stride.
        public intg unfold_into(ref idxspec dst, int d, intg k, intg s)
        {
            intg ns; // size of newly created dimension
            String err = "";
            if (ndim <= 0)
                err += "cannot unfold an idx of maximum order";
            else if ((d < 0) || (d >= ndim))
                err += "unfold: illegal dimension index";
            else if ((k < 1) || (s < 1))
                err += "unfold: kernel and stride must be >= 1";
            ns = 1 + (dim[d] - k) / s;
            if (!String.IsNullOrWhiteSpace(err) && ((ns <= 0) || (dim[d] != s * (ns - 1) + k)))
                err += "unfold: kernel and stride incompatible with size";
            if (!String.IsNullOrWhiteSpace(err))
                throw new Exception(err + ", while unfolding dimension " + d + " to size " + k
                         + " with step " + s + " from idx " + this + " into idx "
                         + dst);
            // this preserves the dim/mod arrays if dst == this
            dst.setndim(ndim + 1);
            dst.offset = offset;
            for (int i = 0; i < ndim; i++)
            {
                dst.dim[i] = dim[i];
                dst.mod[i] = mod[i];
            }
            dst.dim[ndim] = k;
            dst.mod[ndim] = mod[d];
            dst.dim[d] = ns;
            dst.mod[d] = mod[d] * s;
            return ns;
        }

        public void unfold_inplace(int d, intg k, intg s)
        {
            copy(unfold(d, k, s));
        }

        public idxspec unfold(int d, intg k, intg s)
        {
            idxspec r = new idxspec();
            unfold_into(ref r, d, k, s);
            return r;
        }
    }
}
