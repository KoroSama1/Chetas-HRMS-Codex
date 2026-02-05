namespace AttendanceApi.DTOs.HRDto
{
    public class FieldValidationResultDto
    {
        public string Field { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
