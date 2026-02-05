using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.Entities
{
    public class Designation
    {
        public int DesignationId { get; set; }

        [Required, MaxLength(100)]
        public string DesignationName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
