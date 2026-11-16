using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientManagementAPI.Data;
using PatientManagementAPI.DTOs;
using PatientManagementAPI.Exceptions;
using PatientManagementAPI.Models;


namespace PatientManagementAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PatientService> _logger;

        public PatientService(ApplicationDbContext context, ILogger<PatientService> logger)
        {
            _context = context;
            _logger = logger;
        }

       
        // GET: Patient by ID
        public async Task<Patient?> GetPatientByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.PatientID == id,
                    cancellationToken);
        }

         // GET: All Patients
      public async Task<PagedResult<Patient>> GetPatientsAsync(
        PatientQueryParameters parameters,
        CancellationToken cancellationToken)
    
        {
            var pageNumber = Math.Max(1, parameters.PageNumber);
            var pageSize = Math.Clamp(parameters.PageSize, 1, 100);

            IQueryable<Patient> query = _context.Patients.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();

                query = query.Where(p =>
                    p.Pfirstname.Contains(search) ||
                    p.Plastname.Contains(search) ||
                    p.Email.Contains(search));
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            var patients = await query
                .OrderBy(p => p.PatientID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            _logger.LogInformation(
                "Retrieved {Count} patients on page {PageNumber}",
                patients.Count, pageNumber);
 
            return new PagedResult<Patient>
            {
                Items = patients,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize

               
            };
             
        }


        // POST: Create Patient
        public async Task<Patient> CreatePatientAsync(
            CreatePatientRequest request,
            CancellationToken cancellationToken)
        {

            var normalizedEmail = request.Email.Trim();

            var emailExists = await _context.Patients
                .AnyAsync(
                    p => p.Email == normalizedEmail,
                    cancellationToken);

            if (emailExists)
            {
                throw new DuplicateEmailException(normalizedEmail);
            }
            var patient = new Patient
            {
                Pfirstname = request.Pfirstname.Trim(),
                Plastname = request.Plastname.Trim(),
                Email = normalizedEmail,
                Mobile = request.Mobile.Trim(),
                Address = request.Address.Trim(),
                Pincode = request.Pincode.Trim()
            };

            await _context.Patients.AddAsync(
                patient, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return patient;
        }



        // PUT: Update Patient
        public async Task<Patient?> UpdatePatientAsync(
            int id,
            UpdatePatientRequest request,
            CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.PatientID == id,
                    cancellationToken);

            if (patient is null)
                return null;

            patient.Pfirstname = request.Pfirstname;
            patient.Plastname = request.Plastname;
            patient.Email = request.Email;
            patient.Mobile = request.Mobile;
            patient.Address = request.Address;
            patient.Pincode = request.Pincode;

            var emailExists = await _context.Patients
    .AnyAsync(
        p => p.Email == request.Email.Trim()
             && p.PatientID != id,
        cancellationToken);

            if (emailExists)
            {
                throw new DuplicateEmailException(request.Email);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return patient;
        }

        // DELETE: Patient
        public async Task<bool> DeletePatientAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.PatientID == id,
                    cancellationToken);

            if (patient is null)
                return false;

            _context.Patients.Remove(patient);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        
        //Email validation of the patient






    }

}