using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = "User";

        public int Streak { get; set; } = 0;
        public int TotalXP { get; set; } = 0;
        public int Level { get; set; } = 1;

        public ICollection<Deck> Decks { get; set; } = new List<Deck>();
        public ICollection<ReviewLog> ReviewLogs { get; set; } = new List<ReviewLog>();
    }
}
