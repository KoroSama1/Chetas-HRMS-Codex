namespace AttendanceApi.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string TokenId { get; set; }
        public string TokenHash { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }
    }
}
