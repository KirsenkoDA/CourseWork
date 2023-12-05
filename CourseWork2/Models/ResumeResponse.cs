using EmploymentAgency.Models;

namespace CourseWork2.Models
{
    public class ResumeResponse
    {
        public List<Resume> resumes { get; set; } = new List<Resume> { };

        public int pages { get; set; }
        public int currentPage { get; set; }
    }
}
