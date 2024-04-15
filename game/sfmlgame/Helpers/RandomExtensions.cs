
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

    }
}
