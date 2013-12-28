using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using PointerFactory;

namespace idx
{

#warning What if a user wants to create a new idx with no affiliation to this one?

    using intg = System.Int32;

    public class idx<T> : IDisposable
    {

        #region idx memory methods

        public void growstorage()
        {
            if (srgptr.I.growsize(spec.footprint()) < 0)
                throw new Exception("cannot grow storage to " + spec.footprint()
                                     + " size (probably out of memory)");
        }

        private void growstorage_chunk(intg s_chunk)
        {
            if (srgptr.I.growsize_chunk(spec.footprint(), s_chunk) < 0)
                throw new Exception("cannot grow storage to " + spec.footprint()
                                     + " size (probably out of memory)");
        }

        #endregion

        #region idx basic constructors/destructor

        public void Dispose()
        {
            Global.DEBUG_LOW("idx::destructor " + id.ToString());
            srgptr.Dispose();
            if (!(this.pidxdim == null)) { pidxdim.Dispose(); }
        }

        public idx(bool dummy)
        {
            spec.dim = null;
            spec.mod = null;
            srgptr = null;
            pidxdim = null;
        }

        public idx(idx<T> other)
        {
            spec = new idxspec(other.spec);
            srgptr = other.srgptr.Addreference(); //storage.lock
            pidxdim = null;
        }

        #endregion

        #region constructors initialized with an array

        public idx(T[] mat, intg s0, intg s1)
        {
            pidxdim = null;
            spec = new idxspec(0, s0, s1);
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
            srgptr.I.Set(srgptr.I.GetData());
        }

        public idx(T[] mat, intg s0, intg s1, intg s2)
        {
            pidxdim = null;
            spec = new idxspec(0, s0, s1, s2);
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
            srgptr.I.Set(srgptr.I.GetData());
        }

        #endregion

        #region specific constructors for each number of dimensions

        public idx()
        {
            spec = new idxspec(0);
            pidxdim = null;
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
        }

        public idx(intg size0)
        {
            spec = new idxspec(0, size0);
            pidxdim = null;
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
        }

        public idx(intg size0, intg size1)
        {
            spec = new idxspec(0, size0, size1);
            pidxdim = null;
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
        }

        public idx(intg size0, intg size1, intg size2)
        {
            spec = new idxspec(0, size0, size1, size2);
            pidxdim = null;
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
        }

        public idx(intg s0, intg s1, intg s2, intg s3, intg s4 = -1,
            intg s5 = -1, intg s6 = -1, intg s7 = -1)
        {
            spec = new idxspec(0, s0, s1, s2, s3, s4, s5, s6, s7);
            pidxdim = null;
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
        }

        public idx(idxdim d)
        {
            spec = new idxspec(0, d);
            pidxdim = null;
            srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();
        }

        #endregion

        #region constructors from existing srg and offset

        public idx(Ptr<srg<T>> sg, idxspec s)
        {
            pidxdim = null;
            spec = s;
            if (!(sg == null))
                srgptr = sg.Addreference();
            else
                srgptr = new Ptr<srg<T>>(new srg<T>());
            growstorage();

        }

        public idx(Ptr<srg<T>> sg, intg o, intg n, int[] dims, intg[] mods)
        {
            pidxdim = null;

            if (!(sg == null))
            {
                spec = new idxspec(o, n, dims, mods);
                srgptr = sg.Addreference();
            }
            else
                spec = new idxspec(0, n, dims, mods);
            srgptr = new Ptr<srg<T>>(new srg<T>());

            growstorage();

        }

        public idx(Ptr<srg<T>> sg, intg o)
        {
            pidxdim = null;

            if (!(sg == null))
            {
                spec = new idxspec(o);
                srgptr = sg.Addreference();
            }
            else
            {
                spec = new idxspec(0);
                srgptr = new Ptr<srg<T>>(new srg<T>());
            }

            growstorage();

        }

        public idx(Ptr<srg<T>> sg, intg o, intg size0)
        {
            pidxdim = null;

            if (!(sg == null))
            {
                spec = new idxspec(o, size0);
                srgptr = sg.Addreference();
            }
            else
            {
                spec = new idxspec(0, size0);
                srgptr = new Ptr<srg<T>>(new srg<T>());
            }

            growstorage();

        }

