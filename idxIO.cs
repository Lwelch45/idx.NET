using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
namespace idx
{
    public static class idxIO
    {
        // standard lush magic numbers
        public const int MAGIC_FLOAT_MATRIX = 0x1e3d4c51;
        public const int MAGIC_PACKED_MATRIX = 0x1e3d4c52;
        public const int MAGIC_DOUBLE_MATRIX = 0x1e3d4c53;
        public const int MAGIC_INTEGER_MATRIX = 0x1e3d4c54;
        public const int MAGIC_BYTE_MATRIX = 0x1e3d4c55;
        public const int MAGIC_SHORT_MATRIX = 0x1e3d4c56;
        public const int MAGIC_SHORT8_MATRIX = 0x1e3d4c57;
        public const int MAGIC_LONG_MATRIX = 0x1e3d4c58;
        public const int MAGIC_ASCII_MATRIX = 0x2e4d4154;

        // non-standard magic numbers
        public const int MAGIC_UINT_MATRIX = 0x1e3d4c59;
        public const int MAGIC_UINT64_MATRIX = 0x1e3d4c5a;
        public const int MAGIC_INT64_MATRIX = 0x1e3d4c5b;

        // pascal vincent's magic numbers
        public const int MAGIC_UBYTE_VINCENT = 0x0800;
        public const int MAGIC_BYTE_VINCENT = 0x0900;
        public const int MAGIC_SHORT_VINCENT = 0x0B00;
        public const int MAGIC_INT_VINCENT = 0x0C00;
        public const int MAGIC_FLOAT_VINCENT = 0x0D00;
        public const int MAGIC_DOUBLE_VINCENT = 0x0E00;



        public static int get_magic<T>()
        {
            if (typeof(T) == typeof(UInt64)) { return MAGIC_UINT64_MATRIX; }
            if (typeof(T) == typeof(uint)) { return MAGIC_UINT_MATRIX; }
            if (typeof(T) == typeof(long)) { return MAGIC_LONG_MATRIX; }
            if (typeof(T) == typeof(double)) { return MAGIC_DOUBLE_MATRIX; }
            if (typeof(T) == typeof(float)) { return MAGIC_FLOAT_MATRIX; }
            if (typeof(T) == typeof(int)) { return MAGIC_INTEGER_MATRIX; }
            if (typeof(T) == typeof(Byte)) { return MAGIC_BYTE_MATRIX; }
            return 0;
        }

        static public bool is_magic_vincent(int magic)
        {
            if (magic == MAGIC_UBYTE_VINCENT
                || magic == MAGIC_BYTE_VINCENT
                || magic == MAGIC_SHORT_VINCENT
                || magic == MAGIC_INT_VINCENT
                || magic == MAGIC_FLOAT_VINCENT
                || magic == MAGIC_DOUBLE_VINCENT)
                return true;
            return false;
        }

        static public bool is_magic(int magic)
        {
            if (magic == MAGIC_FLOAT_MATRIX
                || magic == MAGIC_PACKED_MATRIX
                || magic == MAGIC_DOUBLE_MATRIX
                || magic == MAGIC_INTEGER_MATRIX
                || magic == MAGIC_BYTE_MATRIX
                || magic == MAGIC_SHORT_MATRIX
                || magic == MAGIC_SHORT8_MATRIX
                || magic == MAGIC_LONG_MATRIX
                || magic == MAGIC_ASCII_MATRIX
                || magic == MAGIC_UINT_MATRIX
                || magic == MAGIC_UINT64_MATRIX
                || magic == MAGIC_INT64_MATRIX)
                return true;
            return false;
        }

        static public bool is_matrix(string name)
        {
            try
            {
                FILE fp = new FILE(name);
                if (fp == null) { Global.eblerror("failed to open " + name); }
                int magic = 0;
                read_matrix_header(ref fp, ref magic);
            }
            catch (Exception ex)
            {
                Global.eblerror(ex.ToString());
                return false;
            }

            return true;
        }

