using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDeckService
    {
        Task<Deck> CreateAsync(Guid userId, DeckDto dto);
        Task<IEnumerable<Deck>> GetMyDecksAsync(Guid userId);
        Task<Deck?> GetByIdAsync(Guid deckId);
        Task DeleteAsync(Guid deckId, Guid userId);
    }
}
