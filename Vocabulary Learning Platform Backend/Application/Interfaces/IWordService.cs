using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWordService
    {
        Task<WordResponseDto> AddWordAsync(Guid userId, Guid deckId, WordDto dto);
        Task<IEnumerable<WordResponseDto>> GetWordsByDeckAsync(Guid userId, Guid deckId);
        Task<WordResponseDto?> GetWordByIdAsync(Guid userId, Guid wordId);
        Task<WordResponseDto> UpdateWordAsync(Guid userId, Guid wordId, WordDto dto);
        Task DeleteWordAsync(Guid userId, Guid wordId);
    }
}
