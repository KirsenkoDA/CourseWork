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
        public string UserId { get; set; }
        [Display(Name = "Пользователь")]
        public IdentityUser User { get; set; }
        [Column("date_created")]
        [Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Должность")]
        [Column("post")]
        public string Post { get; set; }
        [Display(Name = "Информация")]
        [Column("info")]
        public string Info { get; set; }
        [Display(Name = "Образование")]
        public int EducationId { get; set; }
        [Display(Name = "Образование")]
        public Education Education { get; set; }
        [Display(Name = "Зарплата")]
        [Column("salary")]
        public float Salary { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public ICollection<Account> Responds { get; set; }
    }
}
