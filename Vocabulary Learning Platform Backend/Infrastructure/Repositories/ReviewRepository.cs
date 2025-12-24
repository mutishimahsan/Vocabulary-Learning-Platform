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
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewLog?> GetAsync(Guid userId, Guid wordId)
        {
            return await _context.ReviewLogs
                .FirstOrDefaultAsync(r => r.UserId == userId && r.WordId == wordId);
        }

        public async Task<IEnumerable<ReviewLog>> GetDueReviewsAsync(Guid userId, DateTime date)
        {
            return await _context.ReviewLogs
                .Include(r => r.Word)
                .Where(r => r.UserId == userId && r.NextReviewDate <= date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Deck>> GetUserDecksAsync(Guid userId)
        {
            return await _context.Decks
                .Include(d => d.Words)
                .Where(d => d.OwnerId == userId)
                .ToListAsync();
        }


        public async Task<IEnumerable<ReviewLog>> GetByUserIdAsync(Guid userId)
        {
            return await _context.ReviewLogs
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(ReviewLog reviewLog)
        {
            _context.ReviewLogs.Add(reviewLog);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
