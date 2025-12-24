using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DeckRepository : IDeckRepository
    {
        private readonly AppDbContext _context;

        public DeckRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Deck deck)
        {
            _context.Decks.Add(deck);
        }

        public async Task<Deck?> GetByIdAsync(Guid deckId)
        {
            return await _context.Decks
            .FirstOrDefaultAsync(d => d.Id == deckId);
        }

        public async Task<IEnumerable<Deck>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Decks
                .Where(d => d.OwnerId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(Deck deck)
        {
            _context.Decks.Remove(deck);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
