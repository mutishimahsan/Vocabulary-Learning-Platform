using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GamificationService
    {
        public void Apply(User user, bool isCorrect)
        {
            user.TotalXP += isCorrect ? 10 : 2;

            user.Level = user.TotalXP / 100;

            user.Streak = isCorrect
                ? user.Streak + 1
                : 0;
        }
    }
}
