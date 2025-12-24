using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Deck
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public Guid OwnerId { get; set; }

        [JsonIgnore]
        public User Owner { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Word> Words { get; set; } = new List<Word>();
    }
}
