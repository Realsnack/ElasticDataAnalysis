using System;

namespace ElasticDTO
{
    public class TransactionEscalation
    {
        public DateTime TransactionTimestamp { get; set; }
        public string DionId { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public Account Creditor { get; set; }
        public string Direction { get; set; }
        public Account Debtor { get; set; }
        public string JmsId { get; set; }
        public string MatchingRule { get; set; }
        public string MessageType { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
        public int ScreeningResult { get; set; }
        public string ScreeningResultDescription { get; set; }
        public string SpecificSymbol { get; set; }
        public string VariableSymbol { get; set; }
    }
}