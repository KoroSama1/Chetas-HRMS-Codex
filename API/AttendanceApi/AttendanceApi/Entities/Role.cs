using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.Entities
{
    public class Role
    {
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string RoleName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