        static public string get_magic_str(int magic)
        {
            switch (magic)
            {
                // standard format
                case MAGIC_BYTE_MATRIX: return "ubyte";
                case MAGIC_PACKED_MATRIX: return "packed";
                case MAGIC_SHORT_MATRIX: return "short";
                case MAGIC_SHORT8_MATRIX: return "short8";
                case MAGIC_ASCII_MATRIX: return "ascii";
                case MAGIC_INTEGER_MATRIX: return "int";
                case MAGIC_FLOAT_MATRIX: return "float";
                case MAGIC_DOUBLE_MATRIX: return "double";
                case MAGIC_LONG_MATRIX: return "long";
                // non standard
                case MAGIC_UINT_MATRIX: return "uint";
                case MAGIC_UINT64_MATRIX: return "uint64";
                case MAGIC_INT64_MATRIX: return "int64";
                // pascal vincent format
                case MAGIC_BYTE_VINCENT: return "byte (pascal vincent)";
                case MAGIC_UBYTE_VINCENT: return "ubyte (pascal vincent)";
                case MAGIC_SHORT_VINCENT: return "short (pascal vincent)";
                case MAGIC_INT_VINCENT: return "int (pascal vincent)";
                case MAGIC_FLOAT_VINCENT: return "float (pascal vincent)";
                case MAGIC_DOUBLE_VINCENT: return "double (pascal vincent)";
                default:
                    string s = "";
                    s += "unknown type (magic: " + magic + ")";
                    return s;
            }
        }

        public static idxdim get_matrix_dims(string name)
        {
            //open file
            FILE fp = new FILE(name);
            if (fp == null) { Global.eblerror("failed to open " + name); }
            //read it
            int magic = 0;
            idxdim d = read_matrix_header(ref fp, ref magic);
            fp.fclose();
            return d;
        }

        static public idxdim read_matrix_header(ref FILE fp, ref int magic)
        {
            int ndim = 0, v = 0;
            int ndim_min = 3; // std header requires at least 3 dims even empty ones.
            idxdim dims = new idxdim();

            //read magic number

            if (fp.fread(ref magic) != 1)
            {
                fp.fclose();
                Global.eblerror("cannot read magic number");
            }

            if (is_magic(magic))
            {// regular magic numnber, read next number
                if (fp.fread(ref ndim) != 1)
                {
                    fp.fclose();
                    Global.eblerror("cannot read number of dimensions");
                }
                //check number is valid
                if (ndim > Global.MAXDIMS)
                {
                    fp.fclose();
                    Global.eblerror("too many dimensions: " + ndim + "(MAXDIMS = " + Global.MAXDIMS + ").");
                }
            }
            else
            { // unknown magic
                fp.fclose();
                Global.eblerror("unknown magic number: " + magic);

            }

            //read each dimension
            for (int i = 0; (i < ndim) || (i < ndim_min); ++i)
            {
                if (fp.fread(ref v) != 1)
                {
                    fp.fclose();
                    Global.eblerror("failed to read matrix dimensions");
                }
                if (i < ndim)
                { // ndim may be less than ndim_min
                    if (v <= 0)
                    { // check that dimension is valid
                        fp.fclose();
                        Global.eblerror("dimension is negative or zero");
                    }
                    dims.insert_dim(i, v); // insert dimension
                }
            }
            return dims;
        }

        static public void read_matrix_body<T>(ref FILE fp, ref idx<T> m)
        {
            int read_count;
            var fp_0 = fp;
            loops.idx_aloop1(m, (i) =>
            {
                T ii = i.item;
                read_count = fp_0.fread(ref ii); i.item = ii;
                if (read_count != 1)
                    Global.eblerror("Read incorrect number of bytes ");
            });

        }

        static public void read_cast_matrix<T, T2>(ref FILE fp, ref idx<T2> Out)
        {
            idx<T> m = new idx<T>(Out.get_idxdim());
            read_matrix_body(ref fp, ref m);
            idxops.idx_copy(m, Out);
        }

