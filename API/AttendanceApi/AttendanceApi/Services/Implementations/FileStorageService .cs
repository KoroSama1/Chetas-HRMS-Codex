using AttendanceApi.Services.Interfaces;

namespace AttendanceApi.Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadRoot;

        public FileStorageService(IConfiguration config)
        {
            _uploadRoot =
                config["FileStorage:UploadPath"]
                ?? throw new InvalidOperationException("UploadPath not configured");
        }

        public async Task<string> SaveAttendancePhotoAsync(
            int employeeId,
            DateTime date,
            byte[] imageBytes
        )
        {
            var folderPath = Path.Combine(
                _uploadRoot,
                "attendance",
                employeeId.ToString(),
                date.ToString("yyyyMMdd")
            );

            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}.jpg";
            var fullPath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(fullPath, imageBytes);

            //return fullPath.Replace(_uploadRoot, string.Empty);
            return fullPath.Replace(_uploadRoot, string.Empty).Replace("\\", "/");
        }

        public async Task<string> SaveEmployeeProfilePhotoAsync(int employeeId, byte[] imageBytes)
        {
            var folderPath = Path.Combine(_uploadRoot, "employees", employeeId.ToString());

            Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, "profile.jpg");

            await File.WriteAllBytesAsync(fullPath, imageBytes);

            return fullPath.Replace(_uploadRoot, string.Empty).Replace("\\", "/");
        }
    }
}
