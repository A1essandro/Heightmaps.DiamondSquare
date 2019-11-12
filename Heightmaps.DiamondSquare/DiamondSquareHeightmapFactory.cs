using System;
using Heightmaps.DiamondSquare.Internal;

namespace Heightmaps.DiamondSquare
{
    public class DiamondSquareHeightmapFactory
    {

        private FactoryConfiguration _settings;

        public DiamondSquareHeightmapFactory(FactoryConfiguration settings = null)
        {
            _settings = settings;
        }

        public double[][] CreateHeightMap(FactoryConfiguration settings = null)
        {
            if (settings == null && _settings == null)
                throw new ArgumentNullException();

            return new Algorithm(settings ?? _settings).Generate();
        }

    }
}