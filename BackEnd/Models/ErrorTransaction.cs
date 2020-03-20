namespace BackEnd.Models
{
    public class ErrorTransaction : ElasticDTO.ErrorTransaction
    {
        public ErrorTransaction(ScoringDone scoringDone)
        {
            JmsId = scoringDone.JmsId;
            DionId = scoringDone.DionId;
            TimeStamp = scoringDone.Timestamp;
            Error = scoringDone.Error;
        }
    }
}