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

        public int fread<T>(ref T obj)
        {
            Byte[] bytes = new Byte[4];
            switch (Type.GetTypeCode(obj.GetType()))
            {

                case TypeCode.Int32:
                    try
                    {
                        bytes = new Byte[4];
                        fs.Read(bytes, 0, 4);
                        obj = (T)(object)BitConverter.ToInt32(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Boolean:
                    try
                    {
                        bytes = new Byte[1];
                        fs.Read(bytes, 0, 1);
                        obj = (T)(object)BitConverter.ToBoolean(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Char:
                    try
                    {
                        bytes = new Byte[2];
                        fs.Read(bytes, 0, 2);
                        obj = (T)(object)BitConverter.ToChar(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Double:
                    try
                    {
                        bytes = new Byte[8];
                        fs.Read(bytes, 0, 8);
                        obj = (T)(object)BitConverter.ToDouble(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Single: //float
                    try
                    {
                        bytes = new Byte[4];
                        fs.Read(bytes, 0, 4);
                        obj = (T)(object)BitConverter.ToSingle(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Int64:
                    try
                    {
                        bytes = new Byte[8];
                        fs.Read(bytes, 0, 8);
                        obj = (T)(object)BitConverter.ToInt64(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.UInt32:
                    try
                    {
                        bytes = new Byte[4];
                        fs.Read(bytes, 0, 4);
                        obj = (T)(object)BitConverter.ToUInt32(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                default:
                    Console.WriteLine("Reading of type not supported!");
                    return -1;
            }
        }

        public int fwrite<T>(T obj)
        {
            Byte[] bytes;
            try
            {
                switch (Type.GetTypeCode(obj.GetType()))
                {
                    case TypeCode.Int32:
                        bytes = BitConverter.GetBytes((int)(object)obj);
                        break;
                    case TypeCode.Boolean:
                        bytes = BitConverter.GetBytes((Boolean)(object)obj);
                        break;
                    case TypeCode.Char:
                        bytes = BitConverter.GetBytes((Char)(object)obj);
                        break;
                    case TypeCode.Double:
                        bytes = BitConverter.GetBytes((Double)(object)obj);
                        break;
                    case TypeCode.Single: //float
                        bytes = BitConverter.GetBytes((Single)(object)obj);
                        break;
                    case TypeCode.Int64:
                        bytes = BitConverter.GetBytes((Int64)(object)obj);
                        break;
                    case TypeCode.UInt32:
                        bytes = BitConverter.GetBytes((UInt32)(object)obj);
                        break;
                    default:
                        Console.WriteLine("Writing of type not supported!");
                        return -1;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            try
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;	
            }
           
            return 1;
        }

        public int fread<T>(ref T obj, int size)
        {
            Byte[] bytes = new Byte[size];
            fs.Read(bytes, 0, size);
            switch (Type.GetTypeCode(obj.GetType()))
            {

                case TypeCode.Int32:
                    try
                    {
                        obj = (T)(object)BitConverter.ToInt32(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Boolean:
                    try
                    {
                        obj = (T)(object)BitConverter.ToBoolean(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Char:
                    try
                    {
                        obj = (T)(object)BitConverter.ToChar(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Double:
                    try
                    {
                        obj = (T)(object)BitConverter.ToDouble(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Single: //float
                    try
                    {
                        obj = (T)(object)BitConverter.ToSingle(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.Int64:
                    try
                    {
                        obj = (T)(object)BitConverter.ToInt64(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                case TypeCode.UInt32:
                    try
                    {
                        obj = (T)(object)BitConverter.ToUInt32(bytes, 0);
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return -1;
                    }

                default:
                    Console.WriteLine("Reading of type not supported!");
                    return -1;
            }
        }

        public void fclose() { fs.Close(); }

        public void Dispose()
        {
            fs.Dispose();
        }
    }
}
