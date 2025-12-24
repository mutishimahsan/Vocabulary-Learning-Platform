using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUserRepository _userRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IWordRepository wordRepository,
            IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _wordRepository = wordRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Word>> GetTodayReviews(Guid userId)
        {
            // 1. Get existing due reviews
            var reviews = await _reviewRepository.GetDueReviewsAsync(userId, DateTime.UtcNow);
            var dueWords = reviews.Select(r => r.Word).ToList();

            // 2. Add words the user hasn’t reviewed yet
            var userDecks = await _userRepository.GetUserDecksAsync(userId); // <-- implement this
            var allWords = userDecks.SelectMany(d => d.Words);

            var newWords = allWords.Where(w => !reviews.Any(r => r.WordId == w.Id));
            dueWords.AddRange(newWords);

            return dueWords;
        }

        public async Task SubmitReview(Guid userId, Guid wordId, bool isCorrect)
        {
            var word = await _wordRepository.GetByIdAsync(wordId);
            if (word == null || word.Deck.OwnerId != userId)
                throw new UnauthorizedAccessException("Invalid word access");

            var review = await _reviewRepository.GetAsync(userId, wordId);

            if (review == null)
            {
                review = new ReviewLog
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    WordId = wordId,
                    IntervalLevel = isCorrect ? 1 : 0,
                    NextReviewDate = DateTime.UtcNow.AddDays(1),
                    SuccessRate = isCorrect ? 1 : 0
                };
                await _reviewRepository.AddAsync(review);
            }
            else
            {
                review.IntervalLevel = isCorrect ? review.IntervalLevel + 1 : 0;
                review.NextReviewDate = DateTime.UtcNow.AddDays(Math.Max(1, review.IntervalLevel));
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.TotalXP += isCorrect ? 10 : 0;
                user.Streak = isCorrect ? user.Streak + 1 : 0;
                user.Level = user.TotalXP / 100 + 1;
            }

            await _reviewRepository.SaveChangesAsync();
            await _userRepository.SaveChangesAsync();
        }

        // ================================
        //  Get user progress
        // ================================
        public async Task<object> GetUserProgress(Guid userId)
        {
            var reviews = await _reviewRepository.GetByUserIdAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            var totalReviews = reviews.Count();
            var masteredWords = reviews.Count(r => r.IntervalLevel >= 5);

            return new
            {
                user.Name,
                user.TotalXP,
                user.Level,
                user.Streak,
                TotalReviews = totalReviews,
                MasteredWords = masteredWords,
                Accuracy = totalReviews == 0
                    ? 0
                    : reviews.Average(r => r.SuccessRate)
            };
        }

        // ================================
        //  Leaderboard
        // ================================
        public async Task<IEnumerable<object>> GetLeaderboard()
        {
            var users = await _userRepository.GetTopUsersAsync(10);

            return users.Select(u => new
            {
                u.Name,
                u.TotalXP,
                u.Level,
                u.Streak
            });
        }
    }
}
