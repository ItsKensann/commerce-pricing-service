using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Columbia.Cosmos.Common.Interfaces;
using commercepricing.domain.Interfaces;

namespace commercepricing.domain.Models
{
    public class Price : BaseUpdateableModel<Price>, IHasId<string>
    {
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public string EventType { get; set; }

        /// <summary>
        /// Updates the model based on the specified input
        /// </summary>
        /// <param name="model">The model to use to update this model</param>
        public override void Update(Price model)
        {
            if (model == null || Id != model.Id)
                return;
            CreatedDateTime = model.CreatedDateTime ?? CreatedDateTime;
            LastUpdatedDateTime = model.LastUpdatedDateTime ?? LastUpdatedDateTime;
            EventType = model.EventType ?? EventType;
        }
    }
}