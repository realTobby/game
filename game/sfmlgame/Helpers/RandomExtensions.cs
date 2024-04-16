
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmglame.Helpers
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float minValue, float maxValue)
        {
            return (float)random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static bool NextBool(this Random random)
        {
            var randomVal = random.Next(0, 100);
            if(randomVal < 50)
            {
                return true;
            }
            return false;
        }

        public static Color GenerateRandomPastelColor()
        {
            Random random = new Random();

            // Generate high RGB values to create a pastel color
            byte red = (byte)(random.Next(127) + 128);   // 128 to 255
            byte green = (byte)(random.Next(127) + 128); // 128 to 255
            byte blue = (byte)(random.Next(127) + 128);  // 128 to 255
            byte alpha = 255;                            // Full opacity

            return new Color(red, green, blue, alpha);
        }

        public static Color HSLToRGB(float h, float s, float l)
        {
            if (s == 0f)
            {
                // achromatic color (gray scale)
                return new Color((byte)(l * 255), (byte)(l * 255), (byte)(l * 255));
            }
            else
            {
                float q = l < 0.5f ? l * (1 + s) : l + s - l * s;
                float p = 2 * l - q;
                float r = HueToRGB(p, q, h + 1f / 3f);
                float g = HueToRGB(p, q, h);
                float b = HueToRGB(p, q, h - 1f / 3f);
                return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
            }
        }

        private static float HueToRGB(float p, float q, float t)
        {
            if (t < 0f) t += 1f;
            if (t > 1f) t -= 1f;
            if (t < 1f / 6f) return p + (q - p) * 6f * t;
            if (t < 1f / 2f) return q;
            if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6f;
            return p;
        }


    }
}
