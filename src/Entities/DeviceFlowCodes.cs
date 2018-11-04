using System;

namespace IdentityServer4.EntityFramework.Entities
{
    public class DeviceFlowCodes
    {
        public string DeviceCode { get; set; }
        public string UserCode { get; set; }
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Expiration { get; set; }
        public string Data { get; set; }
    }
}