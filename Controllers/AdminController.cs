

using Examen.Models.Requests;
using Examen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Examen.Controllers;

    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("test")]
       public IActionResult Test()
       {
           throw new KeyNotFoundException();
        }
    }

