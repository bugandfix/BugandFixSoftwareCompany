using BugandFixSoftwareCompany.Abstractions;
using BugandFixSoftwareCompany.Data;
using BugandFixSoftwareCompany.Entity;
using Microsoft.EntityFrameworkCore;

namespace BugandFixSoftwareCompany.Implementations;

public class SoftwareDeveloperRepository : ISoftwareDeveloperRepository
{
    private readonly ApplicationDbContext _context;

    public SoftwareDeveloperRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SoftwareDeveloper>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.SoftwareDevelopers.ToListAsync(cancellationToken);
    }

    public async Task<SoftwareDeveloper?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.SoftwareDevelopers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<SoftwareDeveloper> AddAsync(SoftwareDeveloper developer, CancellationToken cancellationToken)
    {
        await _context.SoftwareDevelopers.AddAsync(developer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return developer;
    }

    public async Task<SoftwareDeveloper?> UpdateAsync(SoftwareDeveloper developer, CancellationToken cancellationToken)
    {
        var existing = await _context.SoftwareDevelopers.FindAsync(new object[] { developer.Id }, cancellationToken);
        if (existing == null) return null;

        existing.Name = developer.Name;
        existing.Specialization = developer.Specialization;
        existing.Experience = developer.Experience;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var developer = await _context.SoftwareDevelopers.FindAsync(new object[] { id }, cancellationToken);
        if (developer == null) return false;

        _context.SoftwareDevelopers.Remove(developer);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}