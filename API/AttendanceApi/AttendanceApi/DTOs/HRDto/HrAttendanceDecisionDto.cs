using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.DTOs.HRDto
{
    public class HrAttendanceDecisionDto
    {
        [Required]
        public int AttendanceId { get; set; }

        [Required]
        [RegularExpression(
            "Confirmed|Rejected",
            ErrorMessage = "Status must be Confirmed or Rejected"
        )]
        public string Status { get; set; }

        public string Type { get; set; }
    }
}
