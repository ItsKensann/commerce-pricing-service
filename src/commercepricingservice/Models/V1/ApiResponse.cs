using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text;

namespace commercepricingservice.Models.V1
{   
    /// <summary>
    /// Api Response
    /// </summary>
    [DataContract]
    public partial class ApiResponse : IEquatable<ApiResponse>
    {
        /// <summary>
        /// Service name
        /// </summary>
        [DataMember(Name = "serviceName")]
        public string? ServiceName { get; set; }

        /// <summary>
        /// Transaction ID
        /// </summary>
        [DataMember(Name = "transactionId")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// Operation name
        /// </summary>
        [DataMember(Name = "operationName")]
        public string? OperationName { get; set; }

        /// <summary>
        /// Status code
        /// </summary>
        [DataMember(Name = "statusCode")]
        public int? StatusCode { get; set; }

        /// <summary>
        /// Status description
        /// </summary>
        [DataMember(Name = "statusDescription")]
        public string? StatusDescription { get; set; }

        /// <summary>
        /// List of errors
        /// </summary>
        [DataMember(Name = "errors")]
        public List<string>? Errors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiResponse {\n");
            sb.Append("  ServiceName: ").Append(ServiceName).Append("\n");
            sb.Append("  TransactionId: ").Append(TransactionId).Append("\n");
            sb.Append("  OperationName: ").Append(OperationName).Append("\n");
            sb.Append("  StatusCode: ").Append(StatusCode).Append("\n");
            sb.Append("  StatusDescription: ").Append(StatusDescription).Append("\n");
            sb.Append("  Errors: ").Append(Errors).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ApiResponse)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Equals(ApiResponse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    ServiceName == other.ServiceName ||
                    ServiceName != null &&
                    ServiceName.Equals(other.ServiceName)
                ) &&
                (
                    TransactionId == other.TransactionId ||
                    TransactionId != null &&
                    TransactionId.Equals(other.TransactionId)
                ) &&
                (
                    OperationName == other.OperationName ||
                    OperationName != null &&
                    OperationName.Equals(other.OperationName)
                ) &&
                (
                    StatusCode == other.StatusCode ||
                    StatusCode != null &&
                    StatusCode.Equals(other.StatusCode)
                ) &&
                (
                    StatusDescription == other.StatusDescription ||
                    StatusDescription != null &&
                    StatusDescription.Equals(other.StatusDescription)
                ) &&
                (
                    Errors == other.Errors ||
                    Errors != null &&
                    Errors.SequenceEqual(other.Errors!)
                );
        }

        /// <summary>
        /// hash code getter
        /// </summary>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (ServiceName != null)
                    hashCode = hashCode * 59 + ServiceName.GetHashCode();
                if (TransactionId != null)
                    hashCode = hashCode * 59 + TransactionId.GetHashCode();
                if (OperationName != null)
                    hashCode = hashCode * 59 + OperationName.GetHashCode();
                if (StatusCode != null)
                    hashCode = hashCode * 59 + StatusCode.GetHashCode();
                if (StatusDescription != null)
                    hashCode = hashCode * 59 + StatusDescription.GetHashCode();
                if (Errors != null)
                    hashCode = hashCode * 59 + Errors.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(ApiResponse left, ApiResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApiResponse left, ApiResponse right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}
