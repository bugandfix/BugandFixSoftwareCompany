using BugandFixSoftwareCompany.Entity;

namespace BugandFixSoftwareCompany.Abstractions;

public interface ISoftwareDeveloperRepository
{
    Task<IEnumerable<SoftwareDeveloper>> GetAllAsync(CancellationToken cancellationToken);
    Task<SoftwareDeveloper?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<SoftwareDeveloper> AddAsync(SoftwareDeveloper developer, CancellationToken cancellationToken);
    Task<SoftwareDeveloper?> UpdateAsync(SoftwareDeveloper developer, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}

