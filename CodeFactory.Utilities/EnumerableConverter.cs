using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CodeFactory.Utilities
{
    public class EnumerableConverter<T> : IEnumerable<T>
    {
        private IEnumerable _enumerable;

        public EnumerableConverter(IEnumerable enumerable)
        {
            _enumerable = enumerable;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorConverter<T>(_enumerable.GetEnumerator());
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        #endregion
    }
}
