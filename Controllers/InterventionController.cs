using Examen.Models.Entities;
using Examen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examen.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Technician")]
public class InterventionController : ControllerBase
{
    private readonly IInterventionService _interventionService;

    public InterventionController(IInterventionService interventionService)
    {
        _interventionService = interventionService;
    }
    
    [HttpGet(Name = "GetInterventions")]
    public async Task<IActionResult> Get()
    {
        var interventions = await _interventionService.GetAll();
        return Ok(interventions);
    }

    [HttpGet("{id}", Name = "GetInterventionById")]
    public async Task<IActionResult> GetById(int id)
    {
            var article = await _interventionService.GetById(id);
            return Ok(article);
    }

    [HttpPost(Name = "PostIntervention")]
    public async Task<IActionResult> Post(Intervention intervention)
    {
        var newIntervention = await _interventionService.Add(intervention);

        return CreatedAtAction(
            nameof(GetById), 
            new { id = newIntervention.Id }, 
            newIntervention);
    }
    

    [HttpDelete ("{id}", Name ="DeleteIntervention")]
    public async Task<IActionResult> Delete(int id)
    {
        await _interventionService.Delete(id);

        return Ok();
    }
}