        public idx(Ptr<srg<T>> sg, intg o, intg size0, intg size1)
        {
            pidxdim = null;

            if (!(sg == null))
            {
                spec = new idxspec(o, size0, size1);
                srgptr = sg.Addreference();
            }
            else
            {
                spec = new idxspec(0, size0, size1);
                srgptr = new Ptr<srg<T>>(new srg<T>());
            }

            growstorage();

        }

        public idx(Ptr<srg<T>> sg, intg o, intg size0, intg size1, intg size2)
        {
            pidxdim = null;

            if (!(sg == null))
            {
                spec = new idxspec(o, size0, size1, size2);
                srgptr = sg.Addreference();
            }
            else
            {
                spec = new idxspec(0, size0, size1, size2);
                srgptr = new Ptr<srg<T>>(new srg<T>());
            }

            growstorage();

        }

        public idx(Ptr<srg<T>> sg, intg o, intg s0, intg s1, intg s2,
            intg s3, intg s4 = -1, intg s5 = -1, intg s6 = -1, intg s7 = -1)
        {
            pidxdim = null;

            if (!(sg == null))
            {
                spec = new idxspec(o, s0, s1, s2, s3, s4, s5, s6, s7);
                srgptr = sg.Addreference();
            }
            else
            {
                spec = new idxspec(0, s0, s1, s2, s3, s4, s5, s6, s7);
                srgptr = new Ptr<srg<T>>(new srg<T>());
            }

            growstorage();

        }

        public idx(Ptr<srg<T>> sg, intg o, idxdim d)
        {
            pidxdim = null;

            if (!(sg == null))
            {
                spec = new idxspec(o, d);
                srgptr = sg.Addreference();
            }
            else
            {
                spec = new idxspec(0, d);
                srgptr = new Ptr<srg<T>>(new srg<T>());
            }

            growstorage();

        }

        #endregion

        #region Operators

        public void RecCopy(idx<T> other)
        {
            Ptr<srg<T>> tmp = null;
            if (this.srgptr != null)
                tmp = this.srgptr;
            this.srgptr = other.srgptr.Addreference();
            this.spec = other.spec;
            if (tmp != null) tmp.Dispose();
            if (other.pidxdim != null) this.pidxdim = new Ptr<idxdim>(new idxdim(((idxd<int>)other.pidxdim.I)));
            else this.pidxdim = null;
        }

        public static bool CheckEqual( idx<T> a, idx<T> b)
        {
            //if (a.spec.dim != b.spec.dim) { return false; }
            if (a.spec.ndim != b.spec.ndim) { return false; }
            if (a.spec.mod != b.spec.mod) { return false; }
            return true;
        }

        public idx<T> this[int i]
        {
            get
            {
                return select(0, i);
            }
        }  

        #endregion

        #region resize methods

        public intg setoffset(intg o)
        {
            if (o < 0) { throw new Exception("idx::setoffset: offset must be positive"); }
            if (o > spec.getoffset())
            {
                spec.setoffset(o);
                growstorage();
                return o;
            }
            else
            {
                spec.setoffset(o);
                return o;
            }
        }

        public void add_offset(intg o) { spec.add_offset(o); }

        public virtual void resize(intg s0 = -1, intg s1 = -1, intg s2 = -1, intg s3 = -1,
              intg s4 = -1, intg s5 = -1, intg s6 = -1, intg s7 = -1)
        {
            if (!same_dim(s0, s1, s2, s3, s4, s5, s6, s7))
            { // save some time
                spec.resize(s0, s1, s2, s3, s4, s5, s6, s7);
                growstorage();
            }
        }

        public virtual void resize(idxdim d)
        {
            if (d.order() > spec.ndim)
                throw new Exception("cannot change order of idx in resize while trying to resize "
                         + " from " + this + " to " + d);
            if (!same_dim(d))
            { // save some time if dims are same
                spec.resize(d);
                growstorage();
            }
        }

        public virtual void resize1(intg dimn, intg size)
        {
            if (dimn > spec.ndim) throw new Exception("cannot change order of idx in resize");
            if (spec.dim[dimn] != size)
            {
                spec.resize1(dimn, size);
                growstorage();
            }
        }

