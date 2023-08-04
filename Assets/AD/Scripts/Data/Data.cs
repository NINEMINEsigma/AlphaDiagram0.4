using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AD.BASE;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AD.Experimental.Localization.Cache
{
    public interface ICanOrganizeData<Key, T> :
        IEnumerable,
        IEnumerable<T>
        where Key : ICanKeyCacheAsset
        where T : ICanCacheData<T>
    {

    }
    public interface ICanCacheData<T> where T : ICanCacheData<T>
    {

    }
    public interface ICanKeyCacheAsset
    {
        int pointer { get; set; }
        bool IsEqual(ICanKeyCacheAsset _Right);
    }

    public class CacheAsset<Key, T> :
        ICanOrganizeData<Key, T>
        where Key : ICanKeyCacheAsset
        where T : ICanCacheData<T>
    {
        public class Enumerator : IEnumerator<T>
        {
            public Enumerator(
                List<T> Values,
                List<Key> Keys)
            {
                this.Values = Values;
                this.Keys = Keys;
            }

            List<T> Values;
            List<Key> Keys;

            private int CurrentIndex = 0;

            public T Current => Values[Keys[CurrentIndex].pointer];

            object IEnumerator.Current => Values[Keys[CurrentIndex].pointer];

            public void Dispose()
            {
                CurrentIndex = -1;
            }

            public bool MoveNext()
            {
                return ++CurrentIndex < Keys.Count;
            }

            public void Reset()
            {
                CurrentIndex = -1;
            }
        }

        List<T> Values;
        List<Key> Keys;

        public IEnumerator<T> GetEnumerator()
        { 
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class Cache<T> : 
        ICanCacheData<T>
        where T : Cache<T>
    {

    }
}
