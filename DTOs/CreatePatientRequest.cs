using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PatientManagementAPI.DTOs
{
    public class CreatePatientRequest
    {
    [Required]
    [MaxLength(100)]
    public string Pfirstname { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Plastname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Mobile { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Pincode { get; set; } = string.Empty;
        
    }
}