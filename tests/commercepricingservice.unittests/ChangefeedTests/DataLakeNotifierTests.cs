using AutoFixture;
using commercepricing.infrastructure.Models;
using commercepricingchangefeed.Configurations;
using commercepricingchangefeed.Triggers;
using csc.azure.datalake.client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commercepricingunittests.ChangefeedTests
{
    [TestClass]
    public class DataLakeNotifierTests
    {
        private Mock<IDataLakeClient>? _mockDataLakeClient;
        private Mock<ILogger<CommercePricingDatalakeNotifier>>? _mockLogger;
        private Mock<IOptions<DataLakeConfiguration>>? _mockConfiguration;

        [TestInitialize]
        public void Setup()
        {
            _mockDataLakeClient = new Mock<IDataLakeClient>();
            _mockLogger = new Mock<ILogger<CommercePricingDatalakeNotifier>>();
            _mockConfiguration = new Mock<IOptions<DataLakeConfiguration>>();
            _mockConfiguration.SetupGet(x => x.Value).Returns(new DataLakeConfiguration() { DataLakePaths = "fakePath" });
        }

        [TestMethod]
        public async Task RunAsync_Should_Succeed()
        {
            //Arrange
            var fixture = new Fixture();

            var input = fixture.CreateMany<RetailPricingDto>(1)
                    .ToList()
                    .AsReadOnly();

            var sut = new CommercePricingDatalakeNotifier(_mockLogger!.Object, _mockDataLakeClient!.Object, _mockConfiguration!.Object);

            //Act
            await sut.RunAsync(input);

            //Assert
            _mockDataLakeClient.Verify(x => x.CreateFileAsync(It.IsAny<string>(), It.IsAny<MemoryStream>(), It.IsAny<bool>()), Times.Once);
            _mockLogger.Verify(m => m.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task RunAsync_Should_LogErrorOnException()
        {
            //Arrange
            var fixture = new Fixture();
            var input = fixture.CreateMany<RetailPricingDto>(1)
                    .ToList()
                    .AsReadOnly();

            _mockDataLakeClient!.Setup(x => x.CreateFileAsync(It.IsAny<string>(), It.IsAny<MemoryStream>(), It.IsAny<bool>()))
                .ThrowsAsync(new ApplicationException("This is a test exception"));

            var sut = new CommercePricingDatalakeNotifier(_mockLogger!.Object, _mockDataLakeClient!.Object, _mockConfiguration!.Object);

            //Act
            var exception = await Assert.ThrowsExceptionAsync<ApplicationException>(() => sut.RunAsync(input));

            //Assert
            Assert.AreEqual("This is a test exception", exception.Message);
            _mockDataLakeClient.Verify(x => x.CreateFileAsync(It.IsAny<string>(), It.IsAny<MemoryStream>(), It.IsAny<bool>()), Times.Once);
            _mockLogger.Verify(m => m.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}
