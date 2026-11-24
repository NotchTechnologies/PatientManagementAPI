
using Microsoft.EntityFrameworkCore;
using PatientManagementAPI.Data;
using PatientManagementAPI.Models;

namespace PatientManagementAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all patients
        public async Task<IEnumerable<Patient>> GetPatientsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        // Get patient by ID
        public async Task<Patient?> GetPatientByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.PatientId == id,
                    cancellationToken);
        }
    }
}
