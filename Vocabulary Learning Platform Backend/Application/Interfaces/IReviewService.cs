using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Word>> GetTodayReviews(Guid userId);
        Task SubmitReview(Guid userId, Guid wordId, bool isCorrect);
        Task<object> GetUserProgress(Guid userId);
        Task<IEnumerable<object>> GetLeaderboard();
    }
}
