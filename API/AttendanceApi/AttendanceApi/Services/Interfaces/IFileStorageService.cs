namespace AttendanceApi.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveAttendancePhotoAsync(int employeeId, DateTime date, byte[] imageBytes);

        Task<string> SaveEmployeeProfilePhotoAsync(int employeeId, byte[] imageBytes);
    }
}
