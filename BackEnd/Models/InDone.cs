using System;

namespace BackEnd.Models
{
    public class InDone : ElasticDTO.InDone
    {
        public InDone(string jmsId, string transactionType, OtherParameters otherParameters, Account debtorAccount, Account beneficiaryAccount, string variableSymbol, string specificSymbol, float amount, string currency, string messageType)
        {
            JmsId = jmsId;
            TransactionType = transactionType;
            OtherParameters = otherParameters;
            DebtorAccount = debtorAccount;
            BeneficiaryAccount = beneficiaryAccount;
            VariableSymbol = variableSymbol;
            SpecificSymbol = specificSymbol;
            Amount = amount;
            Currency = currency;
            MessageType = messageType;
        }
    }

    public class OtherParameters : ElasticDTO.OtherParameters
    {
        public OtherParameters(string dionHeaderScreeningRequestUniqueId, string creditorIBAN, string debtorIBAN, string dionHeaderDirection, DateTime dionHeaderMessageTimeStamp, string jmsHeaderPriority, string dionHeaderScreeningMode)
        {
            DionHeaderScreeningRequestUniqueId = dionHeaderScreeningRequestUniqueId;
            CreditorIBAN = creditorIBAN;
            DebtorIBAN = debtorIBAN;
            DionHeaderDirection = dionHeaderDirection;
            DionHeaderMessageTimeStamp = dionHeaderMessageTimeStamp;
            JmsHeaderPriority = jmsHeaderPriority;
            DionHeaderScreeningMode = dionHeaderScreeningMode;
        }
    }

    public class Account : ElasticDTO.Account
    {
        public Account(string countryCode, string bankCode, string name, string prefix, string number)
        {
            CountryCode = countryCode;
            BankCode = bankCode;
            Name = name;
            Prefix = prefix;
            Number = number;
        }
    }
}