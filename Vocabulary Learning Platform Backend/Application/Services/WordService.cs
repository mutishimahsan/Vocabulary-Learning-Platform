using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IDeckRepository _deckRepository;

        public WordService(IWordRepository wordRepository, IDeckRepository deckRepository)
        {
            _wordRepository = wordRepository;
            _deckRepository = deckRepository;
        }

        public async Task<WordResponseDto> AddWordAsync(Guid userId, Guid deckId, WordDto dto)
        {
            var deck = await _deckRepository.GetByIdAsync(deckId);

            if (deck == null || deck.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("Deck not found or access denied.");
            }

            var word = new Word
            {
                Id = Guid.NewGuid(),
                DeckId = deckId,
                Term = dto.Term,
                Definition = dto.Definition,
                ExampleSentence = dto.ExampleSentence,
                PartOfSpeech = dto.PartOfSpeech,
                PronunciationUrl = dto.PronunciationUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _wordRepository.AddAsync(word);
            await _wordRepository.SaveChangesAsync();

            return MapToResponseDto(word);
        }

        public async Task DeleteWordAsync(Guid userId, Guid wordId)
        {
            // Get the word without loading Deck navigation property
            var word = await _wordRepository.GetByIdAsync(wordId);

            if (word == null)
                throw new UnauthorizedAccessException("Word not found");

            // Get the deck separately to check ownership
            var deck = await _deckRepository.GetByIdAsync(word.DeckId);

            if (deck == null || deck.OwnerId != userId)
                throw new UnauthorizedAccessException("Access denied");

            await _wordRepository.DeleteAsync(word);
            await _wordRepository.SaveChangesAsync();
        }

        public async Task<WordResponseDto?> GetWordByIdAsync(Guid userId, Guid wordId)
        {
            // Get the word without loading Deck navigation property
            var word = await _wordRepository.GetByIdAsync(wordId);

            if (word == null)
                return null;

            // Get the deck separately to check ownership
            var deck = await _deckRepository.GetByIdAsync(word.DeckId);

            if (deck == null || deck.OwnerId != userId)
                return null;

            return MapToResponseDto(word);
        }

        public async Task<IEnumerable<WordResponseDto>> GetWordsByDeckAsync(Guid userId, Guid deckId)
        {
            var deck = await _deckRepository.GetByIdAsync(deckId);

            if (deck == null || deck.OwnerId != userId)
                throw new UnauthorizedAccessException("Deck not found or access denied");

            var words = await _wordRepository.GetByDeckIdAsync(deckId);
            return words.Select(MapToResponseDto).ToList();
        }

        public async Task<WordResponseDto> UpdateWordAsync(Guid userId, Guid wordId, WordDto dto)
        {
            // Get the word without loading Deck navigation property
            var word = await _wordRepository.GetByIdAsync(wordId);

            if (word == null)
                throw new UnauthorizedAccessException("Word not found");

            // Get the deck separately to check ownership
            var deck = await _deckRepository.GetByIdAsync(word.DeckId);

            if (deck == null || deck.OwnerId != userId)
                throw new UnauthorizedAccessException("Access denied");

            word.Term = dto.Term;
            word.Definition = dto.Definition;
            word.ExampleSentence = dto.ExampleSentence;
            word.PartOfSpeech = dto.PartOfSpeech;
            word.PronunciationUrl = dto.PronunciationUrl;
            word.UpdatedAt = DateTime.UtcNow;

            await _wordRepository.SaveChangesAsync();
            return MapToResponseDto(word);
        }

        private WordResponseDto MapToResponseDto(Word word)
        {
            return new WordResponseDto
            {
                Id = word.Id,
                DeckId = word.DeckId,
                Term = word.Term,
                Definition = word.Definition,
                ExampleSentence = word.ExampleSentence,
                PartsOfSpeech = word.PartOfSpeech,
                PronunciationUrl = word.PronunciationUrl,
                CreatedAt = word.CreatedAt,
                UpdatedAt = word.UpdatedAt
            };
        }
    }
}