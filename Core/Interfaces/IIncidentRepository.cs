using Core.Entities;

namespace Core.Interfaces
{
    public interface IIncidentRepository
    {
        Task<Incident> GetIncidentByIdAsync(int id);
        Task<IReadOnlyList<Incident>> GetIncidentsAsync();
        Task<Incident> CreateIncidentAsync(Incident incident);
        Task<Incident> UpdateIncidentAsync(Incident incident);
        Task DeleteIncidentAsync(int id);
    }
}