using System;
using System.Threading.Tasks;
using Heightmaps.DiamondSquare.Internal;

namespace Heightmaps.DiamondSquare
{
    public class DiamondSquareHeightmapFactory
    {

        private FactoryConfiguration _configuration;

        public DiamondSquareHeightmapFactory(FactoryConfiguration configuration = null)
        {
            _configuration = configuration;
        }

        public Task<double[][]> CreateHeightMap(FactoryConfiguration configuration = null)
        {
            if (configuration == null && _configuration == null)
                throw new ArgumentNullException();

            configuration = configuration ?? _configuration;
            var algorithm = _getAlgorithm(configuration);

            return algorithm.Generate();
        }

        private IAlgorithm _getAlgorithm(FactoryConfiguration configuration)
        {
            if (configuration.Seed.HasValue)
                return new DefaultAlgorithm(configuration);

            return new AsyncForDefaultSeedAlgorithm(configuration);
        }

    }
}