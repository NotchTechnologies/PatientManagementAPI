using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientManagementAPI.DTOs;
using PatientManagementAPI.Exceptions;
using PatientManagementAPI.Models;

namespace PatientManagementAPI.Services
{
    public interface IPatientService
    { Task<PagedResult<Patient>> GetPatientsAsync(
        PatientQueryParameters parameters,
        CancellationToken cancellationToken);

        Task<Patient?> GetPatientByIdAsync(
        int id,
        CancellationToken cancellationToken);
        
         Task<Patient> CreatePatientAsync(
         CreatePatientRequest request,
         CancellationToken cancellationToken);

   /*  Task<Patient?> UpdatePatientAsync(
        int id,
        UpdatePatientRequest request,
        CancellationToken cancellationToken);
 */
        Task<bool> DeletePatientAsync(
        int id,
        CancellationToken cancellationToken);


    }



}