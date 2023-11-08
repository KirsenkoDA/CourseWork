using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using CourseWork2.Models;

namespace EmploymentAgency.Models
{
    public class Resume
    {
        public int Id { get; set; }
        [Column("user_id")]
        public IdentityUser User { get; set; }
        [Column("date_created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Должность")]
        [Column("post")]
        public string Post { get; set; }
        [Display(Name = "Информация")]
        [Column("info")]
        public string Info { get; set; }
        [Display(Name = "Образование")]
        [Column("education_id")]
        public int EducationId { get; set; }
        public Education Education { get; set; }
        [Display(Name = "Зарплата")]
        [Column("salary")]
        public float Salary { get; set; }
        [Column("status_id")]
        public Status Status { get; set; }
    }
}
