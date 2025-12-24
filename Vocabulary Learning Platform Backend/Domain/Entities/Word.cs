using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Word
    {
        public Guid Id { get; set; }
        public Guid DeckId { get; set; }

        [JsonIgnore]
        public Deck Deck { get; set; } = null!;

        public string Term { get; set; } = null!;
        public string Definition { get; set; } = "";
        public string ExampleSentence { get; set; } = "";
        public string PartOfSpeech { get; set; } = "";
        public string PronunciationUrl { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        [JsonIgnore]
        public ICollection<ReviewLog> ReviewLogs { get; set; } = new List<ReviewLog>();
    }
}
