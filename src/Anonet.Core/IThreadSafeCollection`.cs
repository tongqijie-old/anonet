using System.Collections.Generic;

namespace Anonet.Core
{
    interface IThreadSafeCollection<T> : IEnumerable<T>
    {
        T GetOrAdd(T item);

        T[] GetAll();

        bool Exists(T item);

        void Add(T item);

        void AddRange(IEnumerable<T> items);

        void Remove(T item);

        void RemoveAll();
    }
}
