using System;

namespace AD
{
    public interface IGraph
    {

    }

    public class Graph<T1, T2> : IGraph where T1 : class, new() where T2 : class, new()
    {
        public class Edge
        {
            public Edge() { }
            public Edge(T1 left, T2 right)
            {
                Left = left;
                Right = right;
            }

            public T1 Left = null;
            public readonly Type LeftType = typeof(T1);
            public T2 Right = null;
            public readonly Type RightType = typeof(T2);

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }
            public bool Equals(Graph<T1, T2>.Edge edge)
            {
                return this == edge;
            }
            public bool Equals(Graph<T2, T1>.Edge edge)
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

            public static bool operator ==(Graph<T1, T2>.Edge _Left, Graph<T1, T2>.Edge _Right)
            {
                return (_Left.Left == _Right.Left) && (_Left.Right == _Right.Left);
            }
            public static bool operator !=(Graph<T1, T2>.Edge _Left, Graph<T1, T2>.Edge _Right)
            {
                return (_Left.Left != _Right.Left) || (_Left.Right != _Right.Left);
            }
            public static bool operator ==(Graph<T1, T2>.Edge _Left, Graph<T2, T1>.Edge _Right)
            {
                return (_Left.Left == _Right.Right) && (_Left.Left == _Right.Left);
            }
            public static bool operator !=(Graph<T1, T2>.Edge _Left, Graph<T2, T1>.Edge _Right)
            {
                return (_Left.Left != _Right.Right) || (_Left.Left != _Right.Left);
            }

            public Graph<T1, T2>.Edge Init(Graph<T1, T2>.Edge _Right)
            {
                this.Left = _Right.Left;
                this.Right = _Right.Right;
                return this;
            }
            public Graph<T1, T2>.Edge Init(Graph<T2, T1>.Edge _Right)
            {
                this.Left = _Right.Right;
                this.Right = _Right.Left;
                return this;
            }
        }
    }
}
