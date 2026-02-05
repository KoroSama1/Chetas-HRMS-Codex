using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.Entities
{
    public class Region
    {
        public int RegionId { get; set; }

        [Required, MaxLength(100)]
        public string RegionName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
