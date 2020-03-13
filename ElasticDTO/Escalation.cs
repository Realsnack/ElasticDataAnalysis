using Nest;

namespace ElasticDTO
{
    public class Escalation
    {
        [PropertyName("some_identifier")]
        public string SomeIdentifier { get; set; }

        [PropertyName("severity")]
        public string Severity { get; set; }

        [PropertyName("matching_rule")]
        public string MatchingRule { get; set; }

        [PropertyName("escalation_type")]
        public string EscalationType { get; set; }

        [PropertyName("contractId")]
        public string ContractId { get; set; }

        [PropertyName("sid")]
        public string Sid { get; set; }

        [PropertyName("@timestamp")]
        public string Timestamp { get; set; }

        [PropertyName("escalation_time")]
        public string EscalationTime { get; set; }

        [PropertyName("@version")]
        public string Version { get; set; }

        [PropertyName("message")]
        public string Message { get; set; }

        [PropertyName("parameters")]
        public string Parameters { get; set; }

        [PropertyName("userId")]
        public string UserId { get; set; }

        [PropertyName("host")]
        public string Host { get; set; }

        [PropertyName("path")]
        public string Path { get; set; }
    }
}
