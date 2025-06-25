using commercepricing.infrastructure.Models;
using commercepricingchangefeed.Configurations;
using csc.azure.datalake.client;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commercepricingchangefeed.Triggers;

public class CommercePricingDatalakeNotifier
{
    private readonly ILogger<CommercePricingDatalakeNotifier> _logger;
    private readonly IDataLakeClient _dataLakeClient;
    private readonly IOptions<DataLakeConfiguration> _dataLakeConfig;

    public CommercePricingDatalakeNotifier(ILogger<CommercePricingDatalakeNotifier> logger,
        IDataLakeClient dataLakeClient, IOptions<DataLakeConfiguration> configuration)
    {
        _logger = logger;
        _dataLakeClient = dataLakeClient;
        _dataLakeConfig = configuration;
    }

    [Function("CommercePricingDatalakeNotifier")]
    public async Task RunAsync([CosmosDBTrigger(
        databaseName: "%CosmosDb:DatabaseId%",
        containerName: "%commercepricingMasterCollectionName%",
        Connection = "cosmosdb-connection",
        LeaseContainerName = "%leasesCollectionName%",
        LeaseContainerPrefix = "CommercePricingDatalakeNotifier",
        CreateLeaseContainerIfNotExists = true)]
        IReadOnlyList<RetailPricingDto> documents)
    {
        try
        {
            if (documents != null && documents.Any())
            {
                await Task.WhenAll(documents.Select(async pricingDocuments =>
                {
                    var timestamp = DateTimeOffset.UtcNow;
                    var uniqueId = pricingDocuments.UPC + pricingDocuments.Region + pricingDocuments.LocationId;

                    var json = JsonConvert.SerializeObject(pricingDocuments, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    _logger.LogInformation($"Sending Commerce Pricing snapshot for Id {uniqueId} to Data Lake.");

                    byte[] byteArray = Encoding.ASCII.GetBytes(json);
                    MemoryStream stream = new MemoryStream(byteArray);
                    
                    var path = new StringBuilder()
                        .Append(_dataLakeConfig.Value.DataLakePaths.Substring(0))
                        .Append("/").Append(timestamp.ToString("yyyyMMdd"))
                        .Append("/").Append(uniqueId)
                        .Append('_').Append(timestamp.ToString("yyyyMMddhhmmssFFF"))
                        .Append(".json");

                    await _dataLakeClient.CreateFileAsync(path.ToString(), stream);
                    _logger.LogInformation($"Succesfully sent out Commerce Pricing document for Id {uniqueId} to Data Lake.");
                }));
            }            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}