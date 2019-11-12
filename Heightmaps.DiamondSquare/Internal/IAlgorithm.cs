using System.Threading.Tasks;

namespace Heightmaps.DiamondSquare.Internal
{
    internal interface IAlgorithm
    {

        Task<double[][]> Generate();

    }
}