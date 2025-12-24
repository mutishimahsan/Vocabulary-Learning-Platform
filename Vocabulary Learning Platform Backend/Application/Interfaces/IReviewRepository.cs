using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<ReviewLog?> GetAsync(Guid userId, Guid wordId);
        Task<IEnumerable<ReviewLog>> GetDueReviewsAsync(Guid userId, DateTime date);
        

        Task<IEnumerable<ReviewLog>> GetByUserIdAsync(Guid userId);
        Task AddAsync(ReviewLog reviewLog);
        Task SaveChangesAsync();
    }
}
