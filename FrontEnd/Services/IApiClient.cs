using System.Collections.Generic;
using System.Threading.Tasks;
using FrontEnd.Models;

namespace FrontEnd.Services
{
    public interface IApiClient
    {
        Task<List<TransactionEscalation>> GetTransactionsWithEscalationsAsync();
    }
}