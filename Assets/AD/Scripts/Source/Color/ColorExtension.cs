
using UnityEngine;

namespace AD.Utility
{
    public static class ColorExtension
    {
        public static Color Result_R(this Color self, float value)
        {
            return new Color(value, self.g, self.b, self.a);
        }

        public static Color Result_G(this Color self, float value)
        {
            return new Color(self.r, value, self.b, self.a);
        }

        public static Color Result_B(this Color self, float value)
        {
            return new Color(self.r, self.g, value, self.a);
        }

        public static Color Result_A(this Color self, float value)
        {
            return new Color(self.r, self.g, self.b, value);
        } 
    }
}
