using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
   public class srg<T> : IDisposable
    {
        public Guid id = Guid.NewGuid();
        private T[] Data;

        public srg(int size)
        {
            Array.Resize(ref Data, size);
        }

        public srg() 
        {
            Array.Resize(ref Data, 0);
        }

        public int ChangeSize(int s) { Array.Resize(ref Data, s); return Data.Length; }

        public int size() { return Data.Length; }

        public int growsize(int s) { if (s > Data.Length) { return ChangeSize(s); } else { return Data.Length; } }

        public int growsize_chunk(int s, int chunk) { if (s > Data.Length) { return ChangeSize(s + chunk); } else { return Data.Length; } }

        public T Get(int i) { return Data[i]; }

        public T[] GetData() { return Data; }

        public void Set(int i, T item) { Data[i] = item; }

        public void Set(T[] item) { Data = item; } 

        public void Dispose()
        {
            Data = null;
        }
    }
}
