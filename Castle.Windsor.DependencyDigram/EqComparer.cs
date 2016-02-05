using System;
using System.Collections.Generic;

namespace Castle.Windsor.DependencyDigram
{
    public class EqComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> equals;
        private readonly Func<T, int> hash;

        public EqComparer(Func<T, T, bool> @equals, Func<T, int> hash)
        {
            this.equals = equals;
            this.hash = hash;
        }

        public EqComparer(Func<T, T, bool> @equals)
        {
            this.equals = equals;
        }

        public bool Equals(T x, T y)
        {
            return equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            if (hash != null)
                return hash(obj);
            return obj.GetHashCode();
        }
    }
}