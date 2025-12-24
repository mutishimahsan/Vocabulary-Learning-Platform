using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class WordDto
    {
        public string Term { get; set; } = null!;
        public string Definition { get; set; } = "";
        public string ExampleSentence { get; set; } = "";
        public string PartOfSpeech { get; set; } = "";
        public string PronunciationUrl { get; set; } = "";
    }
}
