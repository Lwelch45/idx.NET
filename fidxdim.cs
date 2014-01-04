using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;

namespace Base
{
   public  class fidxdim : idxd<float>
    {
        public fidxdim()
        {
            offsets = new float[Global.MAXDIMS];
            dims = new float[Global.MAXDIMS] { -1, -1, -1, -1, -1, -1, -1, -1 };
            ndim = -1;
        }

        public fidxdim(idxd<dynamic> s) { dims = new float[Global.MAXDIMS] { -1, -1, -1, -1, -1, -1, -1, -1 }; offsets = new float[Global.MAXDIMS]; setdims(s); }

        public fidxdim(idxspec s) { dims = new float[Global.MAXDIMS] { -1, -1, -1, -1, -1, -1, -1, -1 }; offsets = new float[Global.MAXDIMS]; setdims(s); }

        fidxdim(float s0, float s1 = -1, float s2 = -1, float s3 = -1, float s4 = -1, float s5 = -1, float s6 = -1, float s7 = -1)
        {
            offsets = new float[Global.MAXDIMS];
            dims[0] = s0; dims[1] = s1; dims[2] = s2; dims[3] = s3;
            dims[4] = s4; dims[5] = s5; dims[6] = s6; dims[7] = s7;
            ndim = 0;
            for (int i = 0; i < Global.MAXDIMS; i++)
                if (dims[i] >= 0) ndim++;
                else break;
        }

        public void setdims(idxspec s)
        {
            ndim = s.ndim;
            // copy existing dimensions
            for (int i = 0; i < ndim; ++i)
                dims[i] = s.dim[i];
            // set remaining to -1
            int ord = Math.Max(0, s.ndim);
            for (int i = ord; i < Global.MAXDIMS; i++) { dims[i] = -1; }
        }

        public void setdims(idxd<dynamic> s)
        {
            ndim = s.order();
            // copy existing dimensions
            for (int i = 0; i < s.order(); ++i)
                dims[i] = s.dim(i);
            // set remaining to -1
            int ord = Math.Max(0, s.order());
            for (int i = ord; i < Global.MAXDIMS; i++) { dims[i] = -1; }
            if (!(s.offsets == null))
            {
                for (int i = 0; i < s.order(); ++i) offsets[i] = s.offsets[i];
            }
            else { offsets = null; }
        }

        public void setdims(float n)
        {
            for (int i = 0; i < ndim; ++i)
                dims[i] = n;
        }

        public void insert_dim(int pos, float dim_size)
        {
            if (ndim + 1 > Global.MAXDIMS)
                throw new Exception("error: cannot add another dimension to dim."
                       + " Maximum number of dimensions (" + Global.MAXDIMS + ") reached.");
            // check that dim_size is valid
            if (dim_size <= 0)
                throw new Exception("cannot set negative or zero dimension");
            // check that all dimensions up to pos (excluded) are > 0.
            for (uint i = 0; i < pos; ++i)
                if (dims[i] <= 0)
                    throw new Exception("error: cannot insert dimension " + pos
                        + " after empty dimensions: " + id.ToString());
            // add order of 1
            ndim++;
            if (ndim == 0) // one more if it was empty
                ndim++;
            // shift all dimensions until position pos
            for (uint i = (uint)ndim - 1; i > pos && i >= 1; i--)
                dims[i] = dims[i - 1];
            if (!(offsets == null))
                for (uint i = (uint)ndim - 1; i > pos && i >= 1; i--)
                    offsets[i] = offsets[i - 1];
            // insert new dim
            dims[pos] = dim_size;
            if (!(offsets == null))
                offsets[pos] = 0;
        }

        public float remove_dim(int pos)
        {
            // check that dim_size is valid
            if (ndim == 0)
                throw new Exception("not enough dimensions for removing one in " + id.ToString());
            float rdim = dim(pos);
            // shift all dimensions until position pos
            for (uint i = (uint)pos; i < ndim - 1; i++)
                dims[i] = dims[i + 1];
            dims[ndim - 1] = -1; // empty last dimension
            if (!(offsets == null))
            {
                for (uint i = (uint)pos; i < ndim - 1; i++)
                    offsets[i] = offsets[i + 1];
                offsets[ndim - 1] = 0; // empty last offset
            }
            // decrease order by 1
            ndim--;
            return rdim;
        }

        public void setdim(int dimn, float size)
        {
            if (dimn >= ndim)
                throw new Exception("error: trying to set dimension " + dimn
                    + " to size " + size + " but idxidm has only " + ndim
                         + " dimension(s): " + id.ToString());
            dims[dimn] = size;
        }

        public void setoffset(int dimn, float offset)
        {
            if (dimn >= ndim)
                throw new Exception("error: trying to set offset of dim " + dimn
                 + " to " + offset + " but idxidm has only " + ndim
                         + " dimension(s): " + id.ToString());
            // allocate if not allocated
            if (offsets == null)
            {
                offsets = new float[Global.MAXDIMS] { 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            offsets[dimn] = offset;
        }

        public bool has_offsets() { return !(offsets == null); }

        public void set_max(fidxdim other)
        {
            if (other.order() != ndim)
                throw new Exception("expected same order in " + id.ToString()
                                + " and " + other);
            for (uint i = 0; i < ndim; i++)
                dims[i] = Math.Max(dims[i], other.dim((int)i));
        }

        public void shift_dim(int d, int pos)
        {
            float[] dims2 = new float[Global.MAXDIMS];
            for (int i = 0, j = 0; i < Global.MAXDIMS; ++i)
            {
                if (i == pos)
                    dims2[i] = dims[d];
                else
                {
                    if (j == d)
                        j++;
                    dims2[i] = dims[j++];
                }
            }
            dims = dims2;
            if (!(offsets == null))
                throw new Exception("not implemented (TODO)");
        }       

        public bool empty() { return ndim == 1; }

        public float maxdim()
        {
            float m = 0;
            for (int i = 0; i < ndim; ++i)
                if (m < dims[i]) m = dims[i];
            return m;
        }

        public float offset(int dimn)
        {
            if (dimn >= ndim)
                throw new Exception("trying to access size of dimension " + dimn
                          + " but idxdim's maximum dimensions is " + ndim);
            if (!(offsets == null))
                return offsets[dimn];
            else
                return 0;
        }

        public float nelements()
        {
            float total = 1;
            for (int i = 0; i < ndim; ++i) total *= dim(i);
            return total;
        }

        override public string ToString()
        {
            string outstring = "";

            if (order() <= 0)
                outstring += "<empty>";
            else
            {
                if (has_offsets())
                {
                    bool show = false;
                    for (int i = 0; i < order(); ++i)
                        if (offset(i) != 0)
                        {
                            show = true;
                            break;
                        }
                    if (show)
                    {
                        outstring += "(";
                        outstring += offset(0);
                        for (int i = 1; i < order(); ++i)
                            outstring += "," + offset(i);
                        outstring += ")";
                    }
                }
                outstring += dim(0);
                for (int i = 1; i < order(); ++i)
                    outstring += "x" + dim(i);
            }
            return outstring;

        }

    }

}
