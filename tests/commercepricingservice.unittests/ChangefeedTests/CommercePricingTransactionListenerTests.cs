using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using Columbia.Cosmos.Common;
using Columbia.Cosmos.Common.Interfaces;
using commercepricingchangefeed;
using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricing.infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace commercepricingunittests.ChangefeedTests
{

    [TestClass]
    public class CommercePricingTransactionsListenerTests
    {
        
        private static readonly string RetailPricingId = Guid.NewGuid().ToString();
        private static readonly Guid TransactionId = Guid.NewGuid();
        private Fixture _fixture;
        
        private Mock<ILogger<CommercePricingTransactionsListener>> _mockLogger;
        private Mock<IUpserter<ICommercePricingMasterRepository, RetailPricingDto, string>> _mockUpserter;

        private CommercePricingTransactionsListener _sut;
        private List<Transaction<RetailPricingDto, string>> _testInput;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockLogger = new Mock<ILogger<CommercePricingTransactionsListener>>();
            _mockUpserter = new Mock<IUpserter<ICommercePricingMasterRepository, RetailPricingDto, string>>();

            // For testing the class
            _sut = new CommercePricingTransactionsListener(_mockUpserter.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void Teardown()
        {
            _mockLogger.Verify();
            _mockUpserter.Verify(); 
        }

        [TestMethod]
        public async Task RunAsync_Should_Succeed()
        {
            // Arrange 
            GivenTestInput();
            GivenUpserterSucceeds();

            // Act - Perform the actual work of the test
            await WhenRunAsyncCalled();

            // Assert - Verify the result
            _mockUpserter.Verify(x => x.UpsertAsync(
                It.IsAny<RetailPricingDto>(),
                It.IsAny<Func<RetailPricingDto, string>>(),
                It.IsAny<Func<RetailPricingDto, CosmosResource<RetailPricingDto>, RetailPricingDto>>(),
                It.IsAny<Func<RetailPricingDto, RetailPricingDto>>(),
                It.IsAny<Func<RetailPricingDto, (bool, string)>>(),
                It.IsAny<Func<RetailPricingDto, (bool, string)>>()
                ), Times.Once);
        }

        [TestMethod]
        public async Task RunAsync_New_Transaction_Update_Master()
        {
            // Arrange
            var originalPrices = new PriceDto[]
            {
                new PriceDto { Price = 10.00m },
                new PriceDto { Price = 12.50m }
            };
            var newPrices = new PriceDto[]
            {
                new PriceDto { Price = 15.00m },
                new PriceDto { Price = 18.75m }
            };

            GivenTestInput(newPrices);
            GivenUpserterSucceeds();

            // Act
            await WhenRunAsyncCalled();

            // Assert
            ThenMergeFunctionShouldUpdatePrices(originalPrices, newPrices);

        }

        private void GivenTestInput(PriceDto[] prices = null, string givenEventType = null)
        {
            var transaction = new Transaction<RetailPricingDto, string>()
            {
                Id = TransactionId,
                Payload = _fixture.Build<RetailPricingDto>()
                    .With(o => o.Id, RetailPricingId)
                    .With(o => o.Prices, prices)
                    .Create(),
                EventType = givenEventType
            };

            _testInput = new List<Transaction<RetailPricingDto, string>>() { transaction };
        }

        private void GivenUpserterSucceeds() => _mockUpserter.Setup(upsert => upsert.UpsertAsync(
            It.IsAny<RetailPricingDto>(),
                It.IsAny<Func<RetailPricingDto, string>>(),
                It.IsAny<Func<RetailPricingDto, CosmosResource<RetailPricingDto>, RetailPricingDto>>(),
                It.IsAny<Func<RetailPricingDto, RetailPricingDto>>(),
                It.IsAny<Func<RetailPricingDto, (bool, string)>>(),
                It.IsAny<Func<RetailPricingDto, (bool, string)>>()
                ))
                .Returns(Task.CompletedTask)
                .Verifiable();
            
        private async Task WhenRunAsyncCalled()
        {
            await _sut.RunAsync(_testInput);
        }

        private void ThenMergeFunctionShouldUpdatePrices(PriceDto[] originalPrices, PriceDto[] expectedNewPrices)
        {
            _mockUpserter.Verify(x => x.UpsertAsync(
                It.IsAny<RetailPricingDto>(),
                It.IsAny<Func<RetailPricingDto, string>>(),
                It.Is<Func<RetailPricingDto, CosmosResource<RetailPricingDto>, RetailPricingDto>>(mergeFunc =>
                    MergeFunctionUpdatesPrices(mergeFunc, originalPrices, expectedNewPrices)),
                It.IsAny<Func<RetailPricingDto, RetailPricingDto>>(),
                It.IsAny<Func<RetailPricingDto, (bool, string)>>(),
                It.IsAny<Func<RetailPricingDto, (bool, string)>>()
                ), Times.Once);
        }

        private bool MergeFunctionUpdatesPrices(
            Func<RetailPricingDto, CosmosResource<RetailPricingDto>, RetailPricingDto> mergeFunc,
            PriceDto[] originalPrices,
            PriceDto[] expectedNewPrices)
        {
            // Create mock existing master record with original prices
            var existingMasterRecord = _fixture.Build<RetailPricingDto>()
                .With(x => x.Id, RetailPricingId)
                .With(x => x.Prices, originalPrices)
                .Create();

            var cosmosResource = new CosmosResource<RetailPricingDto>(existingMasterRecord, "test-etag");

            // Create new transaction data with updated prices
            var newTransactionData = _fixture.Build<RetailPricingDto>()
                .With(x => x.Id, RetailPricingId)
                .With(x => x.Prices, expectedNewPrices)
                .Create();

            // Execute the merge function
            var result = mergeFunc(newTransactionData, cosmosResource);

            // Verify the prices were updated correctly
            return result.Prices != null &&
               result.Prices.Length == expectedNewPrices.Length &&
               PricesAreEqual(result.Prices, expectedNewPrices);
        }

        private bool PricesAreEqual(PriceDto[] actual, PriceDto[] expected)
        {
            if (actual.Length != expected.Length) return false;

            for (int i = 0; i < actual.Length; i++)
            {
                if (actual[i].Price != expected[i].Price)
                    return false;
            }

            return true;
        }
    }
}
