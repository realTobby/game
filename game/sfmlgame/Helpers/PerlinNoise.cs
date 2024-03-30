using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmglame.Helpers
{
    using System;

    public class PerlinNoise
    {
        private int[] p;

        public PerlinNoise(int seed)
        {
            Random rng = new Random(seed);
            p = new int[512];
            var permutation = new int[256];

            // Initialize the permutation array with values 0-255
            for (int i = 0; i < 256; i++)
            {
                permutation[i] = i;
            }

            // Shuffle the array using the provided seed
            for (int i = 0; i < permutation.Length - 1; i++)
            {
                int randomIndex = rng.Next(i, permutation.Length);

                // Swap elements
                int temp = permutation[i];
                permutation[i] = permutation[randomIndex];
                permutation[randomIndex] = temp;
            }

            // Duplicate the permutation to avoid overflow
            for (int i = 0; i < 256; i++)
            {
                p[256 + i] = p[i] = permutation[i];
            }
        }

        public double Noise(double x, double y, double z)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            double u = Fade(x);
            double v = Fade(y);
            double w = Fade(z);

            int A = p[X] + Y;
            int AA = p[A] + Z;
            int AB = p[A + 1] + Z;
            int B = p[X + 1] + Y;
            int BA = p[B] + Z;
            int BB = p[B + 1] + Z;

            return Lerp(w, Lerp(v, Lerp(u, Grad(p[AA], x, y, z),
                                             Grad(p[BA], x - 1, y, z)),
                                     Lerp(u, Grad(p[AB], x, y - 1, z),
                                             Grad(p[BB], x - 1, y - 1, z))),
                             Lerp(v, Lerp(u, Grad(p[AA + 1], x, y, z - 1),
                                             Grad(p[BA + 1], x - 1, y, z - 1)),
                                     Lerp(u, Grad(p[AB + 1], x, y - 1, z - 1),
                                             Grad(p[BB + 1], x - 1, y - 1, z - 1))));
        }

        private double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        private double Grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y;
            double v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}