        public void resize_chunk(intg s_chunk, intg s0, intg s1, intg s2, intg s3,
                          intg s4, intg s5, intg s6, intg s7)
        {
            spec.resize(s0, s1, s2, s3, s4, s5, s6, s7);
            growstorage_chunk(s_chunk);
        }

        public bool same_dim(idxdim d)
        {
            if (spec.ndim != d.order())
                return false;
            for (int i = 0; i < spec.ndim; ++i)
                if (spec.dim[i] != d.dim(i))
                    return false;
            return true;
        }

        public static bool same_dim(idx<T> a, idx<T> b)
        {
            if (a.spec.ndim != b.order())
                return false;
            for (int i = 0; i < a.spec.ndim; ++i)
                if (a.spec.dim[i] != b.spec.dim[i])
                    return false;
            return true;
        }

        #endregion

        #region idx manipulation methods

        public idx<T> select(int d, intg i)
        {
            idx<T> r = new idx<T>(srgptr, spec.getoffset());
            spec.select_into(ref r.spec, d, i);
            return r;
        }

        public idx<T> narrow(int d, intg s, intg o)
        {
            idx<T> r = new idx<T>(srgptr, spec.getoffset());
            spec.narrow_into(ref r.spec, d, s, o);
            return r;
        }

        public idx<T> transpose(int d1, int d2)
        {
            idx<T> r = new idx<T>(srgptr, spec.getoffset());
            spec.transpose_into(ref r.spec, d1, d2);
            return r;
        }

        public idx<T> transpose(int[] p)
        {
            idx<T> r = new idx<T>(srgptr, spec.getoffset());
            spec.transpose_into(ref r.spec, p);
            return r;
        }

        public idx<T> unfold(int d, intg k, intg s)
        {
            idx<T> r = new idx<T>(srgptr, spec.getoffset());
            spec.unfold_into(ref r.spec, d, k, s);
            return r;
        }

        public idx<T> view_as_order(int n)
        {
            if (n < 0)
            {
                throw new Exception("view_as_order: input dimension must be positive");
                return this;
            }
            if (n == spec.ndim)
                return this;
            else
            {
                if ((n == 1) && (spec.ndim == 1))
                {
                    // the order is already 1, do nothing and return current idx.
                    return new idx<T>(this);
                }
                else if (n == 1)
                {
                    // the order is not 1, check that data is contiguous and return
                    // a 1D idx.
                    loops.CHECK_CONTIGUOUS1(this);
                    idx<T> r = new idx<T>(getstorage(), 0, spec.nelements());
                    return r;
                }
                else if (n > spec.ndim)
                {
                    intg[] ldim = new intg[n];
                    intg[] lmod = new intg[n];
                    ldim = spec.dim;
                    lmod = spec.mod;
                    for (int i = spec.ndim; i < n; ++i)
                    {
                        ldim[i] = 1;
                        lmod[i] = 1;
                    }
                    idx<T> r = new idx<T>(getstorage(), spec.getoffset(), n, ldim, lmod);
                    if (!(ldim == null)) ldim = null;
                    if ((lmod == null)) lmod = null;
                    return r;
                }
                else
                {
                    throw new Exception("view_as_order is not defined when n < current order");
                    return new idx<T>(this);
                }
            }
        }

        public idx<T> flat()
        {
            return view_as_order(1);
        }

        public idx<T> shift_dim(int d, int pos)
        {
            int[] tr = new int[Global.MAXDIMS];
            for (int i = 0, j = 0; i < spec.ndim; ++i)
            {
                if (i == pos)
                    tr[i] = d;
                else
                {
                    if (j == d)
                        j++;
                    tr[i] = j++;
                }
            }
            return transpose(tr);
        }

        #endregion

        #region field access methods

        public Ptr<srg<T>> getstorage() { return srgptr; }

        public intg dim(int d) { return spec.dim[d]; }

        public intg[] dims() { return spec.dim; }

        public intg mod(int d) { return spec.mod[d]; }

        public intg[] mods() { return spec.mod; }

        public int order() { return spec.ndim; }

        public intg true_order() { return spec.true_order(); }

        public intg offset() { return spec.getoffset(); }

        public intg nelements() { return spec.nelements(); }

        public intg footprint() { return spec.footprint(); }

