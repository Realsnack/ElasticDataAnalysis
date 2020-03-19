using System;
using Nest;

namespace ElasticDTO
{
    public class ScoringDone
    {
        [PropertyName("errorMessage")]
        public ErrorMessage Error { get; set; }
        [PropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        [PropertyName("scoreCode")]
        public int ScoreCode { get; set; }
        [PropertyName("messageType")]
        public string JmsId { get; set; }
        [PropertyName("riskFactors")]
        public RiskFactor[] RiskFactors { get; set; }
        [PropertyName("scoreNum")]
        public string ScoreNum { get; set; }
        [PropertyName("id")]
        public string DionId { get; set; }
    }

    public class ErrorMessage
    {
        [PropertyName("message")]
        public string Message { get; set; }
        [PropertyName("code")]
        public string Code { get; set; }
    }

    public class RiskFactor
    {
        [PropertyName("riskFactorDesc")]
        public string RiskFactorDescription { get; set; }
        [PropertyName("riskFactor")]
        public string RiskFactorRule { get; set; }
    }
}