        public static idx<T> load_matrix<T>(ref FILE fp, idx<T> out_)
        {
            int magic = 0;
            idxdim dims = read_matrix_header(ref fp, ref magic);
            idx<T> Out = new idx<T>();
            if (out_ == null)
            {
                Out = new idx<T>(dims);
            }
            else
            {
                Out.RecCopy(out_);
            }

            if (Out.get_idxdim() != dims)
            { // different order/dimensions
                // if order is different, it's from the input matrix, error
                if (Out.order() != dims.order())
                    Global.eblerror("error: different orders: " + Out + " " + dims);
                // resize output idx
                Out.resize(dims);
            }

            if ((magic == get_magic<T>()))
            {
                // read
                read_matrix_body(ref fp, ref Out);
            }
            else
            { // different type, read original type, then copy/cast into out
                switch (magic)
                {
                    case MAGIC_BYTE_MATRIX:
                    case MAGIC_UBYTE_VINCENT:
                        read_cast_matrix<Byte, T>(ref fp, ref Out);
                        break;
                    case MAGIC_INTEGER_MATRIX:
                    case MAGIC_INT_VINCENT:
                        read_cast_matrix<int, T>(ref fp, ref Out);
                        break;
                    case MAGIC_FLOAT_MATRIX:
                    case MAGIC_FLOAT_VINCENT:
                        read_cast_matrix<float, T>(ref fp, ref Out);
                        break;
                    case MAGIC_DOUBLE_MATRIX:
                    case MAGIC_DOUBLE_VINCENT:
                        read_cast_matrix<double, T>(ref fp, ref Out);
                        break;
                    case MAGIC_LONG_MATRIX:
                        read_cast_matrix<long, T>(ref fp, ref Out);
                        break;
                    case MAGIC_UINT_MATRIX:
                        read_cast_matrix<uint, T>(ref fp, ref Out);
                        break;
                    case MAGIC_UINT64_MATRIX:
                        read_cast_matrix<UInt64, T>(ref fp, ref Out);
                        break;
                    case MAGIC_INT64_MATRIX:
                        read_cast_matrix<Int64, T>(ref fp, ref Out);
                        break;
                    default:
                        Global.eblerror("unknown magic number");
                        break;
                }
            }
            return Out;

        }

        public static idx<T> load_matrix<T>(ref FILE fp)
        {
            int magic = 0;
            idxdim dims = read_matrix_header(ref fp, ref magic);
            idx<T> Out = new idx<T>();

            Out = new idx<T>(dims);


            if (Out.get_idxdim() != dims)
            { // different order/dimensions
                // if order is different, it's from the input matrix, error
                if (Out.order() != dims.order())
                    Global.eblerror("error: different orders: " + Out + " " + dims);
                // resize output idx
                Out.resize(dims);
            }

            if ((magic == get_magic<T>()))
            {
                // read
                read_matrix_body(ref fp, ref Out);
            }
            else
            { // different type, read original type, then copy/cast into out
                switch (magic)
                {
                    case MAGIC_BYTE_MATRIX:
                    case MAGIC_UBYTE_VINCENT:
                        read_cast_matrix<Byte, T>(ref fp, ref Out);
                        break;
                    case MAGIC_INTEGER_MATRIX:
                    case MAGIC_INT_VINCENT:
                        read_cast_matrix<int, T>(ref fp, ref Out);
                        break;
                    case MAGIC_FLOAT_MATRIX:
                    case MAGIC_FLOAT_VINCENT:
                        read_cast_matrix<float, T>(ref fp, ref Out);
                        break;
                    case MAGIC_DOUBLE_MATRIX:
                    case MAGIC_DOUBLE_VINCENT:
                        read_cast_matrix<double, T>(ref fp, ref Out);
                        break;
                    case MAGIC_LONG_MATRIX:
                        read_cast_matrix<long, T>(ref fp, ref Out);
                        break;
                    case MAGIC_UINT_MATRIX:
                        read_cast_matrix<uint, T>(ref fp, ref Out);
                        break;
                    case MAGIC_UINT64_MATRIX:
                        read_cast_matrix<UInt64, T>(ref fp, ref Out);
                        break;
                    case MAGIC_INT64_MATRIX:
                        read_cast_matrix<Int64, T>(ref fp, ref Out);
                        break;
                    default:
                        Global.eblerror("unknown magic number");
                        break;
                }
            }
            return Out;

        }

        public static idx<T> load_matrix<T>(String filename)
        {
            FILE fp = new FILE(filename);
            if (fp == null) { Global.eblerror("load_matrix failed to open " + filename); }
            idx<T> m = new idx<T>();
            try
            {
                m = load_matrix<T>(ref fp);
                fp.fclose();
            }
            catch (System.Exception ex)
            {
                Global.Print(ex.ToString());
                Global.eblerror("while loading " + filename);
            }
            return m;
        }

        public static bool save_matrix<T>(idx<T> m, FILE fp)
        {
            int v;
            v = get_magic<T>();
            if (fp.fwrite(v) != 1) return false;
            v = m.order();
            if (fp.fwrite(v) != 1) return false;
            for (int i = 0; (i < m.order()) || (i < 3); ++i)
            {
                if (i < m.order())
                    v = m.dim(i);
                else
                    v = 1;
                if (fp.fwrite(v) != 1) return false;
            }
            loops.idx_aloop1(m, (i) =>  fp.fwrite(i.item));
            return true;
        }

    }
}