        public bool contiguousp() { return spec.contiguousp(); }

        public bool same_dim(intg s0, intg s1, intg s2, intg s3, intg s4, intg s5,
                     intg s6, intg s7)
        {
            if ((s7 >= 0) && (spec.ndim < 8)) return false;
            if ((spec.ndim == 8) && (s7 != spec.dim[7])) return false;
            if ((s6 >= 0) && (spec.ndim < 7)) return false;
            if ((spec.ndim >= 7) && (s6 != spec.dim[6])) return false;
            if ((s5 >= 0) && (spec.ndim < 6)) return false;
            if ((spec.ndim >= 6) && (s5 != spec.dim[5])) return false;
            if ((s4 >= 0) && (spec.ndim < 5)) return false;
            if ((spec.ndim >= 5) && (s4 != spec.dim[4])) return false;
            if ((s3 >= 0) && (spec.ndim < 4)) return false;
            if ((spec.ndim >= 4) && (s3 != spec.dim[3])) return false;
            if ((s2 >= 0) && (spec.ndim < 3)) return false;
            if ((spec.ndim >= 3) && (s2 != spec.dim[2])) return false;
            if ((s1 >= 0) && (spec.ndim < 2)) return false;
            if ((spec.ndim >= 2) && (s1 != spec.dim[1])) return false;
            if ((s0 >= 0) && (spec.ndim < 1)) return false;
            if ((spec.ndim >= 1) && (s0 != spec.dim[0])) return false;
            return true;
        }

        public idxdim get_idxdim() { idxdim d = new idxdim(); d.setdims(spec); return d; }
        #endregion

        #region data access methods

        public eptr<T> idx_ptr()
        {
            return new eptr<T>(srgptr, spec.getoffset());
        }

        public int[] mod_ptr()
        {
            return spec.mod;
        }

        #endregion

        #region pointer access methods

        public eptr<T> ptr()
        {
            if (spec.ndim != 0) Global.eblerror("not an idx0");
            return new eptr<T>(srgptr, spec.getoffset());
        }

        public eptr<T> ptr(intg i0)
        {
            loops.idx_checkorder1(this, 1);
            if ((i0 < 0) || (i0 >= spec.dim[0])) Global.eblerror("index 0 out of bound");
            return new eptr<T>(srgptr, spec.getoffset());
        }

        public eptr<T> ptr(intg i0, intg i1)
        {
            loops.idx_checkorder1(this, 2);
            if ((i0 < 0) || (i0 >= spec.dim[0])) Global.eblerror("index 0 out of bound");
            if ((i1 < 0) || (i1 >= spec.dim[1])) Global.eblerror("index 1 out of bound");
            return new eptr<T>(srgptr, spec.getoffset() + i0 * spec.mod[0] + i1 * spec.mod[1]);
        }

        public eptr<T> ptr(intg i0, intg i1, intg i2)
        {
            loops.idx_checkorder1(this, 3);
            if ((i0 < 0) || (i0 >= spec.dim[0])) Global.eblerror("index 0 out of bound");
            if ((i1 < 0) || (i1 >= spec.dim[1])) Global.eblerror("index 1 out of bound");
            if ((i2 < 0) || (i2 >= spec.dim[2])) Global.eblerror("index 2 out of bound");
            return new eptr<T>(srgptr, spec.getoffset() + i0 * spec.mod[0] + i1 * spec.mod[1]
      + i2 * spec.mod[2]);
        }

        public static void PTR_ERROR(int v)
        {
            Global.eblerror("idx::get: (error " + v
                + "wrong number of indices, negative or out of bound index");
        }

