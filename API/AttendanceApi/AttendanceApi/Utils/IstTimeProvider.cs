namespace AttendanceApi.Utils
{
    public class IstTimeProvider
    {
        public static readonly TimeZoneInfo IstZone = TimeZoneInfo.FindSystemTimeZoneById(
            "India Standard Time"
        );

        public static DateTime Now =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IstZone);
    }
}
