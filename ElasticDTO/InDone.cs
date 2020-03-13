using Nest;
using System;

namespace ElasticDTO
{
    public class InDone
    {
        [PropertyName("id")]
        public string JmsId { get; set; }

        [PropertyName("transactionType")]
        public string TransactionType { get; set; }

        [PropertyName("otherParameters")]
        public OtherParameters OtherParameters { get; set; }

        [PropertyName("debtorAccount")]
        public Account DebtorAccount { get; set; }

        [PropertyName("beneficiaryAccount")]
        public Account BeneficiaryAccount { get; set; }

        [PropertyName("vs")]
        public string VariableSymbol { get; set; }

        [PropertyName("ss")]
        public string SpecificSymbol { get; set; }

        [PropertyName("amount")]
        public float Amount { get; set; }

        [PropertyName("currency")]
        public string Currency { get; set; }

        [PropertyName("messageType")]
        public string MessageType { get; set; }
    }

    public class OtherParameters
    {
        [PropertyName("dionHeaderScreeningRequestUniqueID")]
        public string DionHeaderScreeningRequestUniqueId { get; set; }

        [PropertyName("creditorIBAN")]
        public string CreditorIBAN { get; set; }

        [PropertyName("debtorIBAN")]
        public string DebtorIBAN { get; set; }

        [PropertyName("dionHeaderDirection")]
        public string DionHeaderDirection { get; set; }

        [PropertyName("dionHeaderMessageTimeStamp")]
        public DateTime DionHeaderMessageTimeStamp { get; set; }
        
        [PropertyName("JMSHeaderPriority")]
        public string JmsHeaderPriority { get; set; }

        [PropertyName("dionHeaderScreeningMode")]
        public string DionHeaderScreeningMode { get; set; }
    }

    public class Account
    {
        [PropertyName("countryCode")]
        public string CountryCode { get; set; }
        [PropertyName("bankCode")]
        public string BankCode { get; set; }
        [PropertyName("name")]
        public string Name { get; set; }
        [PropertyName("prefix")]
        public string Prefix { get; set; }
        [PropertyName("number")]
        public string Number { get; set; }
    }
}
