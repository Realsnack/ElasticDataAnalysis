namespace BackEnd.Models
{
    public class Escalation : ElasticDTO.Escalation
    {
        public Escalation(string someIdentifier, string severity, string matchingRule, string escalationType, string contractId, string sid, string timestamp, string escalationTime, string version, string message, string parameters, string userId, string host, string path)
        {
            SomeIdentifier = someIdentifier;
            Severity = severity;
            MatchingRule = matchingRule;
            EscalationType = escalationType;
            ContractId = contractId;
            Sid = sid;
            Timestamp = timestamp;
            EscalationTime = escalationTime;
            Version = version;
            Message = message;
            Parameters = parameters;
            UserId = userId;
            Host = host;
            Path = path;
        }
    }
}