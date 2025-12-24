using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class LeaderboardDto
    {
        public string Name { get; set; } = null!;
        public int TotalXp { get; set; }
        public int Streak { get; set; }
    }
}
