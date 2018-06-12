using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;

namespace RedisGeoDemo
{
    /// <summary>
    /// Reused and modified code - https://stackoverflow.com/questions/31955977/how-to-store-list-element-in-redis-cache
    /// </summary>
    /// <typeparam name="T">RedisList</typeparam>
    public class RedisList<T> : IList<T>
    {
        private static IDatabase _database;
        private string key;

        public RedisList(string key, IDatabase database)
        {
            this.key = key;
            _database = database;
        }

        private string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private T Deserialize<T>(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public void Insert(int index, T item)
        {
           var before = _database.ListGetByIndex(key, index);
            _database.ListInsertBefore(key, before, Serialize(item));
        }

        public void RemoveAt(int index)
        {
            var value = _database.ListGetByIndex(key, index);
            if (!value.IsNull)
            {
                _database.ListRemove(key, value);
            }
        }

        public T this[int index]
        {
            get
            {
                var value = _database.ListGetByIndex(key, index);
                return Deserialize<T>(value.ToString());
            }
            set
            {
                Insert(index, value);
            }
        }

        public void Add(T item)
        {
            _database.ListRightPush(key, Serialize(item));
        }

        public void Clear()
        {
            _database.KeyDelete(key);
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_database.ListGetByIndex(key, i).ToString().Equals(Serialize(item)))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _database.ListRange(key).CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_database.ListGetByIndex(key, i).ToString().Equals(Serialize(item)))
                {
                    return i;
                }
            }
            return -1;
        }

        public int Count
        {
            get { return (int)_database.ListLength(key); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return _database.ListRemove(key, Serialize(item)) > 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return Deserialize<T>(_database.ListGetByIndex(key, i).ToString());
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return Deserialize<T>(_database.ListGetByIndex(key, i).ToString());
            }
        }
    }
}
