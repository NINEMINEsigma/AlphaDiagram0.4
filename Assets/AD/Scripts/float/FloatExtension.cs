using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD.Utility
{
    public static class FloatExtension
    {
        public static float Sum(this IEnumerator<float> self)
        {
            float sum = 0;
            while (self.MoveNext())
            {
                sum += self.Current;
            }
            return sum;
        }

        public static float Sum(this float[] self)
        {
            float sum = 0;
            foreach (var value in self)
            {
                sum += value;
            }
            return sum;
        }

        public static float Average(this float[] self)
        {
            return self.Sum() / (float)(self.Length);
        }
    }
}
