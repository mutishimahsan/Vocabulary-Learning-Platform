using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SpacedRepetitionService
    {
        public (int nextInterval, DateTime nextReviewDate) Calculate(bool isCorrect, int currentInterval)
        {
            if (isCorrect)
                currentInterval++;
            else
                currentInterval = 1;

            int days = currentInterval switch
            {
                1 => 1,
                2 => 3,
                3 => 7,
                4 => 14,
                _ => 30
            };

            return (currentInterval, DateTime.UtcNow.AddDays(days));
        }
    }
}
