using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AD.BASE;
using QFramework.PointGame;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace AD.Utility.Pipe
{
    public class PipeContext
    {
        public object Head { get; internal set; }
        internal HashSet<object> Objects = new();

        public PipeContext Link(object _Right)
        {
            Objects.Add(_Right);
            return this;
        }

        public HashSet<object> ToHashSet()
        {
            return Objects;
        }

        public HashSet<P> ToHashSet<P>() where P : class
        {
            HashSet<P> result = new();
            foreach (var item in Objects)
                if (item is P r)
                    result.Add(r);
            return result;
        }
    }

    public class PipeContext<T>
    {
        public T Head { get; internal set; }
        internal List<T> Objects = new();

        public PipeContext<T> Link(T _Right)
        {
            Objects.Add(_Right);
            return this;
        }

        public List<T> ToList()
        {
            return Objects;
        }

        public List<P> ToList<P>() where P : class
        {
            List<P> result = new();
            foreach (var item in Objects)
                if (item is P r)
                    result.Add(r);
            return result;
        }
    }

    public class PipeContext<Key, Value>
    {
        public KeyValuePair<Key, Value> Head { get; internal set; }
        internal Dictionary<Key, Value> Objects = new();

        public PipeContext<Key, Value> Link((Key, Value) _Right)
        {
            Objects[_Right.Item1] = _Right.Item2;
            return this;
        }

        public PipeContext<Key, Value> Link(KeyValuePair<Key, Value> _Right)
        {
            Objects[_Right.Key] = _Right.Value;
            return this;
        }

        public PipeContext<Key, Value> Link(Key key, Value value)
        {
            Objects[key] = value;
            return this;
        }

        public Dictionary<Key, Value> ToDictionary()
        {
            return Objects;
        }

        public Dictionary<Key, P> ToDictionary<P>() where P : class
        {
            Dictionary<Key, P> result = new();
            foreach (var item in Objects)
                if (item.Value is P r)
                    result[item.Key] = r;
            return result;
        }

        public List<Value> ToList()
        {
            List<Value> result = new();
            foreach (var item in Objects)
                result.Add(item.Value);
            return result;
        }

        public List<P> ToList<P>() where P : class
        {
            List<P> result = new();
            foreach (var item in Objects)
                if (item.Value is P r)
                    result.Add(r);
            return result;
        }

    }

    public class PipeProperty<T>
    {
        internal PipeProperty(T org)
        {
            target = org;
            pipeLineSteps = new();
        }

        public T target;

        internal List<IPipeLineStep> pipeLineSteps;
        internal bool IsBuilding = false;

        public List<IPipeLineStep> PIPELINE;

        internal object GetValue()
        {
            if (IsBuilding) throw new ADException("this property is still building");
            if (PIPELINE == null) return null;
            object current = target;
            foreach (var step in PIPELINE)
            {
                current = Convert.ChangeType(step.Execute(Convert.ChangeType(target, step._INPUT)), step._OUTPUT);
            }
            return current;
        }

        public void SharedPIPELINETo(PipeProperty<T> other)
        {
            other.PIPELINE = this.PIPELINE;
        }

        public PipeProperty<T> Begin(IPipeLineStep step)
        {
            if (this.IsBuilding) throw new ADException(
                "The pipeline didn't end the build and you tried to start building again, " +
                "or you didn't start building and you tried to end the build");
            this.IsBuilding = true;
            this.pipeLineSteps.Clear();
            this.pipeLineSteps.Add(step);
            return this;
        }

        public PipeProperty<T> Step(IPipeLineStep step)
        {
            if (!this.IsBuilding) throw new ADException(
                "The pipeline has finished building or has not started to build, and you tried to add steps to it");
            if (!this.pipeLineSteps[^1]._OUTPUT.IsAssignableFromOrSubClass(step._INPUT))
                throw new ADException("Pipeline steps must be sequential and of the same type, and do not support inversion or covariance");
            this.pipeLineSteps.Add(step);
            return this;
        }

        public PipeProperty<T> End()
        {
            if (!this.IsBuilding) throw new ADException(
                "The pipeline didn't end the build and you tried to start building again, " +
                "or you didn't start building and you tried to end the build");
            this.IsBuilding = false;
            this.PIPELINE = this.pipeLineSteps;
            this.pipeLineSteps = new();
            return this;
        }

        public bool Result<_Result>(out _Result result, bool IsMightNull = false) where _Result : class
        {
            result = null;
            if (this.IsBuilding) this.End();
            var cat = this.GetValue();
            if (cat == null) return IsMightNull;
            result = cat as _Result;
            return cat is _Result;
        }

        public bool PipeEndAndResult<_Result>(out _Result result, bool IsMightNull = false) where _Result : class
        {
            return this.End().Result(out result, IsMightNull);
        }
    }

    public interface IPipeLineStep
    {
        object Execute(object _Right);
        Type _INPUT { get; }
        Type _OUTPUT { get; }
    }
    public interface IPipeLineStep<_INPUT,_OUTPUT>
    {
        _OUTPUT Execute(_INPUT _Right);
    } 

    public static class ObjectPipe
    {
        public static PipeContext Link(this object self, object _Right)
        {
            PipeContext context = new();
            context.Head = self;
            context.Link(self);
            context.Link(_Right);
            return context;
        }

        public static PipeContext<T> Link<T>(this T self, T _Right) where T : class
        {
            PipeContext<T> context = new();
            context.Head = self;
            context.Link(self);
            context.Link(_Right);
            return context;
        }

        public static PipeContext<Key, Value> Link<Key, Value>(this (Key, Value) self, Key key, Value value)
        {
            PipeContext<Key, Value> context = new();
            context.Link(self);
            context.Head = context.Objects.First();
            context.Link(key, value);
            return context;
        }

        public static PipeContext<Key, Value> Link<Key, Value>(this (Key, Value) self, KeyValuePair<Key, Value> _Right)
        {
            PipeContext<Key, Value> context = new();
            context.Link(self);
            context.Head = context.Objects.First();
            context.Link(_Right.Key, _Right.Value);
            return context;
        }

        public static PipeContext<Key, Value> Link<Key, Value>(this KeyValuePair<Key, Value> self, Key key, Value value)
        {
            PipeContext<Key, Value> context = new();
            context.Link(self);
            context.Head = context.Objects.First();
            context.Link(key, value);
            return context;
        }

        public static PipeContext<Key, Value> Link<Key, Value>(this KeyValuePair<Key, Value> self, KeyValuePair<Key, Value> _Right)
        {
            PipeContext<Key, Value> context = new();
            context.Link(self);
            context.Head = context.Objects.First();
            context.Link(_Right.Key, _Right.Value);
            return context;
        }

        public static PipeProperty<T> Pipe<T>(this T self)
        {
            return new PipeProperty<T>(self);
        }

        public static PipeProperty<T> PipeBegin<T>(this T self, IPipeLineStep step)
        {
            return new PipeProperty<T>(self).Begin(step);
        }

        public static _OUTPUT Step<_INPUT,_OUTPUT>(this _INPUT self, IPipeLineStep<_INPUT, _OUTPUT> step)
        {
            return step.Execute(self);
        }
    }
}
