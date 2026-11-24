
using PatientManagementAPI.Models;

namespace PatientManagementAPI.Services
{
    public interface IPatientService
    {
        // Get all patients
        Task<IEnumerable<Patient>> GetPatientsAsync(
            CancellationToken cancellationToken);

        // Get patient by ID
        Task<Patient?> GetPatientByIdAsync(
            int id,
            CancellationToken cancellationToken);
    }
}
