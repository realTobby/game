
using SFML.Graphics;
using SFML.System;
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

        public static float RGBToHue(byte r, byte g, byte b)
        {
            float red = r / 255.0f;
            float green = g / 255.0f;
            float blue = b / 255.0f;

            float max = Math.Max(Math.Max(red, green), blue);
            float min = Math.Min(Math.Min(red, green), blue);
            float delta = max - min;

            float hue = 0f;
            if (delta == 0)
            {
                hue = 0;
            }
            else if (max == red)
            {
                hue = (green - blue) / delta + (green < blue ? 6 : 0);
            }
            else if (max == green)
            {
                hue = (blue - red) / delta + 2;
            }
            else if (max == blue)
            {
                hue = (red - green) / delta + 4;
            }

            return hue / 6; // Normalize hue to be between 0 and 1
        }

        public static Vector2f Lerp(this Vector2f from, Vector2f to, float t)
        {
            t = Math.Clamp(t, 0.0f, 1.0f); // Ensure t is within the range [0, 1]
            return new Vector2f(
                from.X + (to.X - from.X) * t,
                from.Y + (to.Y - from.Y) * t
            );
        }

    }
}
