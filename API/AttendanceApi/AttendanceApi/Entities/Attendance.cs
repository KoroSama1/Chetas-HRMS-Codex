using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.Entities
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        // -------- DATE --------
        public DateOnly Date { get; set; }

        // -------- CHECK-IN --------
        public TimeOnly CheckInTime { get; set; }
        public string CheckInPhotoPath { get; set; }
        public string CheckInLocationAddress { get; set; }
        public double CheckInLatitude { get; set; }
        public double CheckInLongitude { get; set; }

        [Required, MaxLength(50)]
        public string CheckInStatus { get; set; } // Pending, Approved, Rejected

        // -------- CHECK-OUT --------
        public TimeOnly? CheckOutTime { get; set; }
        public string? CheckOutPhotoPath { get; set; }
        public string? CheckOutLocationAddress { get; set; }
        public double? CheckOutLatitude { get; set; }
        public double? CheckOutLongitude { get; set; }

        [MaxLength(50)]
        public string? CheckOutStatus { get; set; } // Pending, Approved, Rejected

        // -------- HR / VERIFICATION --------
        public int? VerifiedByHRId { get; set; }
        public DateTime? VerifiedAt { get; set; }

        [MaxLength(300)]
        public string? RejectionReason { get; set; }
    }
}
