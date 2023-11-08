using EmploymentAgency.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CourseWork2.Models
{
    public class Account
    {
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public string Info { get; set; }
        public ICollection<Resume> Resumes { get; set; }
        public ICollection<EmployerRequest> EmployerRequests { get; set; }

    }
}
