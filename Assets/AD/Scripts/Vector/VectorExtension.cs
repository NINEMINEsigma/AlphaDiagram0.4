using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD.Utility
{
    public static class VectorExtension
    {
        public static Vector3 ToVector3 (this Vector2 self)
        {
            return self;
        }

        public static Vector2 ToVector2(this Vector3 self)
        {
            return self;
        }
    }
}
