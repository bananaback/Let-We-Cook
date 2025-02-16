using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LetWeCook.Data.Repositories.DietaryPreferenceRepositories
{
    public class DietaryPreferenceRepository : IDietaryPreferenceRepository
    {
        private readonly LetWeCookDbContext _context;

        public DietaryPreferenceRepository(LetWeCookDbContext context)
        {
            _context = context;
        }

        public async Task<DietaryPreference?> GetByValueAsync(string value, CancellationToken cancellationToken = default)
        {
            return await _context.DietaryPreferences
                .FirstOrDefaultAsync(dp => dp.Value == value, cancellationToken);
        }

        public async Task<DietaryPreference> CreateAsync(DietaryPreference dietaryPreference, CancellationToken cancellationToken = default)
        {
            await _context.DietaryPreferences.AddAsync(dietaryPreference, cancellationToken);
            return dietaryPreference; // Unit of Work will handle SaveChangesAsync
        }

        public async Task<DietaryPreference?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.DietaryPreferences
                .FirstOrDefaultAsync(dp => dp.Id == id, cancellationToken);
        }

        public async Task<List<DietaryPreference>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.DietaryPreferences.ToListAsync(cancellationToken);
        }

        public Task<DietaryPreference> UpdateAsync(DietaryPreference dietaryPreference, CancellationToken cancellationToken = default)
        {
            _context.DietaryPreferences.Update(dietaryPreference);
            return Task.FromResult(dietaryPreference);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _context.DietaryPreferences.Remove(new DietaryPreference { Id = id });
            return Task.CompletedTask;
        }
    }
}
