using System;

namespace Heightmaps.DiamondSquare
{
    public class FactoryConfiguration
    {

        public int RawSize { get; }

        public int Size { get; }

        public double Persistence { get; }

        public int? Seed { get; }

        public FactoryConfiguration(int rawSize, double persistence, int? seed)
        {
            RawSize = rawSize;
            Size = (int)Math.Pow(2, RawSize + 1);
            Persistence = persistence;
            Seed = seed;
        }

    }
}
