using System;

namespace Heightmaps.DiamondSquare.Internal
{
    internal sealed class Algorithm
    {

        private FactoryConfiguration _config;

        private Random _rand;

        public Algorithm(FactoryConfiguration config)
        {
            _config = config;
            _rand = config.Seed.HasValue ? new Random(config.Seed.Value) : new Random();
        }

        private double[][] Terra { get; set; }


        public double[][] Generate()
        {
            Terra = new double[_config.Size][];
            for (int i = 0; i < _config.Size; i++)
            {
                Terra[i] = new double[_config.Size];
            }

            var last = _config.Size - 1;
            Terra[0][0] = _getOffset(_config.Size);
            Terra[0][last] = _getOffset(_config.Size);
            Terra[last][0] = _getOffset(_config.Size);
            Terra[last][last] = _getOffset(_config.Size);

            _divide(_config.Size);

            return Terra;
        }


        private void _divide(int stepSize)
        {
            int half;
            while ((half = stepSize / 2) >= 1)
            {
                for (var x = half; x < _config.Size; x += stepSize)
                {
                    for (var y = half; y < _config.Size; y += stepSize)
                    {
                        _square(x, y, half, _getOffset(stepSize));
                    }
                }

                stepSize = half;
            }
        }

        private void _square(int x, int y, int size, double offset)
        {
            var a = _getCellHeight(x - size, y - size, size);
            var b = _getCellHeight(x + size, y + size, size);
            var c = _getCellHeight(x - size, y + size, size);
            var d = _getCellHeight(x + size, y - size, size);
            var average = (a + b + c + d) / 4;
            Terra[x][y] = average + offset;
            _diamond(x, y - size, size);
            _diamond(x - size, y, size);
            _diamond(x, y + size, size);
            _diamond(x + size, y, size);
        }

        private void _diamond(int x, int y, int size)
        {
            var offset = _getOffset(size);
            var a = _getCellHeight(x, y - size, size);
            var b = _getCellHeight(x, y + size, size);
            var c = _getCellHeight(x - size, y, size);
            var d = _getCellHeight(x + size, y, size);
            var average = (a + b + c + d) / 4;
            Terra[x][y] = average + offset;
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
            return Terra[x][y];
        }

    }
}