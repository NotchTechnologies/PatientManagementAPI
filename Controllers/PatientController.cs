using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PatientManagementAPI.Controllers
{
    
[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    [HttpGet]
    public IActionResult GetPatients()
    {
        var patients = new[]
        {
            new { PatientId = 1, Name = "Ramesh" },
            new { PatientId = 2, Name = "Suresh" }
        };

        return Ok(patients);
    }

    [HttpGet("{id}")]
    public IActionResult GetPatient(int id)
    {
        return Ok(new
        {
            PatientId = id,
            Name = "Ramesh"
        });
    }
}

        
    
}