        public eptr<T> ptr(intg i0, intg i1, intg i2, intg i3, intg i4 = -1, intg i5 = -1,
                 intg i6 = -1, intg i7 = -1)
        {
            // check that we passed the right number of indices
            // and that they are all positive
            switch (spec.ndim)
            {
                case 8:
                    if (i7 < 0) PTR_ERROR(-8); break;
                case 7:
                    if ((i6 < 0) || (i7 != -1)) PTR_ERROR(-7); break;
                case 6:
                    if ((i5 < 0) || (i6 != -1)) PTR_ERROR(-6); break;
                case 5:
                    if ((i4 < 0) || (i5 != -1)) PTR_ERROR(-5); break;
                case 4:
                    if ((i3 < 0) || (i2 < 0) || (i1 < 0) || (i0 < 0) || (i4 != -1)) PTR_ERROR(-4); break;
                default:
                    Global.eblerror("idx::get: number of indices and order are different"); break;
            }
            intg k = 0;
            switch (spec.ndim)
            {
                case 8:
                    k += spec.mod[7] * i7; if (i7 >= spec.dim[7]) PTR_ERROR(7);
                    k += spec.mod[6] * i6; if (i6 >= spec.dim[6]) PTR_ERROR(6);
                    k += spec.mod[5] * i5; if (i5 >= spec.dim[5]) PTR_ERROR(5);
                    k += spec.mod[4] * i4; if (i4 >= spec.dim[4]) PTR_ERROR(4);
                    k += spec.mod[3] * i3; if (i3 >= spec.dim[3]) PTR_ERROR(3);
                    break;
                case 7:
                    k += spec.mod[6] * i6; if (i6 >= spec.dim[6]) PTR_ERROR(6);
                    k += spec.mod[5] * i5; if (i5 >= spec.dim[5]) PTR_ERROR(5);
                    k += spec.mod[4] * i4; if (i4 >= spec.dim[4]) PTR_ERROR(4);
                    k += spec.mod[3] * i3; if (i3 >= spec.dim[3]) PTR_ERROR(3);
                    break;
                case 6:
                    k += spec.mod[5] * i5; if (i5 >= spec.dim[5]) PTR_ERROR(5);
                    k += spec.mod[4] * i4; if (i4 >= spec.dim[4]) PTR_ERROR(4);
                    k += spec.mod[3] * i3; if (i3 >= spec.dim[3]) PTR_ERROR(3);
                    break;
                case 5:
                    k += spec.mod[4] * i4; if (i4 >= spec.dim[4]) PTR_ERROR(4);
                    k += spec.mod[3] * i3; if (i3 >= spec.dim[3]) PTR_ERROR(3);
                    break;
                case 4:
                    k += spec.mod[3] * i3; if (i3 >= spec.dim[3]) PTR_ERROR(3);
                    break;
            }
            k += spec.mod[2] * i2; if (i2 >= spec.dim[2]) PTR_ERROR(2);
            k += spec.mod[1] * i1; if (i1 >= spec.dim[1]) PTR_ERROR(1);
            k += spec.mod[0] * i0; if (i0 >= spec.dim[0]) PTR_ERROR(0);
            return new eptr<T>(srgptr, spec.getoffset() + k);
        }
        #endregion

        #region get methods

        public T get()
        {
            if (Global.DEBUG) { loops.idx_checkorder1(this, 0); }
            return srgptr.I.Get(spec.getoffset());

        }

        public T get(intg i0)
        {
            if (Global.DEBUG)
            {
                loops.idx_checkorder1(this, 1);
                if ((i0 < 0) || (i0 >= spec.dim[0]))
                {
                    Global.eblerror("error accessing elt " + i0 + " in " + this + ", index out of bound");
                }
                return srgptr.I.Get(spec.getoffset() + i0 * spec.mod[0]);
            }
            return srgptr.I.Get(spec.getoffset() + i0 * spec.mod[0]);
        }

        public T get(intg i0, intg i1)
        {
            if (Global.DEBUG)
            {
                loops.idx_checkorder1(this, 2);
                if (((i0 < 0) || (i0 >= spec.dim[0])) || ((i1 < 0) || (i1 >= spec.dim[1])))
                {
                    Global.eblerror("error accessing elt " + i0 + "x" + i1 + " in " + this + ", index out of bound");
                }
                return srgptr.I.Get(spec.getoffset() + i0 * spec.mod[0] + i1 * spec.mod[1]);
            }
            return srgptr.I.Get(spec.getoffset() + i0 * spec.mod[0]);
        }

