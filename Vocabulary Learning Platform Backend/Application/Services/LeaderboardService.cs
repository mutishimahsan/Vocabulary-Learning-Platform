using Application.DTO;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LeaderboardService
    {
        private readonly IUserRepository _userRepository;

        public LeaderboardService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<LeaderboardDto>> GetTopUsersAsync()
        {
            var users = await _userRepository.GetTopUsersAsync(10);

            return users.Select(u => new LeaderboardDto
            {
                Name = u.Name,
                TotalXp = u.TotalXP, // Note: Capital "P" in TotalXP
                Streak = u.Streak
            }).ToList();
        }
    }
}
