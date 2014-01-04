using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace idx
{
    public class FILE : IDisposable
    {
        FileStream fs;
        //int offset = 0;

        public FILE(string filename)
        {
            fs = new FileStream(filename, FileMode.OpenOrCreate);
        }

        #region fread

        public int fread<T>(ref T obj) where T : struct
        {
            try
            {
                obj = FILEExt.ReadValue<T>(ref fs);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        public int fread<T>(Action<T> obj) where T : struct
        {
            try
            {
                obj(FILEExt.ReadValue<T>(ref fs));
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        #endregion


        public int fwrite<T>(T obj) where T : struct
        {
            try
            {
                FILEExt.WriteVariable(ref fs, obj);
                return 1;
            }
            catch (System.Exception ex) { return -1; }
        }

        public void fclose() { fs.Close(); }

        public void Dispose()
        {
            fs.Dispose();
        }
    }
}
//public int fread(ref int obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[4];
//        fs.Read(bytes, 0, 4);
//        obj=(BitConverter.ToInt32(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(ref Boolean obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[1];
//        fs.Read(bytes, 0, 1);
//        obj=(BitConverter.ToBoolean(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(ref Char obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[2];
//        fs.Read(bytes, 0, 2);
//        obj=(BitConverter.ToChar(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(ref double obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[8];
//        fs.Read(bytes, 0, 8);
//        obj=(BitConverter.ToDouble(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(ref Single obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[4];
//        fs.Read(bytes, 0, 4);
//        obj=(BitConverter.ToSingle(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(ref Int64 obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[8];
//        fs.Read(bytes, 0, 8);
//        obj=(BitConverter.ToInt64(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(ref UInt32 obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[4];
//        fs.Read(bytes, 0, 4);
//        obj=(BitConverter.ToUInt32(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}

//public int fread(Action<int> obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[4];
//        fs.Read(bytes, 0, 4);
//        obj(BitConverter.ToInt32(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(Action<Boolean> obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[1];
//        fs.Read(bytes, 0, 1);
//        obj(BitConverter.ToBoolean(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(Action<Char> obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[2];
//        fs.Read(bytes, 0, 2);
//        obj(BitConverter.ToChar(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(Action<double> obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[8];
//        fs.Read(bytes, 0, 8);
//        obj(BitConverter.ToDouble(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(Action<Single> obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[4];
//        fs.Read(bytes, 0, 4);
//        obj(BitConverter.ToSingle(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(Action<Int64> obj)
//{
//    try
//    {
//        Byte[] bytes = new Byte[8];
//        fs.Read(bytes, 0, 8);
//        obj(BitConverter.ToInt64(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}
//public int fread(Action<UInt32> obj)
//{
//    try
//    {
//        //BinaryReader br = new BinaryReader(fs);
//        //br.re
//        Byte[] bytes = new Byte[4];
//        fs.Read(bytes, 0, 4);
//        obj(BitConverter.ToUInt32(bytes, 0));
//        return 1;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        return -1;
//    }
//}