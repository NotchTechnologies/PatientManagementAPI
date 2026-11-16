using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientManagementAPI.Exceptions
{
    public class DuplicateEmailException: Exception
    {
        public DuplicateEmailException(string email)
        : base($"A patient with email '{email}' already exists.")
    {
    }
    }
}