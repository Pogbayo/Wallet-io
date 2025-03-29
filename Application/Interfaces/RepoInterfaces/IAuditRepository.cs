using Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
    public interface IAuditLogRepository
    {
        Task AddLogAsync(AuditLog log);
    }
}
