using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heightmaps.DiamondSquare.Internal
{
    public class AsyncForDefaultSeedAlgorithm : IAlgorithm
    {

        private readonly IConfiguration _config;
        private readonly Random _rand;
        private readonly ConfigurationValidator _configurationValidator = new ConfigurationValidator();
        private double[][] _heightmapContext { get; set; }

        public AsyncForDefaultSeedAlgorithm(IConfiguration config)
        {
            _configurationValidator.Validate(config);
            _config = config;
            _rand = new Random();
        }

        public async Task<double[][]> Generate()
        {
            _heightmapContext = new double[_config.Size][];
            for (int i = 0; i < _config.Size; i++)
            {
                _heightmapContext[i] = new double[_config.Size];
            }

            var last = _config.Size - 1;
            _heightmapContext[0][0] = _getOffset(_config.Size);
            _heightmapContext[0][last] = _getOffset(_config.Size);
            _heightmapContext[last][0] = _getOffset(_config.Size);
            _heightmapContext[last][last] = _getOffset(_config.Size);

            await _divide(_config.Size);

            return _heightmapContext;
        }

        private async Task _divide(int stepSize)
        {
            int half;
            while ((half = stepSize / 2) >= 1)
            {
                var taskList = new List<Task>();
                for (var x = half; x < _config.Size; x += stepSize)
                {
                    for (var y = half; y < _config.Size; y += stepSize)
                    {
                        taskList.Add(_square(x, y, half, _getOffset(stepSize)));
                    }
                }

                await Task.WhenAll(taskList);

                stepSize = half;
            }
        }

        private Task _square(int x, int y, int size, double offset)
        {
            var a = _getCellHeight(x - size, y - size, size);
            var b = _getCellHeight(x + size, y + size, size);
            var c = _getCellHeight(x - size, y + size, size);
            var d = _getCellHeight(x + size, y - size, size);
            var average = (a + b + c + d) / 4;
            _heightmapContext[x][y] = average + offset;

            return Task.WhenAll(
                _diamond(x, y - size, size),
                _diamond(x - size, y, size),
                _diamond(x, y + size, size),
                _diamond(x + size, y, size)
            );
        }

        private Task _diamond(int x, int y, int size)
        {
            return Task.Run(() =>
            {
                var offset = _getOffset(size);
                var a = _getCellHeight(x, y - size, size);
                var b = _getCellHeight(x, y + size, size);
                var c = _getCellHeight(x - size, y, size);
                var d = _getCellHeight(x + size, y, size);
                var average = (a + b + c + d) / 4;
                _heightmapContext[x][y] = average + offset;
            });
        }

        /// <summary>
        /// Get random offset. Value depends on current step of divide.
        /// </summary>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        private double _getOffset(int stepSize)
        {
            var offset = stepSize / _config.Size * _rand.Next(-_config.Size, _config.Size);
            var sign = offset < 0 ? -1 : 1;
            return sign * Math.Pow(Math.Abs(offset), 1 / Math.Sqrt(_config.Persistence));
        }

        /// <summary>
        /// Getting height of cell by indexes. Random if cell out of map.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        private double _getCellHeight(int x, int y, int stepSize = 0)
        {
            if (x < 0 || y < 0 || x >= _config.Size || y >= _config.Size)
                return _getOffset(stepSize);
            return _heightmapContext[x][y];
        }
    }
}