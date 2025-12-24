using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDeckRepository
    {
        Task AddAsync(Deck deck);
        Task<Deck?> GetByIdAsync(Guid deckId);
        Task<IEnumerable<Deck>> GetByUserIdAsync(Guid userId);
        Task DeleteAsync(Deck deck);
        Task SaveChangesAsync();
    }
}
