using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class IncidentRepository : IIncidentRepository
{
    private readonly StoreContext _Context;

    public IncidentRepository(StoreContext Context)
    {
        _Context = Context;
    }

    public async Task<Incident> GetIncidentByIdAsync(int id)
    {
        return await _Context.Incidents.FindAsync(id);
    }

    public async Task<IReadOnlyList<Incident>> GetIncidentsAsync()
    {
        return await _Context.Incidents.ToListAsync();
    }

    public async Task<Incident> CreateIncidentAsync(Incident incident)
    {
        _Context.Incidents.Add(incident);
        await _Context.SaveChangesAsync();
        return incident;
    }

    public async Task<Incident> UpdateIncidentAsync(Incident incident)
    {
        _Context.Entry(incident).State = EntityState.Modified;
        await _Context.SaveChangesAsync();
        return incident;
    }

    public async Task DeleteIncidentAsync(int id)
    {
        var incident = await _Context.Incidents.FindAsync(id);
        if (incident != null)
        {
            _Context.Incidents.Remove(incident);
            await _Context.SaveChangesAsync();
        }
    }
}