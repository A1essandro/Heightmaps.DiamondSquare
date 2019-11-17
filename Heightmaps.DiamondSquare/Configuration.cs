using System;

namespace Heightmaps.DiamondSquare
{
    public class Configuration : IConfiguration
    {

        public int RawSize { get; }

        public int Size { get; }

        public double Persistence { get; }

        public int? Seed { get; }

        public Configuration(int rawSize, double persistence, int? seed)
        {
            RawSize = rawSize;
            Size = (int)Math.Pow(2, RawSize + 1);
            Persistence = persistence;
            Seed = seed;
        }

    }
}
