using System.Security.Claims;
using Examen.Datas;
using Examen.Models;
using Examen.Models.Entities;

namespace Examen.Services;

public class InterventionService : IInterventionService
{
    private readonly IInterventionDataAccess _interventionDataAccess;
    private readonly ICustomerDataAccess _customerDataAccess;
    private readonly ITechnicianDataAccess _technicianDataAccess;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceTypeDataAccess _serviceTypeDataAccess;


    public InterventionService(
        IInterventionDataAccess interventionDataAccess,
        ICustomerDataAccess customerDataAccess,
        ITechnicianDataAccess technicianDataAccess,
        IHttpContextAccessor httpContextAccessor,
        IServiceTypeDataAccess serviceTypeDataAccess)
    {
        _interventionDataAccess = interventionDataAccess;
        _customerDataAccess = customerDataAccess;
        _technicianDataAccess = technicianDataAccess;
        _httpContextAccessor = httpContextAccessor;
        _serviceTypeDataAccess = serviceTypeDataAccess;
    }
    
    public async Task<IEnumerable<Intervention>> GetAll()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user == null)
            throw new UnauthorizedAccessException("User not authenticated.");

        if (user.IsInRole("Technician"))
        {
            var technicianId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _interventionDataAccess.GetByTechnicianId(technicianId);
            
        }
        
        return await _interventionDataAccess.GetAll();
    }

    public async Task<Intervention?> GetById(int id)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user == null)
            throw new UnauthorizedAccessException("User not authenticated.");

        if (user.IsInRole("Technician"))
        {
            var technicianId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var interventions = await _interventionDataAccess.GetByTechnicianId(technicianId);
            return interventions.FirstOrDefault(i => i.Id == id);            
        }

        var intervention = await _interventionDataAccess.GetById(id);
        if (intervention == null) 
            throw new KeyNotFoundException($"Intervention with id {id} not found.");
        
        return intervention;
    }

    public async Task<Intervention> Add(Intervention intervention)
    {
        var customer = await _customerDataAccess.GetById(intervention.Customer.Id);
        
        if (customer == null)
            throw new KeyNotFoundException($"Customer with id {intervention.Customer.Id} not found.");
        
        var technicianIdsToCheck = intervention.Technicians.Select(technician => technician.Id).ToList();
        var existingTechnicianIds = await _technicianDataAccess.CheckIfTechniciansExist(technicianIdsToCheck);

        if (!existingTechnicianIds)
            throw new KeyNotFoundException($"Technicians with IDs {string.Join(", ", technicianIdsToCheck)} not found.");        var serviceType = await _serviceTypeDataAccess.GetByName(intervention.ServiceType.Name);
            
        if (serviceType == null)
            throw new ArgumentException("Service type is required.");
        
        if(intervention.ScheduledFor.HasValue && intervention.ScheduledFor.Value < DateTime.UtcNow)
            throw new ArgumentException($"Scheduled date {intervention.ScheduledFor} cannot be in the past.");
        
        intervention.Customer = customer;
        intervention.ServiceType = serviceType;
        
        var technicianIds = intervention.Technicians.Select(t => t.Id).ToList();
        intervention.Technicians = new List<Technician>();
        
        var result = await _interventionDataAccess.Add(intervention, technicianIds);
        return result;
    }

    public async Task Delete(int id)
    {
        await _interventionDataAccess.Delete(id);
    }
}