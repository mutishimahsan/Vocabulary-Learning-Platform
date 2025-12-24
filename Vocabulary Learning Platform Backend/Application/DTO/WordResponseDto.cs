using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class WordResponseDto
    {
        public Guid Id { get; set; }
        public Guid DeckId { get; set; }
        public string Term { get; set; } = null!;
        public string Definition { get; set; } = "";
        public string ExampleSentence { get; set; } = "";
        public string PartsOfSpeech { get; set; } = "";
        public string PronunciationUrl { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
