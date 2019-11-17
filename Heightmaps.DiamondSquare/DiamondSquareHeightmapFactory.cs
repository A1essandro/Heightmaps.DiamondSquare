using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Heightmaps.DiamondSquare.Internal;

[assembly: InternalsVisibleToAttribute("Tests")]

namespace Heightmaps.DiamondSquare
{
    public class DiamondSquareHeightmapFactory
    {

        private IConfiguration _configuration;

        public DiamondSquareHeightmapFactory(IConfiguration configuration = null)
        {
            _configuration = configuration;
        }

        public Task<double[][]> CreateHeightMap(IConfiguration configuration = null)
        {
            if (configuration == null && _configuration == null)
                throw new ArgumentNullException();

            configuration = configuration ?? _configuration;
            var algorithm = _getAlgorithm(configuration);

            return algorithm.Generate();
        }

        private IAlgorithm _getAlgorithm(IConfiguration configuration)
        {
            if (configuration.Seed.HasValue)
                return new DefaultAlgorithm(configuration);

            return new AsyncForDefaultSeedAlgorithm(configuration);
        }

    }
}