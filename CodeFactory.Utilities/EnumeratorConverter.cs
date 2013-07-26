using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CodeFactory.Utilities
{
    public class EnumeratorConverter<T> : IEnumerator<T>
    {
        private IEnumerator _enumerator;

        public EnumeratorConverter(IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        #region IEnumerator<T> Members

        public T Current
        {
            get { return (T)_enumerator.Current; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        #endregion
    }
}
