using BugandFixSoftwareCompany.Abstractions;
using BugandFixSoftwareCompany.Entity;

namespace BugandFixSoftwareCompany.Implementations;

public class SoftwareDeveloperService : ISoftwareDeveloperService
{
    private readonly ISoftwareDeveloperRepository _repository;

    public SoftwareDeveloperService(ISoftwareDeveloperRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<SoftwareDeveloper>> GetAllAsync(CancellationToken cancellationToken) =>
        _repository.GetAllAsync(cancellationToken);

    public Task<SoftwareDeveloper?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        _repository.GetByIdAsync(id, cancellationToken);

    public Task<SoftwareDeveloper> AddAsync(SoftwareDeveloper developer, CancellationToken cancellationToken) =>
        _repository.AddAsync(developer, cancellationToken);

    public Task<SoftwareDeveloper?> UpdateAsync(SoftwareDeveloper developer, CancellationToken cancellationToken) =>
        _repository.UpdateAsync(developer, cancellationToken);

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken) =>
        _repository.DeleteAsync(id, cancellationToken);
}