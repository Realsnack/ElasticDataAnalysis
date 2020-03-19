using System;

namespace BackEnd.Models
{
    public class ScoringDone : ElasticDTO.ScoringDone
    {
        public ScoringDone(ErrorMessage error, DateTime timestamp, int scoreCode, string jmsId, RiskFactor[] riskFactors, string scoreNum, string dionId)
        {
            Error = error;
            Timestamp = timestamp;
            ScoreCode = scoreCode;
            JmsId = jmsId;
            RiskFactors = riskFactors;
            ScoreNum = scoreNum;
            DionId = dionId;
        }
    }

    public class ErrorMessage : ElasticDTO.ErrorMessage
    {
        public ErrorMessage(string message, string code)
        {
            Message = message;
            Code = code;
        }

        public ErrorMessage()
        {

        }
    }

    public class RiskFactor : ElasticDTO.RiskFactor
    {
        public RiskFactor(string riskFactorDescription, string riskFactorRule)
        {
            RiskFactorDescription = riskFactorDescription;
            RiskFactorRule = riskFactorRule;
        }
    }
}