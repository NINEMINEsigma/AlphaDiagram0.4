using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Test.Object.Line
{
    public interface ILine
    {
        public abstract List<Vector3> Line();
    }

    public abstract class Cline: ILine
    {
        [Tooltip("Line Attribute")]
        public float Start { get; set; } = 0;
        public float End { get; set; } = 1;
        public int SamplingFrequency { get; set; } = 100;

        public abstract List<Vector3> Line();
    }

    public abstract class ICurve: Cline
    {
        protected float Calculation(Vector4 from, float t)
        {
            return Calculation(from.x, from.y, from.z, from.w, t);
        }

        protected float Calculation(float Amplitude, float Frequency, float FirstPhase, float Offset, float t)
        {
            return Amplitude * MathF.Sin(Frequency * t * MathF.PI * 2.0f + FirstPhase * MathF.PI * 2.0f) + Offset;
        }
    }

    [Serializable]
    public class StraightLine : Cline
    {
        [Tooltip("StraightLine Attribute")]
        public Vector3 Head = new(), Tail = new();

        public override List<Vector3> Line()
        {
            List<Vector3> cat = new();
            cat.Add(Vector3.Lerp(Head, Tail, Start));
            cat.Add(Vector3.Lerp(Head, Tail, End));
            return cat;
        }
    }

    [Serializable]
    public class EllipticalLine : ICurve
    {
        [Tooltip("EllipticalLine Attribute")]
        public Vector4 X = new(), Y = new(), Z = new();

        public override List<Vector3> Line()
        {
            List<Vector3> cat = new();
            for (int i = 0; i < SamplingFrequency; i++)
            {
                float t = Start + ((float)i / (float)SamplingFrequency) * (End - Start);
                cat.Add(Calculation(t));
            }
            cat.Add(Calculation(Start));
            return cat;
        }

        Vector3 Calculation(float t)
        {
            return new Vector3(Calculation(X, t), Calculation(Y, t), Calculation(Z, t));
        }
    }

    [Serializable]
    public class CircularLine : ICurve
    {
        [Tooltip("CircularLine Attribute")]
        public Vector3 Normal = new(0, 1, 0);
        public float Radius = 1;

        public override List<Vector3> Line()
        {
            List<Vector3> cat = new();
            for (int i = 0; i < SamplingFrequency; i++)
            {
                float t = Start + ((float)i / (float)SamplingFrequency) * (End - Start);
                cat.Add(Calculation(t));
            }
            cat.Add(Calculation(Start));
            return cat;
        }

        Vector3 Calculation(float t)
        {
            throw new();
        }

    }

    [RequireComponent(typeof(LineRenderer)), ExecuteAlways]
    public class LineBehaviour : MonoBehaviour
    {
        LineRenderer line;

        private void Start()
        {
            line = GetComponent<LineRenderer>();
        }

        public bool IsStraight = true;
        public StraightLine STR;
        public EllipticalLine Ell;

        private void Update()
        {
            var cat = (IsStraight) ? (STR.Line().ToArray()) : (Ell.Line().ToArray());
            line.SetPositions(cat);
            line.positionCount = cat.Length;
        }

    }



}
