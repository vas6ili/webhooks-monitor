using System;

namespace WebhooksDiag
{
    public sealed class WebhookEvent
    {
        public string Type { get; set; }
        public string EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public int? TenantId { get; set; }
        public long? OrganizationUnitId { get; set; }
        public long? UserId { get; set; }
        public long? EventSourceId { get; }
    }
}
