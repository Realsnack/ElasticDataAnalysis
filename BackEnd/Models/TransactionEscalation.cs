namespace BackEnd.Models
{
    public class TransactionEscalation : ElasticDTO.TransactionEscalation
    {
        public TransactionEscalation(InDone inDone, Escalation escalation)
        {
            this.Amount = inDone.Amount;
            this.Creditor = inDone.BeneficiaryAccount;
            this.Creditor.AccountNumber = inDone.OtherParameters.CreditorIBAN;
            this.Currency = inDone.Currency;
            this.Debtor = inDone.DebtorAccount;
            this.Debtor.AccountNumber = inDone.OtherParameters.DebtorIBAN;
            this.DionId = inDone.OtherParameters.DionHeaderScreeningRequestUniqueId;
            this.Direction = inDone.OtherParameters.DionHeaderDirection;
            this.MatchingRule = escalation.MatchingRule;
            this.MessageType = inDone.MessageType;
            this.Receiver = inDone.Receiver;
            // this.ScreeningResult = 
            // this.ScreeningResultDescription =
            this.Sender = inDone.Sender;
            this.SpecificSymbol = inDone.SpecificSymbol;
            this.TransactionTimestamp = inDone.OtherParameters.DionHeaderMessageTimeStamp;
            this.VariableSymbol = inDone.VariableSymbol;
        }
    }
}