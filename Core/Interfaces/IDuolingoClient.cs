using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IDuolingoClient
    {
        Task<bool> IsAuthenticated();
        Task<AuthenticationResult> Authenticate(string username);
        Task<IEnumerable<Skill>> GetSkillsAsync();
        Task<Skill> GetSkillAsync(string name);
        Task<Word> GetWordAsync(string id);
    }
}