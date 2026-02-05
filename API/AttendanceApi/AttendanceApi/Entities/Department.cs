using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required, MaxLength(100)]
        public string DepartmentName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
