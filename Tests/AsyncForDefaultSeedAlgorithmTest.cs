using Xunit;
using Moq;
using Heightmaps.DiamondSquare;
using Heightmaps.DiamondSquare.Internal;
using System.Threading.Tasks;

namespace Tests
{
    public class AsyncForDefaultSeedAlgorithmTest
    {

        [Theory]
        [InlineData(1025, null)]
        [InlineData(9, 0)]
        [InlineData(3, null)]
        public async Task SizeTest(int size, int? seed)
        {
            var config = new Mock<IConfiguration>();
            config.SetupGet(x => x.Size).Returns(size);
            config.SetupGet(x => x.Seed).Returns(seed);
            config.SetupGet(x => x.Persistence).Returns(1);

            var algorithm = new AsyncForDefaultSeedAlgorithm(config.Object);

            var result = await algorithm.Generate();

            Assert.Equal(size, result.Length);
            Assert.Equal(size, result[0].Length);
        }

    }
}
