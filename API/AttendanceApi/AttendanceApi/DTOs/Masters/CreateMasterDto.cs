using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.DTOs.Masters
{
    public class CreateMasterDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
