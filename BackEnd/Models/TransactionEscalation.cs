namespace BackEnd.Models
{
    public class TransactionEscalation : ElasticDTO.TransactionEscalation
    {
        public TransactionEscalation(InDone inDone, Escalation escalation)
        {
            Amount = inDone.Amount;
            Creditor = inDone.BeneficiaryAccount;
            Creditor.AccountNumber = inDone.OtherParameters.CreditorIBAN;
            Currency = inDone.Currency;
            Debtor = inDone.DebtorAccount;
            Debtor.AccountNumber = inDone.OtherParameters.DebtorIBAN;
            DionId = inDone.OtherParameters.DionHeaderScreeningRequestUniqueId;
            Direction = inDone.OtherParameters.DionHeaderDirection;
            JmsId = inDone.JmsId;
            MatchingRule = escalation.MatchingRule;
            MessageType = inDone.MessageType;
            Receiver = inDone.Receiver;
            // this.ScreeningResult = 
            // this.ScreeningResultDescription =
            Sender = inDone.Sender;
            SpecificSymbol = inDone.SpecificSymbol;
            TransactionTimestamp = inDone.OtherParameters.DionHeaderMessageTimeStamp;
            VariableSymbol = inDone.VariableSymbol;
        }
    }
}