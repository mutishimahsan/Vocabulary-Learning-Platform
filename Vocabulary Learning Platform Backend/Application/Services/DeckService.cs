using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;

        public DeckService(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        public async Task<Deck> CreateAsync(Guid userId, DeckDto dto)
        {
            var deck = new Deck
            {
                Id = Guid.NewGuid(),
                OwnerId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category
            };

            await _deckRepository.AddAsync(deck);
            await _deckRepository.SaveChangesAsync();
            return deck;
        }

        public async Task<IEnumerable<Deck>> GetMyDecksAsync(Guid userId)
        {
            return await _deckRepository.GetByUserIdAsync(userId);
        }

        public async Task<Deck?> GetByIdAsync(Guid deckId)
        {
            return await _deckRepository.GetByIdAsync(deckId);
        }

        public async Task DeleteAsync(Guid deckId, Guid userId)
        {
            var deck = await _deckRepository.GetByIdAsync(deckId);

            if (deck == null || deck.OwnerId != userId)
                throw new UnauthorizedAccessException("Deck not found or access denied");

            await _deckRepository.DeleteAsync(deck);
            await _deckRepository.SaveChangesAsync();
        }
    }
}
