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
    public class WordRepository : IWordRepository
    {
        private readonly AppDbContext _context;

        public WordRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Word word)
        {
            _context.Words.Add(word);
            return Task.CompletedTask;
        }

        public async Task<Word?> GetByIdAsync(Guid wordId)
        {
            return await _context.Words
                .Include(w => w.Deck)   // <-- include Deck
                .FirstOrDefaultAsync(w => w.Id == wordId);
        }

        public async Task<IEnumerable<Word>> GetByDeckIdAsync(Guid deckId)
        {
            return await _context.Words
            .Where(w => w.DeckId == deckId)
            .ToListAsync();
        }

        public async Task DeleteAsync(Word word)
        {
            _context.Words.Remove(word);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
