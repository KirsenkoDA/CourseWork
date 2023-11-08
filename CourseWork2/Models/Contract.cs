using Microsoft.AspNetCore.Identity;

namespace CourseWork2.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public IdentityUser Manager { get; set; }
        public IdentityUser Client { get; set; }
        public DateTime DateCreated { get; set; }
        public Service Service { get; set; }
    }
}
