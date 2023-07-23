using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AD
{

    public interface IGraph
    {

    }

    public class GraphSet<T> : HashSet<T>
    {

    }

    internal static class GraphAsset<T> where T : class, new()
    {
        public struct Slot
        {
            public HashSet<Graph<T>.Node> V;
            public HashSet<Graph<T>.Edge> E;
        }

        public static Dictionary<int,Slot> graphs = new Dictionary<int, Slot>();


    }


    [Serializable]
    public class GraphException : Exception
    {
        public GraphException() { }
        public GraphException(string message) : base(message) { }
        public GraphException(string message, Exception inner) : base(message, inner) { }
        protected GraphException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    public class Graph<T> : IGraph where T : class, new()
    {
        #region

        public class Edge
        {
            public object _Left, _Right;
            public float _Weight;
            public float Weight
            {
                get { return _Weight; }
                set { _Weight = value; }
            }
            public float Capacity
            {
                get { return _Weight; }
                set { _Weight = value; }
            }
        }

        public class Edge<T1> : Edge where T1 : class, new()
        {
            public T1 Left
            {
                get { return _Left as T1; }
                set { _Left = value; }
            }
            public Type LeftType => typeof(T1);

        }

        public class Edge<T1, T2> : Edge<T1> where T1 : class, new() where T2 : class, new()
        {
            public Edge() { }
            public Edge(T1 left, T2 right)
            {
                Left = left;
                Right = right;
            }

            public T2 Right
            {
                get { return _Right as T2; }
                set { _Right = value; }
            }
            public Type RightType => typeof(T2);

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }
            public bool Equals(Graph<T>.Edge<T1, T2> edge)
            {
                return this == edge;
            }
            public bool Equals(Graph<T>.Edge<T2, T1> edge)
            {
                return this == edge;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return "<" + Left.ToString() + "," + Right.ToString() + ">";
            }

            public static bool operator ==(Graph<T>.Edge<T1, T2> _Left, Graph<T>.Edge<T1, T2> _Right)
            {
                return (_Left.Left == _Right.Left) && (_Left.Right == _Right.Left);
            }
            public static bool operator !=(Graph<T>.Edge<T1, T2> _Left, Graph<T>.Edge<T1, T2> _Right)
            {
                return (_Left.Left != _Right.Left) || (_Left.Right != _Right.Left);
            }
            public static bool operator ==(Graph<T>.Edge<T1, T2> _Left, Graph<T>.Edge<T2, T1> _Right)
            {
                return (_Left.Left == _Right.Right) && (_Left.Left == _Right.Left);
            }
            public static bool operator !=(Graph<T>.Edge<T1, T2> _Left, Graph<T>.Edge<T2, T1> _Right)
            {
                return (_Left.Left != _Right.Right) || (_Left.Left != _Right.Left);
            }

            public Graph<T>.Edge<T1, T2> Init(Graph<T>.Edge<T1, T2> _Right)
            {
                this.Left = _Right.Left;
                this.Right = _Right.Right;
                return this;
            }
            public Graph<T>.Edge<T1, T2> Init(Graph<T>.Edge<T2, T1> _Right)
            {
                this.Left = _Right.Right;
                this.Right = _Right.Left;
                return this;
            }
        }

        public abstract class Node
        {
            public object _That;
            public float Value; 
        }

        public class Node<_T> : Node where _T : class, new()
        {
            public _T that => _That as _T;
            public Type type => typeof(_T);
        }

        public Graph()
        {
            slot = new GraphAsset<T>.Slot();
            GraphAsset<T>.graphs.Add(GetHashCode(),slot); 
        }
        public Graph(int index)
        {
            if (GraphAsset<T>.graphs.ContainsKey(index)) slot = GraphAsset<T>.graphs[index];
            else
            {
                slot = new GraphAsset<T>.Slot();
                GraphAsset<T>.graphs.Add(GetHashCode(), slot);
            }
        }

        private GraphAsset<T>.Slot slot = default;

        #endregion

        #region Add
        /*
        public void Add(T item)
        {
            if (!slot.V.Add(new Node<T>() { _That = item })) throw new GraphException("Graph<" + typeof(T).FullName + "> add item(MainType) failed");
        }
        public void Add(Node<T> item)
        {
            if (!slot.V.Add(item)) throw new GraphException("Graph<" + typeof(T).FullName + "> add item(MainNode) failed");
        }
        public void Add<_P>(_P item) where _P : class, new()
        {
            if (!slot.V.Add(new Node<_P>() { _That = item })) throw new GraphException("Graph<" + typeof(_P).FullName + "> add item(OtherType) failed");
        }
        public void Add<_P>(Node<_P> item) where _P : class, new()
        {
            if (!slot.V.Add(item)) throw new GraphException("Graph<" + typeof(T).FullName + "> add item(OtherNode) failed");
        }
        public void Add(Node item)
        {
            if (!slot.V.Add(item)) throw new GraphException("Graph<" + typeof(T).FullName + "> add item(Node) failed");
        }

        public bool TryAdd(T item)
        {
            return slot.V.Add(new Node<T>() { _That = item });
        }
        public bool TryAdd(Node item)
        {
            return slot.V.Add(item);
        }
        public bool TryAdd(Node<T> item)
        {
            return slot.V.Add(item);
        }

        #endregion

        #region ExceptWith

        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }
        void ISet<Graph<T>.Node<T>>.ExceptWith(IEnumerable<Graph<T>.Node<T>> other)
        {
            throw new NotImplementedException();
        }
        void ISet<Graph<T>.Node>.ExceptWith(IEnumerable<Graph<T>.Node> other)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith<_P>() where _P : class, new()
        {
            List<Node> excepts = new List<Node>();
            foreach (var vertex in slot.V) if (vertex._That.GetType().Equals(typeof(_P))) excepts.Add(vertex);
        }

        #endregion


        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        bool ISet<T>.IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        bool ISet<T>.IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        bool ISet<T>.Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        bool ISet<T>.SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        */
        #endregion
    }
}
