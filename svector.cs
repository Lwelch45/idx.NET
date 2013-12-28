using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointerFactory;
namespace idx
{

    public class svector<T> : IList<T>, IDisposable
    {


        protected internal List<T> L = new List<T>();
        public svector()
        {
            L = new List<T>();

        }

        public svector(int n)
        {
            L = new List<T>(n);
        }

        public void Add(T item)
        {
            L.Add(item);
        }

        public virtual void Clear()
        {
            L.Clear();
        }

        public bool Contains(T item)
        {
            return L.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            L.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return L.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return L.Remove(item);
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return L.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return L.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            L.Insert(index, item);
        }

        public T this[int index]
        {
            get { return L[index]; }
            set { L[index] = value; }
        }

        public void RemoveAt(int index)
        {
            L.RemoveAt(index);
        }

        public System.Collections.IEnumerator GetEnumerator1()
        {
            return GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        #region "IDisposable Support"
        // To detect redundant calls
        private bool disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    L = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
            }
            this.disposedValue = true;
        }

        // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        //Protected Overrides Sub Finalize()
        //    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        //    Dispose(False)
        //    MyBase.Finalize()
        //End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