        public T get(intg i0, intg i1, intg i2)
        {
            if (Global.DEBUG)
            {
                loops.idx_checkorder1(this, 3);
                if (((i0 < 0) || (i0 >= spec.dim[0])) || ((i1 < 0) || (i1 >= spec.dim[1])) || ((i2 < 0) || (i2 >= spec.dim[2])))
                {
                    Global.eblerror("error accessing elt " + i0 + "x" + i1 + "x" + i2 + " in " + this + ", index out of bound");
                }
                return srgptr.I.Get(spec.getoffset() + i0 * spec.mod[0] + i1 * spec.mod[1] + i2 * spec.mod[2]);
            }
            return srgptr.I.Get(spec.getoffset() + i0 * spec.mod[0]);
        }

        public T get(intg i0, intg i1, intg i2, intg i3, intg i4 = -1,
                intg i5 = -1, intg i6 = -1, intg i7 = -1) { return ptr(i0, i1, i2, i3, i4, i5, i6, i7).item; }

        public T gget(intg i0 = 0, intg i1 = 0, intg i2 = 0, intg i3 = 0, intg i4 = 0,
                 intg i5 = 0, intg i6 = 0, intg i7 = 0)
        {
            switch (spec.ndim)
            {
                case 7: i7 = -1; break;
                case 6: i6 = -1; i7 = -1; break;
                case 5: i5 = -1; i6 = -1; i7 = -1; break;
                case 4: i4 = -1; i5 = -1; i6 = -1; i7 = -1; break;
                case 3: return get(i0, i1, i2);
                case 2: return get(i0, i1);
                case 1: return get(i0);
                case 0: return get();
                default: break;
            }
            return ptr(i0, i1, i2, i3, i4, i5, i6, i7).item;
        }
        #endregion

        #region set methods

        public void set(T val)
        {
            if (Global.DEBUG) { loops.idx_checkorder1(this, 0); }
            srgptr.I.Set(spec.getoffset(),val);
        }

        public void set(T val, intg i0) {
            if (Global.DEBUG) { loops.idx_checkorder1(this, 1); }
            srgptr.I.Set(spec.getoffset() + i0 * spec.mod[0], val);
        }

        public void set(T val, intg i0 ,intg i1) {
            if (Global.DEBUG) { loops.idx_checkorder1(this, 2); }
            srgptr.I.Set(spec.getoffset() + i0 * spec.mod[0] + i1 * spec.mod[1], val);
        }

        public void set(T val, intg i0, intg i1, intg i2)
        {
            if (Global.DEBUG) { loops.idx_checkorder1(this, 3); }
            srgptr.I.Set(spec.getoffset() + i0 * spec.mod[0] + i1 * spec.mod[1] + i2 * spec.mod[2], val);
        }

        public void set(T val, intg i0, intg i1, intg i2, intg i3, intg i4 = -1,
                intg i5 = -1, intg i6 = -1, intg i7 = -1) { ptr(i0, i1, i2, i3, i4, i5, i6, i7).item = val; }

        public T gget(T val, intg i0 = 0, intg i1 = 0, intg i2 = 0, intg i3 = 0, intg i4 = 0,
                 intg i5 = 0, intg i6 = 0, intg i7 = 0)
        {
            switch (spec.ndim)
            {
                case 7: i7 = -1; break;
                case 6: i6 = -1; i7 = -1; break;
                case 5: i5 = -1; i6 = -1; i7 = -1; break;
                case 4: i4 = -1; i5 = -1; i6 = -1; i7 = -1; break;
                case 3: set(val, i0, i1, i2); break;
                case 2: set(val, i0, i1); break;
                case 1: set(val, i0); break;
                case 0: set(val); break;
                default: break;
            }
            return ptr(i0, i1, i2, i3, i4, i5, i6, i7).item = val;
        }
        #endregion

        override public String ToString() { 
            String o = "";
            o += "idx: with id " + id.ToString() + "\n";
            o += "storage= " + srgptr.ID.ToString() + "(size=" + srgptr.I.size() + ") \n";
            o += spec.ToString();
            return o; }

        public string info()
        {
            string s = "";
            s += "(" + spec;
            if (spec.ndim == 0)
            { // scalar case
                s += ": " + get();
            }
            else
            { // tensor
                s += " min " + idxops.idx_min(this);
                s += " max " + idxops.idx_max(this);
            }
            s += ")";
            return s;
        }

        public Ptr<srg<T>> srgptr { get; set; }
        public Ptr<idxdim> pidxdim { get; set; }
        public idxspec spec;
        Guid id = Guid.NewGuid();
    }
}
