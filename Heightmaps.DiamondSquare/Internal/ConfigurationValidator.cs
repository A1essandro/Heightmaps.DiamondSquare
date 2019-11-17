using System;

namespace Heightmaps.DiamondSquare.Internal
{
    internal class ConfigurationValidator
    {

        public void Validate(IConfiguration configuration)
        {
            if (configuration.Persistence <= 0)
            {
                throw new ArgumentException($"{nameof(IConfiguration)}.{nameof(IConfiguration.Persistence)} must be greater than 0");
            }

            if (!IsWhole(GetRawSize(configuration)))
            {
                throw new ArgumentException($"Invalid {nameof(IConfiguration)}.{nameof(IConfiguration.Size)}");
            }
        }

        private double GetRawSize(IConfiguration configuration) => Math.Log(configuration.Size - 1, 2);

        /// https://stackoverflow.com/questions/2751593/how-to-determine-if-a-decimal-double-is-an-integer
        private bool IsWhole(double number)
        {
            var res = Math.Abs(number % 1) <= (Double.Epsilon * 100);
            return res;
        }

    }
}