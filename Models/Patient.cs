using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientManagementAPI.Models
{
    public class Patient
    {
         public int PatientId { get; set; }

    public string Pfirstname { get; set; } = string.Empty;

    public string Plastname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Mobile { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Pincode { get; set; } = string.Empty;
    }
}