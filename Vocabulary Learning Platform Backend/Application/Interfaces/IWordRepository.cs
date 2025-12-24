using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWordRepository
    {
        Task AddAsync(Word word);
        Task<Word?> GetByIdAsync(Guid wordId);
        Task<IEnumerable<Word>> GetByDeckIdAsync(Guid deckId);
        Task DeleteAsync(Word word);
        Task SaveChangesAsync();
    }
}
