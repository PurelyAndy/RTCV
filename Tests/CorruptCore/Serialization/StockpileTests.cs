namespace Tests.CorruptCore.Serialization
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using RTCV.CorruptCore;
    using FluentAssertions;

    [TestClass]
    public class StockpileTests
    {
        private const string TestResourceFolder = "../Tests/CorruptCore/Serialization/Resources";
        private static readonly string defaultStockpilePath = Path.Combine(TestResourceFolder, "DefaultStockpile.json");
        private static readonly string defaultStockpile505Path = Path.Combine(TestResourceFolder, "DefaultStockpile505.json");

        /// <summary>
        /// Validate the output of serialization for a default Stockpile object.
        /// </summary>
        [TestMethod]
        public void TestDefaultStockpileSerialization()
        {
            var stockpile = new Stockpile();
            var tempFilePath = Path.GetTempFileName();
            using (var fileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate))
            {
                JsonHelper.Serialize(stockpile, fileStream, Formatting.Indented);
            }

            using (var actualStreamReader = new StreamReader(tempFilePath))
            {
                var actualValue = actualStreamReader.ReadToEnd();

                using (var streamReader = new StreamReader(defaultStockpilePath))
                {
                    var expectedValue = streamReader.ReadToEnd();
                    actualValue.Should().Be(expectedValue);
                }
            }

            File.Delete(tempFilePath);
        }

        [TestMethod]
        public void TestDefaultStockpileDeserialization()
        {
            var expectedStockpile = new Stockpile();
            using (var actualStreamReader = new StreamReader(defaultStockpilePath))
            {
                var actualStockpileSerialized = actualStreamReader.ReadToEnd();
                var actualStockpile = JsonConvert.DeserializeObject<Stockpile>(actualStockpileSerialized);
                actualStockpile.Should().BeEquivalentTo(expectedStockpile);
            }
        }

        /// <summary>
        /// Make sure that the current version of RTCV can read saved stockpiles from version 5.0.5
        /// </summary>
        [TestMethod]
        public void TestDefaultStockpile505Deserialization()
        {
            var expectedStockpile = new Stockpile();
            using (var actualStreamReader = new StreamReader(defaultStockpile505Path))
            {
                var actualStockpileSerialized = actualStreamReader.ReadToEnd();
                var actualStockpile = JsonConvert.DeserializeObject<Stockpile>(actualStockpileSerialized);
                actualStockpile.Should().BeEquivalentTo(expectedStockpile);
            }
        }
    }
}