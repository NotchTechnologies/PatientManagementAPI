
using Microsoft.AspNetCore.Mvc;
using PatientManagementAPI.Services;
using PatientManagementAPI.Models;

namespace PatientManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        // Constructor Dependency Injection
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patient
        [HttpGet]
        public async Task<IActionResult> GetPatients(
            CancellationToken cancellationToken)
        {
            var patients = await _patientService.GetPatientsAsync(
                cancellationToken);

            return Ok(patients);
        }

        // GET: api/patient/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(
            int id,
            CancellationToken cancellationToken)
        {
            var patient = await _patientService.GetPatientByIdAsync(
                id,
                cancellationToken);

            if (patient == null)
            {
                return NotFound(new
                {
                    Message = $"Patient with ID {id} not found."
                });
            }

            return Ok(patient);
        }
    }
}
