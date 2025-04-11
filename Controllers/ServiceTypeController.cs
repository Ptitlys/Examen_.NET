using Examen.Models.Entities;
using Examen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examen.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ServiceTypeController : ControllerBase
{
    private readonly IServiceTypeService _serviceTypeService;

    public ServiceTypeController(IServiceTypeService serviceTypeService)
    {
        _serviceTypeService = serviceTypeService;
    }
    
    [HttpGet(Name = "GetServiceTypes")]
    public async Task<IActionResult> Get()
    {
        var serviceTypes = await _serviceTypeService.GetAll();
        return Ok(serviceTypes);
    }

    [HttpGet("{name}", Name = "GetServiceTypeByName")]
    public async Task<IActionResult> GetByName(string name)
    {
        var serviceType = await _serviceTypeService.GetByName(name);
        return Ok(serviceType);
    }

    [HttpPost(Name = "PostServiceType")]
    public async Task<IActionResult> Post(ServiceType serviceType)
    {
        var newServiceType = await _serviceTypeService.Add(serviceType);

        return CreatedAtAction(
            nameof(GetByName), 
            new { name = newServiceType.Name }, 
            newServiceType);
    }
    

    [HttpDelete ("{id}", Name ="DeleteServiceType")]
    public async Task<IActionResult> Delete(int id)
    {
        await _serviceTypeService.Delete(id);

        return Ok();
    }
}