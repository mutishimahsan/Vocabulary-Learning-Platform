using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReviewLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid WordId { get; set; }
        public Word Word { get; set; } = null!;

        public DateTime LastReviewed { get; set; } = DateTime.UtcNow;
        public DateTime NextReviewDate { get; set; } = DateTime.UtcNow;
        public double SuccessRate { get; set; } = 0;
        public int IntervalLevel { get; set; } = 1;
    }
}
