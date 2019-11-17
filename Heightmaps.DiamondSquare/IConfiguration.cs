namespace Heightmaps.DiamondSquare
{
    public interface IConfiguration
    {

        int Size { get; }

        double Persistence { get; }

        int? Seed { get; }

    }
}