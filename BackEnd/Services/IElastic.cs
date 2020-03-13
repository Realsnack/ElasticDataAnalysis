using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;

namespace BackEnd.Services
{
    public interface IElastic
    {
        Task<IEnumerable<Escalation>> GetEscalationsAsync();
        Task<IEnumerable<InDone>> GetTransactionAsync(string dionId);
    }
}